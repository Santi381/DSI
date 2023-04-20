using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrarRTEnMantenimientoCorrectivo.Clases
{
    class Sesion
    {
        private Usuario usuario;

        public Sesion()
        {

        }
        public Sesion(Usuario usuario)
        {
            this.usuario = usuario;
        }

        public Usuario Usuario { get => usuario; set => usuario = value; }
        public PersonalCientifico getSesion(Usuario user)
        {
            
            Usuario usuario = new Usuario();
            Usuario usss = new Usuario();

            //var listUsuarios = usss.getUsuario();


            usuario = user;
            var nombre = usuario.Usuarioo;

            PersonalCientifico perso = new PersonalCientifico();
            var listPers = perso.getPersonalCientifico();

            PersonalCientifico pers = new PersonalCientifico();

            foreach (PersonalCientifico person in listPers)
            {
                if (nombre.Equals(person.IdUsuario.Usuarioo))
                {
                    pers = person;
                    break;
                }
            }

            return pers;
        }
    }
}
