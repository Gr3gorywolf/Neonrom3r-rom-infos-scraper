using HtmlAgilityPack;
using neonrom3r_scraper.Src.Interfaces;
using neonrom3r_scraper.Src.Models;
using neonrom3r_scraper.Src.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
                /* { (int)Consoles.Nintendo_DS,"Nintendo%20DS/" },
                 { (int)Consoles.Playstation2,"Sony%20Playstation%202/NTSC-U/" },
                 { (int)Consoles.NintendoGamecube,"Nintendo%20Gamecube/NTSC-U/" },*/
            };
        }

        public List<RomData> GetRomsData(int console, Dictionary<string, string> imageMap = null)
        {
            List<RomData> InnerList = new List<RomData>();
            if (imageMap == null)
                imageMap = new Dictionary<string, string>();


            var document = new HtmlWeb();
            var html = document.LoadFromWebAsync(GetBasePath() + GetConsolesLinks()[Convert.ToInt32(console)]).Result;
            var container = html.DocumentNode.Descendants().Where(ax => ax.Name == "tbody").ToList();
            var children = container[0].ChildNodes.Where((node) => node.Name == "tr").ToList();
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
            return "https://www.squid-proxy.xyz/Games/";
        }
    }
}
