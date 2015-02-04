using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BicycleCalculatorWPF
{
    class CTeeth
    {
        public int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int teeth;

        public int Teeth
        {
            get { return teeth; }
            set { teeth = value; }
        }

        public CTeeth(int _id, int _teeth)
        {
            id = _id;
            teeth = _teeth;
        }
    }

    class CGear:ICloneable
    {
        public string name;
        public int num;
        //public int[] teeth = new int[12];
        public List<CTeeth> teeth = new List<CTeeth>();

        public CGear(string _name, int _num, int t1, int t2, int t3, int t4, int t5, int t6, int t7, int t8, int t9, int t10, int t11, int t12)
        {
            name = _name;
            num = _num;
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

        public override string ToString()
        {
            return name;
        }

        public object Clone()
        {
            CGear p = new CGear(name, num, teeth[0].teeth, teeth[1].teeth, teeth[2].teeth, teeth[3].teeth, teeth[4].teeth, teeth[5].teeth, teeth[6].teeth, teeth[7].teeth, teeth[8].teeth, teeth[9].teeth, teeth[10].teeth, teeth[11].teeth);
            return p;
        }
    }


    class CInnerTeeth
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

        public CInnerTeeth(int _id, double _teeth)
        {
            id = _id;
            teeth = _teeth;
        }
    }

    class CInnerGear:ICloneable
    {
        public string name;
        public int num;
        public List<CInnerTeeth> teeth = new List<CInnerTeeth>();


        public CInnerGear(string _name, int _num, double t1, double t2, double t3, double t4, double t5, double t6, double t7, double t8, double t9, double t10, double t11, double t12, double t13, double t14)
        {
            name = _name;
            num = _num;
            teeth.Add(new CInnerTeeth(1, t1));
            teeth.Add(new CInnerTeeth(2, t2));
            teeth.Add(new CInnerTeeth(3, t3));
            teeth.Add(new CInnerTeeth(4, t4));
            teeth.Add(new CInnerTeeth(5, t5));
            teeth.Add(new CInnerTeeth(6, t6));
            teeth.Add(new CInnerTeeth(7, t7));
            teeth.Add(new CInnerTeeth(8, t8));
            teeth.Add(new CInnerTeeth(9, t9));
            teeth.Add(new CInnerTeeth(10, t10));
            teeth.Add(new CInnerTeeth(11, t11));
            teeth.Add(new CInnerTeeth(12, t12));
            teeth.Add(new CInnerTeeth(13, t13));
            teeth.Add(new CInnerTeeth(14, t14));
        }

        public override string ToString()
        {
            return name;
        }

        public object Clone()
        {
            CInnerGear p = new CInnerGear(name, num, teeth[0].Teeth, teeth[1].Teeth, teeth[2].Teeth, teeth[3].Teeth, teeth[4].Teeth, teeth[5].Teeth, teeth[6].Teeth, teeth[7].Teeth, teeth[8].Teeth, teeth[9].Teeth, teeth[10].Teeth, teeth[11].Teeth, teeth[12].Teeth, teeth[13].Teeth);
            return p;
        }
    }

    class CGearList
    {
        public string name;
        public List<CGear> Gears = new List<CGear>();

        public CGearList(string _name)
        {
            name = _name;
        }

        public override string ToString()
        {
            return name;
        }
    }

    class CInnerGearList
    {
        public string name;
        public List<CInnerGear> Gears = new List<CInnerGear>();

        public CInnerGearList(string _name)
        {
            name = _name;
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
}
