using System.Collections.Generic;

namespace SustainableChemistryWeb.ViewModels
{
    public class FunctionalGroupIndexData
    {
        public IEnumerable<SustainableChemistryWeb.Models.FunctionalGroup> FunctionalGroups { get; set; }
        public IEnumerable<SustainableChemistryWeb.Models.NamedReaction> NamedReactions { get; set; }
        public IEnumerable<SustainableChemistryWeb.ViewModels.ReferenceViewModel> References { get; set; }
    }
}
