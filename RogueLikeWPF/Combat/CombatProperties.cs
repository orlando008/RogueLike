using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Shadows.EquipmentEnumerations;

namespace Shadows
{
    public class CombatProperties : INotifyPropertyChanged
    {
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

        private int _percentChanceToStealStartingPosition;
        private int _percentChanceToReRollFailedMovement;
        private int _percentChanceToReRollFailedAction;
        private int _percentIncreaseToMovementRolls;
        private int _percentIncreaseToActionRolls;
        private int _percentIncreaseToBasicAttackExtraChance;
        private int _percentIncreaseToMagicFind;
        private int _percentChanceToRollExtraExperience;
        private int _percentChanceToIncreaseSTR;
        private int _percentChanceToIncreaseDEX;
        private int _percentChanceToIncreaseINT;
        private int _percentChanceToNegateDamage;
        private int _percentChanceToCounterDamage;
        private int _percentChanceToRollExtraGold;
        private int _percentChanceToDefyDeath;
        private int _percentChanceToGainHPOnHit;
        private int _percentChanceToGainActionOnHit;
        private int _percentChanceToIgnoreMovementPointLoss;

        private int _currentHP;
        private int _baseSTR;
        private int _baseDEX;
        private int _baseINT;
        private int _battleSTR;
        private int _battleDEX;
        private int _battleINT;
        private int _equipmentAdjustedSTR;
        private int _equipmentAdjustedDEX;
        private int _equipmentAdjustedINT;
        private int _currentActionPoints;
        private int _currentMovementPoints;
        private int _combatPosition;

        public int PercentChanceToStealStartingPosition
        {
            get
            {
                return _percentChanceToStealStartingPosition;
            }

            set
            {
                _percentChanceToStealStartingPosition = value;
                NotifyPropertyChanged(nameof(PercentChanceToStealStartingPosition));
            }
        }

        public int PercentChanceToReRollFailedMovement
        {
            get
            {
                return _percentChanceToReRollFailedMovement;
            }

            set
            {
                _percentChanceToReRollFailedMovement = value;
                NotifyPropertyChanged(nameof(PercentChanceToReRollFailedMovement));
            }
        }

        public int PercentChanceToReRollFailedAction
        {
            get
            {
                return _percentChanceToReRollFailedAction;
            }

            set
            {
                _percentChanceToReRollFailedAction = value;
                NotifyPropertyChanged(nameof(PercentChanceToReRollFailedAction));
            }
        }

        public int PercentIncreaseToMovementRolls
        {
            get
            {
                return _percentIncreaseToMovementRolls;
            }

            set
            {
                _percentIncreaseToMovementRolls = value;
                NotifyPropertyChanged(nameof(PercentIncreaseToMovementRolls));
            }
        }

        public int PercentIncreaseToActionRolls
        {
            get
            {
                return _percentIncreaseToActionRolls;
            }

            set
            {
                _percentIncreaseToActionRolls = value;
                NotifyPropertyChanged(nameof(PercentIncreaseToActionRolls));
            }
        }

        public int PercentIncreaseToBasicAttackExtraChance
        {
            get
            {
                return _percentIncreaseToBasicAttackExtraChance;
            }

            set
            {
                _percentIncreaseToBasicAttackExtraChance = value;
                NotifyPropertyChanged(nameof(PercentIncreaseToBasicAttackExtraChance));
            }
        }

        public int PercentIncreaseToMagicFind
        {
            get
            {
                return _percentIncreaseToMagicFind;
            }

            set
            {
                _percentIncreaseToMagicFind = value;
                NotifyPropertyChanged(nameof(PercentIncreaseToMagicFind));
            }
        }

        public int PercentChanceToRollExtraExperience
        {
            get
            {
                return _percentChanceToRollExtraExperience;
            }

            set
            {
                _percentChanceToRollExtraExperience = value;
                NotifyPropertyChanged(nameof(PercentChanceToRollExtraExperience));
            }
        }

        public int PercentChanceToIncreaseSTR
        {
            get
            {
                return _percentChanceToIncreaseSTR;
            }

            set
            {
                _percentChanceToIncreaseSTR = value;
                NotifyPropertyChanged(nameof(PercentChanceToIncreaseSTR));
            }
        }

