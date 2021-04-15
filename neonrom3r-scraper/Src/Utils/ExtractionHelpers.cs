﻿using neonrom3r_scraper.Src.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace neonrom3r_scraper.Src.Utils
{
    class ExtractionHelpers
    {
        //it regularize the name to have better results after parse the images names with the roms names
        //ex Pokemon red (U) will be pokemonred by this way is more accurate to find the corresponding portrait based on the name
        public static string NormalizeName(string name)
        {

            string normalized = RemoveLanguajes(name)
                   .ToLower()
                   .Replace("-", "")
                   .Replace("(", "")
                   .Replace(")", "")
                   .Replace(" ", "")
                   .Replace("_", "")
                   .Replace("the", "")
                   .Replace(",", "")
                   .Replace("#nes", "")
                   .Replace("#snes", "")
                   .Replace("v1.001", "")
                   .Replace("v1.000", "")
                   .Replace("v1.002", "")
                   .Replace("v1.003", "")
                  .Replace("v1.004", "")
                  .Replace("v1.005", "")
                  .Replace("v1.006", "")
                  .Replace("v1.007", "")
                  .Replace("v1.008", "")
                 ;


            normalized = Regex.Replace(normalized, "[^a-zA-Z0-9]", "");
            return normalized;
        }

        //remove the rom region tag from the name
        public static string RemoveLanguajes(string Name)
        {
            string InnerName = Name;
            string parentesesr = Regex.Replace(Name, @"\(.*?\)", "");
            string curlyr = Regex.Replace(parentesesr, @"\[.*?\]", "");
            return curlyr.Trim();

        }


        //depending of the region tag that have the rom name it will get the languaje of the rom
        public static string ExtractRegion(string romName)
        {
            var name = romName.ToLower().Trim().Replace("eurasia", "");
            if (name.Contains("(e"))
                return "EU";
            else
            if (name.Contains("(u"))
                return "USA";
            else
            if (name.Contains("(j"))
                return "JAP";
            if (name.Contains("(f"))
                return "FR";
            if (name.Contains("(d"))
                return "DE";
            if (name.Contains("(s"))
                return "SPA";
            if (name.Contains("(s"))
                return "SPA";
            else
                return "----";
        }
        //it get the filename without extensions or weird  characters
        public static string ClearName(string Name, int console)
        {
            string InnerName = System.Web.HttpUtility.UrlDecode(Name);

            if (console == Convert.ToInt32(Consoles.Nintendo_DS))
                InnerName = InnerName.Replace(".nds", "");
            if (InnerName.Length > 6)
                if (InnerName[5] == '-')
                {
                    InnerName = InnerName.Substring(6);

                }
            return InnerName;
        }
    }
}