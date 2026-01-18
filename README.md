# Library Management System - REST API

## 📋 Overview

A comprehensive RESTful API for library management built with .NET 8, featuring JWT authentication, role-based authorization, and complete CRUD operations for books and clients.

## 🏗️ Architecture

- **Clean Architecture** with separate layers (API, Business Logic, Data Access)
- **Repository Pattern** for data access abstraction
- **JWT Authentication** for secure API access
- **Role-Based Authorization** (Librarian, Client)
- **Entity Framework Core** with SQL Server
- **Swagger/OpenAPI** documentation
- **Global Exception Handling**
- **Input Validation** with FluentValidation

## 🚀 Features

### Authentication & Authorization
- User registration and login
- JWT token-based authentication
- Role-based access control (Librarian, Client)

### Book Management
- ✅ Add new books (Librarian only)
- ✅ Remove books (Librarian only)
- ✅ Update book information (Librarian only)
- ✅ List all books
- ✅ Search books by title/author

### Client Management
- ✅ Register new clients
- ✅ View client information
- ✅ View borrowing history

### Borrowing Operations
- ✅ Borrow books (Client)
- ✅ Return books (Client)
- ✅ View currently borrowed books
- ✅ Track borrowing history
- ✅ Prevent borrowing unavailable books

## 📁 Project Structure

```
LibraryManagementSystem/
├── LibraryAPI/                    # Main API project
│   ├── Controllers/               # API endpoints
│   ├── Middleware/                # Custom middleware
│   ├── Program.cs                 # App configuration
│   └── appsettings.json          # Configuration
├── LibraryAPI.Core/              # Business logic & models
│   ├── Models/                    # Domain entities
│   ├── DTOs/                      # Data transfer objects
│   ├── Interfaces/                # Service interfaces
│   └── Services/                  # Business logic
├── LibraryAPI.Data/              # Data access layer
│   ├── Context/                   # DbContext
│   ├── Repositories/              # Repository implementations
│   └── Migrations/                # EF migrations
└── README.md                      # This file
```

## 🛠️ Technology Stack

- **.NET 8.0** - Framework
- **ASP.NET Core Web API** - REST API
- **Entity Framework Core 8.0** - ORM
- **SQL Server** - Database (LocalDB for development)
- **JWT Bearer** - Authentication
- **Swagger/Swashbuckle** - API Documentation
- **BCrypt.Net** - Password hashing

## ⚙️ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recommended) or VS Code
- SQL Server 2019+ or LocalDB (comes with Visual Studio)

## 📦 Installation & Setup

### Step 1: Clone or Extract the Project

```bash
# If using Git
git clone 
cd LibraryManagementSystem

# Or extract the ZIP file and navigate to the folder
```

### Step 2: Restore Dependencies

```bash
dotnet restore
```

### Step 3: Configure Database Connection

Open `LibraryAPI/appsettings.json` and verify the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LibraryDB;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

**Note:** This uses LocalDB. For SQL Server, use:
```json
"DefaultConnection": "Server=localhost;Database=LibraryDB;User Id=sa;Password=YourPassword;TrustServerCertificate=true"
```

### Step 4: Create Database

```bash
cd LibraryAPI
dotnet ef database update
```

This will create the database and apply all migrations.

### Step 5: Run the Application

```bash
dotnet run --project LibraryAPI
```

Or press **F5** in Visual Studio.

The API will start at:
- **HTTPS:** https://localhost:7001
- **HTTP:** http://localhost:5001
- **Swagger UI:** https://localhost:7001/swagger

## 🎯 Quick Start Guide

### 1. Register a Librarian Account

**POST** `/api/auth/register`

```json
{
  "username": "admin",
  "email": "admin@library.com",
  "password": "Admin@123",
  "role": "Librarian"
}
```

### 2. Login

**POST** `/api/auth/login`

```json
{
  "username": "admin",
  "password": "Admin@123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "role": "Librarian"
}
```

### 3. Add Books (Use token from login)

**POST** `/api/books`
**Header:** `Authorization: Bearer <your-token>`

```json
{
  "title": "Clean Code",
  "author": "Robert C. Martin",
  "isbn": "978-0132350884",
  "publishedYear": 2008,
  "totalCopies": 5
}
```

### 4. Register a Client

**POST** `/api/auth/register`

```json
{
  "username": "john_doe",
  "email": "john@example.com",
  "password": "Client@123",
  "role": "Client"
}
```

### 5. Borrow a Book (as Client)

**POST** `/api/borrow/{bookId}`
**Header:** `Authorization: Bearer <client-token>`

### 6. Return a Book (as Client)

**POST** `/api/borrow/return/{borrowingId}`
**Header:** `Authorization: Bearer <client-token>`

## 📚 API Endpoints

### Authentication
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register new user | No |
| POST | `/api/auth/login` | Login and get JWT token | No |

### Books (Librarian Operations)
| Method | Endpoint | Description | Role Required |
|--------|----------|-------------|---------------|
| GET | `/api/books` | Get all books | None |
| GET | `/api/books/{id}` | Get book by ID | None |
| POST | `/api/books` | Add new book | Librarian |
| PUT | `/api/books/{id}` | Update book | Librarian |
| DELETE | `/api/books/{id}` | Remove book | Librarian |
| GET | `/api/books/search?query=` | Search books | None |

### Borrowing (Client Operations)
| Method | Endpoint | Description | Role Required |
|--------|----------|-------------|---------------|
| POST | `/api/borrow/{bookId}` | Borrow a book | Client |
| POST | `/api/borrow/return/{borrowingId}` | Return a book | Client |
| GET | `/api/borrow/my-books` | Get currently borrowed books | Client |
| GET | `/api/borrow/history` | Get borrowing history | Client |

