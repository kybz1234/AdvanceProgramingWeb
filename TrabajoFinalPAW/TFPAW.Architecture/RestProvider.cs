using TFPAW.Architecture.Helpers;

namespace TFPAW.Architecture
{
    public interface IRestProvider
    {
        Task<string> DeleteAsync(string endpoint, string id);
        Task<string> GetAsync(string endpoint, string? id);
        Task<string> PostAsync(string endpoint, string content);
        Task<string> PutAsync(string endpoint, string requestUri, string content);
    }

    // SOLID: SRP - Single Responsibility Principle
    public class RestProvider : IRestProvider
    {
        public async Task<string> GetAsync(string endpoint, string? id)
        {
            try
            {
                var response = await RestProviderHelpers.CreateHttpClient(endpoint)
                    .GetAsync(id);
                return await RestProviderHelpers.GetResponse(response);
            }
            catch (Exception ex)
            {
                throw RestProviderHelpers.ThrowError(endpoint, ex);
            }
        }

        public async Task<string> PostAsync(string endpoint, string content)
        {
            try
            {
                var response = await RestProviderHelpers.CreateHttpClient(endpoint)
                    .PostAsync(endpoint, RestProviderHelpers.CreateContent(content));
                var result = await RestProviderHelpers.GetResponse(response);
                return result;
            }
            catch (Exception ex)
            {
                throw RestProviderHelpers.ThrowError(endpoint, ex);
            }
        }

        public async Task<string> PutAsync(string endpoint, string id, string content)
        {
            try
            {
                var response = await RestProviderHelpers.CreateHttpClient(endpoint)
                    .PutAsync(id, RestProviderHelpers.CreateContent(content));
                var result = await RestProviderHelpers.GetResponse(response);
                return result;
            }
            catch (Exception ex)
            {
                throw RestProviderHelpers.ThrowError(endpoint, ex);
            }
        }

        public async Task<string> DeleteAsync(string endpoint, string id)
        {
            try
            {
                var response = await RestProviderHelpers.CreateHttpClient(endpoint)
                    .DeleteAsync(id);
                var result = await RestProviderHelpers.GetResponse(response);
                return result;
            }
            catch (Exception ex)
            {
                throw RestProviderHelpers.ThrowError(endpoint, ex);
            }
        }
    }
}
