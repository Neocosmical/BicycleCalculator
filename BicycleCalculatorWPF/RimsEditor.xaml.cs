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
using System.Windows.Shapes;

namespace BicycleCalculatorWPF
{
    /// <summary>
    /// RimsEditor.xaml 的交互逻辑
    /// </summary>
    public partial class RimsEditor : Window
    {
        
        List<CRim> list;
        CData data;
        public RimsEditor()
        {
            InitializeComponent();
        }

        public RimsEditor(List<CRim> _list, CData _data, string title)
        {
            list = _list;
            data = _data;
            InitializeComponent();
            this.Title = title;
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listBox1_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonModify.IsEnabled = (listBox1.SelectedIndex != -1);
            ButtonDelete.IsEnabled = (listBox1.SelectedIndex != -1);
            MenuItemModify.IsEnabled = (listBox1.SelectedIndex != -1);
            MenuItemDelete.IsEnabled = (listBox1.SelectedIndex != -1);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            listBox1.Items.Clear();
            foreach (CRim item in list)
            {
                listBox1.Items.Add(item);
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            RimEditor editor = new RimEditor(this.Title, (CRim)listBox1.SelectedItem);
            if (editor.ShowDialog() == true) ButtonSave.IsEnabled = true;
            listBox1.Items.Refresh();
        }

        private void ButtonModify_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            RimEditor editor = new RimEditor(this.Title, (CRim)listBox1.SelectedItem);
            if (editor.ShowDialog() == true) ButtonSave.IsEnabled = true;
            listBox1.Items.Refresh();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Properties.Resources.StringConfirmSave,
                Properties.Resources.StringSave, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            data.SaveDate();
            ButtonSave.IsEnabled = false;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            CRim newitem = new CRim("", 383, 0);
            RimEditor geditor = new RimEditor(this.Title, newitem);
            if (geditor.ShowDialog() != true) return;
            data.rimlist.Add(newitem);
            ButtonSave.IsEnabled = true;
            LoadData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelected();
        }

        private void DeleteSelected()
        {
            if (listBox1.SelectedIndex == -1) return;
            if (MessageBox.Show(Properties.Resources.StringDeleteSelected,
                Properties.Resources.StringDelete, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            foreach (CRim item in listBox1.SelectedItems)
            {
                list.Remove(item);
            }
            LoadData();
            ButtonSave.IsEnabled = true;
        }

        private void listBox1_KeyUp(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(e.Key.ToString());
            if (e.Key == Key.Delete)
                DeleteSelected();
        }
    }
}
