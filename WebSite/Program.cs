using System.Xml;
using System.Xml.Linq;
using WebSite;
using XmlReader = WebSite.XmlReader;

class Program
{
    static void Main()
    {
        string filePath = "/Users/viktoriyabilyk/RiderProjects/WebSite/WebSite/WebSite.xml";
        
        WebSiteManager webSiteManager = new WebSiteManager();
        webSiteManager.GetAndSaveToXml(filePath);
        
        // Читання даних за допомогою XmlReader
        XmlReader xmlReader = new XmlReader();
        List<Page> pagesFromXmlReader = xmlReader.ParseXml(filePath);
        Console.WriteLine("*************************************");
        Console.WriteLine("Читання даних за допомогою XmlReader:");
        Console.WriteLine("*************************************");
        DisplayPages(pagesFromXmlReader);

        
        XmlDocumentHandler doc = new XmlDocumentHandler();
        // Читання даних за допомогою XmlDocument
        List<Page> pagesFromXmlDocument = doc.ParseXml(filePath);
        Console.WriteLine("*************************************");
        Console.WriteLine("Читання даних за допомогою XmlDocument:");
        Console.WriteLine("*************************************");
        DisplayPages(pagesFromXmlDocument);
    }
    
    static void DisplayPages(List<Page> pages)
    {
        foreach (var page in pages)
        {
            Console.WriteLine($"Title: {page.Title}, Type: {page.Type}, Authorize: {page.Authorize}");
            Console.WriteLine($"HasEmail: {page.Chars.HasEmail}, HasNews: {page.Chars.HasNews}, PaidContent: {page.Chars.PaidContent}");
                
            if (page.Chars.HasVoting != null && page.Chars.HasVoting == true)
            {
                Console.WriteLine($"Anonymous: {page.Chars.Anonymous}, Authorization: {page.Chars.Authorization}");
            }
            Console.WriteLine("____________________________________");
        }
    }
}