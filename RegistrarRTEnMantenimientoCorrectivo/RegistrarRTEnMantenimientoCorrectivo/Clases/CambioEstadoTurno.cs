using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class CambioEstadoTurno
    {
        private DateTime fechaHoraDesde;
        private DateTime fechaHoraHasta;
        private Estado estado;


        public CambioEstadoTurno()
        {

        }

        public CambioEstadoTurno(DateTime fechaHoraDesde, DateTime fechaHoraHasta, Estado estado)
        {
            this.fechaHoraDesde = fechaHoraDesde;
            this.fechaHoraHasta = fechaHoraHasta;
            this.estado = estado;
        }

        public DateTime FechaHoraDesde { get => fechaHoraDesde; set => fechaHoraDesde = value; }
        public DateTime FechaHoraHasta { get => fechaHoraHasta; set => fechaHoraHasta = value; }
        public Estado Estado { get => estado; set => estado = value; }

        public List<CambioEstadoTurno> getCambioEstadoTurno()
        {
            List<CambioEstadoTurno> cambioEstadoTurno = new List<CambioEstadoTurno>();
            Estado estado = new Estado();
            var estadoo = estado.getEstado();


            cambioEstadoTurno.Add(new CambioEstadoTurno(new DateTime(2022, 06, 13), new DateTime(2022, 06, 13), estadoo[1]));
            cambioEstadoTurno.Add(new CambioEstadoTurno(new DateTime(2022, 06, 14), Convert.ToDateTime(null), estadoo[1]));

            return cambioEstadoTurno;
        }

        public List<CambioEstadoTurno> getCambioEstadoTurnoEspecifico(int idTurno)
        {
            List<CambioEstadoTurno> listaCambio = new List<CambioEstadoTurno>();
            var sentenciaSql = $"SELECT * FROM CambioEstadoTurno WHERE idTurno = {idTurno}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);

            for (int i = 0; i < tablaResultado.Rows.Count; i++)
            {
                Estado estado = new Estado();
                CambioEstadoTurno cambio = new CambioEstadoTurno();
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
                var est = estado.getEstadoEspecifico(idEstado);
                cambio.Estado = est;
                listaCambio.Add(cambio);
            }

            return listaCambio;
        }

        List<CambioEstadoTurno> cambioActual = new List<CambioEstadoTurno>();
        public bool esActual(List<CambioEstadoTurno> listaCambioTurnos)
        {
            var estado = new Estado();
            var resultado = false;
            foreach (CambioEstadoTurno item in listaCambioTurnos)
            {
                if (item.FechaHoraHasta == DateTime.MinValue)
                {
                    var cambio = item;
                    var estados = estado.esCancelablee(cambio);
                    if (estados == true)
                    {
                        resultado = true;
                        cambioActual.Add(cambio);
                        
                    }
                    
                }
            }
            return resultado;
        }

        public void setFechaFin(int id)
        {
            var sentenciaSql = $"UPDATE c SET c.fechaHoraHasta = '{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}' FROM CambioEstadoTurno c WHERE c.idTurno = {id}"; 
            var tablaResultado = DBHelper.GetDBHelper().EjecutarSQL(sentenciaSql);
        }

        public void newCambioEstadoTurno(int idEstado, int id)
        {
            var sentenciaSql = $"INSERT INTO CambioEstadoTurno VALUES ('{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}', null, {idEstado}, {id})";
            var tabla = DBHelper.GetDBHelper().EjecutarSQL(sentenciaSql);
            
        }

    }
}
