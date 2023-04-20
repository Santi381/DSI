using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    class AsignacionCientificoDelCI
    {
        private DateTime fechaDesde;
        private DateTime fechaHasta;
        private List<Turno> turnos;
        private PersonalCientifico personalCientifico;

        public AsignacionCientificoDelCI()
        {
        }

        public AsignacionCientificoDelCI(DateTime fechaDesde, DateTime fechaHasta, List<Turno> turnos, PersonalCientifico personalCientifico)
        {
            this.fechaDesde = fechaDesde;
            this.fechaHasta = fechaHasta;
            this.turnos = turnos;
            this.personalCientifico = personalCientifico;
        }

        public DateTime FechaDesde { get => fechaDesde; set => fechaDesde = value; }
        public DateTime FechaHasta { get => fechaHasta; set => fechaHasta = value; }
        public List<Turno> Turnos { get => turnos; set => turnos = value; }
        public PersonalCientifico PersonalCientifico { get => personalCientifico; set => personalCientifico = value; }

        public int getLegajo(int idAsignacion)
        {
            var sentenciaSql = $"SELECT a.legajo FROM AsignacionCientificoDelCI a WHERE a.idAsignacion = {idAsignacion}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);

            var legajo = Convert.ToInt32(tablaResultado.Rows[0]["legajo"]);

            return legajo;
        }

        public int getIdAsignacion(RecursoTecnologico rt, Turno turno)
        {
            var numeroRT = rt.NumeroRT;
            var sentenciaSql = $"SELECT t.idAsignacion FROM Turno t WHERE fechaHoraInicio = '{turno.FechaHoraInicio.ToString("yyyy/MM/dd hh:mm:ss")}' AND numeroRT = {numeroRT}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);

            var idAsignacion = Convert.ToInt32(tablaResultado.Rows[0]["idAsignacion"]);

            return idAsignacion;
        }

        public List<string> getDatosCientifico(RecursoTecnologico rtSeleccionado, Turno turno)
        {
            PersonalCientifico personal = new PersonalCientifico();
            var asig = new AsignacionCientificoDelCI();
            var idAsig = asig.getIdAsignacion(rtSeleccionado, turno);
            var legajo = asig.getLegajo(idAsig);
            var datosCientifico = personal.getDatos(legajo);

            return datosCientifico;

        }

    }
}
