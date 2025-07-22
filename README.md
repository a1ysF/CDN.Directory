CDN.Directory — Freelancer Directory Management System

📌 Project Overview
CDN.Directory is a .NET 8 Web Application that allows users to manage a directory of Freelancers (Members).
Each Member can be linked to multiple pre-registered Skillsets and Hobbies.
The system provides a clean MVC-based UI for CRUD operations and a separate API with Swagger for RESTful interactions.

📂 Solution Structure (Clean Architecture)
CDN.Directory.Core
├── Entities / DTOs

CDN.Directory.Infrastructure
├── EF Core / DbContext / Migrations

CDN.Directory.API
├── REST API (Swagger for testing)
├── MemberController (CRUD, Archive, Search)

CDN.Directory.UI
├── Razor Views (CRUD UI for Members)
├── MemberScaffoldController

CDN.Directory.Tests
├── xUnit Tests (Full CRUD, Archive, Search covered)

CDN.Directory.Seeder
├── Seeder for Skillsets & Hobbies Master Data


🔧 Technologies Used
.NET 8

MySQL (cdn_directory_db)

Entity Framework Core (Pomelo MySQL Provider)

AutoMapper

xUnit (Unit Testing)

Bootstrap 5 (UI Styling)

Select2 (Enhanced Dropdown UX)

Swagger (API Documentation)


⚙️ Setup Instructions
1️⃣ Database Setup
Create your MySQL database:

CREATE DATABASE cdn_directory_db;

Connection String (appsettings.json):
"DefaultConnection": "server=localhost;port=3306;database=cdn_directory_db;user=root;password=yourpassword;"

2️⃣ Run EF Core Migrations
Navigate to Infrastructure project:

dotnet ef database update

3️⃣ Seed Master Data (Skillsets / Hobbies)
Option A: Run CDN.Directory.Seeder project.
Option B: Manually run provided SQL insert statements.


🚀 Running the Application
UI (CRUD Razor Pages)
Run: CDN.Directory.UI
Opens at: /MemberScaffold
Features:

Create / Edit / Delete Members

Search by Username / Email

Select from pre-registered Skillsets / Hobbies

API (Optional)
Run: CDN.Directory.API
Swagger available at: /swagger

API Endpoints (Examples):
GET /api/members
POST /api/members
PUT /api/members/{id}
PATCH /api/members/{id}/archive
PATCH /api/members/{id}/unarchive
GET /api/members/search?keyword=


✅ Testing
Navigate to CDN.Directory.Tests:
dotnet test

Tests Cover:

Create Member

Read (List / Single)

Update Member

Delete Member

Archive / Unarchive

Search by Username / Email

NotFound Scenarios


📊 Database Structure (ERD)
Members (Id, Username, Email, PhoneNumber, IsArchived)
SkillsetMaster (Id, Name)
HobbyMaster (Id, Name)
MemberSkillsets (MemberId, SkillsetId)
MemberHobbies (MemberId, HobbyId)

Relationships:

Members → Skillsets via MemberSkillsets

Members → Hobbies via MemberHobbies


🔄 Flow Diagram
UI (Razor Views)
→ MemberScaffoldController (CDN.Directory.UI)
→ EF Core (Infrastructure)
→ MySQL (cdn_directory_db)

API Consumers (Optional)
→ MemberController (CDN.Directory.API)
→ EF Core (Infrastructure)
→ MySQL (cdn_directory_db)

📝 Notes for Assessment / Interview
Follows Clean Architecture

Uses Dependency Injection & AutoMapper

Tests validate API behavior thoroughly

UI provides clean, user-friendly CRUD experience

Separate API layer via Swagger for flexibility

🔗 Optional: GitHub Actions (if implemented)
dotnet restore

dotnet build

dotnet test

Publish artifacts (if applicable)