using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistryWeb.ChemInfo
{
    public enum BondType
    {
        Single = 1,
        Double = 2,
        Triple = 3,
        Aromatic = 4,
        SingleOrDouble = 5,
        SingleOrAromatic = 6,
        DoubleOrAromatic = 7,
        Any = 8,
        Disconnected = 16
    }

    public enum BondStereo
    {
        NotStereoOrUseXYZ = 0,
        Up = 1,
        cisOrTrans = 3,
        Down = 4,
        Either = 6,
        cis = 16,
        trans = 32
    }

    public enum BondTopology
    {
        Either = 0,
        Ring = 1,
        Chain = 3,
    }

    [Flags]
    public enum BondReactingCenterStatus
    {
        notACenter = -1,
        Unmarked = 0,
        aCenter = 1,
        noChange = 2,
        bondMadeOrBroken = 4,
        bondOrderChanges = 8
    }

    class BondTypeConverter : System.ComponentModel.ExpandableObjectConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            if (typeof(Bond).IsAssignableFrom(destinationType))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, System.Type destinationType)
        {
            if (typeof(System.String).IsAssignableFrom(destinationType) && typeof(Bond).IsAssignableFrom(value.GetType()))
            {
                return ((Bond)value).ConnectedAtom.AtomicName;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    };

    [Serializable]
    [System.ComponentModel.TypeConverter(typeof(BondTypeConverter))]
    public class Bond
    {
        public Bond(Atom parent, Atom connectedAtom, BondType type, BondStereo stereo, BondTopology topology, BondReactingCenterStatus rcStatus)
        {
            ParentAtom = parent;
            ConnectedAtom = connectedAtom;
            BondType = type;
            DrawType = type;
            Stereo = stereo;
            Topology = topology;
            ReactingCenter = rcStatus;
            BondLength = 0; // parent.CovalentRadius + connectedAtom.CovalentRadius;
        }

        public Atom ParentAtom { get; }
        public Atom ConnectedAtom { get; }
        public double BondLength { get; }
        public BondType BondType { get; internal set; }
        public BondType DrawType { get; internal set; }
        public BondStereo Stereo { get; internal set; }
        public BondTopology Topology { get; internal set; }
        public BondReactingCenterStatus ReactingCenter { get; internal set; }

        public bool CompareTo(Bond b)
        {
            if (b.ParentAtom.Element != this.ParentAtom.Element) return false;
            if (b.ConnectedAtom.Element != this.ConnectedAtom.Element) return false;
            if (b.BondType != BondType) return false;
            if (b.Stereo != Stereo) return false;
            if (b.Topology != Topology) return false;
            return true;
        }

        //public double DistanceBetweenAtoms
        //{
        //    get
        //    {
        //        return Math.Sqrt(this.DistanceBetweenAtomsSquared);
        //    }

        //}


        //public double DistanceBetweenAtomsSquared
        //{
        //    get
        //    {
        //        return Math.Pow((this.ParentAtom.Location2D.X - this.ConnectedAtom.Location2D.X), 2) + Math.Pow((this.ParentAtom.Location2D.Y - this.ConnectedAtom.Location2D.Y), 2);
        //    }
        //}

        //public int Angle
        //{
        //    get
        //    {
        //        return m_Angle;
        //    }
        //    set
        //    {
        //        m_Angle = value;
        //        this.ParentAtom.Angle_2D = value;
        //    }
        //}

        //public System.Drawing.Point StartPoint
        //{
        //    get
        //    {
        //        return this.ParentAtom.Location2D;
        //    }
        //    //set
        //    //{
        //    //    this.m_ParentAtom.Location2D = new System.Drawing.Point(value.X, value.Y);
        //    //    this.m_StaringPoint = value;
        //    //}
        //}
    }
}