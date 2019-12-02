using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticSearch.Domain.IApplicationService;
using ElasticSearch.Domain.IRepository;
using ElasticSearch.Domain.ViewModel;
using Nest;

namespace ElasticSearch.ApplicationService.SearchService
{
    public class SearchApplicationService : ISearchApplicationService
    {
        private readonly IProductRepository productRepository;
        private readonly IElasticClient elasticClient;

        public SearchApplicationService(IProductRepository productRepository, IElasticClient elasticClient)
        {
            this.productRepository = productRepository;
            this.elasticClient = elasticClient;
        }

        public async Task<List<ProductViewModel>> Find(FilterProductViewModel filter)
        {
            var response = await elasticClient.SearchAsync<ProductViewModel>(
                  s => s.Query(q => q.Term(t => t.CategoryName, filter.Name)));

            var result = response.Documents.ToList();

            return result;
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
    }
}
