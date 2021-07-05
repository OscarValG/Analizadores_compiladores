using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Compilador
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LeerArchivoCode();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            EscribirArchivo("code.cmp", Editor.Text);
        }

        private void Compilar_Click(object sender, RoutedEventArgs e)
        {
            EscribirArchivo("code.cmp", Editor.Text);
            Lexico lex = new Lexico(@"..\code.cmp");
            int cont = LlenarLexico();
            Sintactico Sin = new Sintactico(cont);
            LlenarSintactico();
            LlenarArbol();
        }

        private int LlenarLexico()
        {
            string ruta = @"..\lexico.cmp";
            ListLexico.Items.Clear();
            int cont = 0;
            try
            {
                StreamReader leer = new StreamReader(ruta);
                while (!leer.EndOfStream)
                {
                    string datos = leer.ReadLine();
                    ListLexico.Items.Add(datos) ;
                    cont++;
                }
                leer.Close();

            }
            catch
            {
                StreamWriter crearArchivo = new StreamWriter(ruta);
                crearArchivo.Close();
            }
            return cont;
        }

        private void LlenarSintactico()
        {
            string ruta = @"..\procesoSintactico.cmp";
            ListSintactico.Items.Clear();
            int cont = 0;
            try
            {
                StreamReader leer = new StreamReader(ruta);
                while (!leer.EndOfStream)
                {
                    string datos = leer.ReadLine();
                    ListSintactico.Items.Add(datos);
                }
                leer.Close();

            }
            catch
            {
                StreamWriter crearArchivo = new StreamWriter(ruta);
                crearArchivo.Close();
            }
        }

        private void LlenarArbol()
        {
            string ruta = @"..\arbolSintactico.cmp";
            ListArbolSintactico.Items.Clear();
            int cont = 0;
            try
            {
                StreamReader leer = new StreamReader(ruta);
                while (!leer.EndOfStream)
                {
                    string datos = leer.ReadLine();
                    ListArbolSintactico.Items.Add(datos);
                }
                leer.Close();

            }
            catch
            {
                StreamWriter crearArchivo = new StreamWriter(ruta);
                crearArchivo.Close();
            }
        }
        private void EscribirArchivo( string nombreArchivo, string text)
        {
            string ruta = @"..\" + nombreArchivo;
            StreamWriter escribir = new StreamWriter(ruta,false);
            escribir.Write(text);
            escribir.Close();
        }

        private void LeerArchivoCode()
        {
            string ruta = @"..\code.cmp";
            try
            {
                StreamReader leer = new StreamReader(ruta);
         
                while (!leer.EndOfStream)
                {
                    string datos = leer.ReadLine();
                    Editor.Text += datos + "\n"; 
                }
                leer.Close();
            }
            catch 
            {
                StreamWriter crearArchivo = new StreamWriter(ruta);
                crearArchivo.Close();
            }
        }

    }

}

