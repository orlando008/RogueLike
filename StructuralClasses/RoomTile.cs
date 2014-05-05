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

        public RoomTile(TileType tyleType)
        {
            _tileType = tyleType;
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
