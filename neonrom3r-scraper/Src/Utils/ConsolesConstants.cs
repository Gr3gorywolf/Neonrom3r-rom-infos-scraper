using neonrom3r_scraper.Src.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace neonrom3r_scraper.Src.Utils
{
    class ConsolesConstants
    {

        //the-eye consoles url
        public static Dictionary<int, string> TheeyeConsoles = new Dictionary<int, string>()
        {
                { (int)Consoles.GameBoy,"Nintendo%20Gameboy/" },
                { (int)Consoles.GameBoyAdvance,"Nintendo%20Gameboy%20Advance/" },
                { (int)Consoles.GameBoyColor,"Nintendo%20Gameboy%20Color/" },
                { (int)Consoles.Nintendo,"NES/" },
                { (int)Consoles.SuperNintendo,"SNES/" },
                { (int)Consoles.Nintendo64,"Nintendo%2064/Roms/" },
                { (int)Consoles.Playstation,"Playstation/Games/NTSC/" },
               // { (int)Consoles.Nintendo_DS,"Nintendo%20DS/" },
                {(int)Consoles.Sega_Genesis,"Sega%20Genesis/" },
                { (int)Consoles.Sega_Dreamcast,"Sega%20Dreamcast/" },
        };

        //SquidProxyConsoles consoles url
        public static Dictionary<int, string> SquidProxyConsoles = new Dictionary<int, string>()
        {
                { (int)Consoles.Nintendo_DS,"Nintendo%20DS/" },
                { (int)Consoles.Playstation2,"Sony%20Playstation%202/NTSC-U/" },
                { (int)Consoles.NintendoGamecube,"Nintendo%20Gamecube/NTSC-U/" },
        };

        //thumbnails files
        public static Dictionary<int, string> ThumbnailsConsoles = new Dictionary<int, string>()
        {
                { (int)Consoles.GameBoy,"Nintendo-Game_Boy" },
                { (int)Consoles.GameBoyAdvance,"Nintendo-Game_Boy_Advance" },
                { (int)Consoles.GameBoyColor,"Nintendo-Game_Boy_Color" },
                { (int)Consoles.Nintendo,"Nintendo-Nintendo_Entertainment_System" },
                { (int)Consoles.SuperNintendo,"Nintendo-Super_Nintendo_Entertainment_System" },
                { (int)Consoles.Nintendo64,"Nintendo-Nintendo_64" },
                { (int)Consoles.Playstation,"Sony-PlayStation" },
                { (int)Consoles.Nintendo_DS,"Nintendo-Nintendo_DS" },
                { (int)Consoles.Sega_Genesis,"Sega-Mega_Drive_Genesis" },
                { (int)Consoles.Sega_Dreamcast,"Sega-Dreamcast" },
                { (int)Consoles.Psp,"Sony-Psp" },
                { (int)Consoles.NintendoGamecube,"Nintendo-Gamecube" },
                { (int)Consoles.Playstation2,"Sony-Playstation2" },
        };

        //slugs
        public static Dictionary<int, string> ConsoleSlugs = new Dictionary<int, string>()
        {
                { (int)Consoles.GameBoy,"GB" },
                { (int)Consoles.GameBoyAdvance,"GBA" },
                { (int)Consoles.GameBoyColor,"GBC" },
                { (int)Consoles.Nintendo,"NES" },
                { (int)Consoles.SuperNintendo,"SNES" },
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
