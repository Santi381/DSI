using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    public class Usuario
    {
        private string usuario;
        private string clave;
        private string habilitado;

        public Usuario()
        {
                 
        }

        public Usuario(string usuario, string clave, string habilitado)
        {
            this.usuario = usuario;
            this.clave = clave;
            this.habilitado = habilitado;
        }

        public string Clave { get => clave; set => clave = value; }
        public string Usuarioo { get => usuario; set => usuario = value; }
        public string Habilitado { get => habilitado; set => habilitado = value; }
        
        public bool ValidarUsuario(string nombreUsuario, string password)
        {

            var usuario = getUsuario();


            bool resultado = false;
                foreach (Usuario unusuario in usuario)
                {
                    if (nombreUsuario.Equals(unusuario.Usuarioo) && password.Equals(unusuario.Clave) && unusuario.Habilitado == "S")
                    {
                        resultado = true;
                        
                    
                    }                  

                }
            return resultado;
        }
        

        public void habilitar()
        {
            Habilitado = "S";
        }

        public void inhabilitado()
        {
            Habilitado = "N";
        }

        public Usuario MapearUsuario(DataRow fila)
        {
            Usuario user = new Usuario();
            user.Usuarioo = fila["usuario"].ToString();
            user.Clave = fila["clave"].ToString();
            user.Habilitado = fila["habilitado"].ToString();

            return user;
        }

        public List<Usuario> getUsuario()
        {

            var usuarios = new List<Usuario>();
            var sentenciaSql = $"SELECT * FROM Usuario";
            var tablaResultado = DBHelper.GetDBHelper().ConsultaSQL(sentenciaSql);
            foreach (DataRow filaAfectada in tablaResultado.Rows)
            {
                var usuario = MapearUsuario(filaAfectada);
                usuarios.Add(usuario);
            }
            return usuarios;
            //for (int i = 0; i < tablaResultado.Rows.Count; i++)
            //{
            //    Usuario usuario = new Usuario();
            //    usuario.usuario = tablaResultado.Rows[i]["usuario"].ToString();
            //    usuario.clave = tablaResultado.Rows[i]["clave"].ToString();
            //    usuario.habilitado = tablaResultado.Rows[i]["habilitado"].ToString();
            //    usuarios.Add(usuario);
            //}

            //return usuarios;

        }

    }
}
