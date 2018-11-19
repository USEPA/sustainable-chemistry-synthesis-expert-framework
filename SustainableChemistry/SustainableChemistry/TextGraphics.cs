using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    public class TextGraphics : GraphicObject
    {
        protected System.Drawing.Font m_Font = System.Drawing.SystemFonts.DefaultFont;
        protected String m_Text = string.Empty;
        protected System.Drawing.Color m_Color = System.Drawing.Color.Black;

        public TextGraphics() : base() { }
        public TextGraphics(System.Drawing.Point graphicPosition, String text, System.Drawing.Font textFont, System.Drawing.Color textColor) : base(graphicPosition)
        {
            this.m_Text = text;
            this.m_Font = textFont;
            this.m_Color = textColor;
            m_AutoSize = true;
        }

        public TextGraphics(int posX, int posY, String text, System.Drawing.Font textFont, System.Drawing.Color textColor) : base(posX, posY)
        {
            this.m_Text = text;
            this.m_Font = textFont;
            this.m_Color = textColor;
            m_AutoSize = true;
        }

        public TextGraphics(System.Drawing.Point graphicPosition, String text, System.Drawing.Font textFont, System.Drawing.Color textColor, double rotation) : base(graphicPosition, rotation)
        {
            this.m_Text = text;
            this.m_Font = textFont;
            this.m_Color = textColor;
            m_AutoSize = true;
        }

        TextGraphics(int posX, int posY, String text, System.Drawing.Font textFont, System.Drawing.Color textColor, double rotation) : base(posX, posY, rotation)
        {
            this.m_Text = text;
            this.m_Font = textFont;
            this.m_Color = textColor;
            m_AutoSize = true;
        }

        public System.Drawing.Font Font
        {
            get
            {
                return m_Font;
            }
            set
            {
                m_Font = value;
            }
        }

        System.Drawing.StringAlignment m_HorizontalAlignment = System.Drawing.StringAlignment.Center;
        public System.Drawing.StringAlignment HorizontalAlignment
        {
            get
            {
                return m_HorizontalAlignment;
            }
            set
            {
                m_HorizontalAlignment = value;
            }
        }

        System.Drawing.StringAlignment m_VerticalAlignment = System.Drawing.StringAlignment.Center;
        public System.Drawing.StringAlignment VerticalAlignment
        {
            get
            {
                return m_VerticalAlignment;
            }
            set
            {
                m_VerticalAlignment = value;
            }
        }

        public String Text
        {
            get
            {
                return m_Text;
            }
            set
            {
                m_Text = value;
            }
        }

        public System.Drawing.Color Color
        {
            get
            {
                return m_Color;
            }
            set
            {
                m_Color = value;
            }
        }

        public override bool HitTest(System.Drawing.Point pt)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
            gp.AddRectangle(new System.Drawing.Rectangle(this.m_Position.X - (int)(0.25 * this.m_Size.Width), this.m_Position.Y - (int)(0.25 * this.m_Size.Height), (int)(1.25 * this.m_Size.Width), (int)(1.25 * this.m_Size.Height)));
            if (this.m_Rotation != 0)
            {
                myMatrix.RotateAt((float)(this.m_Rotation), new System.Drawing.PointF((float)this.X, (float)this.Y),
                    System.Drawing.Drawing2D.MatrixOrder.Append);
            }
            gp.Transform(myMatrix);
            return gp.IsVisible(pt);
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            System.Drawing.StringFormat format = new System.Drawing.StringFormat();
            format.Alignment = this.m_HorizontalAlignment;
            format.LineAlignment = this.m_VerticalAlignment;
            System.Drawing.Drawing2D.GraphicsContainer gContainer = g.BeginContainer();
            System.Drawing.Drawing2D.Matrix myMatrix = g.Transform;
            float X = (float)this.X;
            float Y = (float)this.Y;
            if (m_Rotation != 0)
            {
                myMatrix.RotateAt((float)(m_Rotation), new System.Drawing.PointF(X, Y), System.Drawing.Drawing2D.MatrixOrder.Append);
                g.Transform = myMatrix;
            }
            if (m_AutoSize)
            {
                System.Drawing.SizeF mySize = g.MeasureString(m_Text, m_Font);
                m_Size.Width = (int)mySize.Width;
                m_Size.Height = (int)mySize.Height;
                g.DrawString(m_Text, m_Font, new System.Drawing.SolidBrush(m_Color), X, Y, format);
            }
            else
            {
                System.Drawing.RectangleF rect = new System.Drawing.RectangleF(X, Y, (float)m_Size.Width, (float)m_Size.Height);
                g.DrawString(m_Text, m_Font, new System.Drawing.SolidBrush(m_Color), rect, format);
            }
            g.EndContainer(gContainer);
        }
    }
}
