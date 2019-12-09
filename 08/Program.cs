using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace _08
{
    class Program
    {
        static IEnumerable<string> GetLayers(string image, int width, int height)
        {
            var layerSize = width * height;
            Debug.Assert(image.Length % layerSize == 0);
            for (int layerStartIndex = 0; layerStartIndex < image.Length; layerStartIndex += layerSize)
            {
                yield return image.Substring(layerStartIndex, layerSize);
            }
        }

        static int Part1(IEnumerable<string> layers)
        {
            var targetLayer = "";
            var value = int.MaxValue;
            foreach (var layer in layers)
            {
                var numZeroes = layer.Count(ch => ch == '0');
                if (numZeroes < value)
                {
                    value = numZeroes;
                    targetLayer = layer;
                }
            }

            return targetLayer.Count(ch => ch == '1') * targetLayer.Count(ch => ch == '2');
        }

        static char MergePixel(char top, char bottom) => top != '2' ? top : bottom;

        static string MergeLayers(string top, string bottom) => String.Concat(top.Zip(bottom, MergePixel));

        static string RenderImage(IEnumerable<string> layers) => layers.Aggregate(MergeLayers);

        static char Pixel2Char(char pixel) => pixel == '0' ? ' ' : '#';

        static void PrintLine(string line) => Console.WriteLine(String.Join("", line.Select(Pixel2Char)));

        static void PrintImage(string image, int width, int height)
        {
            for (int i = 0; i < height; i++)
            {
                PrintLine(image.Substring(i * width, width));
            }
        }

        static void Main(string[] args)
        {
            var image = System.IO.File.ReadAllText("input.txt").Trim();
            var layers = GetLayers(image, 25, 6);

            Console.WriteLine(Part1(layers));
            PrintImage(RenderImage(layers), 25, 6);
        }
    }
}
