using System;
using UMG_API.Models.DTO;
using UMG_API.Repositories;

namespace UMG_API.Services
{
    public class AuthService
    {
        private readonly UsuarioRepository _repository = new UsuarioRepository();
        private readonly LogService _logService = new LogService();

        public LoginResponseDto Login(string correo, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
            {
                throw new ArgumentException("Debe ingresar usuario y contraseña.");
            }

            var usuario = _repository.ValidarCredenciales(correo.Trim().ToLower(), contrasena);

            if (usuario == null)
            {
                _logService.Registrar(null, "LOGIN_FALLIDO", "Autenticación",
                    $"Intento de inicio de sesión fallido para el correo '{correo}'.");

                throw new InvalidOperationException("Usuario o contraseña incorrectos.");
            }

            _repository.ActualizarUltimoAcceso(usuario.UMG_ID);

            _logService.Registrar(usuario.UMG_ID, "LOGIN", "Autenticación",
                $"El usuario '{usuario.UMG_Usuario}' inició sesión correctamente.");

            return usuario;
        }
    }
}