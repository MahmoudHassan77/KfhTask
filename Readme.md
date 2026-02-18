# Expenses API

ASP.NET Core Web API for managing personal expenses with JWT authentication.

## Quick Start

```bash
# Clone and setup
git clone <repository-url>
cd Expenses
dotnet restore
dotnet build

# Run the API
cd src/Expenses.Api
dotnet run
```

API runs on: https://localhost:7088

## Features

- CRUD operations for expenses
- User registration and JWT authentication
- Password hashing with BCrypt
- Pagination and filtering
- Optimistic concurrency control
- SQLite database with EF Core

## Tech Stack

- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- JWT Bearer Authentication
- BCrypt.Net

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login and get JWT token

### Expenses
- `GET /api/expenses` - List expenses (supports pagination and filtering)
- `GET /api/expenses/{id}` - Get expense by ID
- `POST /api/expenses` - Create expense (requires auth)
- `PUT /api/expenses/{id}` - Update expense (requires auth)
- `DELETE /api/expenses/{id}` - Delete expense (requires auth)

## Usage Examples

### Login
```bash
curl -X POSThttps://localhost:7088/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"demo","password":"demo123"}'
```

### Create Expense
```bash
curl -X POST https://localhost:7088/api/expenses \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Lunch",
    "amount": 100.50,
    "currency": "USD",
    "category": "Food",
    "occurredOn": "2026-02-18"
  }'
```

### Get Expenses with Filters
```bash
curl "https://localhost:7088/api/expenses?category=Food&page=1&pageSize=10"
```


## Database

SQLite database is auto-created on first run with seeded demo users.

For manual setup:
```bash
cd src/Expenses.Api
sqlite3 expenses.db < ../../deploy/InitialCreate.sql
```


## Development


### Create Migration
```bash
cd src/Expenses.Api
dotnet ef migrations add MigrationName
```

### Generate SQL Script
```bash
dotnet ef migrations script --output ../../deploy/script.sql
```



## Configuration

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=expenses.db"
  },
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "ExpensesAPI",
    "Audience": "ExpensesClient",
    "ExpireMinutes": 60
  },
  "AllowedCurrencies": ["EGP", "USD", "EUR"]
}
```
