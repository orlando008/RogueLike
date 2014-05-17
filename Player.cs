﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RogueLike
{
    public class Player
    {
        Point _location;
        int _dungeonLevel;
        int _playerLevel;
        int _playerExperience = 0;
        int _experiencePerLevel = 100;
        OverallMap _overallMap;
        int _visionRadius = 1;
        int _health = 100;
        int _mana = 100;
        int _healthPotions = 1;
        int _manaPotions = 1;
        int _gold = 10;
        bool _specializationChosen = false;
        RogueLike.OverallMap.FirstSpecializations _specialization1;
        RogueLike.OverallMap.WarriorSpecializations _warriorSpecialization;
        RogueLike.OverallMap.MageSpecializations _mageSpecialization;
        RogueLike.OverallMap.ArcherSpecializations _archerSpecialization;

        public Player(OverallMap ovMap)
        {
            int hallway = ovMap.RNG.Next(0, ovMap.LevelHallways[0].Count - 1);

            _location = ovMap.LevelHallways[0][hallway];
            _dungeonLevel = 0;
            _playerLevel = 1;
            _playerExperience = 0;
            _overallMap = ovMap;
        }

        public Point Location
        {
            get
            {
                return _location;
            }
        }

        public RogueLike.OverallMap.FirstSpecializations Specialization
        {
            get
            {
                return _specialization1;
            }
            set
            {
                _specialization1 = value;
                _specializationChosen = true;
            }
        }

        public bool SpecializationChosen
        {
            get
            {
                return _specializationChosen;
            }
        }

        public int DungeonLevel
        {
            get
            {
                return _dungeonLevel;
            }
        }

        public int PlayerLevel
        {
            get
            {
                return _playerLevel;
            }
        }

        public int VisionRadius
        {
            get
            {
                return _visionRadius;
            }
        }

        public int PlayerExperience
        {
            get
            {
                return _playerExperience;
            }
        }
        
        public int Gold
        {
            get
            {
                return _gold;
            }
        }

        public bool MovePlayer(int xDirection, int yDirection, out bool encounteredEnemy)
        {
            Point newLocation = new Point(_location.X + xDirection, _location.Y + yDirection);
            if (_overallMap.IsPointTraversable(newLocation, _dungeonLevel))
            {
                _location = newLocation;

                if (_overallMap.RNG.Next(0, 101) > 70)
                    encounteredEnemy = true;
                else
                    encounteredEnemy = false;
               
                return true;
            }
            else
            {
                encounteredEnemy = false;
                return false;   
            }      
        }

        public string LocationString()
        {
            return "Currently in dungeon level " + _dungeonLevel.ToString() + " at (" + _location.X.ToString() + "," + _location.Y.ToString() + ")";
        }

        public void DrawStats()
        {
            Console.WriteLine("Level: " + _playerLevel.ToString());
            Console.WriteLine("Experience To Next Level: " + (_experiencePerLevel - _playerExperience).ToString());
            Console.WriteLine("Location: " + _location.ToString());
            Console.WriteLine("Health: " + _health.ToString());
            Console.WriteLine("Mana: " + _mana.ToString());

            if(SpecializationChosen)
                Console.WriteLine("Specialization: " + Enum.GetName(typeof(RogueLike.OverallMap.FirstSpecializations), _specialization1));
        }

        public void DrawInventory()
        {
            Console.WriteLine("Inventory:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Gold - " + _gold.ToString());
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Health Potions - " + _healthPotions.ToString());
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Mana Potions - " + _healthPotions.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void GiveExperiencePoints(int exp)
        {
            _playerExperience += exp;

            if (_playerExperience >= _experiencePerLevel)
            {
                int levelsGained = _playerExperience / _experiencePerLevel;
                _playerExperience = _playerExperience - (_experiencePerLevel * (_playerExperience / _experiencePerLevel));

                for (int i = 0; i < levelsGained - 1; i++)
                {
                    _playerLevel += 1;
                    OnLeveledUp(EventArgs.Empty);
                }   
            }
        }

        public void GiveGold(int gold)
        {
            _gold += gold;
        }

        public event EventHandler LeveledUp;

        protected void OnLeveledUp(EventArgs e)
        {
            EventHandler handler = LeveledUp;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
