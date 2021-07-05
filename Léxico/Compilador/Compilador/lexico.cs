using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compilador
{
    class Lexico
    {
        static string ruta;
        static string value;
        static int countLine;
        static bool cadena = false;
        static bool errId = false;
        static bool errNum = false;
        static bool real = false;
        static bool punto = false;
        public Lexico(string path)
        {
            ruta = @path;
            try
            {
                StreamReader leer = new StreamReader(ruta);
                cleanFile();
                countLine = 0;
                while (!leer.EndOfStream)
                {
                    countLine++;
                    if (!cadena)
                        value = "";
                    string datosLinea = leer.ReadLine();
                    inicio(datosLinea, 0);
                }
                if (cadena)
                {
                    writeError(value, "No se encontro el cierre de cadena");
                }
                leer.Close();
            }
            catch
            {
                StreamWriter crearArchivo = new StreamWriter(ruta);
                crearArchivo.Close();
            }
        }

        private void cleanFile()
        {
            string ruta = @"..\lexico.cmp";
            StreamWriter escribir = new StreamWriter(ruta, false);
            escribir.Write("");
            escribir.Close();
            ruta = @"..\errlexico.err";
            StreamWriter escribirErr = new StreamWriter(ruta, false);
            escribirErr.Write("");
            escribirErr.Close();
        }

        private void inicio(string linea, int pos)
        {
            if (cadena)
                q3(linea, pos);
            char x;
            char y;
            try
            {
                x = linea[pos];
            }
            catch
            {
                x = '\n';
            }

            if (x == '\n') //Salto de linea - Sin Tipo
                return;
            else if (Char.IsWhiteSpace(x)) //Espacio en blanco - Sin Tipo
                space(linea, pos + 1);
            else if (Char.IsLetter(x)) //Letra - Tipo:[0,4,19,20,21,22]
            {
                value += x;
                q0(linea, pos + 1);
            }
            else if (Char.IsDigit(x)) //Digito - Tipo:[1,2]
            {
                value += x;
                q1(linea, pos + 1);
            }
            else if (x == '+' || x == '-') //Operador Suma - Tipo:5
            {
                value += x;
                writeFile(value, "5");
                value = "";
                inicio(linea, pos + 1);
            }
            else if (x == '*' || x == '/') //Operador de multiplicación - Tipo:6
            {
                value += x;
                writeFile(value, "6");
                value = "";
                inicio(linea, pos + 1);
            }
            else if (x == '<' || x == '>') //Operador Relacional - Tipo:7
            {
                try
                {
                    y = linea[pos + 1];
                }
                catch
                {
                    y = '\n';
                }
                if (y != '\n' && linea[pos + 1] == '=')
                {
                    value += x + linea[pos + 1];
                    writeFile(value, "7");
                    value = "";
                    inicio(linea, pos + 2);
                }
                else
                {
                    value += x;
                    writeFile(value, "7");
                    value = "";
                    inicio(linea, pos + 1);
                }
            }
            else if (x == '|') //Operador Or - Tipo:8
            {
                try
                {
                    y = linea[pos + 1];
                }
                catch
                {
                    y = '\n';
                }
                if (y != '\n' && linea[pos + 1] == '|')
                {
                    value += x + linea[pos + 1];
                    writeFile(value, "8");
                    value = "";
                    inicio(linea, pos + 2);
                }
                else
                {
                    writeError(value, "El caracter '|' en la linea " + countLine + " en la posición " + pos + "no puede estar solo");
                }
            }
            else if (x == '&') //Operador And - Tipo:9
            {
                try
                {
                    y = linea[pos + 1];
                }
                catch
                {
                    y = '\n';
                }
                if (y != '\n' && linea[pos + 1] == '&')
                {
                    value += x + linea[pos + 1];
                    writeFile(value, "9");
                    value = "";
                    inicio(linea, pos + 2);
                }
                else
                {
                    writeError(value, "El caracter '&' en la linea " + countLine + " en la posición " + pos + "no puede estar solo");
                }
            }
            else if (x == '!') //Operador Not - Tipo:[10,11]
            {
                try
                {
                    y = linea[pos + 1];
                }
                catch
                {
                    y = '\n';
                }
                if (y != '\n' && linea[pos + 1] == '=')
                {
                    value += x + linea[pos + 1];
                    writeFile(value, "11");
                    value = "";
                    inicio(linea, pos + 2);
                }
                else
                {
                    value += x;
                    writeFile(value, "10");
                    value = "";
                    inicio(linea, pos + 1);
                }
            }
            else if (x == '=') //Operador de igualdad - Tipo:[11,18]
            {
                try
                {
                    y = linea[pos + 1];
                }
                catch
                {
                    y = '\n';
                }
                if (y != '\n' && linea[pos + 1] == '=')
                {
                    value += x + linea[pos + 1];
                    writeFile(value, "11");
                    value = "";
                    inicio(linea, pos + 2);
                }
                else
                {
                    value += x;
                    writeFile(value, "18");
                    value = "";
                    inicio(linea, pos + 1);
                }
            }
            else if (x == ';') //Punto y Coma - Tipo:12
            {
                value += x;
                writeFile(value, "12");
                value = "";
                inicio(linea, pos + 1);
            }
            else if (x == ',') //Coma - Tipo - Tipo:13
            {
                value += x;
                writeFile(value, "13");
                value = "";
                inicio(linea, pos + 1);
            }
            else if (x == '(') //Parentesis Abierto - Tipo:14
            {
                value += x;
                writeFile(value, "14");
                value = "";
                inicio(linea, pos + 1);
            }
            else if (x == ')') //Parentesis Cerrado - Tipo:15
            {
                value += x;
                writeFile(value, "15");
                value = "";
                inicio(linea, pos + 1);
            }
            else if (x == '{') //Llave Abierta - Tipo:16
            {
                value += x;
                writeFile(value, "16");
                value = "";
                inicio(linea, pos + 1);
            }
            else if (x == '}') //Llave Cerrada - Tipo:17
            {
                value += x;
                writeFile(value, "17");
                value = "";
                inicio(linea, pos + 1);
            }
            else if (x == '\"')
            {
                cadena = true;
                value += x;
                q3(linea, pos + 1);
            }
            else
            {
                value += x;
                writeError(value, "caracter no identidicado - " + value);
                value = "";
                inicio(linea, pos + 1);
            }
        }



        private void writeError(string value, string error)
        {
            string ruta = @"..\errlexico.err";
            StreamWriter escribir = new StreamWriter(ruta, true);
            escribir.WriteLine(error);
            escribir.Close();
        }

        private void writeFile(string value, string tipo)
        {
            string ruta = @"..\lexico.cmp";
            StreamWriter escribir = new StreamWriter(ruta, true);
            escribir.WriteLine(value + " - " + tipo);
            escribir.Close();
        }

        private void space(string linea, int pos)
        {
            char x;
            try
            {
                x = linea[pos];
            }
            catch
            {
                x = '\n';
            }
            if (Char.IsWhiteSpace(x))
                space(linea, pos + 1);
            else if (x == '\n')
                return;
            else
                inicio(linea, pos);
        }

        private void q0(string linea, int pos)
        {
            char x;
            try
            {
                x = linea[pos];
            }
            catch
            {
                x = '\n';
            }
            if (x == '\n' || Char.IsWhiteSpace(x))
            {
                if (errId)
                {
                    writeError(value, "La cadena \'" + value + "\' no es permitido como parte del identificador");
                    errId = false;
                }
                else if (value == "int" || value == "float" || value == "void")
                    writeFile(value, "4");
                else if (value == "if")
                    writeFile(value, "19");
                else if (value == "else")
                    writeFile(value, "22");
                else if (value == "while")
                    writeFile(value, "20");
                else if (value == "return")
                    writeFile(value, "21");
                else
                    writeFile(value, "0");

                value = "";
                if (x == '\n')
                    return;
                else
                    space(linea, pos + 1);
            }
            else if (Char.IsLetterOrDigit(x) && !errId)
            {
                value += x;
                q0(linea, pos + 1);
            }
            else
            {
                value += x;
                errId = true;
                q0(linea, pos + 1);
            }
        }
        private void q1(string linea, int pos)
        {
            char x;
            try
            {
                x = linea[pos];
            }
            catch
            {
                x = '\n';
            }
            
            if(x == '\n' || Char.IsWhiteSpace(x))
            {
                if (errNum)
                {
                    writeError(value, "No se acepta como numero entero o real - " + value);
                    errNum = false;
                    punto = false;
                    real = false;
                    value = "";
                }
                else if (real)
                {
                    writeFile(value, "2");
                    real = false;
                    punto = false;
                }
                else
                    writeFile(value, "1");

                if (x == '\n')
                    return;
                else
                {
                    value = "";
                    space(linea, pos + 1);
                }
            }
            if (Char.IsDigit(x))
            {
                value += x;
                q1(linea, pos + 1);
            }
            else if (x == '.' && !punto)
            {
                value += x;
                real = true;
                punto = true;
                q1(linea, pos + 1);
            }
            else
            {
                value += x;
                errNum = true;
                q1(linea, pos + 1);
            }
        }

        private void q3(string linea, int pos)
        {
            char x;
            char y;
            try
            {
                x = linea[pos];
            }
            catch
            {
                x = '\n';
            }
            if (x == '\n')
            {
                value += " ";
                return;
            }
            else if (x == '\"')
            {
                value += x;
                writeFile(value, "3");
                cadena = false;
            }
            else
            {
                value += x;
                q3(linea, pos + 1);
            }
        }
    }
}
