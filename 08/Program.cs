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
            image = image.Trim();
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

        static void Main(string[] args)
        {
            var image = System.IO.File.ReadAllText("input.txt");
            var layers = GetLayers(image, 25, 6);

            Console.WriteLine(Part1(layers));

        }
    }
}
