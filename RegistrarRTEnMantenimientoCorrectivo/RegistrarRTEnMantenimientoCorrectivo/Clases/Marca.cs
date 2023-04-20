using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegistrarRTEnMantenimientoCorrectivo.Clases;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class Marca
    {
        
        private string nombre;
        private List<Modelo> modelos;

        public Marca()
        {

        }

        public Marca(string nombre, List<Modelo> modelos)
        {
            this.nombre = nombre;
            this.modelos = modelos;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public List<Modelo> Modelos { get => modelos; set => modelos = value; }

        public List<Marca> getMarcas()
        {

            List<Marca> lista = new List<Marca>();
            Modelo modelo1 = new Modelo();
            var modelos = modelo1.getModelos();
            var listaModel1 = new List<Modelo>();
            listaModel1.Add(modelos[0]);
            var listaModel2 = new List<Modelo>();
            listaModel2.Add(modelos[1]);

            Marca marca1 = new Marca("NikkonS", listaModel1);

            Marca marca2 = new Marca("Shidmazu", listaModel2);

            lista.Add(marca1);
            lista.Add(marca2);

            return lista;

        }
        public string getNombre(int idMarca)
        {
            string stringNombre = "";
            var sentenciaSql = $"SELECT * FROM Marca WHERE idMarca = {idMarca}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);

            stringNombre = tablaResultado.Rows[0]["nombre"].ToString();

            return stringNombre;
        }
    }
}
