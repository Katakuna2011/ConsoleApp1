using EasyAutomationFramework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1.Driver
{
    public class WhatsAppMessage : Web
    {
        public void SendMessage(string message, string to)
        {
            StartBrowser(TypeDriver.GoogleChorme, "C:\\Users\\Administrator\\AppData\\Local\\Google\\Chrome\\User Data");

            Navigate("https://web.whatsapp.com/");

            WaitForLoad();

            Thread.Sleep(TimeSpan.FromSeconds(4));

            try
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));

                var elementSearch = AssignValue(TypeElement.Xpath, "//*[@id=\"side\"]/div[1]/div/div/div[2]/div/div[1]", to, 5);
                elementSearch.element.SendKeys(Keys.Enter);

                var elementMessage = AssignValue(TypeElement.Xpath, "//*[@id=\"main\"]/footer/div[1]/div/span[2]/div/div[2]/div[1]/div/div[1]/p", message);
                elementMessage.element.SendKeys(Keys.Enter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                CloseBrowser();
            }
        }
    }
}
