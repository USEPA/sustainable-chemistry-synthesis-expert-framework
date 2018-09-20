using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    class DSSToxChemicals
    {
        ChemInfo.FunctionalGroupCollection m_FunctionalGroups;

        public int ToxCast_chid { get; set; }
        public string DSSTox_Substance_Id { get; set; }
        public string DSSTox_Structure_Id { get; set; }
        public string DSSTox_QC_Level { get; set; }
        public string Substance_Name { get; set; }
        public string Substance_CASRN { get; set; }
        public string Substance_Type { get; set; }
        public string Substance_Note { get; set; }
        public string Structure_SMILES { get; set; }
        public string Structure_InChI { get; set; }
        public string Structure_InChIKey { get; set; }
        public string Structure_Formula { get; set; }
        public string Structure_MolWt { get; set; }
        public string[] FunctionalGroups { get; set; }

        public DSSToxChemicals()
        {
            m_FunctionalGroups = new ChemInfo.FunctionalGroupCollection();
        }

        public DSSToxChemicals(string newChem)
        {
            string[] parts = newChem.Split('\t');
            float id = 0;
            float.TryParse(parts[0], out id);
            ToxCast_chid = (int)id;
            DSSTox_Substance_Id = parts[1];
            DSSTox_Structure_Id = parts[2];
            DSSTox_QC_Level = parts[3];
            Substance_Name = parts[4];
            Substance_CASRN = parts[5];
            Substance_Type = parts[6];
            Substance_Note = parts[7];
            if (parts.Length > 8) Structure_SMILES = parts[8];
            if (parts.Length > 9) Structure_InChI = parts[9];
            if (parts.Length > 10) Structure_InChIKey = parts[10];
            if (parts.Length > 11) Structure_Formula = parts[11];
            if (parts.Length > 12) Structure_MolWt = parts[12];
            m_FunctionalGroups = new ChemInfo.FunctionalGroupCollection();
        }

        public void AddFunctionalGroups(ChemInfo.FunctionalGroup[] groups)
        {
            m_FunctionalGroups.AddRange(groups);
            List<string> retVal = new List<string>();
            foreach (ChemInfo.FunctionalGroup g in m_FunctionalGroups) retVal.Add(g.Name);
            this.FunctionalGroups = retVal.ToArray<string>();

        }
    }
}
