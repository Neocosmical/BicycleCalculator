using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;
using System.ComponentModel;

namespace BicycleCalculatorWPF
{
    class CTransmissionCal : INotifyPropertyChanged
    {
        public bool ready = false;
        public CGear frnow;
        public CGear bknow;
        public CGear innow;
        public CWheel whnow;
        public int whnow_index;

        private int whlength;

        public PlotModel pm;
        public LineSeries lineSeriesCurrent;
        private double cad;
        private bool isSpd;
        private bool isISO;
        private double tireISO1;
        private double tireISO2;
        private bool isMPH;
        private int curveX = 0;
        private int curveY = 1;


        public int Whlength
        {
            get
            {
                return whlength;
            }
            set
            {
                if (whlength != value)
                {
                    whlength = value;
                    NotifyPropertyChanged("Whlength");
                    Calculate();
                }
            }
        }

        public double TireISO1
        {
            get
            {
                return tireISO1;
            }

            set
            {
                if (tireISO1 != value)
                {
                    tireISO1 = value;
                    NotifyPropertyChanged("TireISO1");
                    CalculateWheelLenth();
                }
            }
        }

        public double TireISO2
        {
            get
            {
                return tireISO2;
            }

            set
            {
                if (tireISO2 != value)
                {
                    tireISO2 = value;
                    NotifyPropertyChanged("TireISO2");
                    CalculateWheelLenth();
                }
            }
        }

        public bool IsISO
        {
            get
            {
                return isISO;
            }

            set
            {
                if (isISO != value)
                {
                    isISO = value;
                    NotifyPropertyChanged("IsISO");
                    CalculateWheelLenth();
                }
            }
        }

        public double Cad
        {
            get
            {
                return cad;
            }

            set
            {
                cad = value;
                NotifyPropertyChanged("Cad");
                NotifyPropertyChanged("CadStr");
                Calculate();
            }
        }

        public string CadStr
        {
            get
            {
                string str = "";
                if (IsSpd)
                {
                    if (IsMPH)
                        str = (Cad / 2.0).ToString("0.0") + " mph";
                    else
                        str = (Cad / 2.0).ToString("0.0") + " km/h";
                }
                else
                {
                    str = Cad.ToString("0.0") + " rpm";
                }
                return str;
            }
        }

        public string SpeedHeaderStr
        {
            get
            {
                string str = "";
                if (IsSpd)
                {
                    str = Properties.Resources.StringCAD + " rpm";
                }
                else
                {
                    if (IsMPH)
                        str = Properties.Resources.StringSpeed + " mph";
                    else
                        str = Properties.Resources.StringSpeed + " km/h";
                }
                return str;
            }
        }

        public bool IsMPH
        {
            get
            {
                return isMPH;
            }

            set
            {
                isMPH = value;
                NotifyPropertyChanged("IsMPH");
                NotifyPropertyChanged("CadStr");
                NotifyPropertyChanged("SpeedHeaderStr");
                Calculate();
            }
        }

        public bool IsSpd
        {
            get
            {
                return isSpd;
            }

            set
            {
                isSpd = value;
                NotifyPropertyChanged("IsSpd");
                NotifyPropertyChanged("SpeedHeaderStr");
                Cad = isSpd ? 50 : 80;
                Calculate();
            }
        }

        public int CurveX
        {
            get
            {
                return curveX;
            }

            set
            {
                curveX = value;
                Calculate();
            }
        }

        public int CurveY
        {
            get
            {
                return curveY;
            }

            set
            {
                curveY = value;
                Calculate();
            }
        }

        OxyPlot.Wpf.PlotView chart;
        System.Windows.Controls.ListView list;
        System.Windows.Controls.Label label0;
        System.Windows.Controls.Label label1;

        public event PropertyChangedEventHandler PropertyChanged;



