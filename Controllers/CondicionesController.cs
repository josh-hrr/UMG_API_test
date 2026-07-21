using System;
using System.Net;
using System.Web.Http;
using UMG_API.Models.DTO;
using UMG_API.Services;

namespace UMG_API.Controllers
{
    [RoutePrefix("api/condiciones")]
    public class CondicionesController : ApiController
    {
        private readonly CondicionService _service = new CondicionService();

        // POST api/condiciones
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] CondicionCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            try
            {
                var condicion = _service.CrearCondicion(
                    dto.UMG_Lab_ID, dto.UMG_Fecha, dto.UMG_Hora_Inicio,
                    dto.UMG_Hora_Fin, dto.UMG_Tipo, dto.UMG_Motivo);

                return Content(HttpStatusCode.Created, condicion);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}