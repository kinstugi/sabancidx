# Case Study: Product Service

This is a .NET RESTful API project for managing products and users. It provides endpoints for creating, retrieving, updating, and deleting products, as well as user-related functionality.

## Table of Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Endpoints](#endpoints)
- [Authentication and Authorization](#authentication-and-authorization)
- [Testing](#testing)

## Getting Started

### Prerequisites

Before running this project, ensure you have the following prerequisites installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 7.0.0)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) (for database operations)
- [Database Engine](e.g., SQL Server, SQLite) and connection string configured in `appsettings.json`

### Installation

1. Clone the repository:

   ```sh
   git clone https://github.com/kinstugi/sabancidx.git
   cd sabancidx
   ```
2. Add migrations and push changes to database (in this case we are using sqlite database)
    ```
    dotnet ef migrations Add InitialMigration
    ```
    ```
    dotnet ef database update
    ```

3.  Build and run the project:
    ```sh
    dotnet build
    dotnet run
    ```

### Usage
This API provides functionality for managing products and users. You can interact with the API using HTTP requests. Below are the available endpoints and their descriptions.

### Endpoints
#### Products
- GET /api/products: Get a list of all products.
- GET /api/products/me: Get a list of all your products.
- GET /api/products/{id}: Get a specific product by ID.
- POST /api/products: Create a new product.
- PUT /api/products/{id}: Update an existing product.
- DELETE /api/products/{id}: Delete a product.

#### Users
- POST /api/users/signup: Register a new user.
- POST /api/users/login: Log in an existing user.


For detailed information on how to use these endpoints, please refer to the API documentation.
- /swagger/index.html

### Authentication and Authorization
This API uses token-based authentication. To access protected endpoints, include a valid authentication token in the Authorization header of your HTTP requests.