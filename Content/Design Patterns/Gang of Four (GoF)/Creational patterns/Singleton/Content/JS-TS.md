# [Singleton design pattern](https://en.wikipedia.org/wiki/Singleton_pattern)

The [Singleton design pattern](https://en.wikipedia.org/wiki/Singleton_pattern) ensures that only one object of a given type exists in your application, effectively resolving the "single/shared resource problem". Some components are best represented by a single instance in the system, such as a base repository of data or an object factory. This pattern is particularly beneficial when the initializer call is expensive, as it ensures that the initialization process is performed only once.

[Singleton](https://en.wikipedia.org/wiki/Singleton_pattern) objects persist throughout the lifecycle of your application. They are akin to [global variables](https://en.wikipedia.org/wiki/Global_variable), but with distinct names. However, both [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) and [global variables](https://en.wikipedia.org/wiki/Global_variable) may encounter concurrency issues, hidden dependencies, and namespace pollution. Unlike [global variables](https://en.wikipedia.org/wiki/Global_variable), [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) have getters and setters.

[Singleton](https://en.wikipedia.org/wiki/Singleton_pattern) objects can be difficult to test and refactor. Instead of directly relying on a [singleton](https://en.wikipedia.org/wiki/Singleton_pattern), it's advisable to depend on an abstraction, such as a protocol or interface. Additionally, consider use the [Dependency injection pattern](https://en.wikipedia.org/wiki/Dependency_injection). 

While [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) offer convenience due to their single instance nature, they can lead to global mutation, making it risky for all parts of the application to change. Therefore, extra caution is needed, especially in concurrent environments.

There's nothing wrong with using [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) as long as you're aware of the potential issues they can bring and ensure they're implemented carefully and appropriately to meet the specific requirements of the application.

#### Example. Singleton

```ts 
export default {
    names = ["Arya", "Sansa", "Robb", "Bran"]
}
```

#### Example

```ts
console.log(Settings.shared)

class Settings {
    names = ["Arya", "Sansa", "Robb", "Bran"]

    shared = new Settings()

    private constructor() {}
}
```

## Lazy instantiation

```cs
class Singleton {
    private static _shared: Singleton

    static get shared(): Singleton {
        if (!this._shared) {
            this._shared = new this()
        }
        return this._shared
    }

    names = ["Arya", "Sansa", "Robb", "Bran"]

    protected constructor() {}
}

console.log(Singleton.shared.names)
```

## [Monostate design pattern](https://wiki.c2.com/?MonostatePattern)

The [Monostate design pattern](https://wiki.c2.com/?MonostatePattern) is a variation of the [Singleton pattern](https://en.wikipedia.org/wiki/Singleton_pattern) where multiple instances of a class can exist, but they all share the same internal state. In this pattern, each instance of the class behaves as if it were a [singleton](https://en.wikipedia.org/wiki/Singleton_pattern), with all instances sharing the same data. This allows for flexibility in terms of object creation while ensuring that all instances maintain consistency in their state.

It is very bad idea, everybody can change the same state.

```cs 
class CompanyCEO {
    private static _name = "John Doe";
    private static _age = 0;
    
    get name(): string {
        return CompanyCEO._name;
    }

    set name(value: string) {
        CompanyCEO._name = value
    }
    
    get age(): number {
        return CompanyCEO._age
    }

    set age(value: number) {
        CompanyCEO._age = value;
    }
}

// Multiple instances
const instance1 = new CompanyCEO()
const instance2 = new CompanyCEO()
const instance3 = new CompanyCEO()

instance1.name = 'John Doe'

console.log(instance1.name)
console.log(instance2.name)
console.log(instance3.name)
```