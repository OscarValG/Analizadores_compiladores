using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compilador
{
    class Semantico
    {
        char[] operador = { '<', '>', '=', '!', '|', '&', '+', '-', '/' };
        char[] simbolos = { '{', '}', '(', ')', ';', '*', '"', '%', ',' };
        public static string error2;
        public string error;

        public Semantico()
        {
            Token.p = 0;
            Token.l = 0;
            error = "";
        }

        public List<Token> crearTokens()
        {
            List<Token> tokens = new List<Token>();
            string ruta = @"..\code.cmp";
            try
            {
                StreamReader lector = new StreamReader(ruta);
                string linea = "";
                string bufer = "";
                int i = 1;
                while ((linea = lector.ReadLine()) != null)
                {
                    for (int j = 0; j < linea.Length; j++)
                    {
                        char c = linea[j];
                        if (c == ' ' || c == '\t')
                        {
                            if (!bufer.Equals(""))
                            {
                                tokens.Add(new Token(bufer, i));
                                bufer = "";
                            }
                        }
                        else
                        {
                            if (esSimbolo(c))
                            {
                                if (!bufer.Equals(""))
                                {
                                    tokens.Add(new Token(bufer, i));
                                    bufer = "";
                                }
                                if (c == '"')
                                {
                                    int fin = linea.LastIndexOf("\"");
                                    if (fin == -1)
                                        error += "Linea " + i + ": falta cerrar comillas";
                                    string tok = linea.Substring(j, fin + 1);
                                    j += fin - j;
                                    tokens.Add(new Token(tok, i));
                                }
                                else tokens.Add(new Token("" + c, i));
                            }
                            else
                            {
                                if (esOperador(c))
                                {
                                    if (bufer.Equals("")) bufer += c;
                                    else
                                    {
                                        char ant = bufer[0];
                                        if (esOperador(ant) && seCombinan(c, ant))
                                        {
                                            bufer += c;
                                            tokens.Add(new Token(bufer, i));
                                            if (bufer.Equals("//"))
                                            {
                                                bufer = "";
                                                break;
                                            }
                                            bufer = "";
                                        }
                                        else
                                        {
                                            tokens.Add(new Token(bufer, i));
                                            bufer = "";
                                            bufer += c;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!bufer.Equals(""))
                                    {
                                        char ant = bufer[0];
                                        if (esOperador(ant))
                                        {
                                            if (ant == '\\')
                                                bufer += c;
                                            tokens.Add(new Token(bufer, i));
                                            bufer = "";
                                            if (ant == '\\')
                                                continue;
                                        }
                                    }
                                    if (Char.IsDigit(c) || Char.IsLetter(c))
                                    {
                                        if (c == 'ñ')
                                            error += "Linea " + i + ": " + c + " no es un caracter valido\n";
                                        else bufer += c;
                                    }
                                    else
                                        error += "Linea " + i + ": " + c + " no es una caracter valido\n";
                                }
                            }
                        }
                    }
                    if (!bufer.Equals(""))
                    {
                        tokens.Add(new Token(bufer, i));
                        bufer = "";
                    }
                    i++;
                }
                if (Token.p > 0) error += "Falta cerrar un parentesis\n";
                if (Token.p < 0) error += "Sobra un parentesis\n";
                if (Token.l > 0) error += "Falta cerrar una llave\n";
                if (Token.l < 0) error += "Sobra una llave\n";
            }
            catch { }
            error += error;
            return tokens;
        }

        bool seCombinan(char c, char ant)
        {
            return ((c == '=' || c == '-' || c == '/' || c == '&') == (ant == c)) || (c == '=' && (ant == '<' || ant == '>'));
        }

        bool esSimbolo(char c)
        {
            foreach (char simbolo in simbolos)
                if (c == simbolo)
                    return true;
            return false;
        }

        bool esOperador(char c)
        {
            foreach (char oper in operador)
                if (c == oper)
                    return true;
            return false;
        }
    }
}
