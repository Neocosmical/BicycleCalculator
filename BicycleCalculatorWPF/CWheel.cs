using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BicycleCalculatorWPF
{
    class CWheel:ICloneable
    {
        public string name;
        public int lenth;

        public CWheel(string _name, int _lenth)
        {
            name = _name;
            lenth = _lenth;
        }

        public override string ToString()
        {
            return name;
        }

        public object Clone()
        {
            CWheel p = new CWheel(name, lenth);
            return p;
        }
    }
}
