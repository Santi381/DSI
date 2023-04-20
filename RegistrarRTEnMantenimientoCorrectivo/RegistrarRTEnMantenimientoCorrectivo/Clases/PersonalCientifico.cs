using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class PersonalCientifico
    {

        private int legajo;
        private string nombre;
        private string apellido;
        private int numeroDocumento;
        private string correoElecInstit;
        private string correoElecPers;
        private long telCelular;
        private Usuario idUsuario;

        public PersonalCientifico(int legajo, string nombre, string apellido, int numeroDocumento, string correoElecInstit, string correoElecPers, long telCelular, Usuario idUsuario)
        {
            this.legajo = legajo;
            this.nombre = nombre;
            this.apellido = apellido;
            this.numeroDocumento = numeroDocumento;
            this.correoElecInstit = correoElecInstit;
            this.correoElecPers = correoElecPers;
            this.telCelular = telCelular;
            this.idUsuario = idUsuario;
        }

        public PersonalCientifico()
        {

        }

        public int Legajo { get => legajo; set => legajo = value; }
        public int NumeroDocumento { get => numeroDocumento; set => numeroDocumento = value; } 
        public string CorreoElecPers { get => correoElecPers; set => correoElecPers = value; }
        public long TelCelular { get => telCelular; set => telCelular = value; }
        public string CorreoElecInstit { get => correoElecInstit; set => correoElecInstit = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public Usuario IdUsuario { get => idUsuario; set => idUsuario = value; }

        public List<PersonalCientifico> getPersonalCientifico()
        {
            List<PersonalCientifico> listaPers = new List<PersonalCientifico>();

            var sentenciaSql = $"SELECT * FROM PersonalCientifico";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);
            for (int i = 0; i < tablaResultado.Rows.Count; i++)
            {
                PersonalCientifico personalCientifico = new PersonalCientifico();
                personalCientifico.Legajo = Convert.ToInt32(tablaResultado.Rows[i]["legajo"]);
                personalCientifico.Nombre = tablaResultado.Rows[i]["nombre"].ToString();
                personalCientifico.Apellido = tablaResultado.Rows[i]["apellido"].ToString();
                personalCientifico.NumeroDocumento = Convert.ToInt32(tablaResultado.Rows[i]["numeroDocumento"]);
                personalCientifico.CorreoElecInstit = tablaResultado.Rows[i]["correoElecInstit"].ToString();
                personalCientifico.CorreoElecPers = tablaResultado.Rows[i]["correoElecPers"].ToString();
                personalCientifico.TelCelular = Convert.ToInt64(tablaResultado.Rows[i]["telCelular"]);
                var idBusqueda = Convert.ToInt32(tablaResultado.Rows[i]["idUsuario"]);
                var usuario = GetUsuario(idBusqueda);
                personalCientifico.IdUsuario = usuario;
                listaPers.Add(personalCientifico);
            }

            return listaPers;
        }

        public PersonalCientifico getPersonalCientificoEspecifico(int id)
        {
            PersonalCientifico personalCientifico = new PersonalCientifico();
            var sentenciaSQL = $"SELECT * FROM PersonalCientifico WHERE legajo = {id}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSQL);
            for (int i = 0; i < tablaResultado.Rows.Count; i++)
            {
                var personal = new PersonalCientifico();
                personal.Legajo = Convert.ToInt32(tablaResultado.Rows[i]["legajo"]);
                personal.Nombre = tablaResultado.Rows[i]["nombre"].ToString();
                personal.Apellido = tablaResultado.Rows[i]["apellido"].ToString();
                personal.NumeroDocumento = Convert.ToInt32(tablaResultado.Rows[i]["numeroDocumento"]);
                personal.CorreoElecInstit = tablaResultado.Rows[i]["apellido"].ToString();
                personal.CorreoElecPers = tablaResultado.Rows[i]["apellido"].ToString();
                personal.TelCelular = Convert.ToInt32(tablaResultado.Rows[i]["numeroDocumento"]);
                var idBusqueda = Convert.ToInt32(tablaResultado.Rows[i]["idUsuario"]);
                var usuario = GetUsuario(idBusqueda);
                personal.IdUsuario = usuario;
                personalCientifico = personal;
            }
            return personalCientifico;
        }

        public List<string> getDatos(int legajo)
        {
            List<string> list = new List<string>();

            var sentenciaSql = $"SELECT p.nombre, p.correoElecPers FROM PersonalCientifico p WHERE p.legajo = {legajo}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);

            var nombre = tablaResultado.Rows[0]["nombre"].ToString();
            var correo = tablaResultado.Rows[0]["correoElecPers"].ToString();

            list.Add(nombre);
            list.Add(correo);
            return list;
        }

        public Usuario GetUsuario(int id)
        {
            var usuario = new Usuario();
            var sentenciaSql = $"SELECT u.* from Usuario u WHERE u.idUsuario = {id}";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);
            for (int i = 0; i < tablaResultado.Rows.Count; i++)
            {
                Usuario user = new Usuario();
                user.Usuarioo = tablaResultado.Rows[i]["usuario"].ToString();
                user.Clave = tablaResultado.Rows[i]["clave"].ToString();
                user.Habilitado = tablaResultado.Rows[i]["clave"].ToString();
                usuario = user;
            }
            return usuario;
        }

        //public bool tengoUsuarioHabilitado()
        //{
        //    if (Usuario.Habilitado == "S")
        //    {
        //    return true;
        //    }
        //    else
        //    {
        //    return false;
        //    }
        //}
    }
}
