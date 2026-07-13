# Database Design

## Overview

The application uses SQL Server with Dapper and Stored Procedures. Business logic is primarily implemented in the ASP.NET Core service layer, while stored procedures are responsible for data access, validation, and maintaining database integrity.

---

# Tables

## Users

Stores user account information.

### Responsibilities

* Store registered users.
* Enforce unique email addresses.
* Support future authentication features such as refresh tokens and roles.

---

## UserAvatars

Stores metadata for a user's profile image.

Only image metadata is stored in the database.

Stored information includes:

* Original file name
* Stored file name
* File extension
* MIME type
* Image width
* Image height
* File size
* Upload timestamp

The physical file path is intentionally **not** stored. The application constructs file paths using configuration so storage locations can change without requiring database changes.

Each user may own only one avatar through a UNIQUE constraint on `UserId`.

---

## ImageUploadAttempts

Tracks temporary image uploads before registration is completed.

This table exists because image upload occurs before a user account has been created.

### Lifecycle

Pending

↓

Completed

or

Expired

or

Failed

Only uploads with a status of **Pending** and an expiration time in the future are considered valid.

Expired uploads are removed later by a background cleanup service.

---

# Registration Flow

Registration is intentionally divided into two stages.

## Stage 1

The user uploads a profile image.

The application:

* validates the file
* extracts image information
* stores the temporary file
* creates an ImageUploadAttempts record
* generates an UploadToken

The UploadToken is returned to the client.

---

## Stage 2

The user submits the registration form.

The application:

* validates the UploadToken
* verifies that the upload is still Pending
* verifies that the upload has not expired
* moves the image to permanent storage
* creates the user
* creates the avatar metadata
* marks the upload as Completed

If any step before database persistence fails, no user record is created.

If the database transaction fails after the file has been moved, the application performs compensating cleanup by deleting the moved file.

---

# Stored Procedure Conventions

Every stored procedure follows the same structure.

* CREATE OR ALTER PROCEDURE
* SET NOCOUNT ON
* TRY/CATCH error handling

Write procedures return a first result set containing:

* ResponseCode
* ResponseMessage

Some write procedures may also return additional data such as a newly created `UserId`.

Read procedures use QueryMultipleAsync.

The first result set always contains:

* ResponseCode
* ResponseMessage

The second result set contains the requested data.

---

# Response Codes

`200`

Operation completed successfully.

`404`

Requested resource was not found.

`409`

Business conflict, such as duplicate email or duplicate avatar.

`410`

The requested upload token exists but is no longer valid because it has already been used or has expired.

`500`

Unexpected database error.

---

# Database Integrity

Database constraints are the source of truth.

Examples include:

* UNIQUE email addresses
* UNIQUE avatar per user
* Foreign key constraints
* CHECK constraints for upload status

Application-level validation improves the user experience, while database constraints guarantee correctness.

---

### Read Stored Procedure Convention

Read stored procedures use `QueryMultipleAsync`.

The first result set always returns:

* `ResponseCode`
* `ResponseMessage`

The second result set returns the requested data.

An empty second result set is **not** considered an error. It indicates that no matching records were found while the stored procedure executed successfully.

`GetExpiredImageUploads` follows this convention. It always returns a `200` response when the query executes successfully, even if the second result set contains zero rows. An empty collection simply means there are no expired uploads for the background cleanup service to process.

Database errors continue to return `500` with an appropriate `ResponseMessage`.


# Future Work

The current schema is designed to support future features including:

* Roles
* Refresh Tokens
* Products
* Categories
* Shopping Cart
* Orders
* Payments
* Reviews
* Background cleanup service
