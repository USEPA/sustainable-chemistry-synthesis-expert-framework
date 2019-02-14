using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    public enum SizeDirection
    {
        Northwest,
        North,
        Northeast,
        East,
        Southeast,
        South,
        Southwest,
        West,
        NA
    };

    public abstract class GraphicObject
    {
        protected System.Drawing.Point m_Position = new System.Drawing.Point(0, 0);
        protected System.Drawing.Size m_Size = new System.Drawing.Size(0, 0);
        protected double m_Rotation = 0;
        protected bool m_AutoSize = false;
        //GraphicObjectCollection m_Container;
        protected bool m_Selected = false;

        //Constructors
        public GraphicObject()
        {
        }

        public GraphicObject(System.Drawing.Point graphicPosition)
        {
            this.m_Position = graphicPosition;
        }

        public GraphicObject(int posX, int posY)
        {
            this.m_Position = new System.Drawing.Point(posX, posY);
        }

        public GraphicObject(System.Drawing.Point graphicPosition, System.Drawing.Size graphicSize)
        {
            this.m_Position = graphicPosition;
            this.m_Size = graphicSize;
        }

        public GraphicObject(int posX, int posY, System.Drawing.Size graphicSize)
        {
            this.m_Position = new System.Drawing.Point(posX, posY);
            this.m_Size = graphicSize;
        }

        public GraphicObject(int posX, int posY, int width, int height)
        {
            this.m_Position = new System.Drawing.Point(posX, posY);
            this.m_Size = new System.Drawing.Size(width, height);
        }

        public GraphicObject(System.Drawing.Point graphicPosition, double Rotation)
        {
            this.m_Position = graphicPosition;
            m_Rotation = Rotation;
        }

        public GraphicObject(int posX, int posY, double Rotation)
        {
            this.m_Position = new System.Drawing.Point(posX, posY);
            m_Rotation = Rotation;
        }

        public GraphicObject(System.Drawing.Point graphicPosition, System.Drawing.Size graphicSize, double Rotation)
        {
            this.m_Position = graphicPosition;
            this.m_Size = graphicSize;
            m_Rotation = Rotation;
        }

        public GraphicObject(int posX, int posY, System.Drawing.Size graphicSize, double Rotation)
        {
            this.m_Position = new System.Drawing.Point(posX, posY);
            this.m_Size = graphicSize;
            m_Rotation = Rotation;
        }

        public GraphicObject(int posX, int posY, int width, int height, double Rotation)
        {
            this.m_Position = new System.Drawing.Point(posX, posY);
            this.m_Size = new System.Drawing.Size(width, height);
            m_Rotation = Rotation;
        }

        // Properties 
        public object Tag { get; set; } = null;

        public virtual bool AutoSize
        {
            get
            {
                return m_AutoSize;
            }

            set
            {
                m_AutoSize = value;
            }
        }

        public virtual bool Selected
        {
            get
            {
                return m_Selected;
            }

            set
            {
                m_Selected = value;
            }
        }

        public virtual int X
        {
            get

            {
                return m_Position.X;
            }

            set
            {
                m_Position.X = value;
            }
        }

        public virtual int Y
        {
            get
            {
                return m_Position.Y;
            }

            set
            {
                m_Position.Y = value;
            }
        }

        public virtual int Height
        {
            get
            {
                return m_Size.Height;
            }
            set
            {
                m_Size.Height = value;
            }
        }

        public virtual int Width
        {
            get
            {
                return m_Size.Width;
            }
            set
            {
                m_Size.Width = value;
            }
        }

        public virtual double Rotation
        {
            get
            {
                return m_Rotation;
            }
            set
            {
                if (System.Math.Abs(value) < 360)
                {
                    m_Rotation = value;
                }
                else
                {
                    throw new System.ArgumentOutOfRangeException("Rotation must be between -360.0 and 360.0");
                }
            }
        }

        //Draw method. Abstract virtual function that must be implemented by the graphic object class.
        public abstract void Draw(System.Drawing.Graphics g);


        // Graphics methods
        //chuck's new code 2/20/04
        //this method indicates whether point is along outline of graphic
        //and if so, what type of cursor should show
        public virtual void BoundaryTest(System.Drawing.Point pt, SizeDirection dir)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black, 3);

            gp.AddRectangle(new System.Drawing.Rectangle(this.m_Position.X - 3, this.m_Position.Y - 3, this.m_Size.Width + 6, this.m_Size.Height + 6));
            if (this.m_Rotation != 0)
            {
                myMatrix.RotateAt((float)this.m_Rotation, new System.Drawing.PointF((float)this.X, (float)this.Y), System.Drawing.Drawing2D.MatrixOrder.Append);
            }

            gp.Transform(myMatrix);
            dir = SizeDirection.NA;
            if (gp.IsOutlineVisible(pt, pen))
            {
                //user has placed the mouse along the outline of the selected
                //object - change the mouse to allow for resizing
                System.Drawing.RectangleF rect = gp.GetBounds();
                if (Math.Abs((int)rect.Left - pt.X) <= 2)
                {
                    if (Math.Abs((int)rect.Top - pt.Y) <= 2)
                        dir = SizeDirection.Northwest;
                    else if (Math.Abs((int)rect.Bottom - pt.Y) <= 2)
                        dir = SizeDirection.Southwest;
                    else
                        dir = SizeDirection.West;
                }
                else if (Math.Abs((int)rect.Right - pt.X) <= 2)
                {
                    if (Math.Abs((int)rect.Top - pt.Y) <= 2)
                        dir = SizeDirection.Northeast;
                    else if (Math.Abs((int)rect.Bottom - pt.Y) <= 2)
                        dir = SizeDirection.Southeast;
                    else
                        dir = SizeDirection.East;
                }
                else if (Math.Abs((int)rect.Top - pt.Y) <= 2)
                    dir = SizeDirection.North;
                else
                    dir = SizeDirection.South;
            }
        }
        //end revised code
        public virtual bool HitTest(System.Drawing.Point pt)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
            gp.AddRectangle(new System.Drawing.Rectangle(this.m_Position.X, this.m_Position.Y, this.m_Size.Width, this.m_Size.Height));
            if (this.m_Rotation != 0)
            {
                myMatrix.RotateAt((float)(this.m_Rotation), new System.Drawing.PointF((float)this.X, (float)this.Y),
                    System.Drawing.Drawing2D.MatrixOrder.Append);
            }
            gp.Transform(myMatrix);
            return gp.IsVisible(pt);
        }

        public virtual bool HitTest(System.Drawing.Rectangle rect)
        {//is this object contained within the supplied rectangle
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
            gp.AddRectangle(new System.Drawing.Rectangle(this.m_Position.X, this.m_Position.Y, this.m_Size.Width, this.m_Size.Height));
            if (this.m_Rotation != 0)
            {
                myMatrix.RotateAt((float)this.m_Rotation, new System.Drawing.PointF((float)this.m_Position.X, (float)this.m_Position.Y),
                    System.Drawing.Drawing2D.MatrixOrder.Append);
            }
            gp.Transform(myMatrix);
            System.Drawing.Rectangle gpRect = System.Drawing.Rectangle.Round(gp.GetBounds());
            return rect.Contains(gpRect);
        }

        public virtual System.Drawing.Point GetPosition()
        {
            System.Drawing.Point myPosition = new System.Drawing.Point(m_Position.X, m_Position.Y);
            return myPosition;
        }

        public virtual void SetPosition(System.Drawing.Point Value)
        {
            //'any value is currently ok,
            //'but I might want to add validation later.
            m_Position = Value;
        }

        public virtual void SetSize(System.Drawing.Size Value)
        {
            m_Size = Value;
        }

        public virtual System.Drawing.Size GetSize()
        {
            System.Drawing.Size mySize = new System.Drawing.Size(m_Size.Width, m_Size.Height);
            return mySize;
        }
    }
}
