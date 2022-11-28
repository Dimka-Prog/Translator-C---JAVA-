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
            get { return Color.LimeGreen; }
        }

        public override Color MenuBorder
        {
            get { return Color.LimeGreen; }
        }

        public override Color MenuItemBorder
        {
            get { return Color.DarkSeaGreen; }
        }
    }
}
