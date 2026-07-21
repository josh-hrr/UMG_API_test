using System;
using UMG_API.Models;
using UMG_API.Repositories;

namespace UMG_API.Services
{
    public class ReservaService
    {
        private readonly ReservaRepository _repository = new ReservaRepository();
        private readonly LogService _logService = new LogService();

        public Reserva CrearReserva(int userId, int labId, DateTime fecha, TimeSpan horaInicio,
                                     TimeSpan horaFin, string motivo)
        {
            // RN-02: no fechas pasadas
            if (fecha.Date < DateTime.Today)
            {
                throw new ArgumentException("No se puede crear una reserva para una fecha pasada.");
            }

            if (horaInicio >= horaFin)
            {
                throw new ArgumentException("La hora de inicio debe ser menor a la hora de fin.");
            }

            // RN-07: motivo obligatorio
            if (string.IsNullOrWhiteSpace(motivo))
            {
                throw new ArgumentException("El motivo de la reserva es obligatorio.");
            }

            // Validar existencia de usuario y laboratorio (RN-04 y FK)
            if (!_repository.ExisteUsuario(userId))
            {
                throw new ArgumentException("El docente especificado no existe o está inactivo.");
            }

            if (!_repository.ExisteLab(labId))
            {
                throw new ArgumentException("El laboratorio especificado no existe o está inactivo.");
            }

            // RN-01: anti-duplicidad / traslape de horario
            if (_repository.ExisteTraslape(labId, fecha, horaInicio, horaFin))
            {
                throw new InvalidOperationException(
                    "Ya existe una reserva activa para ese laboratorio que se traslapa con el horario solicitado.");
            }

            // Validar que no haya un bloqueo/condición (asueto, mantenimiento) en ese horario
            if (_repository.ExisteBloqueo(labId, fecha, horaInicio, horaFin))
            {
                throw new InvalidOperationException(
                    "El laboratorio no está disponible en ese horario debido a un bloqueo registrado (asueto, mantenimiento u otra actividad).");
            }


            var reserva = _repository.Crear(userId, labId, fecha, horaInicio, horaFin, motivo.Trim());

            _logService.Registrar(
                userId,
                "CREAR_RESERVA",
                "Reservas",
                $"El usuario {userId} reservó el laboratorio {labId} para el {fecha:yyyy-MM-dd} de {horaInicio} a {horaFin}. Motivo: {motivo}."
            );

            return reserva;

            // return _repository.Crear(userId, labId, fecha, horaInicio, horaFin, motivo.Trim());
        }
    }
}