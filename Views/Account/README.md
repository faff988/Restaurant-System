# Restaurant Management System

A professional web application for managing restaurant operations, built with ASP.NET Core, Entity Framework, and MySQL.

## Getting Started

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [MySQL Server](https://dev.mysql.com/downloads/installer/) or MariaDB (XAMPP/WAMP)

### Installation Steps
1. **Extract the project package** to your local directory.
2. **Configure the Database**:
   * Open `appsettings.json`.
   * Ensure the `DefaultConnection` string points to your MySQL server (Host, Port, User, and Password).
   * *Note: For Railway hosting, the system is pre-configured to use Environment Variables for security.*
3. **Initialize the Database**:
   The system is designed to handle migrations automatically on startup. However, you can manually run them via the Package Manager Console:
   ```bash
   dotnet ef database update
   ```
4. **Run the Application**:
   Execute the following command in your terminal:
   ```bash
   dotnet run
   ```

## Default Credentials
Upon initial startup, the system automatically seeds the roles and a default administrator account:
* **Email**: `admin@restaurant.com`
* **Password**: `admin123`

## User Roles
The system features a three-tier access control system:
1. **Administrator**: Full access to the Dashboard, User Management, Reports, and System Settings.
2. **Staff**: Access to business operations, including managing live Orders and table Reservations.
3. **Customer**: Access to browse the Menu, place new Orders, and book table Reservations.

## Tech Stack
* **Backend**: C# ASP.NET Core MVC
* **Database**: MySQL / MariaDB via Entity Framework Core
* **Authentication**: Microsoft Identity
* **Frontend**: Razor Pages, Bootstrap 5.3, FontAwesome 6.4

## Documentation Requirement
This project includes automatic database migration logic in `Program.cs` to ensure "Zero-Configuration" deployment on platforms like Railway.

---
*Submitted for Web Frameworks Assignment.*