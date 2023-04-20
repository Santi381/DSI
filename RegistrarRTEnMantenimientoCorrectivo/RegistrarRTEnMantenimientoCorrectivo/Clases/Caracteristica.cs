using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class Caracteristica
    {
        private string nombre;
        private string descripcion;

        public Caracteristica()
        {

        }

        public Caracteristica(string nombre, string descripcion)
        {
            this.nombre = nombre;
            this.descripcion = descripcion;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }

        public Caracteristica getCaracteristicas(int id)
        {
            var sentenciaSql = $"SELECT * FROM Caracteristica WHERE idCaracteristica = {id}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);

            Caracteristica carac = new Caracteristica();
            carac.Nombre = tablaResultado.Rows[0]["nombre"].ToString();
            carac.Descripcion = tablaResultado.Rows[0]["descripcion"].ToString();


            return carac;

        }

        public List<Caracteristica> getCaracteristicasEspecificas(int id)
        {
            List<Caracteristica> listaCarac = new List<Caracteristica>();
            var sentenciaSql = $"SELECT * FROM Caracteristica WHERE idTipoRT = {id}";           
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);

            for (int i = 0; i < tablaResultado.Rows.Count; i++)
            {
                Caracteristica carac = new Caracteristica();
                carac.Nombre = tablaResultado.Rows[i]["nombre"].ToString();
                carac.Descripcion = tablaResultado.Rows[i]["descripcion"].ToString();

                listaCarac.Add(carac);
            }

            return listaCarac;
        }
    }
}
