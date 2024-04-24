using Microsoft.Extensions.Http;
using System.Net.Http.Json;

HttpClient client = new();

try {
    var stringUrl = "https://jsonplaceholder.typicode.com/todos/1/";
    var data = await client.GetFromJsonAsync<Todo>(stringUrl);

    Console.WriteLine($"Todo Id: {data.Id}");
    Console.WriteLine($"User Id: {data.UserId}");
    Console.WriteLine($"Todo Title: {data.Title}");
    Console.WriteLine($"Todo Completed: {data.Completed}");
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