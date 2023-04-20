using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class CambioEstadoRT
    {
        private DateTime fechaHoraDesde;
        private DateTime fechaHoraHasta;
        private Estado estado;

        public CambioEstadoRT()
        {

        }

        public CambioEstadoRT(DateTime fechaHoraDesde, DateTime fechaHoraHasta, Estado estado)
        {
            this.fechaHoraDesde = fechaHoraDesde;
            this.fechaHoraHasta = fechaHoraHasta;
            this.estado = estado;
        }

        public DateTime FechaHoraDesde { get => fechaHoraDesde; set => fechaHoraDesde = value; }
        public DateTime FechaHoraHasta { get => fechaHoraHasta; set => fechaHoraHasta = value; }
        public Estado Estadoo { get => estado; set => estado = value; }

        
        public List<CambioEstadoRT> getCambioEstadoRT()
        {
            
            Estado estado = new Estado();
            var estados = estado.getEstado();
            var estadoPrimero = estados[0];
            List<CambioEstadoRT> cambioEstadoRT = new List<CambioEstadoRT>();

            cambioEstadoRT.Add(new CambioEstadoRT(new DateTime(2022,06,10), Convert.ToDateTime(null), estadoPrimero));

            return cambioEstadoRT;
        }

        public List<CambioEstadoRT> getCambioEstadoRTEspecifico(int numeroRT)
        {
            List<CambioEstadoRT> lista = new List<CambioEstadoRT>();
            var sentenciaSql = $"SELECT * FROM CambioEstadoRT WHERE numeroRT = {numeroRT}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);

            for (int i = 0; i < tablaResultado.Rows.Count; i++)
            {
                Estado estado = new Estado();
                CambioEstadoRT cambio = new CambioEstadoRT();
                cambio.FechaHoraDesde = Convert.ToDateTime(tablaResultado.Rows[i]["fechaHoraDesde"]);
                if (tablaResultado.Rows[i]["fechaHoraHasta"] is DBNull)
                {
                    cambio.FechaHoraHasta = DateTime.MinValue;
                }
                else
                {
                    cambio.FechaHoraHasta = Convert.ToDateTime(tablaResultado.Rows[i]["fechaHoraHasta"]);
                }
                
                var idEstado = Convert.ToInt32(tablaResultado.Rows[i]["idEstado"]);
                cambio.Estadoo = estado.getEstadoEspecifico(idEstado);
                lista.Add(cambio);
            }
            return lista;
        }

        static List<CambioEstadoRT> cambioActual = new List<CambioEstadoRT>();
        public bool esActual(RecursoTecnologico rt)
        {
            var esDisponible = false;
            var listaCambios = rt.CambioEstadoRTT;
            
            for (int i = 0; i < listaCambios.Count; i++)
            {
                if (listaCambios[i].FechaHoraHasta == DateTime.MinValue)
                {
                    
                    var estado = new Estado();
                    var es = estado.esDisponible(listaCambios[i]);
                    if (es == true)
                    {
                        esDisponible = true;
                        cambioActual.Add(listaCambios[i]);
                        break;
                    }
                }
            }
            

            return esDisponible;
        }

        public void setFechaFin(RecursoTecnologico rt)
        {
            var fecha = DateTime.MinValue;
            var numero = rt.NumeroRT;
            var fechart = rt.CambioEstadoRTT;
            var cambio = cambioActual;
            foreach (var item in cambio)
            {
                foreach (var item2 in fechart)
                {
                    if (item.FechaHoraDesde == item2.FechaHoraDesde)
                    {
                        fecha = item2.FechaHoraDesde;
                    }
                }
            }

            var sentenciaSql = $"UPDATE CambioEstadoRT SET fechaHoraHasta = '{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}' WHERE numeroRT = {numero} AND fechaHoraDesde = '{fecha.ToString("yyyy/MM/dd hh:mm:ss")}'";
            var tablaResultado = DBHelper.GetDBHelper().EjecutarSQL(sentenciaSql);

        }

        public void newCambioEstadoRT(int numeroRT, int idEstado)
        {
            var sentenciaSql = $"INSERT INTO CambioEstadoRT VALUES ('{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}', null, {idEstado}, {numeroRT})";
            var tablaResultado = DBHelper.GetDBHelper().EjecutarSQL(sentenciaSql);
        }
    }
}
