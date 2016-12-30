using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using static Shadows.EquipmentGenerationMethods;
using static Shadows.EquipmentEnumerations;

namespace Shadows
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
        int _maxHealth = 100;
        int _maxMana = 100;
        int _mana = 100;
        int _gold = 10;
        bool _specializationChosen = false;
        Shadows.OverallMap.FirstSpecializations _specialization1;

        CommonEnumerations.BaseClassTypes _baseClassType;

        private List<Equipment> _playersEquipment;

        public Player(OverallMap ovMap, CommonEnumerations.BaseClassTypes chosenClass)
        {
            int hallway = ovMap.RNG.Next(0, ovMap.LevelHallways[0].Count - 1);

            _location = ovMap.LevelHallways[0][hallway];
            _dungeonLevel = 0;
            _playerLevel = 1;
            _playerExperience = 0;
            _overallMap = ovMap;
            _baseClassType = chosenClass;

            _playersEquipment = new List<Equipment>();
            _playersEquipment.Add(new Equipment(EquipmentType.Pants, EquipmentPrefix.None, EquipmentSuffix.None));
            _playersEquipment.Add(new Equipment(EquipmentType.Chest, EquipmentPrefix.None, EquipmentSuffix.None));

            switch(chosenClass)
            {
                case CommonEnumerations.BaseClassTypes.Warrior:
                    GiveStartingWarriorEquipment();
                    break;
                case CommonEnumerations.BaseClassTypes.Rogue:
                    GiveStartingRogueEquipment();
                    break;
                case CommonEnumerations.BaseClassTypes.Mage:
                    GiveStartingMageEquipment();
                    break;
            }
            
            
            _playersEquipment.Add(new Equipment(EquipmentType.Boots, EquipmentPrefix.None, EquipmentSuffix.None));
            _playersEquipment.Add(new Equipment(EquipmentType.Belt, EquipmentPrefix.None, EquipmentSuffix.None));
        }

        private void GiveStartingWarriorEquipment()
        {
            _playersEquipment.Add(new Weapon(EquipmentPrefix.None, EquipmentSuffix.None, WeaponTypes.ShortSword));
            _playersEquipment.Add(new OffHand(EquipmentPrefix.None, EquipmentSuffix.None, OffHandTypes.Buckler));
        }

        private void GiveStartingRogueEquipment()
        {
            _playersEquipment.Add(new Weapon(EquipmentPrefix.None, EquipmentSuffix.None, WeaponTypes.ShortBow));
            _playersEquipment.Add(new OffHand(EquipmentPrefix.None, EquipmentSuffix.None, OffHandTypes.Arrows));
        }

        private void GiveStartingMageEquipment()
        {
            _playersEquipment.Add(new Weapon(EquipmentPrefix.None, EquipmentSuffix.None, WeaponTypes.Wand));
            _playersEquipment.Add(new OffHand(EquipmentPrefix.None, EquipmentSuffix.None, OffHandTypes.Tome));
        }

        public Equipment EquippedWeapon
        {
            get
            {
                return _playersEquipment.FirstOrDefault(x => x.EquipmentType1 == EquipmentType.Weapon);
            }
        }

        public Equipment EquippedOffHand
        {
            get
            {
                return _playersEquipment.FirstOrDefault(x => x.EquipmentType1 == EquipmentType.OffHand);
            }
        }

        public Equipment EquippedNecklace
        {
            get
            {
                return _playersEquipment.FirstOrDefault(x => x.EquipmentType1 == EquipmentType.Necklace);
            }
        }

        public Equipment EquippedRing1
        {
            get
            {
                return _playersEquipment.FirstOrDefault(x => x.EquipmentType1 == EquipmentType.Ring);
            }
        }

        public Equipment EquippedRing2
        {
            get
            {
                return _playersEquipment.FirstOrDefault(x => x.EquipmentType1 == EquipmentType.Ring);
            }
        }

        public Equipment EquippedBelt
        {
            get
            {
                return _playersEquipment.FirstOrDefault(x => x.EquipmentType1 == EquipmentType.Belt);
            }
        }


        public Equipment EquippedBoots
        {
            get
            {
                return _playersEquipment.FirstOrDefault(x => x.EquipmentType1 == EquipmentType.Boots);
            }
        }

        public Equipment EquippedHelmet
        {
            get
            {
                return _playersEquipment.FirstOrDefault(x => x.EquipmentType1 == EquipmentType.Helmet);
            }
        }

        public Equipment EquippedChest
        {
            get
            {
                return _playersEquipment.FirstOrDefault(x => x.EquipmentType1 == EquipmentType.Chest);
            }
        }

        public Equipment EquippedPants
        {
            get
            {
                return _playersEquipment.FirstOrDefault(x => x.EquipmentType1 == EquipmentType.Pants);
            }
        }

        public Point Location
        {
            get
            {
                return _location;
            }
        }

        public Shadows.OverallMap.FirstSpecializations Specialization
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

        public int Health
        {
            get
            {
                return _health;
            }
        }


        public int Mana
        {
            get
            {
                return _mana;
            }
        }


        public int MaxHealth
        {
            get
            {
                return _maxHealth;
            }
        }

        public int MaxMana
        {
            get
            {
                return _maxMana;
            }
        }

        public decimal HealthPercentage
        {
            get
            {
                return Convert.ToDecimal(_health) / Convert.ToDecimal(_maxHealth) * 100;
            }
            set { }
        }

        public decimal ManaPercentage
        {
            get
            {
                return Convert.ToDecimal(_mana) / Convert.ToDecimal(_maxMana) * 100;
            }
            set { }
        }

        public decimal ProgressTowardsNextLevel
        {
            get
            {
                return Convert.ToDecimal(_playerExperience) / Convert.ToDecimal(_experiencePerLevel) * 100;
            }
            set { }
        }

        public decimal ExperienceTowardsNextLevel
        {
            get
            {
                return Convert.ToDecimal(_playerExperience);
            }
            set { }
        }


        public decimal ExperiencePerLevel
        {
            get
            {
                return Convert.ToDecimal(_experiencePerLevel);
            }
            set { }
        }

        public CommonEnumerations.BaseClassTypes BaseClassType
        {
            get
            {
                return _baseClassType;
            }

            set
            {
                _baseClassType = value;
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

        public void GiveExperiencePoints(int exp)
        {
            _playerExperience += exp;

            if (_playerExperience >= _experiencePerLevel)
            {
                int levelsGained = _playerExperience / _experiencePerLevel;
                _playerExperience = _playerExperience - (_experiencePerLevel * (_playerExperience / _experiencePerLevel));

                for (int i = 0; i < levelsGained; i++)
                {
                    _playerLevel += 1;
                    _experiencePerLevel = Convert.ToInt32(Convert.ToDouble(_experiencePerLevel) * 1.5);
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
