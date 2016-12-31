using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shadows.InteractableObjects
{
    public enum EnemyForm
    {
        Skeleton=0,
        Spider,
        Orc,
        Goblin,
        Troll,
        Witch,
        Warlock
    }

    public class CombatUnit
    {
        private int _speed = 0;
        private int _health = 1;
        private int _attackPower = 0;  //Min: 5 * dungeon level \ Max: 10 * dungeonLevel
        private int _defensePower = 0; //Min: 2 * dungeon level \ Max: 4 * dungeonLevel
        private int _experienceWorth = 0; //dungeonLevel * 2
        private int _goldWorth = 0; //dungeonLevel * 1.5
        private EnemyForm _enemyForm = EnemyForm.Goblin;
        private int _dungeonLevel = 1;
        int _combatPosition;
        private OverallMap _ovMap;

        public CombatUnit(OverallMap ovMap)
        {
            _ovMap = ovMap;
            _dungeonLevel = ovMap.ThePlayer.DungeonLevel;
            if(_dungeonLevel == 0)
                _dungeonLevel = 1;

            int maxEnemyForm = (int)Enum.GetValues(typeof(EnemyForm)).Cast<EnemyForm>().Max();
            _attackPower = ovMap.RNG.Next(5 * _dungeonLevel, (10 * _dungeonLevel) + 1);
            _defensePower = ovMap.RNG.Next(2 * _dungeonLevel, (4 * _dungeonLevel) + 1);
            _experienceWorth = _dungeonLevel + 5;
            _goldWorth = (int)(_dungeonLevel * 1.5);
            _enemyForm = (EnemyForm)ovMap.RNG.Next(0, maxEnemyForm + 1);
            _health = _dungeonLevel * 5;
            _speed = _dungeonLevel * 2;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GetEnemyFormName());
          
            return sb.ToString();
        }

        public string GetEnemyFormName()
        {
            return Enum.GetName(typeof(EnemyForm), _enemyForm);
        }

        public string FullEnemyStats()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GetEnemyFormName());
            return sb.ToString();
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
    }
}
