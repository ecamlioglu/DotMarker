# DotMarker CMS API

DotMarker is a Content Management System (CMS) API for managing users and contents. The project is built using .NET 9.0 and follows a layered architecture.

## Project Structure

- `DotMarker.API`: Contains controllers for managing contents and users.
- `DotMarker.Application`: Contains DTOs, services, and validations.
- `DotMarker.Domain`: Contains the domain entities.
- `DotMarker.Infrastructure`: Contains caching and data context implementations.
- `DotMarker.Repositories`: Contains repository interfaces and implementations.

## Features

- Manage users and contents
- In-memory database for data storage
- Swagger for API documentation

## Prerequisites

- .NET 9.0 SDK

## Setup Instructions

1. Clone the repository:
   ```sh
   git clone https://github.com/ecamlioglu/DotMarker.git
   cd DotMarker
   ```

2. Build the solution:
   ```sh
   dotnet build
   ```

3. Run the API project:
   ```sh
   cd DotMarker.API
   dotnet run
   ```

4. Open your browser and navigate to `http://localhost:5098/swagger` to view the Swagger API documentation.

## Usage Examples

### User Management

- **Create User**
  ```http
  POST /api/users
  Content-Type: application/json

  {
    "fullName": "John Doe",
    "email": "john.doe@example.com"
  }
  ```

- **Get User by ID**
  ```http
  GET /api/users/{userId}
  ```

- **Update User**
  ```http
  PUT /api/users/{userId}
  Content-Type: application/json

  {
    "fullName": "John Doe",
    "email": "john.doe@example.com"
  }
  ```

- **Get User Contents**
  ```http
  GET /api/users/{userId}/contents
  ```

- **Add Content to User**
  ```http
  POST /api/users/{userId}/contents/add
  Content-Type: application/json

  {
    "title": "Sample Content",
    "description": "This is a sample content.",
    "category": "Sample Category",
    "language": "en"
  }
  ```

- **Remove Content from User**
  ```http
  DELETE /api/users/{userId}/contents/remove/{contentId}
  ```

- **Remove All Contents from User**
  ```http
  DELETE /api/users/{userId}/contents/removeAll
  ```

### Content Management

- **Get Contents by Category**
  ```http
  GET /api/contents/{category}
  ```

- **Get Content Variant**
  ```http
  GET /api/contents/{contentId}/variants/{variantId}
  ```

- **Add Variant to Content**
  ```http
  POST /api/contents/{contentId}/variants/add
  Content-Type: application/json

  {
    "variantName": "Sample Variant"
  }
  ```

- **Remove Variant from Content**
  ```http
  DELETE /api/contents/{contentId}/variants/{variantId}
  ```

- **Remove All Variants from Content**
  ```http
  DELETE /api/contents/{contentId}/variants/removeAll
  ```

## Separation of Concerns

The project follows the principle of separation of concerns to ensure that each layer has a clear responsibility and does not mix concerns. For example, the `DotMarker.API/Controllers` (`DotMarker.API/Controllers/ContentController.cs`, `DotMarker.API/Controllers/UserController.cs`) only handle HTTP requests and responses, while the `DotMarker.Application/Services` (`DotMarker.Application/Services/ContentService.cs`, `DotMarker.Application/Services/UserService.cs`) contain the business logic. The exception handling logic has been moved from `DotMarker.API/Middlewares/ExceptionMiddleware.cs` to a separate middleware class `ExceptionHandlingMiddleware` to keep the controllers clean and focused on their primary responsibilities. The `DotMarker.Application/Interfaces` only contain interface definitions and not implementation details.

## Dependency Injection and Configuration

The project uses dependency injection to manage the dependencies between different layers. This is done in `DotMarker.API/Program.cs`, where all dependencies are registered and resolved through the DI container. The project also uses configuration files (`DotMarker.API/appsettings.json` and `DotMarker.API/appsettings.Development.json`) to manage application settings and environment-specific configurations. All configuration settings are centralized and easily manageable.

## Contributing

We welcome contributions to improve this project. To contribute, please follow these steps:

1. Fork the repository.
2. Create a new branch for your feature or bugfix.
3. Make your changes and commit them with clear and concise messages.
4. Push your changes to your forked repository.
5. Create a pull request to the main repository.

## License

This project is licensed under the MIT License.
