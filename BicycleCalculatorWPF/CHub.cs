using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BicycleCalculatorWPF
{
    class CHub : ICloneable
    {
        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<CValue> vals = new List<CValue>();

        public double LeftFlange
        {
            get { return vals[0].Val; }
            set { vals[0].Val = value; }
        }

        public double RightFlange
        {
            get { return vals[1].Val; }
            set { vals[1].Val = value; }
        }

        public double CenterToLeft
        {
            get { return vals[2].Val; }
            set { vals[2].Val = value; }
        }

        public double CenterToRight
        {
            get { return vals[3].Val; }
            set { vals[3].Val = value; }
        }

        public double SpokeHole
        {
            get { return vals[4].Val; }
            set { vals[4].Val = value; }
        }


        public CHub(string _name, double _leftFlange, double _rightFlange, double _centerToLeft, double _centerToRight, double _spokeHole)
        {
            name = _name;
            vals.Add(new CValue(0, Properties.Resources.StringFlangeL, _leftFlange));
            vals.Add(new CValue(1, Properties.Resources.StringFlangeR, _rightFlange));
            vals.Add(new CValue(2, Properties.Resources.StringCenterL, _centerToLeft));
            vals.Add(new CValue(3, Properties.Resources.StringCenterR, _centerToRight));
            vals.Add(new CValue(4, Properties.Resources.StringSpokeHole, _spokeHole));
        }

        public CHub(string data)
        {
            vals.Add(new CValue(0, Properties.Resources.StringFlangeL, 0));
            vals.Add(new CValue(1, Properties.Resources.StringFlangeR, 0));
            vals.Add(new CValue(2, Properties.Resources.StringCenterL, 0));
            vals.Add(new CValue(3, Properties.Resources.StringCenterR, 0));
            vals.Add(new CValue(4, Properties.Resources.StringSpokeHole, 0));
            Decode(data);
        }

        public override string ToString()
        {
            return name;
        }

        public object Clone()
        {
            CHub p = new CHub(name, LeftFlange, RightFlange, CenterToLeft, CenterToRight, SpokeHole);
            return p;
        }

        public string Encode()
        {
            string str = "";
            str += "Hub,";
            str += name.Replace(",","") + ",";
            str += LeftFlange.ToString() + ",";
            str += RightFlange.ToString() + ",";
            str += CenterToLeft.ToString() + ",";
            str += CenterToRight.ToString() + ",";
            str += SpokeHole.ToString() + ",";
            return str;
        }

        public void Decode(string str)
        {
            string[] strs = str.Split(',');
            if (strs[0] != "Hub") return;
            name = strs[1];
            LeftFlange = Convert.ToDouble(strs[2]);
            RightFlange = Convert.ToDouble(strs[3]);
            CenterToLeft = Convert.ToDouble(strs[4]);
            CenterToRight = Convert.ToDouble(strs[5]);
            SpokeHole = Convert.ToDouble(strs[6]);
        }
    }
    class CHubList
    {
        string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<CHub> list = new List<CHub>();

        public CHubList(string _name)
        {
            name = _name;
        }

        public override string ToString()
        {
            return name;
        }
    }

    class CSpokeResult
    {
        private int crosses = 0;

        public int Crosses
        {
            get { return crosses; }
            set { crosses = value; }
        }

        double lenthleft = 0;

        public double Lenthleft
        {
            get { return lenthleft; }
            set { lenthleft = value; }
        }

        double lenthright = 0;

        public double Lenthright
        {
            get { return lenthright; }
            set { lenthright = value; }
        }

        double tensionratio = 1;

        public double Tensionratio
        {
            get { return tensionratio; }
            set { tensionratio = value; }
        }

        private string remark;

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public CSpokeResult()
        {

        }
    }
}
