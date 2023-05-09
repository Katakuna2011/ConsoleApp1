using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Classes
{
    public class Item
    {
        public string Identificador { get; }

        public decimal Multiplicador { get; set; }



        DateTime _dataEHora;
        public DateTime DataEHora { get { return _dataEHora; } set { _dataEHora = DateTime.Now; } }

        public Item()
        {
            Identificador = Guid.NewGuid().ToString().Substring(0, 6);
        }
        public Item(decimal multiplicador)
        {
            Identificador = Guid.NewGuid().ToString().Substring(0, 6);
            Multiplicador = multiplicador;
        }

        public override string ToString()
        {
            return $"========== Blaze Crash ==========\n" +
                $">>>> Crash atual: {Multiplicador}\n" +
                $">>>> Data e hora: {DataEHora}\n";
        }
    }
}