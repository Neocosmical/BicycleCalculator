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
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;

namespace BicycleCalculatorWPF
{
    /// <summary>
    /// RimEditor.xaml 的交互逻辑
    /// </summary>
    public partial class RimEditor : Window
    {

        CRim item;
        CRim itempt;
        public RimEditor()
        {
            InitializeComponent();
        }

        public RimEditor(string title, CRim _item)
        {
            itempt = _item;
            item = (CRim)itempt.Clone();
            InitializeComponent();
            this.Title = title;
        }


        private void hubMinuButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int no = Convert.ToInt32(b.CommandParameter);
            if (((CValue)dataGridViewHub.Items[no]).Val > 0.0)
                ((CValue)dataGridViewHub.Items[no]).Val -= 0.01;
            dataGridViewHub.Items.Refresh();
        }

        private void hubAddButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int no = Convert.ToInt32(b.CommandParameter);
            if (((CValue)dataGridViewHub.Items[no]).Val < 1000.0)
                ((CValue)dataGridViewHub.Items[no]).Val += 0.01;
            dataGridViewHub.Items.Refresh();
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
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxModel.Text = item.Name;
            dataGridViewHub.Items.Clear();
            foreach (CValue v in item.vals)
            {
                dataGridViewHub.Items.Add(v);
            }
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
