using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class Estado
    {
        private string nombre;
        private string descripcion;
        private string ambito;
        private string esResevable;
        private string esCancelable;
        
        public Estado()
        {

        }
        public Estado(string nombre, string descripcion, string ambito, string esResevable, string esCancelable)
        {
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.ambito = ambito;
            this.esResevable = esResevable;
            this.esCancelable = esCancelable;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public string Ambito { get => ambito; set => ambito = value; }
        public string EsResevable { get => esResevable; set => esResevable = value; }
        public string EsCancelable { get => esCancelable; set => esCancelable = value; }

        public List<Estado> getEstado()
        {
            List<Estado> listaEstados = new List<Estado>();

            var sentenciaSql = $"SELECT * FROM Estado";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);
            for (int i = 0; i < tablaResultado.Rows.Count; i++)
            {
                Estado estado = new Estado();
                estado.Nombre = tablaResultado.Rows[i]["nombre"].ToString();
                estado.Descripcion = tablaResultado.Rows[i]["descripcion"].ToString();
                estado.Ambito = tablaResultado.Rows[i]["ambito"].ToString();
                estado.EsResevable = tablaResultado.Rows[i]["esReservable"].ToString();
                estado.EsCancelable = tablaResultado.Rows[i]["esCancelable"].ToString();
                listaEstados.Add(estado);
            }

            return listaEstados;
        }

        public Estado MapearEstado(DataRow fila)
        {
            Estado estad = new Estado();
            estad.Nombre = fila["nombre"].ToString();
            estad.Descripcion = fila["descripcion"].ToString();
            estad.Ambito = fila["ambito"].ToString();
            estad.EsResevable = fila["esReservable"].ToString();
            estad.EsCancelable = fila["esCancelable"].ToString();
            return estad;
        }

        public Estado getEstadoEspecifico(int id)
        {
            Estado estado = new Estado();
            var sentenciaSql = $"SELECT e.* FROM Estado e WHERE e.idEstado = {id}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);
            foreach (DataRow filaAfectada in tablaResultado.Rows)
            {
                estado = MapearEstado(filaAfectada);
            }
            return estado;


            //estado.Nombre = tablaResultado.Rows[0]["nombre"].ToString();
            //estado.Descripcion = tablaResultado.Rows[0]["descripcion"].ToString();
            //estado.Ambito = tablaResultado.Rows[0]["ambito"].ToString();
            //estado.EsResevable = tablaResultado.Rows[0]["esReservable"].ToString();
            //estado.EsCancelable = tablaResultado.Rows[0]["esCancelable"].ToString();


            //return estado;

        }

        public bool esDisponible(CambioEstadoRT cambio)
        {
            var esDisponible = false;
            var estado = cambio.Estadoo;
            if (estado.Nombre == "Disponible")
            {
                esDisponible= true;
            }

            return esDisponible;
        }

        public bool esCancelablee(CambioEstadoTurno cambioEstadoTurnos)
        {
            var res = false;
            var estado = cambioEstadoTurnos.Estado;
            if (estado.EsCancelable == "S")
            {
                res = true;
            }

            return res;
        }

        public Estado esAmbitoTurnoYCanceladoMantCorrec()
        {
            var estados = getEstado();
            var estado = new Estado();
            foreach (Estado item in estados)
            {
                if (item.Ambito == "Turno" && item.Nombre == "CanceladoMantCorrectivo")
                {
                    estado = item;
                    break;
                }
            }
            return estado;
        }

        public Estado esAmbitoRTYMantCorrec()
        {
            var estados = getEstado();
            var estado = new Estado();
            foreach (Estado item in estados)
            {
                if (item.Ambito == "RecursoTecnologico" && item.Nombre == "MantenimientoCorrectivo")
                {
                    estado = item;
                    break;
                }
            }
            return estado;
        }

    }
}
