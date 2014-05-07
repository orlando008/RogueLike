using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueLike
{
    class Program
    {
        private static bool _exit = false;
        private static OverallMap _ovMap;
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth - 10, Console.LargestWindowHeight- 10);

            _ovMap = new OverallMap();
            _ovMap.CreateLevel();

            Console.WriteLine(_ovMap.GetDrawingOfLevel(0));

            while (!_exit)
            {
                Console.Write(_ovMap.ThePlayer.LocationString());
                Console.Write(">");
                ProcessUserCommand();
            }

        }

        public static void ProcessUserCommand()
        {
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "w":
                    Console.Write("Moving up...");
                    _ovMap.ThePlayer.MovePlayer(0, -1);
                    break;
                case "a":
                    Console.Write("Moving left...");
                    _ovMap.ThePlayer.MovePlayer(-1, 0);
                    break;
                case "s":
                    Console.Write("Moving down...");
                    _ovMap.ThePlayer.MovePlayer(0, 1);
                    break;
                case "d":
                    Console.Write("Moving right...");
                    _ovMap.ThePlayer.MovePlayer(1, 0);
                    break;
                case "x":
                    _exit = true;
                    break;
                case "help":
                    PrintHelp();
                    break;
                case "draw":
                    Console.WriteLine(_ovMap.GetDrawingOfLevel(0));
                    break;
                default:
                    break;
            }
        }

        public static void PrintHelp()
        {
            Console.WriteLine("W/A/S/D");
        }
    }
}
