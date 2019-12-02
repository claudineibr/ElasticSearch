using ElasticSearch.Domain.ViewModel;
using System.Collections.Generic;

namespace ElasticSearch.Domain.IRepository
{
    public interface IProductRepository
    {
        List<ProductViewModel> GetAll();
    }
}
