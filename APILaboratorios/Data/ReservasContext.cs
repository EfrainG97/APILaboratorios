using APILaboratorios.Model;
using Microsoft.EntityFrameworkCore;

namespace APILaboratorios.Data
{
    public class ReservasContext : DbContext
    {
        public ReservasContext(DbContextOptions<ReservasContext> options) : base(options) { }

        public DbSet<Reservas> Reservas { get; set; }
    }
}
