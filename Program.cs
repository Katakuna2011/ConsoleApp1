using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Driver;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Digite a porcentagem alvo: ");
                string alvo = Console.ReadLine();

                Console.Write("Digite o destino da mensagem (conforme salvo no WhatsApp): ");
                string destino = Console.ReadLine();
                var web = new WebScraperBlaze();
                web.GetData("https://blaze.com/pt/games/crash", alvo, destino);
            }
            catch (Exception e)
            {
                Console.WriteLine("Houve um erro...");
                Console.WriteLine($"{e.Message}\n, {e.InnerException}\n, {e.Source}\n");
                throw new Exception(e.Message, e.InnerException);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
