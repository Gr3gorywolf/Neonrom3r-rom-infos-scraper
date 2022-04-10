using HtmlAgilityPack;
using neonrom3r_scraper.Src.Enums;
using neonrom3r_scraper.Src.Interfaces;
using neonrom3r_scraper.Src.Models;
using neonrom3r_scraper.Src.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace neonrom3r_scraper.Src.Scrapers
{
    class EdgeEmulationScraper : IRomScraper
    {
        public string GetBasePath()
        {
            return "https://edgeemu.net/";
        }

        public Dictionary<int, string> GetConsolesLinks()
        {
            return new Dictionary<int, string>()
            {
                { (int)Consoles.Sega_Genesis,"browse-md-{prefix}.htm" },
                { (int)Consoles.Sega_Dreamcast,"browse-dc-{prefix}.htm" },
                 { (int)Consoles.NintendoGamecube,"browse-gc-{prefix}.htm" },
            };
        }

        public List<RomData> GetRomsData(int console, Dictionary<string, string> imageMap = null)
        {
            List<RomData> InnerList = new List<RomData>();
            var prefixes = new List<String> {
                "num", "A", "B", "C", "D", "E", "F",
                "G", "H", "I", "J", "K", "L",
                "M", "N", "0", "P", "Q", "R",
                "S", "T", "U", "V", "W", "X",
                "Y", "Z"
            };
            int foundCount = 0;
            int totalRoms = 0;
            var baseUrl = GetBasePath() + GetConsolesLinks()[Convert.ToInt32(console)];
            var document = new HtmlWeb();
            foreach (string prefix in prefixes)
            {
                var currentUrl = baseUrl.Replace("{prefix}", prefix);
                var html = document.LoadFromWebAsync(currentUrl).Result;
                HtmlNode tableElement;
                try
                {
                    tableElement = html.DocumentNode.Descendants().Where((node) => node.Name == "table").First();
                }
                catch
                {
                    continue;
                }
                var tableRows = tableElement?.ChildNodes.Where((node) => node.Name == "tr").ToList();
                totalRoms += tableRows.Count;
                foreach (var row in tableRows)
                {
                    var columns = row.ChildNodes.Where(ax => ax.Name == "td").ToList();
                    if (columns.Count > 0)
                    {
                        var nameColumn = columns[0];
                        var sizeColum = columns[1];
                        var link = nameColumn.ChildNodes[0].Attributes["href"].Value;
                        var name = ExtractionHelpers.ExtractName(nameColumn.InnerText, console);
                        var thumbnail = ExtractionHelpers.ExtractThumbnail(console, name, imageMap);
                        string region = ExtractionHelpers.ExtractRegion(name);
                        if (thumbnail != null)
                        {
                            foundCount++;
                            InnerList.Add(new RomData
                            {
                                Console = ((Enums.Consoles)console).ToString().Replace('_', ' '),
                                DownloadLink = GetBasePath() + link,
                                Name = name,
                                Portrait = thumbnail,
                                Region = region
                            });
                        }

                    }
                }
            }
            Console.WriteLine(foundCount + " Portraits found of " + totalRoms + " Roms   -> " + (totalRoms - foundCount) + "Portraits not found");
            return InnerList;
        }

        public bool HasConsoleRoms(int console)
        {
            return GetConsolesLinks().ContainsKey(console);
        }
    }
}
