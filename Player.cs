using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RogueLike
{
    public class Player
    {
        Point _location;
        int _dungeonLevel;
        int _playerLevel;
        int _playerExperience;
        OverallMap _overallMap;
        int _visionRadius = 1;

        public Player(OverallMap ovMap)
        {
            int hallway = ovMap.RNG.Next(0, ovMap.LevelHallways[0].Count - 1);

            _location = ovMap.LevelHallways[0][hallway];
            _dungeonLevel = 0;
            _playerLevel = 1;
            _playerExperience = 0;
            _overallMap = ovMap;
        }

        public Point Location
        {
            get
            {
                return _location;
            }
        }

        public int DungeonLevel
        {
            get
            {
                return _dungeonLevel;
            }
        }

        public int PlayerLevel
        {
            get
            {
                return _playerLevel;
            }
        }

        public int VisionRadius
        {
            get
            {
                return _visionRadius;
            }
        }

        public int PlayerExperience
        {
            get
            {
                return _playerExperience;
            }
        }

        public bool MovePlayer(int xDirection, int yDirection)
        {
            _location = new Point(_location.X + xDirection, _location.Y + yDirection);

            return true;
        }

        public string LocationString()
        {
            return "Currently in dungeon level " + _dungeonLevel.ToString() + " at (" + _location.X.ToString() + "," + _location.Y.ToString() + ")";
        }
    }
}
