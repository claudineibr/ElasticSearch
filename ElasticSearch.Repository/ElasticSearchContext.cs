using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ElasticSearch.Repository
{
    public class ElasticSearchContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        public ElasticSearchContext(DbContextOptions<ElasticSearchContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            this._loggerFactory = loggerFactory;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigEntity(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(this._loggerFactory);
        }

        private void ConfigEntity(ModelBuilder modelBuilder)
        {
        }
    }
}