        public static PlotModel LineSeries()
        {
            var plotModel1 = new PlotModel();
            plotModel1.LegendSymbolLength = 24;
            plotModel1.LegendPosition = LegendPosition.LeftTop;
            //plotModel1.Title = "LineSeries";
            var linearAxis1 = new LinearAxis();
            linearAxis1.MajorGridlineStyle = LineStyle.Solid;
            //linearAxis1.MaximumPadding = 0;
            //linearAxis1.MinimumPadding = 0;
            linearAxis1.MinorGridlineStyle = LineStyle.Dot;
            linearAxis1.Position = AxisPosition.Bottom;
            linearAxis1.TickStyle = TickStyle.Inside;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.MajorGridlineStyle = LineStyle.Solid;
            linearAxis2.TickStyle = TickStyle.Inside;
            //linearAxis2.MaximumPadding = 0;
            //linearAxis2.MinimumPadding = 0;
            linearAxis2.MinorGridlineStyle = LineStyle.Dot;
            plotModel1.Axes.Add(linearAxis2);
            plotModel1.PlotAreaBackground = OxyColor.FromArgb(220, 255, 255, 255);
            plotModel1.LegendBorder = OxyColors.Black;
            plotModel1.LegendBorderThickness = 1;
            plotModel1.LegendBackground = OxyColor.FromArgb(200, 255, 255, 255);
            return plotModel1;
        }

        public CTransmissionCal()
        {
            
        }

        public void Init(OxyPlot.Wpf.PlotView _chart, System.Windows.Controls.ListView _list, System.Windows.Controls.Label _label0, System.Windows.Controls.Label _label1)
        {
            chart = _chart;
            list = _list;
            label0 = _label0;
            label1 = _label1;
            lineSeriesCurrent = new LineSeries();
            pm = new PlotModel();
            pm = LineSeries();
            chart.Model = pm;

            lineSeriesCurrent.Color = OxyColors.SkyBlue;
            lineSeriesCurrent.MarkerFill = OxyColors.SkyBlue;
            lineSeriesCurrent.MarkerSize = 4;
            lineSeriesCurrent.MarkerStroke = OxyColors.White;
            lineSeriesCurrent.MarkerStrokeThickness = 1.5;
            lineSeriesCurrent.MarkerType = MarkerType.Circle;
            lineSeriesCurrent.Title = Properties.Resources.StringCurrent;
            pm.Series.Add(lineSeriesCurrent);

            pm.Axes[1].Title = Properties.Resources.StringSpdRatio;
            pm.Axes[0].Title = Properties.Resources.StringGear + " No.";


            frnow.teeth[0].Teeth = Properties.Settings.Default.frnow1;
            frnow.teeth[1].Teeth = Properties.Settings.Default.frnow2;
            frnow.teeth[2].Teeth = Properties.Settings.Default.frnow3;
            frnow.teeth[3].Teeth = Properties.Settings.Default.frnow4;
            frnow.teeth[4].Teeth = Properties.Settings.Default.frnow5;
            frnow.teeth[5].Teeth = Properties.Settings.Default.frnow6;
            frnow.teeth[6].Teeth = Properties.Settings.Default.frnow7;
            frnow.teeth[7].Teeth = Properties.Settings.Default.frnow8;
            frnow.teeth[8].Teeth = Properties.Settings.Default.frnow9;
            frnow.teeth[9].Teeth = Properties.Settings.Default.frnow10;
            frnow.teeth[10].Teeth = Properties.Settings.Default.frnow11;
            frnow.teeth[11].Teeth = Properties.Settings.Default.frnow12;
            for (int i = 0; i < 12; i++)
                if (frnow.teeth[i].Teeth == 0) frnow.teeth[i].Teeth = 1;

            bknow.teeth[0].Teeth = Properties.Settings.Default.bknow1;
            bknow.teeth[1].Teeth = Properties.Settings.Default.bknow2;
            bknow.teeth[2].Teeth = Properties.Settings.Default.bknow3;
            bknow.teeth[3].Teeth = Properties.Settings.Default.bknow4;
            bknow.teeth[4].Teeth = Properties.Settings.Default.bknow5;
            bknow.teeth[5].Teeth = Properties.Settings.Default.bknow6;
            bknow.teeth[6].Teeth = Properties.Settings.Default.bknow7;
            bknow.teeth[7].Teeth = Properties.Settings.Default.bknow8;
            bknow.teeth[8].Teeth = Properties.Settings.Default.bknow9;
            bknow.teeth[9].Teeth = Properties.Settings.Default.bknow10;
            bknow.teeth[10].Teeth = Properties.Settings.Default.bknow11;
            bknow.teeth[11].Teeth = Properties.Settings.Default.bknow12;
            for (int i = 0; i < 12; i++)
                if (bknow.teeth[i].Teeth == 0) bknow.teeth[i].Teeth = 1;

            innow.teeth[0].Teeth = Properties.Settings.Default.innow1;
            innow.teeth[1].Teeth = Properties.Settings.Default.innow2;
            innow.teeth[2].Teeth = Properties.Settings.Default.innow3;
            innow.teeth[3].Teeth = Properties.Settings.Default.innow4;
            innow.teeth[4].Teeth = Properties.Settings.Default.innow5;
            innow.teeth[5].Teeth = Properties.Settings.Default.innow6;
            innow.teeth[6].Teeth = Properties.Settings.Default.innow7;
            innow.teeth[7].Teeth = Properties.Settings.Default.innow8;
            innow.teeth[8].Teeth = Properties.Settings.Default.innow9;
            innow.teeth[9].Teeth = Properties.Settings.Default.innow10;
            innow.teeth[10].Teeth = Properties.Settings.Default.innow11;
            innow.teeth[11].Teeth = Properties.Settings.Default.innow12;
            innow.teeth[12].Teeth = Properties.Settings.Default.innow13;
            innow.teeth[13].Teeth = Properties.Settings.Default.innow14;
            for (int i = 0; i < 14; i++)
                if (innow.teeth[i].Teeth == 0) innow.teeth[i].Teeth = 1;

            Whlength = Properties.Settings.Default.wheellen;
            TireISO1 = Properties.Settings.Default.numUD1;
            TireISO2 = Properties.Settings.Default.numUD2;
            IsMPH = Properties.Settings.Default.IsMPH;
            Cad = Properties.Settings.Default.SpdVal;
            IsSpd = Properties.Settings.Default.IsSpd;
            IsISO = Properties.Settings.Default.IsISO;
        }

