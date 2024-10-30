using System;
using System.Collections.Generic;
using System.Xml;

namespace WebSite
{
    public class XmlDocumentHandler
    {
        public List<Page> ParseXml(string filepath)
        {
            var pages = new List<Page>();
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);

            XmlNodeList? pageNodes = doc.SelectNodes("//Page");
            foreach (XmlNode node in pageNodes)
            {
                var page = new Page
                {
                    Chars = new Characteristics()
                };

                XmlNode? titleNode = node.SelectSingleNode("Title");
                if (titleNode != null)
                {
                    page.Title = titleNode.InnerText;
                }

                XmlNode? typeNode = node.SelectSingleNode("Type");
                if (typeNode != null)
                {
                    page.Type = typeNode.InnerText;
                }

                XmlNode? authorizeNode = node.SelectSingleNode("Authorize");
                if (authorizeNode != null)
                {
                    page.Authorize = bool.Parse(authorizeNode.InnerText);
                }

                XmlNode? charsNode = node.SelectSingleNode("Chars");
                if (charsNode != null)
                {
                    XmlNode? hasEmailNode = charsNode.SelectSingleNode("HasEmail");
                    if (hasEmailNode != null)
                    {
                        page.Chars.HasEmail = bool.Parse(hasEmailNode.InnerText);
                    }

                    XmlNode? hasNewsNode = charsNode.SelectSingleNode("HasNews");
                    if (hasNewsNode != null)
                    {
                        page.Chars.HasNews = bool.Parse(hasNewsNode.InnerText);
                    }

                    XmlNode? hasArchivesNode = charsNode.SelectSingleNode("HasArchives");
                    if (hasArchivesNode != null)
                    {
                        page.Chars.HasArchives = bool.Parse(hasArchivesNode.InnerText);
                    }

                    XmlNode? hasVotingNode = charsNode.SelectSingleNode("HasVoting");
                    if (hasVotingNode != null)
                    {
                        page.Chars.HasVoting = bool.Parse(hasVotingNode.InnerText);
                    }

                    XmlNode? anonymousNode = charsNode.SelectSingleNode("Anonymous");
                    if (anonymousNode != null)
                    {
                        page.Chars.Anonymous = bool.Parse(anonymousNode.InnerText);
                    }

                    XmlNode? authorizationNode = charsNode.SelectSingleNode("Authorization");
                    if (authorizationNode != null)
                    {
                        page.Chars.Authorization = bool.Parse(authorizationNode.InnerText);
                    }

                    XmlNode? paidContentNode = charsNode.SelectSingleNode("PaidContent");
                    if (paidContentNode != null)
                    {
                        page.Chars.PaidContent = bool.Parse(paidContentNode.InnerText);
                    }
                }

                pages.Add(page);
            }

            return pages;
        }
    }
}
