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
        
        XmlReader xmlReader = new XmlReader();
        List<Page> rPages = xmlReader.ParseXml(filePath);

        foreach (var page in rPages)
        {
            Console.WriteLine("XmlReader");
            Console.WriteLine($"Title: {page.Title}, Type: {page.Type}, Authorize: {page.Authorize}");
            Console.WriteLine($"HasEmail: {page.Chars.HasEmail}, HasNews: {page.Chars.HasNews}, PaidContent: {page.Chars.PaidContent}");
            if (page.Chars.HasVoting != null)
            {
                Console.WriteLine($"Anonymous: {page.Chars.Anonymous}, Authorization: {page.Chars.Authorization}");
            }
        }
        
        XmlDocumentHandler doc = new XmlDocumentHandler();
        List<Page> dPages = doc.ParseXml(filePath);
        foreach (var page in dPages)
        {
            Console.WriteLine("XmlDocument: ");
            Console.WriteLine($"Title: {page.Title}, Type: {page.Type}, Authorize: {page.Authorize}");
            Console.WriteLine($"HasEmail: {page.Chars.HasEmail}, HasNews: {page.Chars.HasNews}, PaidContent: {page.Chars.PaidContent}");
            if (page.Chars.HasVoting != null)
            {
                Console.WriteLine($"Anonymous: {page.Chars.Anonymous}, Authorization: {page.Chars.Authorization}");
            }
        }
        
    }
}