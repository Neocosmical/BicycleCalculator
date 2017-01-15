using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;
using System.Windows.Controls.Primitives;
using System.Reflection;

namespace BicycleCalculatorWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
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

        public static PlotModel LineSeries1()
        {
            var plotModel1 = new PlotModel();
            plotModel1.PlotType = PlotType.Cartesian;
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

        PlotModel pm;
        PlotModel pm1;

        LineSeries lineSeriesCurrent;

        public MainWindow()
        {
            switch (Properties.Settings.Default.Language)
            {
                case 0:
                    break;
                case 2:
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                    break;
                case 3:
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
                    break;
            }
            InitializeComponent();
        }

        CData data = new CData();

        bool ready = false;

        int CurveX = 0;
        int CurveY = 1;

        System.Timers.Timer timerCAD = new System.Timers.Timer(50);
        System.Diagnostics.Stopwatch CADwatch = new System.Diagnostics.Stopwatch();
        
        private void Form1_Load(object sender, RoutedEventArgs e)
        {
            LabelVer.Content = String.Format("V{0}         Loading...", AssemblyVersion);
            TextBlockLoad.Visibility = Visibility.Visible;
            /////////////////////////////GUI///////////////////////////////////////
            try
            {
                this.WindowState = Properties.Settings.Default.IsWinMax ? System.Windows.WindowState.Maximized : System.Windows.WindowState.Normal;
                this.Width = Properties.Settings.Default.WinW;
                this.Height = Properties.Settings.Default.WinH;

                InputColumn.Width = new GridLength(Properties.Settings.Default.InputCWidth, GridUnitType.Star);
                AnsColumn.Width = new GridLength(1.0 - Properties.Settings.Default.InputCWidth, GridUnitType.Star);

                if (Properties.Settings.Default.UpRHight == 1.0)
                {
                    Chart1.Visibility = System.Windows.Visibility.Collapsed;
                    DownRow.MinHeight = 0;
                    DownRow.MaxHeight = 0;
                    GridSplitterT1.IsEnabled = false;
                }
                else
                {
                    UpRow.Height = new GridLength(Properties.Settings.Default.UpRHight, GridUnitType.Star);
                    DownRow.Height = new GridLength(1.0 - Properties.Settings.Default.UpRHight, GridUnitType.Star);
                }

                InputColumn1.Width = new GridLength(Properties.Settings.Default.InputC1Width, GridUnitType.Star);
                AnsColumn1.Width = new GridLength(1.0 - Properties.Settings.Default.InputC1Width, GridUnitType.Star);

                UpRow1.Height = new GridLength(Properties.Settings.Default.UpR1Hight, GridUnitType.Star);
                DownRow1.Height = new GridLength(1.0 - Properties.Settings.Default.UpR1Hight, GridUnitType.Star);

            }
            catch
            {
                ;
            }
            ///////////////////////////////////////////////////////////////////////////
            System.Threading.Thread InitThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Init));
            InitThread.Start();
            //DataInit(null);


            //ready = true;
            //Calculate();
            //CalculateSpoke();
        }

        private void Init(object obj)
        {
            data.DataInit();

            pm = new PlotModel();
            pm1 = new PlotModel();
            lineSeriesCurrent = new LineSeries();

            this.Dispatcher.Invoke(new outputDelegate(outputAction));
        }

        void timerCAD_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            labelCAD.Dispatcher.Invoke(new Action(() =>
            {
                if (checkBox1.IsChecked.Value)
                {
                    labelCAD.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                }
                else
                {
                    
                    byte n = Convert.ToByte(Math.Sin(Math.PI * trackBar1.Value / 30000.0 * CADwatch.ElapsedMilliseconds) * 100.0 + 100.0);
                    labelCAD.Foreground = new SolidColorBrush(Color.FromRgb(n, n, n));
                }
            }));
        }


        private delegate void outputDelegate();


        private void RefreshData()
        {
            ready = false;

            int temp0 = WheelcomboBox.SelectedIndex;
            int temp1 = FrNumcomboBox.SelectedIndex;
            int temp2 = FrModelcomboBox.SelectedIndex;
            int temp3 = BkNumcomboBox.SelectedIndex;
            int temp4 = BkModelcomboBox.SelectedIndex;
            int temp5 = InNumcomboBox.SelectedIndex;
            int temp6 = InModelcomboBox.SelectedIndex;
            int temp7 = HubNumcomboBox.SelectedIndex;
            int temp8 = RimNumcomboBox.SelectedIndex;
            
            FrNumcomboBox.Items.Clear();
            BkNumcomboBox.Items.Clear();
            InNumcomboBox.Items.Clear();
            WheelcomboBox.Items.Clear();

            foreach (CGearList list in data.frlists.Lists)
                FrNumcomboBox.Items.Add(list);

            foreach (CGearList list in data.bklists.Lists)
                BkNumcomboBox.Items.Add(list);

            foreach (CGearList list in data.inlists.Lists)
                InNumcomboBox.Items.Add(list);


            foreach (CWheel wheel in data.wheelList)
            {
                WheelcomboBox.Items.Add(wheel);
            }

            HubNumcomboBox.Items.Refresh();
            RimNumcomboBox.Items.Refresh();

            WheelcomboBox.SelectedIndex = 0;
            FrNumcomboBox.SelectedIndex = 0;
            FrModelcomboBox.SelectedIndex = 0;
            BkNumcomboBox.SelectedIndex = 0;
            BkModelcomboBox.SelectedIndex = 0;
            InNumcomboBox.SelectedIndex = 0;
            InModelcomboBox.SelectedIndex = 0;
            HubNumcomboBox.SelectedIndex = 0;
            RimNumcomboBox.SelectedIndex = 0;

            WheelcomboBox.SelectedIndex   = temp0;
            FrNumcomboBox.SelectedIndex   = temp1;
            FrModelcomboBox.SelectedIndex = temp2;
            BkNumcomboBox.SelectedIndex   = temp3;
            BkModelcomboBox.SelectedIndex = temp4;
            InNumcomboBox.SelectedIndex   = temp5;
            InModelcomboBox.SelectedIndex = temp6;
            HubNumcomboBox.SelectedIndex  = temp7;
            RimNumcomboBox.SelectedIndex  = temp8;

            ready = true;
            Calculate();
            CalculateSpoke();
        }

        System.Timers.Timer timerload1;
        private void outputAction()
        {
            ready = false;

            FrNumcomboBox.Items.Clear();
            BkNumcomboBox.Items.Clear();
            InNumcomboBox.Items.Clear();
            WheelcomboBox.Items.Clear();

            foreach (CGearList list in data.frlists.Lists)
                FrNumcomboBox.Items.Add(list);

            foreach (CGearList list in data.bklists.Lists)
                BkNumcomboBox.Items.Add(list);

            foreach (CGearList list in data.inlists.Lists)
                InNumcomboBox.Items.Add(list);


            foreach (CWheel wheel in data.wheelList)
            {
                WheelcomboBox.Items.Add(wheel);
            }

            WheelcomboBox.SelectedIndex = 0;
            FrNumcomboBox.SelectedIndex = 0;
            FrModelcomboBox.SelectedIndex = 0;
            BkNumcomboBox.SelectedIndex = 0;
            BkModelcomboBox.SelectedIndex = 0;
            InNumcomboBox.SelectedIndex = 0;
            InModelcomboBox.SelectedIndex = 0;
            HubNumcomboBox.SelectedIndex = 0;
            RimNumcomboBox.SelectedIndex = 0;


            WheelcomboBox.SelectedIndex = Properties.Settings.Default.wheelid;
            
            FrNumcomboBox.SelectedIndex = Properties.Settings.Default.frnumid;
            FrModelcomboBox.SelectedIndex = Properties.Settings.Default.frmodid;
            BkNumcomboBox.SelectedIndex = Properties.Settings.Default.bknumid;
            BkModelcomboBox.SelectedIndex = Properties.Settings.Default.bkmodid;
            InNumcomboBox.SelectedIndex = Properties.Settings.Default.innumid;
            InModelcomboBox.SelectedIndex = Properties.Settings.Default.inmodid;
            

            timerCAD.Enabled = true;
            timerCAD.Elapsed += timerCAD_Elapsed;
            CADwatch.Start();
            listBox1Grid.ColumnHeaderContextMenu.Opened += ColumnHeaderContextMenu_Opened;

            checkBox2.ToolTip = Properties.Resources.StringEnterISO + "\r\n" + Properties.Resources.StringISOeg;
            numericUpDown1.ToolTip = Properties.Resources.StringTirewidth + "\r\n" + Properties.Resources.StringISOmminch;
            numericUpDown2.ToolTip = Properties.Resources.StringRimdiameter + "\r\n" + Properties.Resources.StringRimdiametermm;
            WheelLenthtextBox.ToolTip = Properties.Resources.StringTireCirc + "\r\n" + Properties.Resources.Stringmm;

            pm = LineSeries();
            pm1 = LineSeries1();
            Chart1.Model = pm;
            Chart2.Model = pm1;
            lineSeriesCurrent.Color = OxyColors.SkyBlue;
            lineSeriesCurrent.MarkerFill = OxyColors.SkyBlue;
            lineSeriesCurrent.MarkerSize = 4;
            lineSeriesCurrent.MarkerStroke = OxyColors.White;
            lineSeriesCurrent.MarkerStrokeThickness = 1.5;
            lineSeriesCurrent.MarkerType = MarkerType.Circle;
            lineSeriesCurrent.Title = Properties.Resources.StringCurrent;
            Chart1.Model.Series.Add(lineSeriesCurrent);

            pm.Axes[1].Title = Properties.Resources.StringSpdRatio;
            pm.Axes[0].Title = Properties.Resources.StringGear + " No.";

            HubNumcomboBox.ItemsSource = data.hublist;
            RimNumcomboBox.ItemsSource = data.rimlist;

            HubNumcomboBox.SelectedIndex = 1;
            RimNumcomboBox.SelectedIndex = 1;


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

            checkBox1.IsChecked = Properties.Settings.Default.IsSpd;
            trackBar1.Value = Properties.Settings.Default.SpdVal;
            checkBox2.IsChecked = Properties.Settings.Default.IsISO;
            tableLayoutPanel10.Visibility = checkBox2.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
            WheelcomboBox.Visibility = checkBox2.IsChecked.Value ? Visibility.Hidden : Visibility.Visible;

            WheelLenthtextBox.Text = Convert.ToString(Properties.Settings.Default.wheellen);
            numericUpDown1.Text = Convert.ToString(Properties.Settings.Default.numUD1);
            numericUpDown2.Text = Convert.ToString(Properties.Settings.Default.numUD2);

            TabControlMain.SelectedIndex = Properties.Settings.Default.TabSlt;

            ready = true;
            Calculate();
            CalculateSpoke();

            timerload1 = new System.Timers.Timer(200);
            timerload1.Enabled = true;
            timerload1.Elapsed += timerload1_Elapsed;
        }

        double TextBlockLoadOpacity = 1;
        void timerload1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timerload1.Interval = 33;
            TextBlockLoadOpacity -= 0.15;
            TextBlockLoad.Dispatcher.Invoke(new Action(() =>
            {
                if(TextBlockLoadOpacity >0)
                    TextBlockLoad.Opacity = TextBlockLoadOpacity;
                else
                    TextBlockLoad.Visibility = Visibility.Hidden;
            }));
            if (TextBlockLoadOpacity < 0)
                timerload1.Enabled = false;
        }


        private void FrNumcomboBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FrNumcomboBox.SelectedItem == null) return;
            bool rtemp = ready;
            ready = false;
            FrModelcomboBox.Items.Clear();


            foreach (CGear gear in ((CGearList)FrNumcomboBox.SelectedItem).Gears)
                FrModelcomboBox.Items.Add(gear);

            FrModelcomboBox.SelectedIndex = 0;
            ready = rtemp;
            Calculate();
        }

        private void BkNumcomboBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BkNumcomboBox.SelectedItem == null) return;
            bool rtemp = ready;
            ready = false;
            BkModelcomboBox.Items.Clear();

            foreach (CGear gear in ((CGearList)BkNumcomboBox.SelectedItem).Gears)
                BkModelcomboBox.Items.Add(gear);

            BkModelcomboBox.SelectedIndex = 0;
            ready = rtemp;
            Calculate();
        }

        private void InNumcomboBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InNumcomboBox.SelectedItem == null) return;
            bool rtemp = ready;
            ready = false;
            InModelcomboBox.Items.Clear();

            foreach (CGear gear in ((CGearList)InNumcomboBox.SelectedItem).Gears)
                InModelcomboBox.Items.Add(gear);
            InModelcomboBox.SelectedIndex = 0;
            ready = rtemp;
            Calculate();
        }

        CGear frnow;
        private void FrModelcomboBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FrModelcomboBox.SelectedItem == null) return;

            bool rtemp = ready;
            ready = false;
            frnow = (CGear)((CGear)FrModelcomboBox.SelectedItem).Clone();
            dataGridViewFr.Items.Clear();
            for (int i = 0; i < frnow.Speeds; i++) dataGridViewFr.Items.Add(frnow.teeth[i]);
            ready = rtemp;
            Calculate();
        }

        CGear bknow;
        private void BkModelcomboBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BkModelcomboBox.SelectedItem == null) return;

            bool rtemp = ready;
            ready = false;
            bknow = (CGear)((CGear)BkModelcomboBox.SelectedItem).Clone();
            dataGridViewBk.Items.Clear();
            for (int i = 0; i < bknow.Speeds; i++) dataGridViewBk.Items.Add(bknow.teeth[i]);
            ready = rtemp;
            Calculate();
        }

        CGear innow;
        private void InModelcomboBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InModelcomboBox.SelectedItem == null) return;
            bool rtemp = ready;
            ready = false;
            innow = (CGear)((CGear)InModelcomboBox.SelectedItem).Clone();
            dataGridViewIn.Items.Clear();
            for (int i = 0; i < innow.Speeds; i++) dataGridViewIn.Items.Add(innow.teeth[i]);
            if (innow.Speeds <= 1) dataGridViewIn.IsEnabled = false;
            else dataGridViewIn.IsEnabled = true;
            ready = rtemp;
            Calculate();
        }
        
        double toothrateold = 0;
        List<CResult> results = new List<CResult>();
        List<int> NaNNumber = new List<int>();
        private void Calculate()
        {
            if (!ready) return;
            pm.Axes[0].MinorStep = double.NaN;
            pm.Axes[0].MajorStep = double.NaN;
            CGear frtemp = frnow;
            CGear bktemp = bknow;
            CGear intemp = innow;
            CWheel whtemp = (CWheel)((CWheel)WheelcomboBox.SelectedItem).Clone();
            try
            {
                whtemp.Lenth = Convert.ToInt32(WheelLenthtextBox.Text);
            }
            catch { }

            double cad = trackBar1.Value;

            listBox1.Items.Clear();

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

                        if (checkBox1.IsChecked.Value)
                        {
                            if (Properties.Settings.Default.IsMPH)
                                resulttemp.Speed1 = cad * 1.609344 / 2.0 * 1000000.0 / 60.0 / whtemp.Lenth / toothrate;
                            else
                                resulttemp.Speed1 = cad / 2.0 * 1000000.0 / 60.0 / whtemp.Lenth / toothrate;
                        }
                        else
                        {
                            if (Properties.Settings.Default.IsMPH)
                                resulttemp.Speed1 = toothrate * cad * whtemp.Lenth * 60 / 1000000.0 * 0.6213712;
                            else
                                resulttemp.Speed1 = toothrate * cad * whtemp.Lenth * 60 / 1000000.0;
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
                        listBox1.Items.Add(resulttemp);

                        
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
                    pm.Axes[1].Title = (checkBox1.IsChecked.Value ? Properties.Resources.StringCAD + "(rpm)" : (Properties.Resources.StringSpeed + (Properties.Settings.Default.IsMPH ? "(mph)" : "(km/h)")));
                    break;
            }

            if (checkBox1.IsChecked.Value)
                SpeedColumn.Header = Properties.Resources.StringCAD + " rpm";
            else
            {
                if (Properties.Settings.Default.IsMPH)
                    SpeedColumn.Header = Properties.Resources.StringSpeed + " mph";
                else
                    SpeedColumn.Header = Properties.Resources.StringSpeed + " km/h";
            }
            labelinfo.Content = Properties.Resources.StringTotaldiff + ": " +
                (Convert.ToDouble(toothratemax / toothratemin * 100.0)).ToString("F0") +
                "%";
            labelinfo1.Content = Properties.Resources.StringTotalCap + ": " +
                (frtemp.teeth[frtemp.Speeds - 1].teeth - frtemp.teeth[0].teeth - bktemp.teeth[bktemp.Speeds - 1].teeth + bktemp.teeth[0].teeth).ToString() + "T";

            if (checkBox1.IsChecked.Value)
            {
                if (Properties.Settings.Default.IsMPH)
                    labelCAD.Content = (trackBar1.Value / 2.0).ToString("0.0") + " mph";
                else
                    labelCAD.Content = (trackBar1.Value / 2.0).ToString("0.0") + " km/h";
            }
            else
            {
                labelCAD.Content = trackBar1.Value.ToString("0.0") + " rpm";
            }
            
            Chart1.ResetAllAxes();
            Chart1.InvalidatePlot(true);
            pm.Axes[1].Minimum = 0;
            //if (pm.Axes[0].ActualMinorStep < 0.9 && pm.Axes[0].MinorStep == double.NaN) pm.Axes[0].MinorStep = 1;
            //if (pm.Axes[0].ActualMajorStep < 0.9 && pm.Axes[0].MajorStep == double.NaN) pm.Axes[0].MajorStep = 1;
            //Chart1.InvalidatePlot(true);


        }

        
        private void AddCurveNow()
        {
            CGear frtemp = (CGear)FrModelcomboBox.SelectedItem;
            CGear bktemp = (CGear)BkModelcomboBox.SelectedItem;
            CGear intemp = (CGear)((CGear)InModelcomboBox.SelectedItem).Clone();
            CWheel whtemp = (CWheel)WheelcomboBox.SelectedItem;

            whtemp.Lenth = Convert.ToInt32(WheelLenthtextBox.Text);

            string tempstr = "";
            tempstr += pm.Axes[0].Title;
            tempstr += " " + frtemp.Name;
            tempstr += "& " + bktemp.Name;
            if (intemp.Speeds != 1)
                tempstr += "& " + intemp.Name;
            string chartname = tempstr + "& " + whtemp.Lenth.ToString()+"mm";
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
            

            Chart1.InvalidatePlot(true);

            
        }


        private void WheelcomboBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            CWheel temp = (CWheel)WheelcomboBox.SelectedItem;
            if (temp != null)
                WheelLenthtextBox.Text = temp.Lenth.ToString();
        }

        private void WheelLenthtextBox_ValueChanged(object sender, EventArgs e)
        {
            Calculate();
        }


        private void ExportFileSp()
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog1.DefaultExt = "csv";
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog1.FileName = Properties.Resources.StringSpokesData + ".csv";
            if (!saveFileDialog1.ShowDialog().Value)
                return;

            CRim rimtemp = rimnow;
            CHub hubtemp = hubnow;

            System.IO.StreamWriter swriter;
            string _filename = saveFileDialog1.FileName;
            try
            {
                swriter = new System.IO.StreamWriter(_filename, false, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Resources.StringUnableexp + "\r\n" + ex.Message);
                return;
            }
            swriter.WriteLine(Properties.Resources.StringSpokesData);
            swriter.WriteLine("");


            swriter.WriteLine(Properties.Resources.StringRims + "," + rimtemp.Name + ",");
            swriter.WriteLine("ERD" + "," + "OSB" + ",");
            swriter.WriteLine(rimtemp.ERD.ToString() + "mm" + "," + rimtemp.OSB.ToString() + "mm");
            swriter.WriteLine("");


            swriter.WriteLine(Properties.Resources.StringHubs + "," + hubtemp.Name + ",");
            string hubstr = "";
            hubstr += Properties.Resources.StringFlangeL + ",";
            hubstr += Properties.Resources.StringFlangeR + ",";
            hubstr += Properties.Resources.StringCenterL + ",";
            hubstr += Properties.Resources.StringCenterR + ",";
            hubstr += Properties.Resources.StringSpokeHole + ",";
            swriter.WriteLine(hubstr);
            hubstr = "";
            hubstr += hubtemp.LeftFlange + "mm" + ",";
            hubstr += hubtemp.RightFlange + "mm" + ",";
            hubstr += hubtemp.CenterToLeft + "mm" + ",";
            hubstr += hubtemp.CenterToRight + "mm" + ",";
            hubstr += hubtemp.SpokeHole + "mm" + ",";
            swriter.WriteLine(hubstr);
            swriter.WriteLine("");

            swriter.WriteLine(Properties.Resources.StringSpokes + "," + TextBoxSpokes.Text);
            swriter.WriteLine("");

            string strtemp = "";
            foreach (GridViewColumn ch in listBox2Grid.Columns)
                strtemp += ch.Header + ",";
            swriter.WriteLine(strtemp);

            foreach (CSpokeResult it in listBox2.Items)
            {
                strtemp = "";
                strtemp += it.Crosses.ToString() + ",";
                strtemp += it.Lenthleft.ToString("0.00") + ",";
                strtemp += it.Lenthright.ToString("0.00") + ",";
                strtemp += it.Remark + ",";
                swriter.WriteLine(strtemp);
            }
            swriter.Close();

        }

        private void ExportFileTr()
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog1.DefaultExt = "csv";
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog1.FileName = Properties.Resources.StringTransData + ".csv";
            if (!saveFileDialog1.ShowDialog().Value)
                return;

            CGear frtemp = (CGear)((CGear)FrModelcomboBox.SelectedItem).Clone();
            CGear bktemp = (CGear)((CGear)BkModelcomboBox.SelectedItem).Clone();
            CGear intemp = (CGear)((CGear)InModelcomboBox.SelectedItem).Clone();
            CWheel whtemp = (CWheel)((CWheel)WheelcomboBox.SelectedItem).Clone();

            for (int i = 0; i < dataGridViewFr.Items.Count; i++)
                frtemp.teeth[i].teeth = ((CTeeth)dataGridViewFr.Items[i]).Teeth;
            for (int i = 0; i < dataGridViewBk.Items.Count; i++)
                bktemp.teeth[i].teeth = ((CTeeth)dataGridViewBk.Items[i]).Teeth;
            for (int i = 0; i < dataGridViewIn.Items.Count; i++)
                intemp.teeth[i].teeth = ((CTeeth)dataGridViewIn.Items[i]).Teeth;

            whtemp.Lenth = Convert.ToInt32(WheelLenthtextBox.Text);
            System.IO.StreamWriter swriter;
            string _filename = saveFileDialog1.FileName;
            try
            {
                swriter = new System.IO.StreamWriter(_filename, false, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Resources.StringUnableexp + "\r\n" + ex.Message);
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
            foreach (GridViewColumn ch in listBox1Grid.Columns)
                strtemp += ch.Header + ",";
            swriter.WriteLine(strtemp);

            foreach (CResult it in listBox1.Items)
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
        

        private void listBox1_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            pm.Annotations.Clear();
            dataGridViewFr.SelectedIndex = -1;
            dataGridViewBk.SelectedIndex = -1;
            dataGridViewIn.SelectedIndex = -1;
            if (listBox1.SelectedItems.Count == 0)
            {
                Chart1.InvalidatePlot(true);
                return;
            }

            CGear frtemp = (CGear)((CGear)FrModelcomboBox.SelectedItem).Clone();
            CGear bktemp = (CGear)((CGear)BkModelcomboBox.SelectedItem).Clone();
            CGear intemp = (CGear)((CGear)InModelcomboBox.SelectedItem).Clone();

            double max = 0;
            double min = 100000000;
            foreach (CResult item in listBox1.SelectedItems)
            {
                int n = item.No1 - 1;
                int f = n / (bktemp.Speeds * intemp.Speeds);
                int i = (n % (bktemp.Speeds * intemp.Speeds)) / bktemp.Speeds;
                int b = (n - f * bktemp.Speeds * intemp.Speeds - i * bktemp.Speeds);
                double gtemp = Convert.ToDouble(((CTeeth)dataGridViewFr.Items[f]).Teeth) / Convert.ToDouble(((CTeeth)dataGridViewBk.Items[b]).Teeth) * ((CTeeth)dataGridViewIn.Items[intemp.Speeds - 1 - i]).Teeth;
                if (gtemp > max) max = gtemp;
                if (gtemp < min) min = gtemp;
                dataGridViewFr.SelectedIndex = f;
                dataGridViewBk.SelectedIndex = b;
                dataGridViewIn.SelectedIndex = i;


                PointAnnotation pointAnnotation1 = new PointAnnotation();
                int nanadd = 0;
                foreach (int nan in NaNNumber)
                    if (n > nan-2) nanadd++;
                pointAnnotation1.X = lineSeriesCurrent.Points[n + nanadd].X;
                pointAnnotation1.Y = lineSeriesCurrent.Points[n + nanadd].Y;
                pointAnnotation1.Size = 6;
                pointAnnotation1.Fill = OxyColors.SkyBlue;
                pointAnnotation1.Stroke = OxyColors.DarkBlue;
                pointAnnotation1.StrokeThickness = 1;
                pointAnnotation1.Text = item.Gear1;
                pm.Annotations.Add(pointAnnotation1);

            }
            if (listBox1.SelectedItems.Count > 1)
            {
                labelinfo2.Content = Properties.Resources.StringIncrement + ": " + ((max / min - 1.0) * 100.0).ToString("0.0") + "%";
                labelinfo2.Visibility = Visibility.Visible;
            }
            else
            {
                labelinfo2.Content = "";
                labelinfo2.Visibility = Visibility.Collapsed;
            }
            Chart1.InvalidatePlot(true);
        }
        
        private void checkBox2_CheckedChanged(object sender, RoutedEventArgs e)
        {
            tableLayoutPanel10.Visibility = checkBox2.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
            WheelcomboBox.Visibility = checkBox2.IsChecked.Value ? Visibility.Hidden : Visibility.Visible;

            CalculateWheelLenth();
        }

        private void CalculateWheelLenth()
        {

            if (checkBox2 == null) return;
            if (checkBox2.IsChecked.Value)
            {
                try
                {
                    if (Convert.ToDouble(numericUpDown1.Text) <= 5)
                    {
                        WheelLenthtextBox.Text = ((Convert.ToDouble(numericUpDown1.Text) * 25.4 * 2 + Convert.ToDouble(numericUpDown2.Text)) * Math.PI).ToString("F0");
                    }
                    else
                    {
                        WheelLenthtextBox.Text = ((Convert.ToDouble(numericUpDown1.Text) * 2.0 + Convert.ToDouble(numericUpDown2.Text)) * Math.PI).ToString("F0");
                    }
                }
                catch { }
            }
            else
            {
                CWheel temp = (CWheel)WheelcomboBox.SelectedItem;
                if(temp != null)
                WheelLenthtextBox.Text = temp.Lenth.ToString("D"); ;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, TextChangedEventArgs e)
        {
            CalculateWheelLenth();
        }

        private void numericUpDown1_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                if (Convert.ToDouble(numericUpDown1.Text) <= 5)
                {
                    numericUpDown1.Text = Convert.ToDouble(numericUpDown1.Text).ToString("F2");
                }
                else
                {
                    numericUpDown1.Text = Convert.ToDouble(numericUpDown1.Text).ToString("F0");
                }
            }
            catch
            {
                numericUpDown1.Text = "50";
            }
        }

        private void numericUpDown2_ValueChanged(object sender, TextChangedEventArgs e)
        {
            CalculateWheelLenth();
        }


        private void numericUpDown2_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                numericUpDown2.Text = Convert.ToDouble(numericUpDown2.Text).ToString("F0");
            }
            catch
            {
                numericUpDown2.Text = "406";
            }
        }

        private void NoneToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CurveX = 0;
            Calculate();
        }

        private void WithCrankToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CurveX = 1;
            Calculate();
        }

        private void WithCassToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CurveX = 2;
            Calculate();
        }

        private void WithInterToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CurveX = 3;
            Calculate();
        }
        
        private void SRToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CurveY = 1;
            Calculate();
        }

        private void GIToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CurveY = 2;
            Calculate();
        }

        private void GRToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CurveY = 3;
            Calculate();
        }

        private void SpdToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CurveY = 4;
            Calculate();
        }


        private void splitContainer2_Panel1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            modelresize();
        }

        private void modelresize()
        {
            
            if (groupBox5.Visibility == Visibility.Visible)
            {
                if (GridInput.ActualWidth < 400)
                {
                    Grid.SetRowSpan(groupBox2, 1);
                    Grid.SetRow(groupBox5, 1);
                    Grid.SetColumn(groupBox5, 1);
                    Grid.SetRowSpan(groupBox5, 2);
                    GridInput.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Auto);
                }
                else
                {
                    Grid.SetRowSpan(groupBox2, 2);
                    Grid.SetRow(groupBox5, 0);
                    Grid.SetColumn(groupBox5, 2);
                    Grid.SetRowSpan(groupBox5, 2);
                    GridInput.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                }
            }
            else
            {
                Grid.SetRowSpan(groupBox2, 2);
                Grid.SetRow(groupBox5, 0);
                Grid.SetColumn(groupBox5, 2);
                Grid.SetRowSpan(groupBox5, 2);
                GridInput.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Auto);
            }
             
        }

        private void kMHToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.IsMPH = false;
            Calculate();
        }

        private void mPHToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.IsMPH = true;
            Calculate();
        }


        private void exitToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void internalHubsToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (groupBox5.Visibility == Visibility.Visible)
                groupBox5.Visibility = Visibility.Collapsed;
            else
                groupBox5.Visibility = Visibility.Visible;
            modelresize();
        }

        private void chartToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Chart1.Visibility == Visibility.Visible)
            {
                Chart1.Visibility = Visibility.Collapsed;
                //MainGrid.RowDefinitions[0].Height = new GridLength(300, GridUnitType.Star);
                //MainGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Auto);
                //GridT.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Auto);
                DownRow.MinHeight = 0;
                DownRow.MaxHeight = 0;
                GridSplitterT1.IsEnabled = false;
            }
            else
            {
                Chart1.Visibility = Visibility.Visible;
                //MainGrid.RowDefinitions[1].Height = new GridLength(300, GridUnitType.Star);
                //GridT.RowDefinitions[1].Height = new GridLength(300, GridUnitType.Star);
                DownRow.MaxHeight = Double.PositiveInfinity;
                DownRow.MinHeight = 100;
                GridSplitterT1.IsEnabled = true;
            }
        }


        private void frMinuButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridViewFr.Items[id - 1]).teeth > 1)
                ((CTeeth)dataGridViewFr.Items[id - 1]).teeth--;
            dataGridViewFr.Items.Refresh();
            Calculate();
        }

        private void frAddButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridViewFr.Items[id - 1]).teeth < 200)
                ((CTeeth)dataGridViewFr.Items[id - 1]).teeth++;
            dataGridViewFr.Items.Refresh();
            Calculate();
        }

        private void bkMinuButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridViewBk.Items[id - 1]).teeth > 1)
                ((CTeeth)dataGridViewBk.Items[id - 1]).teeth--;
            dataGridViewBk.Items.Refresh();
            Calculate();
        }

        private void bkAddButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridViewBk.Items[id - 1]).teeth < 200)
                ((CTeeth)dataGridViewBk.Items[id - 1]).teeth++;
            dataGridViewBk.Items.Refresh();
            Calculate();
        }

        private void inMinuButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridViewIn.Items[id - 1]).teeth > 0.01)
                ((CTeeth)dataGridViewIn.Items[id - 1]).teeth -= 0.01;
            dataGridViewIn.Items.Refresh();
            Calculate();
        }

        private void inAddButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridViewIn.Items[id - 1]).teeth < 20.0)
                ((CTeeth)dataGridViewIn.Items[id - 1]).teeth += 0.01;
            dataGridViewIn.Items.Refresh();
            Calculate();
        }

        private void trackBar1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Calculate();
        }

        private void checkBox1_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox1.IsChecked.Value)
            {
                trackBar1.Value = 50;
                timerCAD.Enabled = false;
                labelCAD.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
            else
            {
                trackBar1.Value = 80;
                timerCAD.Enabled = true;
            }
            Calculate();
        }

        private void WheelLenthtextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Calculate();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WheelLenthtextBox.Text = (Convert.ToInt16(WheelLenthtextBox.Text) + 1).ToString();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            WheelLenthtextBox.Text = (Convert.ToInt16(WheelLenthtextBox.Text) - 1).ToString();
        }

        private void WheelLenthtextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int val = Convert.ToInt16(((TextBox)sender).Text);
            if (e.Delta > 0)
            {
                if (val < 65535)
                    ((TextBox)sender).Text = (val + 1).ToString();
            }
            else if (e.Delta < 0)
            {
                if (val > 0)
                    ((TextBox)sender).Text = (val - 1).ToString();
            }
        }

        private void textBox_MouseWheel_float(object sender, MouseWheelEventArgs e)
        {
            double val = Convert.ToDouble(((TextBox)sender).Text);
            if (e.Delta > 0)
            {
                if (val < 65535)
                    ((TextBox)sender).Text = (Convert.ToDouble(((TextBox)sender).Text) + 0.01).ToString("");
            }
            else if (e.Delta < 0)
            {
                if (val > 0)
                ((TextBox)sender).Text = (Convert.ToDouble(((TextBox)sender).Text) - 0.01).ToString();
            }
        }

        private void TextBox_MouseWheel_Fr(object sender, MouseWheelEventArgs e)
        {
            int id = (int)(((TextBox)sender).Tag);
            if (((CTeeth)dataGridViewFr.Items[id - 1]).Teeth > 1 && e.Delta < 0)
                ((CTeeth)dataGridViewFr.Items[id - 1]).Teeth -= 1;
            if (((CTeeth)dataGridViewFr.Items[id - 1]).Teeth < 200 && e.Delta > 0)
                ((CTeeth)dataGridViewFr.Items[id - 1]).Teeth += 1;
            dataGridViewFr.Items.Refresh();
            Calculate();
        }

        private void TextBox_MouseWheel_Bk(object sender, MouseWheelEventArgs e)
        {
            int id = (int)(((TextBox)sender).Tag);
            if (((CTeeth)dataGridViewBk.Items[id - 1]).Teeth > 1 && e.Delta < 0)
                ((CTeeth)dataGridViewBk.Items[id - 1]).Teeth -= 1;
            if (((CTeeth)dataGridViewBk.Items[id - 1]).Teeth < 200 && e.Delta > 0)
                ((CTeeth)dataGridViewBk.Items[id - 1]).Teeth += 1;
            dataGridViewBk.Items.Refresh();
            Calculate();
        }

        private void TextBox_MouseWheel_In(object sender, MouseWheelEventArgs e)
        {
            int id = (int)(((TextBox)sender).Tag);
            if (((CTeeth)dataGridViewIn.Items[id - 1]).Teeth > 0.01 && e.Delta < 0)
                ((CTeeth)dataGridViewIn.Items[id - 1]).Teeth -= 0.01;
            if (((CTeeth)dataGridViewIn.Items[id - 1]).Teeth < 20.0 && e.Delta > 0)
                ((CTeeth)dataGridViewIn.Items[id - 1]).Teeth += 0.01;

            dataGridViewIn.Items.Refresh();
            Calculate();
        }

        private void TextBox_LostKeyboardFocus_Fr(object sender, KeyboardFocusChangedEventArgs e)
        {
            int id = (int)(((TextBox)sender).Tag);
            double teethtemp = ((CTeeth)dataGridViewFr.Items[id - 1]).Teeth;
            try
            {
                teethtemp = Convert.ToInt16(((TextBox)sender).Text);
            }
            catch
            { }
            if (teethtemp > 200) teethtemp = 200;
            if (teethtemp < 1) teethtemp = 1;
            ((CTeeth)dataGridViewFr.Items[id - 1]).Teeth = teethtemp;
            dataGridViewFr.Items.Refresh();
            Calculate();
        }

        private void TextBox_LostKeyboardFocus_Bk(object sender, KeyboardFocusChangedEventArgs e)
        {
            int id = (int)(((TextBox)sender).Tag);
            double teethtemp = ((CTeeth)dataGridViewBk.Items[id - 1]).Teeth;
            try
            {
                teethtemp = Convert.ToInt16(((TextBox)sender).Text);
            }
            catch
            { }
            if (teethtemp > 200) teethtemp = 200;
            if (teethtemp < 1) teethtemp = 1;
            ((CTeeth)dataGridViewBk.Items[id - 1]).Teeth = teethtemp;
            dataGridViewBk.Items.Refresh();
            Calculate();
        }

        private void TextBox_LostKeyboardFocus_In(object sender, KeyboardFocusChangedEventArgs e)
        {
            int id = (int)(((TextBox)sender).Tag);
            double teethtemp = ((CTeeth)dataGridViewIn.Items[id - 1]).Teeth;
            try
            {
                teethtemp = Convert.ToDouble(((TextBox)sender).Text);
            }
            catch
            { }
            if (teethtemp > 20.0) teethtemp = 20.0;
            if (teethtemp < 0.01) teethtemp = 0.01;
            ((CTeeth)dataGridViewIn.Items[id - 1]).Teeth = teethtemp;
            dataGridViewIn.Items.Refresh();
            Calculate();
        }

        private void unitToolStripMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            kMHToolStripMenuItem.IsChecked = !Properties.Settings.Default.IsMPH;
            mPHToolStripMenuItem.IsChecked = Properties.Settings.Default.IsMPH;
        }

        private void ViewMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            internalHubsToolStripMenuItem.IsChecked = groupBox5.IsVisible;
            chartToolStripMenuItem.IsChecked = Chart1.IsVisible;
        }

        private void selectToolStripMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            selectToolStripMenuItem.Items.Clear();

            //StringSizeToFit
            MenuItem tempsize = new MenuItem();
            tempsize.Header = Properties.Resources.StringSizeToFit;
            tempsize.Click += tempsize_Click;
            tempsize.Visibility = Visibility.Collapsed; //temp
            selectToolStripMenuItem.Items.Add(tempsize);
            //selectToolStripMenuItem.Items.Add(new Separator());

            foreach (GridViewColumn ch in listBox1Grid.Columns)
            {
                MenuItem temp = new MenuItem();
                temp.Header = ch.Header;
                temp.IsChecked = (ch.Width != 0);
                temp.Click += temp_Click;
                temp.Tag = ch;
                selectToolStripMenuItem.Items.Add(temp);
            }
            selectToolStripMenuItem.Items.Add(new Separator());

            MenuItem tempall = new MenuItem();
            tempall.Header = Properties.Resources.StringSelectAll;
            tempall.Click += tempall_Click;
            tempall.Tag = listBox1Grid.Columns;
            selectToolStripMenuItem.Items.Add(tempall);
        }

        void tempsize_Click(object sender, RoutedEventArgs e)
        {
            
        }

        void temp_Click(object sender, EventArgs e)
        {

            if (((MenuItem)sender).IsChecked)
                ((GridViewColumn)((MenuItem)sender).Tag).Width = 0;
            else
                if (((GridViewColumn)((MenuItem)sender).Tag).Width == 0)
                    ((GridViewColumn)((MenuItem)sender).Tag).Width = 50;
             
        }

        void tempall_Click(object sender, EventArgs e)
        {
            foreach (GridViewColumn ch in listBox1Grid.Columns)
            {
                if (ch.Width == 0) ch.Width = 50;
            }
        }

        void ColumnHeaderContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            listBox1Grid.ColumnHeaderContextMenu.Items.Clear();
            
            //StringSizeToFit
            MenuItem tempsize = new MenuItem();
            tempsize.Header = Properties.Resources.StringSizeToFit;
            tempsize.Click += tempsize_Click;
            tempsize.Visibility = Visibility.Collapsed; //temp
            listBox1Grid.ColumnHeaderContextMenu.Items.Add(tempsize);
            //listBox1Grid.ColumnHeaderContextMenu.Items.Add(new Separator());

            foreach (GridViewColumn ch in listBox1Grid.Columns)
            {
                MenuItem temp = new MenuItem();
                temp.Header = ch.Header;
                temp.IsChecked = (ch.Width != 0);
                temp.Click += temp_Click;
                temp.Tag = ch;
                listBox1Grid.ColumnHeaderContextMenu.Items.Add(temp);
            }
            listBox1Grid.ColumnHeaderContextMenu.Items.Add(new Separator());

            MenuItem tempall = new MenuItem();
            tempall.Header = Properties.Resources.StringSelectAll;
            tempall.Click += tempall_Click;
            tempall.Tag = listBox1Grid.Columns;
            listBox1Grid.ColumnHeaderContextMenu.Items.Add(tempall);
        }

        private void saveToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (TabControlMain.SelectedIndex == 0)
                ExportFileTr();
            else if (TabControlMain.SelectedIndex == 1)
                ExportFileSp();
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader)
            {
                //Get clicked column
                GridViewColumn clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column;   //得到单击的列
                if (clickedColumn != null)
                {
                    //Get binding property of clicked column
                    string bindingProperty = (clickedColumn.DisplayMemberBinding as Binding).Path.Path; //得到单击列所绑定的属性
                    System.ComponentModel.SortDescriptionCollection sdc = listBox1.Items.SortDescriptions;
                    System.ComponentModel.ListSortDirection sortDirection = System.ComponentModel.ListSortDirection.Ascending;
                    if (sdc.Count > 0)
                    {
                        System.ComponentModel.SortDescription sd = sdc[0];
                        sortDirection = (System.ComponentModel.ListSortDirection)((((int)sd.Direction) + 1) % 2);  //判断此列当前的排序方式:升序0,倒序1,并取反进行排序。
                        sdc.Clear();
                    }
                    sdc.Add(new System.ComponentModel.SortDescription(bindingProperty, sortDirection));
                }
            }
        }
        
        private void aboutToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AboutWindow abw = new AboutWindow();
                abw.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void instructionsToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpWindow hpw = new HelpWindow();
                hpw.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void donateToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DonateWindow dnw = new DonateWindow();
                dnw.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void AddCurveToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddCurveNow();
        }

        private void ClrCurveToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            pm.Series.Clear(); 
            pm.Series.Add(lineSeriesCurrent);
            Calculate();
            
        }

        private void labelCAD_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ((MenuItem)((Label)sender).ContextMenu.Items[0]).IsChecked = !Properties.Settings.Default.IsMPH;
            ((MenuItem)((Label)sender).ContextMenu.Items[1]).IsChecked = Properties.Settings.Default.IsMPH;
        }

        private void listBox1_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ((MenuItem)((ListView)sender).ContextMenu.Items[0]).IsChecked = !Properties.Settings.Default.IsMPH;
            ((MenuItem)((ListView)sender).ContextMenu.Items[1]).IsChecked = Properties.Settings.Default.IsMPH;
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            

            Properties.Settings.Default.Save();
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void BranchDisToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch (CurveX)
            {
                case 0:
                    ((MenuItem)((MenuItem)sender).Items[0]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[1]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[2]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[4]).IsChecked = true;
                    break;
                case 1:
                    ((MenuItem)((MenuItem)sender).Items[0]).IsChecked = true;
                    ((MenuItem)((MenuItem)sender).Items[1]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[2]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[4]).IsChecked = false;
                    break;
                case 2:
                    ((MenuItem)((MenuItem)sender).Items[0]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[1]).IsChecked = true;
                    ((MenuItem)((MenuItem)sender).Items[2]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[4]).IsChecked = false;
                    break;
                case 3:
                    ((MenuItem)((MenuItem)sender).Items[0]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[1]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[2]).IsChecked = true;
                    ((MenuItem)((MenuItem)sender).Items[4]).IsChecked = false;
                    break;
            }
        }

        private void YAxisToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch (CurveY)
            {

                case 1:
                    ((MenuItem)((MenuItem)sender).Items[0]).IsChecked = true;
                    ((MenuItem)((MenuItem)sender).Items[1]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[2]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[3]).IsChecked = false;
                    break;
                case 2:
                    ((MenuItem)((MenuItem)sender).Items[0]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[1]).IsChecked = true;
                    ((MenuItem)((MenuItem)sender).Items[2]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[3]).IsChecked = false;
                    break;
                case 3:
                    ((MenuItem)((MenuItem)sender).Items[0]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[1]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[2]).IsChecked = true;
                    ((MenuItem)((MenuItem)sender).Items[3]).IsChecked = false;
                    break;
                case 4:
                    ((MenuItem)((MenuItem)sender).Items[0]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[1]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[2]).IsChecked = false;
                    ((MenuItem)((MenuItem)sender).Items[3]).IsChecked = true;
                    break;
            }
        }

        private void ResetAxisToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            pm.ResetAllAxes();
            Chart1.InvalidatePlot(false);
        }

        private void ResetAxisToolStripMenuItem1_Click(object sender, RoutedEventArgs e)
        {
            pm1.ResetAllAxes();
            Chart2.InvalidatePlot(false);
        }

        private void WheelLenthtextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                WheelLenthtextBox.Text = Convert.ToDouble(WheelLenthtextBox.Text).ToString("F0");
            }
            catch
            {
                CalculateWheelLenth();
            }
        }

        private void WheelcomboBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            WheelcomboBox.IsDropDownOpen = true;
        }

        private void WheelcomboBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            int temp = WheelcomboBox.SelectedIndex;
            WheelcomboBox.SelectedIndex = -1;
            WheelcomboBox.SelectedIndex = (temp==-1?1:temp);
        }

        CRim rimnow;
        private void RimNumcomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RimNumcomboBox.SelectedItem == null) return;
            bool rtemp = ready;
            ready = false;
            rimnow = (CRim)((CRim)RimNumcomboBox.SelectedItem).Clone();
            dataGridViewRim.Items.Clear();
            foreach (CValue v in rimnow.vals)
                dataGridViewRim.Items.Add(v);
            ready = rtemp;
            CalculateSpoke();
        }

        CHub hubnow;
        private void HubNumcomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HubNumcomboBox.SelectedItem == null) return;
            bool rtemp = ready;
            ready = false;
            hubnow = (CHub)((CHub)HubNumcomboBox.SelectedItem).Clone();
            dataGridViewHub.Items.Clear();
            foreach (CValue v in hubnow.vals)
                dataGridViewHub.Items.Add(v);
            ready = rtemp;
            CalculateSpoke();
        }

        private void rimMinuButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int no = Convert.ToInt32(b.CommandParameter);
            if (((CValue)dataGridViewRim.Items[no]).Val > 0.0)
                ((CValue)dataGridViewRim.Items[no]).Val -= 0.01;
            dataGridViewRim.Items.Refresh();
            CalculateSpoke();
        }

        private void rimAddButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int no = Convert.ToInt32(b.CommandParameter);
            if (((CValue)dataGridViewRim.Items[no]).Val < 1000.0)
                ((CValue)dataGridViewRim.Items[no]).Val += 0.01;
            dataGridViewRim.Items.Refresh();
            CalculateSpoke();
        }

        private void hubMinuButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int no = Convert.ToInt32(b.CommandParameter);
            if (((CValue)dataGridViewHub.Items[no]).Val > 0.0)
                ((CValue)dataGridViewHub.Items[no]).Val -= 0.01;
            dataGridViewHub.Items.Refresh();
            CalculateSpoke();
        }

        private void hubAddButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int no = Convert.ToInt32(b.CommandParameter);
            if (((CValue)dataGridViewHub.Items[no]).Val < 1000.0)
                ((CValue)dataGridViewHub.Items[no]).Val += 0.01;
            dataGridViewHub.Items.Refresh();
            CalculateSpoke();
        }

        private void hubtextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int no = (int)((TextBox)sender).Tag;
            if (e.Delta > 0)
            {
                if (((CValue)dataGridViewHub.Items[no]).Val < 1000.0)
                    ((CValue)dataGridViewHub.Items[no]).Val += 0.01;
            }
            else if (e.Delta < 0)
            {
                if (((CValue)dataGridViewHub.Items[no]).Val > 0.0)
                    ((CValue)dataGridViewHub.Items[no]).Val -= 0.01;
            }
            dataGridViewHub.Items.Refresh();
            CalculateSpoke();
        }

        private void rimtextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int no = (int)((TextBox)sender).Tag;
            if (e.Delta > 0)
            {
                if (((CValue)dataGridViewRim.Items[no]).Val < 1000.0)
                    ((CValue)dataGridViewRim.Items[no]).Val += 0.01;
            }
            else if (e.Delta < 0)
            {
                if (((CValue)dataGridViewRim.Items[no]).Val > 0.0)
                    ((CValue)dataGridViewRim.Items[no]).Val -= 0.01;
            }
            dataGridViewRim.Items.Refresh();
            CalculateSpoke();

        }

        private void rimTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            int no = (int)(((TextBox)sender).Tag);
            double val = ((CValue)dataGridViewRim.Items[no]).Val;
            try
            {
                val = Convert.ToDouble(((TextBox)sender).Text);
            }
            catch
            { }
            if (val > 1000) val = 1000;
            if (val < 0) val = 0;
            ((CValue)dataGridViewRim.Items[no]).Val = val;
            dataGridViewRim.Items.Refresh();
            CalculateSpoke();
        }

        private void hubTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            int no = (int)(((TextBox)sender).Tag);
            double val = ((CValue)dataGridViewHub.Items[no]).Val;
            try
            {
                val = Convert.ToDouble(((TextBox)sender).Text);
            }
            catch
            { }
            if (val > 1000) val = 1000;
            if (val < 0) val = 0;
            ((CValue)dataGridViewHub.Items[no]).Val = val;
            dataGridViewHub.Items.Refresh();
            CalculateSpoke();
        }

        private void CalculateSpoke()
        {
            if (!ready) return;
            pm1.Series.Clear();
            //pm.Axes[0].MinorStep = double.NaN;
            //pm.Axes[0].MajorStep = double.NaN;
            //CHub hubtemp = frnow;
            CHub hubtemp = hubnow;
            CRim rimtemp = rimnow;
            int Spokes;
            int Crosses;
            try
            {
                Spokes = Convert.ToInt16(TextBoxSpokes.Text);
                if (Spokes < 4) Spokes = 4;
                if (Spokes > 100) Spokes = 100;
            }
            catch
            {
                return;
            }  
         
            try
            {
                Crosses = Convert.ToInt16(TextBoxCrosses.Text);
            }
            catch
            {
                return;
            } 


            if (Spokes <= 3) return;


            ///////////////左侧///////////////////////
            //花鼓孔
            var hubhole = new ScatterSeries();
            hubhole.MarkerType = MarkerType.Circle;
            hubhole.MarkerSize = 3;
            hubhole.Title = Properties.Resources.StringHubHoleL;
            for (int i = 0; i < Spokes / 2; i++)
            {
                double theta = 2.0 * Math.PI / (Spokes / 2.0) * i;
                double x = Math.Sin(theta) * hubtemp.LeftFlange / 2.0;
                double y = Math.Cos(theta) * hubtemp.LeftFlange / 2.0;
                ScatterPoint scatterPoint = new ScatterPoint(x, y);
                hubhole.Points.Add(scatterPoint);
            }
            //轮圈孔
            var rimhole = new ScatterSeries();
            rimhole.MarkerType = MarkerType.Circle;
            rimhole.MarkerSize = 3;
            for (int i = 0; i < Spokes / 2; i++)
            {
                double theta = 2.0 * Math.PI / (Spokes / 2.0) * i;
                double x = Math.Sin(theta) * rimtemp.ERD / 2.0;
                double y = Math.Cos(theta) * rimtemp.ERD / 2.0;
                ScatterPoint scatterPoint = new ScatterPoint(x, y);
                rimhole.Points.Add(scatterPoint);
            }
            //轮圈线
            var rimline = new LineSeries();
            rimline.StrokeThickness = 2;
            for (int i = 0; i < 101; i++)
            {
                double theta = 2.0 * Math.PI / 100.0 * i;
                double x = Math.Sin(theta) * rimtemp.ERD / 2.0;
                double y = Math.Cos(theta) * rimtemp.ERD / 2.0;
                rimline.Points.Add(new DataPoint(x, y));
            }
            //花鼓线
            var hubline = new LineSeries();
            hubline.StrokeThickness = 2;
            for (int i = 0; i < 101; i++)
            {
                double theta = 2.0 * Math.PI / 100.0 * i;
                double x = Math.Sin(theta) * hubtemp.LeftFlange / 2.0 * 1.2;
                double y = Math.Cos(theta) * hubtemp.LeftFlange / 2.0 * 1.2;
                hubline.Points.Add(new DataPoint(x, y));
            }
            hubline.Points.Add(new DataPoint(double.NaN,double.NaN));
            for (int i = 0; i < 101; i++)
            {
                double theta = 2.0 * Math.PI / 100.0 * i;
                double x = Math.Sin(theta) * 7.0;
                double y = Math.Cos(theta) * 7.0;
                hubline.Points.Add(new DataPoint(x, y));
            }
            //辐条
            var spokesline = new LineSeries();
            spokesline.Title = Properties.Resources.StringSpokeL;
            spokesline.StrokeThickness = 1;
            double spokeslenthraw = 0;
            for (int i = 0; i < Spokes / 2; i++)
            {
                DataPoint p1 = new DataPoint();
                DataPoint p2 = new DataPoint();
                p1.X = hubhole.Points[i].X;
                p1.Y = hubhole.Points[i].Y;
                int j = 0;
                if (i % 2 == 0)
                {
                    j = i + Crosses;
                }
                else
                {
                    j = i - Crosses;
                }
                while (j < 0) j += Spokes / 2;
                j = j % (Spokes / 2);
                p2.X = rimhole.Points[j].X;
                p2.Y = rimhole.Points[j].Y;
                if (spokeslenthraw == 0)
                    spokeslenthraw = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
                spokesline.Points.Add(p1);
                spokesline.Points.Add(p2);
                spokesline.Points.Add(new DataPoint(double.NaN, double.NaN));
            }


            /////////////////////右侧///////////////////////
            //花鼓孔
            var hubholer = new ScatterSeries();
            hubholer.MarkerSize = 3;
            hubholer.Title = Properties.Resources.StringHubHoleR;
            hubholer.MarkerType = MarkerType.Circle;
            for (int i = 0; i < Spokes / 2; i++)
            {
                double theta = 2.0 * Math.PI / (Spokes / 2.0) * (i + 0.5);
                double x = Math.Sin(theta) * hubtemp.RightFlange / 2.0;
                double y = Math.Cos(theta) * hubtemp.RightFlange / 2.0;
                ScatterPoint scatterPoint = new ScatterPoint(x, y);
                hubholer.Points.Add(scatterPoint);
            }            
            //轮圈孔
            var rimholer = new ScatterSeries();
            rimholer.MarkerSize = 3;
            rimholer.MarkerType = MarkerType.Circle;
            for (int i = 0; i < Spokes / 2; i++)
            {
                double theta = 2.0 * Math.PI / (Spokes / 2.0) * (i + 0.5);
                double x = Math.Sin(theta) * rimtemp.ERD / 2.0;
                double y = Math.Cos(theta) * rimtemp.ERD / 2.0;
                ScatterPoint scatterPoint = new ScatterPoint(x, y);
                rimholer.Points.Add(scatterPoint);
            }
            //辐条
            var spokesliner = new LineSeries();
            spokesliner.Title = Properties.Resources.StringSpokeR;
            spokesliner.StrokeThickness = 1;
            double spokeslenthrawr = 0;
            for (int i = 0; i < Spokes / 2; i++)
            {
                DataPoint p1 = new DataPoint();
                DataPoint p2 = new DataPoint();
                p1.X = hubholer.Points[i].X;
                p1.Y = hubholer.Points[i].Y;
                int j = 0;
                if (i % 2 == 0)
                {
                    j = i + Crosses;
                }
                else
                {
                    j = i - Crosses;
                }
                while (j < 0) j += Spokes / 2;
                j = j % (Spokes / 2);
                p2.X = rimholer.Points[j].X;
                p2.Y = rimholer.Points[j].Y;
                if (spokeslenthrawr == 0)
                    spokeslenthrawr = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
                spokesliner.Points.Add(p1);
                spokesliner.Points.Add(p2);
                spokesliner.Points.Add(new DataPoint(double.NaN, double.NaN));
            }

            if (disrimline) pm1.Series.Add(rimline);
            if (dishubline) pm1.Series.Add(hubline);
            if (disrightside && dishubhole) pm1.Series.Add(hubholer);
            if (disleftside && dishubhole) pm1.Series.Add(hubhole);
            if (disrightside && disrimhole) pm1.Series.Add(rimholer);
            if (disleftside && disrimhole) pm1.Series.Add(rimhole);
            if (disrightside && disspokesline) pm1.Series.Add(spokesliner);
            if (disleftside && disspokesline) pm1.Series.Add(spokesline);
            Chart2.ResetAllAxes();
            Chart2.InvalidatePlot(true);


            listBox2.Items.Clear();

            for (int i = 0; i < 6; i++)
            {
                DataPoint p1 = new DataPoint();
                DataPoint p2 = new DataPoint();
                p1.X = hubhole.Points[0].X;
                p1.Y = hubhole.Points[0].Y;
                while (i < 0) i += Spokes / 2;
                int j = i % (Spokes / 2);
                p2.X = rimhole.Points[j].X;
                p2.Y = rimhole.Points[j].Y;
                spokeslenthraw = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);

                p1.X = hubholer.Points[0].X;
                p1.Y = hubholer.Points[0].Y;
                while (i < 0) i += Spokes / 2;
                j = i % (Spokes / 2);
                p2.X = rimholer.Points[j].X;
                p2.Y = rimholer.Points[j].Y;
                spokeslenthrawr = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);


                CSpokeResult resulttemp = new CSpokeResult();
                resulttemp.Crosses = i;
                double offsetl = hubtemp.CenterToLeft - rimtemp.OSB;
                double offsetr = hubtemp.CenterToRight + rimtemp.OSB;
                resulttemp.Lenthleft = Math.Sqrt(spokeslenthraw + offsetl * offsetl) - hubtemp.SpokeHole / 2.0;
                resulttemp.Lenthright = Math.Sqrt(spokeslenthrawr + offsetr * offsetr) - hubtemp.SpokeHole / 2.0; ;

                double sinphyl = offsetl / Math.Sqrt(spokeslenthraw);
                double sinphyr = offsetr / Math.Sqrt(spokeslenthrawr);
                resulttemp.Tensionratio = sinphyr / sinphyl;
                resulttemp.Remark = "";
                listBox2.Items.Add(resulttemp);
            }
            if (Crosses > 5)
            {
                int i = Crosses;
                DataPoint p1 = new DataPoint();
                DataPoint p2 = new DataPoint();
                p1.X = hubhole.Points[0].X;
                p1.Y = hubhole.Points[0].Y;
                while (i < 0) i += Spokes / 2;
                int j = i % (Spokes / 2);
                p2.X = rimhole.Points[j].X;
                p2.Y = rimhole.Points[j].Y;
                spokeslenthraw = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);

                p1.X = hubholer.Points[0].X;
                p1.Y = hubholer.Points[0].Y;
                while (i < 0) i += Spokes / 2;
                j = i % (Spokes / 2);
                p2.X = rimholer.Points[j].X;
                p2.Y = rimholer.Points[j].Y;
                spokeslenthrawr = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);


                CSpokeResult resulttemp = new CSpokeResult();
                resulttemp.Crosses = i;
                double offsetl = hubtemp.CenterToLeft - rimtemp.OSB;
                double offsetr = hubtemp.CenterToRight + rimtemp.OSB;
                resulttemp.Lenthleft = Math.Sqrt(spokeslenthraw + offsetl * offsetl) - hubtemp.SpokeHole / 2.0;
                resulttemp.Lenthright = Math.Sqrt(spokeslenthrawr + offsetr * offsetr) - hubtemp.SpokeHole / 2.0; ;

                double sinphyl = offsetl / Math.Sqrt(spokeslenthraw);
                double sinphyr = offsetr / Math.Sqrt(spokeslenthrawr);
                resulttemp.Tensionratio = sinphyr / sinphyl;
                resulttemp.Remark = "";
                listBox2.Items.Add(resulttemp);
            }
        }

        private void OnSpokeAddButtonClick(object sender, RoutedEventArgs e)
        {
            int val = Convert.ToInt16(TextBoxSpokes.Text);
                if (val < 100)
                    TextBoxSpokes.Text = (val + 1).ToString();
        }

        private void OnSpokeMinButtonClick(object sender, RoutedEventArgs e)
        {
            int val = Convert.ToInt16(TextBoxSpokes.Text);
            if (val > 4)
                TextBoxSpokes.Text = (val - 1).ToString();
        }

        private void OnCrossAddButtonClick(object sender, RoutedEventArgs e)
        {
            int val = Convert.ToInt16(TextBoxCrosses.Text);
            if (val < 65535)
                TextBoxCrosses.Text = (val + 1).ToString();
        }

        private void OnCrossMinButtonClick(object sender, RoutedEventArgs e)
        {
            int val = Convert.ToInt16(TextBoxCrosses.Text);
            if (val > 0)
                TextBoxCrosses.Text = (val - 1).ToString();
        }

        private void TextBoxCrosses_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int temp = Convert.ToInt16(((TextBox)sender).Text);
                if (!((TextBox)sender).IsKeyboardFocused && temp < 0) temp = 0;
                if (temp > 100) temp = 100;
                ((TextBox)sender).Text = temp.ToString();
            }
            catch
            {
                int temp = 32;
                ((TextBox)sender).Text = temp.ToString();
            }
            CalculateSpoke();
        }


        private void TextBoxSpokes_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int temp = Convert.ToInt16(((TextBox)sender).Text);
                if (!((TextBox)sender).IsKeyboardFocused && temp < 4) temp = 4;
                if (temp > 100) temp = 100;
                ((TextBox)sender).Text = temp.ToString();
            }
            catch
            {
                int temp = 32;
                ((TextBox)sender).Text = temp.ToString();
            }
            CalculateSpoke();
        }


        bool disleftside = true;
        bool disrightside = true;
        private void LRSideDisToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((MenuItem)((MenuItem)sender).Items[0]).IsChecked = disleftside;
            ((MenuItem)((MenuItem)sender).Items[1]).IsChecked = disrightside;
        }

        private void DisLeftToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            disleftside = !disleftside;
            CalculateSpoke();
        }

        private void DisRightToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            disrightside = !disrightside;
            CalculateSpoke();
        }


        bool disrimline = true;
        bool dishubline = true;
        bool dishubhole = true;
        bool disrimhole = true;
        bool disspokesline = true;
        private void DisItemStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((MenuItem)((MenuItem)sender).Items[0]).IsChecked = disrimline;
            ((MenuItem)((MenuItem)sender).Items[1]).IsChecked = disrimhole;
            ((MenuItem)((MenuItem)sender).Items[2]).IsChecked = dishubline;
            ((MenuItem)((MenuItem)sender).Items[3]).IsChecked = dishubhole;
            ((MenuItem)((MenuItem)sender).Items[4]).IsChecked = disspokesline;
        }

        private void DisAllToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            disleftside = true;
            disrightside = true;
            disrimline = true;
            disrimhole = true;
            dishubline = true;
            dishubhole = true;
            disspokesline = true;
            CalculateSpoke();

        }

        private void RimlineToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            disrimline = !disrimline;
            CalculateSpoke();

        }

        private void RimholeToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            disrimhole = !disrimhole;
            CalculateSpoke();

        }

        private void HublineToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            dishubline = !dishubline;
            CalculateSpoke();

        }

        private void HubholeToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            dishubhole = !dishubhole;
            CalculateSpoke();

        }

        private void SpolineToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            disspokesline = !disspokesline;
            CalculateSpoke();

        }

        private void PlotToolStripMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            //TabControlMain.SelectedIndex
            foreach (var item in ((MenuItem)sender).Items)
            {
                if (item is MenuItem)
                {
                    int temp = Convert.ToInt16((string)((MenuItem)item).Tag);
                    ((MenuItem)item).Visibility = ((temp == TabControlMain.SelectedIndex + 1) ? Visibility.Visible : Visibility.Collapsed);
                }
                else if (item is Separator)
                {
                    int temp = Convert.ToInt16((string)((Separator)item).Tag);
                    ((Separator)item).Visibility = ((temp == TabControlMain.SelectedIndex + 1) ? Visibility.Visible : Visibility.Collapsed);
                }

            }
        }

        private void TabControlMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewMenuItem.Visibility = ((TabControlMain.SelectedIndex == 0) ? Visibility.Visible : Visibility.Collapsed);
            RusultMenuItem.Visibility = ((TabControlMain.SelectedIndex == 0) ? Visibility.Visible : Visibility.Collapsed);
        }

        private void TextBoxCrosses_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                int temp = Convert.ToInt16(((TextBox)sender).Text);
                if (temp < 4) ((TextBox)sender).Text = "4";
                if (temp > 100) ((TextBox)sender).Text = "100";
            }
            catch
            {
                ((TextBox)sender).Text = "32";
            }
        }

        private void LanguageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            ((MenuItem)mi.Items[0]).IsChecked = (Properties.Settings.Default.Language == 0);
            ((MenuItem)mi.Items[2]).IsChecked = (Properties.Settings.Default.Language == 2);
            ((MenuItem)mi.Items[3]).IsChecked = (Properties.Settings.Default.Language == 3);
        }

        private void DftLanguageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = 0;
            MessageBox.Show(Properties.Resources.StringRestartEff);
            Properties.Settings.Default.Save();
        }

        private void EngMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = 2;
            MessageBox.Show(Properties.Resources.StringRestartEff);
            Properties.Settings.Default.Save();
        }

        private void ChnMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = 3;
            MessageBox.Show(Properties.Resources.StringRestartEff);
            Properties.Settings.Default.Save();
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.frnumid = FrNumcomboBox.SelectedIndex;
            Properties.Settings.Default.frmodid = FrModelcomboBox.SelectedIndex;
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

            Properties.Settings.Default.bknumid = BkNumcomboBox.SelectedIndex;
            Properties.Settings.Default.bkmodid = BkModelcomboBox.SelectedIndex;
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

            Properties.Settings.Default.innumid = InNumcomboBox.SelectedIndex;
            Properties.Settings.Default.inmodid = InModelcomboBox.SelectedIndex;
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

            Properties.Settings.Default.IsSpd = checkBox1.IsChecked.Value;
            Properties.Settings.Default.SpdVal = trackBar1.Value;
            Properties.Settings.Default.IsISO = checkBox2.IsChecked.Value;

            Properties.Settings.Default.wheelid = WheelcomboBox.SelectedIndex;
            Properties.Settings.Default.wheellen = Convert.ToInt32(WheelLenthtextBox.Text);
            Properties.Settings.Default.numUD1 = Convert.ToDouble(numericUpDown1.Text);
            Properties.Settings.Default.numUD2 = Convert.ToDouble(numericUpDown2.Text);

            //////////////////////////GUI/////////////////////////////////////////////////
            Properties.Settings.Default.InputCWidth = InputColumn.ActualWidth / GridT.ActualWidth;
            Properties.Settings.Default.UpRHight = UpRow.ActualHeight / GridT.ActualHeight;

            Properties.Settings.Default.InputC1Width = InputColumn1.ActualWidth / GridS.ActualWidth;
            Properties.Settings.Default.UpR1Hight = UpRow1.ActualHeight / GridS.ActualHeight;

            Properties.Settings.Default.IsWinMax = (this.WindowState == System.Windows.WindowState.Maximized);

            Properties.Settings.Default.WinW = this.Width;
            Properties.Settings.Default.WinH = this.Height;

            Properties.Settings.Default.TabSlt = TabControlMain.SelectedIndex;
            ////////////////////////////////////////////////////////////////////////////////
        }


        #region 程序集特性访问器

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        #endregion

        private void EditCassetteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            GearsEditor geditor = new GearsEditor(data.bklists, data, Properties.Resources.StringCassette + Properties.Resources.StringEditDataSet);
            geditor.ShowDialog();
            data.LoadData();
            RefreshData();
        }

        private void EditCranksetsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            GearsEditor geditor = new GearsEditor(data.frlists, data, Properties.Resources.StringCranksets + Properties.Resources.StringEditDataSet);
            geditor.ShowDialog();
            data.LoadData();
            RefreshData();
        }

        private void EditInternalHubMenuItem_Click(object sender, RoutedEventArgs e)
        {
            GearsEditor geditor = new GearsEditor(data.inlists, data, Properties.Resources.StringInternalHub + Properties.Resources.StringEditDataSet);
            geditor.ShowDialog();
            data.LoadData();
            RefreshData();
        }

        private void EditTireMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TiresEditor teditor = new TiresEditor(data.wheelList, data, Properties.Resources.StringTire + Properties.Resources.StringEditDataSet);
            teditor.ShowDialog();
            data.LoadData();
            RefreshData();
        }

        private void EditHubsMenuItem_Click(object sender, RoutedEventArgs e)
        {

            data.LoadData();
            RefreshData();
        }

        private void EditRimsMenuItem_Click(object sender, RoutedEventArgs e)
        {

            data.LoadData();
            RefreshData();
        }
    }
}
