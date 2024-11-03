using System;
using System.Xml;
using System.Xml.Linq;
using WebSite;
using System.Xml.Xsl;
using System.IO;
using System.Xml.Schema;

class Program
{
    static void Main()
    {
        string xmlFilePath = "/Users/viktoriyabilyk/RiderProjects/WebSite/WebSite/WebSite.xml";
        string xsdFilePath = "/Users/viktoriyabilyk/RiderProjects/WebSite/WebSite/WebSchema.xsd";
        string xsltFilePath = "/Users/viktoriyabilyk/RiderProjects/WebSite/WebSite/transform.xslt";
        string outputXmlPath = "/Users/viktoriyabilyk/RiderProjects/WebSite/WebSite/output.xml";
        
        // Перевірка існування вихідного файлу і його видалення
        if (File.Exists(outputXmlPath))
        {
            File.Delete(outputXmlPath);
        }

        // Виведення вмісту вхідного XML
        Console.WriteLine("Вміст вхідного файлу перед трансформацією:");
        Console.WriteLine(File.ReadAllText(xmlFilePath));

        // Створюємо XslCompiledTransform
        XslCompiledTransform xslt = new XslCompiledTransform();
        xslt.Load(xsltFilePath); 

        // Налаштування для XmlWriter
        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true,                  
            IndentChars = "  ",              
            NewLineChars = "\n",              
            NewLineHandling = NewLineHandling.Replace 
        };

        try
        {
            // Трансформуємо XML та зберігаємо результат
            using (XmlWriter writer = XmlWriter.Create(outputXmlPath, settings))
            {
                xslt.Transform(xmlFilePath, writer);
            }

            Console.WriteLine("Transformation complete. Output saved to " + outputXmlPath);

            // Перевірка наявності вихідного файлу
            if (File.Exists(outputXmlPath))
            {
                Console.WriteLine("Output file exists and was successfully created.");
            }
            else
            {
                Console.WriteLine("Output file was not created.");
            }

            // Валідація XML документа
            ValidateXml(xmlFilePath, xsdFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during transformation: {ex.Message}");
        }

        // Виклик WebSiteManager
        WebSiteManager webSiteManager = new WebSiteManager();
        webSiteManager.GetAndSaveToXml(xmlFilePath);
        
        // Читання даних за допомогою XmlReader
        WebSite.XmlReader xmlReader = new WebSite.XmlReader(); // Specify the custom class here
        List<Page> pagesFromXmlReader = xmlReader.ParseXml(xmlFilePath);
        Console.WriteLine("*************************************");
        Console.WriteLine("Читання даних за допомогою XmlReader:");
        Console.WriteLine("*************************************");
        DisplayPages(pagesFromXmlReader);

        // Читання даних за допомогою XmlDocument
        XmlDocumentHandler doc = new XmlDocumentHandler();
        List<Page> pagesFromXmlDocument = doc.ParseXml(xmlFilePath);
        Console.WriteLine("*************************************");
        Console.WriteLine("Читання даних за допомогою XmlDocument:");
        Console.WriteLine("*************************************");
        DisplayPages(pagesFromXmlDocument);
    }

    static void ValidateXml(string xmlFilePath, string xsdFilePath)
    {
        XmlSchemaSet schemas = new XmlSchemaSet();
        schemas.Add("", xsdFilePath); // Додаємо схему

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.Schemas = schemas;
        settings.ValidationType = ValidationType.Schema;
        settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallback);

        // Use fully qualified name for XmlReader
        using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(xmlFilePath, settings))
        {
            try
            {
                while (reader.Read()) ; // Читаємо файл, валідація відбувається під час читання
                Console.WriteLine("XML is valid.");
            }
            catch (XmlException ex)
            {
                Console.WriteLine($"XML Exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }

    static void ValidationCallback(object sender, ValidationEventArgs e)
    {
        if (e.Severity == XmlSeverityType.Warning)
            Console.WriteLine($"Warning: {e.Message}");
        else if (e.Severity == XmlSeverityType.Error)
            Console.WriteLine($"Error: {e.Message}");
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
