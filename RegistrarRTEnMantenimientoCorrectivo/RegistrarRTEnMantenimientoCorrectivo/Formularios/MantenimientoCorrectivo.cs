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
using RegistrarRTEnMantenimientoCorrectivo.Formularios;

namespace RegistrarRTEnMantenimientoCorrectivo.Formularios.RecursoTecnologicoEnMantenimientoCorrectivo
{
    public partial class MantenimientoCorrectivo : Form
    {
        GestorRegIngRTMantCorrec gestor = new GestorRegIngRTMantCorrec();
        private Usuario usuarioLog;
        private DateTime fecha;
        public MantenimientoCorrectivo(Usuario usuario)
        {
            InitializeComponent();
            usuarioLog = usuario;
        }

        public void obtenerCientificoLogueado()
        {
            var userr = gestor.buscarCientifLog(usuarioLog);
            
            textBox1.Text = userr.Nombre + " " + userr.Apellido;
        }

        public void buscarRTCientif()
        {
            gestor.buscarRTCientif();

            
        }

        public void mostrarRT()
        {
            var list = gestor.buscarRTDisponible();

            if (list != null)
            {
                var filas = new string[4];
                foreach (var item in list)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        filas[i] = item[i];
                    }
                    dataGridView1.Rows.Add(filas);
                }
                
            }
            else
            {
                MessageBox.Show("No hay recursos tecnológicos asignados", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }


        private void MantenimientoCorrectivo_Load(object sender, EventArgs e)
        {
            obtenerCientificoLogueado();
            cargarComboBox();
            buscarRTCientif();
            mostrarRT();

        }

        string motivo;
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = true;
            label5.Visible = true;
            comboBox1.Visible = true;
            dataGridView2.Rows.Clear();
            var list = new List<List<string>>();
            fecha = dateTimePicker1.Value;
            motivo = richTextBox1.Text;
            gestor.getMotivo(motivo);
            if (motivo == "")
            {
                MessageBox.Show("Ingrese un motivo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var res = gestor.validarFechaIngresada(fecha);
                if (res == true)
                {
                    var rtSeleccionado = dataGridView1.CurrentRow.Cells[0].Value;
                    list = gestor.obtenerTurnosRTCancelables(Convert.ToInt32(rtSeleccionado), fecha);
                    var filas = new string[3];
                    foreach (var item in list)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            filas[i] = item[i];
                        }
                        dataGridView2.Rows.Add(filas);
                    }


                }
                else
                {
                    MessageBox.Show("Ingrese una fecha válida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }

        public void cargarComboBox()
        {
            comboBox1.Items.Add("Mail");
            comboBox1.Items.Add("WhatsApp");
            comboBox1.SelectedIndex = 0;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            var fechaFin = fecha;
            var listaEstados = gestor.buscarEstados();

            gestor.ingresarEnMantCorrec(motivo, fechaFin, listaEstados[0]);
            gestor.cancelarTurno(listaEstados[1]);
            gestor.obtenerObservadores();

            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
