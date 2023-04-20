using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegistrarRTEnMantenimientoCorrectivo.Formularios;
using RegistrarRTEnMantenimientoCorrectivo.Clases;

namespace RegistrarRTEnMantenimientoCorrectivo
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void textBoxUserName_Click(object sender, EventArgs e)
        {
            if (textBoxUserName.Text == "UserName" )
            {
                textBoxUserName.Clear();
            }

            if (textBoxPassword.Text == "")
            {
                textBoxPassword.Text = "Password";
            }

            if (textBoxPassword.ForeColor != Color.Red)
            {
                panel1.BackColor = Color.MediumTurquoise;
                textBoxUserName.ForeColor = Color.MediumTurquoise;
                panel2.BackColor = Color.White;
                textBoxPassword.ForeColor = Color.White;
            }

            panel1.BackColor = Color.MediumTurquoise;
            textBoxUserName.ForeColor = Color.MediumTurquoise;

        }

        private void textBoxPassword_Click_1(object sender, EventArgs e)
        {

            if (textBoxPassword.Text == "Password")
            {
                textBoxPassword.Clear();

            }
            
            if (textBoxUserName.Text == "")
            {
                textBoxUserName.Text = "UserName";
            }

            if (textBoxUserName.ForeColor != Color.Red)
            {
                textBoxPassword.PasswordChar = '*';
                panel2.BackColor = Color.MediumTurquoise;
                textBoxPassword.ForeColor = Color.MediumTurquoise;
                panel1.BackColor = Color.White;
                textBoxUserName.ForeColor = Color.White;
            }

            panel2.BackColor = Color.MediumTurquoise;
            textBoxPassword.ForeColor = Color.MediumTurquoise;
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxUserName.Text.Equals("") && textBoxPassword.Text.Equals(""))
            {
                MessageBox.Show("Ingrese nombre de usuario y contraseña.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            else
            {
                if (textBoxUserName.Text == "")
                {
                    MessageBox.Show("Ingrese su Nombre de Usuario.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxUserName.Focus();
                    return;
                }
                else
                {
                    if (textBoxPassword.Text == "")
                    {
                        MessageBox.Show("Ingrese su contraseña.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxPassword.Focus();
                        return;
                    }
                    else
                    {
                        bool resultado = false;
                        Usuario usuario = new Usuario();
                        var user = usuario.getUsuario();
                        resultado = usuario.ValidarUsuario(textBoxUserName.Text, textBoxPassword.Text);
                        if (resultado)
                        {
                            Usuario resul = new Usuario();
                            foreach (Usuario unusuario in user)
                            {
                                if (textBoxUserName.Text.Equals(unusuario.Usuarioo) && textBoxPassword.Text.Equals(unusuario.Clave))
                                {
                                    resul = unusuario;
                                }

                            }
                            Main ventana = new Main(resul);
                            ventana.Show();
                            this.Hide();
                        }
                        else
                        {
                            textBoxUserName.ForeColor = Color.Red;
                            textBoxPassword.ForeColor = Color.Red;
                            panel1.BackColor = Color.Red;
                            panel2.BackColor = Color.Red;
                            MessageBox.Show("El usuario ingresado no esta registrado.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }
    }
}
