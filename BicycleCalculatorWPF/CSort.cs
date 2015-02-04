using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BicycleCalculatorWPF
{
    class ListViewItemComparer : System.Collections.IComparer
    {
        private int col;
        public ListViewItemComparer()
        {
            col = 0;
        }
        public ListViewItemComparer(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            try
            {
                //double xx = Convert.ToDouble(((System.Windows.Forms.ListViewItem)x).SubItems[col].Text);
                //double yy = Convert.ToDouble(((System.Windows.Forms.ListViewItem)y).SubItems[col].Text);
                //if (xx > yy) return 1;
                //if (xx < yy) return -1;
                return 0;
            }
            catch { }
            
            //return String.Compare(((System.Windows.Forms.ListViewItem)x).SubItems[col].Text, ((System.Windows.Forms.ListViewItem)y).SubItems[col].Text);
            return 0;
        }
    }
}
