using Microsoft.Extensions.DependencyInjection;

namespace DeltaForce.OpenAI.DependencyInjection.Extentions.ServiceCollections
{
    public static class HttpClientServiceCollection
    {
        /// <summary>
        /// Microsoft's HttpClient Implementations for HttpClientFactory
        /// </summary>
        /// <param name="source"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClients(this IServiceCollection source, IDictionary<string, string> properties)
        {
            foreach(var property in properties.Keys)
            {
                source.AddHttpClient(property, http =>
                {
                    http.BaseAddress = new Uri(properties[property]);
                });
            }

            return source;
        }
    }
}
