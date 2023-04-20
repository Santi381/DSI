using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class CaracteristicaRecurso
    {
        private string valor;
        private Caracteristica caracteristica;

        public CaracteristicaRecurso()
        {

        }

        public CaracteristicaRecurso(string valor, Caracteristica caracteristica)
        {
            this.valor = valor;
            this.caracteristica = caracteristica;
        }

        public string Valor { get => valor; set => valor = value; }
        public Caracteristica Caracteristica { get => caracteristica; set => caracteristica = value; }

        public List<CaracteristicaRecurso> getCaracteristicaEspecifica(int numeroRT)
        {
            List<CaracteristicaRecurso> lista = new List<CaracteristicaRecurso>();
            var sentenciaSql = $"SELECT * FROM CaracteristicaRecurso WHERE numeroRT = {numeroRT}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);
            for (int i = 0; i < tablaResultado.Rows.Count; i++)
            {
                Caracteristica caracteristica = new Caracteristica();
                CaracteristicaRecurso carac = new CaracteristicaRecurso();
                carac.Valor = tablaResultado.Rows[i]["valor"].ToString();
                var idCarac = Convert.ToInt32(tablaResultado.Rows[i]["idCaracteristica"]);
                carac.Caracteristica = caracteristica.getCaracteristicas(idCarac);            
            }

            return lista;
        }
    }
}