        double toothrateold = 0;
        List<CResult> results = new List<CResult>();
        public List<int> NaNNumber = new List<int>();


        public void Calculate()
        {
            if (!ready) return;
            pm.Axes[0].MinorStep = double.NaN;
            pm.Axes[0].MajorStep = double.NaN;
            CGear frtemp = frnow;
            CGear bktemp = bknow;
            CGear intemp = innow;
            CWheel whtemp = (CWheel)whnow.Clone();
            whtemp.Lenth = Whlength;

            list.Items.Clear();

            lineSeriesCurrent.Points.Clear();
            double toothratemax = 0;
            double toothratemin = 0;

            int num = 1;
            NaNNumber.Clear();
            for (int i = 0; i < frtemp.Speeds; i++)
            {
                for (int k = 0; k < intemp.Speeds; k++)
                {
                    for (int j = 0; j < bktemp.Speeds; j++)
                    {
                        CResult resulttemp = new CResult();
                        resulttemp.No1 = num;
                        num++;
                        resulttemp.Gear1 = "";
                        resulttemp.GearT1 = "";
                        if (frtemp.Speeds != 1)
                        {
                            resulttemp.Gear1 += (i + 1).ToString();
                            resulttemp.GearT1 += frtemp.teeth[i].teeth.ToString() + "T";
                        }
                        if (bktemp.Speeds != 1)
                        {
                            if (resulttemp.Gear1 != "")
                            {
                                resulttemp.Gear1 += "x";
                                resulttemp.GearT1 += "x";
                            }
                            resulttemp.Gear1 += (j + 1).ToString();
                            resulttemp.GearT1 += bktemp.teeth[j].teeth.ToString() + "T";
                        }
                        if (intemp.Speeds != 1)
                        {
                            if (resulttemp.Gear1 != "")
                            {
                                resulttemp.Gear1 += "x";
                                resulttemp.GearT1 += "x";
                            }
                            resulttemp.Gear1 += (k + 1).ToString();
                            resulttemp.GearT1 += intemp.teeth[k].Teeth.ToString("0.00");
                        }
                        double toothrateout = (double)frtemp.teeth[i].teeth / (double)bktemp.teeth[j].teeth;
                        double toothrate = toothrateout * intemp.teeth[k].teeth;

                        if (toothratemin == 0) toothratemin = toothrate;
                        if (toothratemax == 0) toothratemax = toothrate;
                        if (toothrate < toothratemin) toothratemin = toothrate;
                        if (toothrate > toothratemax) toothratemax = toothrate;

                        resulttemp.GearRatio1 = toothrate;
                        resulttemp.SpeedRatio1 = toothrate * whtemp.Lenth / 3.1415926535 / 25.4;

                        if (IsSpd)
                        {
                            if (IsMPH)
                                resulttemp.Speed1 = Cad * 1.609344 / 2.0 * 1000000.0 / 60.0 / whtemp.Lenth / toothrate;
                            else
                                resulttemp.Speed1 = Cad / 2.0 * 1000000.0 / 60.0 / whtemp.Lenth / toothrate;
                        }
                        else
                        {
                            if (IsMPH)
                                resulttemp.Speed1 = toothrate * Cad * whtemp.Lenth * 60 / 1000000.0 * 0.6213712;
                            else
                                resulttemp.Speed1 = toothrate * Cad * whtemp.Lenth * 60 / 1000000.0;
                        }

                        if (num >= 3)
                            resulttemp.Increment1 = toothrate / toothrateold - 1.0;
                        toothrateold = toothrate;

                        resulttemp.Remark1 = "";
                        if (frtemp.Speeds != 1 && bktemp.Speeds != 1)
                        {
                            if (i == frtemp.Speeds - 1 && j == 0) resulttemp.Remark1 += Properties.Resources.StringLFLB;
                            if (i == 0 && j == bktemp.Speeds - 1) resulttemp.Remark1 += Properties.Resources.StringSFSB;
                        }

                        if (intemp.Speeds != 1)
                        {
                            if (toothrateout < 1.5) resulttemp.Remark1 += Properties.Resources.StringTorque;
                        }
                        results.Add(resulttemp);
                        list.Items.Add(resulttemp);


                        double tempx = 0;
                        double tempy = 0;
                        switch (CurveX)
                        {
                            case 0://不分支
                                tempx = num - 1;
                                break;
                            case 1://按牙盘
                                tempx = bktemp.Speeds * k + j + 1;
                                break;
                            case 2://按飞轮
                                tempx = i * frtemp.Speeds + k + 1;
                                break;
                            case 3://按内变速
                                tempx = i * bktemp.Speeds + j + 1;
                                break;
                        }


                        switch (CurveY)
                        {
                            case 1://走距速比
                                tempy = toothrate * whtemp.Lenth / 1000.0;
                                break;
                            case 2://GI速比
                                tempy = toothrate * whtemp.Lenth / 3.1415926535 / 25.4;
                                break;
                            case 3://齿比
                                tempy = toothrate;
                                break;
                            case 4://车速踏频
                                tempy = resulttemp.Speed1;
                                break;
                        }
                        if (lineSeriesCurrent.Points.Count >= 2)
                            if (lineSeriesCurrent.Points[lineSeriesCurrent.Points.Count - 2].X != double.NaN)
                                if (tempx - lineSeriesCurrent.Points[lineSeriesCurrent.Points.Count - 2].X <= 0)
                                {
                                    lineSeriesCurrent.Points.Add(new DataPoint(double.NaN, double.NaN));
                                    NaNNumber.Add(num - 1);
                                }
                        lineSeriesCurrent.Points.Add(new DataPoint(tempx, tempy));

                    }
                }
            }

            switch (CurveX)
            {
                case 0://不分支
                    pm.Axes[0].Title = Properties.Resources.StringGear + " No.";
                    break;
                case 1://按牙盘
                    pm.Axes[0].Title = Properties.Resources.StringGear + " No. (" + Properties.Resources.StringBranchfr + ")";
                    break;
                case 2://按飞轮
                    pm.Axes[0].Title = Properties.Resources.StringGear + " No. (" + Properties.Resources.StringBranchbk + ")";
                    break;
                case 3://按内变速
                    pm.Axes[0].Title = Properties.Resources.StringGear + " No. (" + Properties.Resources.StringBranchin + ")";
                    break;
            }


            switch (CurveY)
            {
                case 1://走距速比
                    pm.Axes[1].Title = Properties.Resources.StringSpdRatio;
                    break;
                case 2://GI速比
                    pm.Axes[1].Title = Properties.Resources.StringGI;
                    break;
                case 3://齿比
                    pm.Axes[1].Title = Properties.Resources.StringGearRatio;
                    break;
                case 4://车速踏频
                    pm.Axes[1].Title = (IsSpd ? Properties.Resources.StringCAD + "(rpm)" : (Properties.Resources.StringSpeed + (IsMPH ? "(mph)" : "(km/h)")));
                    break;
            }


            label0.Content = Properties.Resources.StringTotaldiff + ": " +
                (Convert.ToDouble(toothratemax / toothratemin * 100.0)).ToString("F0") +
                "%";
            label1.Content = Properties.Resources.StringTotalCap + ": " +
                (frtemp.teeth[frtemp.Speeds - 1].teeth - frtemp.teeth[0].teeth - bktemp.teeth[bktemp.Speeds - 1].teeth + bktemp.teeth[0].teeth).ToString() + "T";



            chart.ResetAllAxes();
            chart.InvalidatePlot(true);
            pm.Axes[1].Minimum = 0;
            //if (pm.Axes[0].ActualMinorStep < 0.9 && pm.Axes[0].MinorStep == double.NaN) pm.Axes[0].MinorStep = 1;
            //if (pm.Axes[0].ActualMajorStep < 0.9 && pm.Axes[0].MajorStep == double.NaN) pm.Axes[0].MajorStep = 1;
            //Chart1.InvalidatePlot(true);


        }

