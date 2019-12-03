using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace ElasticSearch.WebApi.Utilities
{
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];
            var userName = configuration["elasticsearch:user"];
            var password = configuration["elasticsearch:password"];
            var cloudId = configuration["elasticsearch:cloudId"];

            var credentials = new BasicAuthenticationCredentials(userName, password);
            var pool = new CloudConnectionPool(cloudId, credentials);

            var settings = new ConnectionSettings(pool)
                .DefaultIndex(defaultIndex)
                .EnableDebugMode();


            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }
    }
}
