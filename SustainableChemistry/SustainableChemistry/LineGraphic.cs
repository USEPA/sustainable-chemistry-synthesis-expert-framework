using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    public class LineGraphic : ShapeGraphic
    {
        //Constructors

        public LineGraphic() : base() { }
        public LineGraphic(System.Drawing.Point startPosition) : base(startPosition)
        {
        }

        public LineGraphic(int posX, int posY): base(posX, posY)
        {
        }

        public LineGraphic(System.Drawing.Point startPosition, System.Drawing.Point endPosition):base(startPosition)
        {
            this.m_Size = new System.Drawing.Size(endPosition.X - startPosition.X, endPosition.Y - startPosition.Y);
            this.m_AutoSize = false;
        }


        public LineGraphic(int startX, int startY, System.Drawing.Point endPosition):base(startX, startY)
        {
            this.m_Size = new System.Drawing.Size(endPosition.X - startX, endPosition.Y - startY);
        }


        public LineGraphic(int startX, int startY, int endX, int endY) : base(startX, startY)
        {
            this.m_Size = new System.Drawing.Size(endX - startX, endY - startY);
            this.m_AutoSize = false;
        }


        public LineGraphic(System.Drawing.Point startPosition, System.Drawing.Point endPosition, double lineWidth, System.Drawing.Color lineColor):
            base(startPosition)
        {
            this.m_Size = new System.Drawing.Size(endPosition.X - startPosition.X, endPosition.Y - startPosition.Y);
            this.m_AutoSize = false;
            this.m_lineWidth = lineWidth;
            this.m_lineColor = lineColor;
        }


        public LineGraphic(int startX, int startY, int endX, int endY, float lineWidth, System.Drawing.Color lineColor) : base(startX, startY)
        {
            this.m_Size = new System.Drawing.Size(endX - startX, endY - startY);
            this.m_AutoSize = false;
            this.m_lineWidth = lineWidth;
            this.m_lineColor = lineColor;
        }

        public override bool HitTest(System.Drawing.Point pt)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
            System.Drawing.Pen myPen = new System.Drawing.Pen(this.m_lineColor, (float)this.m_lineWidth + 2);
            float X = (float)this.X;
            float Y = (float)this.Y;
            gp.AddLine(X, Y, X + m_Size.Width, Y + m_Size.Height);
            myMatrix.RotateAt((float)this.m_Rotation, new System.Drawing.PointF(X, Y), System.Drawing.Drawing2D.MatrixOrder.Append);
            gp.Transform(myMatrix);
            return gp.IsOutlineVisible(pt, myPen);
        }

        System.Drawing.Point GetStartPosition()
        {
            return ((GraphicObject)this).GetPosition();
        }

        void SetStartPosition(System.Drawing.Point Value)
        {
            this.SetPosition(Value);
        }

        System.Drawing.Point GetEndPosition()
        {
            System.Drawing.Point endPosition = new System.Drawing.Point(this.m_Position.X, this.m_Position.Y);
            endPosition.X += this.m_Size.Width;
            endPosition.Y += this.m_Size.Height;
            return endPosition;
        }

        void SetEndPosition(System.Drawing.Point Value)
        {
            this.Width = Value.X - m_Position.X;
            this.Height = Value.Y - m_Position.Y;
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            System.Drawing.Drawing2D.GraphicsContainer gContainer = g.BeginContainer();
            System.Drawing.Drawing2D.Matrix myMatrix = g.Transform;
            float X = (float)this.X;
            float Y = (float)this.Y;
            if (m_Rotation != 0)
            {
                myMatrix.RotateAt((float)(m_Rotation), new System.Drawing.PointF(X, Y), System.Drawing.Drawing2D.MatrixOrder.Append);
                g.Transform = myMatrix;
            }
            System.Drawing.Pen myPen = new System.Drawing.Pen(m_lineColor, (float)m_lineWidth);

            g.DrawLine(myPen, X, Y, X + m_Size.Width, Y + m_Size.Height);
            g.EndContainer(gContainer);
        }

        void Draw(System.Drawing.Graphics g, System.Drawing.Drawing2D.AdjustableArrowCap customStartCap, System.Drawing.Drawing2D.AdjustableArrowCap customEndCap)
        {
            System.Drawing.Drawing2D.GraphicsContainer gContainer = g.BeginContainer();
            System.Drawing.Drawing2D.Matrix myMatrix = g.Transform;

            float X = (float)this.X;
            float Y = (float)this.Y;
            if (m_Rotation != 0)
            {
                myMatrix.RotateAt((float)m_Rotation, new System.Drawing.PointF(X, Y), System.Drawing.Drawing2D.MatrixOrder.Append);
                g.Transform = myMatrix;
            }
            System.Drawing.Pen myPen = new System.Drawing.Pen(m_lineColor, (float)m_lineWidth);

            // put startcaps and endcaps on lines
            if (customStartCap != null) myPen.CustomStartCap = customStartCap;
            if (customEndCap != null) myPen.CustomEndCap = customEndCap;

            g.DrawLine(myPen, X, Y, X + m_Size.Width, Y + m_Size.Height);
            g.EndContainer(gContainer);
        }
    }
}
