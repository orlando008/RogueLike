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
        int _experiencePerLevel = 0;
        OverallMap _overallMap;
        int _visionRadius = 1;
        int _health = 6;
        int _maxHealth = 6;
        int _gold = 10;
        int _combatPosition;
        int _baseSTR = 0;
        int _baseDEX = 0;
        int _baseINT = 0;

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
        }

        public void MoveCombatRight()
        {
            if(_combatPosition < 11)
            {
                if(_overallMap.CurrentCombatUnit.CombatPosition != _combatPosition + 1)
                {
                    _combatPosition = _combatPosition + 1;
                    _overallMap.OnStoryMessage(new Program.StoryMessageEventArgs("You advanced 1 step.", System.Windows.Media.Colors.LightPink));
                }
                else
                {
                    _overallMap.OnStoryMessage(new Program.StoryMessageEventArgs("You cannot move there, the enemy is taking up the area.", System.Windows.Media.Colors.LightPink));
                }
                    
            }
            else
            {
                _overallMap.OnStoryMessage(new Program.StoryMessageEventArgs("You cannot move there, a hard wall prevents you.", System.Windows.Media.Colors.LightPink));
            }
                
        }

        public void MoveCombatLeft()
        {
            if(_combatPosition > 1)
            {
                if (_overallMap.CurrentCombatUnit.CombatPosition != _combatPosition - 1)
                {
                    _combatPosition = _combatPosition - 1;
                    _overallMap.OnStoryMessage(new Program.StoryMessageEventArgs("You back up 1 step.", System.Windows.Media.Colors.LightPink));
                }
                else
                {
                    _overallMap.OnStoryMessage(new Program.StoryMessageEventArgs("You cannot move there, the enemy is taking up the area.", System.Windows.Media.Colors.LightPink));
                }
                    
            }
            else
            {
                _overallMap.OnStoryMessage(new Program.StoryMessageEventArgs("You cannot move there, a hard wall prevents you.", System.Windows.Media.Colors.LightPink));
            }
        }

        private void GiveStartingWarriorEquipment()
        {
            _baseDEX = 4;
            _baseSTR = 5;
            _baseINT = 3;

            _playersEquipment.Add(new Weapon(EquipmentPrefix.None, EquipmentSuffix.None, WeaponTypes.ShortSword));
        }

        private void GiveStartingRogueEquipment()
        {
            _baseDEX = 5;
            _baseSTR = 4;
            _baseINT = 3;

            _playersEquipment.Add(new Weapon(EquipmentPrefix.None, EquipmentSuffix.None, WeaponTypes.ShortBow));
        }

        private void GiveStartingMageEquipment()
        {
            _baseDEX = 4;
            _baseSTR = 3;
            _baseINT = 5;

            _playersEquipment.Add(new Weapon(EquipmentPrefix.None, EquipmentSuffix.None, WeaponTypes.Wand));
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

        public int DungeonLevel
        {
            get
            {
                return _dungeonLevel + 1;
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

        public int MaxHealth
        {
            get
            {
                return _maxHealth;
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

        public decimal ProgressTowardsNextLevel
        {
            get
            {
                return Convert.ToDecimal(_playerExperience) / Convert.ToDecimal(ExperiencePerLevel) * 100;
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
                return Convert.ToDecimal(5 * _playerLevel + (5 * (_playerLevel - 1)));
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

        public int CombatPosition
        {
            get
            {
                return _combatPosition;
            }

            set
            {
                _combatPosition = value;
            }
        }


        public bool InCombatPosition1
        {
            get
            {
                return (_combatPosition == 1);
            }
        }

        public bool InCombatPosition2
        {
            get
            {
                return (_combatPosition == 2);
            }
        }

        public bool InCombatPosition3
        {
            get
            {
                return (_combatPosition == 3);
            }
        }

        public bool InCombatPosition4
        {
            get
            {
                return (_combatPosition == 4);
            }
        }

        public bool InCombatPosition5
        {
            get
            {
                return (_combatPosition == 5);
            }
        }

        public bool InCombatPosition6
        {
            get
            {
                return (_combatPosition == 6);
            }
        }

        public bool InCombatPosition7
        {
            get
            {
                return (_combatPosition == 7);
            }
        }

        public bool InCombatPosition8
        {
            get
            {
                return (_combatPosition == 8);
            }
        }

        public bool InCombatPosition9
        {
            get
            {
                return (_combatPosition == 9);
            }
        }


        public bool InCombatPosition10
        {
            get
            {
                return (_combatPosition == 10);
            }
        }

        public bool InCombatPosition11
        {
            get
            {
                return (_combatPosition == 11);
            }
        }

        public bool MovePlayer(int xDirection, int yDirection, out bool encounteredEnemy)
        {
            Point newLocation = new Point(_location.X + xDirection, _location.Y + yDirection);
            if (_overallMap.IsPointTraversable(newLocation, _dungeonLevel))
            {
                _location = newLocation;

                if (_overallMap.EnemyLocations.ContainsKey(DungeonLevel - 1))
                {
                    if (_overallMap.EnemyLocations[DungeonLevel - 1].FirstOrDefault(e => e.DungeonCoordinate == _location) != null)
                    {
                        encounteredEnemy = true;
                        OverallMap.CombatEncounteredEventArgs ce = new OverallMap.CombatEncounteredEventArgs();
                        ce.combatUnit = _overallMap.EnemyLocations[DungeonLevel - 1].FirstOrDefault(e => e.DungeonCoordinate == _location);
                        _overallMap.OnCombatEncountered(ce);
                    }
                    else
                        encounteredEnemy = false;

                }
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

            while(_playerExperience >= ExperiencePerLevel)
            {
                _playerLevel += 1;
                OnLeveledUp(EventArgs.Empty);
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
