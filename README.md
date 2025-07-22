CDN.Directory — Freelancer Directory Management System

📌 Project Overview
CDN.Directory is a .NET 8 Web Application that allows users to manage a directory of Freelancers (Members).
Each Member can be linked to multiple pre-registered Skillsets and Hobbies.
The system provides a clean MVC-based UI for CRUD operations and a separate API with Swagger for RESTful interactions.


🔧 Tech Stack

.NET 8

Razor Pages (CDN.Directory.UI)

Web API (CDN.Directory.API)

Entity Framework Core 8 + Pomelo (MySQL)

AutoMapper

FluentValidation

xUnit for Unit Testing

MySQL Workbench (Local DB)


🎯 Features

Clean Architecture (API / Core / Infrastructure / UI / Tests / Seeder)

Members CRUD (Create, Read, Update, Delete)

Wildcard Search by Username or Email

Pre-Registered Skillsets & Hobbies via Master Tables

Dropdown Selection + Typeahead Filtering (UI)

Data Integrity via FK Constraints

Fully Unit Tested


📂 Solution Structure

├── CDN.Directory.Core
│ ├── Entities (Member, SkillsetMaster, HobbyMaster, Link Tables)
│ └── DTOs (Create, Update, Read)
├── CDN.Directory.Infrastructure
│ ├── DbContext / Migrations (EF Core, MySQL)
│ └── AppDbContextFactory for CLI commands
├── CDN.Directory.API
│ ├── RESTful API
│ └── Swagger for testing
├── CDN.Directory.UI
│ ├── Razor Pages for CRUD
│ └── MemberScaffoldController
├── CDN.Directory.Seeder
│ ├── Seeds Skillsets / Hobbies CSV into DB
├── CDN.Directory.Tests
│ ├── xUnit for API Controllers
│ └── InMemoryDbContext for tests
└── CDN.Directory.sln


🛠️ Setup & Running Locally

Step 1 Prerequisites

MySQL Installed & Running (Workbench / Server)

.NET 8 SDK Installed

Step 2 MySQL Setup
Create a database:

CREATE DATABASE cdn_directory_db;

Ensure collation: utf8_general_ci (case-insensitive)

Step 3 Update Connection Strings
File: appsettings.json (CDN.Directory.API / CDN.Directory.Infrastructure / CDN.Directory.UI)

"ConnectionStrings": {
"DefaultConnection": "server=localhost;port=3306;database=cdn_directory_db;user=root;password=yourpassword;"
}

Step 4 Apply EF Core Migrations
In Infrastructure Project Directory:

dotnet ef database update

(This will generate the full schema into your MySQL Workbench)


📋 ERD Structure

Members (PK)

MemberSkillsets (FK to Members / SkillsetMaster)

MemberHobbies (FK to Members / HobbyMaster)

SkillsetMaster (PK, Unique Names)

HobbyMaster (PK, Unique Names)

✅ Relations Enforced, Cascades on Member Delete.

Step 5 Seeding Initial Data
To pre-populate Skillsets & Hobbies with sample data:

Navigate to CDN.Directory.Seeder:
dotnet run

This seeds CSV of Skillsets / Hobbies into Master Tables.

Step 6 Running Locally (UI)
Navigate to CDN.Directory.UI:

dotnet run

Access: https://localhost:<port>/MemberScaffold

Step 7 Running Locally (API / Swagger)
Navigate to CDN.Directory.API:
dotnet run

Access: https://localhost:<port>/swagger

Step 8 Running Unit Tests
Navigate to CDN.Directory.Tests:

dotnet test


📝 Demonstration Flow for Presentation (Suggested Steps)

Open MySQL Workbench (show schema cdn_directory_db)

Run CDN.Directory.UI (CRUD)

Create Member

View Members

Update Member

Delete Member

Show dropdown search for Skills / Hobbies works

Run API via Swagger (show endpoints)

Show Unit Tests: dotnet test => 3 passed

Explain Clean Architecture flow (UI -> API -> Core -> Infrastructure)


📷 Optional for Presentation

Include screenshots of working UI

ERD.png in repo root for DB visualization


💡 Technologies Used & Why

.NET 8: Modern, robust, future-proof

Clean Architecture: Maintainability, scalability

EF Core + MySQL (Pomelo): Relational DB with .NET-first migrations

Razor Pages: Lightweight UI for CRUD

AutoMapper: Clean DTO / Entity separation

xUnit: Proven testing framework

MySQL Workbench: Visualize schema & verify data


🔍 How to Search / Query

API endpoint GET /members?keyword=foo (Wildcard search on Username / Email)

UI Search box performs LIKE %keyword%


📄 Additional Notes

Database is local. For cloud deployment (Render / Heroku), you would need a cloud DB.

This README assumes running locally for demo purposes.