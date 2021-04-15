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
            return ConsolesConstants.SquidProxyConsoles.ContainsKey(console);
        }
        public List<RomData> GetRomsData(int console, Dictionary<string, string> imageMap = null)
        {
            List<RomData> InnerList = new List<RomData>();
            if (imageMap == null)
                imageMap = new Dictionary<string, string>();


            var document = new HtmlWeb();
            var html = document.LoadFromWebAsync(Constants.SquidProxyBaseurl + ConsolesConstants.SquidProxyConsoles[Convert.ToInt32(console)]).Result;
            var container = html.DocumentNode.Descendants().Where(ax => ax.Name == "tbody").ToList();
            var children = container[0].ChildNodes.Where((node)=>node.Name == "tr").ToList();
            int foundCount = 0;

            foreach (var child in children)
            {
                var romCols = child.ChildNodes.Where(ax => ax.Name == "td").ToList();
                if (romCols.Count > 0)
                {
                    var nameRow = romCols[0];
                    var sizeRow = romCols[1];
                    if (!nameRow.InnerText.ToLower().Contains("parent directory"))
                    {
                        var linkElement = nameRow.ChildNodes.Where(ax => ax.Name == "a").First();
                        var link = linkElement.Attributes["href"].Value;
                        var name = ExtractionHelpers.ExtractName(link, console);
                        var thumbnail = ExtractionHelpers.ExtractThumbnail(console, name, imageMap);
                        string region = ExtractionHelpers.ExtractRegion(name);

                        string infolink = Constants.InfoRepoBaseUrl + ConsolesConstants.ConsoleSlugs[console] + "/" + name.Trim() + ".json";
                        InnerList.Add(new RomData
                        {
                            InfoLink = infolink,
                            Name = name,
                            Portrait = thumbnail,
                            Region = region
                        });
                        if (thumbnail.Length > 0)
                        {
                            foundCount++;
                        }
                    }
                }
            }
            Console.WriteLine(foundCount + " Portadas encontradas de " + children.Count + " Roms   -> " + (children.Count - foundCount) + "Portadas no encotradas");
            return InnerList;
        }

        public List<RomInfo> GetRomsInfos(int console, Dictionary<string, string> imageMap = null)
        {
            List<RomInfo> InnerList = new List<RomInfo>();
            if (imageMap == null)
                imageMap = new Dictionary<string, string>();


            var document = new HtmlWeb();
            var html = document.LoadFromWebAsync(Constants.SquidProxyBaseurl + ConsolesConstants.SquidProxyConsoles[Convert.ToInt32(console)]).Result;
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
                    if (!nameRow.InnerText.ToLower().Contains("parent directory"))
                    {
                        var linkElement = nameRow.ChildNodes.Where(ax => ax.Name == "a").First();
                        var link = linkElement.Attributes["href"].Value;
                        var name = ExtractionHelpers.ExtractName(link, console);
                        var thumbnail = ExtractionHelpers.ExtractThumbnail(console, name, imageMap);
                        string region = ExtractionHelpers.ExtractRegion(name);

                        string infolink = Constants.InfoRepoBaseUrl + ConsolesConstants.ConsoleSlugs[console] + "/" + name.Trim() + ".json";
                        InnerList.Add(new RomInfo
                        {
                            Size = ExtractionHelpers.ExtractSize(Convert.ToInt64(sizeRow.InnerText)),
                            Name = name,
                            Portrait = thumbnail,
                            Region = region,
                            Console = ((Enums.Consoles)console).ToString().Replace('_', ' ')
                        });
                        if (thumbnail.Length > 0)
                        {
                            foundCount++;
                        }
                    }
                }
            }
            Console.WriteLine(foundCount + " Portadas encontradas de " + children.Count + " Roms   -> " + (children.Count - foundCount) + "Portadas no encotradas");
            return InnerList;
        }

       
    }
}
