using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BicycleCalculatorWPF
{
    public class CRim : ICloneable
    {
        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<CValue> vals = new List<CValue>();

        public double ERD
        {
            get { return vals[0].Val; }
            set { vals[0].Val = value; }
        }

        public double OSB
        {
            get { return vals[1].Val; }
            set { vals[1].Val = value; }
        }


        public CRim(string _name, double _ERD, double _OSB)
        {
            name = _name;
            vals.Add(new CValue(0, "ERD", _ERD));
            vals.Add(new CValue(1, "OSB", _OSB));

        }

        public CRim(string data)
        {
            vals.Add(new CValue(0, "ERD", 0));
            vals.Add(new CValue(1, "OSB", 0));
            Decode(data);
        }


        public override string ToString()
        {
            return name;
        }

        public object Clone()
        {
            CRim p = new CRim(name, ERD, OSB);
            return p;
        }

        public string Encode()
        {
            string str = "";
            str += "Rim,";
            str += name.Replace(",", "") + ",";
            str += ERD.ToString() + ",";
            str += OSB.ToString() + ",";
            return str;
        }

        public void Decode(string str)
        {
            string[] strs = str.Split(',');
            if (strs[0] != "Rim") return;
            name = strs[1];
            ERD = Convert.ToDouble(strs[2]);
            OSB = Convert.ToDouble(strs[3]);
        }
    }

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

    class CRimList
    {
        string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<CRim> list = new List<CRim>();

        public CRimList(string _name)
        {
            name = _name;
        }

        public override string ToString()
        {
            return name;
        }
    }

}
