using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    public class GraphicAtom : TextGraphics
    {
        public GraphicAtom(ChemInfo.Atom a, System.Drawing.Font textFont) : base(a.Location2D, a.AtomicSymbol, textFont, a.Color)
        {
            this.m_Font = textFont;
            m_AutoSize = true;
            this.Tag = a;
        }

    }
}
