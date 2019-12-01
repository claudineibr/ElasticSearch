using NascorpLib.Security;
using ElasticSearch.Domain.IApplicationService;
using ElasticSearch.Domain.Utilities;
using ElasticSearch.Domain.ViewModel;
using System.Threading.Tasks;

namespace ElasticSearch.ApplicationService.LoginService
{
    public class LoginApplication : ILoginApplication
    {
        private readonly IJwtTokenApplication jwtToken;

        public LoginApplication(IJwtTokenApplication jwtToken)
        {
            this.jwtToken = jwtToken;
        }

        public async Task<LoginViewModel> Authentication(AuthenticationViewModel authentication)
        {
            var crypt = new Cryptography(Constants.KEYCRYPTOGRAPHY);
            var result = new LoginViewModel();
            result.ApiToken = jwtToken.BuildToken(new BuildTokenViewModel()
            {
                PersonId = crypt.Encrypt("PersonID"),
                PersonName = "Name"
            });

            return await Task.Run(() => result);
        }
    }
}
