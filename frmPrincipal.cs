using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace VisualVideoCTEC
{
    public partial class frmPrincipal : Form
    {
       /* string folderVideo = Application.StartupPath; //Path desde donde se ejecuta la aplicacion
        string folderVideo2 = @"video\";
        string folderVideo3 = @"C:\crypto\videos\"; //Path fijo del contenido
        string folderTp = @"C:\crypto\tp\"; //Path fijo del contenido */

        private string archivoIndice = Application.StartupPath + @"\AppFiles\indice4.info";
        private string archivoIndiceTemp = @"C:\Users\Default\AppData\WinTempApp\indice4.info";
        private string archivoEstadistica = Application.StartupPath + @"\AppFiles\stadistic.txt";
        private string folderWork = Application.StartupPath + @"\AppFiles\Work\";
        private string folderMpFour = Application.StartupPath + @"\AppFiles\MpFour\";
        private string folderFiles = Application.StartupPath + @"\AppFiles\Files\";
        private string pathName = string.Empty;
        private string fileTemp = string.Empty;
        private string dir = "C:\\Users\\Default\\AppData\\WinTempApp\\";

        public frmPrincipal()
        {
            InitializeComponent();
           // CentrarVideo(panel1.Width);
        }
        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            funciones Funcion = new funciones();
            Funcion.CrearArchivoTemporal(archivoIndice, "indice4.info");
            CargarIndiceCurso(archivoIndiceTemp);
            CargarSeguimientoAlumno();
        }
        public void CargarIndiceCurso(string pIndice)
        {
            funciones Funcion = new funciones();
            MemoryStream memFileDES = Funcion.Desencriptar(pIndice);

            string linea;
            StreamReader archivo = new StreamReader(pIndice);

            //StreamReader archivo = new StreamReader(memFileDES);
            //MessageBox.Show(archivo.ToString());
            listBox1.Items.Clear();

            while ((linea = archivo.ReadLine()) != null)
            {
                //byte[] UTF8 = Encoding.UTF8.GetBytes(linea);
                //string lineaUTF8 = Convert.ToBase64String(UTF8);

                //byte[] base64 = Convert.FromBase64String(lineaUTF8);
                //string lineaOK= Encoding.UTF8.GetString(base64);
                string lineaOK = linea;

                switch (lineaOK.Substring(0, 2))
                {
                    case "C-":
                        listBox1.Items.Add("");
                        listBox1.Items.Add(lineaOK.Remove(0, 2));
                        break;
                    case "L-":
                        listBox1.Items.Add(lineaOK.Remove(0, 2));
                        break;
                    case "P-":
                        listBox1.Items.Add(lineaOK.Remove(0, 2));
                        break;
                    case "A-":
                        listBox1.Items.Add(lineaOK.Remove(0, 2));
                        break;
                }
                //MessageBox.Show(linea.Substring(0,3));
            }
            archivo.Close();
            Funcion.BorrarArchivoTemporal(pIndice);
        }
        public void CargarSeguimientoAlumno()
        {
            string linea;
            int fila = 0;

            StreamReader Seguimiento = new StreamReader(archivoEstadistica, Encoding.UTF8);
            dataGridView1.Rows.Clear();
            char caracter = ';';

            while ((linea = Seguimiento.ReadLine()) != null)
            {
                if (fila == 0)
                {
                    dataGridView1.ColumnCount = linea.Split(Convert.ToChar(caracter)).Length;
                    //for (int i = 0; i <dataGridView1.ColumnCount; i++)
                    //{
                    string[] x = linea.Split(';');
                    dataGridView1.Rows.Add(x);
                    //}

                }
            }
            Seguimiento.Close();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                funciones Funcion = new funciones();
                //MessageBox.Show(folderVideo2 + " - "+ listBox1.SelectedItem.ToString());
                DateTime FechaHora = new DateTime();
                FechaHora = DateTime.Now;
                if (listBox1.SelectedItem.ToString().Substring(0, 7) == "Trabajo")
                {
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    //MessageBox.Show(listBox1.SelectedItem.ToString());
                    fileTemp = listBox1.SelectedItem.ToString() + ".pdf";
                    pathName = folderWork + fileTemp;
                    Funcion.CrearArchivoTemporal(pathName, fileTemp);
                    Funcion.Desencriptar(dir + fileTemp);
                    MessageBox.Show(pathName);
                    Funcion.AbrirArchivo(fileTemp);
                    GuardarEstadistica(Convert.ToString(FechaHora), listBox1.SelectedItem.ToString());
                    CargarSeguimientoAlumno();
                }
                else if (listBox1.SelectedItem.ToString().Substring(0, 7) == "Archivo")
                {
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    // MessageBox.Show(listBox1.SelectedItem.ToString());
                    fileTemp = listBox1.SelectedItem.ToString() + ".rar";
                    pathName = folderWork + fileTemp;
                    Funcion.CrearArchivoTemporal(pathName, fileTemp);
                    Funcion.Desencriptar(dir + fileTemp);
                    MessageBox.Show(pathName);
                    Funcion.AbrirArchivo(fileTemp);
                    GuardarEstadistica(Convert.ToString(FechaHora), listBox1.SelectedItem.ToString());
                }
                else
                {
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    //MessageBox.Show(listBox1.SelectedItem.ToString());
                    fileTemp = listBox1.SelectedItem.ToString() + ".mp4";
                    pathName = folderFiles + fileTemp;
                    Funcion.CrearArchivoTemporal(pathName, fileTemp);
                    Funcion.Desencriptar(dir + fileTemp);
                    MessageBox.Show(pathName);
                    axWindowsMediaPlayer1.URL = dir + fileTemp;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    GuardarEstadistica(Convert.ToString(FechaHora), listBox1.SelectedItem.ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            axWindowsMediaPlayer1.URL = null;
            funciones Funcion = new funciones();
            Funcion.BorrarAllArchivoTemporal();
            Application.Exit();
        }
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        public void GuardarEstadistica(string pLinea, string pLeccion)
        {
            try
            {
                StreamWriter FileStadistic = File.AppendText(archivoEstadistica);
                FileStadistic.WriteLine(pLinea + ";" + pLeccion);
                FileStadistic.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void btnCerrarContenidos_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            CentrarVideo();

        }
        public void CentrarVideo(int opcionalint= 0)
        {
            int anchoScreen = Screen.PrimaryScreen.WorkingArea.Width / 2;
            int anchoVideo = axWindowsMediaPlayer1.Width / 2;
            int NewPosYVideo = axWindowsMediaPlayer1.Top;
            int NewPosXVideo = anchoScreen - anchoVideo;
            if (opcionalint > 0)
            {
                axWindowsMediaPlayer1.Location = new Point(NewPosXVideo + (opcionalint/2), NewPosYVideo);
            }
            else
            {
                axWindowsMediaPlayer1.Location = new Point(NewPosXVideo, NewPosYVideo);
            }  
        }
        private void contenidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            //CentrarVideo(panel1.Width);
        }
        private void promocionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HttpWebRequest IrPromociones = (HttpWebRequest)WebRequest.Create("http://www.ctec.com.ar");
        }


    }
}
