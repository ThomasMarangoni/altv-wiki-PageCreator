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
            if (!File.Exists("deps/vehiclesClasses.json"))
            {
                Environment.Exit(1);
            }

            if (!File.Exists("deps/vehicles.json"))
            {
                Environment.Exit(2);
            }

            using var readerVehicleClasses = new StreamReader("deps/vehiclesClasses.json");
            var jsonVehicleClasses = readerVehicleClasses.ReadToEnd();
            var vehicleClasses = JsonConvert.DeserializeObject<List<string>>(jsonVehicleClasses);

            using var readerVehicles = new StreamReader("deps/vehicles.json");
            var jsonVehicles = readerVehicles.ReadToEnd();
            var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(jsonVehicles);

            var sortedVehiclesByName = vehicles.OrderBy(x => x.Name);

            var gallery = File.CreateText("gallery.txt");

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
            Console.WriteLine($"gallery.txt created for {vehicles.Count} vehicles.");

        }
    }
}
