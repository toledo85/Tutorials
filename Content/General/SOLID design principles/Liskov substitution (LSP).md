# [Liskov Substitution Principle](https://en.wikipedia.org/wiki/Liskov_substitution_principle)

In this example, the [record](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record) `Square` (child component) changes the behavior of the [record](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record) `Rectangle` (base component) by overriding the `Width` and `Height` properties. This violates the [Liskov Substitution Principle](https://en.wikipedia.org/wiki/Liskov_substitution_principle), which states that a base type should be replaceable with a subtype without altering the correctness of the program. Child components should not remove or alter the behavior of the base component or violate its invariants.

```csharp
Rectangle rectangle = new();
Square square = new();

SetAndMessure(rectangle);
SetAndMessure(square);

void SetAndMessure(Rectangle rect) {
	rect.Width = 3;
	rect.Height = 4;
	Console.WriteLine($"Expected area to be 12 but got {rect.Area}");
}

record Rectangle(double Width = 0, double Height = 0) {
     public virtual double Width { get; set; } = Width;
     public virtual double Height { get; set; } = Height;
     public double Area => Width * Height;
}

record Square : Rectangle {
    public override double Width { 
        set => base.Width = base.Height = value;
    }

    public override double Height {
        set => base.Width = base.Height = value;
    }
}
```

Output:

```
Expected area to be 12 but got 12
Expected area to be 12 but got 16
```

To address this issue, we can use the [`new`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/new) keyword to explicitly indicate that the override is a new implementation and not meant to replace or alter the behavior of the base component.

```csharp
record Square : Rectangle {
    public new double Width { 
        set => base.Width = base.Height = value;
    }

    public new double Height {
        set => base.Width = base.Height = value;
    }
}
```