# Rental Management System

ASP.NET Core 8 Blazor Server application for managing student rental agreements, including property listings, renter assignment, and rental detail tracking. Built with a 3-layer architecture (DAL/BLL/UI) using Entity Framework Core against a SQL Server database.

## Features

- Search rental properties by city and/or community
- View and edit rental details (type, monthly rent, damage deposit, vacancy, available date)
- Manage renters per rental — add students and remove existing renters
- Search students by last name and add them as renters
- Address management per rental property

## Project Structure

- **RentalManagementSystem** — Class library containing the data access layer (EF Core), business logic services, entities, and view models
- **RentalManagementWebApp** — Blazor Server web application providing the UI

## Requirements

- .NET 8 SDK
- SQL Server with the `StarTed-2024-Oct` database (`.bacpac` file included)

## Getting Started

1. Restore the database using `StarTed-2024-Oct.bacpac` into a local SQL Server instance
2. Update the connection string in `RentalManagementWebApp/appsettings.json` if needed
3. Run the `RentalManagementWebApp` project
