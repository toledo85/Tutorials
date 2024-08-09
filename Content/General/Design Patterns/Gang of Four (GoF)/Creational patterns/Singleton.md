# [Singleton design pattern](https://en.wikipedia.org/wiki/Singleton_pattern)

The [Singleton design pattern](https://en.wikipedia.org/wiki/Singleton_pattern) ensures only one instance of a type exists in your application, which is particularly useful for components that require costly initialization, such as data repositories or object factories. 

## [Static classes](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members) vs. [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) vs [global variables](https://en.wikipedia.org/wiki/Global_variable) 

[Static classes](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members) and [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) are similar in  that neither can be instantiated with the [`new` operator](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/new-operator), but they are not the same. [Static classes](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members) can't be instantiated at all, while singletons allow only one instance. I won’t go into detail here, but I thought it was useful to mention this topic, even if it's not completely related. [Static classes](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members) usually don't store state and are commonly used for utility methods and factories.

```cs
// Factory instance = new(); // ❌ Compile error

Factory.BuildNothing();

static class Factory {
    public static void BuildNothing() {
        Console.WriteLine("Building something super cool");
    }
}
```

On the other hand, a [singleton](https://en.wikipedia.org/wiki/Singleton_pattern) is a design pattern that ensures there is only one instance throughout the application's lifetime, providing global access to it. The [`class`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/class) has a [`private`](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers) [`constructor`](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/constructors) to prevent instantiation outside the class, and a [`public`](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers) [`static`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static) property that holds the single instance and provides global access to it. Singletons are useful for tasks like configuration or logging but should be used carefully to avoid issues with testing and code flexibility.

```cs 
// Singleton instance = new(); // ❌ Compile error

var instance = Singleton.Shared;

Singleton.Shared.DoSomething();

instance.DoSomething();

sealed class Singleton {
    public static readonly Singleton Shared = new();
    
    private Singleton() {
        Console.WriteLine("Instanciating singleton");
    }

    public static void DoSomething() {
        Console.WriteLine("Hello, World!");
    }
}
```

[Singletons](https://en.wikipedia.org/wiki/Singleton_pattern) persist through the application's lifecycle, similar to [global variables](https://en.wikipedia.org/wiki/Global_variable), but with the added advantage of having getters and setters.

## Lazy instantiation

We use a [`private`](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers) [`static`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static) field to store the instance, and a [`public`](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers) [`static`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static) property (or method) to provide global access to it. The property checks if the instance is [`null`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null) before creating it, and only creates it once. If it is, a new instance is created and assigned to the field. 
In either case, the property returns the instance. This approach ensures that the instance is created only once and only when needed, guaranteeing lazy initialization.

```csharp
public sealed class Singleton {
    private static Singleton? _shared;
    
    public static Singleton Shared  {
        get {
            if (_shared is null) {
                _shared = new Singleton();
            }
            
            return _shared;
        }
    }

    private Singleton() {}
}
```

## Thread safety 

Concurrent processes can access the  [singleton](https://en.wikipedia.org/wiki/Singleton_pattern) instance property simultaneously and we cannot predict when the [JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process) compiler will initialize the property. Multiple threads accessing it could lead to a race condition,  potentially resulting in multiple instances being created. For this reason, we require locking.

```cs 
ParallelEnumerable.Range(0, 1000).ForAll(_ => {
    var instance = Singleton.Shared; 
});

ParallelEnumerable.Range(0, 1000).ForAll(_ => {
    var instance = Singleton.Shared; 
});

public sealed class Singleton {
    private static Singleton? _shared;
    private static Lock _lock = new();
    
    public static Singleton Shared  {
        get {
            lock (_lock) {
                if (_shared is null) {
                    _shared = new Singleton();
                }
            }
            
            return _shared;
        }
    }

    private Singleton() {}
}
```

## [Double-check locking](https://en.wikipedia.org/wiki/Double-checked_locking) pattern

Locking requires synchronization, which can be expensive and increase the application's latency. It is only needed during the initial access to the [singleton](https://en.wikipedia.org/wiki/Singleton_pattern). Subsequent accesses do not require locking. To address this, we use the [double-check locking pattern](https://en.wikipedia.org/wiki/Double-checked_locking) pattern. The race condition occurs only the first time, and the lock ensures that only the first thread creates the instance.

```cs 
public sealed class Singleton {
    private static Singleton? _shared;
    private static Lock _lock = new();
    
    public static Singleton Shared  {
        get {
            if (_shared is null) {
                lock (_lock) {
                    if (_shared is null) {
                        _shared = new Singleton();
                    }
                }
            }
            
            return _shared;
        }
    }

    private Singleton() {}
}
```

## [`Lazy<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.lazy-1?view=net-8.0)

All the heavy lifting we’ve done so far is handled behind the scenes by the [`Lazy<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.lazy-1?view=net-8.0) class. This class simplifies the implementation of lazy initialization by ensuring that the instance is created only when it is first accessed, thus improving performance and resource management. 
It also handles thread safety automatically, so you don’t need to manage synchronization yourself. Using this approach results in a more concise and elegant implementation, 
reducing boilerplate code and minimizing the risk of errors in managing the singleton instance.

```csharp
Singleton.Shared.AppState.ForEach(Console.WriteLine);

public sealed class Singleton {
    public List<string> AppState = ["Arya", "Sansa", "Robb", "Bran"];
    
    private static readonly Lazy<Singleton> LazyInstance = 
        new Lazy<Singleton>(() => {
            return new();
        });

    public static Singleton Shared => LazyInstance.Value;
    
    private Singleton() { }
}
```

## Cool implementation

When any [`static`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static) property or field of the `Singleton` class is accessed, 
all other static members of the class are also initialized. For example, if the `Message`, 
it triggers the initialization the instancre, even if it might not be necessary. 

Here's the improved message:

When any [`static`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static) property or field of the `Singleton` class is accessed, 
all [`static`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static)  members of the class are initialized. For example, accessing the `Message` property triggers the initialization of the `Singleton` instance, even if it might not be necessary.

```csharp
public sealed class Singleton {
    public static string Message => "Hello, World!";
    public static Singleton Shared = new();
    
    private Singleton() {
        Console.WriteLine($"Creating {nameof(Singleton)}");
    }
    
    static Singleton() {
        
    }
}
```

We achieve lazy initialization by using a nested class called `Nested`, which holds the `Singleton` instance. The [singleton](https://en.wikipedia.org/wiki/Singleton_pattern) instance is created only when the `Nested` class is first accessed, 
ensuring it is initialized lazily. If a [`static`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static) constructor is defined for the `Singleton` class, 
it will execute when any [`static`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static) member is accessed, potentially initializing the `Singleton` instance via the nested class’s shared property.

```csharp
sealed class Singleton {
    public static string ClassName = string.Empty;

    public static Singleton Shared => Nested.Shared;
    
    private Singleton() {
        Console.WriteLine($"Creating {nameof(Singleton)}");
    }
    
    private static class Nested {
        internal static Singleton Shared { get; } = new();
        
        static Nested() {
            
        }
    }
}
```

## [Singleton pattern](https://en.wikipedia.org/wiki/Singleton_pattern) vs singleton behavior ([dependency injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection))

First we add this [package](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/) to our project:

```shell
dotnet add package Microsoft.Extensions.DependencyInjection
```

Then add this code to our project:

```csharp
IServiceCollection services = new ServiceCollection();

services.AddSingleton<Logger>();

// Actual dependency injecion IoC container
var serviceProvider = services.BuildServiceProvider();

var logger1 = serviceProvider.GetRequiredService<Logger>();
var logger2 = serviceProvider.GetRequiredService<Logger>();
var logger3 = serviceProvider.GetRequiredService<Logger>();

logger1.Log("Hello, World!");

class Logger {
    public void Log(string message) {
        Console.WriteLine(message);
    }
}
```

Here we are utilizing the [.NET](https://learn.microsoft.com/en-us/dotnet/)dependency injection system, none of what we have here implements the singleton pattern. The `Logger` class exhibits singleton-like behavior, 
but it is not a [singleton](https://en.wikipedia.org/wiki/Singleton_pattern) itself.

## [Monostate design pattern](https://wiki.c2.com/?MonostatePattern)

The [Monostate design pattern](https://wiki.c2.com/?MonostatePattern) is a variation of the [singleton pattern](https://en.wikipedia.org/wiki/Singleton_pattern) where multiple instances of a class can exist, but they all share the same internal state. In this pattern, each instance of the class behaves as if it were a [singleton](https://en.wikipedia.org/wiki/Singleton_pattern), with all instances sharing the same data. This allows for flexibility in terms of object creation while ensuring that all instances maintain consistency in their state. It is very bad idea, everybody can change the same state.

```cs 
// Multiple instances
CompanyCEO instance1 = new();
CompanyCEO instance2 = new();
CompanyCEO instance3 = new();

instance1.Name = "John Doe";

Console.WriteLine(instance1.Name);
Console.WriteLine(instance2.Name);
Console.WriteLine(instance3.Name);

sealed class CompanyCEO {
    private static string _name = "";
    private static int _age = 0;
    
    public string Name {
        get => _name;
        set => _name = value;
    }
    
    public int Age {
        get => _age;
        set => _age = value;
    }
}
```

## ✅ Benefits

1. Resource management (especially true for resource-heavy operations).
2. Controlled access to the single instance.
3. Consistent state.

## ❌ Drawbacks

1. Hidden dependencies.
2. Make unit testing harder.
3. Tight coupling.
4. Single point of failure.

If the `Logger` class were a [singleton](https://en.wikipedia.org/wiki/Singleton_pattern) and the `Service` class depended on it, you would be accessing it like this:

```csharp
class Service {
    public void DoSomething() {
        // Hidden dependency
        Logger.Log("Doing something");
    }
}
```

Which means that the `Service` class has a hidden, 
depends on the `Logger` class. 
This is dificult to mock the `Logger` and `Service` classes
even if we are using with [dependency injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection).

The proper way to solve this is using dependency injection,
making the dependency explicit.
Now we can mock the `Logger` class and the `Service` class.

```csharp
// ILogger logger = new Logger();
ILogger fakeLogger = new FakeLogger();

Service service = new(fakeLogger);

interface ILogger {
    public void Log(string message);
}

class FakeLogger : ILogger {
    public void Log(string message) {
        // Empty, do nothing
    }
}

class Service(ILogger logger) : IService {
    private readonly Logger _logger = logger;
}   
```

## Exploring [singleton pattern](https://en.wikipedia.org/wiki/Singleton_pattern) in other languages (bonus) 🎉

Unlike some languages like [C#](https://learn.microsoft.com/en-us/dotnet/csharp/), [Swift](https://www.swift.org) does not have a direct equivalent of [static classes](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members), but you can achieve similar functionality using [enums](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/enumerations/):

```swift 
enum Factory {
    static func buildNothing() {
        print("Building something super cool")
    }
}
```

[Singleton](https://en.wikipedia.org/wiki/Singleton_pattern) on [Swift](https://www.swift.org) (lazy instantiation is guaranteed):

```swift 
final class Singleton {
    static let shared = Singleton()    

    private init() {}
}
```

[Kotlin](https://kotlinlang.org) does not have a direct equivalent of [static classes](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members) neither, lazy instantiation is guaranteed too:

```kotlin
object Singleton {
    // ...
}
```

```kotlin
class Singleton private constructor() {
    // ...

    companion object {
        val shared: Singleton by lazy {
            Singleton()
        }
    }
}
```

#### [JavaScript](https://developer.mozilla.org/en-US/docs/Web/JavaScript)/[TypeScript](https://www.typescriptlang.org)

```ts 
export default {
    // State and behavior...
}
```

```ts
class Singleton {
    private static _shared: Singleton

    static get shared(): Singleton {
        if (!this._shared) {
            this._shared = new this()
        }
        return this._shared
    }

    private constructor() {}
}
```

## "Unique" instance problem (bonus) 🎉

Remember that objects can be cloned, you cannot create multiple instances but you can replicate them.

```ts 
class Singleton {
    value = 0
    static shared = new Singleton()
    private constructor() {}
}

const instance1 = Singleton.shared

const instance2 = {
    ...instance1
}

instance1.value = 10

console.log(instance1 === instance2)
console.log(instance1.value)
console.log(instance2.value)
```

## Real-world examples (bonus) 🎉

The [Redux](https://redux.js.org) store in [React](https://react.dev) applications acts as a singleton, ensuring a singular store instance. This is valid for vanilla [JavaScript](https://developer.mozilla.org/en-US/docs/Web/JavaScript) too.

[Node.js](https://nodejs.org/en) module system caches modules, so repeated require calls for the same module don't recreate the module but return the cached version, acting like a singleton.

```ts
export default {}
```

Some classes in the [Apple](https://developer.apple.com) SDKs provide default instances, including [UIDevice](https://developer.apple.com/documentation/uikit/uidevice), [UIApplication](https://developer.apple.com/documentation/uikit/uiapplication), [UIScreen](https://developer.apple.com/documentation/uikit/uiscreen), [UserDefaults](https://developer.apple.com/documentation/foundation/userdefaults), and [FileManager](https://developer.apple.com/documentation/foundation/filemanager).

```swift 
let device = UIDevice.current
let application = UIApplication.shared
let screen =  UIScreen.main
let userDefaults = UserDefaults.standard
let fileManager = FileManager.default
let urlSession = URLSession.shared
```

> 💡 You can create instances with [`URLSession`](https://developer.apple.com/documentation/foundation/urlsession) for example.

You can also use [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) for create a configuration classes:

```swift 
final class SessionManager {
    static let kCachedUserAuthToken = "authToken"
    static let kCachedAccountId = "accountId"
    static let kCachedActivePatient = "activePatient"
    
    var authToken: String?
    var accountId: Int?
    var predictedFVC: Double?
    
    static let shared = SessionManager()
    
    private init() {}
}
```