using Microsoft.Extensions.Http;
using System.Net.Http.Json;

public class HttpService(
    HttpClient client
) {
    public async Task<T?> InvokeGet<T>(string relativeUrl) {
        return await client.GetFromJsonAsync<T>(relativeUrl);
    }

    public async Task<T?> InvokePost<T>(string relativeUrl, T data) {
        var response = await client.PostAsJsonAsync(relativeUrl, data);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task InvokePut<T>(string relativeUrl, T data) {
        var response = await client.PutAsJsonAsync(relativeUrl, data);
        response.EnsureSuccessStatusCode();
    }

    public async Task InvokeDelete(string relativeUrl) {
        var response = await client.DeleteAsync(relativeUrl);
        response.EnsureSuccessStatusCode();
    }
}