using System;
using UMG_API.Models;
using UMG_API.Repositories;

namespace UMG_API.Services
{
    public class LabService
    {
        private readonly LabRepository _repository = new LabRepository();
        private readonly LogService _logService = new LogService();

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