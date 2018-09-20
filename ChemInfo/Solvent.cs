using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{
    public enum SOLVENT
    {
        NONE = 0,
        ACETONE = 1,
        ACETONITRILE = 2,
        AQUEOUS_AMMONIA = 3,
        BENZOIC_ACID_TOLUENE = 4,
        DCM = 5,
        DMC = 6,
        DMF = 7,
        DMSO = 8,
        ETHANOL = 9,
        HALO_KETONE = 10,
        METHANOL = 11,
        METHANOL_TRIETHYLAMINE = 12,
        NITRENE = 12,
        NITRITES = 13,
        THF = 14,
        TOLUENE = 15,
        WATER = 16
    }


    class Solvent
    {
        public Solvent(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
