using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

    public class CombatUnit : ICombatEntity
    {
        private int _health = 1;
        private int _experienceWorth = 0; //dungeonLevel * 2
        private int _goldWorth = 0; //dungeonLevel * 1.5
        private EnemyForm _enemyForm = EnemyForm.Goblin;
        private int _dungeonLevel = 1;
        int _combatPosition;
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

        int ICombatEntity.GetCurrentHealthPoints()
        {
            return _health;
        }

        int ICombatEntity.GetCurrentActionPoints()
        {
            return _currentActionPoints;   
        }

        int ICombatEntity.GetCurrentMovementPoints()
        {
            return _currentMovementPoints;
        }

        int ICombatEntity.GetCurrentSTR()
        {
            return _adjustedSTR;
        }

        int ICombatEntity.GetCurrentDEX()
        {
            return _adjustedDEX;
        }

        int ICombatEntity.GetCurrentINT()
        {
            return _adjustedINT;
        }

        void ICombatEntity.InitializeBattleValues()
        {
            
        }

        void ICombatEntity.ActionPointAdjustment(int numberOfPoints)
        {
            
        }

        void ICombatEntity.MovementPointAdjustment(int numberOfPoints)
        {
            
        }

        void ICombatEntity.BeginTurn()
        {
            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Enemy turn begins.", System.Windows.Media.Colors.LightPink));
            _ovMap.CurrentCombatLogic.AwardMovementPoints(isPlayer: false);
            _ovMap.CurrentCombatLogic.AwardActionPoints(isPlayer: false);
        }

        void ICombatEntity.GiveEntityTheCombatLogic(CombatLogic clog)
        {
            
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
    }
}
