using System.Xml;
using static System.Console;
namespace WebSite;

public class WebSiteManager
{
    public void GetAndSaveToXml(string filepath)
        {
            var doc = new System.Xml.XmlDocument();
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
                var type = doc.CreateElement("Type");
                string typeOptins = "Advertising - 1,\n    News - 2,\n    Portal - 3,\n    Mirror - 3";
                WebSiteType selectedType = SelectInfoFromUserInput(GetInfoFromUserInput(typeOptins, 4));
                type.InnerText = selectedType.ToString();
                page.AppendChild(type);
                
                var chars = doc.CreateElement("Chars");
                
                if(type.InnerText == WebSiteType.Portal.ToString() || 
                   type.InnerText == WebSiteType.News.ToString() || 
                   type.InnerText == WebSiteType.Mirror.ToString())
                {
                    Write("HasEmail (true/false): ");
                    var hasEmail = doc.CreateElement("HasEmail");
                    hasEmail.InnerText = ReadLine()!;
                    chars.AppendChild(hasEmail);
                }
                
                if(type.InnerText == WebSiteType.News.ToString())
                {
                    Write("HasNews (true/false): ");
                    var hasNews = doc.CreateElement("HasNews");
                    hasNews.InnerText = ReadLine()!;
                    chars.AppendChild(hasNews);
                }
                
                if(type.InnerText == WebSiteType.Mirror.ToString())
                {
                    Write("HasArchive (true/false): ");
                    var hasArchive = doc.CreateElement("HasArchive");
                    hasArchive.InnerText = ReadLine()!;
                    chars.AppendChild(hasArchive);
                }

                Write("PaidContent (true/false): ");
                var paidContent = doc.CreateElement("PaidContent");
                paidContent.InnerText = ReadLine()!;
                chars.AppendChild(paidContent);

                Write("HasVoting Available (true/false): ");
                var voting = doc.CreateElement("HasVoting");
                var available = doc.CreateElement("Available");
                available.InnerText = ReadLine()!;
                voting.AppendChild(available);

                Write("Voting Type: ");
                var votingType = doc.CreateElement("Type");
                votingType.InnerText = ReadLine()!;
                voting.AppendChild(votingType);
                
                Write("Authorize (true/false): ");
                var authorize = doc.CreateElement("Authorize");
                authorize.InnerText = ReadLine()!;
                page.AppendChild(authorize);


                chars.AppendChild(voting);
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
        switch (value)
        {
            case 1:
                return WebSiteType.Advertising;
            case 2:
                return WebSiteType.News;
            case 3:
                return WebSiteType.Portal;
            case 4:
                return WebSiteType.Mirror;
            default:
                throw new Exception("Invalid value provided.");
        }
    }
}
public enum WebSiteType
{
    Advertising,
    News,
    Portal,
    Mirror
}