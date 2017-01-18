using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BicycleCalculatorWPF
{
    public class CRim : CValueNameObj
    {
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

        public CRim()
            : base("Rim")
        {
            vals.Add(new CValue(0, "ERD", 0));
            vals.Add(new CValue(1, "OSB", 0));
        }

        public CRim(string _name, double _ERD, double _OSB)
            : base("Rim", _name)
        {
            vals.Add(new CValue(0, "ERD", _ERD));
            vals.Add(new CValue(1, "OSB", _OSB));
        }

        public CRim(string data)
            : base("Rim")
        {
            vals.Add(new CValue(0, "ERD", 0));
            vals.Add(new CValue(1, "OSB", 0));
            Decode(data);
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
