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

namespace neonrom3r_scraper.Src.Scrapers
{
    class TheEyeScraper : IRomScraper
    {

        public Dictionary<int, string> GetConsolesLinks()
        {
            return new Dictionary<int, string>()
        {
                { (int)Consoles.GameBoy,"Nintendo%20Gameboy/" },
                { (int)Consoles.GameBoyAdvance,"Nintendo%20Gameboy%20Advance/" },
                { (int)Consoles.GameBoyColor,"Nintendo%20Gameboy%20Color/" },
                { (int)Consoles.Nintendo,"NES/" },
                { (int)Consoles.SuperNintendo,"SNES/" },
                { (int)Consoles.Nintendo64,"Nintendo%2064/Roms/" },
                { (int)Consoles.Playstation,"Playstation/Games/NTSC/" },
                { (int)Consoles.Psp,"Playstation%20Portable/ISO/" },
                { (int)Consoles.Nintendo_DS,"Nintendo%20DS/" },
                //{ (int)Consoles.Sega_Genesis,"Sega%20Genesis/" },
                //{ (int)Consoles.Sega_Dreamcast,"Sega%20Dreamcast/" },
        };
        }

        public bool HasConsoleRoms(int console)
        {
            return GetConsolesLinks().ContainsKey(console);
        }


        public List<RomData> GetRomsData(int consoleCode, Dictionary<string, string> imageMap = null)
        {
            List<string> unmatch = new List<string>();
            List<RomData> InnerList = new List<RomData>();
            if (imageMap == null)
                imageMap = new Dictionary<string, string>();
            var document = new HtmlWeb();
            var url = GetBasePath() + GetConsolesLinks()[Convert.ToInt32(consoleCode)];
            var html = document.LoadFromWebAsync(url).Result;
            var container = html.DocumentNode.Descendants().Where(ax => ax.Name == "pre").ToList();
            var children = container[0].ChildNodes;
            var anchorTexts = children.Where(ax => ax.Name == "a").ToList();
            var infoTexts = children.Where(ax => ax.Name == "#text").ToList();
            int foundCount = 0;
            for (int i = 0; i < anchorTexts.Count; i++)
            {


                string name = ExtractionHelpers.ClearName(Path.GetFileNameWithoutExtension(anchorTexts[i].Attributes["href"].Value), consoleCode);
                if (!ExtractionHelpers.IsNameValid(name))
                {
                    continue;
                }
                string portrait = ExtractionHelpers.ExtractThumbnail(consoleCode, name, imageMap);
                string region = ExtractionHelpers.ExtractRegion(name);
                string infolink = Constants.InfoRepoBaseUrl + ConsolesConstants.ConsoleSlugs[consoleCode] + "/" + name.Trim() + ".json";
                string romSize = ExtractionHelpers.ExtractSize(infoTexts[i].InnerText);
                string romDownloadUrl = anchorTexts[i].Attributes["href"].Value;
                if (!infolink.EndsWith("/") && name.Trim() != "" && !infolink.StartsWith(".") && !infolink.EndsWith(".txt") && portrait != null)
                {
                    foundCount++;
                    InnerList.Add(new RomData
                    {
                        Name = name,
                        Portrait = portrait,
                        Region = region,
                        DownloadLink = url + romDownloadUrl,
                        Console = ((Enums.Consoles)consoleCode).ToString().Replace('_', ' '),
                        Size = romSize
                    });
                }

            }
            Console.WriteLine(foundCount + " Portraits found of " + anchorTexts.Count + "   " + (anchorTexts.Count - foundCount) + "Portraits not found");
            return InnerList;
        }

        public string GetBasePath()
        {
            return "https://the-eye.eu/public/rom/";
        }
    }
}
