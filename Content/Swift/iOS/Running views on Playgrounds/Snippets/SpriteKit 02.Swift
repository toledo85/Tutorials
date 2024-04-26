// import PlaygroundSupport
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
// PlaygroundPage.current.liveView = view