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
using System.Windows.Shapes;

namespace BicycleCalculatorWPF
{
    /// <summary>
    /// DonateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DonateWindow : Window
    {
        public DonateWindow()
        {
            InitializeComponent();
        }

        private void Image_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://shenghuo.alipay.com/send/payment/fill.htm?optEmail=carrotkk@sina.com&payAmount=3&title=Donate to Bicycle Calculator&memo=");
        }

        private void Image_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=XPYP748AL2MWS");
        }
    }
}
