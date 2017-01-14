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
    /// GearsEditor.xaml 的交互逻辑
    /// </summary>
    public partial class GearsEditor : Window
    {
        CGearLists gearlists;
        public GearsEditor()
        {
            InitializeComponent();
        }

        public GearsEditor(CGearLists _gearlists, string title)
        {
            gearlists = _gearlists;
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

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(CGearList list in gearlists.Lists)
            {
                foreach (CGear gear in list.Gears)
                {
                    listBox1.Items.Add(gear);
                    //gear.speeds
                }
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(listBox1.SelectedIndex!=-1)
            {
                GearEditor geditor = new GearEditor(this.Title, (CGear)listBox1.SelectedItem, true, true);
                geditor.ShowDialog();
            }
            listBox1.Items.Refresh();
        }
    }
}
