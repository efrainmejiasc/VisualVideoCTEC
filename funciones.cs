using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Windows.Forms;

namespace VisualVideoCTEC
{
    public class funciones
    {
         private  string key = "caro0103";

        // *********************************************************************************************************************************************************
        // **************************************************************************************************************************************************************
        public void CrearArchivoTemporal(string filePath , string fpt)
        {
            string filePathTemp = "C:\\Users\\Default\\AppData\\WinTempApp\\" + fpt;
            System.IO.File.Copy(filePath, filePathTemp, true);
        }
        public void BorrarArchivoTemporal(string filePath)
        {
            File.Delete(filePath);
        }
        public void BorrarAllArchivoTemporal()
        {
            List<string> strFiles = Directory.GetFiles("C:\\Users\\Default\\AppData\\WinTempApp\\", "*").ToList();
            foreach (string fichero in strFiles)
            {
                File.Delete(fichero);
            }
        }
        public void CrearDirectorio()
        {
            string directorio= "C:\\Users\\Default\\AppData\\WinTempApp\\";
            if (!Directory.Exists(directorio))
            {
                System.IO.Directory.CreateDirectory(directorio);
            }
        }
        public void AbrirArchivo(string name)
        {
            string filePathTemp = "C:\\Users\\Default\\AppData\\WinTempApp\\" + name;
            try
            {
                System.Diagnostics.Process.Start(filePathTemp);
            }
            catch (Exception  ex){ MessageBox.Show(ex.ToString() + "open"); }
        }
        public MemoryStream Desencriptar(string pFile)
        {
            MemoryStream memStream = new MemoryStream();
            try
            {
                byte[] ArchivoADesencriptar = File.ReadAllBytes(pFile);
                //string ArchivoADesencriptar = File.ReadAllText(pFile);
                var DES = new DESCryptoServiceProvider();
                DES.IV = Encoding.UTF8.GetBytes(key);
                DES.Key = Encoding.UTF8.GetBytes(key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;
                CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateDecryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(ArchivoADesencriptar, 0, ArchivoADesencriptar.Length);
                cryptoStream.FlushFinalBlock();
                File.WriteAllBytes(pFile, memStream.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return memStream;
        }
        public MemoryStream Encriptar(string pFile)
        {
            MemoryStream memStream = new MemoryStream();
            try
            {
                byte[] ArchivoADesencriptar = File.ReadAllBytes(pFile);
                //string ArchivoADesencriptar = File.ReadAllText(pFile);
                var DES = new DESCryptoServiceProvider();
                DES.IV = Encoding.UTF8.GetBytes(key);
                DES.Key = Encoding.UTF8.GetBytes(key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;
                CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateEncryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(ArchivoADesencriptar, 0, ArchivoADesencriptar.Length);
                cryptoStream.FlushFinalBlock();
                File.WriteAllBytes(pFile, memStream.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return memStream;
        }
        // *********************************************************************************************************************************************************
        // **************************************************************************************************************************************************************
        public  void LimpiarTextbox(Form pForm)
        {
            foreach (Control c in pForm.Controls)
            {
                if (c.GetType().Name.ToString() == "TextBox")
                {
                    c.Text = "";
                }
            }
        }
        public  void DeshabilitarTextbox(Form pForm)
        {
            foreach (Control c in pForm.Controls)
            {
                if (c.GetType().Name.ToString() == "TextBox")
                {
                    c.Enabled = false;
                }
            }
        }


        
        
      


    }
}
