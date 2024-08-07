using TFPAW.Architecture;

namespace TFPAW.Service;

public class OpenStreetMapService
{
    private readonly IRestProvider _restProvider;
    private const string BaseUrl = "https://api.openstreetmap.org";

    public OpenStreetMapService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<string> GetMapDataByBoundingBoxAsync(double minLon, double minLat, double maxLon, double maxLat)
    {
        var bbox = $"{minLon},{minLat},{maxLon},{maxLat}";
        var endpoint = $"{BaseUrl}/api/0.6/map?bbox={bbox}";
        return await _restProvider.GetAsync(endpoint, null);
    }

    public async Task<string> GetNodeDataAsync(long nodeId)
    {
        var endpoint = $"{BaseUrl}/api/0.6/node/{nodeId}";
        return await _restProvider.GetAsync(endpoint, null);
    }
}
