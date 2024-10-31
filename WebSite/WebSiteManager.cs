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

    WriteLine("How many pages add?");
    int pageCount = int.Parse(ReadLine()!);

    List<Page> pages = new List<Page>();

    for (int i = 0; i < pageCount; i++)
    {
        Page page = new Page();

        Write("Title: ");
        page.Title = ReadLine();

        Write("Type: ");
        string typeOptions = "Advertising - 1,\nNews - 2,\nPortal - 3,\nMirror - 4";
        page.Type = SelectInfoFromUserInput(GetInfoFromUserInput(typeOptions, 4)).ToString();

        Characteristics chars = new Characteristics();

        if (page.Type == WebSiteType.Portal.ToString() || 
            page.Type == WebSiteType.News.ToString() || 
            page.Type == WebSiteType.Mirror.ToString())
        {
            Write("HasEmail (true/false): ");
            chars.HasEmail = bool.Parse(ReadLine()!);
        }

        if (page.Type == WebSiteType.News.ToString())
        {
            Write("HasNews (true/false): ");
            chars.HasNews = bool.Parse(ReadLine()!);
        }

        if (page.Type == WebSiteType.Mirror.ToString())
        {
            Write("HasArchive (true/false): ");
            chars.HasArchives = bool.Parse(ReadLine()!);
        }

        Write("Has voting (true/false): ");
        chars.HasVoting = bool.Parse(ReadLine()!);

        if (chars.HasVoting)
        {
            Write("Anonymous (true/false): ");
            chars.Anonymous = bool.Parse(ReadLine()!);
        }
        else
        {
            chars.Authorization = true;
        }

        Write("PaidContent (true/false): ");
        chars.PaidContent = bool.Parse(ReadLine());

        Write("Authorize (true/false): ");
        page.Authorize = bool.Parse(ReadLine());

        page.Chars = chars;
        pages.Add(page);
    }

    // Сортування сторінок за назвою
    pages.Sort(new PageComparer());

    // // Очищення кореневого елемента для нових даних
    root.RemoveAll();

    // Додавання відсортованих сторінок у XML
    foreach (var page in pages)
    {
        XElement pageElement = new XElement("Page",
            new XElement("Title", page.Title),
            new XElement("Type", page.Type),
            new XElement("Authorize", page.Authorize),
            new XElement("Chars",
                new XElement("HasEmail", page.Chars.HasEmail),
                new XElement("HasNews", page.Chars.HasNews),
                new XElement("HasArchive", page.Chars.HasArchives),
                new XElement("HasVoting", page.Chars.HasVoting),
                new XElement("Anonymous", page.Chars.Anonymous),
                new XElement("Authorization", page.Chars.Authorization),
                new XElement("PaidContent", page.Chars.PaidContent)
            )
        );
        root!.Add(pageElement);
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
