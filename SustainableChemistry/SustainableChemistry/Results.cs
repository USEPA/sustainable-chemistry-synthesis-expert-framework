using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    [Serializable]
    [Newtonsoft.Json.JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)]
    internal class Results
    {
        List<FunctionalGroup> groups;
        public Results(string smiles, System.Data.DataTable fGroups, System.Data.DataTable namedReactions, System.Data.DataTable reactants, System.Data.DataTable rxnReactants, System.Data.DataTable byProducts, System.Data.DataTable references)
        {
            Molecule = new ChemInfo.Molecule(smiles);
            groups = new List<FunctionalGroup>();
            this.AddFunctionalGroups(smiles, fGroups, namedReactions, reactants, rxnReactants, byProducts, references);
        }

        [Newtonsoft.Json.JsonProperty]
        internal ChemInfo.Molecule Molecule { get; private set; }
        [Newtonsoft.Json.JsonProperty]
        internal FunctionalGroup[] FunctionalGroups
        {
            get
            {
                return groups.ToArray<FunctionalGroup>();
            }
        }

        private void AddFunctionalGroups(string smiles, System.Data.DataTable fGroups, System.Data.DataTable namedReactions, System.Data.DataTable reactants, System.Data.DataTable rxnReactants, System.Data.DataTable byProducts, System.Data.DataTable references)
        {
            foreach (System.Data.DataRow dr in fGroups.Rows)
            {
                string smarts = dr["Smarts"].ToString();
                if (this.Molecule.FindFunctionalGroup(dr))
                {
                    groups.Add(new FunctionalGroup(dr, namedReactions, reactants, rxnReactants, byProducts, references));
                }
            }
        }
    }

    [Serializable]
    [Newtonsoft.Json.JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)]
    internal class FunctionalGroup
    {
        List<ReactionOutput> m_Reactions;


        public FunctionalGroup(System.Data.DataRow fGroup, System.Data.DataTable namedReactions, System.Data.DataTable reactants, System.Data.DataTable rxnReactants, System.Data.DataTable byProducts, System.Data.DataTable references)
        {
            Name = fGroup["Name"].ToString();
            Smarts = fGroup["Smarts"].ToString();
            URL = string.Empty;
            m_Reactions = new List<ReactionOutput>();
            var results = from myRow in namedReactions.AsEnumerable()
                          where myRow.Field<Int64>("Functional_Group_id") == Convert.ToInt64(fGroup["id"])
                          select myRow;
            foreach (System.Data.DataRow row in results)
            {
                m_Reactions.Add(new ReactionOutput(row, reactants, rxnReactants, byProducts, references));
            }
        }

        [Newtonsoft.Json.JsonProperty]
        public string Name { get; set; }
        [Newtonsoft.Json.JsonProperty]
        public string Smarts { get; set; }
        public string URL { get; set; }
        [Newtonsoft.Json.JsonProperty]
        internal ReactionOutput[] Reactions
        {
            get
            {
                return m_Reactions.ToArray<ReactionOutput>();
            }
        }
    }

    [Serializable]
    [Newtonsoft.Json.JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)]
    internal class ReactionOutput
    {
        public ReactionOutput(System.Data.DataRow reaction, System.Data.DataTable reactants, System.Data.DataTable rxnReactants, System.Data.DataTable byProducts, System.Data.DataTable references)
        {
            Name = reaction["Name"].ToString();
            Int64 id = Convert.ToInt64(reaction["id"]);
            Int64 groupId = Convert.ToInt64(reaction["Functional_Group_id"]);
            URL = reaction["URL"].ToString();
            Product = reaction["Product"].ToString();
            //ByProducts = reaction["ByProducts"].ToString();
            //Reactants = reaction["Reactants"].ToString();
            Catalyst = reaction["Catalyst"].ToString();
            Solvent = reaction["Solvent"].ToString();
            AcidBase = reaction["AcidBase"].ToString();
            List<string> m_Reactants = new List<string>();
            var results = from myRow in rxnReactants.AsEnumerable()
                          where myRow.Field<Int64>("namedreaction_id") == Convert.ToInt64(id)
                          select myRow;
            foreach (System.Data.DataRow row in results)
            {
                m_Reactants.Add(reactants.Rows[Convert.ToInt32(row["reactant_id"])].ToString());
            }
            Reactants = m_Reactants.ToArray<string>();
            List<string> m_ByProducts = new List<string>();
            results = from myRow in byProducts.AsEnumerable()
                          where myRow.Field<Int64>("namedreaction_id") == Convert.ToInt64(id)
                          select myRow;
            foreach (System.Data.DataRow row in results)
            {
                m_ByProducts.Add(reactants.Rows[Convert.ToInt32(row["reactant_id"])].ToString());
            }
            ByProducts = m_ByProducts.ToArray<string>();
            List<Reference> m_References = new List<Reference>();
            results = from myRow in references.AsEnumerable()
                      where myRow.Field<Int64>("Reaction_id") == id
                      && myRow.Field<Int64>("Functional_Group_id")  == groupId
                      select myRow;
            foreach (System.Data.DataRow row in results)
            {
                m_References.Add(new Reference(groupId, id, row["RISData"].ToString()));
            }
            References = m_References.ToArray<Reference>();
        }

        [Newtonsoft.Json.JsonProperty]
        public string Name { get; private set; }
        [Newtonsoft.Json.JsonProperty]
        public string[] Reactants { get; private set; }
        [Newtonsoft.Json.JsonProperty]
        public string URL { get; private set; }
        [Newtonsoft.Json.JsonProperty]
        public string Product { get; private set; }
        [Newtonsoft.Json.JsonProperty]
        public string[] ByProducts { get; private set; }
        [Newtonsoft.Json.JsonProperty]
        public string Catalyst { get; private set; }
        [Newtonsoft.Json.JsonProperty]
        public string Solvent { get; private set; }
        [Newtonsoft.Json.JsonProperty]
        public string AcidBase { get; private set; }
        [Newtonsoft.Json.JsonProperty]
        public Reference[] References { get; private set; }
    }
}
