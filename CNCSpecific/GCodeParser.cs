using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNCSpecific.Milling;

namespace CNCSpecific
{
    public class GCodeParser
    {
        public String CodeString { get; private set; }

        public NCProgram NCProgram { get; set; }

        public void Import(string fileName)
        {
            String code = string.Empty;
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    String line = sr.ReadToEnd();
                    code += line;
                }
            }
            catch (Exception e)
            {
                throw new Exception("The file could not be read: " + e.Message + Environment.NewLine);
            }
            CodeString = code;
        }

        public void Parse()
        {
            
        }
    }
}
