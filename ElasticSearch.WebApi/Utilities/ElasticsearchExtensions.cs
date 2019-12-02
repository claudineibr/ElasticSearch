using Elasticsearch.Net;
using ElasticSearch.Domain.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

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
                .DefaultMappingFor<ProductViewModel>(m => m
                    .Ignore(p => p.CategoryName)
                ).EnableDebugMode();


            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }
    }
}
