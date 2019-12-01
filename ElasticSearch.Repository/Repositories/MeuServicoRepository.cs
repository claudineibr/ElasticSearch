using ElasticSearch.Domain.Classes;
using ElasticSearch.Domain.IRepository;
using System;

namespace ElasticSearch.Repository.Repositories
{
    public class MeuServicoRepository : IMeuServicoRepository
    {
        private readonly ElasticSearchContext context;

        public MeuServicoRepository(ElasticSearchContext _contex)
        {
            this.context = _contex;
        }

        public MeuServico GetMeuServico(int id)
        {
            throw new NotImplementedException();
        }
    }
}
