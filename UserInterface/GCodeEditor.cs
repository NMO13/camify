using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface
{
    internal partial class GCodeEditor : UserControl
    {
        private GCodeOutput _outputWindow;

        internal GCodeEditor(UserControl outputWindow)
        {
            InitializeComponent();
            Font font = new Font("Arial", 12.0f);
            richTextBox1.Font = font;
            _outputWindow = outputWindow as GCodeOutput;
        }

        public void SetCode(String gCode)
        {
            richTextBox1.Clear();
            richTextBox1.Text = gCode;
        }

        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            var line = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);
            if (line >= richTextBox1.Lines.Length)
                return;
            var selectedLine = richTextBox1.Lines[line];
            _outputWindow.SetLine(line, selectedLine);
        }
    }
}
