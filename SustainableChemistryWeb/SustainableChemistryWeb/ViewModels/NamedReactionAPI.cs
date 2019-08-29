using System.Collections.Generic;

namespace SustainableChemistryWeb.ViewModels
{
    public class NamedReactionAPI
    {
        public NamedReactionAPI(Models.NamedReaction rxn)
        {
            AppNamedreactionByProducts = new HashSet<string>();
            AppNamedreactionReactants = new HashSet<string>();
            //AppReference = new HashSet<Reference>();

            Id = rxn.Id;
            Name = rxn.Name;
            Product = rxn.Product;
            Heat = rxn.Heat;
            SolventId = rxn.SolventId;
            Solvent = rxn.Solvent.Name;
            Catalyst = rxn.Catalyst.Name;
            CatalystId = rxn.CatalystId;
            FunctionalGroup = rxn.FunctionalGroup.Name;
            FunctionalGroupId = rxn.FunctionalGroupId;
            AcidBase = rxn.AcidBase;
            Url = rxn.Url;
            Image = rxn.Image;
            foreach (Models.NamedReactionReactants a in rxn.AppNamedreactionReactants)
            {
                AppNamedreactionReactants.Add(a.Reactant.Name);
            }
            foreach (Models.NamedReactionByProducts a in rxn.AppNamedreactionByProducts)
            {
                AppNamedreactionByProducts.Add(a.Reactant.Name);
            }
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Product { get; set; }
        public string Heat { get; set; }
        [System.ComponentModel.DisplayName("Acid/Base")]
        public string AcidBase { get; set; }
        public string Image { get; set; }
        public long CatalystId { get; set; }
        public long FunctionalGroupId { get; set; }
        public long SolventId { get; set; }
        public string Url { get; set; }

        public string Catalyst { get; set; }
        [System.ComponentModel.DisplayName("Functional Group")]
        public string FunctionalGroup { get; set; }
        public string Solvent { get; set; }
        public virtual ICollection<string> AppNamedreactionByProducts { get; set; }
        public virtual ICollection<string> AppNamedreactionReactants { get; set; }
    }
}
