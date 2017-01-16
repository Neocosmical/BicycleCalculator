﻿using System;
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

        CTransmissionCal TransmissionCal = new CTransmissionCal();
        CWheelCal WheelCal = new CWheelCal();
        
        CData data = new CData();

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
            TransmissionCal.ready = false;
            WheelCal.ready = false;

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

            TransmissionCal.ready = true;
            WheelCal.ready = true;
            TransmissionCal.Calculate();
            WheelCal.CalculateSpoke();
        }

        System.Timers.Timer timerload1;
        private void outputAction()
        {
            TransmissionCal.ready = false;
            WheelCal.ready = false;

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

            TransmissionCal.Init(Chart1, listBox1, labelinfo, labelinfo1);
            WheelCal.Init(Chart2, listBox2);

            HubNumcomboBox.ItemsSource = data.hublist;
            RimNumcomboBox.ItemsSource = data.rimlist;

            HubNumcomboBox.SelectedIndex = 1;
            RimNumcomboBox.SelectedIndex = 1;



            checkBox1.IsChecked = Properties.Settings.Default.IsSpd;
            trackBar1.Value = Properties.Settings.Default.SpdVal;
            checkBox2.IsChecked = Properties.Settings.Default.IsISO;
            tableLayoutPanel10.Visibility = checkBox2.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
            WheelcomboBox.Visibility = checkBox2.IsChecked.Value ? Visibility.Hidden : Visibility.Visible;

            WheelLenthtextBox.Text = Convert.ToString(Properties.Settings.Default.wheellen);
            numericUpDown1.Text = Convert.ToString(Properties.Settings.Default.numUD1);
            numericUpDown2.Text = Convert.ToString(Properties.Settings.Default.numUD2);

            TabControlMain.SelectedIndex = Properties.Settings.Default.TabSlt;

            TransmissionCal.ready = true;
            WheelCal.ready = true;

            checkBox1_Click(null, null);

            TransmissionCal.Calculate();
            WheelCal.CalculateSpoke();

            

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
            bool rtemp = TransmissionCal.ready;
            TransmissionCal.ready = false;
            FrModelcomboBox.Items.Clear();


            foreach (CGear gear in ((CGearList)FrNumcomboBox.SelectedItem).Gears)
                FrModelcomboBox.Items.Add(gear);

            FrModelcomboBox.SelectedIndex = 0;
            TransmissionCal.ready = rtemp;
            TransmissionCal.Calculate();
        }

        private void BkNumcomboBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BkNumcomboBox.SelectedItem == null) return;
            bool rtemp = TransmissionCal.ready;
            TransmissionCal.ready = false;
            BkModelcomboBox.Items.Clear();

            foreach (CGear gear in ((CGearList)BkNumcomboBox.SelectedItem).Gears)
                BkModelcomboBox.Items.Add(gear);

            BkModelcomboBox.SelectedIndex = 0;
            TransmissionCal.ready = rtemp;
            TransmissionCal.Calculate();
        }

        private void InNumcomboBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InNumcomboBox.SelectedItem == null) return;
            bool rtemp = TransmissionCal.ready;
            TransmissionCal.ready = false;
            InModelcomboBox.Items.Clear();

            foreach (CGear gear in ((CGearList)InNumcomboBox.SelectedItem).Gears)
                InModelcomboBox.Items.Add(gear);
            InModelcomboBox.SelectedIndex = 0;
            TransmissionCal.ready = rtemp;
            TransmissionCal.Calculate();
        }

        private void FrModelcomboBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FrModelcomboBox.SelectedItem == null) return;

            bool rtemp = TransmissionCal.ready;
            TransmissionCal.ready = false;
            TransmissionCal.frnow = (CGear)((CGear)FrModelcomboBox.SelectedItem).Clone();
            dataGridViewFr.Items.Clear();
            for (int i = 0; i < TransmissionCal.frnow.Speeds; i++) dataGridViewFr.Items.Add(TransmissionCal.frnow.teeth[i]);
            TransmissionCal.ready = rtemp;
            TransmissionCal.Calculate();
        }

        private void BkModelcomboBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BkModelcomboBox.SelectedItem == null) return;

            bool rtemp = TransmissionCal.ready;
            TransmissionCal.ready = false;
            TransmissionCal.bknow = (CGear)((CGear)BkModelcomboBox.SelectedItem).Clone();
            dataGridViewBk.Items.Clear();
            for (int i = 0; i < TransmissionCal.bknow.Speeds; i++) dataGridViewBk.Items.Add(TransmissionCal.bknow.teeth[i]);
            TransmissionCal.ready = rtemp;
            TransmissionCal.Calculate();
        }

        private void InModelcomboBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InModelcomboBox.SelectedItem == null) return;
            bool rtemp = TransmissionCal.ready;
            TransmissionCal.ready = false;
            TransmissionCal.innow = (CGear)((CGear)InModelcomboBox.SelectedItem).Clone();
            dataGridViewIn.Items.Clear();
            for (int i = 0; i < TransmissionCal.innow.Speeds; i++) dataGridViewIn.Items.Add(TransmissionCal.innow.teeth[i]);
            if (TransmissionCal.innow.Speeds <= 1) dataGridViewIn.IsEnabled = false;
            else dataGridViewIn.IsEnabled = true;
            TransmissionCal.ready = rtemp;
            TransmissionCal.Calculate();
        }
        
        private void WheelcomboBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            CWheel temp = (CWheel)WheelcomboBox.SelectedItem;
            if (temp == null) return;
            WheelLenthtextBox.Text = temp.Lenth.ToString();
            TransmissionCal.whnow = temp;
            TransmissionCal.whnow_index = WheelcomboBox.SelectedIndex;
        }

        private void WheelLenthtextBox_ValueChanged(object sender, EventArgs e)
        {
            TransmissionCal.Calculate();
        }


        
        

        private void listBox1_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            dataGridViewFr.SelectedIndex = -1;
            dataGridViewBk.SelectedIndex = -1;
            dataGridViewIn.SelectedIndex = -1;
            TransmissionCal.pm.Annotations.Clear();

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
                foreach (int nan in TransmissionCal.NaNNumber)
                    if (n > nan-2) nanadd++;
                pointAnnotation1.X = TransmissionCal.lineSeriesCurrent.Points[n + nanadd].X;
                pointAnnotation1.Y = TransmissionCal.lineSeriesCurrent.Points[n + nanadd].Y;
                pointAnnotation1.Size = 6;
                pointAnnotation1.Fill = OxyColors.SkyBlue;
                pointAnnotation1.Stroke = OxyColors.DarkBlue;
                pointAnnotation1.StrokeThickness = 1;
                pointAnnotation1.Text = item.Gear1;
                TransmissionCal.pm.Annotations.Add(pointAnnotation1);

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
            TransmissionCal.CurveX = 0;
            TransmissionCal.Calculate();
        }

        private void WithCrankToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TransmissionCal.CurveX = 1;
            TransmissionCal.Calculate();
        }

        private void WithCassToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TransmissionCal.CurveX = 2;
            TransmissionCal.Calculate();
        }

        private void WithInterToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TransmissionCal.CurveX = 3;
            TransmissionCal.Calculate();
        }
        
        private void SRToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TransmissionCal.CurveY = 1;
            TransmissionCal.Calculate();
        }

        private void GIToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TransmissionCal.CurveY = 2;
            TransmissionCal.Calculate();
        }

        private void GRToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TransmissionCal.CurveY = 3;
            TransmissionCal.Calculate();
        }

        private void SpdToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TransmissionCal.CurveY = 4;
            TransmissionCal.Calculate();
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
            TransmissionCal.Calculate();
        }

        private void mPHToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.IsMPH = true;
            TransmissionCal.Calculate();
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
            TransmissionCal.Calculate();
        }

        private void frAddButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridViewFr.Items[id - 1]).teeth < 200)
                ((CTeeth)dataGridViewFr.Items[id - 1]).teeth++;
            dataGridViewFr.Items.Refresh();
            TransmissionCal.Calculate();
        }

        private void bkMinuButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridViewBk.Items[id - 1]).teeth > 1)
                ((CTeeth)dataGridViewBk.Items[id - 1]).teeth--;
            dataGridViewBk.Items.Refresh();
            TransmissionCal.Calculate();
        }

        private void bkAddButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridViewBk.Items[id - 1]).teeth < 200)
                ((CTeeth)dataGridViewBk.Items[id - 1]).teeth++;
            dataGridViewBk.Items.Refresh();
            TransmissionCal.Calculate();
        }

        private void inMinuButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridViewIn.Items[id - 1]).teeth > 0.01)
                ((CTeeth)dataGridViewIn.Items[id - 1]).teeth -= 0.01;
            dataGridViewIn.Items.Refresh();
            TransmissionCal.Calculate();
        }

        private void inAddButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridViewIn.Items[id - 1]).teeth < 20.0)
                ((CTeeth)dataGridViewIn.Items[id - 1]).teeth += 0.01;
            dataGridViewIn.Items.Refresh();
            TransmissionCal.Calculate();
        }

        private void trackBar1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!TransmissionCal.ready) return;
            TransmissionCal.cad = trackBar1.Value;
            TransmissionCal.Calculate();

            if (TransmissionCal.IsSpd)
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

        }

        private void checkBox1_Click(object sender, RoutedEventArgs e)
        {
            //if (!TransmissionCal.ready) return;
            if (checkBox1.IsChecked.Value)
            {
                trackBar1.Value = 50;
                timerCAD.Enabled = false;
                labelCAD.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                if (Properties.Settings.Default.IsMPH)
                    labelCAD.Content = (trackBar1.Value / 2.0).ToString("0.0") + " mph";
                else
                    labelCAD.Content = (trackBar1.Value / 2.0).ToString("0.0") + " km/h";
                SpeedColumn.Header = Properties.Resources.StringCAD + " rpm";
            }
            else
            {
                trackBar1.Value = 80;
                timerCAD.Enabled = true;
                labelCAD.Content = trackBar1.Value.ToString("0.0") + " rpm";
                if (Properties.Settings.Default.IsMPH)
                    SpeedColumn.Header = Properties.Resources.StringSpeed + " mph";
                else
                    SpeedColumn.Header = Properties.Resources.StringSpeed + " km/h";
            }
            TransmissionCal.IsSpd = checkBox1.IsChecked.Value;
            TransmissionCal.Calculate();
        }

        private void WheelLenthtextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try { TransmissionCal.whlength = Convert.ToInt32(WheelLenthtextBox.Text); } catch { }
            TransmissionCal.Calculate();
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
            TransmissionCal.Calculate();
        }

        private void TextBox_MouseWheel_Bk(object sender, MouseWheelEventArgs e)
        {
            int id = (int)(((TextBox)sender).Tag);
            if (((CTeeth)dataGridViewBk.Items[id - 1]).Teeth > 1 && e.Delta < 0)
                ((CTeeth)dataGridViewBk.Items[id - 1]).Teeth -= 1;
            if (((CTeeth)dataGridViewBk.Items[id - 1]).Teeth < 200 && e.Delta > 0)
                ((CTeeth)dataGridViewBk.Items[id - 1]).Teeth += 1;
            dataGridViewBk.Items.Refresh();
            TransmissionCal.Calculate();
        }

        private void TextBox_MouseWheel_In(object sender, MouseWheelEventArgs e)
        {
            int id = (int)(((TextBox)sender).Tag);
            if (((CTeeth)dataGridViewIn.Items[id - 1]).Teeth > 0.01 && e.Delta < 0)
                ((CTeeth)dataGridViewIn.Items[id - 1]).Teeth -= 0.01;
            if (((CTeeth)dataGridViewIn.Items[id - 1]).Teeth < 20.0 && e.Delta > 0)
                ((CTeeth)dataGridViewIn.Items[id - 1]).Teeth += 0.01;

            dataGridViewIn.Items.Refresh();
            TransmissionCal.Calculate();
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
            TransmissionCal.Calculate();
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
            TransmissionCal.Calculate();
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
            TransmissionCal.Calculate();
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
                TransmissionCal.ExportFileTr();
            else if (TabControlMain.SelectedIndex == 1)
                WheelCal.ExportFileSp();
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
            TransmissionCal.AddCurveNow();
        }

        private void ClrCurveToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TransmissionCal.ClearCurve();
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
            switch (TransmissionCal.CurveX)
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
            switch (TransmissionCal.CurveY)
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
            TransmissionCal.pm.ResetAllAxes();
            Chart1.InvalidatePlot(false);
        }

        private void ResetAxisToolStripMenuItem1_Click(object sender, RoutedEventArgs e)
        {
            WheelCal.pm1.ResetAllAxes();
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

        private void RimNumcomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RimNumcomboBox.SelectedItem == null) return;
            bool rtemp = WheelCal.ready;
            WheelCal.ready = false;
            WheelCal.rimnow = (CRim)((CRim)RimNumcomboBox.SelectedItem).Clone();
            dataGridViewRim.Items.Clear();
            foreach (CValue v in WheelCal.rimnow.vals)
                dataGridViewRim.Items.Add(v);
            WheelCal.ready = rtemp;
            WheelCal.CalculateSpoke();
        }

        private void HubNumcomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HubNumcomboBox.SelectedItem == null) return;
            bool rtemp = WheelCal.ready;
            WheelCal.ready = false;
            WheelCal.hubnow = (CHub)((CHub)HubNumcomboBox.SelectedItem).Clone();
            dataGridViewHub.Items.Clear();
            foreach (CValue v in WheelCal.hubnow.vals)
                dataGridViewHub.Items.Add(v);
            WheelCal.ready = rtemp;
            WheelCal.CalculateSpoke();
        }

        private void rimMinuButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int no = Convert.ToInt32(b.CommandParameter);
            if (((CValue)dataGridViewRim.Items[no]).Val > 0.0)
                ((CValue)dataGridViewRim.Items[no]).Val -= 0.01;
            dataGridViewRim.Items.Refresh();
            WheelCal.CalculateSpoke();
        }

        private void rimAddButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int no = Convert.ToInt32(b.CommandParameter);
            if (((CValue)dataGridViewRim.Items[no]).Val < 1000.0)
                ((CValue)dataGridViewRim.Items[no]).Val += 0.01;
            dataGridViewRim.Items.Refresh();
            WheelCal.CalculateSpoke();
        }

        private void hubMinuButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int no = Convert.ToInt32(b.CommandParameter);
            if (((CValue)dataGridViewHub.Items[no]).Val > 0.0)
                ((CValue)dataGridViewHub.Items[no]).Val -= 0.01;
            dataGridViewHub.Items.Refresh();
            WheelCal.CalculateSpoke();
        }

        private void hubAddButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int no = Convert.ToInt32(b.CommandParameter);
            if (((CValue)dataGridViewHub.Items[no]).Val < 1000.0)
                ((CValue)dataGridViewHub.Items[no]).Val += 0.01;
            dataGridViewHub.Items.Refresh();
            WheelCal.CalculateSpoke();
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
            WheelCal.CalculateSpoke();
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
            WheelCal.CalculateSpoke();

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
            WheelCal.CalculateSpoke();
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
            WheelCal.CalculateSpoke();
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
            int temp;
            try
            {
                temp = Convert.ToInt16(((TextBox)sender).Text);
                if (!((TextBox)sender).IsKeyboardFocused && temp < 0) temp = 0;
                if (temp > 100) temp = 100;
                ((TextBox)sender).Text = temp.ToString();
            }
            catch
            {
                temp = 32;
                ((TextBox)sender).Text = temp.ToString();
            }
            WheelCal.Crosses = temp;
            WheelCal.CalculateSpoke();
        }


        private void TextBoxSpokes_TextChanged(object sender, TextChangedEventArgs e)
        {
            int temp;
            try
            {
                temp = Convert.ToInt16(((TextBox)sender).Text);
                if (!((TextBox)sender).IsKeyboardFocused && temp < 4) temp = 4;
                if (temp > 100) temp = 100;
                ((TextBox)sender).Text = temp.ToString();
            }
            catch
            {
                temp = 32;
                ((TextBox)sender).Text = temp.ToString();
            }
            WheelCal.Spokes = temp;
            WheelCal.CalculateSpoke();
        }

        private void LRSideDisToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((MenuItem)((MenuItem)sender).Items[0]).IsChecked = WheelCal.disleftside;
            ((MenuItem)((MenuItem)sender).Items[1]).IsChecked = WheelCal.disrightside;
        }

        private void DisLeftToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WheelCal.disleftside = !WheelCal.disleftside;
            WheelCal.CalculateSpoke();
        }

        private void DisRightToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WheelCal.disrightside = !WheelCal.disrightside;
            WheelCal.CalculateSpoke();
        }

        private void DisItemStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((MenuItem)((MenuItem)sender).Items[0]).IsChecked = WheelCal.disrimline;
            ((MenuItem)((MenuItem)sender).Items[1]).IsChecked = WheelCal.disrimhole;
            ((MenuItem)((MenuItem)sender).Items[2]).IsChecked = WheelCal.dishubline;
            ((MenuItem)((MenuItem)sender).Items[3]).IsChecked = WheelCal.dishubhole;
            ((MenuItem)((MenuItem)sender).Items[4]).IsChecked = WheelCal.disspokesline;
        }

        private void DisAllToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WheelCal.disleftside = true;
            WheelCal.disrightside = true;
            WheelCal.disrimline = true;
            WheelCal.disrimhole = true;
            WheelCal.dishubline = true;
            WheelCal.dishubhole = true;
            WheelCal.disspokesline = true;
            WheelCal.CalculateSpoke();

        }

        private void RimlineToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WheelCal.disrimline = !WheelCal.disrimline;
            WheelCal.CalculateSpoke();

        }

        private void RimholeToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WheelCal.disrimhole = !WheelCal.disrimhole;
            WheelCal.CalculateSpoke();

        }

        private void HublineToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WheelCal.dishubline = !WheelCal.dishubline;
            WheelCal.CalculateSpoke();

        }

        private void HubholeToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WheelCal.dishubhole = !WheelCal.dishubhole;
            WheelCal.CalculateSpoke();

        }

        private void SpolineToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WheelCal.disspokesline = !WheelCal.disspokesline;
            WheelCal.CalculateSpoke();

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
            TransmissionCal.SaveSettings();

            Properties.Settings.Default.frnumid = FrNumcomboBox.SelectedIndex;
            Properties.Settings.Default.frmodid = FrModelcomboBox.SelectedIndex;
            
            Properties.Settings.Default.bknumid = BkNumcomboBox.SelectedIndex;
            Properties.Settings.Default.bkmodid = BkModelcomboBox.SelectedIndex;

            Properties.Settings.Default.innumid = InNumcomboBox.SelectedIndex;
            Properties.Settings.Default.inmodid = InModelcomboBox.SelectedIndex;

            Properties.Settings.Default.IsISO = checkBox2.IsChecked.Value;
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
            HubsEditor heditor = new HubsEditor(data.hublist, data, Properties.Resources.StringHubs + Properties.Resources.StringEditDataSet);
            heditor.ShowDialog();
            data.LoadData();
            RefreshData();
        }

        private void EditRimsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RimsEditor reditor = new RimsEditor(data.rimlist, data, Properties.Resources.StringRims + Properties.Resources.StringEditDataSet);
            reditor.ShowDialog();
            data.LoadData();
            RefreshData();
        }
    }
}
