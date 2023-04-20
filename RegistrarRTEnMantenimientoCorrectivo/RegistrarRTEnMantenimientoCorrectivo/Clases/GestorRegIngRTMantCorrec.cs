using RegistrarRTEnMantenimientoCorrectivo.Formularios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class GestorRegIngRTMantCorrec: ISujetoNotificacionBajaReserva
    {
        private List<IObserverNotificacionBajaReserva> observadores = new List<IObserverNotificacionBajaReserva>();
        PersonalCientifico personalCientifico = new PersonalCientifico();
        AsignaciónResponsableTecnicoRT asignacionVigente = new AsignaciónResponsableTecnicoRT();
        List<List<string>> RTDisponiblesDatos = new List<List<string>>();
        List<RecursoTecnologico> RTDisponibles = new List<RecursoTecnologico>();
        public PersonalCientifico buscarCientifLog(Usuario user)
        {
            Sesion sesion = new Sesion();
            var ses = sesion.getSesion(user);
            personalCientifico = ses;

            return ses;
        }

        public void buscarRTCientif()
        {
            AsignaciónResponsableTecnicoRT asig = new AsignaciónResponsableTecnicoRT();

            asignacionVigente = asig.esAsignacionVigenteCientif(personalCientifico);

        }

        public List<List<string>> buscarRTDisponible()
        {
            AsignaciónResponsableTecnicoRT asig2 = new AsignaciónResponsableTecnicoRT();

            RTDisponibles = asig2.getRTDisponibles(asignacionVigente);

            RTDisponiblesDatos = asig2.getRTDisponibless(RTDisponibles);

            return RTDisponiblesDatos;
        }

        DateTime fechaSelec = DateTime.Now;
        public bool validarFechaIngresada(DateTime fecha)
        {
            var res = false;
            var fechaActual = DateTime.Now;
            if (fechaActual < fecha)
            {
                res = true;
                fechaSelec = fecha;

            }

            return res;
        }

        List<Turno> turnosCancelables = new List<Turno>();
        RecursoTecnologico recSeleccionado = new RecursoTecnologico();
        public List<List<string>> obtenerTurnosRTCancelables(int rtSeleccionado, DateTime fecha)
        {
            Turno turno = new Turno();
            RecursoTecnologico rec = new RecursoTecnologico();
            var fechaIngresada = fecha;
            //var turnosCancelables = new List<Turno>();
            
            recSeleccionado = rec.obtenerRT(rtSeleccionado, RTDisponibles);
            turnosCancelables = rec.getTurnosCancelablesEnPeriodo(recSeleccionado, fechaIngresada);

            var datosAMostrar = turno.getFechaHoraYCientifico(recSeleccionado, turnosCancelables);

            return datosAMostrar;
        }

        public List<Estado> buscarEstados()
        {
            var estados = new Estado();
            var lista = new List<Estado>();

            var estadoTurno = estados.esAmbitoTurnoYCanceladoMantCorrec();
            var estadoRT = estados.esAmbitoRTYMantCorrec();

            lista.Add(estadoRT);
            lista.Add(estadoTurno);

            return lista;

        }

        public void ingresarEnMantCorrec(string motivo, DateTime fecha, Estado estadoRTnuevo)
        {
            var newCambio = new CambioEstadoRT();
            var rt = new RecursoTecnologico();
            

            newCambio.setFechaFin(recSeleccionado);

            rt.newMantenimiento(recSeleccionado, motivo, fecha);
            rt.newCambioEstadoRT(recSeleccionado,estadoRTnuevo);

        }

        public void cancelarTurno(Estado estadoTurnoNuevo)
        {
            //var cambiarFechaFin = new CambioEstadoTurno();
            var cambiarTurno = new Turno();

            cambiarTurno.setFechaFin(recSeleccionado);

            cambiarTurno.newCambioEstadoTurno(estadoTurnoNuevo, recSeleccionado);
        }

        string motivoCorreo = "";
        public string getMotivo(string motivo)
        {
            motivoCorreo = motivo;
            return motivoCorreo;
        }

        public List<string> getTurnosCientifico(PersonalCientifico cientifico, DateTime fechaSelec, int id)
        {
            var sentenciaSql = $"SELECT t.fechaHoraInicio FROM Turno t JOIN AsignacionCientificoDelCI a ON (t.idAsignacion = a.idAsignacion) JOIN PersonalCientifico p ON (a.legajo = p.legajo) WHERE p.nombre = '{cientifico.Nombre}' AND p.apellido = '{cientifico.Apellido}' AND t.NumeroRT = {id}";
            var tabla = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);

            var lista = new List<string>();
            for (int i = 0; i < tabla.Rows.Count; i++)
            {
                var resultado = Convert.ToDateTime(tabla.Rows[i]["fechaHoraInicio"]);
                if (fechaSelec >= resultado)
                {
                    lista.Add(resultado.ToString());
                }
            }
            return lista;
        }
        public PersonalCientifico buscarCientificosConReserva(Turno item)
        {
            var numeroRT = recSeleccionado.NumeroRT;
            PersonalCientifico personalCient = new PersonalCientifico();
            var sentenciaSQL = $"SELECT p.nombre, p.apellido, p.correoElecPers, p.telCelular FROM PersonalCientifico p JOIN AsignacionCientificoDelCI a ON (p.legajo = a.legajo) JOIN Turno t ON (a.idAsignacion = t.idAsignacion) JOIN RecursoTecnologico r ON (r.numeroRT = t.numeroRT) WHERE r.numeroRT = {numeroRT} AND t.fechaHoraInicio = '{item.FechaHoraInicio.ToString("yyyy/MM/dd hh:mm:ss")}'";
            var tabla = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSQL);
            for (int i = 0; i < tabla.Rows.Count; i++)
            {
                PersonalCientifico personal = new PersonalCientifico();
                personal.Nombre = tabla.Rows[i]["nombre"].ToString();
                personal.Apellido = tabla.Rows[i]["apellido"].ToString();
                personal.CorreoElecPers = tabla.Rows[i]["correoElecPers"].ToString();
                personal.TelCelular = Convert.ToInt64(tabla.Rows[i]["telCelular"]);
                personalCient = personal;
            }

            return personalCient;
        }

        public void obtenerObservadores()
        {
            Mail mail = new Mail();
            this.suscribir(mail);
            this.notificar();
        }

        public void suscribir(IObserverNotificacionBajaReserva observer)
        {
            if (!observadores.Contains(observer))
            {
                this.observadores.Add(observer);
            }
            else
            {
                throw new Exception("No existe una suscripcion para agregar a ese observador");
            }
        }

        public void quitar(IObserverNotificacionBajaReserva observer)
        {
            if (observadores.Contains(observer))
            {
                observadores.Remove(observer);
            }
            else
            {
                throw new Exception("No existe una suscripcion para quitar a ese observador");
            }
        }

        public void notificar()
        {
            foreach (IObserverNotificacionBajaReserva observador in observadores)
            {
                var rec = recSeleccionado;
                var recurso = rec.TipoDeRT.Nombre;
                var motivo = motivoCorreo;
                var listaTurnos = turnosCancelables;
                var listaCientificos = new List<PersonalCientifico>();
                var cient = new PersonalCientifico();

                foreach (var item in listaTurnos)
                {
                    cient = this.buscarCientificosConReserva(item);
                    if (listaCientificos.Count != 0)
                    {
                        for (int i = 0; i < listaCientificos.Count; i++)
                        {
                            if (listaCientificos[i].CorreoElecPers != cient.CorreoElecPers)
                            {
                                listaCientificos.Add(cient);
                            }
                        }
                    }
                    else
                    {
                        listaCientificos.Add(cient);
                    }
                    
                }

                var contacto = new List<List<string>>();
                var turnos = new List<List<string>>();

                foreach (var cientifico in listaCientificos)
                {
                    var listaDatos = new List<string>();
                    listaDatos.Add(cientifico.CorreoElecPers);
                    listaDatos.Add(cientifico.TelCelular.ToString());
                    contacto.Add(listaDatos);
                    var listaT = this.getTurnosCientifico(cientifico, fechaSelec, rec.NumeroRT);
                    turnos.Add(listaT);
                    
                }
                observador.enviarNotificacion(contacto, motivo, turnos, recurso);



            }
        }

    }
}
