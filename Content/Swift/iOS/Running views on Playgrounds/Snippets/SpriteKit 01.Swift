// import PlaygroundSupport
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
// PlaygroundPage.current.liveView = view