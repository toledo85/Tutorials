
The [Bridge pattern](https://en.wikipedia.org/wiki/Bridge_pattern) is a structural design pattern that decouples an abstraction from its implementation, allowing them to vary independently. This separation is useful when both abstractions and implementations need to extend in multiple dimensions.

- Decouple abstraction from implementation.
- Connecting components toguether through abstractions.
- A mechanism that decouples an interface (hierarchy) from an implementation (hierarchy).

## Problem

This approach requires creating `IShape` for each rendering mode, leading to duplication and inflexibility. This means every time a new rendering method is introduced, a corresponding circle [`class`](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/classes) must be created, resulting in code redundancy and maintenance challenges. 

```csharp
RasterRendereredCircle circle = new(5);
circle.Draw();
circle.Resize(2);
circle.Draw();

VectorRendereredCircle anotherCircle = new(20);
anotherCircle.Draw();
anotherCircle.Resize(2);
anotherCircle.Draw();

interface IShape {
    void Draw();
    void Resize(double factor);
}

class RasterRendereredCircle(double radius) : IShape {
    private double _radius = radius;
    
    public void Draw() {
        Console.WriteLine($"Drawing pixels for circle of radius {_radius}");
    }
    
    public void Resize(double factor) {
        _radius *= factor;
    }
}

class VectorRendereredCircle(double radius) : IShape {
    private double _radius = radius;
    
    public void Draw() {
        Console.WriteLine($"Drawing pixels for circle of radius {_radius}");
    }
    
    public void Resize(double factor) {
        _radius *= factor;
    }
}
```

## Solution 

I can create a new rendering implementation without modifying the `Circle` class, using the `IRender` [interface](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/interfaces)
as a bridge to decouple the abstraction from the implementation. This allows us to easily switch between different rendering strategies, such as vector and raster rendering, as shown in the code:

```csharp
IRender vectorRenderer = new VectorRenderer();

Circle circle = new(5, vectorRenderer);
circle.Draw();
circle.Resize(2);
circle.Draw();

interface IShape {
    void Draw();
    void Resize(double factor);
}

interface IRender {
    void RenderCircle(double radius);
}

class VectorRenderer : IRender {
    public void RenderCircle(double radius) {
        Console.WriteLine($"Drawing a circle or radius {radius}");
    }
}

class RasterRenderer : IRender {
    public void RenderCircle(double radius) {
        Console.WriteLine($"Drawing pixels for circle of radius {radius}");
    }
}

class Circle(double radius, IRender renderer) : IShape {
    private double _radius = radius;
    
    public void Draw() {
        renderer.RenderCircle(_radius);
    }
    
    public void Resize(double factor) {
        _radius *= factor;
    }
}
```