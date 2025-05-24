using System.ComponentModel.DataAnnotations;

namespace APILaboratorios.Model
{
    public class Reservas
    {
        [Key]
        public int IDReserva { get; set; }
        public int IDUsuario { get; set; }
        public int IDLaboratorio { get; set; }
        public DateTime FechaReserva { get; set; }
        public string? Estado { get; set; }
    }
}
