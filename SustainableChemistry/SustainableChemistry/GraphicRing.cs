using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    class GraphicRing : ShapeGraphic
    {
        public GraphicRing(ChemInfo.Atom[] ring) : base()
        {
            this.AddRing(ring);
        }
        public GraphicRing(System.Drawing.Point graphicPosition, ChemInfo.Atom[] ring) : base(graphicPosition)
        {
            this.AddRing(ring);
        }
        public GraphicRing(int posX, int posY, ChemInfo.Atom[] ring) : base(posX, posY)
        {
            this.AddRing(ring);
        }
        public GraphicRing(System.Drawing.Point graphicPosition, System.Drawing.Size graphicSize, ChemInfo.Atom[] ring) : base(graphicPosition, graphicSize)
        {
            this.AddRing(ring);
        }
        public GraphicRing(int posX, int posY, System.Drawing.Size graphicSize, ChemInfo.Atom[] ring) : base(posX, posY, graphicSize)
        {
            this.AddRing(ring);
        }
        public GraphicRing(int posX, int posY, int width, int height, ChemInfo.Atom[] ring) : base(posX, posY, width, height)
        {
            this.AddRing(ring);
        }
        public GraphicRing(System.Drawing.Point graphicPosition, double Rotation, ChemInfo.Atom[] ring) : base(graphicPosition, Rotation)
        {
            this.AddRing(ring);
        }
        public GraphicRing(int posX, int posY, double Rotation, ChemInfo.Atom[] ring) : base(posX, posY, Rotation)
        {
            this.AddRing(ring);
        }
        public GraphicRing(System.Drawing.Point graphicPosition, System.Drawing.Size graphicSize, double Rotation, ChemInfo.Atom[] ring) : base(graphicPosition, graphicSize, Rotation)
        {
            this.AddRing(ring);
        }
        public GraphicRing(int posX, int posY, System.Drawing.Size graphicSize, double Rotation, ChemInfo.Atom[] ring) : base(posX, posY, graphicSize, Rotation)
        {
            this.AddRing(ring);
        }
        public GraphicRing(int posX, int posY, int width, int height, double Rotation, ChemInfo.Atom[] ring) : base(posX, posY, width, height, Rotation)
        {
            this.AddRing(ring);
        }


        List<GraphicObject> gObjectCollection = new List<GraphicObject>();

        override public bool HitTest(System.Drawing.Point pt)
        {
            foreach(GraphicObject graphicObj in gObjectCollection)
            {
                if (graphicObj.HitTest(pt))
                {
                    return true;
                }
            }
            return false;
        }

        override public bool HitTest(System.Drawing.Rectangle rect)
        {//is this object contained within the supplied rectangle
            foreach (GraphicObject graphicObj in gObjectCollection)
            {
                if (graphicObj.HitTest(rect))
                {
                    return true;
                }
            }
            return false;
        }

        override public void Draw(System.Drawing.Graphics g)
        {//is this object contained within the supplied rectangle
            foreach (GraphicObject graphicObj in gObjectCollection)
            {
                graphicObj.Draw(g);
            }
        }

        override public void SetPosition(System.Drawing.Point Value)
        {
            System.Drawing.Point delta = new System.Drawing.Point((m_Position.X - Value.X), (m_Position.Y - Value.Y));
            m_Position = Value;
            foreach (GraphicObject graphicObj in gObjectCollection)
            {
                    System.Drawing.Point currentLoc = graphicObj.GetPosition();
                    System.Drawing.Point newLoc = new System.Drawing.Point((currentLoc.X - delta.X), (currentLoc.Y - delta.Y));
                    graphicObj.SetPosition(newLoc);
            }
        }

        public void AddRing(ChemInfo.Atom[] ring)
        {

        }

        //chuck added function
        public void Clear()
        {
            gObjectCollection.Clear();
        }

        public void Add(GraphicObject Value)
        {
            int numComp = gObjectCollection.Count;

            if (numComp == 0)
            {
                m_Position = Value.GetPosition();
                m_Size = Value.GetSize();
            }
            if (numComp > 0)
            {
                m_AutoSize = false;
                int oldX = m_Position.X;
                int oldY = m_Position.Y;
                int oldBottom = m_Position.Y + m_Size.Height;
                int oldRight = m_Position.X + m_Size.Width;

                System.Drawing.Point newPos = Value.GetPosition();
                System.Drawing.Size newSize = Value.GetSize();
                int newX = newPos.X;
                int newY = newPos.Y;
                int newBottom = newY + newSize.Height;
                int newRight = newX + newSize.Width;
                System.Drawing.Point offset = new System.Drawing.Point();
                if (newX < oldX) m_Position.X = newX;
                if (newY < oldY) m_Position.Y = newY;
                if (newBottom > oldBottom) m_Size.Height = newBottom - m_Position.Y;
                if (newRight > oldRight) m_Size.Width = newRight - m_Position.X;
            }
            gObjectCollection.Add(Value);
        }
    }
}
