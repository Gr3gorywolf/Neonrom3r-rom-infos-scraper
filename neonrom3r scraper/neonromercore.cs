using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Linq;
namespace neonrom3r_scraper
{
    public class neonromerscraper
    {

        






        //it regularize the name to have better results after parse the images names with the roms names
        //ex Pokemon red (U) will be pokemonred by this way is more accurate to find the corresponding portrait based on the name
        public string normalizename(string name) {

            string normalized = RemoveLanguajes(name)
                   .ToLower()
                   .Replace("-", "")
                   .Replace(" ", "")
                   .Replace("_", "")
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


           normalized= Regex.Replace(normalized, "[^a-zA-Z0-9]", "");
            return normalized;
        }

        //remove the rom region tag from the name
        public string RemoveLanguajes(string Name) {
            string InnerName = Name;
          string parentesesr= Regex.Replace(Name, @"\(.*?\)", "");
            string curlyr= Regex.Replace(parentesesr, @"\[.*?\]", "");
            return curlyr.Trim();

        }


        //depending of the region tag that have the rom name it will get the languaje of the rom
        public string getregion(string romname) {
            var name = romname.ToLower().Trim().Replace("eurasia","");
            if (name.Contains("(e") )
                return "EU";
            else
            if (name.Contains("(u" ))
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
        public string clearname(string Name, int con) {
            string InnerName = HttpUtility.UrlDecode(Name) ;
         
            if (con ==Convert.ToInt32( Enums.Consola.Nintendo_DS))
                 InnerName= InnerName.Replace(".nds", "");
           if(InnerName.Length>6)
            if (InnerName[5]=='-') {
                    InnerName = InnerName.Substring(6);
              
            }

            return InnerName ;
        }

        //it gets all the roms that are available for that console
        public List<models.romsdata> GetRomsData(int console, Dictionary<string, string> imgmap=null) {
            List<string> unmatch = new List<string>();
            List<models.romsdata> InnerList = new List<models.romsdata>();
            if (imgmap == null)
                imgmap = new Dictionary<string, string>();


           var Document = new HtmlWeb();
            var Html = Document.LoadFromWebAsync(linkhelpers.TheeyeBaseurl + linkhelpers.TheeyeConsoles[Convert.ToInt32(console)]).Result;
            var Container = Html.DocumentNode.Descendants().Where(ax=>ax.Name=="pre").ToList();
            var Elements = Container[0].ChildNodes;
            var Anchortexts = Elements.Where(ax => ax.Name == "a").ToList();
            var Infotexts = Elements.Where(ax => ax.Name == "#text").ToList();
            int foundcount = 0;
            for (int i = 0; i < Anchortexts.Count; i++) {

              
                string name = clearname(Path.GetFileNameWithoutExtension(Anchortexts[i].Attributes["href"].Value), console);
                string purename =RemoveLanguajes( normalizename(name));
                string portrait = linkhelpers.ThumbnailsBaseurl + linkhelpers.ThumbnailsConsoles[Convert.ToInt32(console)] + linkhelpers.ThumbnailFolder;
                string region = getregion(name);
                if (!imgmap.ContainsKey(purename))
                { 
                    portrait +=  name + ".png";
                  
                }
                else
                {
                    foundcount++;
                    portrait += imgmap[purename];
                }
                string infolink = linkhelpers.InfoRepoBaseUrl + linkhelpers.RepoConsoles[console] + "/" + name.Trim() + ".json";
                if (!infolink.EndsWith("/") && name.Trim()!="" && !infolink.StartsWith(".") && !infolink.EndsWith(".txt") && imgmap.ContainsKey(purename))
                {
                InnerList.Add(new models.romsdata {
                    InfoLink = infolink,
                    Name = name,
                    Portrait =portrait ,
                    Region=region
                });
                }

            }
            Console.WriteLine(foundcount + " Portadas encontradas de " + Anchortexts.Count+"   "+(Anchortexts.Count-foundcount)+"Portadas no encotradas");
            Console.WriteLine("50%");



            return InnerList;

        }

        //it gets all the roms that are available for that console and parse its information to get the info about that rom
        public List<models.rominfo> GetRomsInfos(int console, Dictionary<string, string> imgmap = null)
        {
            List<models.rominfo> InnerList = new List<models.rominfo>();
            var Document = new HtmlWeb();
            var Html = Document.LoadFromWebAsync(linkhelpers.TheeyeBaseurl + linkhelpers.TheeyeConsoles[Convert.ToInt32(console)]).Result;
            var Container = Html.DocumentNode.Descendants().Where(ax => ax.Name == "pre").ToList();
            var Elements = Container[0].ChildNodes;
            var Anchortexts = Elements.Where(ax => ax.Name == "a").ToList();
            var Infotexts = Elements.Where(ax => ax.Name == "#text").ToList();
            Anchortexts.RemoveAt(0);
            Infotexts.RemoveAt(0);
            int foundcount = 0;
            for (int i = 0; i < Anchortexts.Count; i++)
            {

             
                string name = clearname(Path.GetFileNameWithoutExtension(Anchortexts[i].Attributes["href"].Value), console);
                string infolink = Anchortexts[i].Attributes["href"].Value;
                string purename = RemoveLanguajes(normalizename(name));
                string portrait = linkhelpers.ThumbnailsBaseurl + linkhelpers.ThumbnailsConsoles[Convert.ToInt32(console)] + linkhelpers.ThumbnailFolder; 
                if (!imgmap.ContainsKey(purename))
                    portrait +=  name + ".png";
                else
                {
                    foundcount++;
                    portrait += imgmap[purename];
                 
                }
                string filesize = Infotexts[i].InnerText.ToString().Split("\r\n")[0].Split(':')[1].Substring(2).Trim();
                string region = getregion(name);


                if (!infolink.EndsWith("/") && name.Trim() != "" && !infolink.StartsWith(".") && !infolink.EndsWith(".txt") && imgmap.ContainsKey(purename))
                {

                    InnerList.Add(new models.rominfo
                    {
                        DownloadLink = linkhelpers.TheeyeBaseurl + linkhelpers.TheeyeConsoles[Convert.ToInt32(console)] + infolink,
                        Name = name,
                        Portrait = portrait,
                        Size = filesize,
                        Region = region,
                        Console = ((Enums.Consola) console).ToString().Replace('_', ' ')
                       
                    });

                }
            }
            Console.WriteLine(foundcount + " Portadas encontradas de " + Anchortexts.Count + "   " + (Anchortexts.Count - foundcount) + "Portadas no encotradas");
            Console.WriteLine(" 100%");



            return InnerList;

        }




    }

