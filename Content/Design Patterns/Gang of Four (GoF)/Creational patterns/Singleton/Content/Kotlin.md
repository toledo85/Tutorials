# Singleton Design Pattern

The [Singleton design pattern](https://en.wikipedia.org/wiki/Singleton_pattern) ensures that only one object of a given type exists in your application, effectively resolving the "single/shared resource problem". Some components are best represented by a single instance in the system, such as a base repository of data or an object factory. This pattern is particularly beneficial when the initializer call is expensive, as it ensures that the initialization process is performed only once.

[Singleton](https://en.wikipedia.org/wiki/Singleton_pattern) objects persist throughout the lifecycle of your application. They are akin to [global variables](https://en.wikipedia.org/wiki/Global_variable), but with distinct names. However, both [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) and [global variables](https://en.wikipedia.org/wiki/Global_variable) may encounter concurrency issues, hidden dependencies, and namespace pollution. Unlike [global variables](https://en.wikipedia.org/wiki/Global_variable), [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) have getters and setters.

[Singleton](https://en.wikipedia.org/wiki/Singleton_pattern) objects can be difficult to test and refactor. Instead of directly relying on a [singleton](https://en.wikipedia.org/wiki/Singleton_pattern), it's advisable to depend on an abstraction, such as a protocol or interface. Additionally, consider use the [Dependency injection pattern](https://en.wikipedia.org/wiki/Dependency_injection). 

While [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) offer convenience due to their single instance nature, they can lead to global mutation, making it risky for all parts of the application to change. Therefore, extra caution is needed, especially in concurrent environments.

There's nothing wrong with using [singletons](https://en.wikipedia.org/wiki/Singleton_pattern) as long as you're aware of the potential issues they can bring and ensure they're implemented carefully and appropriately to meet the specific requirements of the application.

## [Object declaration](https://kotlinlang.org/docs/object-declarations.html)

[Kotlin](https://kotlinlang.org) guarantees lazy instantiation

```kotlin
object Singleton {
    var names = mutableListOf("Arya", "Bran", "Robb", "Sansa")
        private set

    fun foo() {
        println("Hello, World!")
    }
}

fun main() {
    // val instance = Singleton() // ❌ Compile error, the constructor is private
    Singleton.shared.names.forEach {
        println(it)
    }

    Singleton.shared.names.foo()
}
```

## [Companion object](https://kotlinlang.org/docs/object-declarations.html)

#### Non-lazy instantiation

```kotlin
class Singleton private constructor() {
    var names = arrayOf("Arya", "Bran", "Robb", "Sansa")
        private set

    companion object {
        private var _shared: NamesService? = null
        val shared: NamesService
           get() {
               _shared = _shared ?: NamesService()
               return _shared!!
           }
    }
}
```

#### Lazy instantiation

```kotlin
class Singleton private constructor() {
    var names = arrayOf("Arya", "Bran", "Robb", "Sansa")
        private set

    companion object {
        val shared: Singleton by lazy {
            Singleton()
        }
    }
}
```

## [Monostate design pattern](https://wiki.c2.com/?MonostatePattern)

The [Monostate design pattern](https://wiki.c2.com/?MonostatePattern) is a variation of the [Singleton pattern](https://en.wikipedia.org/wiki/Singleton_pattern) where multiple instances of a class can exist, but they all share the same internal state. In this pattern, each instance of the class behaves as if it were a [singleton](https://en.wikipedia.org/wiki/Singleton_pattern), with all instances sharing the same data. This allows for flexibility in terms of object creation while ensuring that all instances maintain consistency in their state.

It is very bad idea, everybody can change the same state.

```kotlin 
class CompanyCEO {
    companion object {
        private var _name = ""
        private var _age = 0
    }

    var name: String
        get() = _name
        set(value) { _name = value }

    var age: Int
        get() = _age
        set(value) { _age = value }
}

fun main() {
    // Multiple instances
    val instance1 = CompanyCEO()
    val instance2 = CompanyCEO()
    val instance3 = CompanyCEO()

    instance1.name = "John Doe"

    println(instance1.name)
    println(instance2.name)
    println(instance3.name)
}
```