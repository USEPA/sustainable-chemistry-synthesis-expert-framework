using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    public class GraphicBond :LineGraphic
    {
        GraphicAtom m_parentAtom = null;
        GraphicAtom m_connectedAtom = null;

        public GraphicBond(GraphicAtom parent, GraphicAtom connected, ChemInfo.Bond bond): base(parent.GetPosition(), connected.GetPosition())
        {
            m_parentAtom = parent;
            m_connectedAtom = connected;
            m_BondType = bond.DrawType;
            this.Tag = bond;
        }

        ChemInfo.BondType m_BondType = ChemInfo.BondType.Single;
        ChemInfo.BondType BondType
        {
            get
            {
                return m_BondType;
            }
            set
            {
                m_BondType = value;
            }
        }

        public override bool HitTest(System.Drawing.Point pt)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
            System.Drawing.Pen myPen = new System.Drawing.Pen(this.m_lineColor, (float)this.m_lineWidth + 2);
            float X = (float)this.X;
            float Y = (float)this.Y;
            if (this.m_BondType == ChemInfo.BondType.Single) gp.AddLine(X, Y, X + m_Size.Width, Y + m_Size.Height);
            if (this.m_BondType == ChemInfo.BondType.Double)
            {
                gp.AddLine(X + m_OffsetX, Y + m_OffsetY, X + m_Size.Width + m_OffsetX, Y + m_Size.Height + m_OffsetY);
                gp.AddLine(X - m_OffsetX, Y - m_OffsetY, X + m_Size.Width - m_OffsetX, Y + m_Size.Height - m_OffsetY);
            }
            if (this.m_BondType == ChemInfo.BondType.Triple)
            {
                gp.AddLine(X + m_OffsetX, Y + m_OffsetY, X + m_Size.Width + m_OffsetX, Y + m_Size.Height + m_OffsetY);
                gp.AddLine(X, Y, X + m_Size.Width, Y + m_Size.Height);
                gp.AddLine(X - m_OffsetX, Y - m_OffsetY, X + m_Size.Width - m_OffsetX, Y + m_Size.Height - m_OffsetY);
            }
            myMatrix.RotateAt((float)this.m_Rotation, new System.Drawing.PointF(X, Y), System.Drawing.Drawing2D.MatrixOrder.Append);
            gp.Transform(myMatrix);
            return gp.IsOutlineVisible(pt, myPen);
        }

        System.Drawing.Point GetStartPosition()
        {
            return this.GetPosition();
        }

        void SetStartPosition(System.Drawing.Point Value)
        {
            m_Position = Value;
            m_parentAtom.SetPosition(Value);
        }

        System.Drawing.Point GetEndPosition()
        {
            System.Drawing.Point endPosition = m_connectedAtom.GetPosition();
            //endPosition.X += this.m_Size.Width;
            //endPosition.Y += this.m_Size.Height;
            return endPosition;
        }

        void SetEndPosition(System.Drawing.Point Value)
        {
            m_Size.Width = Value.X - m_Position.X;
            m_Size.Height = Value.Y - m_Position.Y;
            m_connectedAtom.SetPosition(Value);
        }

        float AngleToPoint(System.Drawing.Point Origin, System.Drawing.Point Target)
        {
            //'a cool little utility function, 
            //'given two points finds the angle between them....
            //'forced me to recall my highschool math, 
            //'but the task is made easier by a special overload to
            //'Atan that takes X,Y co-ordinates.
            float Angle;
            Target.X = Target.X - Origin.X;
            Target.Y = Target.Y - Origin.Y;
            Angle = (float)(Math.Atan2(Target.Y, Target.X) / (Math.PI / 180));
            return Angle;
        }

        float distance = 4;
        float m_OffsetX = 0;
        float m_OffsetY = 0;
        void DetermineOffset()
        {
            float factor = 1;
            if (this.m_BondType == ChemInfo.BondType.Triple) factor = (float)2.0;
            float angle = AngleToPoint(new System.Drawing.Point(X, Y), new System.Drawing.Point(X + m_Size.Width, Y + m_Size.Height));
            m_OffsetX = distance * factor* (float)Math.Sin((angle + 90) * (Math.PI / 180));
            m_OffsetY = distance * factor* (float)Math.Cos((angle + 90) * (Math.PI / 180));
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            System.Drawing.Drawing2D.GraphicsContainer gContainer = g.BeginContainer();
            System.Drawing.Drawing2D.Matrix myMatrix = g.Transform;
            this.DetermineOffset();
            float X = (float)this.m_parentAtom.X;
            float Y = (float)this.m_parentAtom.Y;
            if (m_Rotation != 0)
            {
                myMatrix.RotateAt((float)(m_Rotation), new System.Drawing.PointF(X, Y), System.Drawing.Drawing2D.MatrixOrder.Append);
                g.Transform = myMatrix;
            }
            System.Drawing.Pen myPen = new System.Drawing.Pen(m_lineColor, (float)m_lineWidth);
            if (this.m_BondType == ChemInfo.BondType.Single || this.m_BondType == ChemInfo.BondType.Aromatic) g.DrawLine(myPen, X, Y, m_connectedAtom.X, m_connectedAtom.Y);
            if (this.m_BondType == ChemInfo.BondType.Double)
            {
                g.DrawLine(myPen, X + m_OffsetX, Y + m_OffsetY, m_connectedAtom.X + m_OffsetX, m_connectedAtom.Y + m_OffsetY);
                g.DrawLine(myPen, X - m_OffsetX, Y - m_OffsetY, m_connectedAtom.X - m_OffsetX, m_connectedAtom.Y - m_OffsetY);
            }
            if (this.m_BondType == ChemInfo.BondType.Triple)
            {
                g.DrawLine(myPen, X + m_OffsetX, Y + m_OffsetY, m_connectedAtom.X + m_OffsetX, m_connectedAtom.Y + m_OffsetY);
                g.DrawLine(myPen, X, Y, m_connectedAtom.X, m_connectedAtom.Y);
                g.DrawLine(myPen, X - m_OffsetX, Y - m_OffsetY, m_connectedAtom.X - m_OffsetX, m_connectedAtom.Y - m_OffsetY);
            }
            g.EndContainer(gContainer);
        }
    }
}
