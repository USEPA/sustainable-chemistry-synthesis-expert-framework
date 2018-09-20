using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    [Serializable]
    public class Results
    {
        FunctionalGroupOutput groupOutput;

        public Results(ChemInfo.FunctionalGroup functionalGroup, ChemInfo.References references)
        {
            groupOutput = new FunctionalGroupOutput(functionalGroup);
            References = references.GetReferences(functionalGroup.Name);
        }

        public FunctionalGroupOutput FunctionalGroup;
        public ChemInfo.Reference[] References { get; } 
    }

    public class FunctionalGroupOutput
    {
        [NonSerialized]List<ReactionOutput> m_Reactions;

        public FunctionalGroupOutput(ChemInfo.FunctionalGroup group)
        {
            Name = group.Name;
            Smarts = group.Smart;
            m_Reactions = new List<ReactionOutput>();
            foreach (ChemInfo.NamedReaction reaction in group.NamedReactions) m_Reactions.Add(new ReactionOutput(reaction)); 
        }

        public string Name { get; set; }
        public string Smarts { get; set; }
        public string URL { get; set; }
        public ReactionOutput[] Reactions
        {
            get
            {
                return m_Reactions.ToArray<ReactionOutput>();
            }
            set
            {
                m_Reactions.Clear();
                m_Reactions.AddRange(value);
            }
        }
    }

    public class ReactionOutput
    {
        //[NonSerialized] List<string> m_Reactants;
        //[NonSerialized] List<string> m_ByProducts;

        public ReactionOutput(ChemInfo.NamedReaction reaction)
        {
            Name = reaction.Name;
            URL = reaction.URL;
            Product = reaction.Product;
            ByProducts =  reaction.ByProducts;
            Reactants = reaction.Reactants;
            Catalyst = reaction.Catalyst;
            Solvent = reaction.Solvent;
            AcidBase = reaction.AcidBase;
        }

        public string Name { get; set; }
        public string[] Reactants { get; set; }
        public string URL { get; set; }
        public string Product { get; set; }
        public string[] ByProducts { get; set; }
        public string Catalyst { get; set; }
        public string Solvent { get; set; }
        public string AcidBase { get; set; }
    }
}
