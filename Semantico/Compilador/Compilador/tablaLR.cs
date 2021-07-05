using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    class tablaLR
    {
        //public DataTable table = new DataTable();
        public int[,] table = new int[95,46];
        public static string linea = "";
        private static int CONT = 0;
        public void Tablas()
        {
            string ruta = @"..\tabla.lr";
            try
            {
                StreamReader fileSintactico = new StreamReader(ruta);
                List<string> fila = new List<string>();
                while ((linea = fileSintactico.ReadLine()) != null)
                {
                    string[] partes = linea.Split('\t');

                    fila.Clear();
                    for (int i = 0; i < partes.Length; i++)
                    {
                        table[CONT,i] = Convert.ToInt32(partes[i]);
                    }
                    CONT++;
                }
                fileSintactico.Dispose();
                fileSintactico.Close();
            }
            catch (Exception e)
            {

            }
        }
    }
}
