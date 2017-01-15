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
using System.Windows.Controls.Primitives;

namespace BicycleCalculatorWPF
{
    /// <summary>
    /// TireEditor.xaml 的交互逻辑
    /// </summary>
    public partial class TireEditor : Window
    {

        CWheel item;
        CWheel itempt;
        public TireEditor()
        {
            InitializeComponent();
        }

        public TireEditor(string title, CWheel _item)
        {
            itempt = _item;
            item = (CWheel)itempt.Clone();
            InitializeComponent();
            this.Title = title;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxModel.Text = item.Name;
            TextBoxCirc.Text = item.Lenth.ToString();
        }

        private void TextBox_MouseWheel_Bk(object sender, MouseWheelEventArgs e)
        {
            if (item.Lenth > 1 && e.Delta < 0)
                item.Lenth -= 1;
            if (item.Lenth < 20000 && e.Delta > 0)
                item.Lenth += 1;
            TextBoxCirc.Text = item.Lenth.ToString();
        }

        private void TextBox_LostKeyboardFocus_Bk(object sender, KeyboardFocusChangedEventArgs e)
        {
            int temp = item.Lenth;
            try
            {
                temp = Convert.ToInt32(((TextBox)sender).Text);
            }
            catch
            { }
            if (temp > 20000) temp = 20000;
            if (temp < 1) temp = 1;
            item.Lenth = temp;
            TextBoxCirc.Text = item.Lenth.ToString();
        }

        private void bkMinuButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            if (item.Lenth > 1)
                item.Lenth--;
            TextBoxCirc.Text = item.Lenth.ToString();
        }

        private void bkAddButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            if (item.Lenth < 20000)
                item.Lenth++;
            TextBoxCirc.Text = item.Lenth.ToString();
        }

        private void TextBoxModel_TextChanged(object sender, TextChangedEventArgs e)
        {
            item.Name = TextBoxModel.Text;
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            item.CopyTo(itempt);
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
