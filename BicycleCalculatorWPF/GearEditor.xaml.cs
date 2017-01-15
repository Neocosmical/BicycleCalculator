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
    /// GearEditor.xaml 的交互逻辑
    /// </summary>
    public partial class GearEditor : Window
    {
        CGear gear;
        CGear gearpt;
        public GearEditor()
        {
            InitializeComponent();
        }

        public GearEditor(string title, CGear _gear, bool SpeedsEditable, bool TypeEditable)
        {
            gearpt = _gear;
            gear = (CGear)gearpt.Clone();
            InitializeComponent();
            this.Title = title;
            TextBoxSpeeds.IsEnabled = SpeedsEditable;
            TextBoxType.IsEnabled = TypeEditable;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxModel.Text = gear.Name;
            TextBoxSpeeds.SelectedIndex = gear.Speeds - 1;
            
            TextBoxType.SelectedIndex = Convert.ToInt32(gear.Type);
            TextBoxType.Text = gear.Type.ToString();
            dataGridView.Items.Clear();
            foreach (CTeeth t in gear.teeth)
            {
                if(t.id<=gear.Speeds)
                    dataGridView.Items.Add(t);
            }
        }

        private void TextBox_MouseWheel_Bk(object sender, MouseWheelEventArgs e)
        {
            int id = (int)(((TextBox)sender).Tag);
            if (((CTeeth)dataGridView.Items[id - 1]).Teeth > 1 && e.Delta < 0)
                ((CTeeth)dataGridView.Items[id - 1]).Teeth -= 1;
            if (((CTeeth)dataGridView.Items[id - 1]).Teeth < 200 && e.Delta > 0)
                ((CTeeth)dataGridView.Items[id - 1]).Teeth += 1;
            dataGridView.Items.Refresh();
        }

        private void TextBox_LostKeyboardFocus_Bk(object sender, KeyboardFocusChangedEventArgs e)
        {
            int id = (int)(((TextBox)sender).Tag);
            double teethtemp = ((CTeeth)dataGridView.Items[id - 1]).Teeth;
            try
            {
                teethtemp = Convert.ToDouble(((TextBox)sender).Text);
            }
            catch
            { }
            if (teethtemp > 200) teethtemp = 200;
            if (teethtemp < 1) teethtemp = 1;
            ((CTeeth)dataGridView.Items[id - 1]).Teeth = teethtemp;
            dataGridView.Items.Refresh();
        }

        private void bkMinuButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridView.Items[id - 1]).teeth > 1)
                ((CTeeth)dataGridView.Items[id - 1]).teeth--;
            dataGridView.Items.Refresh();
        }

        private void bkAddButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridView.Items[id - 1]).teeth < 200)
                ((CTeeth)dataGridView.Items[id - 1]).teeth++;
            dataGridView.Items.Refresh();
        }

        private void TextBoxModel_TextChanged(object sender, TextChangedEventArgs e)
        {
            gear.Name = TextBoxModel.Text;
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            gear.CopyTo(gearpt);
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void TextBoxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(TextBoxType.SelectedIndex)
            {
                case 0:
                    gear.Type = GearType.Front;
                    break;
                case 1:
                    gear.Type = GearType.Back;
                    break;
                case 2:
                    gear.Type = GearType.Inner;
                    break;
            }
        }

        private void TextBoxSpeeds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gear.Speeds = TextBoxSpeeds.SelectedIndex + 1;
            dataGridView.Items.Clear();
            foreach (CTeeth t in gear.teeth)
            {
                if (t.id <= gear.Speeds)
                    dataGridView.Items.Add(t);
            }
        }
    }
}
