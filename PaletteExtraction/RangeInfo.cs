using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaletteExtraction
{
    class RangeInfo
    {
        public int Color { get; set; }
        public int Index { get; set; }
        public List<Color> Colors { get; set; }
        public int Range { get; set; }
        public List<byte> RList { get; set; }
        public List<byte> GList { get; set; }
        public List<byte> BList { get; set; }
        public int RMax { get; set; }
        public int RMin { get; set; }
        public int GMax { get; set; }
        public int GMin { get; set; }
        public int BMax { get; set; }
        public int BMin { get; set; }
  
    }
}
