using neonrom3r_scraper.Src.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace neonrom3r_scraper.Src.Interfaces
{
    interface IRomScraper
    {
        Dictionary<int, string> GetConsolesLinks();

        string GetBasePath();

        bool HasConsoleRoms(int console);
        //it should return all roms available for a specific console
        List<RomData> GetRomsData(int console, Dictionary<string, string> imageMap = null);
    }
}
