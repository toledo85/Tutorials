# Running views on Playgrounds

## SwiftUI

#### Example

````swift
import PlaygroundSupport
import SwiftUI

struct ContentView: View {
    var body: some View {
        VStack {
            Image(systemName: "globe")
                .imageScale(.large)
                .foregroundColor(.accentColor)
            Text("Hello, world!")
        }
    }
}

// Present the view in the Live View window
// PlaygroundPage.current.liveView = UIHostingController(rootView: ContentView())
PlaygroundPage.current.setLiveView(ContentView())
````

#### Example

```swift
import PlaygroundSupport
import SwiftUI

// Present the view in the Live View window
PlaygroundPage.current.setLiveView(
    ZStack {
        Color.indigo
            .frame(width: 300, height: 300)
        Text("Hello, world!")
    }
)
```

## UIKit

```swift
import PlaygroundSupport
import UIKit

class MyViewController : UIViewController {
    override func loadView() {
        let view = UIView()
        view.backgroundColor = .white
        view.addSubview(label)
        self.view = view

        let label = UILabel()
        label.frame = CGRect(x: 150, y: 200, width: 200, height: 20)
        label.text = "Hello World!"
        label.textColor = .black
    }
}

// Present the view controller in the Live View window
PlaygroundPage.current.liveView = MyViewController()
```

## SpriteKit

#### Example

```swift
import PlaygroundSupport
import SpriteKit

let node = SKSpriteNode()
node.color = .systemGreen
node.size = CGSize(width: 100, height: 100)

let scene = SKScene()
scene.size = CGSize(width: 320, height: 480)
scene.scaleMode = .fill
scene.addChild(node)

let view = SKView(frame: CGRect(x: 0, y: 0, width: 320, height: 480))
view.presentScene(scene)

// Present the view in the Live View window
PlaygroundPage.current.liveView = view
```

#### Example

```swift
import PlaygroundSupport
import SpriteKit

class GameScene: SKScene {
    override func didMove(to view: SKView) {
        self.physicsBody = SKPhysicsBody(edgeLoopFrom: self.frame)
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        guard let touch = touches.first else { return }
        let location = touch.location(in: self)

        let box = SKSpriteNode(color: .systemGreen, size: CGSize(width: 50, height: 50))
        box.position = location
        box.physicsBody = SKPhysicsBody(rectangleOf: CGSize(width: 50, height: 50))

        self.addChild(box)
    }
}

let scene = GameScene()
scene.size = CGSize(width: 320, height: 480)
scene.scaleMode = .fill
scene.backgroundColor = .white

let view = SKView(frame: CGRect(x: 0, y: 0, width: 320, height: 480))
view.presentScene(scene)

// Present the view controller in the Live View window
PlaygroundPage.current.liveView = view
```