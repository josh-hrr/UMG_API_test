using System;
using System.Net;
using System.Web.Http;
using UMG_API.Models.DTO;
using UMG_API.Services;

namespace UMG_API.Controllers
{
    [RoutePrefix("api/reservas")]
    public class ReservasController : ApiController
    {
        private readonly ReservaService _service = new ReservaService();

        // POST api/reservas
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] ReservaCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            try
            {
                var reserva = _service.CrearReserva(
                    dto.UMG_User_ID, dto.UMG_Lab_ID, dto.UMG_Fecha_Reserva,
                    dto.UMG_Hora_Inicio, dto.UMG_Hora_Fin, dto.UMG_Motivo);

                return Content(HttpStatusCode.Created, reserva);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.Conflict, new { mensaje = ex.Message });
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}