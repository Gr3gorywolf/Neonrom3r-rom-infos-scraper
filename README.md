
&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;<img src="https://raw.githubusercontent.com/Gr3gorywolf/Neonrom3r-rom-infos-scraper/master/neonrom3r%20scraper/logo.png"/>
# Neonrom3r-rom-infos-scraper
A .net core simple console application that scrap all the roms info from <a  href="https://the-eye.eu/public/rom/">the-eye.eu</a>  and <a href="https://github.com/libretro-thumbnails/libretro-thumbnails"> libretro-thumbnails </a> and then generate a json containing the info of every rom 

This application is based on modified <a href="https://github.com/Gr3gorywolf/EmulatorGamesSuperscraper">Emulator.games scraper</a>  but this 
one is no 100% dependant of the webpage because once the data is extacted you just need to put that data in an http server to use it on your
app because the data is extracted in json format(that can be parsed with almost all the languajes) you can get an example of what is the output of this scraper <a href="https://github.com/Gr3gorywolf/NeonRom3r/tree/master/Rominfos">Right here</a>
#notes
to get it work you must put the boxarts folder into the debug/release folder because this folder is used to extract the portraits names
and also have the following dependences
-Html agility pack (1.8.11)
-newtonsoft json(12.0.1)