        public int PercentChanceToIncreaseDEX
        {
            get
            {
                return _percentChanceToIncreaseDEX;
            }

            set
            {
                _percentChanceToIncreaseDEX = value;
                NotifyPropertyChanged(nameof(PercentChanceToIncreaseDEX));
            }
        }

        public int PercentChanceToIncreaseINT
        {
            get
            {
                return _percentChanceToIncreaseINT;
            }

            set
            {
                _percentChanceToIncreaseINT = value;
                NotifyPropertyChanged(nameof(PercentChanceToIncreaseINT));
            }
        }

        public int PercentChanceToNegateDamage
        {
            get
            {
                return _percentChanceToNegateDamage;
            }

            set
            {
                _percentChanceToNegateDamage = value;
                NotifyPropertyChanged(nameof(PercentChanceToNegateDamage));
            }
        }

        public int PercentChanceToCounterDamage
        {
            get
            {
                return _percentChanceToCounterDamage;
            }

            set
            {
                _percentChanceToCounterDamage = value;
                NotifyPropertyChanged(nameof(PercentChanceToCounterDamage));
            }
        }

        public int PercentChanceToRollExtraGold
        {
            get
            {
                return _percentChanceToRollExtraGold;
            }

            set
            {
                _percentChanceToRollExtraGold = value;
                NotifyPropertyChanged(nameof(PercentChanceToRollExtraGold));
            }
        }

        public int PercentChanceToDefyDeath
        {
            get
            {
                return _percentChanceToDefyDeath;
            }

            set
            {
                _percentChanceToDefyDeath = value;
                NotifyPropertyChanged(nameof(PercentChanceToDefyDeath));
            }
        }

        public int PercentChanceToGainHPOnHit
        {
            get
            {
                return _percentChanceToGainHPOnHit;
            }

            set
            {
                _percentChanceToGainHPOnHit = value;
                NotifyPropertyChanged(nameof(PercentChanceToGainHPOnHit));
            }
        }

        public int PercentChanceToIgnoreMovementPointLoss
        {
            get
            {
                return _percentChanceToIgnoreMovementPointLoss;
            }

            set
            {
                _percentChanceToIgnoreMovementPointLoss = value;
                NotifyPropertyChanged(nameof(PercentChanceToIgnoreMovementPointLoss));
            }
        }

        public int CurrentHP
        {
            get
            {
                return _currentHP;
            }

            set
            {
                _currentHP = value;
                NotifyPropertyChanged(nameof(CurrentHP));
            }
        }

        public int BaseSTR
        {
            get
            {
                return _baseSTR;
            }

            set
            {
                _baseSTR = value;
                NotifyPropertyChanged(nameof(BaseSTR));
            }
        }

        public int BaseDEX
        {
            get
            {
                return _baseDEX;
            }

            set
            {
                _baseDEX = value;
                NotifyPropertyChanged(nameof(BaseDEX));
            }
        }

        public int BaseINT
        {
            get
            {
                return _baseINT;
            }

            set
            {
                _baseINT = value;
                NotifyPropertyChanged(nameof(BaseINT));
            }
        }

        public int BattleSTR
        {
            get
            {
                return _battleSTR;
            }

            set
            {
                _battleSTR = value;
                NotifyPropertyChanged(nameof(BattleSTR));
            }
        }

        public int BattleDEX
        {
            get
            {
                return _battleDEX;
            }

            set
            {
                _battleDEX = value;
                NotifyPropertyChanged(nameof(BattleDEX));
            }
        }

        public int BattleINT
        {
            get
            {
                return _battleINT;
            }

            set
            {
                _battleINT = value;
                NotifyPropertyChanged(nameof(BattleINT));
            }
        }

        public int EquipmentAdjustedSTR
        {
            get
            {
                return _equipmentAdjustedSTR;
            }

            set
            {
                _equipmentAdjustedSTR = value;
                NotifyPropertyChanged(nameof(EquipmentAdjustedSTR));
            }
        }

