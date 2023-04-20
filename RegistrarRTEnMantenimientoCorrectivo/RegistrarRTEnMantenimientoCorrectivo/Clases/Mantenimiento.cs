using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class Mantenimiento
    {
        private DateTime fechaFin;
        private DateTime fechaInicio;
        private DateTime fechaInicioPrevista;
        private string motivoMantenimiento;
        private List<ExtensionMantenimiento> extensionMantenimiento;

        public Mantenimiento()
        {

        }
        public Mantenimiento(DateTime fechaFin, DateTime fechaInicio, DateTime fechaInicioPrevista, string motivoMantenimiento, List<ExtensionMantenimiento> extensionMantenimiento)
        {
            this.fechaFin = fechaFin;
            this.fechaInicio = fechaInicio;
            this.fechaInicioPrevista = fechaInicioPrevista;
            this.motivoMantenimiento = motivoMantenimiento;
            this.extensionMantenimiento = extensionMantenimiento;
        }

        public DateTime FechaFin { get => fechaFin; set => fechaFin = value; }
        public DateTime FechaInicio { get => fechaInicio; set => fechaInicio = value; }
        public DateTime FechaInicioPrevista { get => fechaInicioPrevista; set => fechaInicioPrevista = value; }
        public string MotivoMantenimiento { get => motivoMantenimiento; set => motivoMantenimiento = value; }
        public List<ExtensionMantenimiento> ExtensionMantenimientoo { get => extensionMantenimiento; set => extensionMantenimiento = value; }

        public List<Mantenimiento> getDatosMant()
        {
            var lista = new List<Mantenimiento>();

            //lista.Add(new Mantenimiento(new DateTime(2022, 06, 10), new DateTime(2022, 06, 08), new DateTime(2022, 06, 08), "descalibrado"));

            return lista;
        }

        public List<Mantenimiento> getMantenimientoEspecifico(int numeroRT)
        {
            List<Mantenimiento> lista = new List<Mantenimiento>();
            var sentenciaSql = $"SELECT * FROM Mantenimiento WHERE numeroRT = {numeroRT}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);

            for (int i = 0; i < tablaResultado.Rows.Count; i++)
            {
                ExtensionMantenimiento extMan = new ExtensionMantenimiento();
                Mantenimiento mant = new Mantenimiento();
                var id = Convert.ToInt32(tablaResultado.Rows[i]["idMantenimiento"]);
                mant.FechaFin = Convert.ToDateTime(tablaResultado.Rows[i]["fechaFin"]);
                mant.FechaInicio = Convert.ToDateTime(tablaResultado.Rows[i]["fechaInicio"]);
                mant.FechaInicioPrevista = Convert.ToDateTime(tablaResultado.Rows[i]["FechaInicioPrevista"]);
                mant.MotivoMantenimiento = tablaResultado.Rows[i]["motivoMantenimiento"].ToString();
                var extension = extMan.getExtensionEspecifica(id);
                mant.ExtensionMantenimientoo= extension;

                lista.Add(mant);
            }

            return lista;
        }

        public void newMantenimiento(RecursoTecnologico rec, string motivo, DateTime fechaFin)
        {
            var sentenciaSql = $"INSERT INTO Mantenimiento VALUES ('{fechaFin.ToString("yyyy/MM/dd hh:mm:ss")}', '{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}', '{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}', '{motivo}', {rec.NumeroRT})";
            var tablaResultado = DBHelper.GetDBHelper().EjecutarSQL(sentenciaSql);
            //var newMant = new Mantenimiento(fechaFin, DateTime.Now, DateTime.Now, motivo, null);
            //return newMant;
        }
    }
}
