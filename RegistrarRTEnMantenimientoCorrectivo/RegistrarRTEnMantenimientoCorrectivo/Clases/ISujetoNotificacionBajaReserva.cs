using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public interface ISujetoNotificacionBajaReserva
    {
        void notificar();

        void suscribir(IObserverNotificacionBajaReserva observerNBR);

        void quitar(IObserverNotificacionBajaReserva observerNBR);

    }
}
