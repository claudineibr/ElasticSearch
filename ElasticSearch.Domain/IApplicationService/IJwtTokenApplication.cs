using ElasticSearch.Domain.ViewModel;

namespace ElasticSearch.Domain.IApplicationService
{
    public interface IJwtTokenApplication
    {
        JwtTokenViewModel BuildToken(BuildTokenViewModel buildToken);
    }
}
