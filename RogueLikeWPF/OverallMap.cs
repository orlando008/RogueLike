﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Shadows.StructuralClasses;
using Shadows.InteractableObjects;

namespace Shadows
{
    public class OverallMap
    {
        private Random _rng = null;
        private Dictionary<int, List<Room>> _levels;
        private Dictionary<int, List<Point>> _levelHallways;
        private Dictionary<int, List<Point>> _levelDiscoveredHallways;
        private Player _player;
        public int _seed = 0;
        private const int MAX_LVL_HEIGHT = 50;
        private const int MAX_LVL_WIDTH = 80;
        private const int MIN_NUMBER_OF_ROOMS = 3;
        private const int MAX_NUMBER_OF_ROOMS = 10;
        private bool _combatResolved = true;
        private CombatUnit _combatUnit;

        public delegate void RoomDiscoveredEventHandler(RoomDiscoveredEventArgs e);
        public delegate void HallDiscoveredEventHandler(HallDiscoveredEventArgs e);
        public delegate void CombatEncounteredEventHandler(CombatEncounteredEventArgs e);

        public event RoomDiscoveredEventHandler RoomDiscovered;
        public event HallDiscoveredEventHandler HallDiscovered;
        public event CombatEncounteredEventHandler CombatEncountered;
        public event EventHandler NothingEncountered;

        public class RoomDiscoveredEventArgs : EventArgs
        {
            public RoomTile roomTileThatWasDiscovered;
        }

        public class CombatEncounteredEventArgs : EventArgs
        {
            public CombatUnit combatUnit;
        }

        public class HallDiscoveredEventArgs : EventArgs
        {
            public Point hallThatWasDiscovered;
        }

        public void OnNothingEncountered(EventArgs e)
        {
            NothingEncountered?.Invoke(null, e);
        }

        public void OnRoomDiscovered(RoomDiscoveredEventArgs e)
        {
            RoomDiscovered?.Invoke(e);
        }

        protected void OnHallDiscovered(HallDiscoveredEventArgs e)
        {
            HallDiscovered?.Invoke(e);
        }

        protected void OnCombatEncountered(CombatEncounteredEventArgs e)
        {
            CombatEncountered?.Invoke(e);
        }


        public enum FirstSpecializations
        {
            Warrior = 0,
            Mage,
            Archer
        }

        public enum WarriorSpecializations
        {
            Duelist = 0,
            Paladin,
            Vanguard
        }

        public enum MageSpecializations
        {
            Summoner = 0,
            Mystic,
            Elementalist
        }

        public enum ArcherSpecializations
        {
            Trapper = 0,
            Hunter,
            Bowman
        }

        public Random RNG
        {
            get
            {
                if (_rng == null)
                {
                    _rng = new Random(Seed);
                }
                    

                return _rng;
            }
        }

        public bool CombatResolved
        {
            get
            {
                return _combatResolved;
            }
        }

        public CombatUnit CurrentCombatUnit
        {
            get
            {
                return _combatUnit;
            }
        }

