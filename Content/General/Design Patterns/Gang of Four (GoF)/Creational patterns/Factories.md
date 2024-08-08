
The [Factory Design Pattern](https://en.wikipedia.org/wiki/Factory_method_pattern) is a creational pattern that abstracts the process of object creation, allowing you to instantiate objects without specifying their exact [`class`](https://docs.swift.org/swift-book/LanguageGuide/ClassesAndStructures.html). It uses a factory method or [`class`](https://docs.swift.org/swift-book/LanguageGuide/ClassesAndStructures.html) to handle object creation, which decouples the client code from the concrete [classes](https://docs.swift.org/swift-book/LanguageGuide/ClassesAndStructures.html) and hides the details of how objects are created. This approach simplifies client code, enhances flexibility and maintainability, supports dependency management, and allows for easy addition of new object types. It also facilitates unit testing by enabling the use of mock objects.

## Problem

[Structs](https://docs.swift.org/swift-book/LanguageGuide/ClassesAndStructures.html) were chosen over [classes](https://docs.swift.org/swift-book/LanguageGuide/ClassesAndStructures.html) to utilize the [memberwise initializer](https://www.hackingwithswift.com/quick-start/understanding-swift/how-do-swifts-memberwise-initializers-work). Therefore, you don't need to define the `Point(x: Double, y: Double)` initializer because in [Swift](https://www.swift.org), [structs](https://docs.swift.org/swift-book/LanguageGuide/ClassesAndStructures.html) have a default memberwise initializer.
Additionally, [structs](https://docs.swift.org/swift-book/LanguageGuide/ClassesAndStructures.html) are printed in the console more nicely than [classes](https://docs.swift.org/swift-book/LanguageGuide/ClassesAndStructures.html), without any extra effort.

The `Point(x: Double, y: Double)` initializer isn't overridden when `init(rho: Double, theta: Double)` is defined, as this initializer is placed in an [`extension`](https://docs.swift.org/swift-book/LanguageGuide/Extensions.html).

One limitation of `init(rho: Double, theta: Double)` is that, although the method parameters clearly indicate initialization from [polar coordinates](https://en.wikipedia.org/wiki/Polar_coordinate_system), it's not immediately obvious that a [cartesian](https://en.wikipedia.org/wiki/Cartesian_coordinate_system) `Point` is being created.

```swift
struct Point {
    var x: Double
    var y: Double
}

extension Point {
    init(rho: Double, theta: Double) {
        x = rho * cos(theta)
        y = rho * sin(theta)
    }
}

let p1 = Point(x: 10, y: 10)
let p2 = Point(rho: 1, theta: 2)

print(p1)
print(p2)
```

## Using factories

You can fix this problem using the [factory pattern](https://en.wikipedia.org/wiki/Factory_method_pattern). By using a [`private`](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/accesscontrol/) [`init`](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/initialization/), you prevent direct initialization of the `Point` [`struct`](https://docs.swift.org/swift-book/LanguageGuide/ClassesAndStructures.html), allowing complex [initialization](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/initialization/) logic behind the scenes. Factory methods like `createCartesian(x: Double, y: Double)` and `fromPolarCoordinates(rho: Double, theta: Double)` provide clear and specific ways to create a `Point`, adding context to the method names and making the code more readable and maintainable. Here's an example:

```swift
struct Point {
    var x: Double
    var y: Double
    
    private init(x: Double, y: Double) {
        self.x = x
        self.y = y
    }
    
    private init(rho: Double, theta: Double) {
        x = rho * cos(theta)
        y = rho * sin(theta)
    }

    static func createCartesian(x: Double, y: Double) -> Point {
        Point(x: x, y: y)
    }
    
    static func fromPolarCoordinates(rho: Double, theta: Double) -> Point {
        Point(rho: rho, theta: theta)
    }
}

// let p1 = Point(x: 10, y: 10)
// let p2 = Point(rho: 1, theta: 2)
let p1 = Point.createCartesian(x: 10, y: 10)
let p2 = Point.fromPolarCoordinates(rho: 1, theta: 2)

print(p1)
print(p2)
```

At some point, if the construction process of your objects becomes too sophisticated, you might want a separate component to handle this process, typically called a factory. In such cases, you can no longer hide the [initializers](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/initialization/) (they cannot remain [`private`](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/accesscontrol/)) within the object itself. Instead of making the [methods `static`](https://www.hackingwithswift.com/read/0/18/static-properties-and-methods), you can create an instance of a factory to manage the construction. For example, you could [instantiate](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/initialization/) a `PointFactory` to create `Point` objects:

```swift
struct Point {
    var x: Double
    var y: Double
}

extension Point {
    init(rho: Double, theta: Double) {
        x = rho * cos(theta)
        y = rho * sin(theta)
    }
}

final class PointFactory {
    func createCartesian(x: Double, y: Double) -> Point {
        Point(x: x, y: y)
    }
    
    func fromPolarCoordinates(rho: Double, theta: Double) -> Point {
        Point(rho: rho, theta: theta)
    }
}

let factory = PointFactory()

let p1 = factory.createCartesian(x: 10, y: 10)
let p2 = factory.fromPolarCoordinates(rho: 1, theta: 2)

print(p1)
print(p2)
```

## Static factories

If you don't need to have a `PointFactory` object, you can use [`static` methods](https://www.hackingwithswift.com/read/0/18/static-properties-and-methods) for the factory [methods](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/methods/). Whether you choose to use [instance methods](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/methods/) or [`static` methods](https://www.hackingwithswift.com/read/0/18/static-properties-and-methods) depends on your preference.  

As long as the [methods](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/methods/) don't require state management, either approach is fine. An instance of `PointFactory` is only necessary if you need to store information, which is quite rare. Factories should not persistence any state.

Since [Swift](https://www.swift.org) doesn’t support [`static` classes](https://en.wiktionary.org/wiki/static_class), we’ll use [enumerations](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/enumerations/) to define the factory, which allows us to prevent the [instantiation](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/initialization/) of a `PointFactory` object.

```swift
enum PointFactory {
    static func createCartesian(x: Double, y: Double) -> Point {
        Point(x: x, y: y)
    }
    
    static func fromPolarCoordinates(rho: Double, theta: Double) -> Point {
        Point(rho: rho, theta: theta)
    }
}

let p1 = PointFactory.createCartesian(x: 10, y: 10)
let p2 = PointFactory.fromPolarCoordinates(rho: 1, theta: 2)

print(p1)
print(p2)
```

## Factory method

So far, the factories have been [methods](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/methods/), but you can also define them as single global [functions](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/functions/) if preferred:

```swift
func createCartesian(x: Double, y: Double) -> Point {
    Point(x: x, y: y)
}

func fromPolarCoordinates(rho: Double, theta: Double) -> Point {
    Point(rho: rho, theta: theta)
}

let p1 = createCartesian(x: 10, y: 10)
let p2 = fromPolarCoordinates(rho: 1, theta: 2)

print(p1)
print(p2)
```

## Inner factories

Let's suppose that you are an object-oriented programming purist and find it unacceptable to expose [initializers](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/initialization/) directly, even if they are wrapped in factory methods. To keep initializers [`private`](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/accesscontrol/), you can use inner factories to encapsulate the creation logic.

```swift
extension Point {
    enum Factory {
        static func createCartesian(x: Double, y: Double) -> Point {
            Point(x: x, y: y)
        }
        
        static func fromPolarCoordinates(rho: Double, theta: Double) -> Point {
            Point(rho: rho, theta: theta)
        }
    }
}

let p1 = Point.Factory.createCartesian(x: 10, y: 10)
let p2 = Point.Factory.fromPolarCoordinates(rho: 1, theta: 2)

print(p1)
print(p2)
```

## Simplifying the design only with [extensions](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/extensions/)

You can move the factory methods to an [`extension`](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/extensions/) of your type, which eliminates the need to create a separate [`class`](https://docs.swift.org/swift-book/LanguageGuide/ClassesAndStructures.html) or [enumeration](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/enumerations/). This approach keeps your main type implementation clean and focused, while still providing a clear and organized way to handle object creation. 

Additionally, using [extensions](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/extensions/) allows you to group related functionality and organize different types of factory [methods](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/methods/). You can also split functionalities into separate [extensions](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/extensions/) as needed. This approach enhances code readability and maintainability, making it easier to manage and expand your code.

```swift
extension Point {
    static func createCartesian(x: Double, y: Double) -> Point {
        Point(x: x, y: y)
    }
}

extension Point {
    static func fromPolarCoordinates(rho: Double, theta: Double) -> Point {
        Point(rho: rho, theta: theta)
    }
}

let p1 = Point.createCartesian(x: 10, y: 10)
let p2 = Point.fromPolarCoordinates(rho: 1, theta: 2)

print(p1)
print(p2)
```

## Abstract factory

This is a rare pattern. This code violates the [Open/Closed rinciple](https://en.wikipedia.org/wiki/Open–closed_principle) because adding a new drink requires modifying the `createDrink` [method](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/methods/) by adding another [`case`](https://www.hackingwithswift.com/read/0/10/switch-case) in the [`switch`](https://www.hackingwithswift.com/read/0/10/switch-case) statement.

```swift
protocol HotDrink {}

class Tea: HotDrink {}

class Coffee: HotDrink {}

enum DrinkType: String {
    case tea = "Preparing tea..."
    case coffee = "Preparing coffee..."
}

enum HotDrinkFactory {
    static func createDrink(type: DrinkType) -> HotDrink? {
        print(type.rawValue)

        return switch type {
            case .tea: Tea()
            case .coffee: Coffee()
        }
    }
}

_ = HotDrinkFactory.createDrink(type: .tea)

_ = HotDrinkFactory.createDrink(type: .coffee)
```

## [UIKit](https://developer.apple.com/documentation/uikit) "real-world" example (bonus) 🎉

The `createScreen(withBackgroundColor color: UIColor)` method returns a [`UIViewController`](https://developer.apple.com/documentation/uikit/uiviewcontroller) rather than a specific [custom view controller](https://developer.apple.com/documentation/uikit/view_controllers). This level of abstraction hides implementation details and enhances flexibility, allowing internal changes without affecting client code.

```swift
extension UIViewController {
    func createScreen(withBackgroundColor color: UIColor) -> UIViewController {
        let vc = UIViewController()
        vc.view.backgroundColor = color
        return vc
    }
}

class ViewController: UIViewController {
    override func viewDidLoad() {
        super.viewDidLoad()
        
        Task { [weak self] in
            try? await Task.sleep(nanoseconds: 1000)
            guard let self else { return }
            let vc =  self.createScreen(withBackgroundColor: .systemIndigo)
            self.show(vc, sender: self)
        }
    }
}
```