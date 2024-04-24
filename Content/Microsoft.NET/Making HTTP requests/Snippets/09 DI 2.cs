using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System.Net.Http.Json;

ServiceCollection services = new();

services.AddHttpClient();
services.AddScoped<TodoService>();

using var serviceProvider = services.BuildServiceProvider();

var todoService = serviceProvider.GetService<TodoService>();

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

public class TodoService(HttpClient client) {
    public async Task<IEnumerable<Todo>> GetTodos() {
        var stringUrl = "https://jsonplaceholder.typicode.com/todos/";
        return await client.GetFromJsonAsync<IEnumerable<Todo>>(stringUrl);
    }
}