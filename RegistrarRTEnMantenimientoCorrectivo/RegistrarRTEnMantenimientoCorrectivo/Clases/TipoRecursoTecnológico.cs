using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class TipoRecursoTecnológico
    {
        private string nombre;
        private string descripcion;
        private List<Caracteristica> caracteristicas;

        public TipoRecursoTecnológico()
        {

        }

        public TipoRecursoTecnológico(string nombre, string descripcion, List<Caracteristica> caracteristicas)
        {
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.caracteristicas = caracteristicas;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public List<Caracteristica> Carasteristicas { get => caracteristicas; set => caracteristicas = value; }

        /*public List<TipoRecursoTecnológico> getTipoRT()
        {

        }*/

        public TipoRecursoTecnológico getTipoRTEspecifico(int tipoRT)
        {
            Caracteristica carac = new Caracteristica();
            TipoRecursoTecnológico tipo = new TipoRecursoTecnológico();
            var sentenciaSql = $"SELECT * FROM TipoRT WHERE idTipoRT = {tipoRT}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);
            var id = Convert.ToInt32(tablaResultado.Rows[0]["idTipoRT"]);
            tipo.Nombre = tablaResultado.Rows[0]["nombre"].ToString();
            tipo.Descripcion = tablaResultado.Rows[0]["descripcion"].ToString();
            var caracteristicas = carac.getCaracteristicasEspecificas(id);
            tipo.Carasteristicas = caracteristicas;

            return tipo;
        }

        public string getNombre(RecursoTecnologico rec)
        {
            string nombre = rec.TipoDeRT.Nombre;
            return nombre;
        }
    }
}
