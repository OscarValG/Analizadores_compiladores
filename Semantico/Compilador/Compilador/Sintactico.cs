using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compilador
{
    class Sintactico
    {
        static string accion = "";
        static int TopePila = 0;
        static int TopeEntrada = -1;
        static bool fin = false;

        // Hacer una pila para la pila
        static List<int> pila = new List<int>();
        // Hacer una pila para la entrada
        static List<int> entrada = new List<int>();


        private List<string> x()
        {
            List<string> ListDatos = new List<string>();
            string ruta = @"..\lexico.cmp";
            try
            {
                StreamReader leer = new StreamReader(ruta);
                while (!leer.EndOfStream)
                {
                    string datos = leer.ReadLine();
                    ListDatos.Add(datos);
                }
                leer.Close();

            }
            catch
            {
                StreamWriter crearArchivo = new StreamWriter(ruta);
                crearArchivo.Close();
            }
            return ListDatos;
        }
        protected void Entrada(List<string> x, int contadorTope)
        {
            entrada.Add(23);
            TopeEntrada++;
            for (int i = contadorTope-1; i >= 0; i--)
            {
                string[] dato = x[i].Split('-');
                entrada.Add(Convert.ToInt32(dato[1].Trim()));
                TopeEntrada++;
            }
            string ListEntrada = "[";
            for (int i = 0; i < entrada.Count(); i++)
            {
                if (i == entrada.Count() - 1)
                    ListEntrada += " " + entrada[i];
                else
                    ListEntrada += " " + entrada[i] + ",";
            }
            ListEntrada += "]";
            //imprimir "Entrada";
            //imprimir entrada.toString() + "<-Tope"
            agregarArchivo("procesoSintactico.cmp", "Entrada: \n" + ListEntrada + "<-Tope");
        }

        protected void Pila()
        {
            pila.Add(23);
            pila.Add(0);
            TopePila = 1;
        }

        public Sintactico(int contaT)
        {
            limpiarArchivo("procesoSintactico.cmp");
            tablaLR t = new tablaLR();
            Reglas r = new Reglas();
            Pila();
            Entrada(x(),contaT);
            t.Tablas();
            r.ReglasL();
            r.Derivaciones();

            Proceso("");
            int accion = 9999;
            while(fin == false)
            {
                //Imprimir "Elemento de pila:" + pila[TopedePila] + "\t"
                //Imprimir "Elemento de entrada:" + entrada[TopedeEntrada]
                agregarArchivo("procesoSintactico.cmp", "Elemento de pila:" + pila[TopePila] + "\t");
                agregarArchivo("procesoSintactico.cmp", "Elemento de entrada:" + entrada[TopeEntrada]);
                
                accion = t.table[pila[TopePila],entrada[TopeEntrada]];
                
                if (accion == -1)
                {
                    //Imprimir "\n"
                    //Imprimir "Análisis sintactico correcto"
                    agregarArchivo("procesoSintactico.cmp", "Analisis Sintactico Correcto");
                    r.imprimeArbol();
                    fin = true;
                    return;
                }
                else if (accion == 0)
                {
                    //Imprimir "Error sintactico"
                    agregarArchivo("procesoSintactico.cmp", "Error Sintactico");
                    return;
                }
                else if (accion > 0)
                {
                    pila.Add(entrada[TopeEntrada]);
                    TopePila++;
                    entrada.RemoveAt(TopeEntrada);
                    TopeEntrada--;
                    pila.Add(accion);
                    TopePila++;
                    Proceso("d" + accion);
                }
                else if (accion < 0)
                {
                    accion += 1;
                    accion = Math.Abs(accion);
                    //Nodo del arbol
                    int auxTerm;
                    auxTerm = (r.ListaReglas[accion - 1].terminos) * 2;
                    for (int i = 0; i < auxTerm; i++)
                    {
                        pila.RemoveAt(TopePila);
                        TopePila--;
                    }
                    pila.Add(r.ListaReglas[accion - 1].id);
                    r.arbol.Add(r.deriv[accion - 1]);
                    r.contArbol++;
                    TopePila++;
                    Proceso("r" + accion);
                    //Imprimir "Elemento de pila-1: " + pila[TopePila-1] + "\t"
                    //Imprimir "Elemento de pila: " + pila[TopePila] + "\t"
                    agregarArchivo("procesoSintactico.cmp", "Elemento de pila-1: " + pila[TopePila - 1] + "\t");
                    agregarArchivo("procesoSintactico.cmp", "Elemento de pila: " + pila[TopePila] + "\t");
                    pila.Add(t.table[pila[TopePila - 1],pila[TopePila]]);
                    TopePila++;
                    Proceso("");
                }
            }
        }

        protected void Proceso(string acc)
        {
            //Imprime pila + " | " + entrada + " | " + acc
            string ListPila = "[";
            for(int i = 0; i < pila.Count(); i++)
            {
                if (i == pila.Count() - 1)
                    ListPila += " " + pila[i];
                else
                    ListPila += " " + pila[i] + ",";
            }
            ListPila += "]";
            string ListEntrada = "[";
            for (int i = 0; i < entrada.Count(); i++)
            {
                if (i == entrada.Count() - 1)
                    ListEntrada += " " + entrada[i];
                else
                    ListEntrada += " " + entrada[i] + ",";
            }
            ListEntrada += "]";
            agregarArchivo("procesoSintactico.cmp",ListPila + " | " + ListEntrada + " | " + acc);
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
