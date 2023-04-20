using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class HorarioRT
    {
        private string diaSemana;
        private int horaDesde;
        private int horaHasta;
        private DateTime vigenciaDesde;
        private DateTime vigenciaHasta;

        public HorarioRT()
        {

        }

        public HorarioRT(string diaSemana, int horaDesde, int horaHasta, DateTime vigenciaDesde, DateTime vigenciaHasta)
        {
            this.diaSemana = diaSemana;
            this.horaDesde = horaDesde;
            this.horaHasta = horaHasta;
            this.vigenciaHasta = vigenciaHasta;
            this.vigenciaDesde = vigenciaDesde;
        }

        public string DiaSemana { get => diaSemana; set => diaSemana = value; }
        public int HoraDesde { get => horaDesde; set => horaDesde = value; }
        public int HoraHasta { get => horaHasta; set => horaHasta = value; }
        public DateTime VigenciaDesde { get => vigenciaDesde; set => vigenciaDesde = value; }
        public DateTime VigenciaHasta { get => vigenciaHasta;set => vigenciaHasta = value; }

        public List<HorarioRT> getDisponibilidad()
        {
            List<HorarioRT> disponibilidad = new List<HorarioRT>();

            //disponibilidad.Add(new HorarioRT("lunes", 8, 12, new DateTime(2022, 06, 01), new DateTime(2022, 06, 30)));

            return disponibilidad;
        }

        public List<HorarioRT> getDisponibilidadEspecifica(int numeroRT)
        {
            List<HorarioRT> disponibilidad = new List<HorarioRT>();
            var sentenciaSql = $"SELECT * FROM HorarioRT WHERE numeroRT = {numeroRT}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);
            for (int i = 0; i < tablaResultado.Rows.Count; i++)
            {
                HorarioRT disp = new HorarioRT();
                disp.DiaSemana = tablaResultado.Rows[i]["diaSemana"].ToString();
                disp.HoraDesde = Convert.ToInt32(tablaResultado.Rows[i]["horaDesde"]);
                disp.HoraHasta = Convert.ToInt32(tablaResultado.Rows[i]["horaHasta"]);
                disp.VigenciaDesde = Convert.ToDateTime(tablaResultado.Rows[i]["vigenciaDesde"]);
                if (tablaResultado.Rows[i]["vigenciaHasta"] is DBNull)
                {
                    disp.VigenciaHasta = DateTime.MinValue;
                }
                else
                {
                    disp.VigenciaHasta = Convert.ToDateTime(tablaResultado.Rows[i]["vigenciaHasta"]);
                }
                
                disponibilidad.Add(disp);
            }

            return disponibilidad;
        }

    }
}
