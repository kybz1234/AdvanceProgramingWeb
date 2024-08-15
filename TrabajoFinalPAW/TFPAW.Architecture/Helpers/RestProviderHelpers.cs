using System.Text;
using TFPAW.Architecture;

public static class RestProviderFactory
{
    public static IRestProvider CreateRestProvider()
    {
        return new RestProvider();   //  Factory Method Pattern
    }
}

public static class RestProviderHelpers
{

    // Patrón: Helper/Utility Class
    public static HttpClient CreateHttpClient(string baseUrl)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl),
            Timeout = TimeSpan.FromSeconds(30) // Ajusta el tiempo de espera según sea necesario
        };
        return httpClient;
    }


    public static HttpContent CreateContent(string content)
    {
        return new StringContent(content, Encoding.UTF8, "application/json");
    }

    public static async Task<string> GetResponse(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public static Exception ThrowError(string endpoint, Exception ex)
    {
        return new Exception($"Error en la solicitud a {endpoint}: {ex.Message}", ex);
    }
}
