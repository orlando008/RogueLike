using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using static Shadows.EquipmentGenerationMethods;
using static Shadows.EquipmentEnumerations;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Shadows
{
    public class Player : INotifyPropertyChanged, ICombatEntity
    {
        Point _location;
        int _dungeonLevel;
        int _playerLevel;
        int _playerExperience = 0;
        OverallMap _overallMap;
        int _visionRadius = 3;
        int _maxHealth = 6;
        int _gold = 10;

        CombatProperties _combatProperties;

        CommonEnumerations.BaseClassTypes _baseClassType;

        private List<Equipment> _playersEquipment;
        private ObservableCollection<CombatAction> _possibleActions;

        public Player(OverallMap ovMap, CommonEnumerations.BaseClassTypes chosenClass)
        {
            int hallway = ovMap.RNG.Next(0, ovMap.LevelHallways[0].Count - 1);

            _combatProperties = new CombatProperties();
            _location = ovMap.LevelHallways[0][hallway];
            _dungeonLevel = 0;
            _playerLevel = 1;
            _playerExperience = 0;
            _overallMap = ovMap;
            _baseClassType = chosenClass;
            _possibleActions = new ObservableCollection<CombatAction>();
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.BasicAttackDagger, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.BasicAttackShortSword, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.BasicAttackLongSword, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.BasicAttackShortBow, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.BasicAttackLongBow, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.BasicAttackCrossBow, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.BasicAttackWand, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.BasicAttackStave, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.BasicAttackStaff, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.SharpenSteel, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.ArrowNock, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.HoneKnowledge, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.BerserkerStrike, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.SnipersShot, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.ArcaneWisdom, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.ConsumeHealthPotion, this._overallMap));
            _possibleActions.Add(new CombatAction(CommonEnumerations.CombatActionTypes.DefensiveStance, this._overallMap));

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

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void MoveCombatRight()
        {
            _overallMap.CurrentCombatLogic.ProcessCombatEntityMovement(true, 1);
        }

        public void MoveCombatLeft()
        {
            _overallMap.CurrentCombatLogic.ProcessCombatEntityMovement(true, -1);
        }

        private void GiveStartingWarriorEquipment()
        {
            CombatProperties.BaseDEX = 4;
            CombatProperties.BaseSTR = 5;
            CombatProperties.BaseINT = 3;

            _playersEquipment.Add(new Weapon(EquipmentPrefix.Successful, EquipmentSuffix.OfPersistence, WeaponTypes.ShortSword));
            _playersEquipment[_playersEquipment.Count - 1].Equipped = true;
            _playersEquipment[_playersEquipment.Count - 1].PrefixValue = 80;
            _playersEquipment[_playersEquipment.Count - 1].SuffixValue = 80;
        }

        private void GiveStartingRogueEquipment()
        {
            CombatProperties.BaseDEX = 5;
            CombatProperties.BaseSTR = 4;
            CombatProperties.BaseINT = 3;

            _playersEquipment.Add(new Weapon(EquipmentPrefix.None, EquipmentSuffix.None, WeaponTypes.ShortBow));
            _playersEquipment[_playersEquipment.Count - 1].Equipped = true;
        }

        private void GiveStartingMageEquipment()
        {
            CombatProperties.BaseDEX = 4;
            CombatProperties.BaseSTR = 3;
            CombatProperties.BaseINT = 5;

            _playersEquipment.Add(new Weapon(EquipmentPrefix.None, EquipmentSuffix.None, WeaponTypes.Wand));
            _playersEquipment[_playersEquipment.Count - 1].Equipped = true;
        }
        
        private int GetValueOfEquippedItems(PrefixSuffixModifier psm)
        {
            int value = 0;
            foreach (Equipment item in _playersEquipment.Where(x => x.Equipped))
            {
                if(GetBaselineModifier(item.EquipmentPrefix1) == psm)
                {
                    value += item.PrefixValue;
                }

                if(GetBaselineModifier(item.EquipmentSuffix1) == psm)
                {
                    value += item.SuffixValue;
                }
            }

            return value;
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
                return _combatProperties.CurrentHP;
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
                return Convert.ToDecimal(Health) / Convert.ToDecimal(_maxHealth) * 100;
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
                return CombatProperties.CombatPosition;
            }

            set
            {
                CombatProperties.CombatPosition = value;
            }
        }


        public bool InCombatPosition1
        {
            get
            {
                return (CombatProperties.CombatPosition == 1);
            }
        }

        public bool InCombatPosition2
        {
            get
            {
                return (CombatProperties.CombatPosition == 2);
            }
        }

        public bool InCombatPosition3
        {
            get
            {
                return (CombatProperties.CombatPosition == 3);
            }
        }

        public bool InCombatPosition4
        {
            get
            {
                return (CombatProperties.CombatPosition == 4);
            }
        }

        public bool InCombatPosition5
        {
            get
            {
                return (CombatProperties.CombatPosition == 5);
            }
        }

        public bool InCombatPosition6
        {
            get
            {
                return (CombatProperties.CombatPosition == 6);
            }
        }

        public bool InCombatPosition7
        {
            get
            {
                return (CombatProperties.CombatPosition == 7);
            }
        }

        public bool InCombatPosition8
        {
            get
            {
                return (CombatProperties.CombatPosition == 8);
            }
        }

        public bool InCombatPosition9
        {
            get
            {
                return (CombatProperties.CombatPosition == 9);
            }
        }


        public bool InCombatPosition10
        {
            get
            {
                return (CombatProperties.CombatPosition == 10);
            }
        }

        public bool InCombatPosition11
        {
            get
            {
                return (CombatProperties.CombatPosition == 11);
            }
        }


        public CombatProperties CombatProperties
        {
            get
            {
                return _combatProperties;
            }

            set
            {
                _combatProperties = value;
                NotifyPropertyChanged(nameof(CombatProperties));
            }
        }

        public ObservableCollection<CombatAction> PossibleActions
        {
            get
            {
                return _possibleActions;
            }

            set
            {
                _possibleActions = value;
            }
        }

        public bool MovePlayerNonCombat(int xDirection, int yDirection)
        {
            Point newLocation = new Point(_location.X + xDirection, _location.Y + yDirection);
            if (_overallMap.IsPointTraversable(newLocation, _dungeonLevel))
            {
                _location = newLocation;

                if (_overallMap.EnemyLocations.ContainsKey(DungeonLevel - 1))
                {
                    if (_overallMap.EnemyLocations[DungeonLevel - 1].FirstOrDefault(e => e.DungeonCoordinate == _location) != null)
                    {
                        OverallMap.CombatEncounteredEventArgs ce = new OverallMap.CombatEncounteredEventArgs();
                        ce.combatUnit = _overallMap.EnemyLocations[DungeonLevel - 1].FirstOrDefault(e => e.DungeonCoordinate == _location);
                        _overallMap.OnCombatEncountered(ce);
                    }

                }

                return true;
            }
            else
            {
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

        void ICombatEntity.InitializeBattleValues()
        {
            this.CombatProperties.BattleDEX = this.CombatProperties.BaseDEX;
            this.CombatProperties.BattleINT = this.CombatProperties.BaseSTR;
            this.CombatProperties.BattleSTR = this.CombatProperties.BaseINT;

            foreach(PrefixSuffixModifier psm in Enum.GetValues(typeof(PrefixSuffixModifier)))
            {
                this.CombatProperties.SetValueOfPropertyBasedOnModifier(psm, GetValueOfEquippedItems(psm));
            }

            if (_overallMap.RNG.Next(1,101) < this.CombatProperties.PercentChanceToIncreaseDEX)
            {
                this.CombatProperties.BattleDEX += 1;
            }

            if (_overallMap.RNG.Next(1, 101) < this.CombatProperties.PercentChanceToIncreaseSTR)
            {
                this.CombatProperties.BattleSTR += 1;
            }

            if (_overallMap.RNG.Next(1, 101) < this.CombatProperties.PercentChanceToIncreaseINT)
            {
                this.CombatProperties.BattleINT += 1;
            }

            this.CombatProperties.CurrentHP = 6;
            this.CombatProperties.CurrentActionPoints = 0;
            this.CombatProperties.CurrentMovementPoints = 0;
        }

        void ICombatEntity.BeginTurn()
        {
            _overallMap.OnStoryMessage(new Program.StoryMessageEventArgs("Your turn begins.", System.Windows.Media.Colors.LightPink));
            _overallMap.CurrentCombatLogic.AwardMovementPoints(isPlayer: true);
            _overallMap.CurrentCombatLogic.AwardActionPoints(isPlayer: true);
        }

        void ICombatEntity.GiveEntityTheCombatLogic(CombatLogic clog)
        {
           
        }

        CombatProperties ICombatEntity.GetCombatProperties()
        {
            return CombatProperties;
        }
    }
}
