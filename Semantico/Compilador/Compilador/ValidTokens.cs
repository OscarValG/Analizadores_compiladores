using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    class ValidTokens
    {

        private List<Token> tokens;
        public string errorT;
        public int i, linea, lineaError = 0, llaves = 1;
        private string aux = "";
        Lectura seman;

        public ValidTokens(List<Token> tokens)
        {
            this.tokens = tokens;
            errorT = "";
            seman = new Lectura();
        }

        public void validarTokens()
        {
            try
            {
                int lim = 4;
                if (tokens.Count() > 4)
                    for (i = 0; i < lim; i++)
                    {
                        String tok = tokens[i].tok;
                        if (tok.Equals("//")) lim++;
                        else aux += tok;
                    }
                if (!aux.Equals("main(){"))
                {
                    errorT += "El codigo debe comenzar con el bloque inicial main(){...}\n";
                    return;
                }
                bool puedeElse = false;
                while (i < tokens.Count())
                {
                    linea = tokens[i].linea;
                    switch (getNextLex())
                    {
                        case "keyR":
                            if (--llaves == 0)
                            {
                                if (i != tokens.Count())
                                    error("NO puede haber codigo fuera del bloque main");
                            }
                            else
                            {
                                if (puedeElse)
                                {
                                    if (getNextLex().Equals("else"))
                                    {
                                        if (!getNextLex().Equals("keyL"))
                                            error("Se esperaba una llave despues del else");
                                    }
                                    else i--;
                                    puedeElse = false;
                                }
                            }
                            break;
                        case "int":
                            if (!intSintax())
                                error("Error en la declaracion de variable tipo entero");
                            break;

                        case "String":
                            if (!stringSintax())
                                error("Error en la declaracion de variable tipo string");
                            break;
                        case "while":
                            if (!whileSintax())
                                error("Error en la instruccion while  ");
                            else llaves++;
                            break;
                        case "if":
                            if (!ifSintax())
                                error("Error en la instruccion if");
                            else puedeElse = true;
                            break;
                        case "printf":
                            if (!printfSintax())
                                error("Error en la instruccion printf");
                            break;
                        case "scanf":
                            if (!scanfSintax())
                                error("Error en la instruccion scanf");
                            break;
                        case "var":
                            if (!asignacion())

                                error("Instruccion no valida");
                            break;
                        case "else":
                            error("Else sin bloque if");
                            break;
                        case "comen":
                            break;
                        case "break":
                            if (!getNextLex().Equals("final"))
                                error("Se esperaba un punto y coma despues del break");
                            if (llaves == 1) error("break en lugar NO valido");
                            break;
                        default: error("No es una instruccion valida"); break;
                    }
                }
            }
            catch (Exception e) { error("NO puede haber codigo fuera del bloque main"); }
        }

        //Listo
        private bool intSintax()
        {
            String varName = tokens[i].tok;
            if (getNextLex().Equals("var"))
            {
                aux = getNextLex();
                if (aux.Equals("asig") && operacion())
                {
                    int j = -2;
                    while (!(tokens[i + j].tok).Equals("=")) j--;
                    j++;
                    String valor = "", v;
                    while (!(v = tokens[i + j].tok).Equals(";"))
                    {
                        j++;
                        valor += v;
                    }
                    if (!seman.add(varName, "int", valor))
                        Lectura.error += "Linea " + tokens[i - 1].linea + ": Uso de variable no declarada\n";
                    return true;
                }
                else if (aux.Equals("final"))
                {
                    seman.add(varName, "int", "");
                    return true;
                }
            }
            return false;
        }
        private bool stringSintax()
        {
            String varName = tokens[i].tok;
            if (getNextLex().Equals("var"))
            {
                aux = getNextLex();
                if (aux.Equals("asig"))
                {
                    aux = getNextLex();
                    if (aux.Equals("cad"))
                    {
                        seman.add(varName, "String", tokens[i - 1].tok.Replace("\"", ""));
                        i++;
                        return true;
                    }
                    if (aux.Equals("final"))
                        return false;
                }
                if (aux.Equals("final"))
                {
                    seman.add(varName, "String", "");
                    return true;
                }
            }
            return false;
        }
        //Listo
        private bool operacion()
        {
            bool var = true;
            while ((aux = getNextLex()) != null)
            {
                switch (aux)
                {
                    case "final":
                        return !var;
                    case "var":
                    case "num":
                        if (!var) return false;
                        var = false;
                        break;
                    case "oper":
                        String tok = tokens[--i].tok;
                        i++;
                        if (!tok.Equals("-") && !tok.Equals("+") && var)
                            return false;
                        var = true;
                        break;
                    case "parL":
                        if (!var) return false;
                        break;
                    case "parR":
                        if (var) return false;
                        if (getNextLex().Equals("final"))
                            return true;
                        i--;
                        break;
                    default: return false;
                }
            }
            return false;
        }
        //Listo
        private bool asignacion()
        {
            aux = getNextLex();
            if (aux.Equals("asig"))
                return operacion();
            return (aux.Equals("incre") || aux.Equals("decre"))
                    && getNextLex().Equals("final");
        }
        //Listo
        private bool ifSintax()
        {
            bool var = true;
            int par = 1;
            if (getNextLex().Equals("parL"))
            {
                while ((aux = getNextLex()) != null)
                {
                    switch (aux)
                    {
                        case "parR":
                            par--;
                            if (getNextLex().Equals("keyL"))
                            {
                                llaves++;
                                if (par == 0 && !var)
                                    return true;
                            }
                            else i--;
                            break;
                        case "var":
                        case "num":
                            if (!var) return false;
                            var = false;
                            break;
                        case "comp":
                        case "and":
                        case "or":
                            if (var) return false;
                            var = true;
                            break;
                        case "oper":
                            String tok = tokens[--i].tok;
                            i++;
                            if (!tok.Equals("-") && !tok.Equals("+") && var)
                                return false;
                            var = true;
                            break;
                        case "parL":
                            par++;
                            if (!var) return false;
                            break;
                        default: return false;
                    }
                }
            }
            return false;
        }
        //Listo
        private bool whileSintax()
        {
            return ifSintax();
        }

        //Problema de agrupacion con parentesis
        private bool printfSintax()
        {
            int par = 1;
            bool coma = true;
            if (getNextLex().Equals("parL") && getNextLex().Equals("cad"))
                while ((aux = getNextLex()) != null)
                    switch (aux)
                    {
                        case "parR":
                            if (--par == 0 && getNextLex().Equals("final") && coma)
                                return true;
                            break;
                        case "coma":
                            if (!coma) return false;
                            coma = false;
                            break;
                        case "var":
                        case "num":
                            if (coma) return false;
                            coma = true;
                            break;
                        case "ref":
                            if (coma || !getNextLex().Equals("var"))
                                return false;
                            coma = true;
                            break;
                        default: return false;
                    }
            return false;
        }
        //Listo
        private bool scanfSintax()
        {
            return printfSintax();
        }
        public String getNextLex()
        {
            if (i < tokens.Count())
                return tokens[i++].lex;
            else return null;
        }
        private void error(String msg)
        {
            if (lineaError != linea)
            {
                errorT += "Linea " + linea + " : " + msg + "\n";
                lineaError = linea;
            }
        }
    }
}
