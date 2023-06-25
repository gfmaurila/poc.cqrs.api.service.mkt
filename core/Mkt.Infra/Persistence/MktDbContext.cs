using Microsoft.EntityFrameworkCore;
using Mkt.Core.Entities;
using System.Net;
using System.Reflection;

namespace Mkt.Infra.Persistence;

public class MktDbContext : DbContext
{
    public MktDbContext(DbContextOptions<MktDbContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Código para desabilitar a validação do certificado
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            // Configuração da string de conexão e outros parâmetros
            //optionsBuilder.UseSqlServer("YourConnectionString");
        }
    }

}
