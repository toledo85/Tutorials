import SwiftUI
import PlaygroundSupport

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

PlaygroundPage.current.liveView = UIHostingController(rootView: ContentView())
