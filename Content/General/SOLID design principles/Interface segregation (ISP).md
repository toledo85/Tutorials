In this example, the `IMachine` [`interface`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface) includes `Print`, `Fax`, and `Scan` methods. However, the `SimplePrinter` only needs the `Print` method, but it is forced to implement `Fax` and `Scan` as well. This results in empty or dummy methods, which can be confusing and lead to maintenance issues. You might not want the `SimplePrinter` to have `Fax` and `Scan` capabilities, but since these methods are required by the `IMachine` [`interface`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface), you need to implement them all.

```csharp
class Document { }

interface IMachine {
	void Print(Document document);
	void Fax(Document document);
	void Scan(Document document);
}

class SimplePrinter : IMachine {
	public void Print(Document document) {
		Console.WriteLine("Printing");
	}

	public void Fax(Document document) {}
	public void Scan(Document document) {}
}

class MultiFunctionPrinter : IMachine {
	public void Print(Document document) {
		Console.WriteLine("Printing with colors the document");
	}

	public void Fax(Document document) {
		Console.WriteLine("Sending fax");
	}

	public void Scan(Document document) {
		Console.WriteLine("Scanning the document");
	}
}
```

This code violates the [Interface Segregation Principle (ISP)](https://en.wikipedia.org/wiki/Interface_segregation_principle) because it forces the `SimplePrinter` [`class`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/class) to implement methods that it doesn't need. 

By having a large, monolithic interface, any change in the `IMachine` interface affects all implementing classes, even those that don't use the new or changed methods. This increases the risk of bugs and makes the code harder to maintain and extend. 

To address this issue, you can split the `IMachine` interface into smaller, more specific interfaces that only include methods relevant to the classes implementing them. This way, `SimplePrinter` would only implement a `IPrinter` interface with a `Print` method, and other devices could implement `IScanner` and `IFax` interfaces as needed.

> 💡 Notice how `MultiFunctionMachine` is composed of other components.

```csharp
SimplePrinter printer = new();

Photocopier scanner = new();

IMultiFunctionDevice multi = new MultiFunctionMachine(
	printer, 
	scanner
);

interface IPrinter {
    void Print(Document document);
}

interface IScanner {
    void Scan(Document document);
}

interface IFax {
    void Fax(Document document);
}

interface IMultiFunctionDevice : IPrinter, IScanner, IFax {}

class SimplePrinter : IPrinter {
    public void Print(Document document) {
        Console.WriteLine("Printing the document");
    }
}

class Photocopier : IPrinter, IScanner {
    public void Print(Document document) =>
        throw new NotImplementedException();

    public void Scan(Document document) =>
        throw new NotImplementedException();
}

struct MultiFunctionMachine(
	IPrinter printer, 
	IScanner scanner
) : IMultiFunctionDevice {
    public void Print(Document document) => printer.Print(document);
    public void Scan(Document document) => scanner.Scan(document);
    public void Fax(Document document) => throw new NotImplementedException();
}
```