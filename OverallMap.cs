using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueLike
{
    public class OverallMap
    {
        public List<Room> _rooms;
        private Random _rng = null;
        private int _lastLevelCreated = 0;
        private Dictionary<int, List<Room>> _levels;

        public Random RNG
        {
            get
            {
                if (_rng == null)
                    _rng = new Random(DateTime.Now.Millisecond);

                return _rng;
            }
        }

        public OverallMap()
        {
        }

        public override string ToString()
        {
            return _levels[0][0].ToString();
        }

        public void CreateLevel()
        {
            if (_levels == null)
                _levels = new Dictionary<int, List<Room>>();

            List<Room> rooms = new List<Room>();
            rooms.Add(new Room(this, RoomType.Hallway));

            _levels.Add(_lastLevelCreated, rooms);
        }
    }
}
