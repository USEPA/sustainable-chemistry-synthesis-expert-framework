using System;

namespace SustainableChemistryWeb.ChemInfo
{
    [Flags]
    public enum ELEMENTS : byte
    {

        // The element will be the last four bits of the flag
        WILD_CARD = 0x00,
        H = 0x01,
        He = 0x02,
        Li = 0x03,
        Be = 0x04,
        B = 0x05,
        C = 0x06,
        N = 0x07,
        O = 0x08,
        F = 0x09,
        Ne = 0x0a,
        Na = 0x0b,
        Mg = 0x0c,
        Al = 0x0d,
        Si = 0x0e,
        P = 0x0f,
        S = 0x10,
        Cl = 0x11,
        Ar = 0x12,
        K = 0x13,
        Ca = 0x14,
        Sc = 0x15,
        Ti = 0x16,
        V = 0x17,
        Cr = 0x18,
        Mn = 0x19,
        Fe = 0x1a,
        Co = 0x1b,
        Ni = 0x1c,
        Cu = 0x1d,
        Zn = 0x1e,
        Ga = 0x1f,
        Ge = 0x20,
        As = 0x21,
        Se = 0x22,
        Br = 0x23,
        Kr = 0x24,
        Rb = 0x25,
        Sr = 0x26,
        Y = 0x27,
        Zr = 0x28,
        Nb = 0x29,
        Mo = 0x2a,
        Tc = 0x2b,
        Ru = 0x2c,
        Rh = 0x2d,
        Pd = 0x2e,
        Ag = 0x2f,
        Cd = 0x30,
        In = 0x31,
        Sn = 0x32,
        Sb = 0x33,
        Te = 0x34,
        I = 0x35,
        Xe = 0x36,
        Cs = 0x37,
        Ba = 0x38,
        La = 0x39,
        Ce = 0x3a,
        Pr = 0x3b,
        Nd = 0x3c,
        Pm = 0x3d,
        Sm = 0x3e,
        Eu = 0x3f,
        Gd = 0x40,
        Tb = 0x41,
        Dy = 0x42,
        Ho = 0x43,
        Er = 0x44,
        Tm = 0x45,
        Yb = 0x46,
        Lu = 0x47,
        Hf = 0x48,
        Ta = 0x49,
        W = 0x4a,
        Re = 0x4b,
        Os = 0x4c,
        Ir = 0x4d,
        Pt = 0x4e,
        Au = 0x4f,
        Hg = 0x50,
        Tl = 0x51,
        Pb = 0x52,
        Bi = 0x53,
        Po = 0x54,
        At = 0x55,
        Rn = 0x56,
        Fr = 0x57,
        Ra = 0x58,
        Ac = 0x59,
        Th = 0x5a,
        Pa = 0x5b,
        U = 0x5c,
        Np = 0x5d,
        Pu = 0x5e,
        Am = 0x5f,
        Cm = 0x60,
        Bk = 0x61,
        Cf = 0x62,
        Es = 0x63,
        Fm = 0x64,
        Md = 0x65,
        No = 0x66,
        Lr = 0x67,
        Rf = 0x68,
        Db = 0x69,
        Sg = 0x6a,
        Bh = 0x6b,
        Hs = 0x6c,
        Mt = 0x6d,
        Ds = 0x6e,
        Rg = 0x6f,
        Cn = 0x70,
        Nh = 0x71,
        Fl = 0x72,
        Mc = 0x73,
        Lv = 0x74,
        Ts = 0x75,
        Og = 0x76,
        Halogen = 0xFF
    };

    public static class Element
    {
        static ChemInfo.list elements;

