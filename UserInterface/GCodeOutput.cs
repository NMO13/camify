using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface
{
    internal partial class GCodeOutput : UserControl
    {
        internal GCodeOutput()
        {
            InitializeComponent();
            Font font = new Font("Arial", 12.0f);
            richTextBox1.Font = font;
        }

        internal void SetLine(int number, string text)
        {
            richTextBox1.Clear();
            string line = string.Format("Line {0}: {1}", number, text);

            Random rnd = new Random();
            int randNumber = rnd.Next(1, 13); // creates a number between 1 and 12
            string time = string.Format("Time {0} second", randNumber);

            string spindle = string.Format("1500 RPM");

            string feed = string.Format("Feed: 200.0000 IPM");

            string tool = "Tool: 0";

            string coolant = "Coolant: OFF";

            richTextBox1.AppendText(line + Environment.NewLine);
            richTextBox1.AppendText(time + Environment.NewLine);
            richTextBox1.AppendText(spindle + Environment.NewLine);
            richTextBox1.AppendText(feed + Environment.NewLine);
            richTextBox1.AppendText(tool + Environment.NewLine);
            richTextBox1.AppendText(coolant + Environment.NewLine);
        }
    }
}
