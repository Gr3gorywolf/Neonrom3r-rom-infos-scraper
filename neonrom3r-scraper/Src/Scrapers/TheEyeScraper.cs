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
    class TheEyeScraper : IRomScraper
    {

        public bool HasConsoleRoms(int console)
        {
            return ConsolesConstants.TheeyeConsoles.ContainsKey(console);
        }

        public List<RomData> GetRomsData(int console, Dictionary<string, string> imageMap = null)
        {
            List<string> unmatch = new List<string>();
            List<RomData> InnerList = new List<RomData>();
            if (imageMap == null)
                imageMap = new Dictionary<string, string>();


            var document = new HtmlWeb();
            var html = document.LoadFromWebAsync(Constants.TheeyeBaseurl + ConsolesConstants.TheeyeConsoles[Convert.ToInt32(console)]).Result;
            var container = html.DocumentNode.Descendants().Where(ax => ax.Name == "pre").ToList();
            var children = container[0].ChildNodes;
            var anchorTexts = children.Where(ax => ax.Name == "a").ToList();
            var infoTexts = children.Where(ax => ax.Name == "#text").ToList();
            int foundCount = 0;
            for (int i = 0; i < anchorTexts.Count; i++)
            {


                string name = ExtractionHelpers.ClearName(Path.GetFileNameWithoutExtension(anchorTexts[i].Attributes["href"].Value), console);
                string purename = ExtractionHelpers.RemoveLanguajes(ExtractionHelpers.NormalizeName(name));
                string portrait = Constants.ThumbnailsBaseurl + ConsolesConstants.ThumbnailsConsoles[Convert.ToInt32(console)] + Constants.ThumbnailFolder;
                string region = ExtractionHelpers.ExtractRegion(name);
                //evaluate that the matching characters are at least the 50% 
                var results = imageMap.Where(image =>
                {
                    return image.Key.Contains(purename);

                }).ToList();


                if (results.Count == 0)
                {
                    portrait += name + ".png";


                }
                else
                {
                    foundCount++;
                    portrait += results[0].Value;
                }
                string infolink = Constants.InfoRepoBaseUrl + ConsolesConstants.ConsoleSlugs[console] + "/" + name.Trim() + ".json";
                if (!infolink.EndsWith("/") && name.Trim() != "" && !infolink.StartsWith(".") && !infolink.EndsWith(".txt") && results.Count > 0)
                {
                    InnerList.Add(new RomData
                    {
                        InfoLink = infolink,
                        Name = name,
                        Portrait = portrait,
                        Region = region
                    });
                }

            }
            Console.WriteLine(foundCount + " Portadas encontradas de " + anchorTexts.Count + "   " + (anchorTexts.Count - foundCount) + "Portadas no encotradas");
            return InnerList;
        }

        public List<RomInfo> GetRomsInfos(int consoleCode, Dictionary<string, string> imageMap = null)
        {

            List<RomInfo> InnerList = new List<RomInfo>();
            var document = new HtmlWeb();
            var html = document.LoadFromWebAsync(Constants.TheeyeBaseurl + ConsolesConstants.TheeyeConsoles[Convert.ToInt32(consoleCode)]).Result;
            var container = html.DocumentNode.Descendants().Where(ax => ax.Name == "pre").ToList();
            var children = container[0].ChildNodes;
            var anchorTexts = children.Where(ax => ax.Name == "a").ToList();
            var infoTexts = children.Where(ax => ax.Name == "#text").ToList();
            anchorTexts.RemoveAt(0);
            infoTexts.RemoveAt(0);
            int foundcount = 0;
            for (int i = 0; i < anchorTexts.Count; i++)
            {
                string name = ExtractionHelpers.ClearName(Path.GetFileNameWithoutExtension(anchorTexts[i].Attributes["href"].Value), consoleCode);
                string infolink = anchorTexts[i].Attributes["href"].Value;
                string purename = ExtractionHelpers.RemoveLanguajes(ExtractionHelpers.NormalizeName(name));
                string portrait = Constants.ThumbnailsBaseurl + ConsolesConstants.ThumbnailsConsoles[Convert.ToInt32(consoleCode)] +  Constants.ThumbnailFolder;
                var results = imageMap.Where(image =>
                {
                    return image.Key.Contains(purename);
                }).ToList();
                if (results.Count == 0)
                    portrait += name + ".png";
                else
                {
                    foundcount++;
                    portrait += results[0].Value;

                }
                string filesize = infoTexts[i].InnerText.ToString().Split("\r\n")[0].Split(':')[1].Substring(2).Trim();
                string region = ExtractionHelpers.ExtractRegion(name);
                string dowloadLink = Constants.TheeyeBaseurl + ConsolesConstants.TheeyeConsoles[Convert.ToInt32(consoleCode)] + infolink;
                if (!infolink.EndsWith("/") && name.Trim() != "" && !infolink.StartsWith(".") && !infolink.EndsWith(".txt") && results.Count > 0)
                {
                    InnerList.Add(new RomInfo
                    {
                        DownloadLink = dowloadLink ,
                        Name = name,
                        Portrait = portrait,
                        Size = filesize,
                        Region = region,
                        Console = ((Enums.Consoles)consoleCode).ToString().Replace('_', ' ')

                    });
                }
            }
            Console.WriteLine(foundcount + " Portadas encontradas de " + anchorTexts.Count + "   " + (anchorTexts.Count - foundcount) + "Portadas no encotradas");
            return InnerList;

        } 
    }
}
