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
            bool reDraw = false;

            switch (userInputArray[0].ToUpper())
            {
                case "MOVERIGHT":
                    Console.WriteLine("Moving right...");
                    MoveCommand(1, 0, userInputArray, out reDraw);
                    break;
                case "MOVELEFT":
                    Console.WriteLine("Moving left...");
                    MoveCommand(-1, 0, userInputArray, out reDraw);
                    break;
                case "MOVEUP":
                    Console.WriteLine("Moving up...");
                    MoveCommand(0, -1, userInputArray, out reDraw);
                    break;
                case "MOVEDOWN":
                    Console.WriteLine("Moving down...");
                    MoveCommand(0, 1, userInputArray, out reDraw);
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
                case "STATS":
                    _ovMap.ThePlayer.DrawStats();
                    break;
                default:
                    Console.WriteLine("Command not recognized.");
                    break;
            }

            if(reDraw)
                _ovMap.DrawLevelDirect(_ovMap.ThePlayer.DungeonLevel, true);
        }

        public static void PrintHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(">----------------------------------------------------------------} " + GAME_NAME + " {----------------------------------------------------------------<");
            sb.AppendLine();

            sb.AppendLine("All commands are case insensitive.");
            sb.AppendLine("Commands are listed with brackets round them for clarity, but no brackets should be used when executing them in-game.");
            sb.AppendLine();

            sb.AppendLine("***Moving***");
            sb.AppendLine("  [MoveRight] - move right 1 tile.  Optional #1: specify a number of tiles to move. Optional #2: Set the 'd' flag to re-draw level immediately after moving.");
            sb.AppendLine("  Example: 'MoveRight 10'");
            sb.AppendLine("  [MoveLeft], [MoveUp], and [MoveDown] work the same way.");
            sb.AppendLine("  The player cannot move through walls, any extraneous move amounts are ignored.");
            sb.AppendLine();

            sb.AppendLine("***Drawing***");
            sb.AppendLine("  [Draw] - re-draw the current level.");
            sb.AppendLine();

            sb.AppendLine("***Player***");
            sb.AppendLine("  [Inventory] - display contents of player inventory.");
            sb.AppendLine("  [Stats] - display stats of player.");

            sb.AppendLine("***Menu Functions***");
            sb.AppendLine("  [NewGame] - start a new game.");
            sb.AppendLine("  [Exit] - exit the game.");

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

        public static void MoveCommand(int xDirection, int yDirection, string[] userInputArray, out bool reDraw)
        {
            int lengthOfMove = 1;
            reDraw = false;

            if (userInputArray.Length > 1)
                lengthOfMove = Convert.ToInt32(userInputArray[1]);

            for (int i = 0; i < lengthOfMove; i++)
            {
                if (_ovMap.ThePlayer.MovePlayer(xDirection, yDirection))
                    _ovMap.DiscoverTilesAroundPlayer();
                else
                {
                    Console.WriteLine(RAN_INTO_WALL);
                    break;
                }
            }

            if (userInputArray.Length > 2)
            {
                if (userInputArray[2].ToString().ToUpper() == "D")
                {
                    reDraw = true;
                }
            }

            _ovMap.DiscoverTilesAroundPlayer();
        }
    }
}
