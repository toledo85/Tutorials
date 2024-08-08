## Example

```csharp
DoSomeComplexMath(Shape.Circle);

void DoSomeComplexMath(Shape shape) {
	// Doing complex calculations...

    var message = shape switch {
        Shape.Square => "Performing calculations for a Square.",
        Shape.Triangle => "Performing calculations for a Triangle.",
        Shape.Circle => "Performing calculations for a Circle.",
        _ => throw new ArgumentOutOfRangeException()
    };

    Console.WriteLine(message);
}

enum Shape {
    Square,
    Triangle,
    Circle
}
```

This code violates the [Open-closed principle](https://en.wikipedia.org/wiki/Open–closed_principle) because adding a new shape requires changing the existing code. You need to update the [`switch`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements) statement to handle the new case added to the `Shape` [enumeration](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum), or add another [`if`-`else`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements) if that’s what you are using. 

This means the code does not follow the [Open-Closed Principle](https://en.wikipedia.org/wiki/Open–closed_principle), which states that "Software entities (classes, modules, functions, etc.) should be open for extension but closed for modification." The principle emphasizes that you should be able to add new functionality without changing existing code.

Additionally, this code could potentially violate the [Liskov Substitution Principle](https://en.wikipedia.org/wiki/Liskov_substitution_principle). 

```csharp
void DoSomeComplexMath(object shape) {
    // This code can break...
}
```

Although the current implementation handles all defined [enum](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum) values, if the `shape` parameter were of type `object`, or if not all cases are handled in the [`switch`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements) statement, it could lead to runtime errors.

We can fix this with the next approach:

```csharp
Square square = new();

DoSomeComplexMath(square);

void DoSomeComplexMath(IShape shape) {
    Console.WriteLine(shape);
}

interface IShape {}

public class Square : IShape {
    public override string ToString() {
        return "Performing calculations for a Square.";
    }
}

public class Triangle : IShape {
    public override string ToString() {
        return "Performing calculations for a Triangle.";
    }
}

public class Circle : IShape {
    public override string ToString() {
        return "Performing calculations for a Circle.";
    }
}
```

## Example

```csharp
Product[] products = [
    new("Apple", "green", 1),
    new("Tree", "green", 100),
    new("House", "blue", 100_000)
];

ProductFilter filter = new();

var productsFilteredByColor = filter.FilterByColor(products, "green");

Console.WriteLine("Green products:");

foreach (var product in productsFilteredByColor) {
	Console.WriteLine(product.Name);
}

public record Product(string Name, string Color, decimal Price) {}

public class ProductFilter {
	public IEnumerable<Product> FilterByColor(
		IEnumerable<Product> products, 
		string color
	) {
		return products.Where(product => product.Color == color);
	}
}
```

The issue with this code is that if you later want to add a new filter, you must modify the `ProductFilter` code. This requires changing existing code, which violates the [Open-closed Principle](https://en.wikipedia.org/wiki/Open–closed_principle). 

```csharp
public class ProductFilter {
	public IEnumerable<Product> FilterByColor(
		IEnumerable<Product> products, 
		string color
	) {
		return products.Where(product => product.Color == color);
	}

	public IEnumerable<Product> FilterByPrice(
		IEnumerable<Product> products, 
		decimal price
	) {
		return products.Where(product => product.Price == price);
	}
}
```

So we can fix this by using the [Specification pattern](https://en.wikipedia.org/wiki/Specification_pattern):

```csharp
BetterFilter filter = new();

var productsFiltered = filter.Filter(
	products,
	new ColorSpecification("green")
);

foreach (var product in productsFiltered) {
	Console.WriteLine(product);
}

public interface ISpecification<T> {
	bool IsSatisfied(T t);
}

public interface IFilter<T> {
	IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
}

public class ColorSpecification(string color) : ISpecification<Product> {
    public bool IsSatisfied(Product product) {
        return product.Color == color;
    }
}

public class BetterFilter : IFilter<Product> {
	public IEnumerable<Product> Filter(
        IEnumerable<Product> items,
        ISpecification<Product> spec
    ) {
		foreach (var i in items) {
			if (spec.IsSatisfied(i)) {
				yield return i;
			}
		}
	}
}
```

Creating all the specifications:

```csharp
public class NameSpecification(string name) : ISpecification<Product> {
    public bool IsSatisfied(Product product) {
        return product.Name == name;
    }
}

public class ColorSpecification(string color) : ISpecification<Product> {
    public bool IsSatisfied(Product product) {
        return product.Color == color;
    }
}

public class PriceSpecification(decimal price) : ISpecification<Product> {
    public bool IsSatisfied(Product product) {
        return product.Price == price;
    }
}
```

Combining the specifications:

```csharp
var productsFiltered = filter.Filter(
	products,
	new CombinedSpecification<Product>(
		new ColorSpecification("green"),
		new PriceSpecification(100)
	)
);

public class CombinedSpecification<T>(
	ISpecification<T> first, 
	ISpecification<T> second
) : ISpecification<T> {
	public bool IsSatisfied(T p) {
		return first.IsSatisfied(p) && second.IsSatisfied(p);
	}
}
```

#### Better approach with [extension methods](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods)

A simpler solution is to use [extension methods](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods). This approach avoids the need for additional filter classes or specifications, improving code readability and maintainability.

```csharp
var productsFiltered = products.FilterByColor("green");

// ...

public static class FilterExtension {
	public static IEnumerable<Product> FilterByColor(
        this IEnumerable<Product> products,
        string color
    ) => products.Where(product => product.Color == color);
}
```

You can define all [extension methods](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) for filters that you want or needs, either in the same class or across multiple classes. You can organize these methods within different modules or even different projects, based on your needs and preferences.

```csharp
public static class ColorFilterExtension {
	// ...
}

public static class SizeFilterExtension {
	// ...
}

public static class PriceFilterExtension {
	// ...
}
```

## Consideration 💡

An important consideration, not directly related to the [Open-closed principle](https://en.wikipedia.org/wiki/Open%E2%80%93closed_principle), is that sometimes there's no need to over-engineer by wrapping built-in code in custom classes. For example, [LINQ](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/introduction-to-linq-queries) allows you to filter data simple and straightforward:

```csharp
var productsFiltered = products.Where(product => 
	product.Color == "green" && 
	product.Price > 100 &&
	product.Name.Contains("apple")
);
```