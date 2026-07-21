using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using UMG_API.Models;
using UMG_API.Repositories;

namespace UMG_API.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _repository = new UsuarioRepository();
        private readonly LogService _logService = new LogService();

        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public Usuario CrearUsuario(string correo, string contrasena, string nombre, string apellido, int rolId)
        {
            if (string.IsNullOrWhiteSpace(correo) || !EmailRegex.IsMatch(correo))
            {
                throw new ArgumentException("El correo electrónico no tiene un formato válido.");
            }

            if (string.IsNullOrWhiteSpace(contrasena) || contrasena.Length < 3)
            {
                throw new ArgumentException("La contraseña debe tener al menos 3 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido))
            {
                throw new ArgumentException("El nombre y apellido son obligatorios.");
            }

            if (!_repository.ExisteRol(rolId))
            {
                throw new ArgumentException("El rol especificado no existe o está inactivo.");
            }

            if (_repository.ExisteCorreo(correo.Trim().ToUpper()))
            {
                throw new InvalidOperationException($"Ya existe un usuario registrado con el correo '{correo}'.");
            }

            var usuario = _repository.Crear(correo.Trim().ToUpper(), contrasena, nombre.Trim().ToUpper(), apellido.Trim().ToUpper(), rolId);

            _logService.Registrar(
                usuario.UMG_ID,
                "CREAR_USUARIO",
                "Usuarios",
                $"Se registró el usuario '{usuario.UMG_Usuario}' con ID {usuario.UMG_ID}."
            );

            return usuario;



            //  return _repository.Crear(correo.Trim().ToUpper(), contrasena, nombre.Trim().ToUpper(), apellido.Trim().ToUpper(), rolId);
        }
    }

}