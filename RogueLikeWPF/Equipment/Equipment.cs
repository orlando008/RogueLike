using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shadows.EquipmentEnumerations;
using static Shadows.EquipmentGenerationMethods;

namespace Shadows
{
    public class Equipment
    {
        private EquipmentType _equipmentType;
        private EquipmentPrefix _equipmentPrefix;
        private EquipmentSuffix _equipmentSuffix;
        private string _name;
        private bool _equipped;
        private Uri _imageSource;
        private int _prefixValue;
        private int _suffixValue;

        public virtual string Name
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

        public virtual Uri ImageSource
        {
            get
            {
                return _imageSource;
            }

            set
            {
                _imageSource = value;
            }
        }

        public EquipmentType EquipmentType1
        {
            get
            {
                return _equipmentType;
            }

            set
            {
                _equipmentType = value;
            }
        }

        public bool Equipped
        {
            get
            {
                return _equipped;
            }

            set
            {
                _equipped = value;
            }
        }

        public EquipmentPrefix EquipmentPrefix1
        {
            get
            {
                return _equipmentPrefix;
            }

            set
            {
                _equipmentPrefix = value;
            }
        }

        public EquipmentSuffix EquipmentSuffix1
        {
            get
            {
                return _equipmentSuffix;
            }

            set
            {
                _equipmentSuffix = value;
            }
        }

        public int PrefixValue
        {
            get
            {
                return _prefixValue;
            }

            set
            {
                _prefixValue = value;
            }
        }

        public int SuffixValue
        {
            get
            {
                return _suffixValue;
            }

            set
            {
                _suffixValue = value;
            }
        }

        public Equipment(EquipmentType et, EquipmentPrefix eprefix, EquipmentSuffix esuffix)
        {
            EquipmentType1 = et;
            EquipmentPrefix1 = eprefix;
            EquipmentSuffix1 = esuffix;

            string prefix = GetStringValueOfPrefix(eprefix);
            string suffix = GetStringValueOfSuffix(esuffix);
            string name = GetStringValueEquipmentType(et);


            Name = prefix + " " + name + " " + suffix;
            Equipped = false;

            _imageSource = new Uri("pack://application:,,,/Images/Weapons/dagger.png");
        }

        protected string GetStringValueOfPrefix(EquipmentPrefix eprefix)
        {
            string prefix = "";
            if (eprefix != EquipmentPrefix.None)
            {
                prefix = Enum.GetName(eprefix.GetType(), eprefix);
            }

            return prefix;
        }

        protected string GetStringValueOfSuffix(EquipmentSuffix esuffix)
        {
            string suffix = "";
            if (esuffix != EquipmentSuffix.None)
            {
                suffix = Enum.GetName(esuffix.GetType(), esuffix);
                suffix = suffix.Replace("Of", "Of ");
            }
            
            return suffix;
        }

        protected string GetStringValueEquipmentType(EquipmentType etype)
        {
            string name = "";

            if(etype == EquipmentType.OffHand)
            {
                name = "";
            }
            else if(etype == EquipmentType.Weapon)
            {
                name = "";
            }
            else
            {
                name = Enum.GetName(etype.GetType(), etype);
            }
            

            return name;
        }

    }
}
