using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueLike.InteractableObjects
{
    public enum ElementalType
    {
        Fire = 0,
        Water,
        Wind
    }

    public enum EnemyForm
    {
        Human = 0,
        Skeleton,
        Spider,
        Orc,
        Goblin,
        Bat,
        Bear,
        Troll,
        Witch,
        Warlock
    }

    public class CombatUnit
    {
        private string _name;
        private int _speed;
        private int _health;
        private int _attackPower;  //Min: 5 * dungeon level \ Max: 10 * dungeonLevel
        private int _defensePower; //Min: 2 * dungeon level \ Max: 4 * dungeonLevel
        private int _experienceWorth; //AttackPower + 
        private int _goldWorth;
        private ElementalType _type;
        private EnemyForm _enemyForm;
        private int _dungeonLevel;

        public CombatUnit(int dungeonLevel)
        {
            int maxEnemyForm = (int)Enum.GetValues(typeof(EnemyForm)).Cast<EnemyForm>().Max();
            int maxElementalType = (int)Enum.GetValues(typeof(ElementalType)).Cast<ElementalType>().Max();
        }
    }
}
