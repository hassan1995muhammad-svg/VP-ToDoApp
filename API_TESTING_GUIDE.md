# API Testing Guide - VP ToDo App

## Base URL
```
http://localhost:5059/api/tasks
```

---

## 1. Create Task (POST)

### Endpoint
```
POST /api/tasks
```

### Valid Request (Success)
```json
{
  "description": "Complete the backend implementation for the ToDo application",
  "deadline": "2025-02-15T18:00:00Z"
}
```

### Response (201 Created)
```json
{
  "id": 1,
  "description": "Complete the backend implementation for the ToDo application",
  "deadline": "2025-02-15T18:00:00Z",
  "status": 1,
  "createdAt": "2025-01-30T10:30:00Z",
  "updatedAt": null,
  "isOverdue": false
}
```

### Invalid Request (Validation Error)
```json
{
  "description": "Short",
  "deadline": "2024-01-01T18:00:00Z"
}
```

### Response (400 Bad Request)
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

---

## 2. Get All Tasks (GET)

### Endpoint
```
GET /api/tasks
```

### Response (200 OK)
```json
[
  {
    "id": 1,
    "description": "Complete the backend implementation",
    "deadline": "2025-02-15T18:00:00Z",
    "status": 1,
    "createdAt": "2025-01-30T10:30:00Z",
    "updatedAt": null,
    "isOverdue": false
  },
  {
    "id": 2,
    "description": "Write comprehensive documentation",
    "deadline": "2025-02-10T18:00:00Z",
    "status": 2,
    "createdAt": "2025-01-29T14:20:00Z",
    "updatedAt": "2025-01-30T09:15:00Z",
    "isOverdue": false
  }
]
```

---

## 3. Get Task by ID (GET)

### Endpoint
```
GET /api/tasks/{id}
```

### Example
```
GET /api/tasks/1
```

### Response (200 OK)
```json
{
  "id": 1,
  "description": "Complete the backend implementation",
  "deadline": "2025-02-15T18:00:00Z",
  "status": 1,
  "createdAt": "2025-01-30T10:30:00Z",
  "updatedAt": null,
  "isOverdue": false
}
```

### Response (404 Not Found)
```json
{
  "message": "Task with ID 99 not found."
}
```

---

## 4. Get Tasks by Status (GET)

### Endpoint
```
GET /api/tasks/status/{status}
```

### Status Values
- `1` = Active
- `2` = Completed

### Example - Get Active Tasks
```
GET /api/tasks/status/1
```

### Example - Get Completed Tasks
```
GET /api/tasks/status/2
```

### Response (200 OK)
```json
[
  {
    "id": 1,
    "description": "Complete the backend implementation",
    "deadline": "2025-02-15T18:00:00Z",
    "status": 1,
    "createdAt": "2025-01-30T10:30:00Z",
    "updatedAt": null,
    "isOverdue": false
  }
]
```

---

## 5. Search Tasks (GET)

### Endpoint
```
GET /api/tasks/search?query={searchTerm}
```

### Example
```
GET /api/tasks/search?query=backend
```

### Response (200 OK)
```json
[
  {
    "id": 1,
    "description": "Complete the backend implementation",
    "deadline": "2025-02-15T18:00:00Z",
    "status": 1,
    "createdAt": "2025-01-30T10:30:00Z",
    "updatedAt": null,
    "isOverdue": false
  }
]
```

---

## 6. Update Task (PUT)

### Endpoint
```
PUT /api/tasks/{id}
```

### Request (Partial Update)
```json
{
  "description": "Complete the backend implementation with unit tests",
  "deadline": "2025-02-20T18:00:00Z",
  "status": 2
}
```

### Note
All fields are optional. Only provided fields will be updated.

### Response (200 OK)
```json
{
  "id": 1,
  "description": "Complete the backend implementation with unit tests",
  "deadline": "2025-02-20T18:00:00Z",
  "status": 2,
  "createdAt": "2025-01-30T10:30:00Z",
  "updatedAt": "2025-01-30T15:45:00Z",
  "isOverdue": false
}
```

