using ElasticSearch.Domain.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticSearch.Domain.IApplicationService
{
    public interface ISearchApplicationService
    {
        Task<List<ProductViewModel>> Find(FilterProductViewModel filter);
        Task ReIndex();
        Task ReIndexMany();
        Task ReIndexBulkAsync();
        Task ReIndexUpdate();
    }
}
