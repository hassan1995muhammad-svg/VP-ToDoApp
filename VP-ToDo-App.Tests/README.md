# VP-ToDo-App.Tests

Unit tests for the VP-ToDo-App backend using xUnit, FluentAssertions, and Moq.

## Test Coverage

### Domain Tests (5 tests)
- `TodoTaskTests` - Entity behavior and business logic

### Application Tests

#### Validators (12 tests)
- `CreateTodoTaskValidatorTests` - Task creation validation
- `UpdateTodoTaskValidatorTests` - Task update validation

#### Commands (4 tests)
- `CreateTaskCommandHandlerTests` - Task creation logic
- `DeleteTaskCommandHandlerTests` - Task deletion logic

#### Queries (2 tests)
- `GetAllTasksQueryHandlerTests` - Retrieve all tasks

### Infrastructure Tests (7 tests)
- `TodoTaskRepositoryTests` - Repository pattern implementation with InMemory database

## Running Tests

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run specific test
dotnet test --filter "FullyQualifiedName~TodoTaskTests"

# Run tests with coverage (requires coverlet)
dotnet test /p:CollectCoverage=true
```

## Dependencies

- **xUnit** - Testing framework
- **FluentAssertions** - Fluent assertion library
- **Moq** - Mocking framework
- **Microsoft.EntityFrameworkCore.InMemory** - In-memory database for testing
