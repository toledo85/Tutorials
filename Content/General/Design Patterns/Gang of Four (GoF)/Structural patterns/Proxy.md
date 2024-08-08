A proxy is an interface for accessing a particular resource, often used for access control, communications, and logging. It typically provides the same interface as the original object but can have radically different underlying behavior. This allows for seamless integration of functionalities such as cross-process calls, logging, or guarding without changing the original interface. By replicating the existing interface and adding necessary functionality, a proxy can manage resources that are remote, expensive to construct, or require additional operations, all while keeping the interface unchanged.

## Example

```csharp
Driver driver = new Driver(18);

IVehicle toyota = new Toyota();

IVehicle car = new CarProxy(driver, toyota);

car.Drive();

public record Driver(int Age) {}

public interface IVehicle {
    void Drive();
}

internal class Toyota: IVehicle {
    public void Drive() {
        Console.WriteLine("Car being driven");
    }
}

public class CarProxy(Driver driver, IVehicle car): IVehicle {
    private IVehicle _car = car;
    private Driver _driver = driver;
    
    public void Drive() {
        if (_driver.Age >= 16) {
            _car.Drive();
        } else {
            Console.WriteLine("Driver too young");
        }
    }
}
```

## Example

One common requirement in real-world systems is to extract each property of a class into a separate class. Instead of having properties as ordinary fields like an [`Int`](https://developer.apple.com/documentation/swift/int) or [`String`](https://developer.apple.com/documentation/swift/string), you can place them into a separate container to add additional functionality.

```swift
class Property<T> where T: Equatable {
    private var _value: T
    
    public var value: T {
        get {
            _value
        }
        set {
            if (newValue == _value) {
                return
            }
            print("Setting value to \(newValue)")
            _value = newValue
        }
    }
    
    init(value: T) {
        self._value = value
    }
}

extension Property: Equatable {
    static func ==(lhs: Property<T>, rhs: Property<T>) -> Bool {
        lhs.value == rhs.value
    }
}

class Creature {
    private let _agility = Property(value: 0)
    
    var agility: Int {
        get { _agility.value }
        set { _agility.value = newValue }
    }
}

let creature = Creature()

creature.agility = 10
creature.agility = 10
creature.agility = 20

print(creature.agility)
```

## Example

Change the getters and setters of the properties. It's just a trap, it doesn't hide

```ts
const courseObject = { 
    title: 'JavaScript - The Complete Guide' 
}

const wrappedCourse = new Proxy(courseObject, {
    get(target, propKey) { 
        if (propKey === 'length') {
            return 0
        }
        return target[propKey] || 'NOT FOUND'
    },
    set(target, propKey, newValue) {
        console.log('Sending data to the analytic servers...')

        if (propKey !== 'rating') {
            target[propKey] = newValue
        }

        return true
    }
}) 

wrappedCourse.rating = 5
wrappedCourse.title = 'Hello, World!'

console.log(wrappedCourse.title)
console.log(wrappedCourse.length)
console.log(wrappedCourse.rating)
console.log(wrappedCourse)
```