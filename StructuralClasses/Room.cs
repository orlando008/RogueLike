using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RogueLike.StructuralClasses;

namespace RogueLike
{
    public enum RoomType
    {
        Hallway = 0,
        Room = 1
    }

    public class Room
    {
        private RoomType _roomType;
        private RoomTile[,] _roomLayout = null;
        private OverallMap _parentMap;
        private Tuple<int, int> _origin;
        private Tuple<int, int> _size;
        private Dictionary<Tuple<int, int>, string> _roomStrings;

        public Room(OverallMap parentMap, RoomType roomType)
        {
            _parentMap = parentMap;
            _roomType = roomType;
            _origin = Tuple.Create<int, int>(-1, -1);

            switch (roomType)
            {
                case RoomType.Room:
                    int widthOfRoom = _parentMap.RNG.Next(5, 25);
                    int heightOfRoom = _parentMap.RNG.Next(5, 25);

                    _size = Tuple.Create<int, int>(widthOfRoom, heightOfRoom);

                    _roomLayout = new RoomTile[heightOfRoom, widthOfRoom];

                    FillInRoomLayoutForRoom();
                    break;
                case RoomType.Hallway:
                    int lengthOfHallway = _parentMap.RNG.Next(1, 10);

                    if (_parentMap.RNG.Next(1, 2) == 1)
                    {
                        _roomLayout = new RoomTile[lengthOfHallway, 1];

                        _size = Tuple.Create<int, int>(lengthOfHallway, 1);
                    }
                    else
                    {
                        _roomLayout = new RoomTile[1, lengthOfHallway];
                        _size = Tuple.Create<int, int>(1, lengthOfHallway);
                    }

                    FillInRoomLayoutForHallway();
                    
                    break;
            }
        }

        public Tuple<int, int> Size
        {
            get
            {
                return _size;
            }
        }

        public Tuple<int, int> Origin
        {
            get
            {
                return _origin;
            }
            set
            {
                _origin = value;
                FitIntoStringDictionary();
            }
        }

        private void FillInRoomLayoutForHallway()
        {
            for (int i = 0; i < _roomLayout.GetLength(0); i++)
            {
                for (int j = 0; j < _roomLayout.GetLength(1); j++)
                {
                    _roomLayout[i, j] = new RoomTile(TileType.HallwayFloor);
                }
            }
        }

        private void FillInRoomLayoutForRoom()
        {
            for (int i = 0; i < _roomLayout.GetLength(0); i++)
            {
                for (int j = 0; j < _roomLayout.GetLength(1); j++)
                {
                    if (i == 0 || i + 1 == _roomLayout.GetLength(0))
                    {
                        _roomLayout[i, j] = new RoomTile(TileType.HorizontalWall);
                    }
                    else if (j == 0 || j + 1 == _roomLayout.GetLength(1))
                    {
                        _roomLayout[i, j] = new RoomTile(TileType.VerticalWall);
                    }
                    else
                    {
                        _roomLayout[i, j] = new RoomTile(TileType.Floor);
                    }
                    
                }
            }
        }

        public void FitIntoStringDictionary()
        {
            _roomStrings = new Dictionary<Tuple<int, int>, string>();
            for (int i = 0; i < _roomLayout.GetLength(0); i++)
            {
                for (int j = 0; j < _roomLayout.GetLength(1); j++)
                {
                    Tuple<int, int> tmpTuple = Tuple.Create<int, int>(Origin.Item1 + j, Origin.Item2 + i);
                    _roomStrings.Add(tmpTuple, _roomLayout[i, j].ToString());
                }
            }
        }

        public string GetStringAtLevel(Tuple<int,int> i)
        {
            if (_roomStrings.ContainsKey(i))
            {
                return _roomLayout[i.Item2 - Origin.Item2, i.Item1 - Origin.Item1].ToString();
            }
            else
            {
                return "";
            }
        }

        public void AddDoorwayToRoom()
        {
            int numberOfDoors = _parentMap.RNG.Next(1, 4);

            bool westWallHasDoor = false;
            bool eastWallHasDoor = false;
            bool northWallHasDoor = false;
            bool southWallHasDoor = false;

            while (!westWallHasDoor && !eastWallHasDoor && !northWallHasDoor && !southWallHasDoor)
            {
                if (_parentMap.RNG.Next(1, 3) == 2)
                {
                    westWallHasDoor = true;
                }

                if (_parentMap.RNG.Next(1, 3) == 2)
                {
                    eastWallHasDoor = true;
                }

                if (_parentMap.RNG.Next(1, 3) == 2)
                {
                    northWallHasDoor = true;
                }

                if (_parentMap.RNG.Next(1, 3) == 2)
                {
                    southWallHasDoor = true;
                }
            }


            int tileToPlaceDoor;
            
            if (westWallHasDoor)
            {
                tileToPlaceDoor = _parentMap.RNG.Next(1, this.Size.Item2-1);
                _roomLayout[tileToPlaceDoor, 0].ThisTileType = TileType.Door;
            }

            if (eastWallHasDoor)
            {
                tileToPlaceDoor = _parentMap.RNG.Next(1, this.Size.Item2-1);
                _roomLayout[tileToPlaceDoor, this.Size.Item1 - 1].ThisTileType = TileType.Door;
            }

            if (northWallHasDoor)
            {
                tileToPlaceDoor = _parentMap.RNG.Next(1, this.Size.Item1-1);
                _roomLayout[0, tileToPlaceDoor].ThisTileType = TileType.Door;
            }

            if (southWallHasDoor)
            {
                tileToPlaceDoor = _parentMap.RNG.Next(1, this.Size.Item1-1);
                _roomLayout[this.Size.Item2 - 1, tileToPlaceDoor].ThisTileType = TileType.Door;
            }
        }
    }
}
