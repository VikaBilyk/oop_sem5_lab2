// change voting
using System.Xml;
using static System.Console;

namespace WebSite;

public class WebSiteManager
{
    public void GetAndSaveToXml(string filepath)
    {
        var doc = new XmlDocument();
        var root = doc.CreateElement("Site");
        doc.AppendChild(root);

        WriteLine("Скільки сторінок ви хочете додати?");
        int pageCount = int.Parse(ReadLine()!);

        for (int i = 0; i < pageCount; i++)
        {
            var page = doc.CreateElement("Page");

            Write("Title: ");
            var title = doc.CreateElement("Title");
            title.InnerText = ReadLine()!;
            page.AppendChild(title);

            Write("Type: ");
            string typeOptions = "Advertising - 1,\nNews - 2,\nPortal - 3,\nMirror - 4";
            WebSiteType selectedType = SelectInfoFromUserInput(GetInfoFromUserInput(typeOptions, 4));
            var type = doc.CreateElement("Type");
            type.InnerText = selectedType.ToString();
            page.AppendChild(type);

            var chars = doc.CreateElement("Chars");

            // Додавання характеристик залежно від типу вебсайту
            if (selectedType == WebSiteType.Portal || 
                selectedType == WebSiteType.News || 
                selectedType == WebSiteType.Mirror)
            {
                Write("HasEmail (true/false): ");
                var hasEmail = doc.CreateElement("HasEmail");
                hasEmail.InnerText = ReadLine()!;
                chars.AppendChild(hasEmail);
            }

            if (selectedType == WebSiteType.News)
            {
                Write("HasNews (true/false): ");
                var hasNews = doc.CreateElement("HasNews");
                hasNews.InnerText = ReadLine()!;
                chars.AppendChild(hasNews);
            }

            if (selectedType == WebSiteType.Mirror)
            {
                Write("HasArchive (true/false): ");
                var hasArchive = doc.CreateElement("HasArchive");
                hasArchive.InnerText = ReadLine()!;
                chars.AppendChild(hasArchive);
            }

            // Додавання елементів голосування
            Write("Has voting (true/false): ");
            bool hasVotingBool = bool.Parse(ReadLine()!);
            var hasVoting = doc.CreateElement("HasVoting");
            hasVoting.InnerText = hasVotingBool.ToString();
            chars.AppendChild(hasVoting);

            if (hasVotingBool)
            {
                Write("Anonymous (true/false): ");
                var anonymous = doc.CreateElement("Anonymous");
                anonymous.InnerText = ReadLine()!;
                chars.AppendChild(anonymous);
            }
            else
            {
                WriteLine("The voting requires authorization");
                var authorization = doc.CreateElement("Authorization");
                authorization.InnerText = "true";
                chars.AppendChild(authorization);
            }

            // Інші характеристики
            Write("PaidContent (true/false): ");
            var paidContent = doc.CreateElement("PaidContent");
            paidContent.InnerText = ReadLine()!;
            chars.AppendChild(paidContent);

            Write("Authorize (true/false): ");
            var authorize = doc.CreateElement("Authorize");
            authorize.InnerText = ReadLine()!;
            page.AppendChild(authorize);

            page.AppendChild(chars);
            root.AppendChild(page);
        }

        doc.Save(filepath);
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

            // Ensure we have a valid input
            if (!int.TryParse(userInput, out value) || value < 1 || value > max)
            {
                WriteLine($"Please enter a valid number between 1 and {max}.");
                continue; // Re-prompt for input
            }

            exit = true; // Valid input
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
