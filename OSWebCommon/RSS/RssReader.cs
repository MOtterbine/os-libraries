using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using OS;

namespace OS.Web.RSS
{
    public class RssReader  
    {

        public static IEnumerable<Rss> GetFeed(string feedUrl)
        {

            XDocument feedXml = null;
            feedXml = XDocument.Load(feedUrl);
            IEnumerable<Rss> feeds = from feed in feedXml.Descendants("item")
                                     //select new Rss
                                     //{
                                     //    Title = feed.Element("title").Value,
                                     //    Link = feed.Element("link").Value,
                                     //    Description = Regex.Match(feed.Element("description").Value, @"^.{1,180}\b(?<!\s)").Value
                                     //};
                                    select new Rss
                                    {
                                        Title = feed.Element("title").Value,
                                        Link = feed.Element("link").Value,
                                        Description = Regex.Match(feed.Element("description").Value, @"^.{1,180}\b(?<!\s)").Value
                                        //Image = new RSSImage()
                                        //{
                                        //    url = feed.Element("image").Element("url").Value,
                                        //    title = feed.Element("title").Element("title").Value,
                                        //    link = feed.Element("link").Element("link").Value
                                        //}

                                    };

            return feeds;

        }
    }
}