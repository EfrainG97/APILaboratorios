using APILaboratorios.Data;
using APILaboratorios.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace APILaboratorios.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]

    public class ReservasController : ControllerBase
    {
        private readonly ReservasContext _context;

        public ReservasController(ReservasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Reservas.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personaje = await _context.Reservas
                .FirstOrDefaultAsync(m => m.IDReserva == id);
            if (personaje == null)
            {
                return NotFound();
            }

            return Ok(personaje);
        }

        [HttpPost]
        public async Task<IActionResult> InsertReserva(Reservas reservas)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC InsertarReserva @p0, @p1, @p2, @p3",
                    reservas.IDUsuario,
                    reservas.IDLaboratorio,
                    reservas.FechaReserva,
                    reservas.Estado ?? ""
                );

                return Ok(new { mensaje = "Reserva insertada correctamente" });
            }
            catch (Exception ex)
            {
                if (ex is Microsoft.Data.SqlClient.SqlException sqlEx)
                {
                    Console.Error.WriteLine($"SQL Error Number: {sqlEx.Number}");
                    Console.Error.WriteLine($"SQL Error Message: {sqlEx.Message}");
                    return BadRequest(new { error = sqlEx.Message, sqlErrorNumber = sqlEx.Number });
                }
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarReserva(int id)
        {
            var sql = "DELETE FROM Reservas WHERE IDReserva = @id";

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;

                    var param = command.CreateParameter();
                    param.ParameterName = "@id";
                    param.Value = id;
                    command.Parameters.Add(param);

                    var result = await command.ExecuteNonQueryAsync();

                    if (result == 0)
                        return NotFound();
                }
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReserva(int id, Reservas reserva)
        {
            if (id != reserva.IDReserva)
                return BadRequest(new { error = "El ID de la reserva no coincide con el parámetro." });

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC ActualizarReserva @p0, @p1, @p2",
                    reserva.IDReserva,
                    reserva.FechaReserva,
                    reserva.Estado ?? ""
                );

                return Ok(new { mensaje = "Reserva actualizada correctamente" });
            }
            catch (Exception ex)
            {
                if (ex is Microsoft.Data.SqlClient.SqlException sqlEx)
                {
                    Console.Error.WriteLine($"SQL Error Number: {sqlEx.Number}");
                    Console.Error.WriteLine($"SQL Error Message: {sqlEx.Message}");
                    return BadRequest(new { error = sqlEx.Message, sqlErrorNumber = sqlEx.Number });
                }
                return BadRequest(new { error = ex.Message });
            }
        }



    }
}
