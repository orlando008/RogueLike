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
        private static string RAN_INTO_WALL = "You ran into a wall.";
        private static string GAME_NAME = "Dungeon Adventure 2014";

        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth - 10, Console.LargestWindowHeight- 10);
            Console.Title = GAME_NAME;

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
                    Console.WriteLine("Moving in X direction...");

                    if (userInputArray.Length > 1)
                        lengthOfMove = Convert.ToInt32(userInputArray[1]);

                    for (int i = 0; i < lengthOfMove; i++)
                    {
                        if (_ovMap.ThePlayer.MovePlayer(1, 0))
                            _ovMap.DiscoverTilesAroundPlayer();
                        else
                        {
                            Console.WriteLine(RAN_INTO_WALL);
                            break;
                        }  
                    }
                        

                    _ovMap.DiscoverTilesAroundPlayer();
                    break;
                case "MOVEY":
                    Console.WriteLine("Moving in Y direction...");
                    lengthOfMove = 1;

                    if (userInputArray.Length > 1)
                        lengthOfMove = Convert.ToInt32(userInputArray[1]);

                    for (int i = 0; i < lengthOfMove; i++)
                    {
                        if (_ovMap.ThePlayer.MovePlayer(0, 1))
                            _ovMap.DiscoverTilesAroundPlayer();
                        else
                        {
                            Console.WriteLine(RAN_INTO_WALL);
                            break;
                        }      
                    }

                    
                    break;
                case "EXIT":
                    _exit = true;
                    break;
                case "HELP":
                    PrintHelp();
                    break;
                case "DRAW":
                    Console.Clear();
                    _ovMap.DrawLevelDirect(_ovMap.ThePlayer.DungeonLevel, true);
                    break;
                case "SEED":
                    Console.WriteLine(_ovMap.Seed.ToString());
                    break;
                case "NEWGAME":
                    StartNewGame();
                    break;
#if DEBUG
                case "DRAWALL":
                    Console.Clear();
                    _ovMap.DrawLevelDirect(_ovMap.ThePlayer.DungeonLevel, false);
                    break;
#endif
                default:
                    Console.WriteLine("Command not recognized.");
                    break;
            }
        }

        public static void PrintHelp()
        {
            StringBuilder sb = new StringBuilder();
            Console.BackgroundColor = ConsoleColor.Yellow;
            sb.AppendLine(">----------------------------------------------------------------} " + GAME_NAME + " {----------------------------------------------------------------<");
            sb.AppendLine();

            sb.AppendLine("All commands are case insensitive.");
            sb.AppendLine("Commands are listed with single quotes around them for clarity, but no quotes should be used when executing them in-game.");
            sb.AppendLine();

            sb.AppendLine("***Moving***");
            sb.AppendLine("  'MoveX' (without quotes) will move you right or left.  You can specify a specific number of tiles, positive or negative.  Positive numbers move you to the right, negative to the left.");
            sb.AppendLine("  Examples: 'MoveX 10', 'MoveX -12'");
            sb.AppendLine("  'MoveY' works the same as 'MoveX', except it moves you up or down.");
            sb.AppendLine();

            sb.AppendLine("***Drawing***");
            sb.AppendLine("  'Draw' will re-draw the current level.");

            Console.WriteLine(sb);
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
            _ovMap.DiscoverTilesAroundPlayer();
            _ovMap.DrawLevelDirect(_ovMap.ThePlayer.DungeonLevel, true);
        }
    }
}
