using System.Net.Http.Json;

namespace WebApp.Services;

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