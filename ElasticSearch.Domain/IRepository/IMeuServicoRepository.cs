using ElasticSearch.Domain.Classes;

namespace ElasticSearch.Domain.IRepository
{
    public interface IMeuServicoRepository
    {
        MeuServico GetMeuServico(int id);
    }
}
