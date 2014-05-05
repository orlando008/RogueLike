using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueLike
{
    class Program
    {
        static void Main(string[] args)
        {
            OverallMap ovMap = new OverallMap();
            ovMap.CreateLevel();
            Console.WriteLine(ovMap.ToString());

            Console.Read();
        }
    }
}
