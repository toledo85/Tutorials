# [Dependency Inversion Principle (DIP)](https://en.wikipedia.org/wiki/Dependency_inversion_principle)

After all, it's important to note that this principle is distinct from [dependency injection](https://en.wikipedia.org/wiki/Dependency_injection); they are not the same thing. Both concepts are used in this example.

In this example, `NumbersService` directly depends on `InMemoryRepository`, violating the [Dependency Inversion Principle (DIP)](https://en.wikipedia.org/wiki/Dependency_inversion_principle). According to [DIP](https://en.wikipedia.org/wiki/Dependency_inversion_principle), "high-level modules should not depend on low-level modules; both should depend on abstractions," and "abstractions should not depend on details; details should depend on abstractions." This design leads to tight coupling, making it difficult to modify the repository implementation without also altering the service.

Additionally, `NumbersService` has a hidden dependency on `InMemoryRepository`, which makes the problem worse.

```csharp
NumbersService service = new();

foreach (var number in service.GetAllNumbers()) {
    Console.WriteLine(number);
}

public class InMemoryRepository {
    public IEnumerable<int> Numbers { get; } = Enumerable.Range(1, 100);
}

public class NumbersService {
    private readonly InMemoryRepository _repository = new();

    public IEnumerable<int> GetAllNumbers() {
        return _repository.Numbers;
    }
}
```

First, the dependency problem will be fixed by applying [dependency injection](https://en.wikipedia.org/wiki/Dependency_injection). In the updated example, an instance of `InMemoryRepository` is passed to `NumbersService` through its constructor.

```csharp
InMemoryRepository repositoru = new InMemoryRepository();

NumbersService service = new(repositoru);

public class NumbersService(InMemoryRepository repository) {
    public IEnumerable<int> GetAllNumbers() {
        return repository.Numbers;
    }
}
```

The current solution doesn’t fully adhere to the [Dependency Inversion Principle (DIP)](https://en.wikipedia.org/wiki/Dependency_inversion_principle) because `NumbersService` still relies on the specific `InMemoryRepository` implementation.

To fully comply with this principle, an abstraction (`IRepository`) is introduced that both `NumbersService` and `InMemoryRepository` depend on. This allows `NumbersService` to depend on `IRepository` rather than the concrete `InMemoryRepository`, making it easy to swap out `InMemoryRepository` with another implementation without changing `NumbersService`. This approach enhances code flexibility and maintainability, resulting in a decoupled architecture where high-level modules are less affected by changes in low-level modules.

```csharp
InMemoryRepository repositoru = new InMemoryRepository();

NumbersService service = new(repositoru);

foreach (var number in service.GetAllNumbers()) {
    Console.WriteLine(number);
}

public interface IRepositoru {
    public IEnumerable<int> Numbers { get; }
}

public class InMemoryRepository : IRepositoru {
    public IEnumerable<int> Numbers { get; } = Enumerable.Range(1, 100);
}

public class NumbersService(IRepositoru repository) {
    public IEnumerable<int> GetAllNumbers() {
        return repository.Numbers;
    }
}
```

## "Real-world" example (bonus) 🎉

Here’s a concise example illustrating the use of both [dependency injection](https://en.wikipedia.org/wiki/Dependency_injection) and the [Dependency Inversion Principle (DIP)](https://en.wikipedia.org/wiki/Dependency_inversion_principle) in an [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) application. In this example, `DataContext` serves as the database context for [CRUD](https://en.wikipedia.org/wiki/Create,_read,_update_and_delete) operations, `IRepository` defines the abstraction, and `Repository` provides the concrete implementation that interacts with `DataContext`. The dependencies will be injected using [.NET's dependency injection system](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection).

```csharp
builder.Services.AddDbContext<DataContext>();

builder.Services.AddScoped<IRepository, Repository>();

// ...

app.MapGet("/api/books", (IRepository repository) => {
    return repository.GetAllBooks();
});

// ...

public class DataContext : DbContext {
    // ...
}

interface IRepository {
    // ...
}

public class Repository(DataContext context) : IRepository {
    // ...
}
```