using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using ElasticSearch.ApplicationService;
using ElasticSearch.Domain.IApplicationService;
using ElasticSearch.Domain.IRepository;
using ElasticSearch.Domain.Utilities;
using ElasticSearch.Repository;
using ElasticSearch.Repository.Repositories;
using System;

namespace ElasticSearch.Test
{
    [TestClass]
    public class MeuServicoApplicationUnitTest
    {
        private IMeuServicoApplicationService meuServico;

        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                var services = new ServiceCollection();
                services.AddDbContext<ElasticSearchContext>(option => option.UseMySql(Config.ConnectionString, mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new Version(5, 7, 17), ServerType.MySql); // replace with your Server Version and Type
                }));
                services.AddScoped<DbContext>(sp => sp.GetService<ElasticSearchContext>());
                services.AddTransient<IMeuServicoRepository, MeuServicoRepository>();
                services.AddTransient<IMeuServicoApplicationService, MeuServicoApplicationService>();

                var serviceProvider = services.BuildServiceProvider();

                meuServico = serviceProvider.GetService<IMeuServicoApplicationService>();

                var result = meuServico.GetMeuServico(1);

                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }    
        }
    }
}
