The [decorator pattern](https://en.wikipedia.org/wiki/Decorator_pattern) allows adding behavior to a class without altering its code or using [inheritance](https://en.wikipedia.org/wiki/Inheritance_(object-oriented_programming)). This approach adheres to the [Open/Closed principle](https://en.wikipedia.org/wiki/Open–closed_principle#:~:text=In%20object%2Doriented%20programming%2C%20the,without%20modifying%20its%20source%20code.) and [Single Responsibility principle](https://en.wikipedia.org/wiki/Single-responsibility_principle) by extending functionality while keeping the original code intact. You can either inherit from the object if possible or use the [decorator pattern](https://en.wikipedia.org/wiki/Decorator_pattern) to wrap the existing object and add new behavior. The decorator may or may not proxy method calls based on whether you need to replicate the original API. 

## Example

You cannot inherit from the [`System.String`](https://learn.microsoft.com/en-us/dotnet/api/system.string?view=net-8.0) [`class`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/class) because it is [`sealed`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/sealed). To add new functionality to [strings](https://learn.microsoft.com/en-us/dotnet/api/system.string?view=net-8.0), you can use the [decorator pattern](https://en.wikipedia.org/wiki/Decorator_pattern). Additionally, you can use the [implicit conversion operator](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators) for better integration:

```csharp
MyString foo = new("Hello, Foo");
MyString bar = "Hello, Bar";

foo.DoSomething();
bar.DoSomething();

public class MyString(string message) {
    public void DoSomething() {
        // Add new behaviour...
        Console.WriteLine(message);
    }

    public static implicit operator MyString(string value) {
        return new MyString(value);
    }
}
```

This is not the best example because you can achieve the same result using [extension methods](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) without creating an additional [`class`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/class) like `MyString` or using the [implicit conversion operator](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators), which simplifies the implementation, is less cumbersome.

```csharp
var message = "Hello, World!";

message.DoSomething();

public static class StringExtensions {
    public static void DoSomething(this string message) {
        // Add new behaviour...
        Console.WriteLine(message);
    }
} 
```

In the near future, we hope to have [extensions](https://docs.swift.org/swift-book/documentation/the-swift-programming-language/extensions/) similar to those in [Swift](https://www.swift.org):

```swift
extension String {
    func doSomething() {
        // Add new behaviour...
        print(self)
    }
}
```

## Example

My `MyStringBuilder` want to behave like a [`StringBuilder`](https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder?view=net-8.0),
I add `Append` method because
you need to replicate the original API.
An then I will add my new method `DoNothing` with new funtionality 
I will do my new API fluent.

```swift
MyStringBuilder builder = new();

var message = builder
    .Append("Foo")
    .Append("Bar")
    .DoNothing()
    .DoNothing()
    .Append("Bal")
    .ToString();

Console.WriteLine(message);

public class MyStringBuilder(StringBuilder? buffer = null) {
    private readonly StringBuilder _buffer = buffer ?? new();

    public MyStringBuilder Append(string other) {
        _buffer.Append(other);
        return this;
    }
    
    public MyStringBuilder DoNothing() {
        // New behaviour...
        Console.WriteLine("This method do nothing.");
        return this;
    }
    
    public override string ToString() {
        return _buffer.ToString();
    }
}
```

## Example

[Multiple inheritance](https://en.wikipedia.org/wiki/Multiple_inheritance) is not supported in languages like [Swift](https://www.swift.org) or [C#](https://learn.microsoft.com/en-us/dotnet/csharp/). The following code will not compile if the commented line is included:

```csharp
class Bird {}

class Lizard {}

// class Dragon: Bird, Lizard {} // Compile error ❌
```

Instead, you can achieve similar functionality using the [decorator pattern](https://en.wikipedia.org/wiki/Decorator_pattern):

```csharp
interface ICanFly {
    void Fly();
}

interface ICanCrawl {
    void Crawl();
}

class Bird : ICanFly {
    public void Fly() {}
}

class Lizard : ICanCrawl {
    public void Crawl() {}
}

class Dragon : ICanFly, ICanCrawl {
    private ICanFly _bird = new Bird();
    private ICanCrawl _lizard = new Lizard();
    
    public void Fly() {
        _bird.Fly();
    }
    
    public void Crawl() {
        _lizard.Crawl();
    }
}
```

Using [interfaces](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface) helps to explicitly communicate the capabilities of a [`class`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/class) to whoever is using it such as `Dragon`, which can fly and crawl.

## Dynamic decorator composition

Imagine you’re working with an application that renders different shapes and you want to add new features to these shapes. Using the [decorator pattern](https://en.wikipedia.org/wiki/Decorator_pattern), you can enhance a shape like a `Square` with additional features:

```csharp
var square = new Square(100);
var redSquare = new ColoredShape(square, "red");

var semiTransparentRedSquare = new TransparentShape(redSquare, 0.5);
var transparentSquare = new TransparentShape(redSquare, 1);

Console.WriteLine(square);
Console.WriteLine(redSquare);
Console.WriteLine(semiTransparentRedSquare);
Console.WriteLine(transparentSquare);

interface IShape {}

class Square(double side): IShape {
    public double Side { get; private set; } = side;
    
    public void Resize(double factor) {
        Side *= factor;
    }

    public override string ToString() {
        return  $"Square with side {Side}";
    }
}

class ColoredShape(IShape baseShape, string color): IShape {
    public override string ToString() {
        return  $"{baseShape} has color {color}";
    }
}

class TransparentShape(IShape baseShape, double transparency): IShape {
    public override string ToString() {
        return  $"{baseShape} has {transparency * 100}% transparency";
    }
}
```

## Static decorator composition

Static decorator composition, as opposed to its dynamic counterpart, is less flexible and is determined entirely at compile time. Unlike dynamic decorators, where you can modify the type or behavior at runtime, static composition requires you to specify the types in advance, leading to less flexibility. 

With static decorators, you cannot change the type or behavior dynamically during runtime. Instead, all decisions about composition must be made at compile time. This limits your ability to adjust or extend functionality based on runtime conditions, as all combinations and behaviors are fixed at compile time.

```csharp
Square square = new(100);

ColoredShape<Square> blueSquare = new(color: "Blue");

TransparentShape<ColoredShape<Square>> blackHalfSquare = new(transparency: 0.5);

Console.WriteLine(blueSquare);    
Console.WriteLine(blackHalfSquare);

interface IShape {}

class Square(double side) : IShape {
    public double Side { get; private set; } = side;

    public Square() : this(0) {}

    public void Resize(double factor) {
        Side *= factor;
    }
    
    public override string ToString() {
        return $"Square with side {Side}";
    }
}

class ColoredShape<T>(
    string color
) : IShape where T : IShape, new() {
    private readonly T _shape = new();
    private string _color = color;
    
    public ColoredShape() : this(string.Empty) {}
    
    public override string ToString() {
        return $"{_shape} has color {_color}";
    }
}

class TransparentShape<T>(
    double transparency
) : IShape where T : IShape, new() {
    private readonly T _shape = new();
    private double _transparency = transparency;
    
    public override string ToString() {
        return $"{_shape} has {_transparency * 100}% transparency";
    }
}
```

## Real world example (bonus) 🎉

Sometimes, you have a component in production that you can't change because it might introduce bugs or you don't have access to the source code. In such cases, you can add new functionality by using the existing component and combining it with others. This way, you can enhance its features without modifying the original code.

For example, consider the [SwiftUI](https://developer.apple.com/xcode/swiftui/) component. You cannot directly add new functionality to the [`Text`](https://developer.apple.com/documentation/swiftui/text) view, but you can extend its capabilities by composing it with other components:

```swift
import SwiftUI

struct TextView: View {
    let message: String
    
    var body: some View {
        Text(message)
            .font(.title)
            .onTapGesture {
                // ...
            }
    }
}

struct ContentView: View {
    var body: some View {
        VStack {
            // Text("Hello, Foo!")
            // Text("Hello, Bar!")
            // Text("Hello, Bal!")
            TextView(message: "Hello, Foo!")
            TextView(message: "Hello, Bar!")
            TextView(message: "Hello, Bal!")
        } // .font(.title).onTapGesture {}
    }
}
```

The [decorator](https://en.wikipedia.org/wiki/Decorator_pattern)-[composite](https://en.wikipedia.org/wiki/Composite_pattern) pattern is used to add functionality to an existing class without altering its source code. This approach is applicable to any component-based framework, such as [Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/), [React](https://reactjs.org/), [Vue](https://vuejs.org/), [Angular](https://angular.io/), [Jetpack Compose](https://developer.android.com/jetpack/compose), [SwiftUI](https://developer.apple.com/xcode/swiftui/), and [Flutter](https://flutter.dev/), among others.