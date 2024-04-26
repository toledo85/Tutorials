import UIKit
import PlaygroundSupport

class MyViewController : UIViewController {
    override func loadView() {
        let label = UILabel()
        label.frame = CGRect(x: 150, y: 200, width: 200, height: 20)
        label.text = "Hello World!"
        label.textColor = .black

        let view = UIView()
        view.backgroundColor = .systemBackground
        view.addSubview(label)
        self.view = view
    }
}

PlaygroundPage.current.liveView = MyViewController()
