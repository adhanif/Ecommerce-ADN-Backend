# Fullstack Ecommerce Fashion ADN

![TypeScript](https://img.shields.io/badge/TypeScript-v.4-green) ![SASS](https://img.shields.io/badge/SASS-v.4-hotpink) ![React](https://img.shields.io/badge/React-v.18-blue) ![Redux toolkit](https://img.shields.io/badge/Redux-v.1.9-brown) ![.NET Core](https://img.shields.io/badge/.NET%20Core-v.8-purple) ![EF Core](https://img.shields.io/badge/EF%20Core-v.8-cyan) ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-v.16-drakblue)

This project involves creating a Fullstack e-commerce platform with a cutting-edge frontend built on TypeScript, React, and Redux Toolkit. The backend is powered by ASP.NET Core 7, utilizing Entity Framework Core for database operations with PostgreSQL. The goal is to deliver a seamless shopping experience for users and provide a robust management system for administrators.

- **Frontend:** TypeScript, React, Redux Toolkit, React Router, Material UI, Jest, React-Hook-Form

  - **Repository:** You can find the frontend project repository [![Frontend Repository](https://img.shields.io/badge/Frontend_Repository-000000?style=for-the-badge&logo=github)](https://github.com/adhanif/Ecommerce-ADN)

  - **Live Demo:** Experience the innovation firsthand by exploring our live demo at [![Ecommerce Fashion ADN](https://img.shields.io/badge/Ecommerce_Fashion_ADN-006400?style=for-the-badge&logo=google-chrome&logoColor=000000)](https://ecommerce-fashion-adn.netlify.app/).

- **Backend:** ASP.NET Core 7, Entity Framework Core, PostgreSQL

## Table of Contents

1. [Technologies and Libraries](#technologies-and-libraries)
2. [Getting Started](#getting-started)
3. [Relational Database Design](#relational-database-design)
4. [Folder Structure](#folder-structure)
5. [Clean Architecture Overview](#clean-architecture-overview)
6. [API Documentation](#api-documentation)
7. [Features](#features)
8. [Testing](#testing)

## Technologies and Libraries

This section outlines the core technologies and essential libraries used in the backend of my e-commerce application, explaining their functions and significance in the overall architecture.

| Technology | Function | Version |
| --- | --- | --- |
| ASP.NET Core | Primary framework for server-side logic, routing, middleware, and dependency management | .NET Core 8.0 |
| Entity Framework Core | ORM (Object-Relational Mapper) for database operations, simplifying SQL queries | 8.0.4 |
| PostgreSQL | Relational database management system for storing application data | 16 |

| Library | Function | Version |
| --- | --- | --- |
| AutoMapper | Automates mapping of data entities to DTOs, reducing manual coding | 10.3.0 |
| Microsoft.AspNetCore.Identity | Manages user authentication, security, password hashing, and role management | 6.0.0 |
| JWT Bearer Authentication | Implements token-based authentication for securing API endpoints | 6.0.0 |
| xUnit | Framework for unit testing, ensuring components work correctly in isolation | 2.4.1 |
| Moq | Mocking library used with xUnit to simulate dependencies during testing | 4.16.1 |

## Getting Started

### Prerequisites

Before you begin, ensure you have the following prerequisites installed on your development environment:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Visual Studio](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/download) (optional but recommended)

### Clone the Repository

Clone the backend repository to your local machine:

```bash
git clone https://github.com/adhanif/fs17-Frontend-project
git clone https://github.com/adhanif/fs17_CSharp_FullStack
cd your-front-repo
cd your-backend-repo
```

#### Set Up PostgreSQL Database

Ensure you have PostgreSQL installed and running. Then, configure your connection strings:

Local Database

```bash
  "ConnectionStrings": {
    "Localhost": "<YOUR_LOCAL_DB_CONNECTION_STRING>"
  }
```

Remote Database

```bash
  "ConnectionStrings": {
    "Remote": "<YOUR_LOCAL_DB_CONNECTION_STRING>"
  }
```

#### Configuration

Update the JWT key and issuer information in your `appsettings.json` or `appsettings.Development.json` file:

```bash
  "Secrets": {
    "JwtKey": "YourJwtSecretKey",
    "Issuer": "YourIssuer"
  }
```

#### Configure Entity Framework Core

Ensure you have the necessary tools and packages installed:

```bash
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
```

#### Create the Database

Run the following commands to create and apply migrations to your database:

```bash
dotnet ef migrations add add Create
dotnet ef database update
```

#### Start the Backend Server

Navigate to the WebAPI layer directory and run the following command to start the backend server on your local machine:

```bash
dotnet watch
```

## Relational database design

![App Screenshot](/images/erd.png)

## Folder Structure

fs17_CSharp_FullStack  
├─ Ecommerce.Controller  
│ ├─ src  
│ │ └─ Controller  
│ │ ├─ AddressController.cs  
│ │ ├─ AuthController.cs  
│ │ ├─ CategoryController.cs  
│ │ ├─ OrderController.cs  
│ │ ├─ ProductController.cs  
│ │ ├─ ReviewController.cs  
│ │ └─ UserController.cs  
│ └─ Ecommerce.Controller.csproj  
├─ Ecommerce.Core  
│ ├─ src  
│ │ ├─ Common  
│ │ │ ├─ AppException.cs  
│ │ │ ├─ BaseQueryOptions.cs  
│ │ │ ├─ ProductQueryOptions.cs  
│ │ │ ├─ UserCredential.cs  
│ │ │ └─ UserQueryOptions.cs  
│ │ ├─ Entity  
│ │ │ ├─ Address.cs  
│ │ │ ├─ BaseEntity.cs  
│ │ │ ├─ Category.cs  
│ │ │ ├─ Order.cs  
│ │ │ ├─ OrderProduct.cs  
│ │ │ ├─ Product.cs  
│ │ │ ├─ ProductImage.cs  
│ │ │ ├─ Review.cs  
│ │ │ └─ User.cs  
│ │ ├─ RepoAbstract  
│ │ │ ├─ IAddressRepo.cs  
│ │ │ ├─ ICategoryRepo.cs  
│ │ │ ├─ IOrderRepo.cs  
│ │ │ ├─ IProductImageRepo.cs  
│ │ │ ├─ IProductRepo.cs  
│ │ │ ├─ IReviewRepo.cs  
│ │ │ └─ IUserRepo.cs  
│ │ ├─ Validation  
│ │ │ ├─ StrongPasswordAttribute.cs  
│ │ │ ├─ UrlValidationAttribute.cs  
│ │ │ └─ ZipCodeValidationAttribute.cs  
│ │ └─ ValueObject  
│ │ ├─ OrderStatus.cs  
│ │ └─ UserRole.cs  
│ └─ Ecommerce.Core.csproj  
├─ Ecommerce.Service  
│ ├─ src  
│ │ ├─ DTO  
│ │ │ ├─ AddressDto.cs  
│ │ │ ├─ CategoryDto.cs  
│ │ │ ├─ OrderDto.cs  
│ │ │ ├─ OrderProductDto.cs  
│ │ │ ├─ ProductDto.cs  
│ │ │ ├─ ProductImageDto.cs  
│ │ │ ├─ ReviewDto.cs  
│ │ │ └─ UserDto.cs  
│ │ ├─ Service  
│ │ │ ├─ AddressService.cs  
│ │ │ ├─ AuthService.cs  
│ │ │ ├─ CategoryService.cs  
│ │ │ ├─ OrderService.cs  
│ │ │ ├─ ProductService.cs  
│ │ │ ├─ ReviewService.cs  
│ │ │ └─ UserService.cs  
│ │ ├─ ServiceAbstract  
│ │ │ ├─ IAddressService.cs  
│ │ │ ├─ IAuthService.cs  
│ │ │ ├─ ICategoryService.cs  
│ │ │ ├─ IOrderService.cs  
│ │ │ ├─ IPasswordService.cs  
│ │ │ ├─ IProductService.cs  
│ │ │ ├─ IReviewService.cs  
│ │ │ ├─ ITokenService.cs  
│ │ │ └─ IUserService.cs  
│ │ └─ Shared  
│ │ └─ MapperProfile.cs  
│ └─ Ecommerce.Service.csproj  
├─ Ecommerce.Test  
│ ├─ src  
│ │ ├─ Core  
│ │ │ └─ removeme.txt  
│ │ └─ Service  
│ │ ├─ AddressServiceTest.cs  
│ │ ├─ AuthServiceTest.cs  
│ │ ├─ CategoryServiceTest.cs  
│ │ ├─ OrderServiceTest.cs  
│ │ ├─ ProductServiceTests.cs  
│ │ ├─ ReviewServiceTest.cs  
│ │ └─ UserServiceTest.cs  
│ └─ Ecommerce.Test.csproj  
├─ Ecommerce.WebAPI

│ ├─ Migrations  
│ │ ├─ 20240522095237_CreateDb.cs  
│ │ ├─ 20240522095237_CreateDb.Designer.cs  
│ │ └─ AppDbContextModelSnapshot.cs  
│ ├─ Properties  
│ │ └─ launchSettings.json  
│ ├─ src  
│ │ ├─ AuthorizationPolicy  
│ │ │ ├─ AdminOrOwnerAccountRequirement.cs  
│ │ │ ├─ AdminOrOwnerOrderRequirement.cs  
│ │ │ ├─ AdminOrOwnerReviewRequirement.cs  
│ │ │ └─ VerifyResourceOwnerAddressRequirement.cs  
│ │ ├─ Database  
│ │ │ ├─ AppDbContext.cs  
│ │ │ ├─ SeedingData.cs  
│ │ │ └─ TimeStampInterceptor.cs  
│ │ ├─ Middleware  
│ │ │ ├─ ExceptionHandlerMiddleware.cs  
│ │ │ └─ VerifyResourceOwnerRequirement.cs  
│ │ ├─ Repo  
│ │ │ ├─ AddressRepo.cs  
│ │ │ ├─ CategoryRepo.cs  
│ │ │ ├─ OrderRepo.cs  
│ │ │ ├─ ProductImageRepo.cs  
│ │ │ ├─ ProductRepo.cs  
│ │ │ ├─ ReviewRepo.cs  
│ │ │ └─ UserRepo.cs  
│ │ ├─ Service  
│ │ │ ├─ PasswordService.cs  
│ │ │ └─ TokenService.cs  
│ │ └─ Program.cs  
│ ├─ appsettings.Development.json  
│ ├─ appsettings.json  
│ ├─ Ecommerce.WebAPI.csproj  
│ └─ Ecommerce.WebAPI.http  
├─ ERD  
│ └─ Schema.erd  
├─ http  
│ ├─ address.http  
│ ├─ category.http  
│ ├─ order.http  
│ ├─ product.http  
│ ├─ review.http  
│ └─ users.http  
├─ Ecommerce.sln  
└─ README.md

## Clean Architecture Overview

This project adheres to Clean Architecture principles, emphasizing separation of concerns and modularity.

![App Screenshot](/images/clean.png)

### Core Domain Layer (Ecommerce.Core)

The **Core Domain Layer** serves as the central repository for core domain logic and entities. It encapsulates essential functionalities, including repository abstractions and value objects. By centralizing these core components, it ensures that the business logic remains focused and independent of external concerns.

### Application Service Layer (Ecommerce.Service)

The **Application Service Layer** implements the business logic of the application. It orchestrates interactions between the controllers and the core domain, handling DTO transformations and business operations related to each resource. This layer is crucial for maintaining the integrity and coherence of the application's business rules.

### Controller Layer (Ecommerce.Controller)

The **Controller Layer** contains the HTTP request and response handling logic. It is organized by resource types such as Auth, Category, Order, Product, Review, and User. Each controller is responsible for managing specific resource endpoints and ensuring that the API correctly handles client requests.

### Infrastructure Layer (Ecommerce.WebAPI)

The **Infrastructure Layer** manages tasks related to infrastructure and external system interactions. It includes the database context, repositories, and middleware for error handling. This layer ensures that the application interacts correctly with the underlying infrastructure components and external dependencies.

### Testing Layer (Ecommerce.Test)

The **Testing Layer** houses unit tests that validate the functionality of the core domain and application services. These tests are essential for ensuring the reliability and correctness of the implemented features. By systematically testing each layer, developers can identify and resolve issues early in the development process.

---

This architectural approach provides a well-structured, maintainable, and scalable codebase. It promotes efficient development and facilitates the seamless integration of new features and improvements into the application.

## API Documentation

All the endpoints of the API are documented and can be tested directly on the generated Swagger page. From there, you can view each endpoint URL, their HTTP methods, request body structures, and authorization requirements.

**Access the Swagger page from this link:**

[![Swagger Page](https://img.shields.io/badge/Swagger-Open%20API%20Specification-85EA2D.svg)](https://fashion-adn.azurewebsites.net/index.html)

Click the button above to explore and test the API endpoints using Swagger.

![App Screenshot](/images/swagger1.png) ![App Screenshot](/images/swagger2.png) ![App Screenshot](/images/swagger3.png) ![App Screenshot](/images/swagger4.png)

## Features

| Feature | Description |
| --- | --- |
| User Authentication and Authorization | Implements secure user registration, login, and role-based access control using JWT tokens. |
| Password Hashing | Ensures sensitive user password information is securely hashed to protect against unauthorized access. |
| Product Management | Provides comprehensive management functionalities including creation, update, deletion, and viewing of product details. |
| Order Processing | Manages the full lifecycle of orders from creation, payment processing, to order fulfillment. |
| Search and Filtering | Implements advanced search and filtering capabilities to facilitate quick product discovery by users. |
| Admin Dashboard | Provides an administrative interface for managing users, orders, products, and other administrative tasks. |
| Unit Testing | Ensures comprehensive test coverage using xUnit and Moq to maintain code quality and reliability. |

## Testing

To execute the unit tests, you can use the .NET CLI. Follow these steps to run the tests from the command line:

**Navigate to the project directory:** Open your terminal or command prompt and go to the root directory of your project.

```bash
cd Ecommerce
```

Execute the tests: Use the following command to run all the tests in the solution:

```bash
dotnet test
```
