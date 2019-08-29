using System;

namespace SustainableChemistryWeb.ChemInfo
{
    public static class MoleFileReader
    {
        //string m_molefile;

        //public MoleFileReader(string filename)
        //{
        //    m_molefile = filename;
        //}

        static public Molecule ReadMoleFile(string filename)
        {
            using (var reader = new System.IO.StreamReader(filename))
            {
                string s = reader.ReadToEnd();
                if (s.Contains("V2000")) return MoleFileReader.ParseMoleFile2(s);
                if (s.Contains("V3000")) throw new Exception("Version 3000 Mol file is currently not supported.");
                throw new Exception("Mol file type not recognized. It is does not contain a version number.");
            }
        }

        //static Molecule ReadMoleFile(string filename)
        //{
        //    using (var reader = new System.IO.StreamReader(filename))
        //    {
        //        string s = reader.ReadToEnd();
        //        if (s.Contains("V2000")) return MoleFileReader.ParseMoleFile2(s);
        //        if (s.Contains("V3000")) throw new Exception("Version 3000 Mol file is currently not supported.");
        //        throw new Exception("Mol file type not recognized. It is does not contain a version number.");
        //    }
        //}

        static Molecule ParseMoleFile2(string moleFile)
        {
            Molecule molecule = new Molecule();
            //atoms.Clear();
            //cycles.Clear();
            //fusedRings.Clear();

            int numAtoms = 0;
            int numBonds = 0;

            // This information is contained in the header (first 3 lines) of the mol file. It is currently not being used, but code has been created for
            // future use.
            string name = string.Empty;
            string program = string.Empty;
            string user = string.Empty;
            //// used to test Line 2 reading below.
            //string MM = string.Empty;
            //string DD = string.Empty;
            //string YY = string.Empty;
            //string HH = string.Empty;
            //string mm = string.Empty;
            int MM = 1;
            int DD = 1;
            int YY = 0;
            int HH = 0;
            int mm = 0;
            string dimensionalCodes = string.Empty;
            //// used to test Line 2 reading below.
            //string scalingFactor1 = string.Empty;
            int scalingFactor1 = 0;
            //// used to test Line 2 reading below.
            //string scalingFactor2 = string.Empty;
            double scalingFactor2 = 0;
            // used to test Line 2 reading below.
            string energy = string.Empty;
            string registryNumber = string.Empty;
            string comments = string.Empty;

            string[] lines = moleFile.Split('\n');
            // Line 1 contains the compound name. It can not be longer than 80 characters, and is allowed to be empty.
            // The length of the line is not relevant in how this is read, so it is not checked.
            name = lines[0];
            //// used to test Line 2 reading below.
            //lines[1] = "IIPPPPPPPPMMDDYYHHmmddSSssssssssssEEEEEEEEEEEERRRRRR";
            // Line 2 is optional. Skip this if it is not there.
            if (lines[1] != string.Empty)
            {
                // Line format: IIPPPPPPPPMMDDYYHHmmddSSssssssssssEEEEEEEEEEEERRRRRR
                //              A2<--A8--><---A10-->A2I2<--F10.5-><---F12.5--><-I6->
                //User's first and last initials (l), program name (P), date/time (M/D/Y,H:m),
                //dimensional codes (d), scaling factors (S, s), energy (E) if modeling program input,
                //internal registry number (R) if input through MDL form.
                if (lines[1].Length > 2) user = lines[1].Substring(0, 2); // II
                if (lines[1].Length > 10) program = lines[1].Substring(2, 8); // PPPPPPPP
                if (lines[1].Length > 20)
                {
                    //// used to test Line 2 reading below.
                    //MM = lines[1].Substring(10, 2); // MMDDYYHHmm
                    //DD = lines[1].Substring(12, 2); // MMDDYYHHmm
                    //YY = lines[1].Substring(14, 2); // MMDDYYHHmm
                    //HH = lines[1].Substring(16, 2); // MMDDYYHHmm
                    //mm = lines[1].Substring(18, 2); // MMDDYYHHmm
                    MM = Convert.ToInt32(lines[1].Substring(10, 2)); // MMDDYYHHmm
                    DD = Convert.ToInt32(lines[1].Substring(12, 2)); // MMDDYYHHmm
                    YY = Convert.ToInt32(lines[1].Substring(14, 2)); // MMDDYYHHmm
                    HH = Convert.ToInt32(lines[1].Substring(16, 2)); // MMDDYYHHmm
                    mm = Convert.ToInt32(lines[1].Substring(18, 2)); // MMDDYYHHmm
                }
                if (lines[1].Length > 22) dimensionalCodes = lines[1].Substring(20, 2); // dd
                                                                                        //// used to test Line 2 reading below.
                                                                                        // if (lines[1].Length > 24) scalingFactor1 = lines[1].Substring(22, 2); //SS
                if (lines[1].Length > 24) scalingFactor1 = Convert.ToInt32(lines[1].Substring(22, 2)); //SS
                                                                                                       //// used to test Line 2 reading below.
                                                                                                       // if (lines[1].Length > 34) scalingFactor2 = lines[1].Substring(24, 10); //ss
                if (lines[1].Length > 34) scalingFactor2 = Convert.ToDouble(lines[1].Substring(24, 10)); //ss
                if (lines[1].Length > 46) energy = lines[1].Substring(34, 12); //EEEEEEEEEEEE
                if (lines[1].Length == 52) registryNumber = lines[1].Substring(46, 6); //RRRRRR
            }
            comments = lines[2];

            // Counts Line
            // aaabbblllfffcccsssxxxrrrpppiiimmmvvvvvv
            numAtoms = Convert.ToInt32(lines[3].Substring(0, 3));
            numBonds = Convert.ToInt32(lines[3].Substring(3, 3));
            int atomLists = Convert.ToInt32(lines[3].Substring(6, 3));
            //int fObsolete = Convert.ToInt32(lines[3].Substring(9, 3));
            bool chiral = false;
            if (Convert.ToInt32(lines[3].Substring(12, 3)) == 1) chiral = true;
            int sText = Convert.ToInt32(lines[3].Substring(15, 3));
            //int xObsolete = Convert.ToInt32(lines[3].Substring(18, 3));
            //int rObsolete = Convert.ToInt32(lines[3].Substring(21, 3));
            //int pObsolete = Convert.ToInt32(lines[3].Substring(24, 3));
            //int iObsolete = Convert.ToInt32(lines[3].Substring(27, 3));
            int properties = Convert.ToInt32(lines[3].Substring(30, 3));
            string version = lines[3].Substring(33, 6);
            for (int i = 0; i < numAtoms; i++)
            {
                Atom a = new ChemInfo.Atom(lines[4 + i].Substring(31, 3).Trim());
                molecule.AddAtom(a);
                // xxxxx.xxxxyyyyy.yyyyzzzzz.zzzz aaaddcccssshhhbbbvvvHHHrrriiimmmnnneee
                //a.x = Convert.ToDouble(lines[4 + i].Substring(0, 10));
                //a.y = Convert.ToDouble(lines[4 + i].Substring(10, 10));
                //a.z = Convert.ToDouble(lines[4 + i].Substring(20, 10));
                string text = lines[4 + i].Substring(34, 2);
                a.MassDiff = Convert.ToInt32(lines[4 + i].Substring(34, 2));
                a.Charge = Convert.ToInt32(lines[4 + i].Substring(36, 3));
                a.StereoParity = Convert.ToInt32(lines[4 + i].Substring(39, 3));
                a.HydrogenCount = Convert.ToInt32(lines[4 + i].Substring(42, 3));
                a.StereoCareBox = Convert.ToInt32(lines[4 + i].Substring(45, 3));
                //a.Valence = Convert.ToInt32(lines[4 + i].Substring(48, 3));
                // string H0 = lines[4 + i].Substring(51, 3);
                // a.HO = Convert.ToInt32(lines[4 + i].Substring(51, 3));
                a.RNotUsed = lines[4 + i].Substring(54, 3);
                a.INotUsed = lines[4 + i].Substring(57, 3);
                a.AtomMapping = Convert.ToInt32(lines[4 + i].Substring(60, 3));
                a.InversionRetension = Convert.ToInt32(lines[4 + i].Substring(63, 3));
                a.ExactChange = Convert.ToInt32(lines[4 + i].Substring(66, 3));
            }
            for (int i = 0; i < numBonds; i++)
            {
                //Bond b = new Bond();
                // 111222tttsssxxxrrrccc
                string line = lines[4 + numAtoms + i];
                int firstAtom = Convert.ToInt32(lines[4 + numAtoms + i].Substring(0, 3));
                int secondAtom = Convert.ToInt32(lines[4 + numAtoms + i].Substring(3, 3));
                BondType bondType = (BondType)Convert.ToInt32(lines[4 + numAtoms + i].Substring(6, 3));
                BondStereo bondStereo = (BondStereo)Convert.ToInt32(lines[4 + numAtoms + i].Substring(9, 3));
                string xNotUsed = lines[4 + numAtoms + i].Substring(12, 3);
                BondTopology bondTopology = (BondTopology)Convert.ToInt32(lines[4 + numAtoms + i].Substring(15, 3));
                int rc = Convert.ToInt32(lines[4 + numAtoms + i].Substring(18, 3));
                BondReactingCenterStatus reactingCenter = BondReactingCenterStatus.Unmarked;
                if (rc == 13) reactingCenter = BondReactingCenterStatus.bondMadeOrBroken | BondReactingCenterStatus.bondOrderChanges | BondReactingCenterStatus.aCenter;
                else if (rc == 12) reactingCenter = BondReactingCenterStatus.bondMadeOrBroken | BondReactingCenterStatus.bondOrderChanges;
                else if (rc == 9) reactingCenter = BondReactingCenterStatus.bondOrderChanges | BondReactingCenterStatus.aCenter;
                else if (rc == 5) reactingCenter = BondReactingCenterStatus.bondMadeOrBroken | BondReactingCenterStatus.aCenter;
                else reactingCenter = (BondReactingCenterStatus)rc;
                molecule.AddBond(molecule.Atoms[firstAtom - 1], molecule.Atoms[secondAtom - 1], bondType, bondStereo, bondTopology, reactingCenter);
            }
            molecule.FindRings();
            return molecule;
        }
    }
}
