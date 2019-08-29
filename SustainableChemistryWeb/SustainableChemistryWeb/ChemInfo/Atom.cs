using System;
using System.Collections.Generic;
using System.Linq;

namespace SustainableChemistryWeb.ChemInfo
{

    public enum AtomType
    {
        NONE = 0x00,
        ORGANIC = 0x01,
        AROMATIC = 0x03,
        WILDCARD = 0x04
    }

    [Flags]
    public enum Chirality
    {
        UNSPECIFIED = 0x00,
        TETRAHEDRAL_CLOCKWISE = 0x01,
        TETRAHEDRAL_COUNTER_CLOCKWISE = 0x02,
        OTHER = 0x04,
        CIP_S = 0x10,
        CIP_R = 0x20
    }

    static class WeiningerInitialInvariant
    {
        public static long ToInt64(Atom a)
        {
            byte[] retval = new byte[8];
            retval[0] = 0;
            retval[1] = 0;
            retval[2] = 0;
            retval[3] = BitConverter.GetBytes(a.Degree)[3];
            retval[4] = BitConverter.GetBytes(a.NumberOfBonds)[3];
            retval[5] = BitConverter.GetBytes(a.AtomicNumber)[3];
            retval[6] = BitConverter.GetBytes(a.Charge)[3];
            retval[7] = BitConverter.GetBytes(a.NumHydrogens)[3];
            return BitConverter.ToInt64(retval, 0);
        }
    }

