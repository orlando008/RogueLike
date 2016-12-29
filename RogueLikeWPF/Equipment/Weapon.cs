﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shadows.EquipmentEnumerations;
using static Shadows.EquipmentGenerationMethods;

namespace Shadows
{
    public class Weapon:Equipment
    {

        private WeaponTypes _weaponType;
        private string _name;

        public WeaponTypes WeaponType
        {
            get
            {
                return _weaponType;
            }

            set
            {
                _weaponType = value;
            }
        }

        public override string Name
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

        public Weapon(EquipmentPrefix ep, EquipmentSuffix es, WeaponTypes wt)
            :base(EquipmentType.Weapon, ep, es)
        {
            WeaponType = wt;
            string prefix = GetStringValueOfPrefix(ep);
            string suffix = GetStringValueOfSuffix(es);
            string name = GetWeaponName(wt);


            Name = prefix + " " + name + " " + suffix;
        }

        private string GetWeaponName(WeaponTypes wt)
        {
            string name = "";
            name = Enum.GetName(wt.GetType(), wt);

            return name;
        }
        
    }
}