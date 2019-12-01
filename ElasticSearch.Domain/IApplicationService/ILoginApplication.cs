using ElasticSearch.Domain.ViewModel;
using System.Threading.Tasks;

namespace ElasticSearch.Domain.IApplicationService
{
    public interface ILoginApplication
    {
        Task<LoginViewModel> Authentication(AuthenticationViewModel authentication);
    }
}
