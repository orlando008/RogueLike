using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shadows.EquipmentEnumerations;

namespace Shadows
{
    public static class EquipmentGenerationMethods
    {

        private static Equipment GenerateRandomPieceOfEquipmentBase(OverallMap m)
        {
            bool prefix = false;
            bool suffix = false;

            if (prefix == false && suffix == false)
            {
                if (m.RNG.Next(0, 2) == 0)
                    prefix = true;
                else
                    suffix = true;
            }

            EquipmentType eqType = EquipmentType.Boots;
            int maxEquipmentType = Enum.GetValues(typeof(EquipmentType)).GetLength(0);
            eqType = (EquipmentType)m.RNG.Next(0, maxEquipmentType);

            List<EquipmentPrefix> PossiblePrefixes = GetListOfAvailablePrefixes(eqType);
            List<EquipmentPrefix> PossibleSuffixes = GetListOfAvailablePrefixes(eqType);



            EquipmentPrefix eqPrefix = EquipmentPrefix.None;
            if(prefix)
            { 
                int pfIndex = m.RNG.Next(0, PossiblePrefixes.Count);
                eqPrefix = PossiblePrefixes[pfIndex];
            }

            EquipmentSuffix eqSuffix = EquipmentSuffix.None;

            if (suffix)
            {
                int sfIndex = m.RNG.Next(0, PossibleSuffixes.Count);
                eqPrefix = PossibleSuffixes[sfIndex];
            }

            if (eqType == EquipmentType.Weapon)
            {
                WeaponTypes weaponType = WeaponTypes.Dagger;
                int maxWeaponType = Enum.GetValues(typeof(WeaponTypes)).GetLength(0);
                weaponType = (WeaponTypes)m.RNG.Next(0, maxWeaponType);

                return new Weapon(eqPrefix, eqSuffix, weaponType);
            }
            else
            {
                return new Equipment(eqType, eqPrefix, eqSuffix);
            }
        }

        public static Equipment GenerateRandomPieceOfEquipment(OverallMap m)
        {
            //20% chance of prefix or suffix

            //75% chance of base line stat change
            //20% chance of mid level stat change
            //5% chance of high level stat change
            return GenerateRandomPieceOfEquipmentBase(m);
        }

        public static Equipment GenerateRandomPieceOfEquipmentBossFight(OverallMap m)
        {
            //100% chance of prefix and suffix

            //50% chance of base line stat change
            //40% chance of mid level stat change
            //10% chance of high level stat change
            return GenerateRandomPieceOfEquipmentBase(m);
        }

        public static Equipment GenerateRandomPieceOfEquipmentTreasureChest(OverallMap m)
        {
            //50% chance of both

            //80% chance of base line stat change
            //15% chance of mid level stat change
            //5% chance of high level stat change

            return GenerateRandomPieceOfEquipmentBase(m);
        }
    }
}
