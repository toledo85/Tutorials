# Making HTTP requests

## Up and running

We will create a [console application](https://learn.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio?pivots=dotnet-8-0) for this purpose. You can do this using [GUI IDE](https://en.wikipedia.org/wiki/Integrated_development_environment)s like [Visual Studio](https://visualstudio.microsoft.com) or [Rider](https://www.jetbrains.com/rider/), but for standardization, all examples will be done using the [.NET CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/).

So, execute this commands in the terminal:

```sh
dotnet new console -o ClientApplication
```

This command will create a folder named ClientApplication, which is nothing more than a [.NET console application](https://learn.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio?pivots=dotnet-8-0). Afterwards, we navigate inside the folder: 

```sh
cd ClientApplication/
```

Then, we'll install these packages using the following [.NET CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/) commands:

```sh
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Http
```

The package `Microsoft.Extensions.DependencyInjection` is not directly related to performing [HTTP requests](https://developer.mozilla.org/en-US/docs/Web/HTTP/Overview). However, we will use it in some examples.

## Coding

So, let's begin coding, delete all the content of the `Program.cs` file and write the following:

```cs
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
```

Next, enter this command in the terminal:

```sh
dotnet run
```

Then, the screen should display something like this, but with more entries:

```
{
    "userId": 2,
    "id": 31,
    "title": "repudiandae totam in est sint facere fuga",
    "completed": false
},
{
    "userId": 2,
    "id": 32,
    "title": "earum doloribus ea doloremque quis",
    "completed": false
}
```

So, congratulations, you've successfully made your first HTTP request, downloaded data from the internet, and displayed it on the screen as a `string`.

## [Serialization and Deserialization](https://learn.microsoft.com/en-us/dotnet/standard/serialization/)

We are sending requests to an [API](https://en.wikipedia.org/wiki/Web_API) that responds with data in [JSON](https://en.wikipedia.org/wiki/JSON) format. To make the downloaded information more usable, we need to convert it into [C#](https://dotnet.microsoft.com/en-us/languages/csharp#:~:text=C%23%20is%20a%20modern%2C%20innovative,5%20programming%20languages%20on%20GitHub.) objects for seamless manipulation, enabling us to extract insights, perform operations, and enhance our application's functionality.

This content isn't exclusively tied to making [HTTP requests](https://developer.mozilla.org/en-US/docs/Web/HTTP/Overview); it can also be useful when reading [JSON](https://en.wikipedia.org/wiki/JSON) from a hard drive or extracting [JSON](https://en.wikipedia.org/wiki/JSON) from any other source.

So, remove the entire last block of code and replace it with this new code:

```cs
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
```

> Note that the `System.Net.Http.Json` namespace did not need to be installed; it's the one that allows us to use the extension method `GetFromJsonAsync`.

So, if you need more control over your [deserialization](https://learn.microsoft.com/en-us/dotnet/standard/serialization/) process, consider rewriting the network code as follows:

```cs
var client = new HttpClient();

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
```

## Designing models

```cs
class Todo {
    // ...
}

class People {
    // ...
}

class News {
    // ...
}

public class Response<T> {
    public string? Message { get; set; }
    public int Status { get; set; }
    public T TodoList { get; set; }
    public Dictionary<string, List<string>> Errors { get; set; }
}
```

#### Modify the property names

If the [JSON](https://en.wikipedia.org/wiki/JSON) data doesn't match the properties of the [C#](https://dotnet.microsoft.com/en-us/languages/csharp#:~:text=C%23%20is%20a%20modern%2C%20innovative,5%20programming%20languages%20on%20GitHub.) object, you can use the `JsonPropertyName` attribute to align them.

```cs
using System.Text.Json.Serialization;

public class Todo {
    [JsonPropertyName("userId")]
    public int User { get; set; }
    [JsonPropertyName("todoId")] 
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }
}
```

> Note that the `System.Text.Json.Serialization` namespace does not need to be installed.
> 
## Configuring properties of the `HttpClient` object

#### Base URL

```cs
var client = new HttpClient {
    BaseAddress = new System.Uri("https://jsonplaceholder.typicode.com/todos/")
};

HttpService httpService = new(client);

class HttpService(HttpClient http) {}
```

In these examples, `HttpClient` is used as a dependency on another class, a topic we will delve into later.

```cs
class HttpService {
    private readonly HttpClient _http;

    public HttpService(HttpClient http, Uri baseUrl) {
        _http = http;
        _http.BaseAddress = baseUrl;
    }
}
```

#### Redirection

```cs
var handler = new HttpClientHandler {
    AllowAutoRedirect = false
};

HttpClient client = new(handler);
```

#### Headers

```cs
using System.Net.Http.Headers;

var mediaType = new MediaTypeWithQualityHeaderValue("application/json");

HttpClient client = new();

client.DefaultRequestHeaders.Add("message", "Hello, World!");
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept(mediaType);
```

> Note that the `System.Net.Http.Headers` namespace did not need to be installed.

## [HTTP request methods](https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods)

#### GET

```cs
public class HttpService(
    HttpClient client
) {
    public async Task<T?> InvokeGet<T>(string relativeUrl) {
        return await client.GetFromJsonAsync<T>(relativeUrl);
    }
}
```

`GetFromJsonAsync` exclusively retrieves the body of the response, whereas `GetAsync` fetches additional information beyond just the body.

#### POST

```cs
public class HttpService(
    HttpClient client
) { 
    public async Task<T?> InvokePost<T>(string relativeUrl, T data) {
        var response = await client.PostAsJsonAsync(relativeUrl, data);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }
}
```

#### PUT

```cs
public class HttpService(
    HttpClient client
) {
    public async Task InvokePut<T>(string relativeUrl, T data) {
        var response = await client.PutAsJsonAsync(relativeUrl, data);
        response.EnsureSuccessStatusCode();
    }
}
```

#### DELETE

```cs
public class HttpService(
    HttpClient client
) {
    public async Task InvokeDelete(string relativeUrl) {
        var response = await client.DeleteAsync(relativeUrl);
        response.EnsureSuccessStatusCode();
    }
}
```

## [HttpRequestMessage](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httprequestmessage?view=net-8.0) and `SendAsync`

#### Example

```cs
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
```

#### Example

```cs
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
```

## Handling errors

```cs
public class HttpService(
    HttpClient client
) {
    // public async Task<T?> InvokeGet(string url) {
    public async Task<T> InvokeGet<T>(string url) {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await client.SendAsync(request);
        
        if (httpResponse.IsSuccessStatusCode == false) {
            var errorJson = await httpResponse.Content.ReadAsStringAsync();
            // Console.WriteLine(errorJson);
            // return null;
            throw new HttpRequestException(errorJson);
        }  

        return await response.Content.ReadFromJsonAsync<T>();
    }
}
```

Some commented lines were left because one approach is to return `null` in the absence of data, but in this case it was chosen to launch an exection and handle it in the client code of this [API](https://en.wikipedia.org/wiki/API) (this term was previously used to refer to a [Web API](https://en.wikipedia.org/wiki/Web_API), but in this case, it denotes a broader [Application Programming Interface](https://en.wikipedia.org/wiki/API)). 

```cs
public class HttpService(
    HttpClient client
) {   
    public async Task<T?> InvokePost<T>(string url, T data) {
        var response = await client.PostAsJsonAsync(url, data);
        
        // if (response.StatusCode != System.Net.HttpStatusCode.OK) {}
        if (response.IsSuccessStatusCode == false) {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(error);
        }

        return await response.Content.ReadFromJsonAsync<T>();
    }
}
```

#### Encapsulating functionality 

```cs
public class HttpService(
    HttpClient client
) {
    public async Task<T?> InvokeGet<T>(string url) {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await client.SendAsync(request);
        await HandlePotentialError(response);
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T?> InvokePost<T>(string url, T data) {
        var response = await client.PostAsJsonAsync(url, data);
        await HandlePotentialError(response);
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task InvokePut<T>(string url, T data) {
        var response = await client.PutAsJsonAsync(url, data);
        await HandlePotentialError(response);
    }

    public async Task InvokeDelete(string url) {
        var response = await client.DeleteAsync(url);
        await HandlePotentialError(response);
    }

    private async Task HandlePotentialError(HttpResponseMessage httpResponse) {
        // response.EnsureSuccessStatusCode();
        if (httpResponse.IsSuccessStatusCode == false) {
            var errorJson = await httpResponse.Content.ReadAsStringAsync();
            throw new HttpRequestException(errorJson);
        }  
    }
}
```

#### Custom error

You can create your custom `Exception`

```cs
public class WebApiException(string errorJson) : Exception {
    public ErrorResponse? ErrorResponse { get; } = 
        JsonSerializer.Deserialize<ErrorResponse>(errorJson);
}

public class HttpService(
    HttpClient client
) {
    private async Task HandlePotentialError(HttpResponseMessage httpResponse) {
        if (httpResponse.IsSuccessStatusCode == false) {
            var errorJson = await httpResponse.Content.ReadAsStringAsync();
            throw new WebApiException(errorJson);
        }  
    }
}
```

## Using [Dependency injection (DI)](https://en.wikipedia.org/wiki/Dependency_injection)

#### Manual approach

```cs
using Microsoft.Extensions.Http;
using System.Net.Http.Json;

HttpClient client = new();
TodoService todoService = new(client);

try {
    var todos = await todoService.GetTodos();

    foreach (var todo in todos) {
        Console.WriteLine($"Todo Id: {data.Id}");
        Console.WriteLine($"User Id: {data.UserId}");
        Console.WriteLine($"Todo Title: {data.Title}");
        Console.WriteLine($"Todo Completed: {data.Completed}");
    }
} catch (HttpRequestException e) {
    Console.WriteLine("\nException Caught!");
    Console.WriteLine("Message :{0} ", e.Message);
}

public class Todo {
   // The Todo class defined earlier...
}

public class TodoService(HttpClient client) {
    public async Task<IEnumerable<Todo>> GetTodos() {
        var stringUrl = "https://jsonplaceholder.typicode.com/todos/";
        return await client.GetFromJsonAsync<IEnumerable<Todo>>(stringUrl);
    }
}
```

#### Using [.NET dependency injection system](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)

```cs
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
        Console.WriteLine($"Todo Id: {data.Id}");
        Console.WriteLine($"User Id: {data.UserId}");
        Console.WriteLine($"Todo Title: {data.Title}");
        Console.WriteLine($"Todo Completed: {data.Completed}");
    }
} catch (HttpRequestException e) {
    Console.WriteLine("\nException Caught!");
    Console.WriteLine("Message :{0} ", e.Message);
}

public class Todo  {
    // The Todo class defined earlier...
}

public class TodoService(HttpClient client) {
    public async Task<IEnumerable<Todo>> GetTodos() {
        var stringUrl = "https://jsonplaceholder.typicode.com/todos/";
        return await client.GetFromJsonAsync<IEnumerable<Todo>>(stringUrl);
    }
}
```

## Implement [resilient HTTP requests](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests)

In this section, we're utilizing concepts such as [dependency injection (DI)](https://en.wikipedia.org/wiki/Dependency_injection) and the [principle of dependency inversion](https://en.wikipedia.org/wiki/Dependency_inversion_principle), [IHttpClientFactory](https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory), and the [.NET dependency injection system](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection). These ideas won't be explained in detail; they are mentioned for informational purposes only.

```cs
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
        Console.WriteLine($"Todo Id: {data.Id}");
        Console.WriteLine($"User Id: {data.UserId}");
        Console.WriteLine($"Todo Title: {data.Title}");
        Console.WriteLine($"Todo Completed: {data.Completed}");
    }
} catch (HttpRequestException e) {
    Console.WriteLine("\nException Caught!");
    Console.WriteLine("Message :{0} ", e.Message);
}

public class Todo {
   // The Todo class defined earlier...
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
```

## [ASP.NET](https://dotnet.microsoft.com/en-us/apps/aspnet)

In this section, we won't be discussing the [ASP.NET](https://dotnet.microsoft.com/en-us/apps/aspnet) ecosystem. 

Now, to work, we need to create a [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) Web Assembly project project using the following commands:

```sh
dotnet new blazorwasm-empty -o WebApp
cd WebApp/
dotnet restore
```

#### Register

In [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) Web Assembly project, 
there's no need to register `HTTPClient` in the dependency container; 
it comes pre-configured by default. However, for other types of applications, manual configuration may be necessary.

Steps to follow:

1. You need to install the package
2. You have to add to the dependency container 

```cs
// Adding to the HttpClient to the dependency container 
builder.Services.AddHttpClient(client => {
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
});
```

#### Using `HttpClient` within a Blazor code-inline component:

```tsx
@page "/"

@inject HttpClient Http

<h1>Todos</h1>

<ul>
    @foreach (var todo in _todos) {
        if(todo.Completed) {
            <li style="color: green;">@todo.Title</li>
        } else {
            <li style="color: red;">@todo.Title</li>
        }
    }
</ul>

@code {
    private List<Todo> _todos = new();

    protected override async Task OnInitializedAsync() {
        try {
            var stringUrl = "https://jsonplaceholder.typicode.com/todos/";
            _todos = await Http.GetFromJsonAsync<List<Todo>>(stringUrl);
        } catch (HttpRequestException e) {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }

    class Todo {
        // The Todo class defined earlier...
    }
}
```

#### Using `HttpClient` within a Blazor code-behind component:

`Index.razor` file:

```tsx
@page "/"

<h1>Todos</h1>

<ul>
    @foreach (var todo in _todos) {
        if(todo.Completed) {
            <li style="color: green;">@todo.Title</li>
        } else {
            <li style="color: red;">@todo.Title</li>
        }
    }
</ul>
```

`Index.razor.cs` file:

```cs
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
```

#### Using `HttpClient` within a service

First, create the service:

```cs
public interface IHttpService {
    Task<T> GetData<T>(string url);
}

public class HttpService : IHttpService {
    private readonly HttpClient _client;

    public HttpService(HttpClient client) {
        _client = client;
    }

    public async Task<T> GetData<T>(string url) {
        return await _client.GetFromJsonAsync<T>(url);
    }
}
```

Next, register the service in the dependency container:

```cs
builder.Services.AddScoped<IDataService, DataService>();
```

Finally, use it within a component:

```tsx
@page "/"
@inject WebApp.Services.IDataService DataService

<h1>Todos</h1>

<ul>
    @foreach (var todo in _todos) {
        if(todo.Completed) {
            <li style="color: green;">@todo.Title</li>
        } else {
            <li style="color: red;">@todo.Title</li>
        }
    }
</ul>

@code {
    private List<Todo> _todos = new();

    protected override async Task OnInitializedAsync() {        
        var stringUrl = "https://jsonplaceholder.typicode.com/todos/";
        _todos = await DataService.GetData<List<Todo>>(stringUrl);
    }

    class Todo {
        // The Todo class defined earlier...
    }
}
```

## Conclusion

In conclusion, we've demonstrated how to make [HTTP requests](https://developer.mozilla.org/en-US/docs/Web/HTTP/Overview) in [.NET](https://dotnet.microsoft.com/en-us/), covering various aspects that might initially seem unrelated. However, understanding these facets enriches our comprehension of how to effectively leverage these concepts to enhance our real-world projects.