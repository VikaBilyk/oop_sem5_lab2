using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Serilog;

namespace WebSite
{
    public class WebSiteManager
    {
        private readonly ILogger _logger;

        public WebSiteManager()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public void GetAndSaveToXml(string filepath)
        {
            XDocument xmlDoc;

            // Load or create XML document
            if (File.Exists(filepath))
            {
                xmlDoc = XDocument.Load(filepath);
                _logger.Information("Loaded existing XML file: {FilePath}", filepath);
            }
            else
            {
                xmlDoc = new XDocument(new XElement("Site"));
                _logger.Information("Created new XML document.");
            }

            XElement root = xmlDoc.Element("Site");

            _logger.Information("How many pages would you like to add?");
            int pageCount = int.Parse(Console.ReadLine()!);

            List<Page> pages = new List<Page>();

            for (int i = 0; i < pageCount; i++)
            {
                Page page = new Page();

                _logger.Information("Title: ");
                page.Title = Console.ReadLine();

                string typeOptions = "Advertising - 1, News - 2, Portal - 3, Mirror - 4";
                page.Type = SelectInfoFromUserInput(GetInfoFromUserInput(typeOptions, 4)).ToString();

                Characteristics chars = new Characteristics();

                if (page.Type == WebSiteType.Portal.ToString() ||
                    page.Type == WebSiteType.News.ToString() ||
                    page.Type == WebSiteType.Mirror.ToString())
                {
                    _logger.Information("HasEmail (true/false): ");
                    chars.HasEmail = bool.Parse(Console.ReadLine()!);
                }

                if (page.Type == WebSiteType.News.ToString())
                {
                    _logger.Information("HasNews (true/false): ");
                    chars.HasNews = bool.Parse(Console.ReadLine()!);
                }

                if (page.Type == WebSiteType.Mirror.ToString())
                {
                    _logger.Information("HasArchive (true/false): ");
                    chars.HasArchives = bool.Parse(Console.ReadLine()!);
                }

                _logger.Information("Has voting (true/false): ");
                chars.HasVoting = bool.Parse(Console.ReadLine()!);

                if (chars.HasVoting)
                {
                    _logger.Information("Anonymous (true/false): ");
                    chars.Anonymous = bool.Parse(Console.ReadLine()!);
                }
                else
                {
                    chars.Authorization = true;
                }

                _logger.Information("PaidContent (true/false): ");
                chars.PaidContent = bool.Parse(Console.ReadLine());

                _logger.Information("Authorize (true/false): ");
                page.Authorize = bool.Parse(Console.ReadLine());

                page.Chars = chars;
                pages.Add(page);
            }

            // Sort and add pages to XML
            pages.Sort(new PageComparer());
            root.RemoveAll();

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
            _logger.Information("Data successfully saved to XML file at: {FilePath}", filepath);
        }

        private int GetInfoFromUserInput(string input, int max)
        {
            int value;
            do
            {
                _logger.Information("{Input}", input);
                if (!int.TryParse(Console.ReadLine(), out value) || value < 1 || value > max)
                {
                    _logger.Warning("Please enter a valid number between 1 and {Max}", max);
                }
            } while (value < 1 || value > max);

            return value;
        }

        private WebSiteType SelectInfoFromUserInput(int value) => value switch
        {
            1 => WebSiteType.Advertising,
            2 => WebSiteType.News,
            3 => WebSiteType.Portal,
            4 => WebSiteType.Mirror,
            _ => throw new ArgumentOutOfRangeException(nameof(value), "Invalid type selection.")
        };
    }

    public enum WebSiteType
    {
        Advertising,
        News,
        Portal,
        Mirror
    }
}
