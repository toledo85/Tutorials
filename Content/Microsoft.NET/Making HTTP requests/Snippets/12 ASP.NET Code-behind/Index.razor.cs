using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace WebApp.Pages;

public partial class Index : ComponentBase {
    [Inject] public HttpClient Http { get; set; }
    private List<Todo> _todos = new();

    protected override async Task OnInitializedAsync() {
        try {
            var stringUrl = "https://jsonplaceholder.typicode.com/todos/";
            _todos = await Http.GetFromJsonAsync<List<Todo>>(stringUrl) ?? new();
        } catch (HttpRequestException e) {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }

    public class Todo {
        // The Todo class defined earlier...
    }
}