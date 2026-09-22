using BibliotecaWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Obra> Obras { get; set; }
    }
}