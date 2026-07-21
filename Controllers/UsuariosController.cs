using System;
using System.Net;
using System.Web.Http;
using UMG_API.Models.DTO;
using UMG_API.Services;

namespace UMG_API.Controllers
{
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : ApiController
    {
        private readonly UsuarioService _service = new UsuarioService();

        // POST api/usuarios
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] UsuarioCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            try
            {
                var usuario = _service.CrearUsuario(
                    dto.UMG_Usuario, dto.UMG_Contrasena, dto.UMG_Nombre, dto.UMG_Apellido, dto.UMG_Rol_ID);

                return Content(HttpStatusCode.Created, usuario);
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