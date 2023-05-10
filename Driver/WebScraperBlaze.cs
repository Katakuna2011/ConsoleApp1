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

namespace ConsoleApp1.Driver
{
    public class WebScraperBlaze : Web
    {
        public DataTable GetData(string link, int porcentagemAlvo, string destinoMensagem)
        {
            bool staleElement = true;

            if (driver == null)
                StartBrowser();

            List<Item> items = new List<Item>();

            Navigate(link);

            WaitForLoad();

            Thread.Sleep(5000);

            while (staleElement)
            {
                try
                {
                    //var historicoCrash = Click(TypeElement.Xpath, "//*[@id=\"crash-recent\"]/div[2]/div[1]", 5);
                    var historicoCrash = Navigate("https://blaze.com/pt/games/crash?modal=crash_history_index");

                    var crashElements = GetValue(TypeElement.Xpath, "//*[@id=\"history\"]", 5)
                        .element.FindElements(By.ClassName("bet"));


                    foreach (var item in crashElements)
                    {
                        if (items.Count <= 99)
                        {
                            Item itemCrash = new Item();

                            string multiplicador = item.FindElement(By.ClassName("bet-amount")).Text;

                            itemCrash.Multiplicador = decimal.Parse(RemoveChar(multiplicador));


                            items.Add(itemCrash);

                            staleElement = true;
                        }
                        else
                        {
                            //Console.Clear();

                            Console.WriteLine($"CRASH ATUAL: {items[0].Multiplicador}\n");

                            Thread.Sleep(3000);

                            try
                            {
                                var fechaHistoricoCrash = Click(TypeElement.Xpath, "//*[@id=\"root\"]/main/div[3]/div/div[1]/i", 5);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Janela já fechada");
                            }

                            if (HasTheFirstFiveChanged())
                            {
                                var newCrashElement = GetValue(TypeElement.Xpath, "//*[@id=\"crash-recent\"]/div[2]/div[2]/span[1]").Value;
                                Item newItemCrash = new Item(decimal.Parse(RemoveChar(newCrashElement)));
                                items.RemoveAt(99);
                                items.Insert(0, newItemCrash);

                                var prob = new Probabilidade(GetProbability(items));
                                Console.WriteLine(prob.ToString() + "\n");

                                if (prob.ProbabilidadeAtual == porcentagemAlvo)
                                {
                                    new WhatsAppMessage().SendMessage(prob.ToString(), "Bruno");
                                }
                            }

                            //Console.ReadKey();
                        }
                    }
                }
                catch (Exception)
                {
                    staleElement = false;
                    CloseBrowser();
                    throw;
                }
            }


            // double probabilidadeDeCrashAtual = GetProbability(items);

            //var prob = new Probabilidade(GetProbability(items));

            // Console.WriteLine(prob.ToString());

            Console.ReadKey();

            CloseBrowser();

            return Base.ConvertTo(items);
        }
        public string RemoveChar(string texto)
        {
            var str = texto;
            var charsToRemove = new string[] { " ", "x", "X" };
            foreach (var @char in charsToRemove)
            {
                str = str.Replace(@char, string.Empty);
            }
            return str;
        }

        public double GetProbability(List<Item> lista)
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

        public bool HasTheFirstFiveChanged()
        {
            List<string> listaTeste = new List<string>();
            bool noChanges = true;
            while (noChanges)
            {
                List<string> fiveRecent = new List<string>() {
                GetValue(TypeElement.Xpath, "//*[@id=\"crash-recent\"]/div[2]/div[2]/span[1]").Value,
                GetValue(TypeElement.Xpath, "//*[@id=\"crash-recent\"]/div[2]/div[2]/span[2]").Value,
                GetValue(TypeElement.Xpath, "//*[@id=\"crash-recent\"]/div[2]/div[2]/span[3]").Value,
                GetValue(TypeElement.Xpath, "//*[@id=\"crash-recent\"]/div[2]/div[2]/span[4]").Value,
                GetValue(TypeElement.Xpath, "//*[@id=\"crash-recent\"]/div[2]/div[2]/span[5]").Value
                };

                if (listaTeste.Count < 5)
                {
                    listaTeste = fiveRecent;
                }

                for (int i = 0; i < fiveRecent.Count; i++)
                {
                    if (fiveRecent[i] != listaTeste[i])
                    {
                        noChanges = false;
                    }
                }
            }
            return true;
        }
    }
}
