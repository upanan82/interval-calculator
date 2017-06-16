using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2(string data)
        {
            InitializeComponent();
            this.data = data;
        }

        string data;
        int j = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0 || comboBox1.Text == "Not chosen")
                MessageBox.Show("No interval selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                j = comboBox1.SelectedIndex;
                this.DialogResult = DialogResult.OK;
            }
        }

        public int Res()
        {
            return j - 1;
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Not chosen");
            string[] str = data.Split(':');
            for (int i = 0; i < str.Length - 1; i = i + 2)
                comboBox1.Items.Add(str[i] + " ... " + str[i + 1]);
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = '\0';
            comboBox1.Focus();
        }
    }
}
