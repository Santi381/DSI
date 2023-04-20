using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegistrarRTEnMantenimientoCorrectivo.Clases;
using RegistrarRTEnMantenimientoCorrectivo.Formularios.RecursoTecnologicoEnMantenimientoCorrectivo;

namespace RegistrarRTEnMantenimientoCorrectivo.Formularios
{
    public partial class Main : Form
    {
        private string nombre;
        public Usuario usuario;

        public Main(Usuario user)
        {
            InitializeComponent();
            usuario = user;
            nombre = user.Usuarioo;
            Saludo.Text = "¡Bienvenido " + nombre + "!";
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login ventana = new Login();
            ventana.Show();
            this.Hide();
        }

        private void EnMantenimientoCorrectivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            MantenimientoCorrectivo ventana = new MantenimientoCorrectivo(usuario);
            ventana.Show();
            
        }

    }
}
