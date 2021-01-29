using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace VehiclePageCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var vehiclesClassesJson = "deps/vehiclesClasses.json";
            var vehiclesJson = "deps/vehicles.json";

            var vehiclesModelsGalleryFile = "Vehicle_Models_Gallery";
            var vehiclesModelsSnippetFile = "Vehicle_Models_Snippets";
            var fileExtension = ".txt";

            /*
             * Read JSON files from gta-v-data-dumps by DurtyFree
             */

            if (!File.Exists(vehiclesClassesJson))
            {
                Environment.Exit(1);
            }

            if (!File.Exists(vehiclesJson))
            {
                Environment.Exit(2);
            }

            using var readerVehicleClasses = new StreamReader(vehiclesClassesJson);
            var jsonVehicleClasses = readerVehicleClasses.ReadToEnd();
            var vehicleClasses = JsonConvert.DeserializeObject<List<string>>(jsonVehicleClasses);

            using var readerVehicles = new StreamReader(vehiclesJson);
            var jsonVehicles = readerVehicles.ReadToEnd();
            var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(jsonVehicles);

            var sortedVehiclesByName = vehicles.OrderBy(x => x.Name);

            /*
             * Generate Content for wiki site Template:Ped_Models_Gallery
             */
            var gallery = File.CreateText(vehiclesModelsGalleryFile + fileExtension);

            gallery.WriteLine("<noinclude>");
            gallery.WriteLine("{{Template:" + vehiclesModelsGalleryFile + "/doc}}");
            gallery.WriteLine("</noinclude>");

            foreach (var vehicleClass in vehicleClasses)
            {
                gallery.WriteLine("==" + vehicleClass + "==");
                gallery.WriteLine("<hovergallery>");
                var vehiclesByClass = sortedVehiclesByName.Where(x => x.Class == vehicleClass);
                foreach (var vehicle in vehiclesByClass)
                {
                    if (vehicle.DlcName.ToLower() == "titleupdate")
                    {
                        gallery.WriteLine($"Image:{vehicle.Name.ToLower()}.png|'''Name:'''<br><code>{vehicle.Name.ToLower()}</code><br>'''Hash (Hex):<br>'''<code>0x{vehicle.HexHash}</code><br>'''Display Name:''' <br><code>{vehicle.DisplayName}</code>");
                    }
                    else
                    {
                        gallery.WriteLine($"Image:{vehicle.Name.ToLower()}.png|'''Name:'''<br><code>{vehicle.Name.ToLower()}</code><br>'''Hash (Hex):<br>'''<code>0x{vehicle.HexHash}</code><br>'''Display Name:''' <br><code>{vehicle.DisplayName}</code><br>'''DLC:'''<br><code>{vehicle.DlcName.ToLower()}</code>");
                    }
                }
                gallery.WriteLine("</hovergallery>");
            }

            gallery.WriteLine("Created with [https://github.com/DurtyFree/gta-v-data-dumps GTA V Data Dumps from DurtyFree]");
            gallery.Close();
            Console.WriteLine($"{vehiclesModelsGalleryFile + fileExtension} created for {vehicles.Count} vehicles.");


            /*
             * Generate Content for wiki site Template:Vehicle_Models_Snippet
             */

            var snippets = File.CreateText(vehiclesModelsSnippetFile + fileExtension);

            snippets.WriteLine("<noinclude>");
            snippets.WriteLine("{{Template:" + vehiclesModelsSnippetFile + "/doc}}");
            snippets.WriteLine("</noinclude>");

            snippets.WriteLine("===Javascript===");
            snippets.WriteLine("<syntaxhighlight lang=\"javascript\">");
            snippets.WriteLine("let VehicleModel {");

            var i = 0;
            foreach (var vehicle in sortedVehiclesByName)
            {
                if (i < sortedVehiclesByName.Count() - 1)
                    snippets.WriteLine($"\t{vehicle.Name.ToLower()}: 0x{vehicle.HexHash},");
                else
                    snippets.WriteLine($"\t{vehicle.Name.ToLower()}: 0x{vehicle.HexHash}");

                i++;
            }
            snippets.WriteLine("};");
            snippets.WriteLine("</syntaxhighlight>");
            snippets.WriteLine("Created with [https://github.com/DurtyFree/gta-v-data-dumps GTA V Data Dumps from DurtyFree]");

            snippets.WriteLine("");
            snippets.WriteLine("===Typescript===");
            snippets.WriteLine("<syntaxhighlight lang=\"typescript\">");
            snippets.WriteLine("export enum VehicleModel {");

            i = 0;
            foreach (var vehicle in sortedVehiclesByName)
            {
                if (i < sortedVehiclesByName.Count() - 1)
                    snippets.WriteLine($"\t{vehicle.Name.ToLower()} = 0x{vehicle.HexHash},");
                else
                    snippets.WriteLine($"\t{vehicle.Name.ToLower()} =  0x{vehicle.HexHash}");

                i++;
            }

            snippets.WriteLine("};");
            snippets.WriteLine("</syntaxhighlight>");
            snippets.WriteLine("Created with [https://github.com/DurtyFree/gta-v-data-dumps GTA V Data Dumps from DurtyFree]");

            snippets.WriteLine("");
            snippets.WriteLine("===Lua===");
            snippets.WriteLine("<syntaxhighlight lang=\"lua\">");
            snippets.WriteLine("VehicleModel = {");

            foreach (var vehicle in sortedVehiclesByName)
            {
                snippets.WriteLine($"\t{vehicle.Name.ToLower()} = 0x{vehicle.HexHash},");
            }

            snippets.WriteLine("};");
            snippets.WriteLine("</syntaxhighlight>");
            snippets.WriteLine("Created with [https://github.com/DurtyFree/gta-v-data-dumps GTA V Data Dumps from DurtyFree]");

            snippets.Close();
            Console.WriteLine($"{vehiclesModelsSnippetFile + fileExtension} created for {vehicles.Count} vehicles.");

            Console.WriteLine("This tool is using data files from https://github.com/DurtyFree/gta-v-data-dumps");
        }
    }
}
