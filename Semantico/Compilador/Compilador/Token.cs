using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    class Token
    {
        public string tok;
        public int linea;
        public string lex;
        static public int p, l;


        public Token(string t, int i)
        {

            tok = t;
            linea = i;
            lex = "null";

            switch (tok)
            {
                case "+":
                    lex = "oper";
                    break;
                case "++":
                    lex = "incre";
                    break;
                case "-":
                    lex = "oper";
                    break;
                case "--":
                    lex = "decre";
                    break;
                case "*":
                    lex = "oper";
                    break;
                case "/":
                    lex = "oper";
                    break;
                case "//":
                    lex = "comen";
                    break;
                case "||":
                    lex = "or";
                    break;
                case "&&":
                    lex = "and";
                    break;
                case "<":
                    lex = "comp";
                    break;
                case "<=":
                    lex = "comp";
                    break;
                case ">":
                    lex = "comp";
                    break;
                case ">=":
                    lex = "comp";
                    break;
                case "==":
                    lex = "comp";
                    break;
                case "!=":
                    lex = "comp";
                    break;
                case "=":
                    lex = "asig";
                    break;
                case ";":
                    lex = "final";
                    break;
                case ",":
                    lex = "coma";
                    break;
                case "%":
                    lex = "mod";
                    break;
                case "&":
                    lex = "ref";
                    break;
                case "main":
                    lex = tok;
                    break;
                case "while":
                    lex = tok;
                    break;
                case "if":
                    lex = tok;
                    break;
                case "else":
                    lex = tok;
                    break;
                case "int":
                    lex = tok;
                    break;
                case "string":
                case "String":
                    lex = "String";
                    break;
                case "break":
                    lex = tok;
                    break;
                case "printf":
                    lex = tok;
                    break;
                case "scanf":
                    lex = tok;
                    break;
                case "(":
                    lex = "parL";
                    p++;
                    break;
                case ")":
                    lex = "parR";
                    p--;
                    break;
                case "{":
                    lex = "keyL";
                    l++;
                    break;
                case "}":
                    lex = "keyR";
                    l--;
                    break;
                default:
                    if(tok.StartsWith("\"") && tok.EndsWith("\""))
                    {
                        lex = "cad";
                    }
                    else
                    {
                        try
                        {
                            Convert.ToInt32(tok);
                            lex = "cad";
                        }
                        catch
                        {
                            if (Char.IsLetter(tok[0]))
                            {
                                lex = "var";
                            }
                            else
                            {
                                Semantico.error2 += "Linea " + linea + ": \"" + tok + "\" no es un token valido"; 
                            }
                        }
                    }
                    break;
            }
        }

    }
}
