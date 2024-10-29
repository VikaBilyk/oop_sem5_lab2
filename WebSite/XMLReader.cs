namespace WebSite;
using System;
using System.Collections.Generic;
using System.Xml;

public class XMLReader
{
    public List<Page> ParseXml(string xmlFilePath)
    {
        var pages = new List<Page>();

        using (XmlReader reader = XmlReader.Create(xmlFilePath))
        {
            Page page = null;

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

                        case "Chars":
                            // Читаємо дочірні елементи Chars
                            while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name == "Chars"))
                            {
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    switch (reader.Name)
                                    {
                                        case "HasEmail":
                                            reader.Read();
                                            page!.Chars.HasEmail = bool.Parse(reader.Value);
                                            break;
                                        case "HasNews":
                                            reader.Read();
                                            page!.Chars.HasNews = bool.Parse(reader.Value);
                                            break;
                                        case "hasArchives":
                                            page!.Chars.hasArchives = new Voting();
                                            break;
                                        case "Available":
                                            reader.Read();
                                            page!.Chars.hasArchives!.Available = bool.Parse(reader.Value);
                                            break;
                                        case "Type":
                                            reader.Read();
                                            page!.Chars.hasArchives!.Type = reader.Value;
                                            break;
                                        case "PaidContent":
                                            reader.Read();
                                            page!.Chars.PaidContent = bool.Parse(reader.Value);
                                            break;
                                    }
                                }
                            }
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
