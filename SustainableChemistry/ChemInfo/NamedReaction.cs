using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{

    public enum AcidBase
    {
        NONE = 0,
        ACID = 1,
        BASE = 2,
        ACID_BASE = 3
    }

       public static class Reactants
    {
		public static string[] ReactantList
		{
			get
			{
				return new string[]
					{
						"1,3 - dibromopropane",
						"1,3 - dichloropropane",
						"1,3 Propanedithiol",
						"2 Alkenes",
						"2 Alcohol",
						"2 Azides",
						"2 Esters",
						"2 Sulfhydryl groups",
						"3 - Bromo - 1 - butene",
						"3 - bromo - 1 - propanol",
						"3 Formaldehyde",
						"ACETAL",
						"Acetone",
						"Acid Chloride",
						"Acid Halides",
						"ACYLOIN",
						"ALCOHOL",
						"ALCOHOL, ALLYLIC",
						"ALDEHYDE",
						"aliphatic amines",
						"Alkaline hydrogen peroxide",
						"alkali Metal salt thiol",
						"ALKANE",
						"ALKENE",
						"ALKENE - ALCOHOL",
						"ALKENE - ALDEHYDE",
						"ALKENE - ALKYNE",
						"ALKENE - AMINE",
						"ALKENE - CARBOXYLIC ACID",
						"ALKENE - KETONE",
						"ALKENE - THIOETHER",
						"Alkenoic acid",
						"Alkyl Diaol",
                        "Alkyl bromide",
                        "Alkyl Halide",
                        "ALKYNE",
                        "ALKYNE - ALCOHOL",
                        "ALLENE",
                        "ALLOPHANATE",
                        "Allyl Acetate",
                        "Alpha Haloester",
                        "Alpha Ketoacid",
                        "AMIDE",
                        "AMIDE - ESTER",
                        "AMIDINE",
                        "AMINE",
                        "AMINE OXIDE",
                        "AMINO - ACID",
                        "AMINO - ALCOHOL",
                        "AMINO - ALDEHYDE",
                        "AMINO - ESTER",
                        "AMINO - ETHER",
                        "AMINO - KETONE",
                        "AMINO - NITRILE",
                        "AMINO - THIOETHER",
                        "AMINO - THIOL",
                        "ammonia",
                        "Ammonium carbamate",
                        "ANHYDRIDES",
                        "Aniline",
                        "ARENES",
                        "AROMATIC",
                        "Aromatic aldehyde",
                        "Aromatic Amine",
                        "aromatic diazonium",
                        "Aryl halides",
                        "AZIDE",
                        "AZIDO - ALCOHOL",
                        "AZIDO - AMINE",
                        "AZIRIDINE",
                        "AZIRINE",
                        "AZO COMPOUND",
                        "azodicarboxylate",
                        "AZOXY COMPOUND",
                        "B - Keto ester",
                        "benzylimidates",
                        "BIARYL",
                        "BISAMIDE",
                        "BORANE",
                        "BORONIC ACID",
                        "Br2",
                        "BUNTE SALT",
                        "Calcium Cyanamide",
                        "CARBAMATES",
                        "Carbenes",
                        "Carbon dioxide",
                        "Carbon monoxide",
                        "carbon disulfide",
                        "CARBONATE",
                        "Carboxyl",
                        "CARBOXYLIC ACID",
                        "CARBOXYLIC ESTER",
                        "CBr4",
                        "Chloramine",
                        "chlorophosphate",
                        "COUMARIN",
                        "CUMULENE",
                        "CYANAMIDE",
                        "Cyanide",
                        "CYANIDES, ACYL",
                        "CYANO IMINE",
                        "CYANOHYDRIN",
                        "CYCLOBUTANE",
                        "Cyclobutanol",
                        "CYCLOBUTENE",
                        "Cyclopentene",
                        "CYCLOPROPANE",
                        "DIALDEHYDE",
                        "Dialkylsulfates",
                        "DIAMINE",
                        "DIAZO COMPOUND",
                        "DIAZONIUM COMPOUND",
                        "DICARBOXYLIC ACID",
                        "DIENE",
                        "DIESTER",
                        "DIHALIDE",
                        "DIHALIDES(GEMINAL)",
                        "DIKETONE",
                        "DIOL",
                        "DISULFIDE",
                        "DITHIOACETAL",
                        "DITHIOCARBAMATE",
                        "DITHIOKETAL",
                        "DITHIOL",
                        "DIYNE",
                        "Emamtopure 1 sulfinamides",
                        "ENAMIDE",
                        "ENAMINE",
                        "ENOL",
                        "ENOL ETHER",
                        "Enolates",
                        "EPISULFIDE",
                        "Epoxide",
                        "EPOXY - ALCOHOL",
                        "EPOXY - AMIDE",
                        "EPOXY - CARBOXYLIC ACID",
                        "EPOXY - KETONE",
                        "EPOXY - NITRILE",
                        "ESTER",
                        "ESTER - AMIDE",
                        "ESTER - SULFIDE",
                        "Ethenol",
                        "ETHER",
                        "ETHER, SILYL",
                        "ETHER - AMINE",
                        "ETHER - ESTER",
                        "ETHER - KETONE",
                        "Ethyl formate",
                        "Ethylene Carbonate",
                        "Formaldhyde",
                        "FORMAMIDE",
                        "Glutamate",
                        "GLYCIDIC ESTER",
                        "HALIDE, ACYL",
                        "HALIDE, ALKYL",
                        "HALIDE, ALLYLIC",
                        "HALIDE, SULFONYL",
                        "HALO - ALDEHYDE",
                        "HALO - ALKYNE",
                        "HALO - AMIDE",
                        "HALO - AMINE",
                        "HALO - AZIDE",
                        "HALO - CARBOXYLIC ACID",
                        "HALO - ETHER",
                        "HALOHYDRIN",
                        "HALO - KETONE",
                        "HALO - LACTAM",
                        "HALO - LACTONE",
                        "HALO - NITRO",
                        "HALO - NITROSO",
                        "HALO - SILANE",
                        "HALO - SULFONE",
                        "HALO - SULFOXIDE",
                        "HETEROCYCLE",
                        "HYDRAZIDE",
                        "HYDRAZINE",
                        "HYDRAZONE",
                        "Hydrogen",
                        "Hydrogen chloride",
                        "Hydrogen Cyanide",
                        "Hydrogen Peroxide",
                        "Hydrogen Sulfide",
                        "HYDROXAMIC ACID",
                        "HYDROXY - AMIDE",
                        "HYDROXY - AMINE",
                        "HYDROXY - AZIRIDINE",
                        "HYDROXY - CARBOXYLIC ACID",
                        "HYDROXY - ESTER",
                        "HYDROXY - KETONE",
                        "HYDROXYLAMINE",
                        "HYDROXY - NITRO",
                        "HYDROXY - PHOSPHONATE",
                        "HYDROXY - THIOCYANATE",
                        "HYDROXY - THIOETHER",
                        "IMIDE",
                        "IMINE",
                        "IMINO ESTER",
                        "ISOCYANATE",
                        "ISONITRILE",
                        "ISOTHIOCYANATE",
                        "KETENE",
                        "KETENIMINE",
                        "KETO - ALDEHYDE",
                        "KETO - AMIDE",
                        "KETO - CARBOXYLIC ACID",
                        "KETO - ESTER",
                        "KETONE",
                        "KETO - SULFONE",
                        "LACTAM",
                        "LACTONE",
                        "Methanol",
                        "Methylfomate",
                        "NITRATE",
                        "NITRILE",
                        "NITRITE",
                        "NITRO",
                        "NITRO - ALCOHOL",
                        "Nitroalkanes",
                        "Nitrobenzene",
                        "NITRONE",
                        "NITROSO - AMINE",
                        "Nitrous Acid",
                        "ORTHO ESTER",
                        "OSAZONE",
                        "OXIME",
                        "Oxazolones",
                        "PEROXIDE",
                        "Phenol",
                        "Phenylboronic acid",
                        "PHOSPHINE",
                        "Phosphine Oxide",
                        "PHOSPHONATE ESTER",
                        "Phosphonic Acid",
                        "PHOSPHORANE",
                        "Potassium Thiocyanate",
                        "Primary Alcohol",
                        "Primary amines",
                        "Propynyl bromide",
                        "Propyne",
                        "QUINONE",
                        "RSO2I",
                        "RX",
                        "Secondary Amine",
                        "SELENIDE",
                        "SELENOCARBONATE",
                        "SELENOETHER - ALDEHYDE",
                        "SELENOETHER - KETONE",
                        "SELENOXIDE",
                        "SILANE",
                        "Silyl chloride",
                        "Silyl Enol Ethers",
                        "Sodium",
                        "Sodium Azide",
                        "Sodium thiosulfate",
                        "Sulfides",
                        "SULFINIC ACID",
                        "SULFONAMIDE",
                        "SULFONATE ESTER",
                        "SULFONE",
                        "Sulfonyl chloride",
                        "SULFONYL IMINE",
                        "SULFOXIDE",
                        "sulfur",
                        "TELLURIDE",
                        "Tertiary Amine",
                        "THIIRANE(EPISULFIDE)",
                        "THIOCARBAMATE",
                        "THIOCARBOXYLIC ACID",
                        "Thiocyanate",
                        "THIOESTER",
                        "THIOETHER(SULFIDE)",
                        "THIOETHER - ALDEHYDE",
                        "THIOETHER - KETONE",
                        "THIOKETONE",
                        "THIOL",
                        "THIOLACTAM",
                        "THIOUREA",
                        "Toulene",
                        "TRIAZOLE",
                        "TRIAZOLINE",
                        "TRIHALIDE",
                        "Trihaloethanol",
                        "TRIOXANE",
                        "UREA",
                        "urethane",
                        "vinyl azides",
                        "VINYL ETHER",
                        "Vinyl Ketone",
                        "Vinyl Halides",
                        "VINYL PHOSPHINE",
                        "VINYL SILANE",
                        "VINYL SULFIDE",
                        "VINYL SULFONE",
                        "Vinyl triflates",
                        "Water",
                        "water + CO2",
                        "X",
                        "X2",
                        "XANTHATE"
                    };
            }
		}
	}

    [Serializable]
    public class NamedReaction
    {
        References m_refList;
        List<System.Drawing.Image> m_RxnImage;
        [NonSerialized] List<string> m_ByProducts;
        SOLVENT m_Solvent;
        AcidBase m_AcidBase;

        public NamedReaction(string line, System.Data.DataRow row)
        {
            string[] parts = line.Split('\t');
            Name = parts[1];
            row["Name"] = Name;
            FunctionalGroup = parts[0];
            row["FunctionalGroup"] = Name;
            m_refList = new References();
            m_RxnImage = new List<System.Drawing.Image>();
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\USEPA\\SustainableChemistry\\" + parts[0] + "\\" + parts[1];
            if (System.IO.Directory.Exists(directory))
            {
                string[] imageFiles = System.IO.Directory.GetFiles(directory, "*.jpg");
                foreach (string file in imageFiles)
                    m_RxnImage.Add(System.Drawing.Image.FromFile(file));
                string[] references = System.IO.Directory.GetFiles(directory, "*.ris");
                foreach (string file in references)
                    m_refList.Add(new Reference(FunctionalGroup, Name, System.IO.File.ReadAllText(file)));
            }
            //row["Image"] = m_RxnImage;
            //Reactants = reactants;
            URL = parts[2];
            row["URL"] = URL;
            ReactantA = parts[3];
            row["ReactantA"] = ReactantA;
            ReactantB = parts[4];
            row["ReactantB"] = ReactantB;
            ReactantC = parts[5];
            row["ReactantC"] = ReactantC;
            Product = parts[10];
            row["Product"] = Product;
            Heat = parts[7];
            row["Heat"] = Heat;
            this.SetAcidBase(parts[6]);
            row["AcidBase"] = AcidBase;
            Catalyst = parts[8];
            row["Catalyst"] = Catalyst;
            this.SetSolvent(parts[9]);
            row["Solvent"] = Solvent;
            m_ByProducts = new List<string>();
            m_ByProducts.Add(parts[11]);
            row["ByProducts"] = parts[11];
        }

        public NamedReaction(FunctionalGroup functGroup, string name, string url, string reactA, string reactB, string reactC, string product, string acidBase, string heat, string catalyst, string solvent, string[] byPrduct, System.Data.DataRow row)
        {
            Name = name;
            FunctionalGroup = functGroup.Name;
            m_refList = new References();
            m_RxnImage = new List<System.Drawing.Image>();
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\USEPA\\SustainableChemistry\\" + functGroup + "\\" + Name;
            if (functGroup.Name == "PHOSPHONATE ESTER")
                directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\USEPA\\SustainableChemistry\\" + functGroup + "\\" + Name;
            if (System.IO.Directory.Exists(directory))
            {
                string[] imageFiles = System.IO.Directory.GetFiles(directory, "*.jpg");
                foreach (string file in imageFiles)
                    m_RxnImage.Add(System.Drawing.Image.FromFile(file));
                string[] references = System.IO.Directory.GetFiles(directory, "*.ris");
                foreach (string file in references)
                    m_refList.Add(new Reference(functGroup.Name, name, System.IO.File.ReadAllText(file)));
            }
            //Reactants = reactants;
            URL = url;
            ReactantA = reactA;
            ReactantB = reactB;
            ReactantC = reactC;
            Product = product;
            Heat = heat;
            this.SetAcidBase(acidBase);
            Catalyst = catalyst;
            this.SetAcidBase(catalyst);
            this.SetSolvent(solvent);
            m_ByProducts = new List<string>();
            //row["Image"] = m_RxnImage;
            //Reactants = reactants;
            row["Name"] = Name;
            row["FunctionalGroup"] = Name;
            row["Image"] = m_RxnImage;
            row["URL"] = URL;
            row["ReactantA"] = ReactantA;
            row["ReactantB"] = ReactantB;
            row["ReactantC"] = ReactantC;
            row["Product"] = Product;
            row["Heat"] = Heat;
            row["AcidBase"] = AcidBase;
            row["Catalyst"] = Catalyst;
            row["Solvent"] = Solvent;
            row["ByProducts"] = String.Empty;
        }

        public Reference GetReference(string value)
        {
            return this.m_refList[value];
        }

        public string FunctionalGroup { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        //public string[] Reactants { get; set; }
        public string ReactantA { get; set; }
        public string ReactantB { get; set; }
        public string ReactantC { get; set; }
        public string[] Reactants
        {
            get
            {
                List<string> temp = new List<string>();
                if (!string.IsNullOrEmpty(ReactantA)) temp.Add(ReactantA);
                if (!string.IsNullOrEmpty(ReactantB)) temp.Add(ReactantB);
                if (!string.IsNullOrEmpty(ReactantC)) temp.Add(ReactantC);
                return temp.ToArray<string>();
            }
            set
            {
                ReactantA = string.Empty;
                ReactantB = string.Empty;
                ReactantC = string.Empty;
                if (value.Length > 1) ReactantA = value[0];
                if (value.Length > 2) ReactantB = value[1];
                if (value.Length > 3) ReactantC = value[2];
            }
        }
        //public string AcidBase { get; set; }
        public string Heat { get; set; }
        public string Catalyst { get; set; }
        public string Solvent {
            get
            {
                return m_Solvent.ToString();
            }
            set
            {
                m_Solvent = (SOLVENT)Enum.Parse(typeof(SOLVENT), value);
            }
        }
        public string Product { get; set; }
        public string[] ByProducts
        {
            get
            {
                return m_ByProducts.ToArray<string>();
            }
            set
            {
                m_ByProducts.Clear();
                m_ByProducts.AddRange(value);
            }
        }
        public string AcidBase {
            get
            {
                if (m_AcidBase == ChemInfo.AcidBase.ACID) return "acid";
                if (m_AcidBase == ChemInfo.AcidBase.BASE) return "base";
                if (m_AcidBase == ChemInfo.AcidBase.BASE) return "base/heat";
                if (m_AcidBase == ChemInfo.AcidBase.ACID_BASE) return "base/acid";
                return "N/A";
            }
            set
            {
                this.SetAcidBase(value);
            }
        }

        public System.Drawing.Image[] ReactionImage
        {
            get
            {
                return m_RxnImage.ToArray<System.Drawing.Image>();
            }
        }

        public References References
        {
            get
            {
                return m_refList;
            }
        }

        public void SetAcidBase(string acidBase)
        {
            m_AcidBase = ChemInfo.AcidBase.NONE;
            if (acidBase.ToLower() == "acid") m_AcidBase = ChemInfo.AcidBase.ACID;
            if (acidBase.ToLower() == "base") m_AcidBase = ChemInfo.AcidBase.BASE;
            if (acidBase.ToLower() == "base/heat") m_AcidBase = ChemInfo.AcidBase.BASE;
            if (acidBase.ToLower() == "base/acid") m_AcidBase = ChemInfo.AcidBase.ACID_BASE;
        }

        public AcidBase GetAcidBase()
        {
            return m_AcidBase;
        }

        public void SetAcidBase(AcidBase acidBase)
        {
            m_AcidBase = acidBase;
        }

        void SetSolvent(String solvent)
        {
            m_Solvent = SOLVENT.NONE;
            if (solvent.ToLower() == "aceton") m_Solvent = SOLVENT.ACETONE;
            if (solvent.ToLower() == "acetonitrile ") m_Solvent = SOLVENT.ACETONITRILE;
            if (solvent.ToLower() == "aqueous ammonia/ the treated with lead nitrate") m_Solvent = SOLVENT.AQUEOUS_AMMONIA;
            if (solvent.ToLower() == "benzoic acid /toluene") m_Solvent = SOLVENT.BENZOIC_ACID_TOLUENE;
            if (solvent.ToLower() == "dcm") m_Solvent = SOLVENT.DCM;
            if (solvent.ToLower() == "dmc") m_Solvent = SOLVENT.DMC;
            if (solvent.ToLower() == "dmf") m_Solvent = SOLVENT.DMF;
            if (solvent.ToLower() == "dmso") m_Solvent = SOLVENT.DMSO;
            if (solvent.ToLower() == "ethanol") m_Solvent = SOLVENT.ETHANOL;
            if (solvent.ToLower() == "halo ketones") m_Solvent = SOLVENT.HALO_KETONE;
            if (solvent.ToLower() == "methanol") m_Solvent = SOLVENT.METHANOL;
            if (solvent.ToLower() == "methanol/ triethylamine") m_Solvent = SOLVENT.METHANOL_TRIETHYLAMINE;
            if (solvent.ToLower() == "nitrene") m_Solvent = SOLVENT.NITRENE;
            if (solvent.ToLower() == "nitrites") m_Solvent = SOLVENT.NITRITES;
            if (solvent.ToLower() == "thf") m_Solvent = SOLVENT.THF;
            if (solvent.ToLower() == "toluene") m_Solvent = SOLVENT.TOLUENE;
            if (solvent.ToLower() == "water") m_Solvent = SOLVENT.WATER;
        }
    }
}