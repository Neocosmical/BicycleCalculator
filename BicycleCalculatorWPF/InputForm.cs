using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BicycleCalculatorWPF
{
    public partial class InputForm : Form
    {
        public string name = "";

        public InputForm(string title, string content, string _name)
        {
            InitializeComponent();
            this.Text = title;
            label1.Text = content;
            name = _name;
            textBox1.Text = _name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = textBox1.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
