# Bookstore_games
## Backend Overview
### Technologies Used
.NET Core 7+: The core framework for building the backend services.
Entity Framework Core: Used for database access and ORM.
PostgreSQL: The relational database used for storing application data.
Ardalis.Specification: Implements the Specification pattern for clean and reusable query logic.
MediatR: Facilitates the CQRS pattern and handles application requests via handlers.
FluentValidation: Provides a fluent interface for validating incoming requests.
Serilog: Logging framework for capturing application logs.
## Architecture
There is two project: BG.LocalWeb and BG.LocalApi. BG.LocalWeb is responsible for user management. BG.LocalApi is responsible for authors/books management.
The backend follows a layered architecture, which consists of the following layers:

### Domain Layer: Contains the core business logic, including entities and domain services. Specifications for filtering and querying data are also placed here.

### Application Layer: This layer contains the application’s use cases, including MediatR request handlers, queries, and commands. It orchestrates the business logic by interacting with the domain and infrastructure layers.

### Infrastructure Layer: Provides implementations for data access and external services. This includes repositories, database context configurations, and third-party service integrations.

### API Layer: Exposes the application’s functionalities via RESTful endpoints using ASP.NET Core. It includes controllers, middleware, and Swagger documentation.

## Setting Up the Backend
### Prerequisites
Before you start, ensure you have the following installed on your machine:

.NET Core SDK 7+
PostgreSQL
A running instance of your PostgreSQL database with the necessary connection details.
Configuration
Database Configuration:

Open appsettings.json and provide your PostgreSQL connection strings
Configure your JWT settings in appsettings.json
Apply the latest migration for each database


# Frontend
## Overview
The frontend of this project is built using Angular and communicates with a .NET Core backend via RESTful APIs. The application provides a user interface for managing books and authors, with functionalities such as adding, editing, deleting, and filtering books, as well as handling user authentication with JWT tokens.

## Structure
src/app: Contains the core application components and services.
src/assets: Static assets like images and styles.
src/environments: Environment-specific configurations.
src/styles.scss: Global styles for the application.

## Authentication
The application uses JWT (JSON Web Token) for authentication.

