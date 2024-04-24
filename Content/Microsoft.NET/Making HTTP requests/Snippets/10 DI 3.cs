using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System.Net.Http.Json;

ServiceCollection services = new();

services.AddHttpClient("ApplicationApi", client => {
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
});

services.AddScoped<ITodoService, TodoService>();

using var serviceProvider = services.BuildServiceProvider();

var todoService = serviceProvider.GetService<ITodoService>();

try {
    var todos = await todoService.GetTodos();

    foreach (var todo in todos) {
        Console.WriteLine(todo.Title);
    }
} catch (HttpRequestException e) {
    Console.WriteLine("\nException Caught!");
    Console.WriteLine("Message :{0} ", e.Message);
}

public class Todo {
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }
}

public interface ITodoService {
    Task<IEnumerable<Todo>> GetTodos();
}

public class TodoService(IHttpClientFactory factory) : ITodoService {
    public async Task<IEnumerable<Todo>> GetTodos() {
        var client = factory.CreateClient("ApplicationApi");
        return await client.GetFromJsonAsync<IEnumerable<Todo>>("todos");
    }
}