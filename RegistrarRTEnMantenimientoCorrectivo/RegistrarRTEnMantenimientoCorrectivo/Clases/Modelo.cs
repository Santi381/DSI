using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class Modelo
    {
        private string nombre;

        public Modelo()
        {

        }

        public Modelo (string nombre)
        {
            this.nombre = nombre;
        }

        public string Nombre { get => nombre; set => nombre = value; }

        public List<Modelo> getModelos()
        {   
            var list = new List<Modelo>(); 

            Modelo modelo1 = new Modelo();
            modelo1.nombre = "MM-400/800";

            Modelo modelo2 = new Modelo();
            modelo2.nombre = "TXB622L";

            list.Add(modelo1);
            list.Add(modelo2);

            return list; 
        }

        public Modelo getModeloEspecifico(int id)
        {
            Modelo modelo = new Modelo();
            var sentenciaSql = $"SELECT * FROM Modelo WHERE idModelo = {id}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);

            modelo.nombre = tablaResultado.Rows[0]["nombre"].ToString();

            return modelo;
        }

        public int getIdMarca(string nombre)
        {
            var sentenciaSql = $"SELECT idMarca FROM Modelo WHERE nombre = '{nombre}'";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);

            var marcaId = Convert.ToInt32(tablaResultado.Rows[0]["idMarca"]);
            return marcaId;
        }

        public (string, string) getDatos(RecursoTecnologico rec)
        {
            Modelo modelo = new Modelo();
            string stringModelo = rec.Modelo.Nombre;
            var idMarca = modelo.getIdMarca(stringModelo);
            Marca marcax = new Marca();
            string stringMarca = marcax.getNombre(idMarca);
            return (stringModelo, stringMarca);
        }
    }
}