        public void CalculateWheelLenth()
        {
            //if (checkBox2 == null) return;
            if (IsISO)
            {
                try
                {
                    if (TireISO1 <= 5)
                    {
                        Whlength = Convert.ToInt32((TireISO1 * 25.4 * 2 + TireISO2) * Math.PI);
                    }
                    else
                    {
                        Whlength = Convert.ToInt32((TireISO1 * 2.0 + TireISO2) * Math.PI);
                    }
                }
                catch { }
            }
            else
            {
                CWheel temp = whnow;
                if (temp != null)
                    Whlength = temp.Lenth;
            }
        }

        public void AddCurveNow()
        {
            CGear frtemp = frnow;
            CGear bktemp = bknow;
            CGear intemp = innow;
            CWheel whtemp = whnow;

            whtemp.Lenth = Whlength;

            string tempstr = "";
            tempstr += pm.Axes[0].Title;
            tempstr += " " + frtemp.Name;
            tempstr += "& " + bktemp.Name;
            if (intemp.Speeds != 1)
                tempstr += "& " + intemp.Name;
            string chartname = tempstr + "& " + whtemp.Lenth.ToString() + "mm";
            /*
            while (Chart1.Series.FindByName(chartname) != null)
            {
                InputForm inputform = new InputForm(Properties.Resources.StringAddCurve, Properties.Resources.StringCurveexist + ": \r\n\"" + chartname + "\"\r\n" + Properties.Resources.StringRenameandAdd, chartname);
                if (inputform.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    chartname = inputform.name;
                else
                    return;
            }*/
            //foreach (DataPoint dp in lineSeriesCurrent..Points) dp.MarkerSize = 4;
            InputWindow inputform = new InputWindow(Properties.Resources.StringAddCurve, Properties.Resources.StringInputCName, chartname);
            if (inputform.ShowDialog().Value)
                chartname = inputform.name;
            else
                return;


            LineSeries temp = new LineSeries();
            //temp.Color = OxyColors.SkyBlue;
            //temp.MarkerFill = OxyColors.SkyBlue;
            temp.MarkerSize = 4;
            temp.MarkerStroke = OxyColors.White;
            temp.MarkerStrokeThickness = 2;
            temp.MarkerType = MarkerType.Circle;
            temp.Title = chartname;
            foreach (DataPoint dp in lineSeriesCurrent.Points)
                temp.Points.Add(new DataPoint(dp.X, dp.Y));
            pm.Series.Add(temp);


            chart.InvalidatePlot(true);


        }

