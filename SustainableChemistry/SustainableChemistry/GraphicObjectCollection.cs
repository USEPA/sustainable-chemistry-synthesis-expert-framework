using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{

    public class GraphicObjectCollection : List<GraphicObject>
    {
        protected int m_HorizRes;
        protected int m_VertRes;

        public GraphicObjectCollection()
        {
            m_HorizRes = 72;
            m_VertRes = 72;
        }

        public GraphicObjectCollection(List<GraphicObject> objects)
        {
            m_HorizRes = 72;
            m_VertRes = 72;
            this.AddRange(objects);
        }

        public GraphicObjectCollection(GraphicObject[] objects)
        {
            m_HorizRes = 72;
            m_VertRes = 72;
            this.AddRange(objects);
        }

        public int HorizontalResolution
        {
            get
            {
                return m_HorizRes;
            }

            set
            {
                m_HorizRes = value;
            }
        }

        public int VerticalResolution
        {
            get

            {
                return m_VertRes;
            }
            set
            {
                m_VertRes = value;
            }
        }

        public void DrawObjects(System.Drawing.Graphics g, double Scale)
        {
            System.Drawing.Drawing2D.GraphicsContainer gCon = g.BeginContainer();
            System.Drawing.Drawing2D.Matrix myOriginalMatrix = g.Transform;
            g.PageUnit = System.Drawing.GraphicsUnit.Pixel;
            g.ScaleTransform((float)Scale, (float)Scale);
            foreach (GraphicObject obj in this)
            {
                obj.Draw(g);
            }
            g.EndContainer(gCon);
            g.Transform = myOriginalMatrix;
        }

        public void DrawSelectedObject(System.Drawing.Graphics g, GraphicObject selectedObject, double Scale)
        {
            System.Drawing.Drawing2D.GraphicsContainer gCon1 = g.BeginContainer();
            g.ScaleTransform((float)Scale, (float)Scale, System.Drawing.Drawing2D.MatrixOrder.Append);
            System.Drawing.Drawing2D.GraphicsContainer gCon2 = g.BeginContainer();
            g.PageUnit = System.Drawing.GraphicsUnit.Pixel;

            if (selectedObject != null)
            {
                System.Drawing.Pen selectionPen = new System.Drawing.Pen(System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.HotTrack));
                selectionPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                selectionPen.Width = 1;

                if (selectedObject.Rotation != 0)
                {
                    System.Drawing.Drawing2D.Matrix myMatrix = g.Transform;
                    myMatrix.RotateAt((float)selectedObject.Rotation, new System.Drawing.PointF((float)selectedObject.X, (float)selectedObject.Y), System.Drawing.Drawing2D.MatrixOrder.Append);
                    g.Transform = myMatrix;
                }
            }
            g.EndContainer(gCon2);
            g.EndContainer(gCon1);
        }

        public GraphicObject FindObjectAtPoint(System.Drawing.Point pt)
        {
            foreach (GraphicObject drawObj in this)
            {
                if (drawObj.HitTest(pt)) return drawObj;
            }
            return null;
        }
    }
}
