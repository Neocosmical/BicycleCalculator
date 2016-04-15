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
    /// InputWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputWindow : Window
    {
        
        public string name = "";

        public InputWindow(string title, string content, string _name)
        {
            InitializeComponent();            
            this.Title = title;
            label1.Text = content;
            name = _name;
            textBox1.Text = _name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            name = textBox1.Text;
            this.DialogResult = true;
            this.Close();
            //this.DialogResult = DialogResult.OK;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
            //this.DialogResult = DialogResult.Cancel;
        }

    }
}
