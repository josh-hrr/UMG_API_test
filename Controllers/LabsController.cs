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

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var labs = _service.ObtenerTodos();
            return Content(HttpStatusCode.OK, labs);
        }

        // PUT api/labs/{id}
        [HttpPut]
        [Route("{id:int}")]

        public IHttpActionResult Put(int id, [FromBody] LabUpdateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            try
            {
                _service.ActualizarLaboratorio(id, dto.UMG_Nombre, dto.UMG_Estado);
                return Content(HttpStatusCode.OK, new { mensaje = "Laboratorio actualizado correctamente." });
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