### Response (404 Not Found)
```json
{
  "success": false,
  "message": "Task with ID 99 was not found."
}
```

---

## 7. Mark Task as Completed (PATCH)

### Endpoint
```
PATCH /api/tasks/{id}/complete
```

### Example
```
PATCH /api/tasks/1/complete
```

### Response (200 OK)
```json
{
  "message": "Task marked as completed successfully."
}
```

### Response (404 Not Found)
```json
{
  "success": false,
  "message": "Task with ID 99 was not found."
}
```

---

## 8. Delete Task (DELETE)

### Endpoint
```
DELETE /api/tasks/{id}
```

### Example
```
DELETE /api/tasks/1
```

### Response (200 OK)
```json
{
  "message": "Task deleted successfully."
}
```

### Response (404 Not Found)
```json
{
  "success": false,
  "message": "Task with ID 99 was not found."
}
```

---

## Testing with cURL

### Create Task
```bash
curl -X POST "http://localhost:5059/api/tasks" \
  -H "Content-Type: application/json" \
  -d '{
    "description": "Test task with more than 10 characters",
    "deadline": "2025-02-15T18:00:00Z"
  }'
```

### Get All Tasks
```bash
curl -X GET "http://localhost:5059/api/tasks"
```

### Get Active Tasks
```bash
curl -X GET "http://localhost:5059/api/tasks/status/1"
```

### Search Tasks
```bash
curl -X GET "http://localhost:5059/api/tasks/search?query=backend"
```

### Update Task
```bash
curl -X PUT "http://localhost:5059/api/tasks/1" \
  -H "Content-Type: application/json" \
  -d '{
    "description": "Updated task description with more details",
    "status": 2
  }'
```

### Mark as Completed
```bash
curl -X PATCH "http://localhost:5059/api/tasks/1/complete"
```

### Delete Task
```bash
curl -X DELETE "http://localhost:5059/api/tasks/1"
```

---

## Testing with PowerShell

### Create Task
```powershell
$body = @{
    description = "Test task with more than 10 characters"
    deadline = "2025-02-15T18:00:00Z"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5059/api/tasks" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

### Get All Tasks
```powershell
Invoke-RestMethod -Uri "http://localhost:5059/api/tasks" -Method Get
```

### Delete Task
```powershell
Invoke-RestMethod -Uri "http://localhost:5059/api/tasks/1" -Method Delete
```

---

## Task Status Enum

| Value | Status    | Description                          |
|-------|-----------|--------------------------------------|
| 1     | Active    | Task is active and not yet completed |
| 2     | Completed | Task has been marked as completed    |

---

## Overdue Task Detection

A task is marked as **overdue** when:
- `Deadline < Current UTC Time`
- `Status == Active (1)`

The `isOverdue` property in the response will be `true` for overdue tasks.

---

## Validation Rules Summary

### Description
- **Required**: Yes
- **Min Length**: 10 characters
- **Max Length**: 500 characters

### Deadline
- **Required**: No
- **Validation**: Cannot be in the past (if provided)

### Status
- **Values**: 1 (Active), 2 (Completed)
- **Default**: 1 (Active) for new tasks

---

## Error Response Format

All error responses follow this structure:

```json
{
  "success": false,
  "message": "Error message",
  "errors": [
    "Detailed error 1",
    "Detailed error 2"
  ]
}
```

---

## HTTP Status Codes

| Code | Description              | When Used                          |
|------|--------------------------|------------------------------------|
| 200  | OK                       | Successful GET, PUT, PATCH, DELETE |
| 201  | Created                  | Successful POST                    |
| 400  | Bad Request              | Validation errors                  |
| 404  | Not Found                | Resource not found                 |
| 500  | Internal Server Error    | Unexpected server error            |
