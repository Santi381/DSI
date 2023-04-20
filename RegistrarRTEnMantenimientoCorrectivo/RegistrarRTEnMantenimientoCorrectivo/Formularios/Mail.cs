using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using RegistrarRTEnMantenimientoCorrectivo.Clases;

namespace RegistrarRTEnMantenimientoCorrectivo.Formularios
{
    public partial class Mail : Form, IObserverNotificacionBajaReserva
    {
        public Mail()
        {
            InitializeComponent();
        }

        public void enviarNotificacion(List<List<string>> contacto, string motivo, List<List<string>> turnos, string rec)
        {
            var to = contacto;
            var motivoo = motivo;
            var body = turnos;
            var recurso = rec;
            var displayName = "Gestión de Recursos Tecnológicos";

            this.enviarMail(to, motivoo, body, displayName, recurso);
        }

        public void enviarMail(List<List<string>> to, string motivo, List<List<string>> body, string name, string recurso)
        {
            string from = "gestionRecursosTecnologicos@hotmail.com";
            string displayName = name;
            try
            {
                for (int i = 0; i < to.Count; i++)
                {
                    var too = to[i];

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(from, displayName);
                    mail.To.Add(too[0]);

                    var turnos = body[i];
                    var cuerpo = "Los turnos que han sido cancelados son: ";
                    for (int f = 0; f < turnos.Count; f++)
                    {
                        if (f == turnos.Count - 1)
                        {
                            cuerpo += turnos[f] + ", debido a que el recurso " + recurso + " fue ingresado a mantenimiento correctivo por " + motivo;
                        }
                        else
                        {
                            cuerpo += turnos[f] + ", ";
                        }
                    }
                    mail.Subject = "Cancelación de turno";
                    mail.Body = cuerpo;
                    mail.IsBodyHtml = false;

                    SmtpClient client = new SmtpClient("smtp.office365.com", 587);
                    client.Credentials = new NetworkCredential(from, "Trabajopractico123.");
                    client.EnableSsl = true;


                    client.Send(mail);
                }

                MessageBox.Show("¡Correo enviado exitosamente!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar este correo. Por favor verifique los datos o intente más tarde.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
