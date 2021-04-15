﻿using neonrom3r_scraper.Src.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace neonrom3r_scraper.Src.Interfaces
{
    interface IRomScraper
    {

        bool HasConsoleRoms(int console);
        //it should return all roms available for a specific console
        List<RomData> GetRomsData(int console, Dictionary<string, string> imageMap = null);

        //it should get all the roms that are available for that console and parse its information to get the info about that rom
        List<RomInfo> GetRomsInfos(int console, Dictionary<string, string> imageMap = null);
    }
}