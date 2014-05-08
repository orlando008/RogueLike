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


            StartNewGame();

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
            string[] userInputArray = userInput.Split(" ".ToCharArray());
            int lengthOfMove = 1;

            switch (userInputArray[0].ToUpper())
            {
                case "MOVEX":
                    Console.Write("Moving in X direction...");

                    if (userInputArray.Length > 1)
                        lengthOfMove = Convert.ToInt32(userInputArray[1]);

                    for(int i = 0; i < lengthOfMove; i++)
                        _ovMap.ThePlayer.MovePlayer(1, 0);
                    break;
                case "MOVEY":
                    Console.Write("Moving in Y direction...");
                    lengthOfMove = 1;

                    if (userInputArray.Length > 1)
                        lengthOfMove = Convert.ToInt32(userInputArray[1]);

                    for(int i = 0; i < lengthOfMove; i++)
                        _ovMap.ThePlayer.MovePlayer(0, 1);
                    break;
                case "EXIT":
                    _exit = true;
                    break;
                case "HELP":
                    PrintHelp();
                    break;
                case "DRAW":
                    Console.WriteLine(_ovMap.GetDrawingOfLevel(0));
                    break;
                case "SEED":
                    Console.WriteLine(_ovMap.Seed.ToString());
                    break;
                case "NEWGAME":
                    StartNewGame();
                    break;
                default:
                    Console.WriteLine("Command not recognized.");
                    break;
            }
        }

        public static void PrintHelp()
        {
            Console.WriteLine("W/A/S/D");
        }

        public static void StartNewGame()
        {
            Console.Clear();

            int seed = 0;
#if DEBUG
            Console.Write("Seed?");
            string seedString = Console.ReadLine();

            if (Int32.TryParse(seedString, out seed) == false)
                seed = 0;
            else
                seed = Convert.ToInt32(seedString);
#endif

            Console.Clear();
            _ovMap = new OverallMap(seed);
            _ovMap.CreateLevel();
            Console.WriteLine(_ovMap.GetDrawingOfLevel(0));
        }
    }
}
