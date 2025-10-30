# VP-ToDoApp
Verbund Pflegehilfe - ToDo App

# VP ToDo App - Backend API

ToDo application backend built with .NET 8, following **Clean Architecture** principles with **CQRS pattern**, **MediatR**, **Repository Pattern**, **In Memory**, and comprehensive logging using **Serilog**.

### **Clean Architecture Layers**

API Layer (Controllers)        
Application Layer (CQRS/MediatR)   
Domain Layer (Entities)     
Infrastructure Layer (Repos)  

##  **Quick Start**

Choose your preferred deployment method:

### **Docker (Recommended)** 

Both frontend and backend services can be started without docker simply by using dotetnet CLI or Visual Studio.

# Navigate to the backend project directory
cd VP-ToDo-App

# Run the backend
dotnet run

# Navigate to the backend project directory
cd VP-ToDo-App

# Run the backend
dotnet run

# Navigate to the frontend directory
cd frontend

# Install dependencies (first time only)
npm install

# Start the development server
npm start

# If you have docker desktop installed, you can run the entire application (API + Frontend) using Docker Compose.

```bash
# Start API + Frontend Server in containers

    docker-compose docker-compose.fullstack.yml up

```

##  **Documentation**

| Document | Description |
|----------|-------------|
| [API_TESTING_GUIDE.md] | API endpoints with examples |


## Features

### **Task Management**
-  Create tasks (minimum 10 characters description)
-  Update tasks (description, deadline, status)
-  Delete tasks
-  Mark tasks as completed
-  View all tasks
-  Filter tasks by status (Active/Completed)
-  Search tasks by description
-  Automatic overdue detection (deadline < current time)

### **Validation Rules**
- Task description: 10-500 characters (required)
- Deadline: Cannot be in the past
- Status: Active or Completed
- Error messages displayed for validation failures

## Database Schema

### **Tasks Table**
| Column      | Type          | Constraints                    |
|-------------|---------------|--------------------------------|
| Id          | INT           | PRIMARY KEY, IDENTITY(1,1)     |
| Description | NVARCHAR(500) | NOT NULL                       |
| Deadline    | DATETIME2     | NULL                           |
| Status      | INT           | NOT NULL, DEFAULT 1 (Active)   |
| CreatedAt   | DATETIME2     | NOT NULL, DEFAULT GETUTCDATE() |
| UpdatedAt   | DATETIME2     | NULL                           |

**Indexes:**
- `IX_Tasks_Status` on Status column
- `IX_Tasks_Deadline` on Deadline column

## Local Setup Instructions

### **Prerequisites**
- .NET 8.0 SDK
- Visual Studio 2022 / VS Code / Rider

###  **Access Swagger Documentation**

The API will start at: `http://localhost:5059`
Navigate to: `http://localhost:5059/swagger`

## API Endpoints

### **Tasks Controller** (`/api/tasks`)

| Method | Endpoint               | Description                 |
|--------|------------------------|-----------------------------|
| GET    | `/api/tasks`           | Get all tasks               |
| GET    | `/api/tasks/{id}`      | Get task by ID              |
| GET    | `/api/tasks/status/{status}` | Get tasks by status (1=Active, 2=Completed) |
| GET    | `/api/tasks/search?query={query}` | Search tasks by description |
| POST   | `/api/tasks`           | Create a new task           |
| PUT    | `/api/tasks/{id}`      | Update an existing task     |
| PATCH  | `/api/tasks/{id}/complete` | Mark task as completed  |
| DELETE | `/api/tasks/{id}`      | Delete a task               |

**Full API documentation:** [API_TESTING_GUIDE.md](API_TESTING_GUIDE.md)

## Logging

Logs are written to:
- **Console**: All log levels
- **File**: `logs/todo-app-YYYYMMDD.log` (rolling daily)

## Error Handling

The application includes global exception handling middleware that returns structured error responses:

## **Validation Error Response**
```json
{
  "success": false,
  "message": "Validation failed",
  "errors": [
    "Task description must be at least 10 characters long.",
    "Deadline cannot be in the past."
  ]
}
```

##  Author

**Muhammad Hassan**
- GitHub: [@hassan1995muhammad-svg](https://github.com/hassan1995muhammad-svg)

##  License

This project is open-source
