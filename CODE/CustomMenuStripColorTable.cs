using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CSharpToJavaTranslator
{
    public class CustomMenuStripColorTable : System.Windows.Forms.ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return Color.FromArgb(111, 221, 151); }
        }

        public override Color MenuBorder
        {
            get { return Color.FromArgb(111, 221, 151); }
        }

        public override Color MenuItemBorder
        {
            get { return Color.DarkSeaGreen; }
        }
    }
}
