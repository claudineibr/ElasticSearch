using ElasticSearch.Domain.IApplicationService;
using ElasticSearch.Domain.IRepository;
using ElasticSearch.Domain.ViewModel;
using System.Threading.Tasks;

namespace ElasticSearch.ApplicationService
{
    public class MeuServicoApplicationService : IMeuServicoApplicationService
    {
        private readonly IMeuServicoRepository meuServicoRepository;

        public MeuServicoApplicationService(IMeuServicoRepository _meuServicoRepository)
        {
            this.meuServicoRepository = _meuServicoRepository;
        }

        public async Task<MeuServicoViewModel> GetMeuServico(int id)
        {
            return new MeuServicoViewModel();
        }
    }
}
