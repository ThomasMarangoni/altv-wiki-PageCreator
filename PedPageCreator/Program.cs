using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PedModelsPageCreator;

namespace PedPageCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var pedsJson = "deps/peds.json";

            var pedsModelsGalleryFile = "Ped_Models_Gallery";
            var pedsModelsSnippetFile = "Ped_Models_Snippets";
            var fileExtension = ".txt";

            /*
             * Read JSON files from gta-v-data-dumps by DurtyFree
             */

            if (!File.Exists(pedsJson))
            {
                Environment.Exit(2);
            }

            using var readerPeds = new StreamReader(pedsJson);
            var jsonPeds = readerPeds.ReadToEnd();
            var peds = JsonConvert.DeserializeObject<List<Ped>>(jsonPeds);

            var sortedPedsByName = peds.OrderBy(x => x.Name);

            /*
             * Generate Content for wiki site Template:Ped_Models_Gallery
             */
            var gallery = File.CreateText(pedsModelsGalleryFile + fileExtension);

            gallery.WriteLine("<noinclude>");
            gallery.WriteLine("{{Template:" + pedsModelsGalleryFile + "/doc}}");
            gallery.WriteLine("</noinclude>");

            gallery.WriteLine("<hovergallery>");
            foreach (var ped in sortedPedsByName)
            {
                if (ped.DlcName.ToLower() == "titleupdate")
                {
                    gallery.WriteLine($"Image:{ped.Name.ToLower()}.png|'''Name:'''<br><code>{ped.Name.ToLower()}</code><br>'''Hash (Hex):<br>'''<code>0x{ped.HexHash}</code><br>'''Type:''' <br><code>{ped.Pedtype.ToLower()}</code>");
                }
                else
                {
                    gallery.WriteLine($"Image:{ped.Name.ToLower()}.png|'''Name:'''<br><code>{ped.Name.ToLower()}</code><br>'''Hash (Hex):<br>'''<code>0x{ped.HexHash}</code><br>'''Type:''' <br><code>{ped.Pedtype.ToLower()}</code><br>'''DLC:'''<br><code>{ped.DlcName.ToLower()}</code>");
                }
            }

            gallery.WriteLine("</hovergallery>");


            gallery.WriteLine("Created with [https://github.com/DurtyFree/gta-v-data-dumps GTA V Data Dumps from DurtyFree]");
            gallery.Close();
            Console.WriteLine($"{pedsModelsGalleryFile + fileExtension} created for {peds.Count} peds.");


            /*
             * Generate Content for wiki site Template:Ped_Models_Snippet
             */

            var snippets = File.CreateText(pedsModelsSnippetFile + fileExtension);

            snippets.WriteLine("<noinclude>");
            snippets.WriteLine("{{Template:" + pedsModelsSnippetFile + "/doc}}");
            snippets.WriteLine("</noinclude>");

            snippets.WriteLine("===Javascript===");
            snippets.WriteLine("<syntaxhighlight lang=\"javascript\">");
            snippets.WriteLine("let PedModel {");

            var i = 0;
            foreach (var ped in sortedPedsByName)
            {
                if (i < sortedPedsByName.Count() - 1)
                    snippets.WriteLine($"\t{ped.Name.ToLower()}: 0x{ped.HexHash},");
                else
                    snippets.WriteLine($"\t{ped.Name.ToLower()}: 0x{ped.HexHash}");

                i++;
            }
            snippets.WriteLine("};");
            snippets.WriteLine("</syntaxhighlight>");
            snippets.WriteLine("Created with [https://github.com/DurtyFree/gta-v-data-dumps GTA V Data Dumps from DurtyFree]");

            snippets.WriteLine("");
            snippets.WriteLine("===Typescript===");
            snippets.WriteLine("<syntaxhighlight lang=\"typescript\">");
            snippets.WriteLine("export enum PedModel {");

            i = 0;
            foreach (var ped in sortedPedsByName)
            {
                if (i < sortedPedsByName.Count() - 1)
                    snippets.WriteLine($"\t{ped.Name.ToLower()} = 0x{ped.HexHash},");
                else
                    snippets.WriteLine($"\t{ped.Name.ToLower()} =  0x{ped.HexHash}");

                i++;
            }

            snippets.WriteLine("};");
            snippets.WriteLine("</syntaxhighlight>");
            snippets.WriteLine("Created with [https://github.com/DurtyFree/gta-v-data-dumps GTA V Data Dumps from DurtyFree]");

            snippets.WriteLine("");
            snippets.WriteLine("===Lua===");
            snippets.WriteLine("<syntaxhighlight lang=\"lua\">");
            snippets.WriteLine("PedModel = {");

            foreach (var ped in sortedPedsByName)
            {
                snippets.WriteLine($"\t{ped.Name.ToLower()} = 0x{ped.HexHash},");
            }

            snippets.WriteLine("};");
            snippets.WriteLine("</syntaxhighlight>");
            snippets.WriteLine("Created with [https://github.com/DurtyFree/gta-v-data-dumps GTA V Data Dumps from DurtyFree]");

            snippets.Close();
            Console.WriteLine($"{pedsModelsSnippetFile + fileExtension} created for {peds.Count} peds.");

            Console.WriteLine("This tool is using data files from https://github.com/DurtyFree/gta-v-data-dumps");
        }
    }
}
