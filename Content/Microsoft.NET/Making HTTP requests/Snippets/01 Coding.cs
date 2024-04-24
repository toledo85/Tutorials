using Microsoft.Extensions.Http;

HttpClient client = new();

try {
    var stringUrl = "https://jsonplaceholder.typicode.com/todos/";
    var responseBody = await client.GetStringAsync(stringUrl);
    Console.WriteLine(responseBody);
} catch (HttpRequestException e) {
    Console.WriteLine("\nException Caught!");
    Console.WriteLine("Message :{0} ", e.Message);
}