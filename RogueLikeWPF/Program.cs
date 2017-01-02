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
            public System.Windows.Media.Color messageColor;

            public StoryMessageEventArgs()
            {

            }

            public StoryMessageEventArgs(string storyMessage, System.Windows.Media.Color msgColor)
            {
                this.StoryMessage = storyMessage;
                this.messageColor = msgColor;
            }
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
                    MoveCommand(1, 0);
                    break;
                case "MOVELEFT":
                    MoveCommand(-1, 0);
                    break;
                case "MOVEUP":
                    MoveCommand(0, -1);
                    break;
                case "MOVEDOWN":
                    MoveCommand(0, 1);
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
            smea.StoryMessage = "You've arrived at the Ruins of the Shadow seeking riches and glory.";
            smea.messageColor = System.Windows.Media.Colors.White;
            OnStoryMessage(smea);
            smea.StoryMessage = "As you enter the underground catacombs the door seals itself behind you...";    
            OnStoryMessage(smea);
            smea.StoryMessage = "You briefly doubt yourself... was this really worth it?";
            OnStoryMessage(smea);
        }

        public void MoveCommand(int xDirection, int yDirection)
        {
            if (_ovMap.CombatResolved == false)
            {
                return;
            }

            if (_ovMap.ThePlayer.MovePlayerNonCombat(xDirection, yDirection))
            {
                _ovMap.DiscoverTilesAroundPlayer();
            }
        }
    }
}
