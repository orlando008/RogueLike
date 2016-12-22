using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueLike
{
    public class Program
    {
        private static bool _exit = false;
        public static OverallMap _ovMap;
        private static string RAN_INTO_WALL = "You ran into a wall.";
        private static string GAME_NAME = "Dungeon Adventure 2014";
        public static bool _consoleMode = true;

        public delegate void InputNeededEventArgsEventHandler(InputNeededEventArgs e);

        public static event InputNeededEventArgsEventHandler InputNeeded;
        public static event OverallMap.DrawPortionEventHandler DrawPortion;

        public static event EventHandler DrawBegin;
        public static event EventHandler DrawEnd;

        public static event EventHandler MapCreated;
        public static event EventHandler PlacePlayer;

        public class InputNeededEventArgs: EventArgs
        {
            public string InputText;
        }

        protected static void OnInputNeeded(InputNeededEventArgs e)
        {
            InputNeeded?.Invoke(e);
        }

        protected static void OnDrawBegin(EventArgs e)
        {
            DrawBegin?.Invoke(null, e);
        }

        protected static void OnMapCreated(EventArgs e)
        {
            MapCreated?.Invoke(null, e);
        }

        protected static void OnPlacePlayer(EventArgs e)
        {
            PlacePlayer?.Invoke(null, e);
        }

        protected static void OnDrawEnd(EventArgs e)
        {
            DrawEnd?.Invoke(null, e);
        }

        public static void Main(string[] args)
        {
            if (args.Length == 1)
                _consoleMode = false;

            if (_consoleMode)
            {
                Console.SetWindowSize(Console.LargestWindowWidth - 10, Console.LargestWindowHeight - 10);
                Console.Title = GAME_NAME;
            }

            StartNewGame();

            while (!_exit)
            {
                if(_consoleMode)
                {
                    Console.Write(_ovMap.ThePlayer.LocationString());
                    Console.Write(">");

                    ProcessUserCommand(Console.ReadLine());
                }
                else
                {
                    _exit = true;
                }

            }

        }

        private static void _ovMap_DrawPortion(OverallMap.DrawPortionEventArgs e)
        {
            DrawPortion?.Invoke(e);
        }

        public static void ProcessUserCommand(string userInputText)
        {
            if (userInputText == null)
                return;

            string userInput = userInputText.Trim();

            string[] userInputArray = userInput.Split(" ".ToCharArray());
            bool reDraw = false;

            switch (userInputArray[0].ToUpper())
            {
                case "MOVERIGHT":
                    if(_consoleMode)
                        Console.WriteLine("Moving right...");
                    MoveCommand(1, 0, userInputArray, out reDraw);
                    break;
                case "MOVELEFT":
                    if (_consoleMode)
                        Console.WriteLine("Moving left...");
                    MoveCommand(-1, 0, userInputArray, out reDraw);
                    break;
                case "MOVEUP":
                    if (_consoleMode)
                        Console.WriteLine("Moving up...");
                    MoveCommand(0, -1, userInputArray, out reDraw);
                    break;
                case "MOVEDOWN":
                    if (_consoleMode)
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
                    if (_consoleMode)
                        Console.Clear();
                    _ovMap.DrawLevelDirect(_ovMap.ThePlayer.DungeonLevel, true);
                    break;
                case "SEED":
                    if (_consoleMode)
                        Console.WriteLine(_ovMap.Seed.ToString());
                    break;
                case "NEWGAME":
                    StartNewGame();
                    break;
#if DEBUG
                case "DRAWALL":
                    if (_consoleMode)
                        Console.Clear();
                    _ovMap.DrawLevelDirect(_ovMap.ThePlayer.DungeonLevel, false);
                    break;
#endif
                case "STATS":
                    if (_consoleMode)
                        _ovMap.ThePlayer.DrawStats();
                    break;
                case "INVENTORY":
                    if (_consoleMode)
                        _ovMap.ThePlayer.DrawInventory();
                    break;
                default:
                    if (_consoleMode)
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
            if(_consoleMode)
                Console.Clear();

            int seed = 0;
#if DEBUG
            string seedString = "test";
            if (_consoleMode)
            {
                Console.Write("Seed?");
                seedString = Console.ReadLine();
            }

            if (Int32.TryParse(seedString, out seed) == false)
                seed = 0;
            else
                seed = Convert.ToInt32(seedString);
#endif

            if (_consoleMode)
                Console.Clear();

            _ovMap = new OverallMap(seed);
            _ovMap.DrawPortion += _ovMap_DrawPortion;
            _ovMap.DrawBegin += _ovMap_DrawBegin;
            _ovMap.DrawEnd += _ovMap_DrawEnd;
            OnMapCreated(null);

            _ovMap.CreateLevel();
            _ovMap.DiscoverTilesAroundPlayer();
            _ovMap.DrawLevelDirect(_ovMap.ThePlayer.DungeonLevel, true);

            OnPlacePlayer(null);
        }

        private static void _ovMap_DrawEnd(object sender, EventArgs e)
        {
            OnDrawEnd(e);
        }

        private static void _ovMap_DrawBegin(object sender, EventArgs e)
        {
            OnDrawBegin(e);
        }

        public static void MoveCommand(int xDirection, int yDirection, string[] userInputArray, out bool reDraw)
        {
            int lengthOfMove = 1;
            reDraw = false;
            bool encounteredEnemy = false;

            if (userInputArray.Length > 1)
                lengthOfMove = Convert.ToInt32(userInputArray[1]);

            for (int i = 0; i < lengthOfMove; i++)
            {
                if (_ovMap.ThePlayer.MovePlayer(xDirection, yDirection, out encounteredEnemy))
                {
                    _ovMap.DiscoverTilesAroundPlayer();

                    if (encounteredEnemy)
                    {
                        _ovMap.GenerateRandomEnemyEncounter();
                        break;
                    }
                }
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
