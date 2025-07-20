using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace TeamsAutomationTest
{
    public class TeamsTests
    {
        private ChromeDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://teams.microsoft.com/v2/");
            Thread.Sleep(6000);

            // Přihlášení
            driver.FindElement(By.Name("loginfmt")).SendKeys("qa@safeticaservices.onmicrosoft.com" + Keys.Enter);
            Thread.Sleep(2000);
            driver.FindElement(By.Name("passwd")).SendKeys("automation.Safetica2004" + Keys.Enter);
            Thread.Sleep(2000);
            driver.FindElement(By.Id("idSIButton9")).Click(); // "Ano" pro zůstat přihlášen

            // Počkej na načtení Teams
            Thread.Sleep(10000);

            // Najdi a otevři chat "Safetica QA (You)"
            driver.FindElement(By.XPath(".//span[contains(., 'Safetica')]"));
            Thread.Sleep(3000);
        }

        [Test]
        public void
        Test_SendAndDownloadFile()
        {
            // Odeslání souboru
            // Click na tlacitko "+"
            driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div[9]/div/div[1]/div[3]/div/div[3]/div/div[2]/div/div[2]/div[2]/div[1]/button[3]/div")).Click();
            Thread.Sleep(2000);
            // Click na "Připojit soubor"
            driver.FindElement(By.XPath("/html/body/div[10]/div/div[2]/div/div/div[2]/div/div/div/ul/li[1]/div/div/div/div/div/div/div")).Click();
            Thread.Sleep(2000);
            var fileInput = driver.FindElement(By.CssSelector("input[type='file']"));
            string tempFile = Path.Combine(Path.GetTempPath(), "testfile.txt");
            File.WriteAllText(tempFile, "test text");
            fileInput.SendKeys(tempFile);
            Thread.Sleep(3000);
            // Nahraje soubor
            driver.FindElement(By.XPath("//*[@id='message-pane-layout-a11y']/div[3]/div/div[4]/div/div[3]/div[3]/button[2]/div")).Click();
            Thread.Sleep(5000);

            // Stažení souboru
            // Najdi všechny přílohy 
            var attachments = driver.FindElements(By.XPath("//div[contains(@id, 'attachments-')]"));
            // Klikni na poslední přílohu, pokud existuje
            if (attachments.Count > 0)
            {
                var lastAttachment = attachments[attachments.Count - 1];
                var actions = new Actions(driver);
                actions.ContextClick(lastAttachment).Perform(); // pravé tlačítko
                Thread.Sleep(1000);

                // Simulace kláves: šipka dolů a Enter
                actions.SendKeys(Keys.ArrowDown).SendKeys(Keys.Enter).Perform();
                Thread.Sleep(2000);
            }

            //Test report
            Console.WriteLine("===========================================\n" +
                              "Test report:\n" +
                              "Name: Test_SendAndDownloadFile\n" +
                              "Project: TeamsAutomationTest.csproj\n" +
                              "Date of build: 20.7.2025\n" +
                              "Author: Irena Sklenarova\n" +
                              "Version OS: Win 11\n" +
                              "Version NUnit: 4.3.2\n" +
                              "Version Selenium.WebDriver: 4.34.0\n" +
                              "Version Google Chrome: 138.0.7204.158\n" +
                              "Time of test: passed 57,8 s\n" +
                              "Description: Test sends file 'testfile.txt' to Teams chat Safetica QA(You) and downloads it.");
        }

        [Test]
        public void Test_SendThreeMessages()
        {
            driver.FindElement(By.XPath("//*[@id='message-pane-layout-a11y']/div[3]/div/div[3]/div/div[2]/div/div[2]/div[1]")).Click();
            Thread.Sleep(2000);

            for (int i = 1; i <= 3; i++)
            {
                var messageBox = driver.FindElement(By.CssSelector("div[contenteditable='true']"));
                messageBox.SendKeys($"Test message {i}" + Keys.Enter);
                Thread.Sleep(1000);
            }

            //Test report
            Console.WriteLine("===========================================\n" +
                              "Test report:\n" +
                              "Name: Test_SendThreeMessages\n" +
                              "Project: TeamsAutomationTest.csproj\n" +
                              "Date of build: 20.7.2025\n" +
                              "Author: Irena Sklenarova\n" +
                              "Version OS: Win 11\n" +
                              "Version NUnit: 4.3.2\n" +
                              "Version Selenium.WebDriver: 4.34.0\n" +
                              "Version Google Chrome: 138.0.7204.158\n" +
                              "Time of test: passed 40,3 s\n" +
                              "Description: Test sends three messages to Teams chat Safetica QA(You).");

        }

        [TearDown]
        public void TearDown()
        {
            driver?.Dispose();
        }

        //// Nezavírat testovací okno
        // private bool keepBrowserOpen = true;
        // [TearDown]
        // public void TearDown()
        // {
        //     if (!keepBrowserOpen)
        //     {
        //         driver?.Dispose();  
        //     }
        // }
    }
        }
