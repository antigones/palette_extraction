using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace PaletteExtraction
{
    class Program
    {

        static void Main(string[] args)
        {
            PaletteGenerator PaletteGenerator = new PaletteGenerator();
            PaletteGenerator.InputImage = new Bitmap(@"images\test.gif");
            List<Color> pcolors = PaletteGenerator.getPaletteWithHistogram(3);

            Console.WriteLine("Palette with getPaletteWithHistogram");
            foreach (Color c in pcolors)
            {
                Console.WriteLine(c.Name);
            }

            List<Color> mcColors = PaletteGenerator.getPaletteWithMedianCut();
            Console.WriteLine("Palette with getPaletteWithMedianCut");
            foreach (Color c in mcColors)
            {
                Console.WriteLine(c.Name);
            }
        }


    }
}
  
