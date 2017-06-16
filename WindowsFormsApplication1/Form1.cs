using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Range C;
        Series S = new Series();
        FileStream fileStream;
        StreamReader streamReader;
        StreamWriter streamWriter;

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Opening file...";
            try
            {
                openFileDialog1.Filter = "JSON File|*.json";
                openFileDialog1.Title = "Open File";
                openFileDialog1.FileName = "";
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    fileStream = new FileStream(openFileDialog1.FileName, FileMode.Open);
                    streamReader = new StreamReader(fileStream);
                    try
                    {
                        string line,
                               json = "";
                        while ((line = streamReader.ReadLine()) != null)
                            json += line;
                        Interval[] arr = JsonConvert.DeserializeObject<Interval[]>(json);
                        S = new Series();
                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (arr[i].A.IndexOf(',') == -1 && arr[i].B.IndexOf(',') == -1)
                            {
                                int a = Convert.ToInt32(arr[i].A);
                                int b = Convert.ToInt32(arr[i].B);
                                IntegerRange T = new IntegerRange(a, b);
                                S.Add(T.Retu());
                            }
                            else
                            {
                                double a = Convert.ToDouble(arr[i].A);
                                double b = Convert.ToDouble(arr[i].B);
                                RealRange T = new RealRange(a, b);
                                S.Add(T.Retu());
                            }
                            UpdLast();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    streamReader.Close();
                    fileStream.Close();
                    toolStripStatusLabel1.Text = "Open file: " + openFileDialog1.FileName;
                }
                else toolStripStatusLabel1.Text = "Opening canceled";
            }
            catch (Exception) { }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Range[] arr = S.Get();
                int size = arr.Length;
                if (size == 0)
                {
                    MessageBox.Show("Nothing to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                toolStripStatusLabel1.Text = "Saving file...";
                saveFileDialog1.Filter = "JSON File|*.json";
                saveFileDialog1.Title = "Save File";
                saveFileDialog1.FileName = "file1.json";
                DialogResult result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    fileStream = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                    streamWriter = new StreamWriter(fileStream);
                    try
                    {
                        Lad[] product = new Lad[size];
                        for (int i = 0; i < size; i++)
                        {
                            product[i] = new Lad();
                            product[i].A = arr[i].ToString().Split(':')[0].ToString();
                            product[i].B = arr[i].ToString().Split(':')[1].ToString();
                        }
                        string output = JsonConvert.SerializeObject(product);
                        streamWriter.Write(output);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    streamWriter.Close();
                    fileStream.Close();
                    toolStripStatusLabel1.Text = "Save file: " + saveFileDialog1.FileName;
                }
                else toolStripStatusLabel1.Text = "Saving canceled";
            }
            catch (Exception) { }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox5.Text == "" || textBox6.Text == "")
                    throw new Exception("The result interval is not correct.");
                S.Add(C);
                string str1 = C.ToString().Split(':')[0].ToString(),
                       str2 = C.ToString().Split(':')[1].ToString();
                toolStripStatusLabel1.Text = "Added:  " + str1 + " ... " + str2;
                addToolStripMenuItem.Enabled = false;
                UpdLast();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Deleting ...";
            string str = Upd();
            Form2 form2 = new Form2(str);
            form2.StartPosition = FormStartPosition.CenterParent;
            form2.ShowDialog(this);
            toolStripStatusLabel1.Text = "Deletion canceled";
            if (form2.DialogResult == DialogResult.OK)
            {
                S.Del(form2.Res());
                toolStripStatusLabel1.Text = "The interval was successfully deleted";
                UpdLast();
            }
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            comboBox1.Text = '+'.ToString();
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            addToolStripMenuItem.Enabled = false;
            comboBox2.Text = "Not chosen".ToString();
            comboBox3.Text = "Not chosen".ToString();
            toolStripStatusLabel1.Text = "All cleared";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox5.Clear();
            textBox6.Clear();
            string a1T = textBox1.Text,
                   a2T = textBox2.Text,
                   b1T = textBox3.Text,
                   b2T = textBox4.Text;
            try
            {
                if (a1T == "" || a2T == "")
                {
                    if (comboBox2.SelectedIndex == 0)
                        throw new Exception("Fill in all the fields.");
                    else
                    {
                        a1T = textBox1.Text;
                        a2T = textBox2.Text;
                    }
                }
                if (b1T == "" || b2T == "")
                {
                    if (comboBox3.SelectedIndex == 0)
                        throw new Exception("Fill in all the fields.");
                    else
                    {
                        b1T = textBox3.Text;
                        b2T = textBox4.Text;
                    }
                }
                double a1 = Convert.ToDouble(a1T),
                       a2 = Convert.ToDouble(a2T),
                       b1 = Convert.ToDouble(b1T),
                       b2 = Convert.ToDouble(b2T);
                if (a1 > a2 || b1 > b2)
                    throw new Exception("Invalid interval.");
                if (a1T.IndexOf(',') == -1 && a2T.IndexOf(',') == -1 && b1T.IndexOf(',') == -1 && b2T.IndexOf(',') == -1)
                    integerFunc(a1, a2, b1, b2);
                else realFunc(a1, a2, b1, b2);
                textBox5.Enabled = true;
                textBox6.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void integerFunc(double a, double aa, double b, double bb)
        {
            try
            {
                string sign = comboBox1.Text;
                if (sign == "/")
                {
                    realFunc(a, aa, b, bb);
                    return;
                }
                int a1 = Convert.ToInt32(a),
                    a2 = Convert.ToInt32(aa),
                    b1 = Convert.ToInt32(b),
                    b2 = Convert.ToInt32(bb);
                IntegerRange A = new IntegerRange(a1, a2);
                IntegerRange B = new IntegerRange(b1, b2);
                switch (sign)
                {
                    case "+":
                        C = (IntegerRange)A.Add(B);
                        break;
                    case "-":
                        C = (IntegerRange)A.Sub(B);
                        break;
                    case "*":
                        C = (IntegerRange)A.Mul(B);
                        break;
                    case "/":
                        C = (IntegerRange)A.Div(B);
                        break;
                    default:
                        C = (IntegerRange)A.Add(B);
                        break;
                }
                string str1 = C.ToString().Split(':')[0].ToString(),
                       str2 = C.ToString().Split(':')[1].ToString();
                textBox6.Text = str1;
                textBox5.Text = str2;
                toolStripStatusLabel1.Text = "Result:  " + str1 + " ... " + str2;
                addToolStripMenuItem.Enabled = true;
            }
            catch (OverflowException)
            {
                throw;
            }
            catch (FormatException)
            {
                throw;
            }
        }

        private void realFunc(double a1, double a2, double b1, double b2)
        {
            string sign = comboBox1.Text;
            RealRange A = new RealRange(a1, a2);
            RealRange B = new RealRange(b1, b2);
            switch (sign)
            {
                case "+":
                    C = (RealRange)A.Add(B);
                    break;
                case "-":
                    C = (RealRange)A.Sub(B);
                    break;
                case "*":
                    C = (RealRange)A.Mul(B);
                    break;
                case "/":
                    C = (RealRange)A.Div(B);
                    break;
                default:
                    C = (RealRange)A.Add(B);
                    break;
            }
            string str1 = C.ToString().Split(':')[0].ToString(),
                   str2 = C.ToString().Split(':')[1].ToString();
            textBox6.Text = str1;
            textBox5.Text = str2;
            toolStripStatusLabel1.Text = "Result:  " + str1 + " ... " + str2;
            addToolStripMenuItem.Enabled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            comboBox2.SelectedIndex = 0;
            Func(e, textBox1);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            comboBox2.SelectedIndex = 0;
            Func(e, textBox2);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            comboBox3.SelectedIndex = 0;
            Func(e, textBox3);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            comboBox3.SelectedIndex = 0;
            Func(e, textBox4);
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = '\0';
            textBox6.Focus();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = '\0';
            textBox5.Focus();
        }

        private void Func(KeyPressEventArgs e, TextBox textBox)
        {
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != '-') && (e.KeyChar != '\b') && e.KeyChar != 13)
            {
                MessageBox.Show("Invalid input format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.KeyChar = '\0';
                textBox.Focus();
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = '\0';
            comboBox1.Focus();
        }

        public string Upd()
        {
            string str = "";
            Range[] arr = S.Get();
            for (int i = 0; i < arr.Length; i++)
                str += arr[i].ToString() + ":";
            return str;
        }

        public void UpdLast()
        {
            comboBox3.Items.Clear();
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Not chosen");
            comboBox3.Items.Add("Not chosen");
            string[] str = Upd().Split(':');
            for (int i = 0; i < str.Length - 1; i = i + 2)
            {
                comboBox2.Items.Add(str[i] + " ... " + str[i + 1]);
                comboBox3.Items.Add(str[i] + " ... " + str[i + 1]);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.StartPosition = FormStartPosition.CenterParent;
            form3.ShowDialog(this);
        }

        public class Interval
        {
            public string A { get; set; }
            public string B { get; set; }
        }

        public class Lad
        {
            public string A;
            public string B;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                textBox1.Text = "";
                textBox2.Text = "";
            }
            else
            {
                Range[] arr = S.Get();
                textBox1.Text = arr[comboBox2.SelectedIndex - 1].ToString().Split(':')[0].ToString();
                textBox2.Text = arr[comboBox2.SelectedIndex - 1].ToString().Split(':')[1].ToString();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                textBox3.Text = "";
                textBox4.Text = "";
            }
            else
            {
                Range[] arr = S.Get();
                textBox3.Text = arr[comboBox3.SelectedIndex - 1].ToString().Split(':')[0].ToString();
                textBox4.Text = arr[comboBox3.SelectedIndex - 1].ToString().Split(':')[1].ToString();
            }
        }

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = '\0';
            comboBox3.Focus();
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = '\0';
            comboBox2.Focus();
        }
    }
}
