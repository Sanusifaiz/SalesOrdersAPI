# Sales Management API

## Overview

The Sales Management API is a backend web application built with ASP.NET Core. It provides endpoints for managing sales orders, products, and a dashboard for analyzing sales data. The application includes features such as Docker integration, logging, authentication using JWT, and real-time updates with SignalR.

## Features

- **Sales Endpoint**:
  - Display a list of sales orders.
  - Create a new sales order.
  - Delete an existing sales order.

- **Products Endpoint**:
  - Display a list of products.
  - Create a new product.
  - Retrieve a product by name.

- **Dashboard Endpoint**:
  - Display products with the highest quantity sold.
  - Display products with the highest price.

- **User Endpoint**:
  - Register a new user.
  - Login and retrieve a JWT token.

- **Authentication**:
  - User registration and login with JWT token generation.

- **Real-Time Updates**:
  - Live sales updates using SignalR.

- **Docker Integration**:
  - Dockerfile for containerization.
  - Instructions to build and run the application using Docker.

- **Logging**:
  - Captures important events, requests, and errors.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [SQLite](https://www.sqlite.org/download.html)

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/yourusername/sales-management-api.git
cd sales-management-api
```

### Configuration
Create an appsettings.json file in the root directory with the following content:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=salesmanagement.db"
  },
  "Jwt": {
    "Key": "YourJwtSecretKey",
    "Issuer": "yourdomain.com",
    "Audience": "yourdomain.com"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### Build and Run
Restore dependencies and build the project:

```bash
dotnet restore
dotnet build
```

Run the application:

```bash
dotnet run
```

### Database Migration
Apply migrations to the SQLite database:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### API Endpoints

#### Sales Endpoint
 GET `/api/salesorder`
* Display a list of sales orders.

 POST `/api/salesorder`
* Create a new sales order.

 DELETE `/api/salesorder/{id}`
* Delete an existing sales order.

#### Products Endpoint

 GET `/api/products`
* Display a list of products.

 POST `/api/products`
* Create a new product.

 GET `/api/products/name/{name}`
* Retrieve a product by name.

#### Dashboard Endpoint

 GET `/api/dashboard/products-highest-quantity-sold`
* Display products with the highest quantity sold.

 GET `/api/dashboard/products-highest-price`
* Display products with the highest price.

#### User Endpoint

 POST `/api/user/signup`
* Register a new user.

 POST `/api/user/login`
* Login and retrieve a JWT token.


### SingalR Hub

Endpoint: `/liveSalesUpdates`

## Authentication

The API uses JWT (JSON Web Tokens) for authentication. After registering or logging in, you will receive a token which must be included in the Authorization header of subsequent requests.

### Register a User

Endpoint: POST /api/user/signup

Request body:

```json
{
  "username": "yourusername",
  "password": "yourpassword"
}
```

### Login

Endpoint: POST `/api/user/login`

Request body:

```json
{
  "username": "yourusername",
  "password": "yourpassword"
}
```

Response

```json
{
  "username": "yourusername",
  "token": "access token"
}
```

Include this token in the Authorization header of your requests:

```makefile
Authorization: Bearer token
```

## Docker Integration

### Dockerfile

A Dockerfile is included to build the application into a Docker image.

### Build and Run with Docker

Build the Docker image:

```bash
docker build -t sales-orders-api .
```

Run the Docker container:

```bash
docker run -d -p 5000:80 --name sales-orders-api sales-orders-api
```

The API will be available at http://localhost:5000.

## Logging
The application uses Serilog for logging. Logs are configured in the appsettings.json file. Adjust the configuration as needed.

## Real-Time Updates with SignalR

SignalR is used for real-time updates. The hub is available at /hubs/sales. Clients can connect to receive real-time sales updates.

### Example Client Connection

Using JavaScript, you can connect to the SignalR hub as follows:

```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/liveSalesUpdates")
    .build();

connection.on("NewSalesOrder", (message) => {
    console.log("New sales order update received: ", message);
});

connection.on("DeletedSalesOrder", (message) => {
    console.log("Deleted sales order update received: ", message);
});

connection.start()
    .then(() => console.log("Connected to the sales live updates hub"))
    .catch(err => console.error("Error connecting to the sales live updates hub: ", err));
```


## Testing
The application includes unit tests for services. To run the tests, use the following command:

```bash
dotnet test
```