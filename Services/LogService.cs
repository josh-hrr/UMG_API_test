using UMG_API.Repositories;

namespace UMG_API.Services
{
    public class LogService
    {
        private readonly LogRepository _repository = new LogRepository();

        public void Registrar(int? userId, string accion, string modulo, string descripcion)
        {
            try
            {
                _repository.Registrar(userId, accion, modulo, descripcion);
            }
            catch
            {
                // Un fallo al escribir el log NUNCA debe interrumpir la operación principal
                // (ej. que falle el registro de log no debe impedir que la reserva se cree).
                // Si lo necesitas, aquí se podría escribir a un archivo local como respaldo.
            }
        }
    }
}