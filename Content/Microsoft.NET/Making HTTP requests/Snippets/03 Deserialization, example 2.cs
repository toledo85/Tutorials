using Microsoft.Extensions.Http;
using System.Text.Json;

HttpClient client = new();

try {
    var stringUrl = "https://jsonplaceholder.typicode.com/todos/";
    var responseBody = await client.GetStringAsync(stringUrl);
     
    var options = new JsonSerializerOptions {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    var todos = JsonSerializer.Deserialize<List<Todo>>(responseBody, options);

    foreach (var todo in todos) {
        Console.WriteLine($"Todo Id: {todo.Id}");
        Console.WriteLine($"User Id: {todo.UserId}");
        Console.WriteLine($"Todo Title: {todo.Title}");
        Console.WriteLine($"Todo Completed: {todo.Completed}");
    }
} catch (Exception e) {
    Console.WriteLine("\nException Caught!");
    Console.WriteLine("Message :{0} ", e.Message);
}

public class Todo {
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }
}