# Authentication Documentation

## Overview
Authentication uses:
*   JWT Access Tokens
*   Refresh Tokens
*   HTTP-only Cookies
*   BCrypt Password Hashing

The MVC application authenticates users through cookies while internally using JWT authentication.

*Access tokens are stored inside HTTP-only cookies rather than browser storage. This protects tokens from JavaScript access while still allowing the application to authenticate using the JWT Bearer authentication handler.*

---

## Authentication Flow

### Login
1.  Login
2.  Validate email
3.  Verify BCrypt password
4.  Generate Access Token
5.  Generate Refresh Token
6.  Save Refresh Token
7.  Write Authentication Cookies
8.  Redirect

### Logout
1.  Logout
2.  Read Refresh Token
3.  Revoke Refresh Token
4.  Delete Cookies
5.  Redirect

---

## Automatic Token Refresh
Every incoming request passes through `RefreshTokenMiddleware`. This middleware runs before authentication.

1.  **Access Token Expired?**
    *   **No:** Continue Request.
    *   **Yes:** Proceed to Refresh Token check.
2.  **Refresh Token Exists?**
    *   **No:** Continue Request.
    *   **Yes:** `RefreshAsync()`
        *   Rotate Refresh Token.
        *   Generate New Access Token.
        *   Update Cookies.
        *   Continue Request.

*If the middleware refreshes an expired access token, the newly generated token is written to both the response cookie and HttpContext.Items.*

*The response cookie is not available until the browser sends the next request, so the JWT authentication handler first checks HttpContext.Items. This allows the current request to authenticate using the refreshed token without requiring another round trip.*

---

## Cookie Strategy
Two cookies are used, and the client never interacts with either:

| Cookie | Type | Features |
| :--- | :--- | :--- |
| **Access Token** | HTTP-only | Secure, SameSite=Strict, Short lifetime |
| **Refresh Token** | HTTP-only | Secure, SameSite=Strict, Long lifetime |

---

## Refresh Token Rotation
Every refresh operation:
1.  Validates the refresh token.
2.  Revokes the previous refresh token before issuing a replacement.
3.  Generates a new refresh token.
4.  Generates a new access token.
5.  Stores the new refresh token.
6.  Updates both cookies.

*This limits the lifetime of stolen refresh tokens. Replay detection and concurrent session detection were intentionally deferred.*

---

## Image Upload Pipeline
Image upload is intentionally separated from registration because a profile image must be validated and stored before a user account exists. An `UploadToken` links the temporary upload to the later registration request.

1.  Choose Image
2.  `UploadImage()`
3.  Validate
4.  Temporary Storage
5.  Extract Metadata
6.  Create Upload Attempt
7.  Return `UploadToken`

*The client displays metadata and stores the `UploadToken` inside a hidden field. The `UploadToken` is later submitted during registration.*

---

## Application Architecture
The MVC layer communicates only through `ViewModels`.

### Communication Path
`ViewModel` -> `DTO` -> `Service` -> `Repository` -> `Stored Procedure`

*   Responses follow the reverse path.
*   Views never communicate directly with `DTOs`.

### Services
*   `UserService`
*   `TokenService`
*   `RefreshTokenService`
*   `ImageService`
*   `ImageUploadService`
*(Each service owns one responsibility.)*

### Repositories
*   `UserRepository`
*   `RefreshTokenRepository`
*   `ImageUploadAttemptRepository`
*(Repositories contain only data-access logic.)*

### RefreshTokenMiddleware
*    - Automatically refreshes expired access tokens.
*    - Executes before authentication.

---

## Configuration

### Jwt
*   `SecretKey`, `Issuer`, `Audience`, `ExpiryMinutes`, `RefreshTokenExpiryDays`

### ImageStorage
*   `TempFolder`, `AvatarsFolder`, `MaxFileSizeBytes`, `UploadTokenExpiryMinutes`

---

## Security Decisions
The project utilizes:
*   BCrypt password hashing
*   HTTP-only cookies
*   Secure cookies
*   SameSite=Strict
*   JWT authentication
*   Refresh token rotation
*   Image signature validation
*   Temporary upload tracking
*   Automatic access token refresh

---

## Deferred Features
The following were intentionally left for future branches:
*   Role-Based Access Control (RBAC)
*   Replay detection
*   Concurrent session detection
*   Background cleanup hosted service
*   Authorization policies
*   Permission system