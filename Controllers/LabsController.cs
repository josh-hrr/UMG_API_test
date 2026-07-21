using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UMG_API.Models.DTO;
using UMG_API.Services;

namespace UMG_API.Controllers
{
    [RoutePrefix("api/labs")]
    public class LabsController : ApiController
    {
        private readonly LabService _service = new LabService();

        // POST api/labs
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] LabCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            try
            {
                var lab = _service.CrearLaboratorio(dto.UMG_Nombre);
                return Content(HttpStatusCode.Created, lab);
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