    public class Enums {
        public enum Consola {
            GameBoy=1,
            GameBoyAdvance = 2,
            GameBoyColor = 3,
            Nintendo = 4,
            SuperNintendo = 5,
            Nintendo64 = 6,
            Playstation = 7,
            Nintendo_DS = 8,
            Sega_Genesis = 9,
            Sega_Dreamcast = 10

        };


    }

    //is used to do the GET request more easier and clear
    public class linkhelpers {
        public const string TheeyeBaseurl = "https://the-eye.eu/public/rom/";
        public const string ThumbnailsBaseurl = "https://raw.githubusercontent.com/libretro-thumbnails/";
        public const string ThumbnailFolder = "/master/Named_Boxarts/";
        public const string InfoRepoBaseUrl = "https://raw.githubusercontent.com/Gr3gorywolf/NeonRom3r/master/Rominfos/";
        public static Dictionary<int, string> TheeyeConsoles = new Dictionary<int, string>()
        {
                { 1,"Nintendo%20Gameboy/" },
                { 2,"Nintendo%20Gameboy%20Advance/" },
                { 3,"Nintendo%20Gameboy%20Color/" },
                { 4,"NES/" },
                { 5,"SNES/" },
                { 6,"Nintendo%2064/Roms/" },
                { 7,"Playstation/Games/NTSC/" },
                { 8,"Nintendo%20DS/" },
                { 9,"Sega%20Genesis/" },
                { 10,"Sega%20Dreamcast/" },


        };
        public static Dictionary<int, string> ThumbnailsConsoles = new Dictionary<int, string>()
        {
                { 1,"Nintendo_-_Game_Boy" },
                { 2,"Nintendo_-_Game_Boy_Advance" },
                { 3,"Nintendo_-_Game_Boy_Color" },
                { 4,"Nintendo_-_Nintendo_Entertainment_System" },
                { 5,"Nintendo_-_Super_Nintendo_Entertainment_System" },
                { 6,"Nintendo_-_Nintendo_64" },
                { 7,"Sony_-_PlayStation" },
                { 8,"Nintendo_-_Nintendo_DS" },
                { 9,"Sega_-_Mega_Drive_-_Genesis" },
                { 10,"Sega_-_Dreamcast" },


        };
        public static Dictionary<int, string> RepoConsoles = new Dictionary<int, string>()
        {
                { 1,"GB" },
                { 2,"GBA" },
                { 3,"GBC" },
                { 4,"NES" },
                { 5,"SNES" },
                { 6,"N64" },
                { 7,"PSX" },
                { 8,"NDS" },
                { 9,"Genesis" },
                { 10,"Dreamcast" },


        };




    }
    //data modeling. the rominfo is used to an individual rom and the romsdata for collections of roms
    public class models {

        public class rominfo{
            public string Name { get; set; }
            public string Portrait { get; set; }
            public string Size { get; set; }
            public string Region { get; set; }
            public string Console { get; set; }
            public string DownloadLink { get; set; }
        }
       public class romsdata {
            public string Name { get; set; }
            public string Portrait { get; set; }
            public string InfoLink { get; set; }
            public string Region { get; set; }
        }

    }

}
