The [Single Responsibility Principle (SRP)](https://en.wikipedia.org/wiki/Single_responsibility_principle) states that a class should have only one reason to change, meaning that a class should have only one responsibility. In other words, a class should encapsulate only one aspect of functionality or behavior. If a class has multiple responsibilities, it becomes more difficult to understand, maintain, and modify. By adhering to the [SRP](https://en.wikipedia.org/wiki/Single_responsibility_principle), each class becomes more focused and less dependent on other classes, making the codebase more modular and flexible.

For example, let's consider a scenario where a [`record`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record) called `Square`:

```swift
Square square = new(5.0);

square.DisplayInfo();

record Square(double Side) {
    public double Area => Side;

    public void DisplayInfo() {
        Console.WriteLine(this);
        Console.WriteLine($"Square with side length {Side}");
        Console.WriteLine($"Area: {Area} square units");
    }
}
```

The `Square` [`record`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record) currently violates the [SRP](https://en.wikipedia.org/wiki/Single_responsibility_principle) by encompassing various responsibilities, such as representing itself based on a given side length, calculating its area, and providing detailed information. A better design would be to separate these responsibilities into distinct components. For example:

```swift
Square square = new(5.0);

Printer printer = new();

printer.DisplayInfo(square);

record Square(double Side) {
    public double Area => Side * Side;
}

class Printer {
    public void DisplayInfo(Square square) {
        Console.WriteLine(square);
        Console.WriteLine($"Square with side length {square.Side}");
        Console.WriteLine($"Area: {square.Area} square units");
    }
}
```

You can simplify it further by using [extension methods](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods):

```csharp
Square square = new(5.0);

square.DisplayInfo();

record Square(double Side) {
    public double Area => Side * Side;
}

static class SquarePrinterExtension {
    public static void DisplayInfo(this Square square) {
        Console.WriteLine(square);
        Console.WriteLine($"Square with side length {square.Side}");
        Console.WriteLine($"Area: {square.Area} square units");
    }
}
```