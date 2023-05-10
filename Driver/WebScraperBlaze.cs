using EasyAutomationFramework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1.Classes;
using ConsoleApp1.Driver;
using javax.xml.stream.events;
using java.awt;

namespace ConsoleApp1.Driver
{
    public class WebScraperBlaze : Web
    {
        bool staleElement = true;
        List<Item> items = new List<Item>();
        string probabilidade;
        public DataTable GetData(string link, string porcentagemAlvo, string destinoMensagem)
        {

            if (driver == null)
                StartBrowser();


            Navigate(link);

            WaitForLoad();

            Thread.Sleep(2000);

            while (staleElement)
            {
                try
                {
                    Thread.Sleep(3000);
                    if(items.Count <= 99)
                    {
                        items = FindElements();
                        Console.WriteLine($"\nCinco primeiros: {items[0].Multiplicador}, {items[1].Multiplicador}, {items[2].Multiplicador}, {items[3].Multiplicador}, {items[4].Multiplicador} \n");
                        Console.WriteLine($"Cinco últimos: {items[95].Multiplicador}, {items[96].Multiplicador}, {items[97].Multiplicador}, {items[98].Multiplicador}, {items[99].Multiplicador} \n");
                        Console.WriteLine($"Probabilidade atual: {UpdateProbability(items)}%");
                    }                    

                    Thread.Sleep(3000);

                    List<Item> listaverificacao = new List<Item> ();
                    listaverificacao = HasTheListChanged(items);

                    if (listaverificacao != items)
                    {
                        items = listaverificacao;   

                        Console.WriteLine($"\nCinco primeiros: {items[0].Multiplicador}, {items[1].Multiplicador}, {items[2].Multiplicador}, {items[3].Multiplicador}, {items[4].Multiplicador} \n");
                        Console.WriteLine($"Cinco últimos: {items[95].Multiplicador}, {items[96].Multiplicador}, {items[97].Multiplicador}, {items[98].Multiplicador}, {items[99].Multiplicador} \n");
                        probabilidade = UpdateProbability(items);
                        Console.WriteLine($"Probabilidade atual: {probabilidade}%");
                    }

                    if(probabilidade == porcentagemAlvo)
                    {
                        new WhatsAppMessage().SendMessage($"Probabilidade atual: {probabilidade}%", destinoMensagem);
                    }
                }
                catch (Exception)
                {
                    staleElement = false;
                    CloseBrowser();
                    throw;
                }
            }

            Console.ReadKey();

            CloseBrowser();

            return Base.ConvertTo(items);
        }

        public List<Item> FindElements()
        {
            List<Item> listaDeItems = new List<Item>();
            var historicoCrash = Navigate("https://blaze.com/pt/games/crash?modal=crash_history_index");

            WaitForLoad();

            var crashElements = GetValue(TypeElement.Xpath, "//*[@id=\"history\"]", 5)
                .element.FindElements(By.ClassName("bet"));
            try
            {
                foreach (var item in crashElements)
                {
                    if (listaDeItems.Count <= 99)
                    {
                        Item itemCrash = new Item();

                        string multiplicador = item.FindElement(By.ClassName("bet-amount")).Text;

                        itemCrash.Multiplicador = decimal.Parse(RemoveChar(multiplicador));
                        listaDeItems.Add(itemCrash);

                        staleElement = true;
                    }
                }
                closeStatistics();
                return listaDeItems;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Source + "\n" + e.StackTrace);
                throw;
            }            
        }

        public void closeStatistics()
        {
            try
            {
                var fechaHistoricoCrash = Click(TypeElement.Xpath, "//*[@id=\"root\"]/main/div[3]/div/div[1]/i", 5);
            }
            catch (Exception)
            {
                Console.WriteLine("Janela já fechada");
            }
        }

        public string UpdateProbability(List<Item> lista)
        {
            try
            {
                var prob = new Probabilidade(GetProbability(items));                
                return prob.Porcentagem.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Source + "\n" + e.StackTrace);
                throw;
            }

            
        }

        public string RemoveChar(string texto)
        {
            try
            {
                var str = texto;
                var charsToRemove = new string[] { " ", "x", "X" };
                foreach (var @char in charsToRemove)
                {
                    str = str.Replace(@char, string.Empty);
                }
                return str;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Source + "\n" + e.StackTrace);
                throw;
            }

        }

        public double GetProbability(List<Item> lista)
        {
            try
            {
                List<Item> listaDeItems = new List<Item>();

                foreach (var item in lista)
                {
                    if (item.Multiplicador >= 2)
                    {
                        listaDeItems.Add(item);
                    }
                }

                double probabilidade = Convert.ToDouble(listaDeItems.Count) / Convert.ToDouble(lista.Count);

                return probabilidade;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Source + "\n" + e.StackTrace);
                throw;
            }
            
        }

        public List<Item> HasTheListChanged(List<Item> lista)
        {
            try
            {
                List<Item> listaItem = new List<Item>();
                listaItem = FindElements();

                Thread.Sleep(3000);

                var listaStringItems = new List<string>();
                
                var listaStringCompara = new List<string>();

                foreach (var item in lista)
                {
                    if(lista.IndexOf(item) < 5)
                    {
                        listaStringItems.Add(item.Multiplicador.ToString());
                    }
                }

                foreach (var item in listaItem)
                {
                    if(lista.IndexOf(item) < 5)
                    {
                        listaStringCompara.Add(item.Multiplicador.ToString());
                    }
                }

                foreach (var item in listaStringItems)
                {
                    string itemLista = null;
                    foreach (var item1 in listaStringCompara)
                    {
                        itemLista = item1;
                    }

                    if (item != itemLista)
                    {
                        Console.WriteLine("Houve Mudança...");
                        return listaItem;
                    }
                    else
                        break;
                }
                return lista;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Source + "\n" + e.StackTrace);
                throw;
            }           
        }
    }
}
