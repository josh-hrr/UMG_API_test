using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UMG_API.Models;
using UMG_API.Repositories;

namespace UMG_API.Services
{
    public class CondicionService
    {
        private readonly CondicionRepository _repository = new CondicionRepository();

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

            return _repository.Crear(labId, fecha, horaInicio, horaFin, tipo.Trim(), motivo.Trim());
        }
    }

}