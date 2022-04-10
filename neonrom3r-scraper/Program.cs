using System;
using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Linq;
using neonrom3r_scraper.Src.Utils;
using neonrom3r_scraper.Src.Scrapers;
using neonrom3r_scraper.Src.Interfaces;

namespace neonrom3r_scraper
{
    class Program
    {
        static void Main(string[] args)
        {

            //will parse all images and extract all parsed data to json
            CompileImages();
            // will read the parsed images json and then will parse with the datata extracted from 
            //the-eye.eu
            CompileRomData();



            Console.WriteLine("extraccion completa...");


        }


        //it get all the images and generate the images based of html data that is in boxarts folder
        //also this method creates a json that have an array of objects that containes the following format
        // [{<normalized name>:<libretroportraits portrait name>}]
        public static void CompileImages()
        {
            if (!Directory.Exists("Boxartslist"))
                Directory.CreateDirectory("Boxartslist");

            foreach (var consoleKey in ConsolesConstants.ConsoleSlugs.Keys)
            {
                Dictionary<string, string> InnerList = new Dictionary<string, string>();
                var arch = File.ReadAllText("../../../BoxartsInfos/" + ConsolesConstants.ThumbnailsConsoles[consoleKey] + ".txt");
                var names = arch.Split("\n");
                foreach (var name in names)
                {
                    var currentname = name.Replace("\r","");
                    var normalizedname = ExtractionHelpers.NormalizeName(Path.GetFileNameWithoutExtension(currentname));
                    if (!InnerList.ContainsKey(normalizedname))
                        InnerList.Add(normalizedname, currentname);

                    var outputFile = File.CreateText("Boxartslist/" + ConsolesConstants.ConsoleSlugs[consoleKey] + ".json");
                    outputFile.Write(JsonConvert.SerializeObject(InnerList));
                    outputFile.Close();
                }
                Console.WriteLine(ConsolesConstants.ConsoleSlugs[consoleKey] + " portraits extracted!");
            }



        }
        /*it get from the-eye.eu roms and then normalizes its names to compare to the previously compiled boxarts
        to get the link of the portrait for that rom if is completedata it will extract it twice
        1rst extraction : gets the info of all roms
        2nd extraction : gets the info of every rom
         after the extraction it will create a folder for each console containing the jsons that have the info of that rom
         and also it will create a folder called data that will contain jsons with  all infos of every game of the console
        
             */
        public static void CompileRomData()
        {

            List<IRomScraper> scrapers = new List<IRomScraper>()
            {
               // new TheEyeScraper(),
                //new TheRomDepotScraper(),
                new EdgeEmulationScraper(),
                new SquidProxyScraper()

            };

            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");


            foreach (var consoleKey in ConsolesConstants.ConsoleSlugs.Keys)
            {
                var scraper = scrapers.Where((scr) => scr.HasConsoleRoms(consoleKey)).FirstOrDefault();
                if (scraper == null)
                {
                    Console.WriteLine("No scraper found for: " + ConsolesConstants.ConsoleSlugs[consoleKey]);
                }
                else
                {
                    var console = ConsolesConstants.ConsoleSlugs[consoleKey];
                    Dictionary<string, string> imgmap =
                JsonConvert
                .DeserializeObject<Dictionary<string, string>>(File.ReadAllText("Boxartslist/" + console + ".json"));

                    Console.WriteLine("Getting rom infos for:" + console);
                    var romsDataFile = File.CreateText("Data/" + console + ".json");
                    romsDataFile.Write(JsonConvert.SerializeObject(scraper.GetRomsData(consoleKey, imgmap)));
                    romsDataFile.Close();
                }
            }
        }
    }
}
