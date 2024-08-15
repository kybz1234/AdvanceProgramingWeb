using System.Net.Http.Headers;
using System.Text;

namespace TFPAW.Architecture.Helpers;

// Patrón: Extension Method
internal static class HttpProviderExtentions
{
    // Método de extensión para agregar un encabezado predeterminado al HttpClient.
    private static void AddDefaultRequestHeader(this HttpClient client, string name, string value)
    {
        var defaultHeaders = client.DefaultRequestHeaders;
        if (defaultHeaders.Contains(name))
            defaultHeaders.Remove(name);
        defaultHeaders.Add(name, value);
    }
}
