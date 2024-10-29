using System.Collections.Generic;
using System.Xml;
using WebSite;

public class XMLReader
{
    public List<Page> ParseXml(string xmlFilePath)
    {
        var pages = new List<Page>();

        using (XmlReader reader = XmlReader.Create(xmlFilePath))
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
                            if (page != null)
                            {
                                reader.Read();
                                page.Title = reader.Value;
                            }
                            break;

                        case "Type":
                            if (page != null)
                            {
                                reader.Read();
                                page.Type = reader.Value;
                            }
                            break;

                        case "Chars":
                            if (page != null && page.Chars == null)
                            {
                                page.Chars = new Characteristics();
                            }
                            while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name == "Chars"))
                            {
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    switch (reader.Name)
                                    {
                                        case "HasEmail":
                                            reader.Read();
                                            if (page?.Chars != null)
                                            {
                                                page.Chars.HasEmail = bool.Parse(reader.Value);
                                            }
                                            break;

                                        case "HasNews":
                                            reader.Read();
                                            if (page?.Chars != null)
                                            {
                                                page.Chars.HasNews = bool.Parse(reader.Value);
                                            }
                                            break;

                                        case "hasArchives":
                                            if (page?.Chars != null && page.Chars.hasArchives == null)
                                            {
                                                page.Chars.hasArchives = new Voting();
                                            }
                                            break;

                                        case "Available":
                                            reader.Read();
                                            if (page?.Chars?.hasArchives != null)
                                            {
                                                page.Chars.hasArchives.Available = bool.Parse(reader.Value);
                                            }
                                            break;

                                        case "Type":
                                            reader.Read();
                                            if (page?.Chars?.hasArchives != null)
                                            {
                                                page.Chars.hasArchives.Type = reader.Value;
                                            }
                                            break;

                                        case "PaidContent":
                                            reader.Read();
                                            if (page?.Chars != null)
                                            {
                                                page.Chars.PaidContent = bool.Parse(reader.Value);
                                            }
                                            break;
                                    }
                                }
                            }
                            break;

                        case "Authorize":
                            if (page != null)
                            {
                                reader.Read();
                                page.Authorize = bool.Parse(reader.Value);
                            }
                            break;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Page")
                {
                    if (page != null)
                    {
                        pages.Add(page);
                        page = null;
                    }
                }
            }
        }
        return pages;
    }
}
