using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;

namespace BicycleCalculatorWPF
{
    class CWheelCal
    {
        public bool ready = false;

        public CRim rimnow;
        public CHub hubnow;


        OxyPlot.Wpf.PlotView chart;
        System.Windows.Controls.ListView list;
        public PlotModel pm;

        public int Spokes;
        public int CrossesL;
        public int CrossesR;

        public bool disrimline = true;
        public bool dishubline = true;
        public bool dishubhole = true;
        public bool disrimhole = true;
        public bool disspokesline = true;
        public bool disleftside = true;
        public bool disrightside = true;

        public static PlotModel LineSeries()
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


        public CWheelCal()
        {

        }

        public void Init(OxyPlot.Wpf.PlotView _chart, System.Windows.Controls.ListView _list)
        {
            chart = _chart;
            list = _list;

            pm = new PlotModel();
            pm = LineSeries();
            chart.Model = pm;
        }


        public void Calculate()
        {
            if (!ready) return;
            pm.Series.Clear();
            //pm.Axes[0].MinorStep = double.NaN;
            //pm.Axes[0].MajorStep = double.NaN;
            //CHub hubtemp = frnow;
            CHub hubtemp = hubnow;
            CRim rimtemp = rimnow;
            
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
            hubline.Points.Add(new DataPoint(double.NaN, double.NaN));
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
                DataPoint p1 = new DataPoint(hubhole.Points[i].X, hubhole.Points[i].Y);
                int j = 0;
                if (i % 2 == 0)
                {
                    j = i + CrossesL;
                }
                else
                {
                    j = i - CrossesL;
                }
                while (j < 0) j += Spokes / 2;
                j = j % (Spokes / 2);

                DataPoint p2 = new DataPoint(rimhole.Points[j].X, rimhole.Points[j].Y);
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
                DataPoint p1 = new DataPoint(hubholer.Points[i].X, hubholer.Points[i].Y);
                int j = 0;
                if (i % 2 == 0)
                {
                    j = i + CrossesR;
                }
                else
                {
                    j = i - CrossesR;
                }
                while (j < 0) j += Spokes / 2;
                j = j % (Spokes / 2);

                DataPoint p2 = new DataPoint(rimholer.Points[j].X, rimholer.Points[j].Y);
                if (spokeslenthrawr == 0)
                    spokeslenthrawr = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
                spokesliner.Points.Add(p1);
                spokesliner.Points.Add(p2);
                spokesliner.Points.Add(new DataPoint(double.NaN, double.NaN));
            }

            if (disrimline) pm.Series.Add(rimline);
            if (dishubline) pm.Series.Add(hubline);
            if (disrightside && dishubhole) pm.Series.Add(hubholer);
            if (disleftside && dishubhole) pm.Series.Add(hubhole);
            if (disrightside && disrimhole) pm.Series.Add(rimholer);
            if (disleftside && disrimhole) pm.Series.Add(rimhole);
            if (disrightside && disspokesline) pm.Series.Add(spokesliner);
            if (disleftside && disspokesline) pm.Series.Add(spokesline);
            chart.ResetAllAxes();
            chart.InvalidatePlot(true);


            list.Items.Clear();

            for (int i = 0; i < 6; i++)
            {
                DataPoint p1 = new DataPoint(hubhole.Points[0].X, hubhole.Points[0].Y);
                while (i < 0) i += Spokes / 2;
                int j = i % (Spokes / 2);
                DataPoint p2 = new DataPoint(rimhole.Points[j].X, rimhole.Points[j].Y);
                spokeslenthraw = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);

                p1 = new DataPoint(hubhole.Points[0].X, hubhole.Points[0].Y);
                while (i < 0) i += Spokes / 2;
                j = i % (Spokes / 2);
                p2 = new DataPoint(rimhole.Points[j].X, rimhole.Points[j].Y);
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
                list.Items.Add(resulttemp);
            }
            if (CrossesL > 5)
            {
                int i = CrossesL;
                DataPoint p1 = new DataPoint(hubhole.Points[0].X, hubhole.Points[0].Y);
                while (i < 0) i += Spokes / 2;
                int j = i % (Spokes / 2);

                DataPoint p2 = new DataPoint(rimhole.Points[j].X, rimhole.Points[j].Y);
                spokeslenthraw = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);

                p1 = new DataPoint(hubhole.Points[0].X, hubhole.Points[0].Y);
                while (i < 0) i += Spokes / 2;
                j = i % (Spokes / 2);
                p2 = new DataPoint(rimhole.Points[j].X, rimhole.Points[j].Y);
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
                list.Items.Add(resulttemp);
            }
            if (CrossesR > 5 && CrossesR != CrossesL)
            {
                int i = CrossesR;
                DataPoint p1 = new DataPoint(hubhole.Points[0].X, hubhole.Points[0].Y);
                while (i < 0) i += Spokes / 2;
                int j = i % (Spokes / 2);

                DataPoint p2 = new DataPoint(rimhole.Points[j].X, rimhole.Points[j].Y);
                spokeslenthraw = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);

                p1 = new DataPoint(hubhole.Points[0].X, hubhole.Points[0].Y);
                while (i < 0) i += Spokes / 2;
                j = i % (Spokes / 2);
                p2 = new DataPoint(rimhole.Points[j].X, rimhole.Points[j].Y);
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
                list.Items.Add(resulttemp);
            }
        }

        public void ExportFileSp()
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
                System.Windows.MessageBox.Show(Properties.Resources.StringUnableexp + "\r\n" + ex.Message);
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

            swriter.WriteLine(Properties.Resources.StringSpokes + "," + Spokes);
            swriter.WriteLine("");

            string strtemp = "";

            strtemp += Properties.Resources.StringCrosses + ",";
            strtemp += Properties.Resources.StringSpokeLL + ",";
            strtemp += Properties.Resources.StringSpokeLR + ",";
            strtemp += Properties.Resources.StringTenRat + ",";
            strtemp += Properties.Resources.StringRemark + ",";

            swriter.WriteLine(strtemp);

            foreach (CSpokeResult it in list.Items)
            {
                strtemp = "";
                strtemp += it.Crosses.ToString() + ",";
                strtemp += it.Lenthleft.ToString("0.00") + "mm,";
                strtemp += it.Lenthright.ToString("0.00") + "mm,";
                strtemp += (it.Tensionratio * 100.0).ToString("0.00") + "%,";
                strtemp += it.Remark + ",";
                swriter.WriteLine(strtemp);
            }
            swriter.Close();

        }

    }
}
