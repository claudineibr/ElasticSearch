using ElasticSearch.Domain.Classes;
using ElasticSearch.Domain.ViewModel;
using System.Threading.Tasks;

namespace ElasticSearch.Domain.IApplicationService
{
    public interface IMeuServicoApplicationService
    {
        Task<MeuServicoViewModel> GetMeuServico(int id);
    }
}
