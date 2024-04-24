using Microsoft.Extensions.Http;
using System.Net.Http.Json;

var jsonPayload = "{\"title\":\"foo\",\"body\":\"bar\",\"userId\":1}";

var requestMessage = new HttpRequestMessage {
    Method = HttpMethod.Post,
    RequestUri = new Uri("https://jsonplaceholder.typicode.com/posts"),
    Content = new StringContent(jsonPayload),
};

var httpClient = new HttpClient();

try {
    var response = await httpClient.SendAsync(requestMessage);
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine(content);
} catch (HttpRequestException e) {
    Console.WriteLine("\nException Caught!");
    Console.WriteLine("Message :{0} ", e.Message);
}

// Example 02
public class HttpService(
    HttpClient client
) {
    public async Task<T?> InvokeGet<T>(string relativeUrl) {
        var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }
}