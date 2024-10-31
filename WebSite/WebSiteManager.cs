using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using static System.Console;

namespace WebSite
{
    public class WebSiteManager
    {
        public void GetAndSaveToXml(string filepath)
        {
            XDocument xmlDoc;

            // Завантажуємо існуючий файл або створюємо новий документ
            if (File.Exists(filepath))
            {
                xmlDoc = XDocument.Load(filepath);
            }
            else
            {
                xmlDoc = new XDocument(new XElement("Site"));
            }

            XElement root = xmlDoc.Element("Site");

            WriteLine("Скільки сторінок ви хочете додати?");
            int pageCount = int.Parse(ReadLine()!);

            for (int i = 0; i < pageCount; i++)
            {
                XElement pageElement = new XElement("Page");

                Write("Title: ");
                pageElement.Add(new XElement("Title", ReadLine()));

                Write("Type: ");
                string typeOptions = "Advertising - 1,\nNews - 2,\nPortal - 3,\nMirror - 4";
                WebSiteType selectedType = SelectInfoFromUserInput(GetInfoFromUserInput(typeOptions, 4));
                pageElement.Add(new XElement("Type", selectedType.ToString()));

                XElement charsElement = new XElement("Chars");

                // Додавання характеристик залежно від типу вебсайту
                if (selectedType == WebSiteType.Portal || 
                    selectedType == WebSiteType.News || 
                    selectedType == WebSiteType.Mirror)
                {
                    Write("HasEmail (true/false): ");
                    charsElement.Add(new XElement("HasEmail", ReadLine()));
                }

                if (selectedType == WebSiteType.News)
                {
                    Write("HasNews (true/false): ");
                    charsElement.Add(new XElement("HasNews", ReadLine()));
                }

                if (selectedType == WebSiteType.Mirror)
                {
                    Write("HasArchive (true/false): ");
                    charsElement.Add(new XElement("HasArchive", ReadLine()));
                }

                // Додавання елементів голосування
                Write("Has voting (true/false): ");
                bool hasVotingBool = bool.Parse(ReadLine()!);
                var hasVotingElement = new XElement("HasVoting", hasVotingBool);
                charsElement.Add(hasVotingElement);

                if (hasVotingBool)
                {
                    Write("Anonymous (true/false): ");
                    hasVotingElement.Add(new XElement("Anonymous", ReadLine()));
                }
                else
                {
                    WriteLine("The voting requires authorization");
                    hasVotingElement.Add(new XElement("Authorization", "true"));
                }

                // Інші характеристики
                Write("PaidContent (true/false): ");
                charsElement.Add(new XElement("PaidContent", ReadLine()));

                Write("Authorize (true/false): ");
                pageElement.Add(new XElement("Authorize", ReadLine()));

                pageElement.Add(charsElement);
                root?.Add(pageElement);
            }

            xmlDoc.Save(filepath);
            WriteLine("Дані збережено у XML файл.");
        }

        private static int GetInfoFromUserInput(string input, int max)
        {
            bool exit = false;
            int value = 0;
            while (!exit)
            {
                WriteLine(input);
                WriteLine("___________________________________");

                string? userInput = ReadLine();

                if (!int.TryParse(userInput, out value) || value < 1 || value > max)
                {
                    WriteLine($"Please enter a valid number between 1 and {max}.");
                    continue;
                }

                exit = true;
                WriteLine("___________________________________");
            }

            return value;
        }

        private static WebSiteType SelectInfoFromUserInput(int value)
        {
            return value switch
            {
                1 => WebSiteType.Advertising,
                2 => WebSiteType.News,
                3 => WebSiteType.Portal,
                4 => WebSiteType.Mirror,
                _ => throw new Exception("Invalid value provided.")
            };
        }
    }

    public enum WebSiteType
    {
        Advertising,
        News,
        Portal,
        Mirror
    }
}
