using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class Turno
    {
        private DateTime fechaGeneracion;
        private string diaSemana;
        private DateTime fechaHoraInicio;
        private DateTime fechaHoraFin;
        private List<CambioEstadoTurno> cambioEstadoTurno;

        public Turno()
        {

        }

        public Turno(DateTime fechaGeneracion, string diaSemana, DateTime fechaHoraInicio, DateTime fechaHoraFin, List<CambioEstadoTurno> cambioEstadoTurno)
        {
            this.fechaGeneracion = fechaGeneracion;
            this.diaSemana = diaSemana;
            this.fechaHoraInicio = fechaHoraInicio;
            this.fechaHoraFin = fechaHoraFin;
            this.cambioEstadoTurno = cambioEstadoTurno;

        }

        public DateTime FechaGeneracion { get => fechaGeneracion; set => fechaGeneracion = value; }
        public string DiaSemana { get => diaSemana; set => diaSemana = value; }
        public DateTime FechaHoraInicio { get => fechaHoraInicio; set => fechaHoraInicio = value; }
        public DateTime FechaHoraFin { get => fechaHoraFin; set => fechaHoraFin = value; }
        public List<CambioEstadoTurno> CambioEstadoTurno { get => cambioEstadoTurno; set => cambioEstadoTurno = value; }

        public List<Turno> getTurnos()
        {
            List<Turno> turnos = new List<Turno>();
            CambioEstadoTurno cambioEstadoTurno = new CambioEstadoTurno();
            var listaCambioEstadoTurno = cambioEstadoTurno.getCambioEstadoTurno();


            turnos.Add(new Turno(new DateTime(2022, 06, 10, 15,16,24), new DateTime(2022, 06, 10).DayOfWeek.ToString(), new DateTime(2022, 06, 19, 08,00,00), new DateTime(2022, 06, 19, 09,00,00), listaCambioEstadoTurno));
            turnos.Add(new Turno(new DateTime(2022, 06, 13, 12,35,56), new DateTime(2022, 06, 13).DayOfWeek.ToString(), new DateTime(2022, 06, 17, 10,00,00), new DateTime(2022, 06, 17, 11,00,00), listaCambioEstadoTurno));

            return turnos; 
        }

        public List<Turno> getTurnosEspecificos(int numeroRT)
        {
            List<Turno> turnos = new List<Turno>();
            var sentenciaSql = $"SELECT * FROM Turno WHERE numeroRT = {numeroRT}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);

            for (int i = 0; i < tablaResultado.Rows.Count; i++)
            {
                CambioEstadoTurno cambioTurno = new CambioEstadoTurno();
                Turno turno = new Turno();
                var id = Convert.ToInt32(tablaResultado.Rows[i]["idTurno"]);
                turno.FechaGeneracion = Convert.ToDateTime(tablaResultado.Rows[i]["fechaGeneracion"]);
                turno.DiaSemana = tablaResultado.Rows[i]["diaSemana"].ToString();
                turno.FechaHoraInicio = Convert.ToDateTime(tablaResultado.Rows[i]["fechaHoraInicio"]);
                turno.FechaHoraFin = Convert.ToDateTime(tablaResultado.Rows[i]["fechaHoraFin"]);
                var cambio = cambioTurno.getCambioEstadoTurnoEspecifico(id);
                turno.CambioEstadoTurno = cambio;
                turnos.Add(turno);
            }

            return turnos;
        }

        static List<Turno> listaTurnoss = new List<Turno>();
        public List<Turno> esCancelableEnPeriodo(List<Turno> listaTurnos, DateTime fechaIngresada)
        {
            var turnos = false;
            listaTurnoss.Clear();
            var turnosCancelables = new List<CambioEstadoTurno>();
            var cambioEstadoTurno = new CambioEstadoTurno();
            foreach (Turno item in listaTurnos)
            {
                if (item.FechaHoraFin > DateTime.Now && item.FechaHoraInicio <= fechaIngresada)
                {
                    turnosCancelables = item.CambioEstadoTurno;
                    turnos = cambioEstadoTurno.esActual(turnosCancelables);
                    if (turnos == true)
                    {
                        listaTurnoss.Add(item);
                    }
                    
                }
            }
            return listaTurnoss;

        }

        public string getFechaHora(Turno turno)
        {

            var fechaHora = turno.FechaHoraInicio.ToString(); ;
            
            return fechaHora;
        }

        public List<List<string>> getFechaHoraYCientifico(RecursoTecnologico rtSeleccionado, List<Turno> turnosCancelables)
        {
            var fechaHora = "";
            
            var nombre = "";
            var resultado = new List<string>();
            var datos = new List<string>();
            var listaDatos = new List<string>();
            var superLista = new List<List<string>>();
            foreach (Turno item in turnosCancelables)
            {

                resultado = getDatos(rtSeleccionado, item);
                superLista.Add(resultado);
                

            }
            

            return superLista;

        }

        public List<string> getDatos(RecursoTecnologico rtSeleccionado, Turno item)
        {
            var asig = new AsignacionCientificoDelCI();
            var fechaHora = getFechaHora(item);
            var datos = asig.getDatosCientifico(rtSeleccionado, item);
            var nombre = datos[0];
            var correo = datos[1];
            var listaDatos = new List<string>();
            listaDatos.Add(fechaHora);
            listaDatos.Add(nombre);
            listaDatos.Add(correo);
            return listaDatos;
        }

        public void setFechaFin(RecursoTecnologico rt)
        {
            var lista = listaTurnoss;
            foreach (var item in lista)
            {
                var sentenciaSql = $"SELECT t.idTurno FROM Turno t WHERE t.numeroRT = {rt.NumeroRT} AND t.fechaHoraInicio = '{item.FechaHoraInicio.ToString("yyyy/MM/dd hh:mm:ss")}'";
                var tabla = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);
                var id = Convert.ToInt32(tabla.Rows[0]["idTurno"]);
                CambioEstadoTurno cambio = new CambioEstadoTurno();
                cambio.setFechaFin(id);
            }
            

            //for (int i = 0; i < tabla.Rows.Count; i++)
            //{
            //    var id = 
            //    lista.Add(id);
            //}

            //CambioEstadoTurno cambio = new CambioEstadoTurno();
            //for (int i = 0; i < lista.Count; i++)
            //{
            //    
            //}
            
            
        }

        public void newCambioEstadoTurno(Estado estadoTurnoNuevo, RecursoTecnologico rec)
        {
            var cambioTurno = new CambioEstadoTurno();
            var lista = listaTurnoss;
            var sentenciaSQL = $"SELECT idEstado FROM Estado WHERE nombre = '{estadoTurnoNuevo.Nombre}'";
            var tabla = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSQL);
            var idEstado = Convert.ToInt32(tabla.Rows[0]["idEstado"]);

            foreach (var item in lista)
            {
                var sentenciaSql = $"SELECT t.idTurno FROM Turno t JOIN RecursoTecnologico r ON t.numeroRT = r.numeroRT WHERE r.numeroRT = {rec.NumeroRT} AND t.fechaHoraInicio = '{item.FechaHoraInicio.ToString("yyyy/MM/dd hh:mm:ss")}'";
                var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);
                var id = Convert.ToInt32(tablaResultado.Rows[0]["idTurno"]);
                cambioTurno.newCambioEstadoTurno(idEstado, id);
            }
            
            
        }

    }
}
