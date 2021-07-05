using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    class Lectura
    {
        static public List<Simbolo> TABSIM;
        static public string error;

        public Lectura()
        {
            TABSIM = new List<Simbolo>();
            error = "";
        }
        private int calcular(string operacion)
        {
            int tam = operacion.Length;
            string[] A = new string[100];
            string[] operaciones = new string[100];
            int pos = 0;
            int sum = 0;
            string aux = "";
            operaciones[0] = "+";
            int index_operacion = 1;
            for(int i = 0; i < tam; i++)
            {
                char c = operacion[i];
                if (c == '+' || c == '-' || c == '*' || c == '/')
                {
                    A[pos] = aux;
                    operaciones[index_operacion] = Convert.ToString(operacion[i]);
                    pos++;
                    index_operacion++;
                    aux = "";
                }
                else
                    aux += operacion[i];
            }
            A[pos] = aux;
            pos++;
            for(int i = 0; i < pos; i++)
            {
                int num = Convert.ToInt32(A[i]);
                switch (operaciones[i])
                {
                    case "+": sum += num; break;
                    case "-": sum -= num; break;
                    case "*": sum *= num; break;
                    case "/": sum /= num; break;
                }
            }
            return sum;
        }

        public bool add(string sim, string tipo, string valor)
        {
            if(tipo.Equals("int") && !valor.Equals(""))
            {
                int num = 0;
                try
                {
                    num = calcular(valor);
                }
                catch
                {
                    try
                    {
                        foreach(Simbolo simbolo in Lectura.TABSIM)
                        {
                            valor = valor.Replace(simbolo.sim, simbolo.valor);
                        }
                        num = calcular(valor);
                    }
                    catch
                    {
                        return false;
                    }
                }
                Lectura.TABSIM.Add(new Simbolo(sim, tipo, valor.ToString()));
            }
            else
            {
                Lectura.TABSIM.Add(new Simbolo(sim, tipo, valor));
            }
            return true;
        }
        public class Simbolo
        {
            public string sim, tipo, valor;
            public Simbolo(string sim, string tipo, string valor)
            {
                this.sim = sim;
                this.tipo = tipo;
                this.valor = valor;
            }

            public string[] toArray()
            {
                string[] values = { sim, tipo, valor };
                return values;
            }
        }
    }
}
