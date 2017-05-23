using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace VisualVideoCTEC
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            txtuser.Focus();
            timer1.Interval = 2000;     
        }
        public void EnviarDatos(string pUser, string pPass)
        {
            try
            {
                string mail = pUser;
                string contraseña = pPass;

                ASCIIEncoding encode = new ASCIIEncoding();
                //if ($user == "ram" && $pass == "ctec")
                string datosaEnviar = "user=" + mail + "&pass=" + contraseña;
                byte[] datos = Encoding.ASCII.GetBytes(datosaEnviar);
                ///Encoding datos1 = Encoding.GetEncoding(datosaEnviar);

                WebRequest request = WebRequest.Create("http://www.ctec.com.ar/c-sharp.php");
                request.Method = "POST";
                request.ContentType = "Application/x-www-form-urlencoded";
                request.ContentLength = datos.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(datos, 0, datos.Length);
                stream.Close();


                WebResponse response = request.GetResponse();
                stream = response.GetResponseStream();

                StreamReader sr = new StreamReader(stream);

                if (sr.ReadToEnd() == "1")
                {
                    //MessageBox.Show(sr.ReadToEnd());
                    //Oculta el formulario de login
                    this.Hide();
                    MostrarFrmPrincipal();
                }
                else
                {
                    //MessageBox.Show(sr.ReadToEnd());
                    lblMensajeError.Text = "Error: Intente nuevamente por favor.";
                    timer1.Start();
                }

                sr.Close();
                stream.Close();

                
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.Message);
                
                panel1.BackColor = Color.OrangeRed;
                label1.Font= new Font("Arial", 18, FontStyle.Bold);
                label1.Text = "ERROR: Indice Corrupto.\nReporte Error:\n(cod - 20145)";
                label1.Location = new Point(label1.Location.X - 30,label1.Location.Y-30);
                label1.Refresh();

                //---______________________HS
                funciones Funcion = new funciones();
                Funcion.LimpiarTextbox(this);
                Funcion.DeshabilitarTextbox(this);
                // EMC --HS-------------------------------------
                this.Show();
            }
        }

        public static ArrayList getMacAddress()
        {
            // Contador para un ciclo
            int i = 0;
            // Colección de direcciones MAC
            ArrayList DireccionesMAC = new ArrayList();
            // Información de las tarjetas de red
            NetworkInterface[] interfaces = null;
            // Obtener todas las interfaces de red de la PC
            interfaces = NetworkInterface.GetAllNetworkInterfaces();
            // Validar la cantidad de tarjetas de red que tiene
            if (interfaces != null && interfaces.Length > 0)
            {
                // Recorrer todas las interfaces de red
                foreach (NetworkInterface adaptador in interfaces)
                {
                    // Obtener la dirección fisica
                    PhysicalAddress direccion = adaptador.GetPhysicalAddress();
                    // Obtener en modo de arreglo de bytes la dirección
                    byte[] bytes = direccion.GetAddressBytes();
                    // Variable que tendra la dirección visible
                    string mac_address = string.Empty;
                    // Recorrer todos los bytes de la direccion
                    for (i = 0; i < bytes.Length; i++)
                    {
                        // Pasar el byte a un formato legible para el usuario
                        mac_address += bytes[i].ToString("X2");
                        if (i != bytes.Length - 1)
                        {
                            // Agregar un separador, por formato
                            mac_address += "-";
                        }
                    }
                    // Agregar la direccion MAC a la lista
                    DireccionesMAC.Add(mac_address);
                }
            }
            // Valor de retorno, la lista de direcciones MAC
            return DireccionesMAC;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnviarDatos(txtuser.Text,txtpass.Text);
           // MostrarFrmPrincipal();
        }
        public void MostrarFrmPrincipal()
        {
            // EMC -- HS//
            funciones Funcion = new funciones();
            Funcion.CrearDirectorio();
            //**********************************//
            frmPrincipal frmMain = new frmPrincipal();
            frmMain.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblMensajeError.Text = "";
            txtuser.Text = "";
            txtpass.Text = "";
            txtuser.Focus();
            timer1.Stop();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Send the URL to the operating system.
            System.Diagnostics.Process.Start(e.Link.LinkData as string);
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            // Add a link to the LinkLabel.
            //LinkLabel.Link link = new LinkLabel.Link();
            //link.LinkData = "http://www.ctec.com.ar/";

            linkLabel1.Text = "www.ctec.com.ar";
            linkLabel1.Links.Add(0,linkLabel1.Text.Length,"http://www.ctec.com.ar/");
        }
    }
}
