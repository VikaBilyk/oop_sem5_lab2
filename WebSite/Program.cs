using System;
using System.Xml;
using System.Xml.Linq;
using WebSite;
using System.Xml.Xsl;
using System.IO;
using System.Xml.Schema;
using Serilog;
using Serilog.Sinks.File;

class Program
{
    static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()  // Log to console
            .WriteTo.File("logs\\log-.txt", rollingInterval: RollingInterval.Day) 
            .CreateLogger();

        string xmlFilePath = "/Users/viktoriyabilyk/RiderProjects/WebSite/WebSite/WebSite.xml";
        string xsdFilePath = "/Users/viktoriyabilyk/RiderProjects/WebSite/WebSite/WebSchema.xsd";
        string xsltFilePath = "/Users/viktoriyabilyk/RiderProjects/WebSite/WebSite/transform.xslt";
        string outputXmlPath = "/Users/viktoriyabilyk/RiderProjects/WebSite/WebSite/output.xml";
        
        if (File.Exists(outputXmlPath))
        {
            File.Delete(outputXmlPath);
            Log.Information("Existing output file deleted: {OutputXmlPath}", outputXmlPath);
        }
        
        Log.Information("Вміст вхідного файлу перед трансформацією: {Content}", File.ReadAllText(xmlFilePath));

        
        XslCompiledTransform xslt = new XslCompiledTransform();
        xslt.Load(xsltFilePath); 

        
        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true,                  
            IndentChars = "  ",              
            NewLineChars = "\n",              
            NewLineHandling = NewLineHandling.Replace 
        };

        try
        {
            
            using (XmlWriter writer = XmlWriter.Create(outputXmlPath, settings))
            {
                xslt.Transform(xmlFilePath, writer);
            }

            Log.Information("Transformation complete. Output saved to {OutputXmlPath}", outputXmlPath);

            
            if (File.Exists(outputXmlPath))
            {
                Log.Information("Output file exists and was successfully created.");
            }
            else
            {
                Log.Warning("Output file was not created.");
            }

            
            ValidateXml(xmlFilePath, xsdFilePath);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during transformation");
        }
        
        WebSiteManager webSiteManager = new WebSiteManager();
        webSiteManager.GetAndSaveToXml(xmlFilePath);
        
        // Читання даних за допомогою XmlReader
        WebSite.XmlReader xmlReader = new WebSite.XmlReader();
        List<Page> pagesFromXmlReader = xmlReader.ParseXml(xmlFilePath);
        Log.Information("*************************************");
        Log.Information("Читання даних за допомогою XmlReader:");
        Log.Information("*************************************");
        DisplayPages(pagesFromXmlReader);

        // Читання даних за допомогою XmlDocument
        XmlDocumentHandler doc = new XmlDocumentHandler();
        List<Page> pagesFromXmlDocument = doc.ParseXml(xmlFilePath);
        Log.Information("*************************************");
        Log.Information("Читання даних за допомогою XmlDocument:");
        Log.Information("*************************************");
        DisplayPages(pagesFromXmlDocument);
        
        Log.CloseAndFlush();
    }

    static void ValidateXml(string xmlFilePath, string xsdFilePath)
    {
        XmlSchemaSet schemas = new XmlSchemaSet();
        schemas.Add("", xsdFilePath); 

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.Schemas = schemas;
        settings.ValidationType = ValidationType.Schema;
        settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallback);
        
        using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(xmlFilePath, settings))
        {
            try
            {
                while (reader.Read()) ;
                Log.Information("XML is valid.");
            }
            catch (XmlException ex)
            {
                Log.Error(ex, "XML Exception");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred");
            }
        }
    }

    static void ValidationCallback(object sender, ValidationEventArgs e)
    {
        if (e.Severity == XmlSeverityType.Warning)
            Log.Warning("Warning: {Message}", e.Message);
        else if (e.Severity == XmlSeverityType.Error)
            Log.Error("Error: {Message}", e.Message);
    }

    static void DisplayPages(List<Page> pages)
    {
        foreach (var page in pages)
        {
            Log.Information("Title: {Title}, Type: {Type}, Authorize: {Authorize}", page.Title, page.Type, page.Authorize);
            Log.Information("HasEmail: {HasEmail}, HasNews: {HasNews}, PaidContent: {PaidContent}", 
                page.Chars.HasEmail, page.Chars.HasNews, page.Chars.PaidContent);
                
            if (page.Chars.HasVoting != null && page.Chars.HasVoting == true)
            {
                Log.Information("Anonymous: {Anonymous}, Authorization: {Authorization}", 
                    page.Chars.Anonymous, page.Chars.Authorization);
            }
            Log.Information("____________________________________");
        }
    }
}
