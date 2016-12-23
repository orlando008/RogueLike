using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueLike.StructuralClasses
{
    public enum TileType
    {
        Floor = 0,
        HorizontalWall = 1,
        VerticalWall = 2,
        Door = 3,
        HallwayFloor = 4
    }

    public class RoomTile
    {
        TileType _tileType;
        bool _connected = false;
        bool _discovered = false;
        Room _parentRoom = null;
        int _x;
        int _y;

        public RoomTile(Room parentRoom, int x, int y, TileType tyleType)
        {
            _x = x;
            _y = y;
            _tileType = tyleType;
            _parentRoom = parentRoom;
        }

        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        public Room ParentRoom
        {
            get
            {
                return _parentRoom;
            }
            set
            {
                _parentRoom = value;
            }
        }

        public TileType ThisTileType
        {
            get
            {
                return _tileType;
            }
            set
            {
                _tileType = value;
            }
        }

        public bool Connected
        {
            get
            {
                return _connected;
            }
            set
            {
                _connected = value;
            }
        }

        public bool Discovered
        {
            get
            {
                return _discovered;
            }
            set
            {
                _discovered = value;
                if (value)
                {
                    OverallMap.RoomDiscoveredEventArgs e = new OverallMap.RoomDiscoveredEventArgs();
                    e.roomTileThatWasDiscovered = this;
                    ParentRoom.ParentMap.OnRoomDiscovered(e);
                }
                    
            }
        }

        public override string ToString()
        {
            switch (_tileType)
            {
                case TileType.Floor:
                    return ".";
                case TileType.Door:
                    return "+";
                case TileType.HorizontalWall:
                    return "-";
                case TileType.VerticalWall:
                    return "|";
                case TileType.HallwayFloor:
                    return "#";
                default:
                    return " ";
            }
        }
    }
}
