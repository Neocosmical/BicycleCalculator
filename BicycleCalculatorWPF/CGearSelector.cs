using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BicycleCalculatorWPF
{
    class CGearSelector
    {
        ComboBox NumcomboBox;
        ComboBox ModelcomboBox;
        DataGrid dataGridView;
        CTransmissionCal TransmissionCal;
        CGearLists lists;
        bool is_float = false;

        public CGearSelector(bool _is_float = false)
        {
            is_float = _is_float;
        }

        public void Init(ComboBox _NumcomboBox, ComboBox _ModelcomboBox, DataGrid _dataGridView, CTransmissionCal _TransmissionCal, CGearLists _lists)
        {
            NumcomboBox = _NumcomboBox;
            ModelcomboBox = _ModelcomboBox;
            dataGridView = _dataGridView;
            TransmissionCal = _TransmissionCal;
            lists = _lists;
            
            NumcomboBox.SelectionChanged += NumcomboBox_SelectionChanged;
            ModelcomboBox.SelectionChanged += ModelcomboBox_SelectionChanged;


            NumcomboBox.Items.Clear();
            foreach (CGearList list in lists.Lists)
                NumcomboBox.Items.Add(list);
            NumcomboBox.SelectedIndex = 0;
            ModelcomboBox.SelectedIndex = 0;
            switch (lists.type)
            {
                case GearType.Front:
                    NumcomboBox.SelectedIndex = Properties.Settings.Default.frnumid;
                    ModelcomboBox.SelectedIndex = Properties.Settings.Default.frmodid;
                    break;
                case GearType.Back:
                    NumcomboBox.SelectedIndex = Properties.Settings.Default.bknumid;
                    ModelcomboBox.SelectedIndex = Properties.Settings.Default.bkmodid;
                    break;
                case GearType.Inner:
                    NumcomboBox.SelectedIndex = Properties.Settings.Default.innumid;
                    ModelcomboBox.SelectedIndex = Properties.Settings.Default.inmodid;
                    break;
            }

        }

        public void TextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            int id = (int)(((TextBox)sender).Tag);
            double teethtemp = ((CTeeth)dataGridView.Items[id - 1]).Teeth;
            if (is_float)
            {
                try
                {
                    teethtemp = Convert.ToDouble(((TextBox)sender).Text);
                }
                catch
                { }
                if (teethtemp > 200) teethtemp = 200;
                if (teethtemp < 0.0001) teethtemp = 0.0001;
            }
            else
            {
                try
                {
                    teethtemp = Convert.ToInt16(((TextBox)sender).Text);
                }
                catch
                { }
                if (teethtemp > 200) teethtemp = 200;
                if (teethtemp < 1) teethtemp = 1;
            }
            ((CTeeth)dataGridView.Items[id - 1]).Teeth = teethtemp;
            dataGridView.Items.Refresh();
            TransmissionCal.Calculate();
        }

        public void TextBox_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            int id = (int)(((TextBox)sender).Tag);
            double step = 0.0;
            if (is_float) step = 0.1;
            else step = 1.0;
            if (((CTeeth)dataGridView.Items[id - 1]).Teeth > step && e.Delta < 0)
                ((CTeeth)dataGridView.Items[id - 1]).Teeth -= step;
            if (((CTeeth)dataGridView.Items[id - 1]).Teeth < 200 && e.Delta > 0)
                ((CTeeth)dataGridView.Items[id - 1]).Teeth += step;
            dataGridView.Items.Refresh();
            TransmissionCal.Calculate();
        }
        
        public void MinuButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            double step = 0.0;
            if (is_float) step = 0.1;
            else step = 1.0;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridView.Items[id - 1]).teeth > step)
                ((CTeeth)dataGridView.Items[id - 1]).teeth -= step;
            dataGridView.Items.Refresh();
            TransmissionCal.Calculate();
        }

        public void AddButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            double step = 0.0;
            if (is_float) step = 0.1;
            else step = 1.0;
            int id = Convert.ToInt32(b.CommandParameter);
            if (((CTeeth)dataGridView.Items[id - 1]).teeth < 200)
                ((CTeeth)dataGridView.Items[id - 1]).teeth += step;
            dataGridView.Items.Refresh();
            TransmissionCal.Calculate();
        }

        private void ModelcomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ModelcomboBox.SelectedItem == null) return;

            bool rtemp = TransmissionCal.ready;
            TransmissionCal.ready = false;
            dataGridView.Items.Clear();
            switch (lists.type)
            {
                case GearType.Front:
                    TransmissionCal.frnow = (CGear)((CGear)ModelcomboBox.SelectedItem).Clone();
                    for (int i = 0; i < TransmissionCal.frnow.Speeds; i++) dataGridView.Items.Add(TransmissionCal.frnow.teeth[i]);
                    break;
                case GearType.Back:
                    TransmissionCal.bknow = (CGear)((CGear)ModelcomboBox.SelectedItem).Clone();
                    for (int i = 0; i < TransmissionCal.bknow.Speeds; i++) dataGridView.Items.Add(TransmissionCal.bknow.teeth[i]);
                    break;
                case GearType.Inner:
                    TransmissionCal.innow = (CGear)((CGear)ModelcomboBox.SelectedItem).Clone();
                    for (int i = 0; i < TransmissionCal.innow.Speeds; i++) dataGridView.Items.Add(TransmissionCal.innow.teeth[i]);
                    break;
            }
            TransmissionCal.ready = rtemp;
            TransmissionCal.Calculate();
        }

        private void NumcomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NumcomboBox.SelectedItem == null) return;
            bool rtemp = TransmissionCal.ready;
            TransmissionCal.ready = false;
            ModelcomboBox.Items.Clear();
            
            foreach (CGear gear in ((CGearList)NumcomboBox.SelectedItem).Gears)
                ModelcomboBox.Items.Add(gear);
            if (lists.type == GearType.Inner)
                dataGridView.IsEnabled = ((CGearList)NumcomboBox.SelectedItem).speeds > 1;
            ModelcomboBox.SelectedIndex = 0;
            TransmissionCal.ready = rtemp;
            TransmissionCal.Calculate();
        }

        public void RefreshData()
        {

            int temp1 = NumcomboBox.SelectedIndex;
            int temp2 = ModelcomboBox.SelectedIndex;

            NumcomboBox.Items.Clear();


            foreach (CGearList list in lists.Lists)
                NumcomboBox.Items.Add(list);


            NumcomboBox.SelectedIndex = 0;
            ModelcomboBox.SelectedIndex = 0;


            NumcomboBox.SelectedIndex = temp1;
            ModelcomboBox.SelectedIndex = temp2;
        }

        public void SaveSettings()
        {
            switch (lists.type)
            {
                case GearType.Front:
                    Properties.Settings.Default.frnumid = NumcomboBox.SelectedIndex;
                    Properties.Settings.Default.frmodid = ModelcomboBox.SelectedIndex;
                    break;
                case GearType.Back:
                    Properties.Settings.Default.bknumid = NumcomboBox.SelectedIndex;
                    Properties.Settings.Default.bkmodid = ModelcomboBox.SelectedIndex;
                    break;
                case GearType.Inner:
                    Properties.Settings.Default.innumid = NumcomboBox.SelectedIndex;
                    Properties.Settings.Default.inmodid = ModelcomboBox.SelectedIndex;
                    break;
            }
        }
    }
}
