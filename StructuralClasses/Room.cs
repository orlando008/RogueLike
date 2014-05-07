using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RogueLike.StructuralClasses;
using System.Drawing;

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
        private Point _origin;
        private Point _size;
        private Dictionary<Point, string> _roomStrings;

        public Room(OverallMap parentMap, RoomType roomType)
        {
            _parentMap = parentMap;
            _roomType = roomType;
            _origin = new Point(-1, -1);

            switch (roomType)
            {
                case RoomType.Room:
                    int widthOfRoom = _parentMap.RNG.Next(5, 25);
                    int heightOfRoom = _parentMap.RNG.Next(5, 25);

                    _size = new Point(widthOfRoom, heightOfRoom);

                    _roomLayout = new RoomTile[heightOfRoom, widthOfRoom];

                    FillInRoomLayoutForRoom();
                    break;
                case RoomType.Hallway:
                    int lengthOfHallway = _parentMap.RNG.Next(1, 10);

                    if (_parentMap.RNG.Next(1, 2) == 1)
                    {
                        _roomLayout = new RoomTile[lengthOfHallway, 1];

                        _size = new Point(lengthOfHallway, 1);
                    }
                    else
                    {
                        _roomLayout = new RoomTile[1, lengthOfHallway];
                        _size = new Point(1, lengthOfHallway);
                    }

                    FillInRoomLayoutForHallway();
                    
                    break;
            }
        }

        public Point Size
        {
            get
            {
                return _size;
            }
        }

        public Point Origin
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

        public RoomTile[,] RoomLayout
        {
            get
            {
                return _roomLayout;
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
            _roomStrings = new Dictionary<Point, string>();
            for (int i = 0; i < _roomLayout.GetLength(0); i++)
            {
                for (int j = 0; j < _roomLayout.GetLength(1); j++)
                {
                    Point tmpPoint = new Point(Origin.X + j, Origin.Y + i);
                    _roomStrings.Add(tmpPoint, _roomLayout[i, j].ToString());
                }
            }
        }

        public string GetStringAtPoint(Point i)
        {
            if (_roomStrings.ContainsKey(i))
            {
                return _roomLayout[i.Y - Origin.Y, i.X - Origin.X].ToString();
            }
            else
            {
                return "";
            }
        }

        public RoomTile GetRoomTileAtPoint(Point i)
        {
            if (_roomStrings.ContainsKey(i))
            {
                return _roomLayout[i.Y - Origin.Y, i.X - Origin.X];
            }
            else
            {
                return null;
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
                tileToPlaceDoor = _parentMap.RNG.Next(1, this.Size.Y-1);
                _roomLayout[tileToPlaceDoor, 0].ThisTileType = TileType.Door;
            }

            if (eastWallHasDoor)
            {
                tileToPlaceDoor = _parentMap.RNG.Next(1, this.Size.Y-1);
                _roomLayout[tileToPlaceDoor, this.Size.X - 1].ThisTileType = TileType.Door;
            }

            if (northWallHasDoor)
            {
                tileToPlaceDoor = _parentMap.RNG.Next(1, this.Size.X-1);
                _roomLayout[0, tileToPlaceDoor].ThisTileType = TileType.Door;
            }

            if (southWallHasDoor)
            {
                tileToPlaceDoor = _parentMap.RNG.Next(1, this.Size.X-1);
                _roomLayout[this.Size.Y - 1, tileToPlaceDoor].ThisTileType = TileType.Door;
            }
        }

        public void GetUnconnectedDoorway()
        {

        }
    }
}
