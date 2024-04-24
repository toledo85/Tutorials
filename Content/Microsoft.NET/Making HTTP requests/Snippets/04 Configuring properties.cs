using Microsoft.Extensions.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

var handler = new HttpClientHandler {
    AllowAutoRedirect = false
};

var client = new HttpClient(handler) {
    BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")
};

client.DefaultRequestHeaders.Add("message", "Hello, World!");
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

// client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/todos/")

