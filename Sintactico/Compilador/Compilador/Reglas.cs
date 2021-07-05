using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    class Reglas
    {
        private int contadorReglas = 0;
        private string linea;

        public List<string> deriv = new List<string>();
        public int contArbol = 0;
        public List<string> arbol = new List<string>();

        public int id = 0;
        public int terminos = 0;
        public string nombre = "";

        public Reglas()
        {

        }

        public Reglas(int id, int terminos, string nombre)
        {
            this.id = id;
            this.terminos = terminos;
            this.nombre = nombre;
        }

        public List<Reglas> ListaReglas = new List<Reglas>();

        public void ReglasL()
        {
            string ruta = @"..\reglas.lr";
            try
            {
                StreamReader fileSintactico = new StreamReader(ruta);
                while((linea = fileSintactico.ReadLine()) != null)
                {
                    string[] partes = linea.Split('\t');
                    Reglas r = new Reglas(Convert.ToInt32(partes[0]), Convert.ToInt32(partes[1]), partes[2]);
                    ListaReglas.Add(r);
                    contadorReglas++;
                }
                fileSintactico.Dispose();
                fileSintactico.Close();
            }
            catch
            {

            }
        }

        public void Derivaciones()
        {
            string ruta = @"..\compilador20a.inf";
            try
            {
                StreamReader file = new StreamReader(ruta);
                while((linea = file.ReadLine()) != null)
                {
                    deriv.Add(linea);
                }
                file.Dispose();
                file.Close();
            }
            catch { }
        }

        public void imprimeArbol()
        {
            limpiarArchivo("arbolSintactico.cmp");
            int cont = 0;
            string linea = "";
            for(int i = contArbol - 1; i >= 0; i--)
            {
                linea = "";
                if (arbol[i].Contains("<")){
                    cont++;
                }
                for (int j = 0; j < cont; j++)
                {
                    if (i != contArbol - 1)
                    {
                        //Imprimir un \t
                        //agregarArchivo("arbolSintactico.cmp", "\t");
                        linea += "\t";
                    }
                }
                if (arbol[i].Contains("\\e"))
                {
                    cont -= 1;
                }
                //Imprimir arbol[i]
                linea += arbol[i];
                agregarArchivo("arbolSintactico.cmp", linea);
            }
        }

        void agregarArchivo(string nombreArchivo, string dato)
        {
            string ruta = @"..\" + nombreArchivo;
            StreamWriter escribir = new StreamWriter(ruta, true);
            escribir.WriteLine(dato);
            escribir.Close();
        }

        void limpiarArchivo(string nombreArchivo)
        {
            string ruta = @"..\" + nombreArchivo;
            StreamWriter escribir = new StreamWriter(ruta, false);
            escribir.Write("");
            escribir.Close();
        }

    }
}


