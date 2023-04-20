using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public interface IObserverNotificacionBajaReserva
    {
        void enviarNotificacion(List<List<string>> contacto, string motivo, List<List<string>> turnos, string rec);
    }
}
