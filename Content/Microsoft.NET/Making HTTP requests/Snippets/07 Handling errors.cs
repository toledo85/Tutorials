using Microsoft.Extensions.Http;
using System.Net.Http.Json;

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
        if (httpResponse.IsSuccessStatusCode == false) {
            var errorJson = await httpResponse.Content.ReadAsStringAsync();
            // throw new WebApiException(errorJson);
            throw new HttpRequestException(errorJson);
        }  
    }
}

// public class WebApiException(string errorJson) : Exception {
//     public ErrorResponse? ErrorResponse { get; } = 
//         JsonSerializer.Deserialize<ErrorResponse>(errorJson);
// }