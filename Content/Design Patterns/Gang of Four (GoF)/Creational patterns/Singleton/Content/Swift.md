# Singleton Design Pattern

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

Unlike some languages like [C#](https://learn.microsoft.com/en-us/dotnet/csharp/), [Swift](https://www.swift.org) does not have a direct equivalent of static classes, but you can achieve similar functionality using [enums](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/enumerations/):

#### Example. Static class functionality using enums

```swift 
enum ThisIsNotASingleton {
    static var names = ["Arya", "Sansa", "Robb", "Bran"]

    static func foo() {
        print("Hello, World!")
    }
}

// let instance = ThisIsNotASingleton() // ❌ Compile error

ThisIsNotASingleton.names.forEach { name in
    print(name)
}

ThisIsNotASingleton.foo()
```

#### Example. Singleton

```swift 
final class Singleton {
    private(set) var names = ["Arya", "Sansa", "Robb", "Bran"]
    static let shared = Singleton()    
    private init() {}
}

// let instance = Singleton() // ❌ Compile error, the constructor is private
let instance = Singleton.shared

instance.names.forEach { name in
    print(name)
}
```

[Swift](https://www.swift.org) guarantees lazy instantiation

## [Monostate design pattern](https://wiki.c2.com/?MonostatePattern)

The [Monostate design pattern](https://wiki.c2.com/?MonostatePattern) is a variation of the [Singleton pattern](https://en.wikipedia.org/wiki/Singleton_pattern) where multiple instances of a class can exist, but they all share the same internal state. In this pattern, each instance of the class behaves as if it were a [singleton](https://en.wikipedia.org/wiki/Singleton_pattern), with all instances sharing the same data. This allows for flexibility in terms of object creation while ensuring that all instances maintain consistency in their state.

It is very bad idea, everybody can change the same state.

```swift 
final class CompanyCEO {
    private static var _name = ""
    private static var _age = 0
    
    var name: String {
        get { Self._name }
        set { Self._name = newValue }
    }
    
    var age: Int {
        get { Self._age }
        set { Self._age = newValue }
    }
}

// Multiple instances
let instance1 = CompanyCEO()
let instance2 = CompanyCEO()
let instance3 = CompanyCEO()

instance1.name = "John Doe"

print(instance1.name)
print(instance2.name)
print(instance3.name)
```