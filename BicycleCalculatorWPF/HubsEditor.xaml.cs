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
    /// HubsEditor.xaml 的交互逻辑
    /// </summary>
    public partial class HubsEditor : Window
    {

        List<CHub> list;
        CData data;
        public HubsEditor()
        {
            InitializeComponent();
        }

        public HubsEditor(List<CHub> _list, CData _data, string title)
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
            foreach (CHub item in list)
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
            HubEditor editor = new HubEditor(this.Title, (CHub)listBox1.SelectedItem);
            if (editor.ShowDialog() == true) ButtonSave.IsEnabled = true;
            listBox1.Items.Refresh();
        }

        private void ButtonModify_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            HubEditor editor = new HubEditor(this.Title, (CHub)listBox1.SelectedItem);
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
            CHub newitem = new CHub("", 42.5, 42.5, 36, 36, 2.6);
            HubEditor geditor = new HubEditor(this.Title, newitem);
            if (geditor.ShowDialog() != true) return;
            data.hublist.Add(newitem);
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
            foreach (CHub item in listBox1.SelectedItems)
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
