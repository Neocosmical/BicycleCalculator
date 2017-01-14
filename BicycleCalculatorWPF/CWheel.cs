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

        public CWheel(string data)
        {
            Decode(data);
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

        public string Encode()
        {
            string str = "";
            str += "Wheel,";
            str += name.Replace(",", "") + ",";
            str += lenth.ToString() + ",";
            return str;
        }

        public void Decode(string str)
        {
            string[] strs = str.Split(',');
            if (strs[0] != "Wheel") return;
            name = strs[1];
            lenth = Convert.ToInt32(strs[2]);
        }
    }
}
