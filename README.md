# E-Commerce Platform

A full-stack e-commerce web application built with **ASP.NET Core MVC**, **Dapper**, **SQL Server**, and **JWT authentication**.

The project covers the core functionality of an online store, including authentication, product browsing, shopping carts, checkout, order management, user profiles, and an admin panel.

## Tech Stack

### Backend

* C#
* ASP.NET Core MVC
* Dapper
* SQL Server
* Stored Procedures
* JWT Authentication
* Refresh Tokens
* BCrypt

### Frontend

* Razor Views
* HTML
* CSS
* Bootstrap
* JavaScript
* Fetch API / AJAX

### Development

* Visual Studio
* Git
* GitHub

## Features

### Authentication

* User registration and login
* BCrypt password hashing
* JWT access tokens
* Refresh tokens
* HTTP-only cookies
* Automatic token refresh
* Role-based authorization
* Customer and Admin roles

### Products & Categories

* Browse products
* Product details
* Product search
* Category filtering
* Date filtering
* Product CRUD
* Category CRUD
* Product stock management
* Product image uploads
* Soft delete and restore

### Shopping Cart

* Add products to cart
* Update quantities
* Remove products
* Prevent invalid quantities
* Stock validation
* Persistent database-backed cart
* AJAX cart operations

### Checkout & Orders

* Checkout page
* Cart summary
* Order creation
* Order items
* Stock validation
* Database transactions
* Automatic cart clearing after successful order
* Order history
* Order details

Order status flow:

```text
Pending → Processing → Shipped → Delivered
                    ↘
                    Cancelled
```

### User Profile

* View and update profile
* Change password
* Profile image upload
* Automatic replacement of old profile images
* Client-side dirty checking
* JWT claims refreshed after profile updates

### Admin Panel

A separate Admin Area is protected using role-based authorization.

Admins can:

* View dashboard statistics
* Manage products
* Manage categories
* Manage users
* View low-stock products
* Soft delete records
* Restore deleted records

## Architecture

The application follows a layered architecture:

```text
View
  ↓
ViewModel
  ↓
Controller
  ↓
Service
  ↓
Dapper
  ↓
Stored Procedure
  ↓
SQL Server
```

DTOs are used to transfer data between application layers where appropriate.

The project intentionally uses **Dapper and stored procedures instead of Entity Framework Core**, and authentication is implemented without ASP.NET Identity.

## Database

The main database entities include:

```text
Users
Roles
RefreshTokens

Categories

Products
ProductImages

Cart
CartItems

Orders
OrderItems
```

Database operations are primarily handled through stored procedures.

Read procedures commonly return:

```text
Result Set 1
ResponseCode
ResponseMessage

Result Set 2
Data
```

Dapper's `QueryMultipleAsync()` is used to consume multiple result sets.

## Security

The application includes:

* BCrypt password hashing
* JWT authentication
* HTTP-only cookies
* Refresh tokens
* Role-based authorization
* Claims-based user identification
* CSRF/antiforgery protection
* Parameterized database queries
* Server-side validation

Admin endpoints are protected server-side using role authorization.

```csharp
[Authorize(Roles = "Admin")]
```

## Getting Started

### Prerequisites

Install the following:

* .NET SDK
* SQL Server
* Visual Studio
* Git

### 1. Clone the repository

```bash
git clone https://github.com/Umer-Iftikhar/E-commerce.git
cd E-commerce
```

### 2. Configure the database

Create a SQL Server database and run the SQL scripts included with the project.

The scripts create the required:

* Tables
* Relationships
* Stored procedures
* Seed data

Update the application's connection string with your SQL Server configuration.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Use your own SQL Server instance and database name.

### 3. Configure application settings

Add the required JWT and image-storage configuration to your application settings.

Do not commit production secrets, credentials, or sensitive configuration to Git.

### 4. Run the application

From the project directory:

```bash
dotnet restore
dotnet build
dotnet run
```

Or open the solution in Visual Studio and run the ASP.NET Core application.

### 5. Create an account

Register a user through the application and log in.

The application will use JWT authentication and refresh tokens to maintain the authenticated session.

### 6. Admin access

An Admin user is required to access the Admin Area.

Admin endpoints are protected with:

```csharp
[Area("Admin")]
```
```csharp
[Authorize(Roles = "Admin")]
```

## Project Structure

```text
E-Commerce/
│
├── Database/
│   │
│   ├── Scripts/
│   │   ├── 001_initialScript.sql
│   │   ├── 002_AddProductsAndCategories.sql
│   │   ├── 003_AddCart.sql
│   │   ├── 004_AddOrders.sql
│   │   └── 005_SeedAdmin.sql
│   │
│   └── StoredProcedures/
│       ├── Cart/
│       ├── Categories/
│       ├── Dashboard/
│       ├── Orders/
│       ├── Products/
│       ├── Profile/
│       ├── RefreshToken/
│       └── Users/
│
└── E-Commerce/
    │
    ├── Areas/
    │   └── Admin/
    │       ├── Controllers/
    │       └── Views/
    │
    ├── Constants/
    ├── Controllers/
    ├── CustomAttributes/
    ├── DTOs/
    ├── Data/
    ├── Helpers/
    ├── Middlewares/
    ├── Models/
    ├── Properties/
    ├── Services/
    ├── Settings/
    ├── ViewModels/
    ├── Views/
    │
    └── wwwroot/
        ├── css/
        │   ├── auth.css
        │   ├── cart.css
        │   ├── checkout.css
        │   ├── profile.css
        │   └── site.css
        │
        ├── icons/
        │
        ├── js/
        │   ├── admin/
        │   │   ├── categories.js
        │   │   ├── products.js
        │   │   └── users.js
        │   │
        │   ├── cart.js
        │   ├── checkout.js
        │   ├── orders.js
        │   ├── product-search.js
        │   ├── profile.js
        │   └── site.js
        │
        ├── lib/
        └── uploads/
```

## Project Status

**Completed**

The project is considered complete and is no longer under active development.

The main goal was to build a complete e-commerce application while gaining practical experience with ASP.NET Core MVC, Dapper, SQL Server, stored procedures, JWT authentication, authorization, database transactions, AJAX, file handling, and Git-based development.
