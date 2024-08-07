using System.Net.Http.Headers;
using System.Text;

namespace TFPAW.Architecture.Helpers;

internal static class HttpProviderExtentions
{
    private static void AddDefaultRequestHeader(this HttpClient client, string name, string value)
    {
        var defaultHeaders = client.DefaultRequestHeaders;
        if (defaultHeaders.Contains(name))
            defaultHeaders.Remove(name);
        defaultHeaders.Add(name, value);
    }
}

// DESIGN_PATTERN: Factory Method

public static class RestProviderHelpers
{
    public static HttpClient CreateHttpClient(string endpoint)
    {
        var client = new HttpClient { BaseAddress = new Uri(endpoint) };
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }

    // Método de fábrica para crear contenido JSON
    public static StringContent CreateContent(string content) => new(content, Encoding.UTF8, "application/json");

    public static async Task<string> GetResponse(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public static Exception ThrowError(string endpoint, Exception ex) => new ApplicationException($"Error getting data from {endpoint}", ex);
}
