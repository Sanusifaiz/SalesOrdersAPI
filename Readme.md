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

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker](https://www.docker.com/get-started)
- [SQLite](https://www.sqlite.org/download.html)

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/yourusername/sales-management-api.git
cd sales-management-api