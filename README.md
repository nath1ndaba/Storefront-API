# Storefront API

A clean, well-architected ASP.NET Core Web API for e-commerce operations.

## Architecture

This API follows **Clean Architecture** principles with clear separation of concerns:

- **Domain Layer** - Core entities (Product, Cart, CartItem)
- **Application Layer** - Business logic with CQRS pattern (MediatR)
- **Infrastructure Layer** - Data access (EF Core, Repositories)
- **API Layer** - RESTful endpoints (Minimal APIs)

## Tech Stack

- .NET 8
- ASP.NET Core Minimal APIs
- Entity Framework Core (In-Memory)
- MediatR (CQRS)
- FluentValidation
- AutoMapper
- Serilog (Structured Logging)
- NSwag (OpenAPI + TypeScript Client Generation)

## Features

### Products
- Get all products
- Get product by ID
- Create product
- Update product
- Delete product (soft delete)

### Cart
- Get cart by session ID
- Add item to cart
- Update cart item quantity
- Remove item from cart
- Clear entire cart

## Getting Started

### Run the API

```bash
cd src/Storefront.API
dotnet run
```

API starts at: **https://localhost:7164**

### Explore the API

- **Swagger UI**: https://localhost:7164/swagger
- **OpenAPI Spec**: https://localhost:7164/swagger/v1/swagger.json
- **API Info**: https://localhost:7164

## API Endpoints

### Products
```
GET    /api/products          - Get all products
GET    /api/products/{id}     - Get product by ID
POST   /api/products          - Create product
PUT    /api/products/{id}     - Update product
DELETE /api/products/{id}     - Delete product
```

### Cart
```
GET    /api/cart?sessionId={id}  - Get cart
POST   /api/cart                 - Add to cart
PATCH  /api/cart/{itemId}        - Update cart item
DELETE /api/cart/{itemId}        - Remove from cart
DELETE /api/cart?sessionId={id}  - Clear cart
```

## Generate TypeScript Client

**For Frontend Integration:**

```bash
# Start the API first
cd src/Storefront.API
dotnet run

# In a new terminal, generate the client
.\generate-client.ps1
```

This creates: `frontend/src/api/storefront-client.ts`

**Usage in React/Vue/Angular:**

```typescript
import { ProductsClient, CartClient } from './api/storefront-client';

// Create clients
const productsClient = new ProductsClient('http://localhost:7164');
const cartClient = new CartClient('http://localhost:7164');

// Use with full TypeScript support!
const products = await productsClient.getAllProducts();
const cart = await cartClient.getCart('session-123');

// Add to cart
await cartClient.addToCart({
  sessionId: 'session-123',
  productId: 1,
  quantity: 2
});
```

## Sample Data

The API includes 8 pre-seeded products:
- Wireless Mouse ($29.99)
- Gaming Keyboard ($89.99)
- Cotton T-Shirt ($19.99)
- Running Shoes ($79.99)
- Coffee Maker ($49.99)
- Bestseller Novel ($14.99)
- Yoga Mat ($24.99)
- Face Cream ($34.99)

## Project Structure

```
src/
├── Storefront.Domain/       # Core entities, no dependencies
├── Storefront.Application/  # CQRS handlers, DTOs, validation
├── Storefront.Infrastructure/ # Data access, repositories
└── Storefront.API/          # Minimal API endpoints
```

## Architecture Highlights

### CQRS Pattern
- **Commands** - Write operations (Create, Update, Delete)
- **Queries** - Read operations (Get, List)
- Clear separation using MediatR

### Repository Pattern
- Generic repository for common operations
- Specialized repositories for complex queries
- Unit of Work for transaction management

### Clean Code
- Dependency Injection with Scrutor
- FluentValidation for input validation
- AutoMapper for object mapping
- Serilog for structured logging

## Logging

Logs are written to:
- **Console** - Colored, human-readable
- **Files** - `logs/storefront-YYYYMMDD.txt` (7-day retention)

## Testing the API

### Using Swagger UI

1. Navigate to https://localhost:7164/swagger
2. Try the interactive endpoints

### Example: Add to Cart

```bash
curl -X POST "http://localhost:7164/api/cart" \
  -H "Content-Type: application/json" \
  -d '{
    "sessionId": "test-user-123",
    "productId": 1,
    "quantity": 2
  }'
```

### Example: Get Cart

```bash
curl "http://localhost:7164/api/cart?sessionId=test-user-123"
```

## Design Decisions

### Why Clean Architecture?
- **Testability** - Business logic independent of infrastructure
- **Maintainability** - Clear boundaries and dependencies
- **Flexibility** - Easy to swap out database or UI

### Why CQRS?
- **Separation of Concerns** - Read and write operations are separate
- **Scalability** - Can optimize queries and commands independently
- **Clarity** - Clear intent of each operation

### Why In-Memory Database?
- **Assessment Scope** - Sufficient for demonstration
- **Easy Setup** - No configuration needed
- **Swappable** - Repository pattern makes DB swap trivial

### Why NSwag?
- **Type Safety** - Generates TypeScript client with full types
- **Developer Experience** - Frontend gets IntelliSense and validation
- **Consistency** - Single source of truth from OpenAPI spec

## Technical Assessment Notes

**What was required:**
- Simple CRUD API
- 4 endpoints (GET products, GET product, POST cart, PATCH cart)
- In-memory database

**What was delivered:**
- Clean Architecture with 4 layers
- CQRS pattern with MediatR
- 10 endpoints with full CRUD
- Repository pattern
- Input validation
- Structured logging
- OpenAPI documentation
- TypeScript client generation

**Why this architecture?**
- Demonstrates senior-level design thinking
- Production-ready patterns
- Maintainable and testable
- Appropriate complexity for the scope

## Future Enhancements

Given more time, valuable additions would be:
- JWT Authentication
- Real database (SQL Server/PostgreSQL)
- Integration tests
- Docker containerization
- CI/CD pipeline
- API versioning
- Rate limiting
- Health checks

---

Built with ❤️ using Clean Architecture and modern .NET best practices.
