﻿using ElasticSearch.Domain.IApplicationService;
using ElasticSearch.Domain.IRepository;
using ElasticSearch.Domain.ViewModel;
using Microsoft.Extensions.Configuration;
using Nest;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearch.ApplicationService.SearchService
{
    public class SearchApplicationService : ISearchApplicationService
    {
        private readonly IProductRepository productRepository;
        private readonly IElasticClient elasticClient;
        private readonly IConfiguration configuration;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public SearchApplicationService(IProductRepository productRepository,
                                        IElasticClient elasticClient,
                                        IConfiguration configuration)
        {
            this.productRepository = productRepository;
            this.elasticClient = elasticClient;
            this.configuration = configuration;
        }

        public async Task<List<ProductViewModel>> Find(FilterProductViewModel filter)
        {
            var response = await elasticClient.SearchAsync<ProductViewModel>(
                  s => s.Query(q => q.Term(t => t.Name, filter.Name)));

            return response.Documents.ToList();
        }

        public async Task ReIndex()
        {
            await elasticClient.DeleteByQueryAsync<ProductViewModel>(q => q.MatchAll());
            var products = productRepository.GetAll().ToArray();

            foreach (var product in products)
            {
                await elasticClient.IndexDocumentAsync(product);
            }
        }

        public async Task ReIndexMany()
        {
            await elasticClient.DeleteByQueryAsync<ProductViewModel>(q => q.MatchAll());
            var products = productRepository.GetAll().ToArray();
            var indexManyAsyncResponse = await elasticClient.IndexManyAsync(products);
            if (indexManyAsyncResponse.Errors)
            {
                foreach (var itemWithError in indexManyAsyncResponse.ItemsWithErrors)
                {
                    logger.Error("Failed to index document {0}: {1}", itemWithError.Id, itemWithError.Error);
                }
            }
        }

        public async Task ReIndexBulkAsync()
        {
            await elasticClient.DeleteByQueryAsync<ProductViewModel>(q => q.Index("products").MatchAll());
            var products = productRepository.GetAll().ToArray();
            await elasticClient.BulkAsync(b => b.Index("products").IndexMany(products));
        }

        public async Task ReIndexUpdate()
        {
            var products = productRepository.GetAll().ToArray();

            foreach (var product in products)
            {
                var any = await elasticClient.SearchAsync<ProductViewModel>(
                  s => s.Query(q => q.Term(t => t.ProductCode, product.ProductCode)));

                if (any.Documents.Count() == 0)
                {
                    await elasticClient.IndexDocumentAsync(product);
                    continue;
                }

                //await elasticClient.UpdateAsync(product);
            }
        }
    }
}