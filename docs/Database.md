# Database Documentation

## Overview
The application uses SQL Server, Dapper, and Stored Procedures.
Business logic is implemented primarily within the ASP.NET Core service layer, while stored procedures are responsible for data access, validation, and maintaining database integrity.
The database is intentionally designed so that application validation improves the user experience, while database constraints remain the final source of truth.

---

## Database Tables

### Users
Stores registered user accounts.

#### Responsibilities
*   Store user information.
*   Enforce unique email addresses.
*   Support authentication.
*   Act as the parent table for user-related data.

#### Notes
*   Passwords are never stored in plain text. Only BCrypt password hashes are stored.

### UserAvatars
Stores metadata describing each user's profile image.
*   Only metadata is stored in the database.
*   Stored information includes:
    *   Stored file name
    *   File extension
    *   MIME type
    *   Image width
    *   Image height
    *   File size
    *   Upload timestamp

#### Notes
*   The physical storage path is intentionally not stored.
*   The application constructs physical paths using configuration (`ImageStorageSettings`), allowing storage locations to change without database modifications.
*   Each user may own only one avatar through a UNIQUE constraint on `UserId`.

### ImageUploadAttempts
Tracks temporary image uploads before registration is completed.
*   Image upload occurs before a user exists, making a temporary tracking table necessary.

#### Responsibilities
*   Track uploaded temporary images.
*   Associate uploads with an `UploadToken`.
*   Prevent reuse of uploaded images.
*   Support expiration and cleanup.

#### Upload Status Lifecycle
`Pending` -> `Completed` | `Expired` | `Failed`

*   Only uploads that are **Pending** and **Not expired** are considered valid.
*   Expired uploads are intended to be removed by a background cleanup service.

### RefreshTokens
Stores refresh tokens used for JWT authentication.

#### Responsibilities
*   Persist refresh tokens.
*   Support token rotation.
*   Allow token revocation.
*   Support user logout.
*   Each refresh token belongs to exactly one user.

---

## Registration Architecture
Registration is intentionally divided into two independent stages.

### Stage 1 - Temporary Image Upload
The user uploads an image before submitting the registration form.
The application:
1.  Validates extension.
2.  Validates file size.
3.  Validates image signature (magic bytes).
4.  Extracts metadata.
5.  Stores the image in temporary storage.
6.  Creates an `ImageUploadAttempts` record.
7.  Generates an `UploadToken`.

*The `UploadToken` is returned to the client and acts as the temporary connection between the uploaded image and the future registration request.*

### Stage 2 - User Registration
The user submits the registration form.
The application:
1.  Validates the `UploadToken`.
2.  Retrieves the pending upload attempt.
3.  Creates the user.
4.  Moves the image into permanent storage.
5.  Creates avatar metadata.
6.  Marks the upload as `Completed`.

*   If registration fails before database persistence, no user is created.
*   If database persistence fails after the image has been moved, compensating cleanup removes the moved file.

---

## Stored Procedure Conventions
Every stored procedure follows the same structure:
*   `CREATE OR ALTER PROCEDURE`
*   `SET NOCOUNT ON`
*   `TRY/CATCH`

### Write Procedures
Write procedures return a single response object:
*   `ResponseCode`
*   `ResponseMessage`
*   *(Some procedures also return additional information such as `UserId`)*

### Read Procedures
Read procedures use `QueryMultipleAsync`.
*   The first result set always returns: `ResponseCode`, `ResponseMessage`.
*   The second result set contains the requested data.
*   An empty second result set is not considered an error. *(Example: GetExpiredImageUploads returns 200 even when no expired uploads exist).*

---

## Response Codes

| Code | Meaning |
| :--- | :--- |
| 200 | Success |
| 404 | Resource not found |
| 409 | Business conflict |
| 410 | Upload token expired or already used |
| 500 | Unexpected database error |

---

## Database Integrity
Business rules are enforced at two layers:
1.  **Application validation:** Exists to provide useful feedback to users.
2.  **Database constraints:** Guarantee correctness (UNIQUE email, UNIQUE avatar per user, Foreign keys, CHECK constraints, NOT NULL constraints).

*The database always remains the final authority.*

---

## Future Database Work
The current schema is designed to support:
*   Roles
*   Permissions
*   Products
*   Categories
*   Inventory
*   Shopping Cart
*   Orders
*   Payments
*   Reviews
*   Background cleanup service