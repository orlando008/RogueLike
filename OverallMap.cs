using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using RogueLike.StructuralClasses;

namespace RogueLike
{
    public class OverallMap
    {
        public List<Room> _rooms;
        private Random _rng = null;
        private Dictionary<int, List<Room>> _levels;
        private Dictionary<int, List<Point>> _levelHallways;
        private Player _player;

        public Random RNG
        {
            get
            {
                if (_rng == null)
                    _rng = new Random(DateTime.Now.Millisecond);

                return _rng;
            }
        }

        public Player ThePlayer
        {
            get
            {
                return _player;
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

        public Dictionary<int, List<Point>> LevelHallways
        {
            get
            {
                if (_levelHallways == null)
                    _levelHallways = new Dictionary<int, List<Point>>();

                return _levelHallways;
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
                if (room.Origin.Y + room.Size.Y > maxY)
                {
                    maxY = room.Origin.Y + room.Size.Y;
                }

                if (room.Origin.X + room.Size.X > maxX)
                {
                    maxX = room.Origin.X + room.Size.X;
                }
            }

            maxY += 5;
            maxX += 5;

            for (int i = 0; i < maxY; i++)
            {
                for (int j = 0; j < maxX; j++)
                {
                    Point tmpPoint = new Point(j, i);

                    if(_player != null && (tmpPoint.Equals(_player.Location)))
                    {
                        s += "@";
                    }
                    else
                    {
                        string tmp = "";

                        foreach (Room room in _levels[level])
                        {
                            tmp = room.GetStringAtPoint(tmpPoint);

                            if (tmp != "")
                                break;
                        }

                        if (tmp != "")
                        {
                            s += tmp;
                        }
                        else
                        {

                            int p = LevelHallways[level].FindIndex(x => x.X == tmpPoint.X && x.Y == tmpPoint.Y);

                            if (p != -1)
                                s += "#";
                            else
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

            
            Levels.Add(Levels.Count, rooms);
            ConnectTwoDoorways(Levels.Count -1);

            PlacePlayerOnMap();
        }

        public Point FindValidOriginForRoom(List<Room> rooms, Room proposedRoom)
        {
            bool placed = false;
            int tries = 0;

            while (!placed || tries > 2500)
            {
                int x = RNG.Next(0, 100);
                int y = RNG.Next(0, 100);

                Rectangle roomRectangle = new Rectangle(x, y, proposedRoom.Size.X + 1, proposedRoom.Size.Y + 1);

                bool intersectionFound = false;
                foreach (Room r in rooms)
                {
                    Rectangle tmpRect = new Rectangle(r.Origin.X, r.Origin.Y, r.Size.X, r.Size.Y);

                    if (tmpRect.IntersectsWith(roomRectangle))
                    {
                        intersectionFound = true;
                        break;
                    }
                }

                if (!intersectionFound)
                {
                    placed = true;
                    return new Point(x, y);
                }
            }

            return new Point(-1, -1);
        }

        public void PlacePlayerOnMap()
        {
            _player = new Player(this);
        }

        public void ConnectTwoDoorways(int level)
        {
            Room room1 = null;
            Point room1DoorWay = new Point(0,0);
            Point room1DoorWayGlobal = new Point(0, 0);
            Room room2 = null;
            Point room2DoorWay = new Point(0, 0);
            Point room2DoorWayGlobal = new Point(0, 0);
            
            foreach (Room r in _levels[level])
            {
                //For each potential vertical wall door
                for (int i = 0; i < r.RoomLayout.GetLength(0); i++)
                {
                    if (r.RoomLayout[i, 0].ThisTileType == TileType.Door && r.RoomLayout[i, 0].Connected == false)
                    {
                        if (room1 == null)
                        {
                            room1 = r;
                            room1DoorWay = new Point(i, 0);
                        }
                        else if (!room1.Equals(r))
                        {
                            room2 = r;
                            room2DoorWay = new Point(i, 0);
                        }

                        break;
                    }
                    else if (r.RoomLayout[i, r.RoomLayout.GetLength(1) -1].ThisTileType == TileType.Door && r.RoomLayout[i, r.RoomLayout.GetLength(1) -1].Connected == false)
                    {
                        if (room1 == null)
                        {
                            room1 = r;
                            room1DoorWay = new Point(i, r.RoomLayout.GetLength(1) - 1);
                        }
                        else if (!room1.Equals(r))
                        {
                            room2 = r;
                            room2DoorWay = new Point(i, r.RoomLayout.GetLength(1) - 1);
                        }

                        break;
                    }
                }

                //For each potential horizontal wall door
                for (int i = 0; i < r.RoomLayout.GetLength(1); i++)
                {
                    if (r.RoomLayout[0, i].ThisTileType == TileType.Door && r.RoomLayout[0, i].Connected == false)
                    {
                        if (room1 == null)
                        {
                            room1 = r;
                            room1DoorWay = new Point(0, i);
                        }
                        else if(!room1.Equals(r))
                        {
                            room2 = r;
                            room2DoorWay = new Point(0, i);
                        }

                        break;
                    }
                    else if (r.RoomLayout[r.RoomLayout.GetLength(0) - 1, i].ThisTileType == TileType.Door && r.RoomLayout[r.RoomLayout.GetLength(0) - 1, i].Connected == false)
                    {
                        if (room1 == null)
                        {
                            room1 = r;
                            room1DoorWay = new Point(i, r.RoomLayout.GetLength(1) - 1);
                        }
                        else if (!room1.Equals(r))
                        {
                            room2 = r;
                            room2DoorWay = new Point(i, r.RoomLayout.GetLength(1) - 1);
                        }

                        break;
                    }
                }
            }

            room1DoorWayGlobal = new Point(room1DoorWay.X + room1.Origin.X, room1DoorWay.Y + room1.Origin.Y);
            room2DoorWayGlobal = new Point(room2DoorWay.X + room2.Origin.X, room2DoorWay.Y + room2.Origin.Y);

            List<Point> _pointsBetweenDoors = new List<Point>();


            while (room1.RoomLayout[room1DoorWay.X, room1DoorWay.Y].Connected == false)
            {
                Point pointToTry;

                int ultimateXDirection = room2DoorWayGlobal.X - room1DoorWayGlobal.X;
                int ultimateYDirection = room2DoorWayGlobal.Y - room1DoorWayGlobal.Y;

                if (_pointsBetweenDoors.Count > 0)
                {
                    ultimateXDirection = room2DoorWayGlobal.X - _pointsBetweenDoors[_pointsBetweenDoors.Count - 1].X;
                    ultimateYDirection = room2DoorWayGlobal.Y - _pointsBetweenDoors[_pointsBetweenDoors.Count - 1].Y;
                }

                if (ultimateXDirection < 0)
                {
                    ultimateXDirection = -1;
                }
                else if (ultimateXDirection > 0)
                {
                    ultimateXDirection = 1;
                }

                if (ultimateYDirection < 0)
                {
                    ultimateYDirection = -1;
                }
                else if (ultimateYDirection > 0)
                {
                    ultimateYDirection = 1;
                }

                if (_pointsBetweenDoors.Count == 0)
                {
                    pointToTry = new Point(room1DoorWayGlobal.X + ultimateXDirection, room1DoorWayGlobal.Y);
                }
                else
                {
                    if (ultimateXDirection != 0)
                    {
                        pointToTry = new Point(_pointsBetweenDoors[_pointsBetweenDoors.Count - 1].X + ultimateXDirection, _pointsBetweenDoors[_pointsBetweenDoors.Count - 1].Y);
                    }
                    else if (ultimateYDirection != 0)
                    {
                        pointToTry = new Point(_pointsBetweenDoors[_pointsBetweenDoors.Count - 1].X, _pointsBetweenDoors[_pointsBetweenDoors.Count - 1].Y + ultimateYDirection);
                    }
                    else
                    {
                        //break out of the while
                        room1.RoomLayout[room1DoorWay.X, room1DoorWay.Y].Connected = true;
                        room2.RoomLayout[room2DoorWay.X, room2DoorWay.Y].Connected = true;
                        break;
                    }

                    
                }

                

                if (!PointIsClear(pointToTry, level))
                {
                    pointToTry = new Point(room1DoorWayGlobal.X, room1DoorWayGlobal.Y + ultimateYDirection);

                    if (!PointIsClear(pointToTry, level))
                    {
                        pointToTry = new Point(room1DoorWayGlobal.X - ultimateXDirection, room1DoorWayGlobal.Y);

                        if (!PointIsClear(pointToTry, level))
                        {
                            pointToTry = new Point(room1DoorWayGlobal.X, room1DoorWayGlobal.Y - ultimateYDirection);

                            if (!PointIsClear(pointToTry, level))
                            {
                                Console.WriteLine("Could not connect doorways");
                            }
                            else
                            {
                                _pointsBetweenDoors.Add(new Point(pointToTry.X, pointToTry.Y));
                            }
                        }
                        else
                        {
                            _pointsBetweenDoors.Add(new Point(pointToTry.X, pointToTry.Y));
                        }
                    }
                    else
                    {
                        _pointsBetweenDoors.Add(new Point(pointToTry.X, pointToTry.Y));
                    }
                }
                else
                {
                    _pointsBetweenDoors.Add(new Point(pointToTry.X, pointToTry.Y));
                }

                if ((pointToTry.Y + 1 == room2DoorWayGlobal.X && pointToTry.X == room2DoorWayGlobal.Y) ||
                    (pointToTry.Y - 1 == room2DoorWayGlobal.X && pointToTry.X == room2DoorWayGlobal.Y) ||
                    (pointToTry.Y == room2DoorWayGlobal.X && pointToTry.X + 1 == room2DoorWayGlobal.Y) ||
                    (pointToTry.Y == room2DoorWayGlobal.X && pointToTry.X - 1 == room2DoorWayGlobal.Y) )
                {
                    room1.RoomLayout[room1DoorWay.X, room1DoorWay.Y].Connected = true;
                    room2.RoomLayout[room2DoorWay.X, room2DoorWay.Y].Connected = true;
                }

                LevelHallways.Add(level, _pointsBetweenDoors);
                Console.WriteLine(GetDrawingOfLevel(0));
                LevelHallways.Remove(0);
                
            }

            
            LevelHallways.Add(level,_pointsBetweenDoors);
        }

        public bool PointIsClear(Point pointToTry, int level)
        {
            if (pointToTry.X < 0 || pointToTry.X > 250 || pointToTry.Y < 0 || pointToTry.Y > 250)
                return false;

            foreach (Room r in _levels[level])
            {
                if (r.GetRoomTileAtPoint(pointToTry) != null)
                    return false;
            }

            return true;
        }
    }
}
