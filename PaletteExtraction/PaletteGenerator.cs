using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaletteExtraction
{
    class PaletteGenerator
    {
        public Bitmap InputImage;
        GroupComparer gc = new GroupComparer();

        int calcMedianValue(List<byte> values)
        {
            //it's not a median value, it's a mean value (simplification here)
            values.Sort();
            int mIdx = (int)Math.Round((double)values.Count / 2);
            return values[mIdx];
        }

        List<List<Color>> splitPixels(List<Color> colors, int medianValue, int dimension)
        {
            List<List<Color>> bins = new List<List<Color>>() ;
            bins.Add(new List<Color>());
            bins.Add(new List<Color>());
      
            if (dimension == 0)
            {
                for (int i = 0; i < colors.Count; i++)
                {
                    Color color = colors[i];
                    int r = color.R;
                    if (r <= medianValue)
                        bins[0].Add(color);
                    else
                        bins[1].Add(color);
                }
            }
            if (dimension == 1)
            {
                for (int i = 0; i < colors.Count; i++)
                {
                    Color color = colors[i];
                    int g = color.G;
                    if (g <= medianValue)
                        bins[0].Add(color);
                    else
                        bins[1].Add(color);
                }
            }
            if (dimension == 2)
            {
                for (int i = 0; i < colors.Count; i++)
                {
                    Color color = colors[i];
                    int b = color.B;
                    if (b <= medianValue)
                        bins[0].Add(color);
                    else
                        bins[1].Add(color);
                }
            }
            // 0: lower, 1:upper
            return bins;
        }

        RangeInfo GetMaxSpanDimension(List<List<Color>> bins)
        {
            RangeInfo maxRange;
            List<RangeInfo> ranges = new();

            for (int i = 0; i < bins.Count; i++)
            {
                List<Color> colors = bins[i];
                List<byte> rList = new();
                List<byte> gList = new();
                List<byte> bList = new();

                for (int j = 0; j < colors.Count; j++)
                {
                    Color color = colors[j];

                    byte r = color.R;
                    byte g = color.G;
                    byte b = color.B;


                    rList.Add(r);
                    gList.Add(g);
                    bList.Add(b);
                }
                rList.Sort();
                gList.Sort();
                bList.Sort();
                int rMax = rList[rList.Count - 1];
                int rMin = rList[0];
                int gMax = gList[gList.Count - 1];
                int gMin = gList[0];
                int bMax = bList[bList.Count - 1];
                int bMin = bList[0];
                ranges.Add(
                   new RangeInfo
                   {
                       Color = 0,
                       Index = i,
                       Colors = colors,
                       Range = rMax - rMin,
                       RList = rList,
                       GList = gList,
                       BList = bList,
                       RMax = rMax,
                       RMin = rMin,
                       GMax = gMax,
                       GMin = gMin,
                       BMax = bMax,
                       BMin = bMin
                   });
                ranges.Add(
                   new RangeInfo
                   {
                       Color = 1,
                       Index = i,
                       Colors = colors,
                       Range = gMax - gMin,
                       RList = rList,
                       GList = gList,
                       BList = bList,
                       RMax = rMax,
                       RMin = rMin,
                       GMax = gMax,
                       GMin = gMin,
                       BMax = bMax,
                       BMin = bMin
                   });
                ranges.Add(
                new RangeInfo
                {
                    Color = 2,
                    Index = i,
                    Colors = colors,
                    Range = bMax - bMin,
                    RList = rList,
                    GList = gList,
                    BList = bList,
                    RMax = rMax,
                    RMin = rMin,
                    GMax = gMax,
                    GMin = gMin,
                    BMax = bMax,
                    BMin = bMin
                });

            }
            
            ranges.Sort(gc);

            maxRange = ranges[0];
            return maxRange;
        }

        List<List<Color>> processBins(int i, List<List<Color>> groups)
        {
            if (i < 9)
            {
                List<List<Color>> bins = new();

                RangeInfo groupToSplit = GetMaxSpanDimension(groups);
                if (groupToSplit.Color == 0)
                {
                    //Console.WriteLine("splitting along red");
                    int medianValue = calcMedianValue(groupToSplit.RList);
                    bins = splitPixels(groups[groupToSplit.Index], medianValue, 0);
                }

                if (groupToSplit.Color == 1)
                {
                    //Console.WriteLine("splitting along green");
                    int medianValue = calcMedianValue(groupToSplit.GList);
                    bins = splitPixels(groups[groupToSplit.Index], medianValue, 1);
                }

                if (groupToSplit.Color == 2)
                {
                    //Console.WriteLine("splitting along blue");
                    int medianValue = calcMedianValue(groupToSplit.BList);
                    bins = splitPixels(groups[groupToSplit.Index], medianValue, 2);
                }

                groups.RemoveAt(groupToSplit.Index);
                groups.Insert(0, bins[0]);
                groups.Insert(0, bins[1]);

                return processBins(i + 1, groups);
            }
            else
            {
            
                return groups;
            }
        }

        public List<Color> getPaletteWithMedianCut()
        {
            List<Color> colorPalette = new();

            int w = InputImage.Width;
            int h = InputImage.Height;


            List<List<Color>> bins = new();
            bins.Add(new List<Color>() );

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                   Color color = InputImage.GetPixel(x,y);

                    bins[0].Add(color);
                }
            }
            
            List<List<Color>> totBins = new();
            totBins.Add(bins[0]);

            int i = 0;

            List<List<Color>> gs = processBins(i, totBins);
           
            for (int j = 0; j < gs.Count; j++)
            {
                
                // get colors in bin
                List<Color> colors = gs[j];
                Color meanColor = CalcMeanColor(colors);
                colorPalette.Add(meanColor);
            }
            return colorPalette;
        }

        Color CalcMeanColor(List<Color> colors)
        {
            int sumR = 0;
            int sumG = 0;
            int sumB = 0;
            for (int c = 0; c < colors.Count; c++)
            {
                Color color = colors[c];

                byte r = color.R;
                byte g = color.G;
                byte b = color.B;
                sumR = sumR + r;
                sumG = sumG + g;
                sumB = sumB + b;
            }

            byte avgR = (byte)(sumR / colors.Count);
            byte avgG = (byte)(sumG / colors.Count);
            byte avgB = (byte)(sumB / colors.Count);

            Color finalColor = Color.FromArgb(avgR, avgG, avgB);
            return finalColor;
        }

        public List<Color> getPaletteWithHistogram(int nBins)
        {
            List<Color> colorPalette = new List<Color>();
            
            Dictionary<ValueTuple<int, int, int>, List<Color>> bins =
                    new Dictionary<ValueTuple<int, int, int>, List<Color>>();

       
            double binLen = 256.0 / nBins;

            for (int i = 0; i < nBins; i++)
                for (int j = 0; j < nBins; j++)
                    for (int k = 0; k < nBins; k++)
                        bins[(i, j, k)] = new List<Color>();


            for (int x = 0; x < InputImage.Width; x++)
                for (int y = 0; y < InputImage.Height; y++)
                {
                    Color pixelColor = InputImage.GetPixel(x, y);
                    byte r = pixelColor.R;
                    byte g = pixelColor.G;
                    byte b = pixelColor.B;

                    int first = (int)Math.Floor(r / binLen);
                    int second = (int)Math.Floor(g / binLen);
                    int third = (int)Math.Floor(b / binLen);

                    bins[(first, second, third)].Add(pixelColor);
                }

            SortedDictionary<int, List<ValueTuple<int, int, int>>> binCount =
                new SortedDictionary<int, List<ValueTuple<int, int, int>>>();
            // collection of tuples is needed to avoid clashing counts
            // binCount just maps the count to bin
            for (int i = 0; i < nBins; i++)
                for (int j = 0; j < nBins; j++)
                    for (int k = 0; k < nBins; k++)
                    {
                        int l = bins[(i, j, k)].Count;
                        if (!binCount.ContainsKey(l))
                            binCount[l] = new List<(int, int, int)>();
                        binCount[l].Add((i, j, k));
                    }


            // sort binCount by key and *try* to take first 10 to generate a 10-colour palette
            int[] sortedKeys = binCount.Keys.ToArray();
            List<ValueTuple<int, int, int>> topTenBuckets = new List<ValueTuple<int, int, int>>();

            int cc = 0;
            // be careful about number of available keys here
            while (cc < 10 && cc < sortedKeys.Length)
            {
                List<ValueTuple<int, int, int>> b = binCount[sortedKeys.ElementAt(sortedKeys.Length - 1 - cc)];
                foreach (ValueTuple<int, int, int> element in b)
                    topTenBuckets.Add(element);
                cc++;
            }

            for (int i = 0; i < topTenBuckets.Count; i++)
            {

                (int, int, int) mesh = topTenBuckets.ElementAt(i);

                // get colors in bin
                List<Color> colors = bins[mesh];

                Decimal perc = 100 * ((decimal)colors.Count / ((decimal)InputImage.Width * (decimal)InputImage.Height));
                Console.WriteLine("{0:N2}", perc);

                // handle mono-color img
                if (colors.Count > 0)
                {
                    Color meanColor = CalcMeanColor(colors);
                    colorPalette.Add(meanColor);
                }
            }
            return colorPalette;
        }

        Bitmap ScaleImage(Bitmap inputImage)
        {
            var imgWidth = inputImage.Width;
            var imgHeight = inputImage.Height;

            var maxDimension = imgWidth > imgHeight ? imgWidth : imgHeight;

            var scale = (Decimal)maxDimension / 100;
            var canvasHeight = (int)(imgHeight / scale);
            var canvasWidth = (int)(imgWidth / scale);

            Console.WriteLine((canvasHeight * canvasWidth).ToString());

            Bitmap resizedBitmap = new Bitmap(inputImage, new Size(canvasWidth, canvasHeight));
            return resizedBitmap;
        }
    }
}
