﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Shadows
{
    public enum EnemyForm
    {
        Goblin,
        Orc,
        Troll,
        SkeletonArcher,
        GhostHuntress,
        DemonSniper,
        Witch,
        Warlock,
        EtherealSpirit
    }

    public class CombatUnit : ICombatEntity, INotifyPropertyChanged
    {
        private int _health = 1;
        private int _experienceWorth = 0; //dungeonLevel * 2
        private int _goldWorth = 0; //dungeonLevel * 1.5
        private EnemyForm _enemyForm = EnemyForm.Goblin;
        private int _dungeonLevel = 1;

        private OverallMap _ovMap;
        private Point _dungeonCoordinate;
        int _baseSTR = 0;
        int _baseDEX = 0;
        int _baseINT = 0;
        int _adjustedSTR = 0;
        int _adjustedDEX = 0;
        int _adjustedINT = 0;
        int _currentActionPoints = 0;
        int _currentMovementPoints = 0;

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
        CombatProperties _combatProperties;

        public CombatUnit(OverallMap ovMap, Point dungeonCoordinate)
        {
            _ovMap = ovMap;
            _dungeonLevel = ovMap.ThePlayer.DungeonLevel;
            DungeonCoordinate = dungeonCoordinate;
            if (_dungeonLevel == 0)
                _dungeonLevel = 1;

            int maxEnemyForm = (int)Enum.GetValues(typeof(EnemyForm)).Cast<EnemyForm>().Max();
            _experienceWorth = _dungeonLevel;
            _goldWorth = (int)(_dungeonLevel);
            _enemyForm = (EnemyForm)ovMap.RNG.Next(0, maxEnemyForm + 1);
            _health = 6;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GetEnemyFormName());
          
            return sb.ToString();
        }

        public string GetEnemyFormName()
        {
            string fullEnumName = Enum.GetName(typeof(EnemyForm), _enemyForm);
            return Regex.Replace(fullEnumName, "(\\B[A-Z])", " $1");
        }

        public string FullEnemyStats()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GetEnemyFormName());
            return sb.ToString();
        }

        void ICombatEntity.InitializeBattleValues()
        {
            _combatProperties = new CombatProperties();
            _combatProperties.CombatPosition = 8;
        }

        void ICombatEntity.BeginTurn()
        {
            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs($"***{this.GetEnemyFormName()} turn begins***", System.Windows.Media.Colors.LightPink));
            _ovMap.CurrentCombatLogic.AwardMovementPoints(isPlayer: false);
            _ovMap.CurrentCombatLogic.AwardActionPoints(isPlayer: false);

            //Try to get in range.
            while(CombatProperties.CurrentMovementPoints > 0)
            {
                if (Math.Abs(CombatProperties.CombatPosition - _ovMap.ThePlayer.CombatPosition) == 1)
                    break;

                _ovMap.CurrentCombatLogic.ProcessCombatEntityMovement(isPlayer: false, direction:-1);
                NotifyPropertyChanged("");
            }

            //If in range, attack as many times as possible.
            while(CombatProperties.CurrentActionPoints > 0)
            {
                if (Math.Abs(CombatProperties.CombatPosition - _ovMap.ThePlayer.CombatPosition) == 1)
                {
                    _ovMap.CurrentCombatLogic.ProcessCombatEntityAction(isPlayer: false, ca: new CombatAction(CommonEnumerations.CombatActionTypes.BasicAttackDagger, _ovMap));
                }
                else
                {
                    break;
                }

                NotifyPropertyChanged("");
            }

            //Any remainder movement points can be used to move back
            while (CombatProperties.CurrentMovementPoints > 0)
            {
                if (CombatProperties.CombatPosition == 11)
                    break;

                _ovMap.CurrentCombatLogic.ProcessCombatEntityMovement(isPlayer: false, direction: 1);

                NotifyPropertyChanged("");
            }

            //Any remainder action points can be used to defend
            while (CombatProperties.CurrentActionPoints > 0)
            {
                _ovMap.CurrentCombatLogic.ProcessCombatEntityAction(isPlayer: false, ca: new CombatAction(CommonEnumerations.CombatActionTypes.DefensiveStance, _ovMap));
                NotifyPropertyChanged("");
            }

            if(CombatProperties.CurrentActionPoints > 0 || CombatProperties.CurrentMovementPoints > 0)
                _ovMap.CurrentCombatLogic.EndTurn(isPlayer: false);
        }

        void ICombatEntity.GiveEntityTheCombatLogic(CombatLogic clog)
        {
            
        }

        public int GetCombatPosition()
        {
            return CombatPosition;
        }

        public void CombatPositionAdjustment(int adjustment)
        {
            CombatPosition += adjustment;
        }

        public CombatProperties GetCombatProperties()
        {
            return _combatProperties;
        }


        public int ExperienceWorth
        {
            get
            {
                return _experienceWorth;
            }
        }

        public int GoldWorth
        {
            get
            {
                return _goldWorth;
            }
        }

        public Uri ImageSource
        {
            get
            {
                return new Uri("pack://application:,,,/Images/Enemies/" + GetEnemyFormName() + ".png");
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

        public int CombatPosition
        {
            get
            {
                if (CombatProperties != null)
                    return CombatProperties.CombatPosition;
                else
                    return 0;
            }

            set
            {
                if (CombatProperties != null)
                    CombatProperties.CombatPosition = value;
                NotifyPropertyChanged(nameof(CombatPosition));
            }
        }

        public Point DungeonCoordinate
        {
            get
            {
                return _dungeonCoordinate;
            }

            set
            {
                _dungeonCoordinate = value;
            }
        }

        public bool InCombatPosition1
        {
            get
            {
                return (CombatPosition == 1);
            }
        }

        public bool InCombatPosition2
        {
            get
            {
                return (CombatPosition == 2);
            }
        }

        public bool InCombatPosition3
        {
            get
            {
                return (CombatPosition == 3);
            }
        }

        public bool InCombatPosition4
        {
            get
            {
                return (CombatPosition == 4);
            }
        }

        public bool InCombatPosition5
        {
            get
            {
                return (CombatPosition == 5);
            }
        }

        public bool InCombatPosition6
        {
            get
            {
                return (CombatPosition == 6);
            }
        }

        public bool InCombatPosition7
        {
            get
            {
                return (CombatPosition == 7);
            }
        }

        public bool InCombatPosition8
        {
            get
            {
                return (CombatPosition == 8);
            }
        }

        public bool InCombatPosition9
        {
            get
            {
                return (CombatPosition == 9);
            }
        }


        public bool InCombatPosition10
        {
            get
            {
                return (CombatPosition == 10);
            }
        }

        public bool InCombatPosition11
        {
            get
            {
                return (CombatPosition == 11);
            }
        }
    }
}
