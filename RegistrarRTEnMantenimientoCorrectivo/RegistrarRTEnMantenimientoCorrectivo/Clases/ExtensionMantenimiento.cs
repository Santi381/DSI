using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class ExtensionMantenimiento
    {
        private DateTime fecha;
        private DateTime fechaFinPrevista;
        private string motivo;

        public ExtensionMantenimiento()
        {

        }
        public ExtensionMantenimiento(DateTime fecha, DateTime fechaFinPrevista, string motivo)
        {
            this.fecha = fecha;
            this.fechaFinPrevista = fechaFinPrevista;
            this.motivo = motivo;
        }

        public DateTime Fecha { get => fecha; set => fecha = value; }
        public DateTime FechaFinPrevista { get => fechaFinPrevista; set => fechaFinPrevista = value; }
        public string Motivo { get => motivo; set => motivo = value; }


        public List<ExtensionMantenimiento> getExtensionEspecifica(int id)
        {
            List<ExtensionMantenimiento> lista = new List<ExtensionMantenimiento>();
            var sentenciaSql = $"SELECT * FROM ExtensionMantenimiento WHERE idMantenimiento = {id}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);
            
            for (int i = 0; i < tablaResultado.Rows.Count; i++)
            {
                ExtensionMantenimiento ext = new ExtensionMantenimiento();
                ext.Fecha = Convert.ToDateTime(tablaResultado.Rows[i]["fecha"]);
                ext.FechaFinPrevista = Convert.ToDateTime(tablaResultado.Rows[i]["fechaFinPrevista"]);
                ext.Motivo = tablaResultado.Rows[i]["motivo"].ToString();
                lista.Add(ext);
            }
            
            return lista;
        }
    }
}