        public int Seed
        {
            get
            {
                if (_seed == 0)
                {
                    _seed = DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
                }

                return _seed;
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

        public Dictionary<int, List<Point>> LevelDiscoveredHallways
        {
            get
            {
                if (_levelDiscoveredHallways == null)
                    _levelDiscoveredHallways = new Dictionary<int, List<Point>>();

                return _levelDiscoveredHallways;
            }
        }

        public OverallMap(int seed)
        {
            _seed = seed;
        }

        public void GetMax(out int maxX, out int maxY, int level)
        {
            maxX = 0;
            maxY = 0;

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
        }

        public void CreateLevel()
        {
            int numberOfRooms = RNG.Next(MIN_NUMBER_OF_ROOMS, MAX_NUMBER_OF_ROOMS);

            List<Room> rooms = new List<Room>();

            Room tmpRoom;
            for (int i = 0; i < numberOfRooms; i++)
            {
                tmpRoom = new Room(this);

                tmpRoom.Origin = FindValidOriginForRoom(rooms, tmpRoom);

                if (tmpRoom.Origin.X == -1)
                    Console.WriteLine("Could not place room.");
                else
                {
                    tmpRoom.AddDoorwayToRoom();
                    rooms.Add(tmpRoom);
                }
            }

            
            Levels.Add(Levels.Count, rooms);

            while (GetCountOfUnconnectedDoors(Levels.Count-1) > 0)
                ConnectTwoDoorways(Levels.Count-1);

            PlacePlayerOnMap();
            DiscoverTilesAroundPlayer();
        }

        public int GetCountOfUnconnectedDoors(int level)
        {
            int count = 0;
            foreach (Room r in Levels[level])
            {
                foreach (RoomTile rt in r.RoomLayout)
                {
                    if (rt.ThisTileType == TileType.Door & rt.Connected == false)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public Point FindValidOriginForRoom(List<Room> rooms, Room proposedRoom)
        {
            bool placed = false;
            int tries = 0;

            while (!placed || tries > 2500)
            {
                int x = RNG.Next(2, MAX_LVL_WIDTH);
                int y = RNG.Next(2, MAX_LVL_HEIGHT);

                Rectangle roomRectangle = new Rectangle(x, y, proposedRoom.Size.X + 3, proposedRoom.Size.Y + 3);

                bool intersectionFound = false;
                foreach (Room r in rooms)
                {
                    Rectangle tmpRect = new Rectangle(r.Origin.X, r.Origin.Y, r.Size.X + 3, r.Size.Y + 3);

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

                tries++;
            }

            return new Point(-1, -1);
        }

        public void PlacePlayerOnMap()
        {
            _player = new Player(this);
            _player.LeveledUp += new EventHandler(_player_LeveledUp);
        }

        private void _player_LeveledUp(object sender, EventArgs e)
        {
            Console.WriteLine("Achieved Level " + ThePlayer.PlayerLevel.ToString() + "!");

            switch (ThePlayer.PlayerLevel)
            {
                case 5:
                    Console.WriteLine("At level 5, you get to choose your first specialization.  Which one will you choose?");
                    listSpecializations();

                    while (!ThePlayer.SpecializationChosen)
                    {
                        string userInput = Console.ReadLine().ToUpper();
                        int userInputInteger;
                        if (Int32.TryParse(userInput, out userInputInteger))
                        {
                            if (userInputInteger >= 0 && userInputInteger < Enum.GetValues(typeof(FirstSpecializations)).Length)
                            {
                                ThePlayer.Specialization = (FirstSpecializations)userInputInteger;
                                Console.WriteLine("Congratulations, you are now specialized: " + Enum.GetName(typeof(FirstSpecializations), userInputInteger) + "!");
                            }
                        }
                    }
                    break;
            }
        }

        private void listSpecializations()
        {
            Array specializationValues = Enum.GetValues(typeof(FirstSpecializations));
            for (int i = 0; i < specializationValues.Length; i++)
            {
                Console.WriteLine(i.ToString() + " - " + Enum.GetName(typeof(FirstSpecializations), specializationValues.GetValue(i)));
            }
            
        }

        public void ConnectTwoDoorways(int level)
        {
            Room room1 = null;
            Point room1DoorWay = new Point(0,0);
            Point room1DoorWayGlobal = new Point(0, 0);
            Room room2 = null;
            Point room2DoorWay = new Point(0, 0);
            Point room2DoorWayGlobal = new Point(0, 0);
            int tries = 0;

            GetRoomAndUnconnectedDoorway(out room1, out room1DoorWay, null, level);
            GetRoomAndUnconnectedDoorway(out room2, out room2DoorWay, room1, level);

            room1DoorWayGlobal = new Point(room1DoorWay.X + room1.Origin.X, room1DoorWay.Y + room1.Origin.Y);

            if (room2 == null)
            {
                //coonect to a hallway
                room2DoorWayGlobal = new Point(LevelHallways[level][0].X, LevelHallways[level][0].Y);
            }
            else
            {
                room2DoorWayGlobal = new Point(room2DoorWay.X + room2.Origin.X, room2DoorWay.Y + room2.Origin.Y);
            }

            List<Point> pointsBetweenDoors = new List<Point>();

            int maxX = 0;
            int maxY = 0;

            bool favorX = false;

            GetMax(out maxX, out maxY, level);

            while (room1.RoomLayout[room1DoorWay.X, room1DoorWay.Y].Connected == false)
            {
                Point pointToTry;

                int ultimateXDirection = room2DoorWayGlobal.X - room1DoorWayGlobal.X;
                int ultimateYDirection = room2DoorWayGlobal.Y - room1DoorWayGlobal.Y;

                if (pointsBetweenDoors.Count > 0)
                {
                    ultimateXDirection = room2DoorWayGlobal.X - pointsBetweenDoors[pointsBetweenDoors.Count - 1].X;
                    ultimateYDirection = room2DoorWayGlobal.Y - pointsBetweenDoors[pointsBetweenDoors.Count - 1].Y;
                }

                if (Math.Abs(ultimateXDirection) != 0 && Math.Abs(ultimateYDirection) != 0)
                {
                    if(RNG.Next(0, 2) == 1)
                        favorX = true;
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

                Point basePoint;

                if (pointsBetweenDoors.Count == 0)
                {
                    basePoint = new Point(room1DoorWayGlobal.X, room1DoorWayGlobal.Y);
                }
                else
                {
                    basePoint = pointsBetweenDoors[pointsBetweenDoors.Count - 1];       
                }

                bool triedXFirst = false;

                if (ultimateXDirection != 0 && favorX)
                {
                    triedXFirst = true;
                    pointToTry = new Point(basePoint.X + ultimateXDirection, basePoint.Y);
                }
                else if (ultimateYDirection != 0)
                {
                    pointToTry = new Point(basePoint.X, basePoint.Y + ultimateYDirection);
                }
                else
                {
                    //break out of the while
                    room1.RoomLayout[room1DoorWay.X, room1DoorWay.Y].Connected = true;

                    if(room2 != null)
                        room2.RoomLayout[room2DoorWay.X, room2DoorWay.Y].Connected = true;
                    break;
                }

                 

                if (!PointIsClearForHallway(pointToTry, level, pointsBetweenDoors, maxX, maxY))
                {
                    if(triedXFirst)
                        pointToTry = new Point(basePoint.X, basePoint.Y + ultimateYDirection);
                    else
                        pointToTry = new Point(basePoint.X + ultimateXDirection, basePoint.Y);

                    if (!PointIsClearForHallway(pointToTry, level, pointsBetweenDoors, maxX, maxY))
                    {
                        pointToTry = new Point(basePoint.X + (-ultimateXDirection), basePoint.Y);

                        if (!PointIsClearForHallway(pointToTry, level, pointsBetweenDoors, maxX, maxY))
                        {
                            pointToTry = new Point(basePoint.X, basePoint.Y + (-ultimateYDirection));

                            if (!PointIsClearForHallway(pointToTry, level, pointsBetweenDoors, maxX, maxY))
                            {
                                pointToTry = new Point(basePoint.X + (-ultimateYDirection), basePoint.Y);

                                if (!PointIsClearForHallway(pointToTry, level, pointsBetweenDoors, maxX, maxY))
                                {
                                    pointToTry = new Point(basePoint.X, basePoint.Y + (-ultimateXDirection));

                                    if (!PointIsClearForHallway(pointToTry, level, pointsBetweenDoors, maxX, maxY))
                                    {
                                        if (LevelHallways.Count == 0 || tries > 10)
                                            break;

                                        //if two doorways can't be connected, try to connect to another hallway
                                        pointsBetweenDoors.Clear();
                                        int hallway = RNG.Next(0, LevelHallways[level].Count-1);
                                        room2DoorWayGlobal = new Point(LevelHallways[level][hallway].X, LevelHallways[level][hallway].Y);
                                        room2 = null;
                                        tries++;
                                        
                                        //Console.WriteLine("Doors at (" + room1DoorWayGlobal.X.ToString() + "," + room1DoorWayGlobal.Y.ToString() + ") could not be connected to doorway at (" + room2DoorWayGlobal.X.ToString() + "," + room2DoorWayGlobal.Y.ToString() + ")");
                                    }
                                    else
                                    {
                                        pointsBetweenDoors.Add(new Point(pointToTry.X, pointToTry.Y));
                                    }
                                }
                                else
                                {
                                    pointsBetweenDoors.Add(new Point(pointToTry.X, pointToTry.Y));
                                }
                            }
                            else
                            {
                                pointsBetweenDoors.Add(new Point(pointToTry.X, pointToTry.Y));
                            }
                        }
                        else
                        {
                            pointsBetweenDoors.Add(new Point(pointToTry.X, pointToTry.Y));
                        }
                    }
                    else
                    {
                        pointsBetweenDoors.Add(new Point(pointToTry.X, pointToTry.Y));
                    }
                }
                else
                {
                    pointsBetweenDoors.Add(new Point(pointToTry.X, pointToTry.Y));
                }

                if ((pointToTry.Y + 1 == room2DoorWayGlobal.Y && pointToTry.X == room2DoorWayGlobal.X) ||
                    (pointToTry.Y - 1 == room2DoorWayGlobal.Y && pointToTry.X == room2DoorWayGlobal.X) ||
                    (pointToTry.Y == room2DoorWayGlobal.Y && pointToTry.X + 1 == room2DoorWayGlobal.X) ||
                    (pointToTry.Y == room2DoorWayGlobal.Y && pointToTry.X - 1 == room2DoorWayGlobal.X) )
                {
                    room1.RoomLayout[room1DoorWay.X, room1DoorWay.Y].Connected = true;

                    if(room2 != null)
                        room2.RoomLayout[room2DoorWay.X, room2DoorWay.Y].Connected = true;
                }
                
            }

            if (LevelHallways.ContainsKey(level))
            {
                LevelHallways[level].AddRange(pointsBetweenDoors);
            }
            else
            {
                LevelHallways.Add(level, pointsBetweenDoors);
            }
        }

        public bool PointIsClearForHallway(Point pointToTry, int level, List<Point> pointsAlreadyTried, int maxX, int maxY)
        {
            if (pointsAlreadyTried != null & pointsAlreadyTried.Count > 0)
            {
                bool tmp = pointsAlreadyTried.Any(x => x.Equals(pointToTry));
                if (tmp)
                    return false;
            }

            if (pointToTry.X < 1 || pointToTry.X > maxX || pointToTry.Y < 1 || pointToTry.Y > maxY)
                return false;

            foreach (Room r in _levels[level])
            {
                if (r.GetRoomTileAtPoint(pointToTry) != null)
                    return false;
            }

            return true;
        }

        private void GetRoomAndUnconnectedDoorway(out Room room1, out Point room1DoorWay, Room roomItCantBe, int level)
        {
            room1 = null;
            room1DoorWay = new Point(0,0);

            List<Room> listOfRooms = null;

            int ordering = RNG.Next(0, 8);

            switch (ordering)
            {
                case 0:
                    listOfRooms = _levels[level].OrderBy(x => x.Size.X).ToList<Room>();
                    break;
                case 1:
                    listOfRooms = _levels[level].OrderBy(x => x.Size.Y).ToList<Room>();
                    break;
                case 2:
                    listOfRooms = _levels[level].OrderByDescending(x => x.Size.X).ToList<Room>();
                    break;
                case 3:
                    listOfRooms = _levels[level].OrderByDescending(x => x.Size.Y).ToList<Room>();
                    break;
                case 4:
                    listOfRooms = _levels[level].OrderBy(x => x.Origin.X).ToList<Room>();
                    break;
                case 5:
                    listOfRooms = _levels[level].OrderBy(x => x.Origin.Y).ToList<Room>();
                    break;
                case 6:
                    listOfRooms = _levels[level].OrderByDescending(x => x.Origin.X).ToList<Room>();
                    break;
                case 7:
                    listOfRooms = _levels[level].OrderByDescending(x => x.Origin.Y).ToList<Room>();
                    break;
                default:
                    listOfRooms = _levels[level].OrderBy(x => x.Size.X).ToList<Room>();
                    break;
            }

            foreach (Room r in listOfRooms)
            {
                if (r.Equals(roomItCantBe)) continue;

                //For each potential vertical wall door
                for (int x = 0; x < r.RoomLayout.GetLength(0); x++)
                {
                    if (r.RoomLayout[x, 0].ThisTileType == TileType.Door && r.RoomLayout[x, 0].Connected == false)
                    {
                        room1 = r;
                        room1DoorWay = new Point(x, 0);

                        return;
                    }
                    else if (r.RoomLayout[x, r.RoomLayout.GetLength(1) - 1].ThisTileType == TileType.Door && r.RoomLayout[x, r.RoomLayout.GetLength(1) - 1].Connected == false)
                    {
                        room1 = r;
                        room1DoorWay = new Point(x, r.RoomLayout.GetLength(1) - 1);
                        return;
                    }
                }

                //For each potential horizontal wall door
                for (int y = 0; y < r.RoomLayout.GetLength(1); y++)
                {
                    if (r.RoomLayout[0, y].ThisTileType == TileType.Door && r.RoomLayout[0, y].Connected == false)
                    {
                        room1 = r;
                        room1DoorWay = new Point(0, y);

                        return;
                    }
                    else if (r.RoomLayout[r.RoomLayout.GetLength(0) - 1, y].ThisTileType == TileType.Door && r.RoomLayout[r.RoomLayout.GetLength(0) - 1, y].Connected == false)
                    {
                        room1 = r;
                        room1DoorWay = new Point(r.RoomLayout.GetLength(0) - 1, y);
                        
                        return;
                    }
                }
            }
        }

        public void DiscoverTilesAroundPlayer()
        {
            List<Point> pointsToDiscover = new List<Point>();

            pointsToDiscover.Add(ThePlayer.Location);

            for (int i = 1; i <= ThePlayer.VisionRadius; i++)
            {
                pointsToDiscover.Add(new Point(ThePlayer.Location.X, ThePlayer.Location.Y + i));
                pointsToDiscover.Add(new Point(ThePlayer.Location.X, ThePlayer.Location.Y - i));
                pointsToDiscover.Add(new Point(ThePlayer.Location.X + i, ThePlayer.Location.Y + i));
                pointsToDiscover.Add(new Point(ThePlayer.Location.X - i, ThePlayer.Location.Y - i));
                pointsToDiscover.Add(new Point(ThePlayer.Location.X - i, ThePlayer.Location.Y + i));
                pointsToDiscover.Add(new Point(ThePlayer.Location.X + i, ThePlayer.Location.Y - i));
                pointsToDiscover.Add(new Point(ThePlayer.Location.X + i, ThePlayer.Location.Y));
                pointsToDiscover.Add(new Point(ThePlayer.Location.X - i, ThePlayer.Location.Y));
            }

            foreach (Point p in pointsToDiscover)
            {
                DiscoverTileAtPoint(p, ThePlayer.DungeonLevel);
            }
        }

        public void DiscoverTileAtPoint(Point p, int level)
        {
            RoomTile roomTile;

            foreach (Room room in _levels[level])
            {
                roomTile = room.GetRoomTileAtPoint(p);

                if (roomTile != null)
                {
                    roomTile.Discovered = true;
                    return;
                }

            }

            if (LevelHallways.ContainsKey(level) & LevelHallways[level].Contains(p))
            {
                if (!LevelDiscoveredHallways.ContainsKey(level))
                    LevelDiscoveredHallways.Add(level, new List<Point>());
                LevelDiscoveredHallways[level].Add(p);

                HallDiscoveredEventArgs e = new HallDiscoveredEventArgs();
                e.hallThatWasDiscovered = p;
                OnHallDiscovered(e);
            }
        
        }

        public bool IsPointTraversable(Point p, int level)
        {
            RoomTile roomTile;

            foreach (Room room in _levels[level])
            {
                roomTile = room.GetRoomTileAtPoint(p);

                if (roomTile != null)
                {
                    if (roomTile.ThisTileType == TileType.HorizontalWall || roomTile.ThisTileType == TileType.VerticalWall)
                        return false;
                    else if (roomTile.ThisTileType == TileType.Floor || roomTile.ThisTileType == TileType.Door)
                        return true;
                }

            }

            if (LevelHallways.ContainsKey(level) & LevelHallways[level].Contains(p))
            {
                return true;
            }

            return false;
        }

        public void ResolveCombat(CombatUnit cunit)
        {
            _combatResolved = true;
            ThePlayer.GiveExperiencePoints(cunit.ExperienceWorth);
            ThePlayer.GiveGold(cunit.GoldWorth);
        }

        public void FleeCombat(CombatUnit cunit)
        {
            _combatResolved = true;
        }

        public void GenerateRandomEnemyEncounter()
        {
            _combatResolved = false;
            _combatUnit = new CombatUnit(this);

            CombatEncounteredEventArgs e = new CombatEncounteredEventArgs();
            e.combatUnit = _combatUnit;

            OnCombatEncountered(e);

            //Console.WriteLine("You encountered an enemy!");
            //Console.WriteLine(cunit.ToString());
            //Console.WriteLine("FIGHT|ENEMYSTATS|FLEE");

            //bool combatResolved = false;
            //while (!combatResolved)
            //{
            //    string userInput = "";// Console.ReadLine();

            //    switch (userInput.Trim().ToUpper())
            //    {
            //        case "ENEMYSTATS":
            //            Console.WriteLine(cunit.FullEnemyStats());
            //            break;
            //        case "FIGHT":
            //            Console.WriteLine("You fought.");
            //            Console.WriteLine("Gained " + cunit.ExperienceWorth.ToString() + " experience and " + cunit.GoldWorth.ToString() + " gold!");
            //            ThePlayer.GiveExperiencePoints(cunit.ExperienceWorth);
            //            ThePlayer.GiveGold(cunit.GoldWorth);
            //            combatResolved = true;
            //            break;
            //        case "FLEE":
            //            Console.WriteLine("You ran away.");
            //            combatResolved = true;
            //            break;
            //        default:
            //            Console.WriteLine("Command not recognized in combat.");
            //            combatResolved = true;
            //            break;
            //    }
            //}
            
        }
    }
}