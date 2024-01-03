# ASP.NET Core 8 API Template
---

- A very basic API.NET Core 8 Web API that uses PostgreSQL as the database
- Inspired by Vertical Slice Architecture
- Various dev settings can be configured in **appsettings.json**

### Contains the following basic features:
- Identity with JWT access tokens and refresh tokens
- Logging
- User profile


### Before running the project:
- Add dev https cert to localhost
    ```
    dotnet dev-certs https --trust
    ```
- Add migrations
    ```
    Add-Migration Initial
    Update-Database
    ```
- Run using the _**https**_ debug profile in Visaul Studio