using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Shadows
{
    public class CombatAction
    {
        private string _name;
        private int _range;
        private string _fullDescription;
        private int _additionChancePercentage;
        private OverallMap _ovMap;

        public CombatAction(CommonEnumerations.CombatActionTypes name, OverallMap ovmap)
        {
            _ovMap = ovmap;
            switch (name)
            {
                case CommonEnumerations.CombatActionTypes.BasicAttackDagger:
                    Name = "Basic Attack (Dagger)";
                    Range = 1;
                    AdditionChancePercentage = 60;
                    FullDescription = "Deals 1 damage to enemy.  Additional {0}% chance to deal another 1 damage.  Additional roll only occurs when STR >= enemy STR.";
                    break;
                case CommonEnumerations.CombatActionTypes.BasicAttackShortSword:
                    Name = "Basic Attack (Short Sword)";
                    Range = 1;
                    AdditionChancePercentage = 70;
                    FullDescription = "Deals 1 damage to enemy.  Additional {0}% chance to deal another 1 damage.  Additional roll only occurs when STR > enemy STR.";
                    break;
                case CommonEnumerations.CombatActionTypes.BasicAttackLongSword:
                    Name = "Basic Attack (Long Sword)";
                    Range = 1;
                    AdditionChancePercentage = 80;
                    FullDescription = "Deals 1 damage to enemy.  Additional {0}% chance to deal another 1 damage.  Additional roll only occurs when STR > enemy STR and DEX >= enemy DEX.";
                    break;
                case CommonEnumerations.CombatActionTypes.BasicAttackShortBow:
                    Name = "Basic Attack (Short Bow)";
                    Range = 2;
                    AdditionChancePercentage = 60;
                    FullDescription = "Deals 1 damage to enemy.  Additional {0}% chance to deal another 1 damage.  Additional roll only occurs when DEX >= enemy DEX.";
                    break;
                case CommonEnumerations.CombatActionTypes.BasicAttackLongBow:
                    Name = "Basic Attack (Long Bow)";
                    Range = 2;
                    AdditionChancePercentage = 70;
                    FullDescription = "Deals 1 damage to enemy.  Additional {0}% chance to deal another 1 damage.  Additional roll only occurs when DEX > enemy DEX.";
                    break;
                case CommonEnumerations.CombatActionTypes.BasicAttackCrossBow:
                    Name = "Basic Attack (Crossbow)";
                    Range = 2;
                    AdditionChancePercentage = 80;
                    FullDescription = "Deals 1 damage to enemy.  Additional {0}% chance to deal another 1 damage.  Additional roll only occurs when DEX > enemy DEX and enemy is not in adjacent tile.";
                    break;
                case CommonEnumerations.CombatActionTypes.BasicAttackWand:
                    Name = "Basic Attack (Wand)";
                    Range = 2;
                    AdditionChancePercentage = 60;
                    FullDescription = "Deals 1 damage to enemy.  Additional {0}% chance to deal another 1 damage.  Additional roll only occurs when INT >= enemy INT.";
                    break;
                case CommonEnumerations.CombatActionTypes.BasicAttackStave:
                    Name = "Basic Attack (Stave)";
                    Range = 2;
                    AdditionChancePercentage = 70;
                    FullDescription = "Deals 1 damage to enemy.  Additional {0}% chance to deal another 1 damage.  Additional roll only occurs when INT > enemy INT.";
                    break;
                case CommonEnumerations.CombatActionTypes.BasicAttackStaff:
                    Name = "Basic Attack (Staff)";
                    Range = 2;
                    AdditionChancePercentage = 80;
                    FullDescription = "Deals 1 damage to enemy.  Additional {0}% chance to deal another 1 damage.  Additional roll only occurs when INT > enemy INT and enemy is not in adjacent tile.";
                    break;
                case CommonEnumerations.CombatActionTypes.SharpenSteel:
                    Name = "Sharpen Steel";
                    Range = 0;
                    AdditionChancePercentage = 5;
                    FullDescription = "Increase basic weapon additional effects by {0}% on next basic attack.";
                    break;
                case CommonEnumerations.CombatActionTypes.ArrowNock:
                    Name = "Arrow Nock";
                    Range = 0;
                    AdditionChancePercentage = 5;
                    FullDescription = "Increase basic weapon additional effects by {0}% on next basic attack.";
                    break;
                case CommonEnumerations.CombatActionTypes.HoneKnowledge:
                    Name = "Hone Knowledge";
                    Range = 0;
                    AdditionChancePercentage = 5;
                    FullDescription = "Increase basic weapon additional effects by {0}% on next basic attack.";
                    break;
                case CommonEnumerations.CombatActionTypes.BerserkerStrike:
                    Name = "Berserker's Strike";
                    Range = 1;
                    AdditionChancePercentage = 25;
                    FullDescription = "Strike the enemy for 3 damage, but have {0}% chance to take 1 damage.";
                    break;
                case CommonEnumerations.CombatActionTypes.SnipersShot:
                    Name = "Sniper's Shot";
                    Range = 3;
                    AdditionChancePercentage = 50;
                    FullDescription = "Strike the enemy for 3 damage, but have {0}% chance to lose all movement next turn.";
                    break;
                case CommonEnumerations.CombatActionTypes.ArcaneWisdom:
                    Name = "Arcane Wisdom";
                    Range = 3;
                    AdditionChancePercentage = 50;
                    FullDescription = "Strike the enemy for 3 damage, but have {0}% chance to lose all actions next turn.";
                    break;
                case CommonEnumerations.CombatActionTypes.ConsumeHealthPotion:
                    Name = "Consume Health Potion";
                    Range = 0;
                    AdditionChancePercentage = 0;
                    FullDescription = "Gain 2 HP.";
                    break;
                case CommonEnumerations.CombatActionTypes.DefensiveStance:
                    Name = "Defensive Stance";
                    Range = 0;
                    AdditionChancePercentage = 0;
                    FullDescription = "Negate 1 damage at the next opportunity.  -1 movement next turn.";
                    break;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public int Range
        {
            get
            {
                return _range;
            }

            set
            {
                _range = value;
            }
        }

        public string FullDescription
        {
            get
            {
                return string.Format(_fullDescription, _additionChancePercentage);
            }

            set
            {
                _fullDescription = value;
            }
        }

        public int AdditionChancePercentage
        {
            get
            {
                return _additionChancePercentage;
            }

            set
            {
                _additionChancePercentage = value;
            }
        }

        public virtual void PerformAction()
        {

        }

        public override string ToString()
        {
            return Name;
        }

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => CombatActionClicked(), _canExecute));
            }
        }

        private bool _canExecute = true;
        public void CombatActionClicked()
        {
            _ovMap.CurrentCombatLogic.ProcessCombatEntityAction(true, this);
        }
    }

    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
