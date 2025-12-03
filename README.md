# Storefront API

A clean, well-architected ASP.NET Core Web API for e-commerce operations, built for the &Wider Full-Stack Developer technical assessment.

## 📋 Table of Contents

- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Features](#features)
- [Getting Started](#getting-started)
- [API Documentation](#api-documentation)
- [Project Structure](#project-structure)
- [Design Decisions](#design-decisions)
- [TypeScript Client Generation](#typescript-client-generation)
- [Testing the API](#testing-the-api)

---

## 🏗️ Architecture

This API follows **Clean Architecture** principles with clear separation of concerns across four distinct layers:

```
┌─────────────────────────────────────┐
│         API Layer                   │  ← Minimal APIs, Endpoints, Middleware
├─────────────────────────────────────┤
│      Application Layer              │  ← CQRS (Commands/Queries), Handlers, DTOs
├─────────────────────────────────────┤
│    Infrastructure Layer             │  ← Repositories, DbContext, External Services
├─────────────────────────────────────┤
│       Domain Layer                  │  ← Entities, Business Logic (No Dependencies)
└─────────────────────────────────────┘
```

### Why Clean Architecture?

- **Testability**: Business logic is independent of infrastructure
- **Maintainability**: Clear boundaries and dependencies
- **Flexibility**: Easy to swap databases, frameworks, or UI
- **Scalability**: Well-organized for team collaboration

---

## 🛠️ Tech Stack

| Technology | Purpose |
|------------|---------|
| **.NET 8** | Modern framework |
| **ASP.NET Core Minimal APIs** | Lightweight endpoints |
| **Entity Framework Core** | ORM (In-Memory Database) |
| **MediatR** | CQRS pattern implementation |
| **FluentValidation** | Input validation |
| **AutoMapper** | Object-object mapping |
| **Serilog** | Structured logging |
| **NSwag** | OpenAPI documentation + TypeScript client generation |
| **Scrutor** | Convention-based DI registration |

---

## ✨ Features

### Domain Features
- ✅ **Product Management**: Full CRUD operations
- ✅ **Shopping Cart**: Session-based cart management
- ✅ **Soft Deletes**: Data retention with `IsDeleted` flag
- ✅ **Audit Fields**: Track `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy`

### Technical Features
- ✅ **Clean Architecture**: 4-layer separation
- ✅ **CQRS Pattern**: Commands and Queries with MediatR
- ✅ **Repository Pattern**: Data access abstraction
- ✅ **Unit of Work**: Transaction management
- ✅ **Input Validation**: FluentValidation with pipeline behaviors
- ✅ **Structured Logging**: Serilog with console + file output
- ✅ **OpenAPI Documentation**: Swagger UI with NSwag
- ✅ **TypeScript Client Generation**: Auto-generate type-safe client for frontend

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Any IDE: Visual Studio 2022, VS Code, or Rider

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd StorefrontAPI
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run the API**
   ```bash
   cd src/Storefront.API
   dotnet run
   ```

5. **Access Swagger UI**
   
   The API will start and display:
   ```
   Now listening on: https://localhost:7164
   ```
   
   Open your browser to: **https://localhost:7164/swagger**

---

## 📚 API Documentation

### Base URL
```
https://localhost:7164
```

### Endpoints

#### **Products**

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/products` | Get all products |
| `GET` | `/api/products/{id}` | Get product by ID |
| `POST` | `/api/products` | Create a new product |
| `PUT` | `/api/products/{id}` | Update an existing product |
| `DELETE` | `/api/products/{id}` | Delete a product (soft delete) |

#### **Cart**

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/cart?sessionId={id}` | Get cart by session ID |
| `POST` | `/api/cart` | Add item to cart |
| `PATCH` | `/api/cart/{itemId}` | Update cart item quantity |
| `DELETE` | `/api/cart/{itemId}` | Remove item from cart |
| `DELETE` | `/api/cart?sessionId={id}` | Clear entire cart |

### Sample Requests

**Get All Products:**
```bash
curl https://localhost:7164/api/products
```

**Add to Cart:**
```bash
curl -X POST https://localhost:7164/api/cart \
  -H "Content-Type: application/json" \
  -d '{
    "sessionId": "user-session-123",
    "productId": 1,
    "quantity": 2
  }'
```

**Get Cart:**
```bash
curl https://localhost:7164/api/cart?sessionId=user-session-123
```

---

## 📁 Project Structure

```
StorefrontAPI/
├── src/
│   ├── Storefront.Domain/              # Core entities, no dependencies
│   │   ├── Common/                     # Base entity, interfaces
│   │   ├── Entities/                   # Product, Cart, CartItem
│   │   └── Enums/                      # ProductCategory
│   │
│   ├── Storefront.Application/         # Business logic (CQRS)
│   │   ├── Common/
│   │   │   ├── Behaviors/              # MediatR pipeline behaviors
│   │   │   ├── DTOs/                   # Data transfer objects
│   │   │   ├── Exceptions/             # Custom exceptions
│   │   │   ├── Interfaces/             # Repository interfaces
│   │   │   └── Mappings/               # AutoMapper profiles
│   │   ├── Products/
│   │   │   ├── Commands/               # Create, Update, Delete
│   │   │   ├── Queries/                # GetAll, GetById
│   │   │   └── DTOs/                   # Product DTOs
│   │   └── Cart/
│   │       ├── Commands/               # Add, Update, Remove, Clear
│   │       ├── Queries/                # GetCart
│   │       └── DTOs/                   # Cart DTOs
│   │
│   ├── Storefront.Infrastructure/      # Data access
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   └── Configurations/         # EF Core entity configs
│   │   └── Repositories/               # Repository implementations
│   │       ├── GenericRepository.cs
│   │       ├── ProductRepository.cs
│   │       ├── CartRepository.cs
│   │       └── UnitOfWork.cs
│   │
│   └── Storefront.API/                 # Entry point
│       ├── Endpoints/                  # Minimal API endpoints
│       ├── Extensions/                 # Service registration
│       ├── Middleware/                 # Exception handling
│       └── Program.cs                  # Application startup
│
├── logs/                               # Serilog log files (ignored by git)
└── README.md                           # This file
```

---

## 💡 Design Decisions

### Why Clean Architecture?

Clean Architecture provides clear separation of concerns, making the codebase:
- **Testable**: Business logic can be tested without external dependencies
- **Maintainable**: Each layer has a single responsibility
- **Flexible**: Easy to swap out infrastructure (database, UI, etc.)

### Why CQRS?

**Command Query Responsibility Segregation (CQRS)** separates read and write operations:
- **Commands**: Modify state (Create, Update, Delete)
- **Queries**: Return data without modification (Get, List)

**Benefits:**
- Clear intent for each operation
- Easier to optimize separately
- Better testability
- Simpler debugging

### Why Repository Pattern?

The Repository Pattern abstracts data access:
- Provides a collection-like interface for domain objects
- Allows swapping data sources without changing business logic
- Makes unit testing easier with mock repositories

### Why In-Memory Database?

For this assessment:
- ✅ **Simplicity**: No database setup required
- ✅ **Speed**: Fast for demonstrations
- ✅ **Portability**: Runs anywhere without configuration

**In Production**: The repository pattern makes it trivial to swap to SQL Server, PostgreSQL, or any other database - just change the `DbContext` configuration!

### Why NSwag?

NSwag provides:
- ✅ **OpenAPI Documentation**: Interactive Swagger UI
- ✅ **TypeScript Client Generation**: Type-safe frontend integration
- ✅ **Single Source of Truth**: API contract defined once

---

## 🎯 TypeScript Client Generation

### For Frontend Integration

NSwag can auto-generate a fully-typed TypeScript client from your API!

**Step 1: Ensure API is running**
```bash
dotnet run --project src/Storefront.API/Storefront.API.csproj
```

**Step 2: Install NSwag CLI** (one-time setup)
```bash
dotnet tool install -g NSwag.ConsoleCore
```

**Step 3: Generate TypeScript Client**
```bash
nswag openapi2tsclient \
  /input:https://localhost:7164/swagger/v1/swagger.json \
  /output:frontend/src/api/storefront-client.ts \
  /template:Fetch \
  /generateClientClasses:true \
  /generateDtoTypes:true \
  /typeScriptVersion:5.0
```

**Step 4: Use in Your Frontend**

```typescript
import { ProductsClient, CartClient } from './api/storefront-client';

// Create clients
const productsApi = new ProductsClient('https://localhost:7164');
const cartApi = new CartClient('https://localhost:7164');

// Use with full TypeScript support!
const products = await productsApi.getAllProducts();
console.log(products.data); // Fully typed!

await cartApi.addToCart({
  sessionId: 'user-123',
  productId: 1,
  quantity: 2
});
```

**Benefits:**
- ✅ Full IntelliSense/autocomplete
- ✅ Compile-time type checking
- ✅ No typos in API calls
- ✅ Auto-updates when API changes

---

## 🧪 Testing the API

### Using Swagger UI

1. Navigate to `https://localhost:7164/swagger`
2. Explore the interactive API documentation
3. Try out endpoints with the "Try it out" button

### Using cURL

**Get all products:**
```bash
curl https://localhost:7164/api/products
```

**Get product by ID:**
```bash
curl https://localhost:7164/api/products/1
```

**Add item to cart:**
```bash
curl -X POST https://localhost:7164/api/cart \
  -H "Content-Type: application/json" \
  -d '{
    "sessionId": "test-user",
    "productId": 1,
    "quantity": 2
  }'
```

**Get cart:**
```bash
curl "https://localhost:7164/api/cart?sessionId=test-user"
```

**Update cart item:**
```bash
curl -X PATCH https://localhost:7164/api/cart/1 \
  -H "Content-Type: application/json" \
  -d '{"quantity": 5}'
```

### Sample Data

The API includes 8 pre-seeded products:
- Wireless Mouse ($29.99) - Electronics
- Gaming Keyboard ($89.99) - Electronics
- Cotton T-Shirt ($19.99) - Clothing
- Running Shoes ($79.99) - Sports
- Coffee Maker ($49.99) - Home
- Bestseller Novel ($14.99) - Books
- Yoga Mat ($24.99) - Sports
- Face Cream ($34.99) - Beauty

---

## 📝 Logging

### Console Logs
Real-time colored logs in the console during development.

### File Logs
Structured logs written to: `logs/storefront-YYYYMMDD.txt`

**Log Retention:** 7 days

**Example Log Entry:**
```json
{
  "Timestamp": "2024-12-03 14:23:45.123",
  "Level": "Information",
  "Message": "Handled GetAllProductsQuery in 42ms",
  "Properties": {
    "MachineName": "DEV-PC",
    "Application": "StorefrontAPI"
  }
}
```

---

## 📦 Dependencies

### Core Packages
- `Microsoft.AspNetCore.OpenApi` - OpenAPI support
- `Microsoft.EntityFrameworkCore.InMemory` - In-memory database
- `NSwag.AspNetCore` - OpenAPI documentation + client generation

### Application Packages
- `MediatR` - CQRS pattern
- `FluentValidation` - Input validation
- `FluentValidation.DependencyInjectionExtensions` - DI integration
- `AutoMapper` - Object mapping
- `AutoMapper.Extensions.Microsoft.DependencyInjection` - DI integration

### Logging
- `Serilog.AspNetCore` - Structured logging
- `Serilog.Sinks.Console` - Console output
- `Serilog.Sinks.File` - File output
- `Serilog.Enrichers.Environment` - Environment enrichers

### Tools
- `Scrutor` - Assembly scanning for DI

---

## 🎓 Assessment Notes

### What Was Required
- Simple CRUD API
- 4 endpoints: GET products, GET product by ID, POST cart, PATCH cart
- In-memory database
- Clean code

### What Was Delivered
- **10 endpoints** with full CRUD for Products and Cart
- **Clean Architecture** with 4 distinct layers
- **CQRS pattern** for clear command/query separation
- **Repository Pattern** for data access abstraction
- **Input Validation** with FluentValidation
- **Structured Logging** with Serilog
- **OpenAPI Documentation** with interactive Swagger UI
- **TypeScript Client Generation** for seamless frontend integration
- **Production-ready patterns** while keeping appropriate scope

### Architecture Highlights
- **Domain-Driven Design**: Entities model business concepts
- **Separation of Concerns**: Each layer has a single responsibility
- **Dependency Inversion**: High-level modules don't depend on low-level modules
- **SOLID Principles**: Applied throughout the codebase
- **Testability**: Business logic isolated from infrastructure

### Time Investment
Approximately 8 hours to design, implement, and document this solution.

---

## 🚀 Future Enhancements

Given more time, valuable additions would include:

- **Authentication & Authorization**: JWT-based auth with role-based access
- **Database Persistence**: SQL Server/PostgreSQL with migrations
- **Caching**: Redis or in-memory caching
- **API Versioning**: Support for v1, v2, etc.
- **Rate Limiting**: Protect against abuse
- **Health Checks**: Monitoring endpoints
- **Integration Tests**: Automated testing
- **Docker**: Containerization
- **CI/CD Pipeline**: Automated builds and deployments

---

## 👤 Author

**Nkosi** - Full-Stack Developer Candidate

Built for the &Wider technical assessment, December 2024.

---

## 📄 License

This project was created as part of a technical assessment for &Wider.