        public void ClearCurve()
        {
            pm.Series.Clear();
            pm.Series.Add(lineSeriesCurrent);
            Calculate();
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.frnow1 = frnow.teeth[0].Teeth;
            Properties.Settings.Default.frnow2 = frnow.teeth[1].Teeth;
            Properties.Settings.Default.frnow3 = frnow.teeth[2].Teeth;
            Properties.Settings.Default.frnow4 = frnow.teeth[3].Teeth;
            Properties.Settings.Default.frnow5 = frnow.teeth[4].Teeth;
            Properties.Settings.Default.frnow6 = frnow.teeth[5].Teeth;
            Properties.Settings.Default.frnow7 = frnow.teeth[6].Teeth;
            Properties.Settings.Default.frnow8 = frnow.teeth[7].Teeth;
            Properties.Settings.Default.frnow9 = frnow.teeth[8].Teeth;
            Properties.Settings.Default.frnow10 = frnow.teeth[9].Teeth;
            Properties.Settings.Default.frnow11 = frnow.teeth[10].Teeth;
            Properties.Settings.Default.frnow12 = frnow.teeth[11].Teeth;

            Properties.Settings.Default.bknow1 = bknow.teeth[0].Teeth;
            Properties.Settings.Default.bknow2 = bknow.teeth[1].Teeth;
            Properties.Settings.Default.bknow3 = bknow.teeth[2].Teeth;
            Properties.Settings.Default.bknow4 = bknow.teeth[3].Teeth;
            Properties.Settings.Default.bknow5 = bknow.teeth[4].Teeth;
            Properties.Settings.Default.bknow6 = bknow.teeth[5].Teeth;
            Properties.Settings.Default.bknow7 = bknow.teeth[6].Teeth;
            Properties.Settings.Default.bknow8 = bknow.teeth[7].Teeth;
            Properties.Settings.Default.bknow9 = bknow.teeth[8].Teeth;
            Properties.Settings.Default.bknow10 = bknow.teeth[9].Teeth;
            Properties.Settings.Default.bknow11 = bknow.teeth[10].Teeth;
            Properties.Settings.Default.bknow12 = bknow.teeth[11].Teeth;

            Properties.Settings.Default.innow1 = innow.teeth[0].Teeth;
            Properties.Settings.Default.innow2 = innow.teeth[1].Teeth;
            Properties.Settings.Default.innow3 = innow.teeth[2].Teeth;
            Properties.Settings.Default.innow4 = innow.teeth[3].Teeth;
            Properties.Settings.Default.innow5 = innow.teeth[4].Teeth;
            Properties.Settings.Default.innow6 = innow.teeth[5].Teeth;
            Properties.Settings.Default.innow7 = innow.teeth[6].Teeth;
            Properties.Settings.Default.innow8 = innow.teeth[7].Teeth;
            Properties.Settings.Default.innow9 = innow.teeth[8].Teeth;
            Properties.Settings.Default.innow10 = innow.teeth[9].Teeth;
            Properties.Settings.Default.innow11 = innow.teeth[10].Teeth;
            Properties.Settings.Default.innow12 = innow.teeth[11].Teeth;
            Properties.Settings.Default.innow13 = innow.teeth[12].Teeth;
            Properties.Settings.Default.innow14 = innow.teeth[13].Teeth;

            Properties.Settings.Default.IsSpd = IsSpd;
            Properties.Settings.Default.SpdVal = Cad;
            Properties.Settings.Default.IsMPH = IsMPH;
            Properties.Settings.Default.wheelid = whnow_index;
            Properties.Settings.Default.wheellen = Whlength;
            Properties.Settings.Default.IsISO = IsISO;
            Properties.Settings.Default.numUD1 = TireISO1;
            Properties.Settings.Default.numUD2 = TireISO2;
        }

