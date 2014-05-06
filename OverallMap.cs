using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RogueLike
{
    public class OverallMap
    {
        public List<Room> _rooms;
        private Random _rng = null;
        private int _lastLevelCreated = 0;
        private Dictionary<int, List<Room>> _levels;
        private Tuple<int, int> _locationOfPlayer;

        public Random RNG
        {
            get
            {
                if (_rng == null)
                    _rng = new Random(DateTime.Now.Millisecond);

                return _rng;
            }
        }

        public Dictionary<int, List<Room>> Levels
        {
            get
            {
                if (_levels == null)
                    _levels = new Dictionary<int, List<Room>>();

                return _levels;
            }
        }

        public OverallMap()
        {
        }

        public string GetDrawingOfLevel(int level)
        {
            string s = "";

            int maxY = 0;
            int maxX = 0;

            foreach (Room room in _levels[level])
            {
                if (room.Origin.Item2 + room.Size.Item2 > maxY)
                {
                    maxY = room.Origin.Item2 + room.Size.Item2;
                }

                if (room.Origin.Item1 + room.Size.Item1 > maxX)
                {
                    maxX = room.Origin.Item1 + room.Size.Item1;
                }
            }

            maxY += 5;
            maxX += 5;

            for (int i = 0; i < maxY; i++)
            {
                for (int j = 0; j < maxX; j++)
                {
                    Tuple<int, int> tmpTuple = Tuple.Create<int, int>(j, i);

                    if (tmpTuple.Equals(_locationOfPlayer))
                    {
                        s += "@";
                    }
                    else
                    {
                        string tmp = "";

                        foreach (Room room in _levels[level])
                        {
                            tmp = room.GetStringAtLevel(tmpTuple);

                            if (tmp != "")
                                break;
                        }

                        if (tmp != "")
                        {
                            s += tmp;
                        }
                        else
                        {
                            s += " ";
                        }
                    }         
                }

                s += "\n";
                
            }

            return s;
        }

        public void CreateLevel()
        {
            int numberOfRooms = RNG.Next(3, 20);

            List<Room> rooms = new List<Room>();

            Room tmpRoom;
            for (int i = 0; i < numberOfRooms; i++)
            {
                tmpRoom = new Room(this, RoomType.Room);

                tmpRoom.Origin = FindValidOriginForRoom(rooms, tmpRoom);

                tmpRoom.AddDoorwayToRoom();
                rooms.Add(tmpRoom);
            }
            
            Levels.Add(_lastLevelCreated, rooms);

            PlacePlayerOnMap();
        }

        public Tuple<int, int> FindValidOriginForRoom(List<Room> rooms, Room proposedRoom)
        {
            bool placed = false;
            int tries = 0;

            while (!placed || tries > 2500)
            {
                int x = RNG.Next(0, 100);
                int y = RNG.Next(0, 100);

                Rectangle roomRectangle = new Rectangle(x, y, proposedRoom.Size.Item1 + 1, proposedRoom.Size.Item2 + 1);

                bool intersectionFound = false;
                foreach (Room r in rooms)
                {
                    Rectangle tmpRect = new Rectangle(r.Origin.Item1, r.Origin.Item2, r.Size.Item1, r.Size.Item2);

                    if (tmpRect.IntersectsWith(roomRectangle))
                    {
                        intersectionFound = true;
                        break;
                    }
                }

                if (!intersectionFound)
                {
                    placed = true;
                    return Tuple.Create<int, int>(x, y);
                }
            }

            return Tuple.Create<int, int>(0, 0);
        }

        public void PlacePlayerOnMap()
        {
            _locationOfPlayer = Tuple.Create<int, int>(0, 0);
        }
    }
}
