using System;
using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Linq;
namespace neonrom3r_scraper
{
    class Program
    {
        static void Main(string[] args)
        {

            //will parse all images and extract all parsed data to json
            compileimages();
            // will read the parsed images json and then will parse with the datata extracted from 
            //the-eye.eu
            //compiledata(true);


         
            Console.WriteLine("extraccion completa...");
            
            
        }


        //it get all the images and generate the images based of html data that is in boxarts folder
        //also this method creates a json that have an array of objects that containes the following format
        // [{<normalized name>:<libretroportraits portrait name>}]
        public static void compileimages()
        {
            neonromerscraper scraper = new neonromerscraper();
            if (!Directory.Exists("Boxartslist"))
                Directory.CreateDirectory("Boxartslist");
            for (int i = 1; i < 11; i++)
            {
                Dictionary<string, string> InnerList = new Dictionary<string, string>();
                var arch=File.ReadAllText("BoxartsInfos/" + linkhelpers.ThumbnailsConsoles[i] + ".txt");
                var names = arch.Split("\n");
                List<string> nombres = new List<string>();
                foreach (var xd in names)
                {
                    var currentname = xd;
                    var normalizedname = scraper.normalizename(Path.GetFileNameWithoutExtension(currentname));
                    nombres.Add(currentname);
                    if (!InnerList.ContainsKey(normalizedname))
                        InnerList.Add(normalizedname, currentname);

                    var f = File.CreateText("Boxartslist/" + linkhelpers.RepoConsoles[i] + ".json");
                    f.Write(JsonConvert.SerializeObject(InnerList));
                    f.Close();
                }

                Console.WriteLine("extracted!");
            }

        }
        /*it get from the-eye.eu roms and then normalizes its names to compare to the previously compiled boxarts
        to get the link of the portrait for that rom if is completedata it will extract it twice
        1rst extraction : gets the info of all roms
        2nd extraction : gets the info of every rom
         after the extraction it will create a folder for each console containing the jsons that have the info of that rom
         and also it will create a folder called data that will contain jsons with  all infos of every game of the console
        
             */
        public static void compiledata(bool iscompletedata) {


            neonromerscraper scraper = new neonromerscraper();
           
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");

            for (int i = 1; i < 11; i++)
            {

                Dictionary<string, string> imgmap =
             JsonConvert
             .DeserializeObject<Dictionary<string, string>>(File.ReadAllText("Boxartslist/" + linkhelpers.RepoConsoles[i] + ".json"));

                Console.WriteLine("Obteniendo datos de:" + linkhelpers.RepoConsoles[i]);
                var archi = File.CreateText("Data/" + linkhelpers.RepoConsoles[i] + ".json");
                archi.Write(JsonConvert.SerializeObject(scraper.GetRomsData(i, imgmap)));
                archi.Close();





                if (iscompletedata)
                {
                    if (!Directory.Exists(linkhelpers.RepoConsoles[i]))
                        Directory.CreateDirectory(linkhelpers.RepoConsoles[i]);

                    var infos = scraper.GetRomsInfos(i, imgmap);
                    foreach (var inf in infos)
                    {
                        var archi2 = File.CreateText(linkhelpers.RepoConsoles[i] + "/" + inf.Name + ".json");
                        archi2.Write(JsonConvert.SerializeObject(inf));
                        archi2.Close();


                    }
                }
                ////////////////////
            }


        }



    }
}
