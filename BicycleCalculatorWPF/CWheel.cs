using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BicycleCalculatorWPF
{
    public class CWheel:ICloneable
    {
        private string name;
        private int lenth;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int Lenth
        {
            get
            {
                return lenth;
            }

            set
            {
                lenth = value;
            }
        }

        public CWheel(string _name, int _lenth)
        {
            Name = _name;
            Lenth = _lenth;
        }

        public CWheel(string data)
        {
            Decode(data);
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            CWheel p = new CWheel(Name, Lenth);
            return p;
        }

        public string Encode()
        {
            string str = "";
            str += "Wheel,";
            str += Name.Replace(",", "") + ",";
            str += Lenth.ToString() + ",";
            return str;
        }

        public void Decode(string str)
        {
            string[] strs = str.Split(',');
            if (strs[0] != "Wheel") return;
            Name = strs[1];
            Lenth = Convert.ToInt32(strs[2]);
        }

        public void CopyTo(CWheel pt)
        {
            pt.Lenth = lenth;
            pt.Name = name;
        }
    }
}
