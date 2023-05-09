using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Classes
{
    public class Probabilidade
    {
        double _probabilidade;
        public double ProbabilidadeAtual { get { return _probabilidade; } set { _probabilidade = _probabilidade / 100; } }

        int _porcentagem;
        //public int Porcentagem { get { return _porcentagem; } set { value = Convert.ToInt32(ProbabilidadeAtual * 100); } }

        public Probabilidade() { }
        public Probabilidade(double probabilidade)
        {
            _probabilidade = probabilidade;
            _porcentagem = Convert.ToInt32(ProbabilidadeAtual * 100);
        }
        public override string ToString()
        {
            return $">>>> Probabilidade atual: {ProbabilidadeAtual * 100}/100\n" +
                   $">>>> Porcentagem: {ProbabilidadeAtual} - {_porcentagem}%\n";
        }

    }
}
