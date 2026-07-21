using System;
using System.Collections.Generic;
using UMG_API.Models;
using UMG_API.Models.DTO;
using UMG_API.Repositories;

namespace UMG_API.Services
{
    public class LabService
    {
        private readonly LabRepository _repository = new LabRepository();
        private readonly LogService _logService = new LogService();
        public List<LabListDto> ObtenerTodos()
        {
            return _repository.ObtenerTodos();
        }

        public void ActualizarLaboratorio(int id, string nombre, int estado)
        {
            if (!_repository.Existe(id))
            {
                throw new ArgumentException("El laboratorio especificado no existe.");
            }

            if (string.IsNullOrWhiteSpace(nombre.ToUpper()))
            {
                throw new ArgumentException("El nombre del laboratorio es obligatorio.");
            }

            if (nombre.Length > 30)
            {
                throw new ArgumentException("El nombre del laboratorio no puede superar los 30 caracteres.");
            }

            if (estado != 0 && estado != 1)
            {
                throw new ArgumentException("El estado debe ser 0 (inactivo) o 1 (activo).");
            }

            if (_repository.ExisteNombreEnOtro(nombre.Trim().ToUpper(), id))
            {
                throw new InvalidOperationException($"Ya existe otro laboratorio con el nombre '{nombre.ToUpper()}'.");
            }

            bool exito = _repository.Actualizar(id, nombre.Trim(), estado);

            if (!exito)
            {
                throw new InvalidOperationException("No se pudo actualizar el laboratorio.");
            }

            string accion = estado == 0 ? "inactivado" : "activado";
            _logService.Registrar(null, "EDITAR_LABORATORIO", "Laboratorios",
                $"Se actualizó el laboratorio con ID {id} (nombre: '{nombre}', {accion}).");
        }

        public Lab CrearLaboratorio(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del laboratorio es obligatorio.");
            }

            if (nombre.Length > 30)
            {
                throw new ArgumentException("El nombre del laboratorio no puede superar los 30 caracteres.");
            }

            if (_repository.ExisteNombre(nombre.Trim().ToUpper()))
            {
                throw new InvalidOperationException($"Ya existe un laboratorio con el nombre '{nombre.ToUpper()}'.");
            }

            var lab = _repository.Crear(nombre.Trim().ToUpper());

            _logService.Registrar(
                null, // no hay usuario autenticado en este flujo todavía
                "CREAR_LABORATORIO",
                "Laboratorios",
                $"Se registró el laboratorio '{lab.UMG_Nombre.ToUpper()}' con ID {lab.UMG_ID}."
            );

            return lab;
        }
    }
}