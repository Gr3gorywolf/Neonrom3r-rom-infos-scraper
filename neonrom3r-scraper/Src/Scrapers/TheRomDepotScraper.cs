using HtmlAgilityPack;
using neonrom3r_scraper.Src.Enums;
using neonrom3r_scraper.Src.Interfaces;
using neonrom3r_scraper.Src.Models;
using neonrom3r_scraper.Src.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace neonrom3r_scraper.Src.Scrapers
{
    class TheRomDepotScraper : IRomScraper
    {

        public bool HasConsoleRoms(int console)
        {
            return GetConsolesLinks().ContainsKey(console);
        }
        public Dictionary<int, string> GetConsolesLinks()
        {
            return new Dictionary<int, string>()
            {
                 { (int)Consoles.Nintendo_DS,"Nintendo%20DS/Europe/" },
                  { (int)Consoles.NintendoGamecube,"Nintendo%20Gamecube/US/" },
                    { (int)Consoles.Psp,"Sony%20Playstation%20Portable/US/" },
                    { (int)Consoles.Playstation2,"Sony%20Playstation%202/US/" },

            };
        }

        public List<RomData> GetRomsData(int console, Dictionary<string, string> imageMap = null)
        {
            List<RomData> InnerList = new List<RomData>();
            if (imageMap == null)
                imageMap = new Dictionary<string, string>();


            var document = new HtmlWeb();
            var url = GetBasePath() + GetConsolesLinks()[Convert.ToInt32(console)];
            var html = document.LoadFromWebAsync(url).Result;
            var container = html.DocumentNode.Descendants().Where(ax => ax.Name == "pre").ToList();
            var children = container[0].ChildNodes.Where((node) => node.Name == "a").ToList();
            int foundCount = 0;

            foreach (var child in children)
            {

                var link = child.Attributes["href"].Value;
                var clearName = ExtractionHelpers.ClearName(link, console);
                if (!ExtractionHelpers.IsNameValid(clearName))
                {
                    continue;
                }

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
                        Region = region,
                        DownloadLink = url + link,
                        Size = "--"
                    });
                }
            }
            Console.WriteLine(foundCount + " Portraits found of " + children.Count + " Roms   -> " + (children.Count - foundCount) + "Portraits not found");
            return InnerList;
        }

        public string GetBasePath()
        {
            return "https://theromdepot.com/roms/";
        }
    }
}