        public int EquipmentAdjustedDEX
        {
            get
            {
                return _equipmentAdjustedDEX;
            }

            set
            {
                _equipmentAdjustedDEX = value;
                NotifyPropertyChanged(nameof(EquipmentAdjustedDEX));
            }
        }

        public int EquipmentAdjustedINT
        {
            get
            {
                return _equipmentAdjustedINT;
            }

            set
            {
                _equipmentAdjustedINT = value;
                NotifyPropertyChanged(nameof(EquipmentAdjustedINT));
            }
        }

        public int CurrentActionPoints
        {
            get
            {
                return _currentActionPoints;
            }

            set
            {
                _currentActionPoints = value;
                NotifyPropertyChanged(nameof(CurrentActionPoints));
            }
        }

        public int CurrentMovementPoints
        {
            get
            {
                return _currentMovementPoints;
            }

            set
            {
                _currentMovementPoints = value;
                NotifyPropertyChanged(nameof(CurrentMovementPoints));
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
                NotifyPropertyChanged(nameof(CombatPosition));
            }
        }

        public int PercentChanceToGainActionOnHit
        {
            get
            {
                return _percentChanceToGainActionOnHit;
            }

            set
            {
                _percentChanceToGainActionOnHit = value;
                NotifyPropertyChanged(nameof(PercentChanceToGainActionOnHit));
            }
        }

        public CombatProperties()
        {

        }

        public void SetValueOfPropertyBasedOnModifier(EquipmentEnumerations.PrefixSuffixModifier psm, int value)
        {
            switch (psm)
            {
                case PrefixSuffixModifier.None:
                    return;
                case PrefixSuffixModifier.ChanceToCounterDamage:
                    PercentChanceToCounterDamage = value;
                    return;
                case PrefixSuffixModifier.ChanceToDefyDeath:
                    PercentChanceToDefyDeath = value;
                    return;
                case PrefixSuffixModifier.ChanceToGainActionOnHit:
                    PercentChanceToGainActionOnHit = value;
                    return;
                case PrefixSuffixModifier.ChanceToGainHealthOnHit:
                    PercentChanceToGainHPOnHit = value;
                    return;
                case PrefixSuffixModifier.ChanceToIgnoreMovementPointLoss:
                    PercentChanceToIgnoreMovementPointLoss = value;
                    return;
                case PrefixSuffixModifier.ChanceToIncreaseBattleDEX:
                    PercentChanceToIncreaseDEX = value;
                    return;
                case PrefixSuffixModifier.ChanceToIncreaseBattleINT:
                    PercentChanceToIncreaseINT = value;
                    return;
                case PrefixSuffixModifier.ChanceToIncreaseBattleSTR:
                    PercentChanceToIncreaseSTR = value;
                    return;
                case PrefixSuffixModifier.ChanceToNegateDamage:
                    PercentChanceToNegateDamage = value;
                    return;
                case PrefixSuffixModifier.ChanceToReRollFailedAction:
                    PercentChanceToReRollFailedAction = value;
                    return;
                case PrefixSuffixModifier.ChanceToReRollFailedMovement:
                    PercentChanceToReRollFailedMovement = value;
                    return;
                case PrefixSuffixModifier.ChanceToRollExtraGold:
                    PercentChanceToRollExtraGold = value;
                    return;
                case PrefixSuffixModifier.ChanceToStealStartingPosition:
                    PercentChanceToStealStartingPosition = value;
                    return;
                case PrefixSuffixModifier.IncreaseToActionRolls:
                    PercentIncreaseToActionRolls = value;
                    return;
                case PrefixSuffixModifier.IncreaseToBasicAttackChance:
                    PercentIncreaseToBasicAttackExtraChance = value;
                    return;
                case PrefixSuffixModifier.IncreaseToMagicFind:
                    PercentIncreaseToMagicFind = value;
                    return;
                case PrefixSuffixModifier.IncreaseToMovementRolls:
                    PercentIncreaseToMovementRolls = value;
                    return;
                case PrefixSuffixModifier.IncreaseToRollExtraExp:
                    PercentChanceToRollExtraExperience = value;
                    return;
                default:
                    return;
            }
        }
    }
}