        public void ExportFileTr()
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog1.DefaultExt = "csv";
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog1.FileName = Properties.Resources.StringTransData + ".csv";
            if (!saveFileDialog1.ShowDialog().Value)
                return;

            CGear frtemp = (CGear)frnow.Clone();
            CGear bktemp = (CGear)bknow.Clone();
            CGear intemp = (CGear)innow.Clone();
            CWheel whtemp = (CWheel)whnow.Clone();

            //for (int i = 0; i < dataGridViewFr.Items.Count; i++)
            //    frtemp.teeth[i].teeth = ((CTeeth)dataGridViewFr.Items[i]).Teeth;
            //for (int i = 0; i < dataGridViewBk.Items.Count; i++)
            //    bktemp.teeth[i].teeth = ((CTeeth)dataGridViewBk.Items[i]).Teeth;
            //for (int i = 0; i < dataGridViewIn.Items.Count; i++)
            //    intemp.teeth[i].teeth = ((CTeeth)dataGridViewIn.Items[i]).Teeth;

            whtemp.Lenth = Whlength;
            System.IO.StreamWriter swriter;
            string _filename = saveFileDialog1.FileName;
            try
            {
                swriter = new System.IO.StreamWriter(_filename, false, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(Properties.Resources.StringUnableexp + "\r\n" + ex.Message);
                return;
            }
            swriter.WriteLine(Properties.Resources.StringTransData);
            swriter.WriteLine("");


            swriter.WriteLine(Properties.Resources.StringTire + "," + whtemp.Name + "," + whtemp.Lenth.ToString() + "mm");

            string frstr = "";
            for (int i = 0; i < frtemp.Speeds; i++)
                frstr += frtemp.teeth[i].teeth.ToString() + "/";
            frstr += "T";
            swriter.WriteLine(Properties.Resources.StringCranksets + "," + frtemp.Name + "," + frstr);

            string bkstr = "";
            for (int i = 0; i < bktemp.Speeds; i++)
                bkstr += bktemp.teeth[i].teeth.ToString() + "/";
            bkstr += "T";
            swriter.WriteLine(Properties.Resources.StringCassette + "," + bktemp.Name + "," + bkstr);

            string instr = "";
            for (int i = 0; i < intemp.Speeds; i++)
                instr += intemp.teeth[i].teeth.ToString("0.00") + "/";
            swriter.WriteLine(Properties.Resources.StringInternalHub + "," + intemp.Name + "," + instr);

            swriter.WriteLine("");
            string strtemp = "";

            strtemp += "No."+",";
            strtemp += Properties.Resources.StringGear+",";
            strtemp += Properties.Resources.StringGearTeeth+",";
            strtemp += Properties.Resources.StringGearRatio+",";
            strtemp += Properties.Resources.StringGI+",";
            if (IsSpd)
            {
                strtemp += Properties.Resources.StringCAD + " rpm";
            }
            else
            {
                if (IsMPH)
                    strtemp += Properties.Resources.StringSpeed + " mph";
                else
                    strtemp += Properties.Resources.StringSpeed + " km/h";
            }
            strtemp += ",";
            strtemp += Properties.Resources.StringIncrement+",";
            strtemp += Properties.Resources.StringRemark + ",";

            swriter.WriteLine(strtemp);

            foreach (CResult it in list.Items)
            {
                strtemp = "";
                strtemp += it.No1.ToString() + ",";
                strtemp += it.Gear1 + ",";
                strtemp += it.GearT1 + ",";
                strtemp += it.GearRatio1.ToString("0.00") + ",";
                strtemp += it.SpeedRatio1.ToString("0.0") + ",";
                strtemp += it.Speed1.ToString("0.0") + ",";
                strtemp += it.Increment1.ToString("P2") + ",";
                strtemp += it.Remark1 + ",";
                swriter.WriteLine(strtemp);
            }
            swriter.Close();
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
