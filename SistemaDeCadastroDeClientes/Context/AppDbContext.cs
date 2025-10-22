using Microsoft.EntityFrameworkCore;
using SistemaDeCadastroDeClientes.Models;

namespace SistemaDeCadastroDeClientes.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Fornecedor> Fornecedores { get; set; }

    }
}
