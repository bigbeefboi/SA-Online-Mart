# SA-Online-Mart

## Getting Started

Follow these steps to set up and run the project.

### Prerequisites

- Visual Studio 2022
- SQL Server Management Studio (SSMS) or SQL Server tools within Visual Studio

### Setup Instructions

1. **Open the Project**
   - Open the project using Visual Studio 2022.

2. **Create a Database**
   - You can create a database in either SSMS or within Visual Studio.
   - Name the database as you like.
   - Once the database is created, add the connection string to the `appsettings.json` file in the project.

3. **Apply Migrations and Update the Database**
   - Open the Package Manager Console in Visual Studio.
   - Navigate to the project directory:
     ```powershell
     cd .\TutorialsEUIdentity
     ```
     or
     ```powershell
     cd .\SAOnlineMart
     ```
   - Run the following command to add the initial migration:
     ```powershell
     dotnet ef migrations add init
     ```
   - Update the database with the following command:
     ```powershell
     dotnet ef database update
     ```

4. **Troubleshooting**
   - If the migration or database update does not work:
     1. Remove the migration files from the `Migrations` folder.
     2. Repeat the steps to apply migrations and update the database.
     3. Use the following command to check the current path:
        ```powershell
        dir
        ```

5. **Contact**
   - If you encounter any issues, please contact me on Teams Sir.

---

