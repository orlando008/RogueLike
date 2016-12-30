using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shadows
{
    public class Program
    {
        public OverallMap _ovMap;

        public delegate void StoryMessageEventHandler(StoryMessageEventArgs e);

        public event EventHandler MapCreated;
        public event EventHandler PlacePlayer;
        public event StoryMessageEventHandler StoryMessage;


        public Program()
        {
        }

        protected void OnMapCreated(EventArgs e)
        {
            MapCreated?.Invoke(null, e);
        }

        protected void OnPlacePlayer(EventArgs e)
        {
            PlacePlayer?.Invoke(null, e);
        }

        public void OnStoryMessage(StoryMessageEventArgs e)
        {
            StoryMessage?.Invoke(e);
        }

        public class StoryMessageEventArgs : EventArgs
        {
            public string StoryMessage;
        }

        public void ProcessUserCommand(string userInputText)
        {
            if (userInputText == null)
                return;

            string userInput = userInputText.Trim();

            string[] userInputArray = userInput.Split(" ".ToCharArray());

            switch (userInputArray[0].ToUpper())
            {
                case "MOVERIGHT":
                    MoveCommand(1, 0, userInputArray);
                    break;
                case "MOVELEFT":
                    MoveCommand(-1, 0, userInputArray);
                    break;
                case "MOVEUP":
                    MoveCommand(0, -1, userInputArray);
                    break;
                case "MOVEDOWN":
                    MoveCommand(0, 1, userInputArray);
                    break;

                default:
                    break;
            }
        }

        public void StartNewGame(CommonEnumerations.BaseClassTypes bct)
        {
            int seed = 0;

            _ovMap = new OverallMap(seed);
            OnMapCreated(null);

            _ovMap.CreateLevel(bct);
            _ovMap.DiscoverTilesAroundPlayer();

            OnPlacePlayer(null);

            StoryMessageEventArgs smea = new StoryMessageEventArgs();
            smea.StoryMessage = "You wake up in dressed in rags in a dimly lit hallway.  How did you get here?";

            OnStoryMessage(smea);
        }

        public void MoveCommand(int xDirection, int yDirection, string[] userInputArray)
        {
            if (_ovMap.CombatResolved == false)
            {
                return;
            }
                
            int lengthOfMove = 1;
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
                    else
                    {
                        _ovMap.OnNothingEncountered(null);
                    }
                }
            }

            _ovMap.DiscoverTilesAroundPlayer();
        }
    }
}