        static Element()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ChemInfo.list));
            System.Xml.XmlReader reader = System.Xml.XmlReader.Create(new System.IO.StringReader(Properties.Resources.elementString));
            elements = (list)serializer.Deserialize(reader);
        }

        static public bool ValidateSymbol(string symbol)
        {
            if (symbol == "*") return true;
            for (int i = 1; i < 119; i++)
            {
                if (((ELEMENTS)i).ToString() == symbol) return true;
            }
            return false;
        }

        static public ELEMENTS GetElementForSymbol(string symbol)
        {
            if (symbol == "*") return ELEMENTS.WILD_CARD;
            for (int i = 1; i < 119; i++)
            {
                if (((ELEMENTS)i).ToString() == symbol) return (ELEMENTS)i;
            }
            return (ELEMENTS)0;
        }

        static public string Symbol(ELEMENTS e)
        {
            return e.ToString();
        }

        static public string Symbol(int AtomicNumber)
        {
            return ((ELEMENTS)AtomicNumber).ToString();
        }

        static public string Name(ELEMENTS e)
        {
            listAtom atom = (listAtom)elements.atom[(int)e];
            listAtomLabel a = (listAtomLabel)((listAtom)elements.atom[(int)e]).Items[2];
            return a.value;
        }

        static public string Name(int AtomicNumber)
        {
            listAtomLabel a = (listAtomLabel)((listAtom)elements.atom[AtomicNumber]).Items[2];
            return a.value;
        }

        static public string Name(string symbol)
        {
            return Element.Name(Element.GetElementForSymbol(symbol));
        }

        static public String LookupValue(ELEMENTS e, string dictRef)
        {
            listAtom atom = (listAtom)elements.atom[(int)e];
            foreach (object item in atom.Items)
            {
                if (item.GetType() == typeof(ChemInfo.listAtomScalar))
                {
                    listAtomScalar scalar = (listAtomScalar)item;
                    if (scalar.dictRef == dictRef)
                    {
                        return scalar.Value; ;
                    }
                }
                if (item.GetType() == typeof(ChemInfo.listAtomArray))
                {
                    listAtomArray array = (listAtomArray)item;
                    if (array.dictRef == dictRef)
                    {
                        return array.Value; ;
                    }
                }
            }
            listAtomLabel a = (listAtomLabel)((listAtom)elements.atom[(int)e]).Items[2];
            return "0";
        }

        static public double Mass(ELEMENTS e)
        {
            return Convert.ToDouble(Element.LookupValue(e, "bo:mass"));
        }

        static public double Mass(int AtomicNumber)
        {
            return Element.Mass((ELEMENTS)AtomicNumber);
        }

        static public double Mass(string symbol)
        {
            return Element.Mass(Element.GetElementForSymbol(symbol));
        }

        static public double ExactMass(ELEMENTS e)
        {
            return Convert.ToDouble(Element.LookupValue(e, "bo:exactMass"));
        }

        static public double ExactMass(int AtomicNumber)
        {
            return Element.Mass((ELEMENTS)AtomicNumber);
        }

        static public double ExactMass(string symbol)
        {
            return Element.Mass(Element.GetElementForSymbol(symbol));
        }

        static public int Period(ELEMENTS e)
        {
            return Convert.ToInt32(Element.LookupValue(e, "bo:period"));
        }

        static public int Period(int AtomicNumber)
        {
            return Element.Period((ELEMENTS)AtomicNumber);
        }

        static public int Period(string symbol)
        {
            return Element.Period(Element.GetElementForSymbol(symbol));
        }

        static public int Group(ELEMENTS e)
        {
            return Convert.ToInt32(Element.LookupValue(e, "bo:group"));
        }

        static public int Group(int AtomicNumber)
        {
            return Element.Period((ELEMENTS)AtomicNumber);
        }

        static public int Group(string symbol)
        {
            return Element.Period(Element.GetElementForSymbol(symbol));
        }

        static public string ElecctronConfiguration(ELEMENTS e)
        {
            return Element.LookupValue(e, "bo:electronicConfiguration");
        }

        static public string ElecctronConfiguration(int AtomicNumber)
        {
            return Element.ElecctronConfiguration((ELEMENTS)AtomicNumber);
        }

        static public string ElecctronConfiguration(string symbol)
        {
            return Element.ElecctronConfiguration(Element.GetElementForSymbol(symbol));
        }

        static public int[] ElementColor(ELEMENTS e)
        {
            string[] temp = LookupValue(e, "bo:elementColor").Split(' ');
            int[] retVal = new int[3];
            retVal[0] = (int)(Convert.ToDouble(temp[0]) * 255);
            retVal[1] = (int)(Convert.ToDouble(temp[1]) * 255);
            retVal[2] = (int)(Convert.ToDouble(temp[2]) * 255);
            return retVal;
        }

        static public int[] ElementColor(int AtomicNumber)
        {
            return Element.ElementColor((ELEMENTS)AtomicNumber);
        }

        static public int[] DefaultColor(string symbol)
        {
            return Element.ElementColor(Element.GetElementForSymbol(symbol));
        }

        static public double CovalentRadius(ELEMENTS e)
        {
            return Convert.ToDouble(LookupValue(e, "bo:radiusCovalent"));
        }

        static public double CovalentRadius(int AtomicNumber)
        {
            return Element.CovalentRadius((ELEMENTS)AtomicNumber);
        }

        static public double CovalentRadius(string symbol)
        {
            return Element.CovalentRadius(Element.GetElementForSymbol(symbol));
        }



        // <atom id = "H" >
        //  < scalar dataType="xsd:Integer" dictRef="bo:atomicNumber">1</scalar>
        //  <label dictRef = "bo:symbol" value="H" />
        //  <label dictRef = "bo:name" xml:lang="en" value="Hydrogen" />
        //  <scalar dataType = "xsd:float" dictRef="bo:mass" units="units:atmass">1.008</scalar>
        //  <scalar dataType = "xsd:float" dictRef="bo:exactMass" units="units:atmass">1.007825032</scalar>
        //  <scalar dataType = "xsd:float" dictRef="bo:ionization" units="units:ev">13.5984</scalar>
        //  <scalar dataType = "xsd:float" dictRef="bo:electronAffinity" units="units:ev" errorValue="3">0.75420375</scalar>
        //  <scalar dataType = "xsd:float" dictRef="bo:electronegativityPauling" units="boUnits:paulingScaleUnit">2.20</scalar>
        //  <scalar dataType = "xsd:string" dictRef="bo:nameOrigin" xml:lang="en">Greek 'hydro' and 'gennao' for 'forms water'</scalar>
        //  <scalar dataType = "xsd:float" dictRef="bo:radiusCovalent" units="units:ang">0.37</scalar>
        //  <scalar dataType = "xsd:float" dictRef="bo:radiusVDW" units="units:ang">1.2</scalar>
        //  <array title = "color" dictRef="bo:elementColor" size="3" dataType="xsd:float">1.00 1.00 1.00</array>
        //  <scalar dataType = "xsd:float" dictRef="bo:boilingpoint"  units="siUnits:kelvin">20.28</scalar>
        //  <scalar dataType = "xsd:float" dictRef="bo:meltingpoint"  units="siUnits:kelvin">14.01</scalar>
        //  <scalar dataType = "xsd:string" dictRef="bo:periodTableBlock">s</scalar>
        //  <array dataType = "xsd:string" dictRef="bo:discoveryCountry">uk</array>
        //  <scalar dataType = "xsd:date" dictRef="bo:discoveryDate">1766</scalar>
        //  <array dataType = "xsd:string" dictRef="bo:discoverers">C.Cavendish</array>
        //  <scalar dataType = "xsd:int" dictRef="bo:period">1</scalar>
        //  <scalar dataType = "xsd:int" dictRef="bo:group">1</scalar>
        //  <scalar dataType = "xsd:string" dictRef="bo:electronicConfiguration">1s1</scalar>
        //  <scalar dataType = "xsd:string" dictRef="bo:family">Non-Metal</scalar>
        //</atom>
    }




    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.xml-cml.org/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.xml-cml.org/schema", IsNullable = false)]
    public partial class list
    {

        private listMetadata[] metadataListField;

        private listAtom[] atomField;

        private string idField;

        private string conventionField;

        private string titleField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("metadata", IsNullable = false)]
        public listMetadata[] metadataList
        {
            get
            {
                return this.metadataListField;
            }
            set
            {
                this.metadataListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("atom")]
        public listAtom[] atom
        {
            get
            {
                return this.atomField;
            }
            set
            {
                this.atomField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string convention
        {
            get
            {
                return this.conventionField;
            }
            set
            {
                this.conventionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.xml-cml.org/schema")]
    public partial class listMetadata
    {

        private string nameField;

        private string contentField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.xml-cml.org/schema")]
    public partial class listAtom
    {

        private object[] itemsField;

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("array", typeof(listAtomArray))]
        [System.Xml.Serialization.XmlElementAttribute("label", typeof(listAtomLabel))]
        [System.Xml.Serialization.XmlElementAttribute("scalar", typeof(listAtomScalar))]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.xml-cml.org/schema")]
    public partial class listAtomArray
    {

        private string titleField;

        private string dictRefField;

        private byte sizeField;

        private bool sizeFieldSpecified;

        private string dataTypeField;

        private string delimiterField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dictRef
        {
            get
            {
                return this.dictRefField;
            }
            set
            {
                this.dictRefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sizeSpecified
        {
            get
            {
                return this.sizeFieldSpecified;
            }
            set
            {
                this.sizeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataType
        {
            get
            {
                return this.dataTypeField;
            }
            set
            {
                this.dataTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string delimiter
        {
            get
            {
                return this.delimiterField;
            }
            set
            {
                this.delimiterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.xml-cml.org/schema")]
    public partial class listAtomLabel
    {

        private string dictRefField;

        private string valueField;

        private string langField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dictRef
        {
            get
            {
                return this.dictRefField;
            }
            set
            {
                this.dictRefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.xml-cml.org/schema")]
    public partial class listAtomScalar
    {

        private string dataTypeField;

        private string dictRefField;

        private string unitsField;

        private byte errorValueField;

        private bool errorValueFieldSpecified;

        private string langField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataType
        {
            get
            {
                return this.dataTypeField;
            }
            set
            {
                this.dataTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dictRef
        {
            get
            {
                return this.dictRefField;
            }
            set
            {
                this.dictRefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string units
        {
            get
            {
                return this.unitsField;
            }
            set
            {
                this.unitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte errorValue
        {
            get
            {
                return this.errorValueField;
            }
            set
            {
                this.errorValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool errorValueSpecified
        {
            get
            {
                return this.errorValueFieldSpecified;
            }
            set
            {
                this.errorValueFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}
