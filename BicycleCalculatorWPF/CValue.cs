using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BicycleCalculatorWPF
{
    public class CValue
    {
        private int no;

        public int No
        {
            get { return no; }
            set { no = value; }
        }

        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private double val = 0;

        public double Val
        {
            get { return val; }
            set { val = value; }
        }

        public CValue(int _no, string _name, double _val)
        {
            name = _name;
            val = _val;
            no = _no;
        }
    }

    public class CValueNameObj : ICloneable
    {

        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Type = "";

        public List<CValue> vals = new List<CValue>();

        public CValueNameObj(string _type, string _name = "")
        {
            Type = _type;
            name = _name;
        }

        public object Clone()
        {
            Type t = this.GetType();
            object obj = Activator.CreateInstance(t);
            this.CopyTo((CValueNameObj)obj);
            return obj;
        }

        public override string ToString()
        {
            return name;
        }
        
        public void CopyTo(CValueNameObj pt)
        {
            pt.Name = Name;
            pt.vals.Clear();
            foreach (CValue value in vals)
                pt.vals.Add(new CValue(value.No, value.Name, value.Val));
        }
        
        public string Encode()
        {
            string str = "";
            str += Type + ",";
            str += name.Replace(",", "") + ",";
            foreach (CValue value in vals)
                str += value.Val.ToString() + ",";
            return str;
        }

        public void Decode(string str)
        {
            string[] strs = str.Split(',');
            if (strs[0] != Type) return;
            name = strs[1];
            for (int i = 2; i < strs.Length; i++)
            {
                if (strs[i] != "")
                    vals[i - 2].Val = Convert.ToDouble(strs[i]);
            }
        }
    }
}
