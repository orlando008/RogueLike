using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows
{
    public class EquipmentEnumerations
    {
        public enum EquipmentType
        {
            Shoulders = 0,
            Helmet,
            Necklace,
            Ring,
            Belt,
            Weapon,
            Chest,
            OffHand,
            Gloves,
            Pants,
            Boots
        }

        public enum WeaponTypes
        {
            Dagger = 0,
            ShortSword,
            LongSword,
            ShortBow,
            LongBow,
            CrossBow,
            Wand,
            Stave,
            Staff
        }

        public enum OffHandTypes
        {
            Buckler,
            Aegis,
            Arrows,
            Bolts,
            Tome, //Mage
            Skull //Mage
        }

        public enum ChestTypes
        {
            Armor, //Warrior
            Vest, //Rogue
            Tunic //Mage
        }

        public enum PantsTypes
        {
            Gaiters, //Warrior
            Leggings, //Rogue
            Stockings //Mage
        }

        public enum BootsTypes
        {
            Greaves, //Warrior
            Boots, //Rogue
            Slippers //Mage   
        }

        public enum GlovesTypes
        {
            Gauntlets, //Warrior
            Gloves, //Rogue
            Handwraps //Mage
        }

        public enum BeltTypes
        {
            Waistguard, //Warrior
            Sash, //Mage
            Belt //Rogue
        }

        public enum HelmetTypes
        {
            Helm, //Warrior
            Hood, //Rogue
            Diadem //Mage
        }

        public enum ShoulderTypes
        {
            Pauldrons, //Warrior
            Spaulders, //Rogue
            Mantle //Mage
        }

        public enum PrefixSuffixModifier
        {
            None,
            ChanceToStealStartingPosition,         //% chance to steal starting position
            ChanceToReRollFailedMovement,     //% change to re-roll failed movement
            ChanceToReRollFailedAction,     //% chance to re-roll failed action
            IncreaseToMovementRolls,          //% increase to movement rolls
            IncreaseToActionRolls,       //% increase to action rolls
            IncreaseToBasicAttackChance,          //% increase to basic attack extra chance
            IncreaseToMagicFind,      //% increase to magic find
            IncreaseToRollExtraExp,           //% chance to roll extra experience
            ChanceToIncreaseBattleSTR,         //% chance to increase STR 
            ChanceToIncreaseBattleDEX,          //% chance to increase DEX
            ChanceToIncreaseBattleINT,    //% chance to increase INT
            ChanceToNegateDamage,        //% chance to negate damage
            ChanceToCounterDamage,       //% chance to counter damage
            ChanceToRollExtraGold,      //% change to roll extra gold
            ChanceToDefyDeath,          //% chance to defy death
            ChanceToGainHealthOnHit,       //% chance to gain HP on hit
            ChanceToGainActionOnHit,       //% chance to gain action point on hit
            ChanceToIgnoreMovementPointLoss        //% chance to ignore movement point loss
        }

        public enum EquipmentPrefix
        {
            None = 0,
            Sneaky,        
            Successful,     
            Persistent,     
            Swift,          
            Diligent,       
            Sharp,          
            Treasured,      
            Wise,           
            Strong,         
            Agile,          
            Intelligent,    
            Shadowy,        
            Vengeful,       
            Fortunate,      
            Vital,          
            Vampiric,       
            Enduring,       
            Carefree        
        }

        public enum EquipmentSuffix
        {
            None = 0,
            OfSneaking,
            OfSuccess,
            OfPersistence,
            OfSwiftness,
            OfDiligence,
            OfSharpness,
            OfTreasure,
            OfWisdom,
            OfStrength,
            OfAgility,
            OfIntelligence,
            OfTheShadow,
            OfVengeance,
            OfFortune,
            OfVitality,
            OfVampirism,
            OfEndurance,
            OfEase
        }

        public static PrefixSuffixModifier GetBaselineModifier(EquipmentPrefix ep)
        {
            return (PrefixSuffixModifier)Convert.ToInt32(ep);
        }

        public static PrefixSuffixModifier GetBaselineModifier(EquipmentSuffix es)
        {
            return (PrefixSuffixModifier)Convert.ToInt32(es);
        }

        public static string GetModifierWithValue(PrefixSuffixModifier psm, int amount)
        {
            if (psm == PrefixSuffixModifier.None)
                return "";
            else
                return string.Format(GetBaselineModifierText(psm), amount.ToString());
        }

        public static string GetBaselineModifierText(PrefixSuffixModifier psm)
        {
            switch(psm)
            {
                case PrefixSuffixModifier.None:
                    return "";
                case PrefixSuffixModifier.ChanceToCounterDamage:
                    return "Chance to counter damage +{0}%";
                case PrefixSuffixModifier.ChanceToDefyDeath:
                    return "Chance to defy death +{0}%";
                case PrefixSuffixModifier.ChanceToGainActionOnHit:
                    return "Chance to gain 1 action when dealing damage +{0}%";
                case PrefixSuffixModifier.ChanceToGainHealthOnHit:
                    return "Chance to restore 1 HP when dealing damage +{0}%";
                case PrefixSuffixModifier.ChanceToIgnoreMovementPointLoss:
                    return "Chance to ignore movement point loss +{0}%";
                case PrefixSuffixModifier.ChanceToIncreaseBattleDEX:
                    return "Chance to increase battle DEX +{0}%";
                case PrefixSuffixModifier.ChanceToIncreaseBattleINT:
                    return "Chance to increase battle INT +{0}%";
                case PrefixSuffixModifier.ChanceToIncreaseBattleSTR:
                    return "Chance to increase battle STR +{0}%";
                case PrefixSuffixModifier.ChanceToNegateDamage:
                    return "Chance to negate 1 damage +{0}%";
                case PrefixSuffixModifier.ChanceToReRollFailedAction:
                    return "Chance to re-roll failed action roll +{0}%";
                case PrefixSuffixModifier.ChanceToReRollFailedMovement:
                    return "Chance to re-roll failed movement roll +{0}%";
                case PrefixSuffixModifier.ChanceToRollExtraGold:
                    return "Chance to roll for addition gold +{0}%";
                case PrefixSuffixModifier.ChanceToStealStartingPosition:
                    return "Chance to steal battle start +{0}%";
                case PrefixSuffixModifier.IncreaseToActionRolls:
                    return "Increase action rolls by {0}%";
                case PrefixSuffixModifier.IncreaseToBasicAttackChance:
                    return "Basic attack +{0}%";
                case PrefixSuffixModifier.IncreaseToMagicFind:
                    return "Magic Find +{0}%";
                case PrefixSuffixModifier.IncreaseToMovementRolls:
                    return "Increase movement rolls by {0}%";
                case PrefixSuffixModifier.IncreaseToRollExtraExp:
                    return "Chance to roll for additional experience point +{0}%";
                default:
                    return "";
            }
        }

        public static List<EquipmentSuffix> GetListOfAvailableSuffixes(EquipmentType equipType)
        {
            List<EquipmentSuffix> lst = new List<EquipmentSuffix>();

            foreach(EquipmentPrefix ep in GetListOfAvailablePrefixes(equipType))
            {
                lst.Add((EquipmentSuffix)Convert.ToInt32(ep));
            }
            return lst;
        }

        public static List<EquipmentPrefix> GetListOfAvailablePrefixes(EquipmentType equipType)
        {
            List<EquipmentPrefix> lst = new List<EquipmentPrefix>();

            switch(equipType)
            {
                case EquipmentType.Shoulders:
                    lst.Add(EquipmentPrefix.Persistent);
                    lst.Add(EquipmentPrefix.Swift);
                    lst.Add(EquipmentPrefix.Diligent);
                    lst.Add(EquipmentPrefix.Strong);
                    lst.Add(EquipmentPrefix.Shadowy);
                    lst.Add(EquipmentPrefix.Fortunate);
                    lst.Add(EquipmentPrefix.Vital);
                    lst.Add(EquipmentPrefix.Enduring);
                    return lst;
                case EquipmentType.Helmet:
                    lst.Add(EquipmentPrefix.Persistent);
                    lst.Add(EquipmentPrefix.Diligent);
                    lst.Add(EquipmentPrefix.Treasured);
                    lst.Add(EquipmentPrefix.Wise);
                    lst.Add(EquipmentPrefix.Intelligent);
                    lst.Add(EquipmentPrefix.Shadowy);
                    lst.Add(EquipmentPrefix.Vengeful);
                    lst.Add(EquipmentPrefix.Vital);
                    return lst;
                case EquipmentType.Necklace:
                    lst.Add(EquipmentPrefix.Sneaky);
                    lst.Add(EquipmentPrefix.Successful);
                    lst.Add(EquipmentPrefix.Swift);
                    lst.Add(EquipmentPrefix.Sharp);
                    lst.Add(EquipmentPrefix.Treasured);
                    lst.Add(EquipmentPrefix.Wise);
                    lst.Add(EquipmentPrefix.Intelligent);
                    lst.Add(EquipmentPrefix.Fortunate);
                    return lst;
                case EquipmentType.Weapon:
                    lst.Add(EquipmentPrefix.Persistent);
                    lst.Add(EquipmentPrefix.Diligent);
                    lst.Add(EquipmentPrefix.Sharp);
                    lst.Add(EquipmentPrefix.Vengeful);
                    lst.Add(EquipmentPrefix.Vampiric);
                    lst.Add(EquipmentPrefix.Enduring);
                    lst.Add(EquipmentPrefix.Carefree);
                    return lst;
                case EquipmentType.Chest:
                    lst.Add(EquipmentPrefix.Swift);
                    lst.Add(EquipmentPrefix.Wise);
                    lst.Add(EquipmentPrefix.Strong);
                    lst.Add(EquipmentPrefix.Intelligent);
                    lst.Add(EquipmentPrefix.Shadowy);
                    lst.Add(EquipmentPrefix.Vital);
                    lst.Add(EquipmentPrefix.Enduring);
                    return lst;
                case EquipmentType.OffHand:
                    lst.Add(EquipmentPrefix.Persistent);
                    lst.Add(EquipmentPrefix.Diligent);
                    lst.Add(EquipmentPrefix.Sharp);
                    lst.Add(EquipmentPrefix.Agile);
                    lst.Add(EquipmentPrefix.Shadowy);
                    lst.Add(EquipmentPrefix.Vengeful);
                    lst.Add(EquipmentPrefix.Vampiric);
                    lst.Add(EquipmentPrefix.Enduring);
                    return lst;
                case EquipmentType.Ring:
                    lst.Add(EquipmentPrefix.Sneaky);
                    lst.Add(EquipmentPrefix.Successful);
                    lst.Add(EquipmentPrefix.Sharp);
                    lst.Add(EquipmentPrefix.Treasured);
                    lst.Add(EquipmentPrefix.Intelligent);
                    lst.Add(EquipmentPrefix.Fortunate);
                    lst.Add(EquipmentPrefix.Carefree);
                    return lst;
                case EquipmentType.Belt:
                    lst.Add(EquipmentPrefix.Swift);
                    lst.Add(EquipmentPrefix.Wise);
                    lst.Add(EquipmentPrefix.Agile);
                    lst.Add(EquipmentPrefix.Shadowy);
                    lst.Add(EquipmentPrefix.Fortunate);
                    lst.Add(EquipmentPrefix.Vital);
                    lst.Add(EquipmentPrefix.Carefree);
                    return lst;
                case EquipmentType.Gloves:
                    lst.Add(EquipmentPrefix.Successful);
                    lst.Add(EquipmentPrefix.Persistent);
                    lst.Add(EquipmentPrefix.Treasured);
                    lst.Add(EquipmentPrefix.Strong);
                    lst.Add(EquipmentPrefix.Agile);
                    lst.Add(EquipmentPrefix.Vengeful);
                    lst.Add(EquipmentPrefix.Vital);
                    lst.Add(EquipmentPrefix.Carefree);
                    return lst;
                case EquipmentType.Pants:
                    lst.Add(EquipmentPrefix.Sneaky);
                    lst.Add(EquipmentPrefix.Diligent);
                    lst.Add(EquipmentPrefix.Wise);
                    lst.Add(EquipmentPrefix.Strong);
                    lst.Add(EquipmentPrefix.Agile);
                    lst.Add(EquipmentPrefix.Vital);
                    lst.Add(EquipmentPrefix.Vampiric);
                    lst.Add(EquipmentPrefix.Carefree);
                    return lst;
                case EquipmentType.Boots:
                    lst.Add(EquipmentPrefix.Sneaky);
                    lst.Add(EquipmentPrefix.Successful);
                    lst.Add(EquipmentPrefix.Swift);
                    lst.Add(EquipmentPrefix.Strong);
                    lst.Add(EquipmentPrefix.Agile);
                    lst.Add(EquipmentPrefix.Vengeful);
                    lst.Add(EquipmentPrefix.Vampiric);
                    lst.Add(EquipmentPrefix.Carefree);
                    return lst;
                default:
                    return lst;
            }
        }
    }
}
