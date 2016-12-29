using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shadows.EquipmentEnumerations;

namespace Shadows
{
    public class OffHand:Equipment
    {
        private OffHandTypes _offHandType;
        private string _name;

        public OffHandTypes OffHandType
        {
            get
            {
                return _offHandType;
            }

            set
            {
                _offHandType = value;
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


        public OffHand(EquipmentPrefix ep, EquipmentSuffix es, OffHandTypes offt)
            :base(EquipmentType.OffHand, ep, es)
        {
            OffHandType = offt;
            string prefix = GetStringValueOfPrefix(ep);
            string suffix = GetStringValueOfSuffix(es);
            string name = GetWeaponName(offt);


            Name = prefix + " " + name + " " + suffix;
        }

        private string GetWeaponName(OffHandTypes wt)
        {
            string name = "";
            name = Enum.GetName(wt.GetType(), wt);

            return name;
        }
    }
}
