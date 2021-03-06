﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shadows.StructuralClasses;
using System.Drawing;

namespace Shadows
{
    public class Room
    {
        private RoomTile[,] _roomLayout = null;
        private OverallMap _parentMap;
        private Point _origin;
        private Point _size;
        private const int MIN_ROOM_WIDTH = 5;
        private const int MAX_ROOM_WIDTH = 10;

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

        public OverallMap ParentMap
        {
            get
            {
                return _parentMap;
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
            }
        }

        public RoomTile[,] RoomLayout
        {
            get
            {
                return _roomLayout;
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
                        _roomLayout[x, y] = new RoomTile(this, x, y, TileType.VerticalWall);
                    }
                    else if (y == 0 || y + 1 == _roomLayout.GetLength(1))
                    {
                        _roomLayout[x, y] = new RoomTile(this, x, y,TileType.HorizontalWall);
                    }
                    else
                    {
                        _roomLayout[x, y] = new RoomTile(this, x, y, TileType.Floor);
                    }
                    
                }
            }
        }

        public void DiscoverAllTilesInRoom()
        {
            for (int x = 0; x < _roomLayout.GetLength(0); x++)
            {
                for (int y = 0; y < _roomLayout.GetLength(1); y++)
                {
                    _roomLayout[x, y].Discovered = true;
                }
            }
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
            int numberOfDoors = _parentMap.RNG.Next(1, 7);

            bool westWallHasDoor = false;
            bool eastWallHasDoor = false;
            bool northWallHasDoor = false;
            bool southWallHasDoor = false;

            switch (numberOfDoors)
            {
                case 1:
                    westWallHasDoor = true;
                    eastWallHasDoor = true;
                    break;
                case 2:
                    northWallHasDoor = true;
                    southWallHasDoor = true;
                    break;
                case 3:
                    westWallHasDoor = true;
                    southWallHasDoor = true;
                    break;
                case 4:
                    eastWallHasDoor = true;
                    northWallHasDoor = true;
                    break;
                case 5:
                    westWallHasDoor = true;
                    northWallHasDoor = true;
                    break;
                case 6:
                    eastWallHasDoor = true;
                    southWallHasDoor = true;
                    break;
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
    }
}
