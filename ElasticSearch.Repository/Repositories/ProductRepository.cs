using ElasticSearch.Domain.IRepository;
using ElasticSearch.Domain.ViewModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ElasticSearch.Repository.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public List<ProductViewModel> GetAll()
        {
            return JsonConvert.DeserializeObject<List<ProductViewModel>>(File.ReadAllText("./products.json"));
        }
    }
}
