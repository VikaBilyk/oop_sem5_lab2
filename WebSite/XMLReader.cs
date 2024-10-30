using System.Xml;

namespace WebSite;

public class XmlReader
{

    public List<Page> ParseXml(string xmlFilePath)
    {
        var pages = new List<Page>();

        using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(xmlFilePath))
        {
            Page? page = null;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "Page":
                            page = new Page { Chars = new Characteristics() };
                            break;

                        case "Title":
                            reader.Read();
                            page!.Title = reader.Value;
                            break;

                        case "Type":
                            reader.Read();
                            page!.Type = reader.Value;
                            break;

                        case "HasEmail":
                            reader.Read();
                            page!.Chars.HasEmail = bool.Parse(reader.Value);
                            break;

                        case "HasNews":
                            reader.Read();
                            page!.Chars.HasNews = bool.Parse(reader.Value);
                            break;
                        case "HasArchive":
                            reader.Read();
                            page!.Chars.HasArchives = bool.Parse(reader.Value);
                            break;

                        case "HasVoting":
                            reader.Read();
                            page!.Chars.HasVoting = bool.Parse(reader.Value);
                            break;
                        
                        case "Anonymous":
                            reader.Read();
                            page!.Chars.Anonymous = bool.Parse(reader.Value);
                            break;
                        case "Authorization":
                            reader.Read();
                            page!.Chars.Authorization = bool.Parse(reader.Value);
                            break;
                        
                        case "PaidContent":
                            reader.Read();
                            page!.Chars.PaidContent = bool.Parse(reader.Value);
                            break;
                        
                        case "Authorize":
                            reader.Read();
                            page!.Authorize = bool.Parse(reader.Value);
                            break;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Page")
                {
                    pages.Add(page!);
                    page = null;
                }
            }
        }
        return pages;
    }
    
}