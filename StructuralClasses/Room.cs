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
        private RoomTile[,] _roomLayout = null;
        private OverallMap _parentMap;
        private Point _origin;
        private Point _size;
        private List<Point> _roomStrings;
        private const int MIN_ROOM_WIDTH = 5;
        private const int MAX_ROOM_WIDTH = 15;

        public Room(OverallMap parentMap)
        {
            _parentMap = parentMap;
            _origin = new Point(-1, -1);

            int widthOfRoom = _parentMap.RNG.Next(MIN_ROOM_WIDTH, MAX_ROOM_WIDTH);
            int heightOfRoom = _parentMap.RNG.Next(MIN_ROOM_WIDTH, MAX_ROOM_WIDTH);

            _size = new Point(widthOfRoom, heightOfRoom);

            _roomLayout = new RoomTile[_size.X, _size.Y];

            FillInRoomLayoutForRoom();
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
            for (int x = 0; x < _roomLayout.GetLength(0); x++)
            {
                for (int y = 0; y < _roomLayout.GetLength(1); y++)
                {
                    _roomLayout[x, y] = new RoomTile(TileType.HallwayFloor);
                }
            }
        }

        private void FillInRoomLayoutForRoom()
        {
            for (int x = 0; x < _roomLayout.GetLength(0); x++)
            {
                for (int y = 0; y < _roomLayout.GetLength(1); y++)
                {
                    if (x == 0 || x + 1 == _roomLayout.GetLength(0))
                    {
                        _roomLayout[x, y] = new RoomTile(TileType.VerticalWall);
                    }
                    else if (y == 0 || y + 1 == _roomLayout.GetLength(1))
                    {
                        _roomLayout[x, y] = new RoomTile(TileType.HorizontalWall);
                    }
                    else
                    {
                        _roomLayout[x, y] = new RoomTile(TileType.Floor);
                    }
                    
                }
            }
        }

        public void FitIntoStringDictionary()
        {
            _roomStrings = new List<Point>();
            for (int x = 0; x < _roomLayout.GetLength(0); x++)
            {
                for (int y = 0; y < _roomLayout.GetLength(1); y++)
                {
                    Point tmpPoint = new Point(Origin.X + x, Origin.Y + y);
                    _roomStrings.Add(tmpPoint);
                }
            }
        }

        public string GetStringAtPoint(Point thePoint, bool onlyDiscovered)
        {
            RoomTile rt = GetRoomTileAtPoint(thePoint);

            if (rt == null)
                return "";
            else if (rt.Discovered == false && onlyDiscovered)
                return "";
            else
                return rt.ToString();
        }

        public RoomTile GetRoomTileAtPoint(Point thePoint)
        {
            Point normalizedPoint = new Point(thePoint.X - Origin.X, thePoint.Y - Origin.Y);

            if (normalizedPoint.X < this.Size.X && normalizedPoint.Y < this.Size.Y && normalizedPoint.X >= 0 && normalizedPoint.Y >= 0)
            {
                return _roomLayout[normalizedPoint.X, normalizedPoint.Y];
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
                _roomLayout[0, tileToPlaceDoor].ThisTileType = TileType.Door;
            }

            if (eastWallHasDoor)
            {
                tileToPlaceDoor = _parentMap.RNG.Next(1, this.Size.Y-1);
                _roomLayout[this.Size.X - 1, tileToPlaceDoor].ThisTileType = TileType.Door;
            }

            if (northWallHasDoor)
            {
                tileToPlaceDoor = _parentMap.RNG.Next(1, this.Size.X-1);
                _roomLayout[tileToPlaceDoor, 0].ThisTileType = TileType.Door;
            }

            if (southWallHasDoor)
            {
                tileToPlaceDoor = _parentMap.RNG.Next(1, this.Size.X-1);
                _roomLayout[tileToPlaceDoor, this.Size.Y - 1].ThisTileType = TileType.Door;
            }
        }

        public void GetUnconnectedDoorway()
        {

        }
    }
}
