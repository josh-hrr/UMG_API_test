using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UMG_API.Models;
using UMG_API.Models.DTO;
using UMG_API.Repositories;

namespace UMG_API.Services
{
    public class CondicionService
    {
        private readonly CondicionRepository _repository = new CondicionRepository();
        private readonly LogService _logService = new LogService();

        public List<CondicionListDto> ObtenerTodas()
        {
            return _repository.ObtenerTodas();
        }

        public void ActualizarCondicion(int id, int? labId, DateTime fecha, TimeSpan horaInicio,
                                  TimeSpan horaFin, string tipo, string motivo, int estado)
        {
            if (!_repository.Existe(id))
            {
                throw new ArgumentException("El bloqueo especificado no existe.");
            }

            if (horaInicio >= horaFin)
            {
                throw new ArgumentException("La hora de inicio debe ser menor a la hora de fin.");
            }

          
            if (string.IsNullOrWhiteSpace(motivo))
            {
                throw new ArgumentException("El motivo del bloqueo es obligatorio.");
            }

            if (estado != 0 && estado != 1)
            {
                throw new ArgumentException("El estado debe ser 0 (inactivo) o 1 (activo).");
            }

            if (labId.HasValue && !_repository.ExisteLab(labId.Value))
            {
                throw new ArgumentException("El laboratorio especificado no existe o está inactivo.");
            }

            bool exito = _repository.Actualizar(id, labId, fecha, horaInicio, horaFin, tipo.Trim().ToUpper(), motivo.Trim().ToUpper(), estado);

            if (!exito)
            {
                throw new InvalidOperationException("No se pudo actualizar el bloqueo.");
            }

            _logService.Registrar(null, "EDITAR_CONDICION", "Condiciones",
                $"Se actualizó el bloqueo con ID {id} (estado: {(estado == 1 ? "activo" : "inactivo")}).");
        }

        public void InactivarCondicion(int id)
        {
            if (!_repository.Existe(id))
            {
                throw new ArgumentException("El bloqueo especificado no existe.");
            }

            bool exito = _repository.Inactivar(id);

            if (!exito)
            {
                throw new InvalidOperationException("No se pudo inactivar el bloqueo.");
            }

            _logService.Registrar(null, "INACTIVAR_CONDICION", "Condiciones",
                $"Se inactivó el bloqueo con ID {id}.");
        }

        public Condicion CrearCondicion(int? labId, DateTime fecha, TimeSpan horaInicio,
                                         TimeSpan horaFin, string tipo, string motivo)
        {
            if (fecha.Date < DateTime.Today)
            {
                throw new ArgumentException("No se puede registrar un bloqueo para una fecha pasada.");
            }

            if (horaInicio >= horaFin)
            {
                throw new ArgumentException("La hora de inicio debe ser menor a la hora de fin.");
            }           

            if (string.IsNullOrWhiteSpace(motivo))
            {
                throw new ArgumentException("El motivo del bloqueo es obligatorio.");
            }

            // Si se especifica un laboratorio puntual, debe existir y estar activo
            if (labId.HasValue && !_repository.ExisteLab(labId.Value))
            {
                throw new ArgumentException("El laboratorio especificado no existe o está inactivo.");
            }

            var condicion = _repository.Crear(labId, fecha, horaInicio, horaFin, tipo.Trim().ToUpper(), motivo.Trim().ToUpper());

            _logService.Registrar(
                null,
                "CREAR_CONDICION",
                "Condiciones",
                $"Se registró un bloqueo tipo '{condicion.UMG_Tipo.ToUpper()}' para el {condicion.UMG_Fecha:yyyy-MM-dd}" +
                (labId.HasValue ? $" en el laboratorio {labId}." : " aplicable a todos los laboratorios.")
            );

            return condicion;
            //  return _repository.Crear(labId, fecha, horaInicio, horaFin, tipo.Trim(), motivo.Trim());
        }
    }

}