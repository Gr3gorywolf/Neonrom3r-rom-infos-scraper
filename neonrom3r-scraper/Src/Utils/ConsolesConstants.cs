using neonrom3r_scraper.Src.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace neonrom3r_scraper.Src.Utils
{
    class ConsolesConstants
    {
         
         

        //thumbnails files
        public static Dictionary<int, string> ThumbnailsConsoles = new Dictionary<int, string>()
        {
                { (int)Consoles.GameBoy,"Nintendo_-_Game_Boy" },
                { (int)Consoles.GameBoyAdvance,"Nintendo_-_Game_Boy_Advance" },
                { (int)Consoles.GameBoyColor,"Nintendo_-_Game_Boy_Color" },
                { (int)Consoles.Nintendo,"Nintendo_-_Nintendo_Entertainment_System" },
                { (int)Consoles.SuperNintendo,"Nintendo_-_Super_Nintendo_Entertainment_System" },
                { (int)Consoles.Nintendo64,"Nintendo_-_Nintendo_64" },
                { (int)Consoles.Playstation,"Sony_-_PlayStation" },
                { (int)Consoles.Nintendo_DS,"Nintendo_-_Nintendo_DS" },
                { (int)Consoles.Sega_Genesis,"Sega_-_Mega_Drive_-_Genesis" },
                { (int)Consoles.Sega_Dreamcast,"Sega_-_Dreamcast" },
                { (int)Consoles.NintendoGamecube,"Nintendo_-_GameCube" },
                { (int)Consoles.Playstation2,"Sony_-_PlayStation_2" },
                { (int)Consoles.Psp,"Sony_-_PlayStation_Portable" },
        };

        //slugs
        public static Dictionary<int, string> ConsoleSlugs = new Dictionary<int, string>()
        {
                { (int)Consoles.SuperNintendo,"SNES" },
                { (int)Consoles.Nintendo,"NES" },
                { (int)Consoles.GameBoy,"GB" },
                { (int)Consoles.GameBoyAdvance,"GBA" },
                { (int)Consoles.GameBoyColor,"GBC" },
                { (int)Consoles.Nintendo64,"N64" },
                { (int)Consoles.Playstation,"PSX" },
                { (int)Consoles.Nintendo_DS,"NDS" },
                { (int)Consoles.Sega_Genesis,"Genesis" },
                { (int)Consoles.Sega_Dreamcast,"Dreamcast" },
                { (int)Consoles.Psp,"PSP" },
                { (int)Consoles.NintendoGamecube,"Gamecube" },
                { (int)Consoles.Playstation2,"PS2" }
        };
    }
}
