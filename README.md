# ReXLPG
This project, named ReXLPG (likely "Retinue XL PG" or similar, with "PG" referring to Paying Guest accommodation), is a full-stack .NET 8.0 web application designed for managing a Paying Guest (PG) facility. It handles user registration, profile management, enquiry processing, and document uploads in a secure, authenticated environment.


Key Components:
ReXLPG (API Layer): An ASP.NET Core Web API that provides RESTful endpoints for user authentication (JWT-based), user management, and enquiry operations. It includes Swagger documentation, CORS support for the UI, and file upload capabilities (up to 150 MB per request). The API uses Entity Framework Core with SQL Server for data persistence.
ReXLPgWebUI: An ASP.NET Core MVC web application that serves as the user interface, consuming the API via JWT authentication stored in cookies. It provides views for user interactions, likely including forms for registration, enquiries, and admin panels.
ReXLPgDA (Data Access Layer): Contains the Entity Framework DbContext, repositories, and data access classes for users, enquiries, and related entities.
ReXLPgDM (Data Models): Defines the core domain models, including:
User: Stores detailed user information (name, contact, Aadhar number, emergency contacts, purpose of stay, workplace details, etc.).
Enquiry: Manages accommodation enquiries with statuses (Open, Canceled, Converted) and associated comments/notes.
UserDocument: Handles binary data for user images and ID documents (Aadhar, work ID).
ReXLPgDAS (Data Access Services): Provides service layer abstractions for business logic, validation, and utilities.
ReXLUtil: Utility classes for extensions, image handling, and exception management.
Technologies and Features:
Backend: ASP.NET Core 8.0, Entity Framework Core, SQL Server, JWT authentication, Swagger/OpenAPI.
Frontend: ASP.NET Core MVC with Razor views, runtime compilation for development.
Security: JWT-based auth with cookie support in the UI, CORS configuration, request size limits.
Database: SQL Server with migrations for schema evolution (includes tables for users, enquiries, documents, and comments).
Additional Features: File uploads for images/documents, enquiry status tracking, user role management, and integration hooks (e.g., WhatsApp mentioned in config).
The application appears to be tailored for Indian PG operations, incorporating local requirements like Aadhar verification and emergency contact details. It's structured for scalability with layered architecture (API, DA, DM, Services, UI).
