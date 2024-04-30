# [Singleton design pattern](https://en.wikipedia.org/wiki/Singleton_pattern)

The [Singleton design pattern](https://en.wikipedia.org/wiki/Singleton_pattern) ensures that only one object of a given type exists in your application, effectively resolving the "single/shared resource problem". Some components are best represented by a single instance in the system, such as a base repository of data or an object factory. This pattern is particularly beneficial when the initializer call is expensive, as it ensures that the initialization process is performed only once.

[Singleton](https://en.wikipedia.org/wiki/Singleton_pattern) objects persist throughout the lifecycle of your application. They are akin to [global variables](https://en.wikipedia.org/wiki/Global_variable), but with distinct names. However, both [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) and [global variables](https://en.wikipedia.org/wiki/Global_variable) may encounter concurrency issues, hidden dependencies, and namespace pollution. Unlike [global variables](https://en.wikipedia.org/wiki/Global_variable), [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) have getters and setters.

[Singleton](https://en.wikipedia.org/wiki/Singleton_pattern) objects can be difficult to test and refactor. Instead of directly relying on a [singleton](https://en.wikipedia.org/wiki/Singleton_pattern), it's advisable to depend on an abstraction, such as a protocol or interface. Additionally, consider use the [Dependency injection pattern](https://en.wikipedia.org/wiki/Dependency_injection). 

While [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) offer convenience due to their single instance nature, they can lead to global mutation, making it risky for all parts of the application to change. Therefore, extra caution is needed, especially in concurrent environments.

There's nothing wrong with using [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) as long as you're aware of the potential issues they can bring and ensure they're implemented carefully and appropriately to meet the specific requirements of the application.

## Difference between static classes and a [singleton](https://en.wikipedia.org/wiki/Singleton_pattern) design pattern

A static class and the [singleton](https://en.wikipedia.org/wiki/Singleton_pattern) are both used to provide a single instance of a class and share functionality across multiple parts of a program. However, there are some key differences between them.

A static class cannot be instantiated; it exists solely to hold static members (methods, properties) that can be accessed without creating an instance of the class. A [singleton](https://en.wikipedia.org/wiki/Singleton_pattern) involves creating a single instance of a class and providing a way to access that instance globally, that class can be instantiated, but it ensures that only one instance is ever created.

Static classes typically do not contain state and are frequently utilized for utility functions or helper methods that can be reused across different parts of the program. 
[Singletons](https://en.wikipedia.org/wiki/Singleton_pattern), on the other hand, can contain state as they are instantiated like regular classes, are utilized to ensure the existence of only one instance of a class, they usually maintain a single instance of themselves throughout the application's lifetime and it is globally accessible.

Static classes are more rigid; they cannot be extended or modified at runtime. [Singleton](https://en.wikipedia.org/wiki/Singleton_pattern) are also rigid in terms of extension because their constructors are typically private to prevent instantiation from outside the class. However, they can be modified or replaced with a different implementation if needed, providing more flexibility compared to static classes.

Static classes cannot be instantiated using the `new` keyword. 
They are implicitly sealed. The compiler prevents the creation of instances of static classes, and you can only access their static members directly.

#### Example. Static class

```cs
// ThisIsNotASingleton instance = new(); // ❌ Compile error

ThisIsNotASingleton.names.ForEach(Console.WriteLine);

ThisIsNotASingleton.Foo();

static class ThisIsNotASingleton {
    public static List<string> names = ["Arya", "Sansa", "Robb", "Bran"];

    public static void Foo() {
        Console.WriteLine("Hello, World!");
    }
}
```

#### Example. Singleton

```cs 
// var instance = new Singleton(); // ❌ Compile error, the constructor is private
var instance = Singleton.Shared;

instance.names.ForEach(Console.WriteLine);

sealed class Singleton {
    public List<string> names = ["Arya", "Sansa", "Robb", "Bran"];

    public static readonly Singleton Shared = new Singleton();
    
    private Singleton() {}

    public void Foo() {
        Console.WriteLine("Hello, World!");
    }
}
```

## Lazy instantiation

The `Singleton` instance previously defined, is typically initialized when the containing type is first accessed by the application. This initialization occurs before any other code in the application can access the shared field.

This means that the `Singleton` instance is created eagerly, not lazily. An instance is created as soon as the class is loaded into memory, regardless of whether it is ever used or not. This can be advantageous in some cases where you want to ensure that the [singleton](https://en.wikipedia.org/wiki/Singleton_pattern) instance is available immediately when the application starts.

For implementations requiring lazy initialization of the singleton instance, techniques such as the [double-check locking pattern](https://en.wikipedia.org/wiki/Double-checked_locking) or the [Lazy<T>](https://learn.microsoft.com/en-us/dotnet/api/system.lazy-1?view=net-8.0) class can be employed. These approaches ensure that the [singleton](https://en.wikipedia.org/wiki/Singleton_pattern) instance is created only upon first access, rather than eagerly during class loading.

#### Double-check locking pattern

```cs
class Singleton {
    private static volatile Singleton? _shared;
    private static readonly object Locker = new object();

    public static Singleton? Shared {
        get {
            if (_shared is null) {
                lock(Locker) {
                    _shared ??= new();
                }
            }
            return _shared;
        }
    }

    private Singleton() {}
}
```

#### [Lazy<T>](https://learn.microsoft.com/en-us/dotnet/api/system.lazy-1?view=net-8.0)

```cs 
public class Singleton {
    public static Singleton Shared => LazyInstance.Value;
    
    private static readonly Lazy<Singleton> LazyInstance = 
        new Lazy<Singleton>(() => {
            return new();
        });

    private Singleton() { }
}
```

## [Monostate design pattern](https://wiki.c2.com/?MonostatePattern)

The [Monostate design pattern](https://wiki.c2.com/?MonostatePattern) is a variation of the [Singleton pattern](https://en.wikipedia.org/wiki/Singleton_pattern) where multiple instances of a class can exist, but they all share the same internal state. In this pattern, each instance of the class behaves as if it were a [singleton](https://en.wikipedia.org/wiki/Singleton_pattern), with all instances sharing the same data. This allows for flexibility in terms of object creation while ensuring that all instances maintain consistency in their state.

It is very bad idea, everybody can change the same state.

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