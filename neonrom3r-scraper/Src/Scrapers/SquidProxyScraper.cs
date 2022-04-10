using HtmlAgilityPack;
using neonrom3r_scraper.Src.Enums;
using neonrom3r_scraper.Src.Interfaces;
using neonrom3r_scraper.Src.Models;
using neonrom3r_scraper.Src.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace neonrom3r_scraper.Src.Scrapers
{
    class SquidProxyScraper : IRomScraper
    {
        public bool HasConsoleRoms(int console)
        {
            return GetConsolesLinks().ContainsKey(console);
        }
        public Dictionary<int, string> GetConsolesLinks()
        {
            return new Dictionary<int, string>()
            {
                { (int)Consoles.GameBoy,"Nintendo%20Gameboy/" },
                { (int)Consoles.GameBoyAdvance,"Nintendo%20Gameboy%20Advance/" },
                { (int)Consoles.GameBoyColor,"Nintendo%20Gameboy%20Color/" },
                { (int)Consoles.Nintendo,"NoIntro%20Collection/Nintendo%20-%20Nintendo%20Entertainment%20System%20[headered]/" },
                { (int)Consoles.SuperNintendo,"NoIntro%20Collection/Nintendo%20-%20Super%20Nintendo%20Entertainment%20System/" },
                { (int)Consoles.Nintendo64,"Nintendo%2064/Big%20Endian/" },
                { (int)Consoles.Playstation,"Playstation%201/" },
                { (int)Consoles.Nintendo_DS,"Nintendo%20DS/" },
                { (int)Consoles.Playstation2,"Playstation%202/" },
               // { (int)Consoles.NintendoGamecube,"Nintendo%20Gamecube/US/" },
                { (int)Consoles.Psp,"Playstation%20Portable/ISO/" },
            };
        }

        public List<RomData> GetRomsData(int console, Dictionary<string, string> imageMap = null)
        {
            List<RomData> InnerList = new List<RomData>();
            if (imageMap == null)
                imageMap = new Dictionary<string, string>();


            var document = new HtmlWeb();
            var baseUrl = GetBasePath() + GetConsolesLinks()[Convert.ToInt32(console)];
            var html = document.LoadFromWebAsync(baseUrl).Result;
            List<HtmlNode> container = new List<HtmlNode>();
            List<HtmlNode> children = new List<HtmlNode>();
            if (html.DocumentNode.InnerHtml.Contains("tbody"))
            {
                container = html.DocumentNode.Descendants().Where(ax => ax.Name == "tbody").ToList();
                children = container.First()?.ChildNodes.Where((node) => node.Name == "tr").ToList();
            }else
            {
                container = html.DocumentNode.Descendants().Where(ax => ax.Name == "body").ToList();
                children = container[0].ChildNodes.Where((node) => node.Name == "a").ToList();
            }

            int foundCount = 0;

            foreach (var child in children)
            {
                var romCols = child.ChildNodes.Where(ax => ax.Name == "td").ToList();
                if (romCols.Count > 0)
                {
                    var nameRow = romCols[0];
                    var sizeRow = romCols[1];
                    if (!ExtractionHelpers.IsNameValid(nameRow.InnerText))
                    {
                        continue;
                    }
                    var linkElement = nameRow.ChildNodes.Where(ax => ax.Name == "a").First();
                    var link = linkElement.Attributes["href"].Value;
                    var name = ExtractionHelpers.ExtractName(link, console);
                    var thumbnail = ExtractionHelpers.ExtractThumbnail(console, name, imageMap);
                    string region = ExtractionHelpers.ExtractRegion(name);
                    if (thumbnail != null)
                    {
                        foundCount++;
                        InnerList.Add(new RomData
                        {
                            Console = ((Enums.Consoles)console).ToString().Replace('_', ' '),

                            Name = name,
                            DownloadLink = baseUrl + link,
                            Portrait = thumbnail,
                            Region = region
                        });
                    }
                }
                else
                {
                    var link = child.Attributes["href"].Value;
                    var name = ExtractionHelpers.ExtractName(link, console);
                    var thumbnail = ExtractionHelpers.ExtractThumbnail(console, name, imageMap);
                    string region = ExtractionHelpers.ExtractRegion(name);
                    if (thumbnail != null)
                    {
                        foundCount++;
                        InnerList.Add(new RomData
                        {
                            Console = ((Enums.Consoles)console).ToString().Replace('_', ' '),
                            DownloadLink =baseUrl + link,
                            Name = name,
                            Portrait = thumbnail,
                            Region = region
                        });
                    }
                }
            }
            Console.WriteLine(foundCount + " Portraits found of " + children.Count + " Roms   -> " + (children.Count - foundCount) + "Portraits not found");
            return InnerList;
        }

        public string GetBasePath()
        {
            return "https://www.squid-proxy.xyz/";
        }
    }
}
