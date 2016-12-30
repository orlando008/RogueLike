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

        public enum EquipmentPrefix
        {
            None = 0,
            Strong, //1xStrength
            Tempered, //2xStrength
            Powerful, //3xStrength
            Scholarly, //1xIntellect
            Mystical, //2xIntellect
            Arcane, //3xIntellect
            Acute, //1xDexterity
            Sharp, //2xDexterity
            Keen //3xDexterity
        }

        public enum EquipmentSuffix
        {
            None = 0,
            OfForce, //1xStrength
            OfMight, //2xStrength
            OfSupremacy, //3xStrength
            OfCuriosity, //1xIntellect
            OfKnowledge, //2xIntellect
            OfWisdom, //3xIntellect
            OfQuickness, //1xDexterity
            OfHastening, //2xDexterity
            OfSwiftness //3xDexterity
        }
    }
}