    class IntArrayTypeConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            if ((typeof(int[])).IsAssignableFrom(destinationType))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, System.Type destinationType)
        {
            if ((typeof(System.String)).IsAssignableFrom(destinationType) && (typeof(int[]).IsAssignableFrom(value.GetType())))
            {
                string retVal = string.Empty;
                int[] v = (int[])value;
                if (v != null && v.Length != 0)
                {
                    for (int i = 0; i < v.Length - 1; i++)
                    {
                        retVal = retVal + v[i].ToString() + " , ";
                    }
                    retVal = retVal + v[v.Length - 1];
                }
                //else return v[0].
                return retVal;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    };

    class AtomTypeConverter : System.ComponentModel.ExpandableObjectConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            if ((typeof(string)).IsAssignableFrom(destinationType))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, System.Type destinationType)
        {
            if ((typeof(System.String)).IsAssignableFrom(destinationType) && (typeof(Atom).IsAssignableFrom(value.GetType())))
            {
                return ((Atom)value).AtomicName;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    };

    public class WeiningerInvariant : Comparer<WeiningerInvariant>
    {
        Atom m_Atom;
        public int NumberOfConnections { get; set; } = 0;
        public int NumberOfNonHydrogenBonds { get; set; } = 0;
        public int AtomicNumber { get; set; } = 0;
        public int SignOfCharge { get; set; } = 0;
        public int Charge { get; set; } = 0;
        public int NumberOfAttachedHydrogens { get; set; } = 0;

        public WeiningerInvariant(Atom a)
        {
            m_Atom = a;
            this.Reset();
        }

        public void Reset()
        {
            this.NumberOfConnections = m_Atom.Degree;
            this.NumberOfNonHydrogenBonds = m_Atom.NumberOfNonHydrogens;
            this.AtomicNumber = m_Atom.AtomicNumber;
            this.SignOfCharge = m_Atom.Charge;
            this.Charge = m_Atom.Charge;
            this.NumberOfAttachedHydrogens = m_Atom.NumHydrogens;
        }

        public void AddExtendedConnectivity(WeiningerInvariant y)
        {
            this.NumberOfConnections = y.NumberOfConnections;
            this.NumberOfNonHydrogenBonds = y.NumberOfNonHydrogenBonds;
            this.AtomicNumber = y.AtomicNumber;
            this.SignOfCharge = y.SignOfCharge;
            this.Charge = y.Charge;
            this.NumberOfAttachedHydrogens = y.NumberOfAttachedHydrogens;
        }

        public void AddExtendedConnectivity(Atom a)
        {
            this.NumberOfConnections = a.Degree;
            this.NumberOfNonHydrogenBonds = a.NumberOfNonHydrogens;
            this.AtomicNumber = a.AtomicNumber;
            this.SignOfCharge = a.Charge;
            this.Charge = a.Charge;
            this.NumberOfAttachedHydrogens = a.NumHydrogens;
        }

        public static bool operator >(WeiningerInvariant w1, WeiningerInvariant w2)
        {
            // Number of connections is the first test.
            if (w1.NumberOfConnections > w2.NumberOfConnections) return true;
            // Followed by the number of non-hydrogen bonds.
            if (w1.NumberOfNonHydrogenBonds > w2.NumberOfNonHydrogenBonds) return true;
            //Then atomic number        
            if (w1.AtomicNumber > w2.AtomicNumber) return true;
            // Sign of charge, and then charge, but why both. I'm guessing 1980s computer issues that Moore's Law solved.
            // This can be done in one sort, which the same results, especially since the comparer returns an integer.
            if (w1.SignOfCharge > w2.SignOfCharge) return true;
            // Next is number of attached hydrogens.
            if (w1.NumberOfAttachedHydrogens > w2.NumberOfAttachedHydrogens) return true;
            return false;
        }

        public static bool operator <(WeiningerInvariant w1, WeiningerInvariant w2)
        {
            // Number of connections is the first test.
            if (w1.NumberOfConnections < w2.NumberOfConnections) return true;
            // Followed by the number of non-hydrogen bonds.
            if (w1.NumberOfNonHydrogenBonds < w2.NumberOfNonHydrogenBonds) return true;
            //Then atomic number        
            if (w1.AtomicNumber < w2.AtomicNumber) return true;
            // Sign of charge, and then charge, but why both. I'm guessing 1980s computer issues that Moore's Law solved.
            // This can be done in one sort, which the same results, especially since the comparer returns an integer.
            if (w1.SignOfCharge < w2.SignOfCharge) return true;
            // Next is number of attached hydrogens.
            if (w1.NumberOfAttachedHydrogens < w2.NumberOfAttachedHydrogens) return true;
            return false;
        }

        public static bool operator ==(WeiningerInvariant w1, WeiningerInvariant w2)
        {
            // Number of connections is the first test.
            if (w1.NumberOfConnections != w2.NumberOfConnections) return false;
            // Followed by the number of non-hydrogen bonds.
            if (w1.NumberOfNonHydrogenBonds != w2.NumberOfNonHydrogenBonds) return false;
            //Then atomic number        
            if (w1.AtomicNumber != w2.AtomicNumber) return false;
            // Sign of charge, and then charge, but why both. I'm guessing 1980s computer issues that Moore's Law solved.
            // This can be done in one sort, which the same results, especially since the comparer returns an integer.
            if (w1.SignOfCharge != w2.SignOfCharge) return false;
            // Next is number of attached hydrogens.
            if (w1.NumberOfAttachedHydrogens != w2.NumberOfAttachedHydrogens) return false;
            return true;
        }

        public static bool operator !=(WeiningerInvariant w1, WeiningerInvariant w2)
        {
            // Number of connections is the first test.
            if ((w1.NumberOfConnections == w2.NumberOfConnections) &&
            // Followed by the number of non-hydrogen bonds.
                (w1.NumberOfNonHydrogenBonds == w2.NumberOfNonHydrogenBonds) &&
            //Then atomic number        
                (w1.AtomicNumber == w2.AtomicNumber) &&
            // Sign of charge, and then charge, but why both. I'm guessing 1980s computer issues that Moore's Law solved.
            // This can be done in one sort, which the same results, especially since the comparer returns an integer.
                (w1.SignOfCharge == w2.SignOfCharge) &&
                // Next is number of attached hydrogens.
                (w1.NumberOfAttachedHydrogens == w2.NumberOfAttachedHydrogens)) return false;
            return true;
        }

        public override int Compare(WeiningerInvariant x, WeiningerInvariant y)
        {
            // Number of connections is the first test.
            if (x.NumberOfConnections != y.NumberOfConnections) return x.NumberOfConnections - y.NumberOfConnections;
            // Followed by the number of non-hydrogen bonds.
            if (x.NumberOfNonHydrogenBonds != y.NumberOfNonHydrogenBonds) return x.NumberOfNonHydrogenBonds - y.NumberOfNonHydrogenBonds;
            //Then atomic number        
            if (x.AtomicNumber != y.AtomicNumber) return x.AtomicNumber - y.AtomicNumber;
            // Sign of charge, and then charge, but why both. I'm guessing 1980s computer issues that Moore's Law solved.
            // This can be done in one sort, which the same results, especially since the comparer returns an integer.
            if (x.Charge != y.Charge) return x.Charge - y.Charge;
            // Next is number of attached hydrogens.
            if (x.NumberOfAttachedHydrogens != y.NumberOfAttachedHydrogens) return x.NumberOfAttachedHydrogens - y.NumberOfAttachedHydrogens;
            return 0;
        }
    }

    [Serializable]
    [System.ComponentModel.TypeConverter(typeof(AtomTypeConverter))]
    [Newtonsoft.Json.JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)]
    public class Atom
    {
        private BondCollection m_Bonds = new BondCollection();
        private List<Atom> m_ConnectedAtoms = new List<Atom>();

        //int degree;
        ELEMENTS m_Element;
        int m_Isotope;
        int m_Charge;
        int m_ExplicitHydrogens;
        Chirality m_Chiral;
        int _x = 0;
        int _y = 0;
        System.Random random = new Random();

        // Constructors
        public Atom(string element)
        {
            if (element == "*") m_Element = ELEMENTS.WILD_CARD;
            else m_Element = (ELEMENTS)Enum.Parse(typeof(ELEMENTS), element);
            m_ExplicitHydrogens = 0;
            //degree = 0;
            m_Isotope = 0;
            m_Charge = 0;
            m_Chiral = Chirality.UNSPECIFIED;
            m_Chiral = Chirality.UNSPECIFIED;
            this.SetColor(SustainableChemistryWeb.ChemInfo.Element.ElementColor(m_Element));
            //this.m_AtomicMass = SustainableChemistryWeb.ChemInfo.Element.ExactMass(e);
            _x = (int)(random.NextDouble() * 100);
            _y = (int)(random.NextDouble() * 100);
            //m_WeiningerInvariant = new WeiningerInvariant(this);
        }

        public Atom(string element, AtomType type)
        {
            if (element == "*") m_Element = ELEMENTS.WILD_CARD;
            else if (element == "X") m_Element = ELEMENTS.Halogen;
            else m_Element = (ELEMENTS)Enum.Parse(typeof(ELEMENTS), element);
            m_ExplicitHydrogens = 0;
            m_Isotope = 0;
            m_Charge = 0;
            m_Chiral = Chirality.UNSPECIFIED;
            AtomType = type;
            m_Chiral = Chirality.UNSPECIFIED;
            if (this.Element != ELEMENTS.Halogen) this.SetColor(SustainableChemistryWeb.ChemInfo.Element.ElementColor(m_Element));
            else Color = System.Drawing.Color.FromName("olivedrab");
            _x = (int)(random.NextDouble() * 100);
            _y = (int)(random.NextDouble() * 100);
            //m_WeiningerInvariant = new WeiningerInvariant(this);
        }

        public Atom(string element, AtomType type, Chirality chirality)
        {
            //degree = 0;
            if (element == "*") m_Element = ELEMENTS.WILD_CARD;
            else m_Element = (ELEMENTS)Enum.Parse(typeof(ELEMENTS), element);
            m_ExplicitHydrogens = 0;
            m_Isotope = 0;
            m_Charge = 0;
            m_Chiral = Chirality.UNSPECIFIED;
            AtomType = AtomType.NONE;
            m_Chiral = chirality;
            this.SetColor(SustainableChemistryWeb.ChemInfo.Element.ElementColor(m_Element));
            _x = (int)(random.NextDouble() * 100);
            _y = (int)(random.NextDouble() * 100);
            //m_WeiningerInvariant = new WeiningerInvariant(this);
        }

        public Atom(string element, int isotope)
        {
            //degree = 0;
            if (element == "*") m_Element = ELEMENTS.WILD_CARD;
            else m_Element = (ELEMENTS)Enum.Parse(typeof(ELEMENTS), element);
            m_ExplicitHydrogens = 0;
            isotope = (byte)isotope;
            m_Charge = 0;
            m_Chiral = Chirality.UNSPECIFIED;
            AtomType = AtomType.NONE;
            m_Chiral = Chirality.UNSPECIFIED;
            this.SetColor(SustainableChemistryWeb.ChemInfo.Element.ElementColor(m_Element));
            _x = (int)(random.NextDouble() * 100);
            _y = (int)(random.NextDouble() * 100);
            //m_WeiningerInvariant = new WeiningerInvariant(this);
        }

        public Atom(string element, AtomType type, int isotope, Chirality chirality, int hCount, int charge, int atomClass)
        {
            //degree = 0;
            if (element == "*") m_Element = ELEMENTS.WILD_CARD;
            else m_Element = (ELEMENTS)Enum.Parse(typeof(ELEMENTS), element);
            m_ExplicitHydrogens = hCount;
            m_Isotope = isotope;
            m_Charge = charge;
            m_Chiral = chirality;
            AtomType = type;
            this.SetColor(SustainableChemistryWeb.ChemInfo.Element.ElementColor(m_Element));
            _x = (int)(random.NextDouble() * 100);
            _y = (int)(random.NextDouble() * 100);
            //m_WeiningerInvariant = new WeiningerInvariant(this);
        }

        [System.ComponentModel.Browsable(false)]
        public bool Visited { get; set; } = false;
        [System.ComponentModel.Browsable(false)]
        public bool inGroup { get; set; } = false;

        /// <summary>
        /// Gets the atomic number of the current element.
        /// </summary>
        /// <remarks>
        /// The atomic number is the 
        /// </remarks>
        [Newtonsoft.Json.JsonProperty]
        public int AtomicNumber
        {
            get
            {
                return (int)m_Element;
            }
        }

        [Newtonsoft.Json.JsonProperty]
        public string AtomicSymbol
        {
            get
            {
                return m_Element.ToString();
            }
        }

        [Newtonsoft.Json.JsonProperty]
        public string AtomicName
        {
            get
            {
                return SustainableChemistryWeb.ChemInfo.Element.Name(m_Element);
            }
        }

        /// <summary>
        /// Gets the atom's color.
        /// </summary>
        /// <remarks>
        /// The atom color is represented as a <see cref="System.Drawing.Color"/> value.
        /// </remarks>
        /// <value>The atom's color.</value>
        [Newtonsoft.Json.JsonProperty]
        public System.Drawing.Color Color { get; set; }

        /// <summary>
        /// Gets the number of non-Hydrogen atoms connected to the current atom.
        /// </summary>
        /// <remarks>
        /// The degree is the number of non-Hydrogen atoms connected to the current atom. Hydrogens are excluded from the connected atom count because 
        /// of the way that hydrogens are handled on organic atoms. The <a href="http://opensmiles.org/opensmiles.html#orgsbst">OpenSmiles Specification</a> for an
        /// organic atom indicates that implicit hydrogens are added such that the valence of the atom is in its lowest normal state for the element. 
        /// </remarks>
        /// <value>Number of connected non-Hydrogen atoms.</value>
        [Newtonsoft.Json.JsonProperty]
        public int Degree
        {
            get
            {
                return this.ConnectedAtoms.Length;
            }
        }

        [Newtonsoft.Json.JsonProperty]
        public int NumberOfBonds
        {
            get
            {
                int retVal = 0;
                foreach (Atom a in this.ConnectedAtoms)
                {
                    Bond b = null;
                    foreach (Bond testBond in this.BondedAtoms)
                    {
                        if (testBond.ConnectedAtom == a) b = testBond;
                    }
                    if (b == null)
                    {
                        foreach (Bond testBond in a.BondedAtoms)
                        {
                            if (testBond.ConnectedAtom == this) b = testBond;
                        }
                    }
                    if (b.BondType == BondType.Single) retVal = retVal + 1;
                    if (b.BondType == BondType.Aromatic) retVal = retVal + 1;
                    if (b.BondType == BondType.Double) retVal = retVal + 2;
                    if (b.BondType == BondType.Triple) retVal = retVal + 3;
                }
                if (this.AtomType == AtomType.AROMATIC) retVal = retVal + 1;
                return retVal;
            }
        }

        [Newtonsoft.Json.JsonProperty]
        public int NumHydrogens
        {
            get
            {
                //if (HydrogenCount != 0) return HydrogenCount;
                if (m_ExplicitHydrogens != 0) return m_ExplicitHydrogens;
                if (this.AtomType == AtomType.ORGANIC || this.AtomType == AtomType.NONE)
                {
                    return this.Valence - this.NumberOfBonds;
                }
                if (this.AtomType == AtomType.AROMATIC)
                {
                    switch (this.Element)
                    {
                        case ELEMENTS.B:
                            return 2;
                        case ELEMENTS.C:
                            return 3 - this.ConnectedAtoms.Length;
                        case ELEMENTS.N:
                            return 0;
                        case ELEMENTS.O:
                            return 0;
                        case ELEMENTS.S:
                            return 2;
                        case ELEMENTS.P:
                            return 1;
                        default:
                            return 0;
                    }
                }
                return 0;
            }
        }

        [Newtonsoft.Json.JsonProperty]
        public int NumPiElectrons
        {
            get
            {
                int bonds = this.NumberOfBonds + this.NumHydrogens;
                switch (this.Element)
                {
                    case ELEMENTS.B:
                        if (this.Degree == 2 && bonds == 3) return 1;
                        return 0;
                    case ELEMENTS.C:
                        if ((this.Degree + this.NumHydrogens) == 3)
                        {
                            if (bonds == 3) return 1;
                            if (bonds == 4 && this.Charge == 0)
                            {
                                foreach (Atom a in this.ConnectedAtoms)
                                {
                                    if (a.Element != ELEMENTS.C)
                                        if (this.GetBond(a).BondType == BondType.Double) return 0;
                                }
                                return 1;
                            }
                            if (this.Charge == -1 && bonds == 3) return 2;
                        }
                        if ((this.Degree + this.NumHydrogens) == 2 && bonds == 3 && (this.Charge == 1 || this.Charge == -1)) return 1;
                        return 0;
                    case ELEMENTS.N:
                        if ((this.Degree + this.NumHydrogens) == 3)
                        {
                            if (this.NumberOfBonds == 3 && Charge == 0) return 2;
                            if (this.NumberOfBonds == 4 && this.Charge == 1) return 1;
                            if (this.NumberOfBonds == 5 && this.Charge == 1) return 1;
                        }
                        if ((this.Degree + this.NumHydrogens) == 2)
                        {
                            if (this.NumberOfBonds == 2 && this.Charge == -1) return 2;
                            if (this.NumberOfBonds == 3 && this.Charge == 0) return 1;
                        }
                        return 0;
                    case ELEMENTS.O:
                        if (this.Charge == 0 && this.ConnectedAtoms.Length == 2) return 2;
                        if (this.Charge == +1 && this.NumberOfBonds == 3 && this.ConnectedAtoms.Length == 2) return 2;
                        return 0;
                    case ELEMENTS.S:
                        if ((this.Degree + this.NumHydrogens) == 2 && this.ConnectedAtoms.Length == 2) return 2;
                        if ((this.Degree + this.NumHydrogens) == 2 && this.ConnectedAtoms.Length == 2 && this.NumberOfBonds == 3 && this.Charge == +1) return 1;
                        if ((this.Degree + this.NumHydrogens) == 3 && this.NumberOfBonds == 4)
                        {
                            foreach (Atom a in this.ConnectedAtoms)
                            {
                                if (a.Element == ELEMENTS.O && this.GetBond(a).BondType == BondType.Double) return 2;
                            }
                        }
                        if (this.Degree == 3 && this.NumberOfBonds == 3)
                        {
                            foreach (Atom a in this.ConnectedAtoms)
                            {
                                if (a.Element == ELEMENTS.O && this.Charge == +1 && a.Charge == -1) return 2;
                            }
                        }
                        return 0;
                    case ELEMENTS.P:
                        if ((this.Degree + this.NumHydrogens) == 3) return 0;
                        return 1;
                    case ELEMENTS.As:
                        if ((this.Degree + this.NumHydrogens) == 3) return 0;
                        return 1;
                    case ELEMENTS.Se:
                        if ((this.Degree + this.NumHydrogens) == 2 && this.NumberOfBonds == 2 && this.Charge == 0) return 2;
                        if ((this.Degree + this.NumHydrogens) == 2 && this.NumberOfBonds == 3 && this.Charge == +1) return 1;
                        if ((this.Degree + this.NumHydrogens) == 3)
                        {
                            foreach (Atom a in this.ConnectedAtoms)
                            {
                                if (a.Element == ELEMENTS.O)
                                {
                                    if (this.GetBond(a).BondType == BondType.Double && a.Charge == 0) return 2;
                                    if (this.Charge == +1 && a.Charge == -1 && this.GetBond(a).BondType == BondType.Single) return 1;
                                }
                            }
                        }
                        return 0;

                    default:
                        return 0;
                }
            }
        }

        public ELEMENTS Element { get { return m_Element; } }

        public int Isotope
        {
            get
            {
                return (int)m_Isotope;
            }
            set
            {
                m_Isotope = (byte)value;
            }
        }

        public int ExplicitHydrogens
        {
            get
            {
                return m_ExplicitHydrogens;
            }
            set
            {
                m_ExplicitHydrogens = value;
            }
        }

        [Newtonsoft.Json.JsonProperty]
        public AtomType AtomType { get; set; } = AtomType.NONE;

        public int WeiningerRank { get; set; } = 0;

        public long WeiningerInitialInvariant
        {
            get
            {
                byte[] retval = new byte[8];
                retval[0] = 0;
                retval[1] = 0;
                retval[2] = BitConverter.GetBytes(this.Degree)[0];
                retval[3] = BitConverter.GetBytes(this.NumberOfBonds)[0];
                retval[4] = BitConverter.GetBytes(this.AtomicNumber)[0];
                retval[5] = this.m_Charge > 0 ? (byte)1 : (byte)0;
                retval[6] = BitConverter.GetBytes(this.Charge)[0];
                retval[7] = BitConverter.GetBytes(this.NumHydrogens)[0];
                if (BitConverter.IsLittleEndian) Array.Reverse(retval);
                return BitConverter.ToInt64(retval, 0);
            }
        }


        public Atom[] SetHydrogens()
        {
            List<Atom> hydrogens = new List<Atom>();
            for (int i = this.NumberOfBonds; i < this.Valence; i++)
            {
                Atom h = new Atom("H");
                this.AddBond(h, BondType.Single, BondStereo.NotStereoOrUseXYZ, BondTopology.Chain, BondReactingCenterStatus.notACenter);
                this.m_ConnectedAtoms.Add(h);
                h.Color = System.Drawing.Color.Fuchsia;
                hydrogens.Add(h);
            }
            return hydrogens.ToArray();
        }

        public Atom RemoveOneHydrogen()
        {
            foreach (Atom a in this.ConnectedAtoms)
            {
                if (a.Element == ELEMENTS.H)
                {
                    Bond bondToRemove = null;
                    foreach (Bond b in this.m_Bonds)
                    {
                        if (b.ConnectedAtom == a) bondToRemove = b;
                    }
                    if (bondToRemove != null)
                    {
                        this.m_Bonds.Remove(bondToRemove);
                        this.m_ConnectedAtoms.Remove(a);
                        return a;
                    }
                }
            }
            return null;
        }

        public Atom[] RemoveHydrogens()
        {
            List<Atom> hydrogens = new List<Atom>();
            foreach (Atom a in this.ConnectedAtoms)
            {
                if (a.Element == ELEMENTS.H)
                {
                    Bond bondToRemove = null;
                    foreach (Bond b in this.m_Bonds)
                    {
                        if (b.ConnectedAtom == a) bondToRemove = b;
                    }
                    if (bondToRemove != null)
                    {
                        this.m_Bonds.Remove(bondToRemove);
                        this.m_ConnectedAtoms.Remove(a);
                        hydrogens.Add(a);
                    }
                }
            }
            return hydrogens.ToArray();
        }

        void SetColor(int[] argb)
        {
            if (argb.Length == 3) this.Color = System.Drawing.Color.FromArgb(argb[0], argb[1], argb[2]);
            else if (argb.Length == 4) this.Color = System.Drawing.Color.FromArgb(argb[0], argb[1], argb[2], argb[3]);
            else this.Color = System.Drawing.Color.Black;
        }

        void ResetColor(System.Drawing.Color color)
        {
            this.SetColor(SustainableChemistryWeb.ChemInfo.Element.ElementColor(m_Element));
        }

        public System.Drawing.Color DefaultColor
        {
            get
            {
                int[] c = SustainableChemistryWeb.ChemInfo.Element.ElementColor(m_Element);
                if (c.Length == 3) return System.Drawing.Color.FromArgb(c[0], c[1], c[2]);
                else if (c.Length == 4) return System.Drawing.Color.FromArgb(c[0], c[1], c[2], c[3]);
                else return System.Drawing.Color.Black;
            }
        }

        public System.Drawing.Point Location2D
        {
            get
            {
                return new System.Drawing.Point(_x, _y);
            }
            set
            {
                _x = value.X;
                _y = value.Y;
                //foreach (Bond b in this.BondedAtoms)
                //{
                //    b.Angle = b.Angle;
                //}
            }
        }

        public double DeltaX { get; set; } = 0;
        public double DeltaY { get; set; } = 0;

        //double m_AtomicMass;
        public double AtomicMass
        {
            get
            {
                return SustainableChemistryWeb.ChemInfo.Element.ExactMass(m_Element);
            }
        }

        public double CovalentRadius
        {
            get
            {
                if (m_Element == ELEMENTS.Halogen) return SustainableChemistryWeb.ChemInfo.Element.CovalentRadius(SustainableChemistryWeb.ChemInfo.ELEMENTS.Cl);
                if (m_Element == ELEMENTS.WILD_CARD) return SustainableChemistryWeb.ChemInfo.Element.CovalentRadius(SustainableChemistryWeb.ChemInfo.ELEMENTS.C);
                return SustainableChemistryWeb.ChemInfo.Element.CovalentRadius(m_Element);
            }
        }

        public void AddConnectedAtom(Atom a)
        {
            this.m_ConnectedAtoms.Add(a);
        }

        public Atom[] ConnectedAtoms
        {
            get
            {
                return this.m_ConnectedAtoms.ToArray();
            }
        }

        public Bond AddBond(Atom atom, BondType type, BondStereo stereo, BondTopology topology, BondReactingCenterStatus rcStatus)
        {
            Bond b = new Bond(this, atom, type, BondStereo.NotStereoOrUseXYZ, BondTopology.Either, BondReactingCenterStatus.notACenter);
            this.BondedAtoms.Add(b);
            return b;
        }

        [System.ComponentModel.TypeConverter(typeof(BondCollectionTypeConverter))]
        public BondCollection BondedAtoms
        {
            get
            {
                return m_Bonds;
            }
        }

        public Bond GetBond(Atom a)
        {
            foreach (Bond b in m_Bonds)
            {
                if (b.ConnectedAtom == a)
                    return b;
            }
            foreach (Bond b in a.BondedAtoms)
            {
                if (b.ConnectedAtom == this)
                    return b;
            }
            return null;
        }

        public Bond[] GetBondsToAtomByElement(Atom a)
        {
            List<Bond> bonds = new List<Bond>();
            foreach (Bond b in m_Bonds)
            {
                if (b.ConnectedAtom.Element == a.Element)
                    bonds.Add(b);
            }
            return bonds.ToArray<Bond>();
        }

        public Bond[] GetBondsToElement(ELEMENTS element)
        {
            List<Bond> bonds = new List<Bond>();
            foreach (Bond b in m_Bonds)
            {
                if (b.ConnectedAtom.Element == element)
                    bonds.Add(b);
            }
            return bonds.ToArray<Bond>();
        }

        public Bond[] GetBondsToElementSymbol(String element)
        {
            List<Bond> bonds = new List<Bond>();
            foreach (Bond b in m_Bonds)
            {
                if (b.ConnectedAtom.Element.ToString() == element)
                    bonds.Add(b);
            }
            return bonds.ToArray<Bond>();
        }

        public Bond[] GetCompatibileBonds(Bond bondToCompare)
        {
            List<Bond> bonds = new List<Bond>();
            foreach (Bond b in m_Bonds)
                if (b.CompareTo(bondToCompare)) bonds.Add(b);
            return bonds.ToArray<Bond>();
        }


        [System.ComponentModel.TypeConverter(typeof(IntArrayTypeConverter))]
        public int[] PossibleValences
        {
            get
            {
                switch (this.Element)
                {
                    case ELEMENTS.B:
                        return new int[] { 3 };
                    case ELEMENTS.C:
                        return new int[] { 4 };
                    case ELEMENTS.N:
                        return new int[] { 3, 5 };
                    case ELEMENTS.O:
                        return new int[] { 2 };
                    case ELEMENTS.S:
                        return new int[] { 2, 4, 6 };
                    case ELEMENTS.P:
                        return new int[] { 3, 5 };
                    case ELEMENTS.F:
                    case ELEMENTS.Cl:
                    case ELEMENTS.I:
                    case ELEMENTS.Br:
                        return new int[] { 1 };
                    default:
                        return new int[0];
                }
            }
        }

        public int NumberOfNonHydrogens
        {
            get
            {
                int retVal = 0;
                foreach (Atom a in this.m_ConnectedAtoms)
                {
                    if (a.Element != ELEMENTS.H) retVal++;
                }
                return retVal;
            }
        }


        public int Charge
        {
            get
            {
                return m_Charge;
            }
            set
            {
                m_Charge = value;
            }
        }

        public int Valence
        {
            get
            {
                int[] possible = this.PossibleValences;
                if (possible.Length == 1) return possible[0];
                foreach (int i in possible)
                {
                    if (this.NumberOfBonds <= i) return i;
                }
                return 0;
            }
        }

        public Chirality Chirality
        {
            get
            {
                return this.m_Chiral;
            }
            set
            {
                m_Chiral = value;
            }
        }

        public WeiningerInvariant WeiningerInvariant { get; internal set; }

        public int WeiningerSymmetryClass { get; set; } = 0;
        public int WeiningerProductOfPrimes
        {
            get
            {
                int[] primes = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71 };
                int retVal = 1;
                foreach (Atom a in m_ConnectedAtoms)
                {
                    retVal = retVal * a.WeiningerRank;
                }
                return retVal;
            }
        }

        // Values from the Atom Table of a Mole File.
        //[System.ComponentModel.BrowsableAttribute(false)]
        //public double x { get; set; } = 0.0;
        //[System.ComponentModel.BrowsableAttribute(false)]
        //public double y { get; set; } = 0.0;
        //[System.ComponentModel.BrowsableAttribute(false)]
        //public double z { get; set; } = 0.0;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal int MassDiff { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal int StereoParity { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal int HydrogenCount { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal int StereoCareBox { get; set; } = 0;
        //public int HO;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal string RNotUsed { get; set; } = string.Empty;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal string INotUsed { get; set; } = string.Empty;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal int AtomMapping { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal int InversionRetension { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal int ExactChange { get; set; } = 0;
    }
}