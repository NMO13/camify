using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface
{
    public partial class RoughPartSpecDialog : Form
    {
        private bool m_InputValid = true;
        public RoughPartSpecDialog()
        {
            InitializeComponent();
        }

        private double StringToDouble(string input, out bool valid)
        {
            double numVal = -1;
            if (Double.TryParse(input, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out numVal))
                valid = true;
            else
                valid = false;
            return numVal;
        }

        private void RoughPartSpec_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_InputValid)
            {
                e.Cancel = true;
                m_InputValid = true;
            }
        }

        private void Button_OK_Click(object sender, EventArgs e)
        {
            bool valid = true;
            X = StringToDouble(textBox1.Text, out valid);
            if (!valid)
            {
                m_InputValid = false;
                return;
            }
            Y = StringToDouble(textBox2.Text, out valid);
            if (!valid)
                m_InputValid = false;
        }

        internal double X { get; private set; }
        internal double Y { get; private set; }
        internal double Z { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {
            bool valid = true;
            X = StringToDouble(textBox1.Text, out valid);
            if (!valid)
            {
                m_InputValid = false;
                return;
            }
            Y = StringToDouble(textBox2.Text, out valid);
            if (!valid)
            {
                m_InputValid = false;
                return;
            }
            Z = StringToDouble(textBox2.Text, out valid);
            if (!valid)
            {
                m_InputValid = false;
                return;
            }
        }
    }
}
