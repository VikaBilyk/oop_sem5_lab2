// See https://aka.ms/new-console-template for more information

using System.Xml.Linq;
using WebSite;

class Program
{
    static void Main()
    {
        string filePath = "/Users/viktoriyabilyk/RiderProjects/WebSite/WebSite/WebSite.xml";
        
        XDocument doc = XDocument.Load(filePath);
        
        XMLReader xmlReader = new XMLReader();
        List<Page> pages = xmlReader.ParseXml(filePath);

        foreach (var page in pages)
        {
            Console.WriteLine($"Title: {page.Title}, Type: {page.Type}, Authorize: {page.Authorize}");
            Console.WriteLine($"HasEmail: {page.Chars.HasEmail}, HasNews: {page.Chars.HasNews}, PaidContent: {page.Chars.PaidContent}");
            if (page.Chars.hasArchives != null)
            {
                Console.WriteLine($"HasArchives - Available: {page.Chars.hasArchives.Available}, Type: {page.Chars.hasArchives.Type}");
            }
        }
        
    }
}