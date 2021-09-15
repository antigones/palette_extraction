using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaletteExtraction
{
    class GroupComparer : IComparer<RangeInfo>
    {
        public int Compare(RangeInfo x, RangeInfo y)
        {
            if (y.Range > x.Range) return 1;
            if (y.Range < x.Range) return -1;
            return 0;
        }
    }
}