### Clients
| Method | Endpoint | Description | Role Required |
|--------|----------|-------------|---------------|
| GET | `/api/clients` | Get all clients | Librarian |
| GET | `/api/clients/{id}` | Get client details | Librarian/Own |

## 🧪 Testing the API

### Using Swagger UI
1. Navigate to `https://localhost:7001/swagger`
2. Click on **Authorize** button (🔓)
3. Enter: `Bearer <your-token>`
4. Test endpoints directly from the browser

### Using Postman/Thunder Client
1. Import the collection or create requests manually
2. Set Authorization header: `Authorization: Bearer <token>`
3. Test all endpoints

### Sample Test Flow

```bash
# 1. Register Librarian
POST /api/auth/register
Body: {"username":"librarian1","email":"lib@test.com","password":"Pass@123","role":"Librarian"}

# 2. Login
POST /api/auth/login
Body: {"username":"librarian1","password":"Pass@123"}
# Copy the token

# 3. Add Book (use token)
POST /api/books
Headers: Authorization: Bearer 
Body: {"title":"Design Patterns","author":"Gang of Four","isbn":"978-0201633610","publishedYear":1994,"totalCopies":3}

# 4. Register Client
POST /api/auth/register
Body: {"username":"client1","email":"client@test.com","password":"Pass@123","role":"Client"}

# 5. Client Login
POST /api/auth/login
Body: {"username":"client1","password":"Pass@123"}
# Copy the client token

# 6. Borrow Book (use client token)
POST /api/borrow/1
Headers: Authorization: Bearer 

# 7. View Borrowed Books
GET /api/borrow/my-books
Headers: Authorization: Bearer 

# 8. Return Book
POST /api/borrow/return/1
Headers: Authorization: Bearer 
```

## 🔐 Security Features

- **Password Hashing** with BCrypt
- **JWT Authentication** with configurable expiration
- **Role-Based Authorization** (Librarian, Client)
- **HTTPS Enforcement** in production
- **CORS Configuration** for cross-origin requests
- **Input Validation** for all endpoints

## 🗄️ Database Schema

### Users Table
- Id (PK)
- Username (Unique)
- Email (Unique)
- PasswordHash
- Role (Librarian/Client)
- CreatedAt

### Books Table
- Id (PK)
- Title
- Author
- ISBN (Unique)
- PublishedYear
- TotalCopies
- AvailableCopies
- CreatedAt

### Borrowings Table
- Id (PK)
- BookId (FK)
- UserId (FK)
- BorrowedDate
- DueDate
- ReturnedDate (Nullable)
- Status (Borrowed/Returned)

## 🐛 Troubleshooting

### Database Connection Issues

**Error:** "Cannot open database"
```bash
# Reset database
dotnet ef database drop
dotnet ef database update
```

### Port Already in Use

**Error:** "Address already in use"
- Change ports in `LibraryAPI/Properties/launchSettings.json`
- Or kill the process using the port

### JWT Token Issues

**Error:** "Invalid token"
- Check token expiration (default 24 hours)
- Ensure token is passed as: `Bearer <token>`
- Verify JWT secret in appsettings.json

## 📝 Development Notes

### Adding New Migration

```bash
cd LibraryAPI
dotnet ef migrations add 
dotnet ef database update
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true
```

## 🎨 Best Practices Implemented

- ✅ **Clean Architecture** - Separation of concerns
- ✅ **Repository Pattern** - Data access abstraction
- ✅ **Dependency Injection** - Loose coupling
- ✅ **Async/Await** - Non-blocking operations
- ✅ **Exception Handling** - Global error handling
- ✅ **Logging** - Structured logging with Serilog
- ✅ **Validation** - Input validation
- ✅ **Documentation** - Swagger/OpenAPI
- ✅ **Security** - JWT, HTTPS, password hashing

## 📊 Demo Preparation Checklist

Before your interview:
- [ ] Test all endpoints in Swagger
- [ ] Prepare sample data (2-3 books, 2 users)
- [ ] Test borrowing and returning books flow
- [ ] Verify role-based access control
- [ ] Test error scenarios (borrowing unavailable book, etc.)
- [ ] Have Swagger UI open and ready
- [ ] Ensure database is seeded with test data

## 🎤 Interview Demo Flow

1. **Show the Solution Structure** (2 min)
   - Explain Clean Architecture
   - Show separation of concerns

2. **Run the Application** (1 min)
   - Start the API
   - Open Swagger UI

3. **Demonstrate Authentication** (3 min)
   - Register Librarian
   - Register Client
   - Show JWT token generation

4. **Show Librarian Operations** (4 min)
   - Add new books
   - Update book information
   - Show role-based access

5. **Show Client Operations** (4 min)
   - Borrow a book
   - View borrowed books
   - Return a book
   - Show borrowing restrictions

6. **Highlight Key Features** (2 min)
   - Security measures
   - Error handling
   - Validation
   - Database relationships

## 📧 Support

If you encounter any issues, check:
1. .NET 8 SDK is installed: `dotnet --version`
2. Database connection string is correct
3. All NuGet packages are restored
4. Database migrations are applied

## 🏆 Why This Solution Stands Out

1. **Production-Ready Code** - Not a simple demo
2. **Best Practices** - Clean Architecture, SOLID principles
3. **Complete Features** - All requirements + extras
4. **Security First** - JWT, password hashing, authorization
5. **Well Documented** - Clear README, code comments
6. **Professional Structure** - Organized, maintainable
7. **Error Handling** - Robust error management
8. **Easy to Demo** - Swagger UI, clear endpoints

---

## 📝 License

This project is created for interview assessment purposes.

**Good luck with your interview! 🚀**
