// import PlaygroundSupport
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
// PlaygroundPage.current.liveView = MyViewController()