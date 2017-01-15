using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BicycleCalculatorWPF
{
    public enum GearType
    {
        Front,
        Back,
        Inner,
    }

    public class CTeeth
    {
        public int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public double teeth;

        public double Teeth
        {
            get { return teeth; }
            set { teeth = value; }
        }

        public CTeeth(int _id, double _teeth)
        {
            id = _id;
            teeth = _teeth;
        }
    }

    public class CGear:ICloneable
    {
        private GearType type;
        private string name;
        private int speeds;
        //public int[] teeth = new int[12];
        public List<CTeeth> teeth = new List<CTeeth>();
        public CGearList Parent;

        public string TeethStr
        {
            get
            {
                string str = "";
                foreach (CTeeth t in teeth)
                {
                    if (t.id <= speeds)
                        str += "[" + t.Teeth.ToString() + "] ";
                }
                return str;
            }
        }

        public GearType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

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

        public int Speeds
        {
            get
            {
                return speeds;
            }

            set
            {
                speeds = value;
            }
        }

        public CGear(GearType _type, string _name, int _speeds)
        {
            Type = _type;
            Name = _name;
            Speeds = _speeds;
        }

        public CGear(string data)
        {
            Decode(data);
        }

        public CGear(GearType _type, string _name, int _speeds, double t1, double t2, double t3, double t4, double t5, double t6, double t7, double t8, double t9, double t10, double t11, double t12)
        {
            Type = _type;
            Name = _name;
            Speeds = _speeds;
            teeth.Add(new CTeeth(1, t1));
            teeth.Add(new CTeeth(2, t2));
            teeth.Add(new CTeeth(3, t3));
            teeth.Add(new CTeeth(4, t4));
            teeth.Add(new CTeeth(5, t5));
            teeth.Add(new CTeeth(6, t6));
            teeth.Add(new CTeeth(7, t7));
            teeth.Add(new CTeeth(8, t8));
            teeth.Add(new CTeeth(9, t9));
            teeth.Add(new CTeeth(10, t10));
            teeth.Add(new CTeeth(11, t11));
            teeth.Add(new CTeeth(12, t12));
        }

        public CGear(GearType _type, string _name, int _speeds, double t1, double t2, double t3, double t4, double t5, double t6, double t7, double t8, double t9, double t10, double t11, double t12, double t13, double t14)
        {
            Type = _type;
            Name = _name;
            Speeds = _speeds;
            teeth.Add(new CTeeth(1, t1));
            teeth.Add(new CTeeth(2, t2));
            teeth.Add(new CTeeth(3, t3));
            teeth.Add(new CTeeth(4, t4));
            teeth.Add(new CTeeth(5, t5));
            teeth.Add(new CTeeth(6, t6));
            teeth.Add(new CTeeth(7, t7));
            teeth.Add(new CTeeth(8, t8));
            teeth.Add(new CTeeth(9, t9));
            teeth.Add(new CTeeth(10, t10));
            teeth.Add(new CTeeth(11, t11));
            teeth.Add(new CTeeth(12, t12));
            teeth.Add(new CTeeth(13, t13));
            teeth.Add(new CTeeth(14, t14));
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            CGear p = new CGear(Type, Name, Speeds);
            foreach (CTeeth _teeth in teeth)
                p.teeth.Add(new CTeeth(_teeth.Id, _teeth.Teeth));
            return p;
        }

        public void CopyTo(CGear gearpt)
        {
            gearpt.Name = name;
            gearpt.Speeds = speeds;
            gearpt.type = type;
            gearpt.teeth.Clear();
            gearpt.teeth.AddRange(teeth);
        }

        public string Encode()
        {
            string str = "";
            str += "Gear,";
            str += Type.ToString() + ",";
            str += Name.Replace(",", "") + ",";
            str += Speeds.ToString() + ",";
            foreach (CTeeth _teeth in teeth)
                str += _teeth.Teeth.ToString() + ",";
            return str;
        }

        public void Decode(string str)
        {
            string[] strs = str.Split(',');
            if (strs[0] != "Gear") return;
            switch(strs[1])
            {
                case "Front":
                    Type = GearType.Front;
                    break;
                case "Back":
                    Type = GearType.Back;
                    break;
                case "Inner":
                    Type = GearType.Inner;
                    break;
            }
            Name = strs[2];
            Speeds = Convert.ToInt32(strs[3]);
            teeth.Clear();
            for (int i = 4; i < strs.Length; i++)
            {
                try
                {
                    if (strs[i] == "")
                        teeth.Add(new CTeeth(i - 3, 0));
                    else
                        teeth.Add(new CTeeth(i - 3, Convert.ToDouble(strs[i])));
                }
                catch { }
            }
        }
    }

    public class CGearList
    {
        public string name;
        public int speeds = 1;
        public List<CGear> Gears = new List<CGear>();

        public CGearList(string _name, int _speeds)
        {
            name = _name;
            speeds = _speeds;
        }

        public override string ToString()
        {
            return name;
        }
    }

    class CResult
    {
        int No;

        public int No1
        {
            get { return No; }
            set { No = value; }
        }
        string Gear;

        public string Gear1
        {
            get { return Gear; }
            set { Gear = value; }
        }
        string GearT;

        public string GearT1
        {
            get { return GearT; }
            set { GearT = value; }
        }
        double GearRatio;

        public double GearRatio1
        {
            get { return GearRatio; }
            set { GearRatio = value; }
        }
        double SpeedRatio;

        public double SpeedRatio1
        {
            get { return SpeedRatio; }
            set { SpeedRatio = value; }
        }
        double Speed;

        public double Speed1
        {
            get { return Speed; }
            set { Speed = value; }
        }
        double Increment;

        public double Increment1
        {
            get { return Increment; }
            set { Increment = value; }
        }
        string Remark;

        public string Remark1
        {
            get { return Remark; }
            set { Remark = value; }
        }
    }

    public class CGearLists
    {
        public GearType type;
        public string name;
        public List<CGearList> Lists = new List<CGearList>();

        public CGearLists(string _name, GearType _type)
        {
            name = _name;
            type = _type;
            Lists = new List<CGearList>();
        }

        public override string ToString()
        {
            return name;
        }
    }
    
}
