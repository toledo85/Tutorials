using Microsoft.Extensions.Http;
using System.Net.Http.Json;

HttpClient client = new();
TodoService todoService = new(client);

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