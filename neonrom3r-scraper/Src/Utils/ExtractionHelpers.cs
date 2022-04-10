using neonrom3r_scraper.Src.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace neonrom3r_scraper.Src.Utils
{
    class ExtractionHelpers
    {

        public static bool IsNameValid(string romName)
        {
            if (romName.Contains("../"))
                return false;
            if (romName.ToLower().Contains("parent directory"))
                return false;
            if (romName.Trim().Length == 0)
                return false;
            return true;
        }
        /// <summary>
        ///it regularize the name to have better results after parse the images names with the roms names \n
        ///ex Pokemon red (U) will be pokemonred by this way is more accurate to find the corresponding portrait based on the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        public static string NormalizeName(string name)
        {

            string normalized = RemoveLanguajes(name)
                   .ToLower()
                   .Replace("-", "")
                   .Replace("(", "")
                   .Replace(")", "")
                   .Replace(" ", "")
                   .Replace("_", "")
                   .Replace("_-_","")
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

        // extract the size of the rom  from file name
        public static string ExtractSize(string romName)
        {
            return romName.Split("\r\n")[0].Split(':')[1].Substring(2).Trim();
        }


        //extract the name of the rom  from file name
        public static string ExtractName(string romName, int console)
        {
            return ClearName(Path.GetFileNameWithoutExtension(romName), console);
        }

        //Extracts the thumbnail from the rom name and a map of <normalizedName,imageUrl>
        //if no portrait is found it will return null
        public static string ExtractThumbnail(int console, string romName, Dictionary<string, string> imageMap)
        {
            string portrait = Constants.ThumbnailsBaseurl + ConsolesConstants.ThumbnailsConsoles[Convert.ToInt32(console)] + Constants.ThumbnailFolder;
            string normalizedName = ExtractionHelpers.RemoveLanguajes(ExtractionHelpers.NormalizeName(romName));
            var results = imageMap.Where(image =>
            {
                return image.Key.Contains(normalizedName);

            }).ToList();
            if (results.Count == 0)
            {
                portrait = null;
            }
            else
            {
                portrait += results[0].Value;
            }

            return portrait;
        }

        //converts file size in bytes to display size
       public static string ExtractSize(long bytes)
        {
            var size = (bytes / 1024f) / 1024f;
            if (size < 1024)
            {
                return size + " M";
            }
            else
            {
                return size/1024  + " G";
            }
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