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

        public Room(OverallMap parentMap, RoomType roomType)
        {
            _parentMap = parentMap;
            _roomType = roomType;

            switch (roomType)
            {
                case RoomType.Room:
                    int widthOfRoom = _parentMap.RNG.Next(5, 25);
                    int heightOfRoom = _parentMap.RNG.Next(5, 25);

                    _roomLayout = new RoomTile[heightOfRoom, widthOfRoom];

                    break;
                case RoomType.Hallway:
                    int lengthOfHallway = _parentMap.RNG.Next(1, 10);

                    if (_parentMap.RNG.Next(1, 2) == 1)
                    {
                        _roomLayout = new RoomTile[lengthOfHallway, 1];
                    }
                    else
                    {
                        _roomLayout = new RoomTile[1, lengthOfHallway];
                    }

                    FillInRoomLayoutForHallway();
                    
                    break;
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

                    if (j == 0 || j + 1 == _roomLayout.GetLength(1))
                    {
                        _roomLayout[i, j] = new RoomTile(TileType.VerticalWall);
                    }
                    
                }
            }
        }

        public override string ToString()
        {
            string renderString = "";

            for (int i = 0; i < _roomLayout.GetLength(0); i++)
            {
                for (int j = 0; j < _roomLayout.GetLength(1); j++)
                {
                    renderString += _roomLayout[i, j].ToString();
                }

                if(i < _roomLayout.GetLength(0) -1)
                    renderString += "\n";
            }

            return renderString;
        }
    }
}
