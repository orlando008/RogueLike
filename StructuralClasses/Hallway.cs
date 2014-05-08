using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RogueLike.StructuralClasses
{
    public class Hallway : Point
    {
        private bool _discovered = false;

        public bool Discovered
        {
            get
            {
                return _discovered;
            }
            set
            {
                _discovered = value;
            }
        }
    }
}
