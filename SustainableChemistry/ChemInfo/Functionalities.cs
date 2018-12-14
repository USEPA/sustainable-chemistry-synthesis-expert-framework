using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{
    //[Serializable]
    //public class FunctionalGroup
    //{
    //    public FunctionalGroup()
    //    {
    //    }

    //    public FunctionalGroup(string name, string smart)
    //    {
    //        Name = name;
    //        Smart = smart;
    //    }

    //    public string Name { get; set; }
    //    public string Smart { get; set; }
    //}

    public static class Functionalities
    {

        static public String[] Elements(Molecule m)
        {
            List<string> retVal = new List<string>();
            Atom[] atoms = m.Atoms;
            foreach (Atom a in atoms)
            {
                if (!retVal.Contains(a.Element.ToString())) retVal.Add(a.Element.ToString());
            }
            return retVal.ToArray();
        }

        static public Atom[] FindElement(Molecule m, String element)
        {
            List<Atom> retVal = new List<Atom>();
            Atom[] atoms = m.Atoms;
            foreach (Atom a in atoms)
            {
                if (a.Element.ToString() == element)
                {
                    retVal.Add(a);
                }
            }
            return retVal.ToArray<Atom>();
        }

        static public Atom[] FindElements(Molecule m, string[] elements)
        {
            List<Atom> retVal = new List<Atom>();
            Atom[] atoms = m.Atoms;
            foreach (Atom a in atoms)
            {
                if (elements.Contains(a.Element.ToString()))
                {
                    retVal.Add(a);
                }
            }
            return retVal.ToArray<Atom>();
        }

        static public Atom[] FindChloride(Molecule m)
        {
            return FindElement(m, "Cl");
        }

        static public Atom[] FindHalides(Molecule m)
        {
            string[] halogens = { "F", "Cl", "Br", "I" };
            return FindElements(m, halogens);
        }

        static public Atom[] FindBromide(Molecule m)
        {
            return FindElement(m, "Br");
        }

        static public Atom[] BranchAtoms(Molecule m)
        {
            List<Atom> retVal = new List<Atom>();
            foreach (Atom a in m.Atoms)
            {
                if (a.BondedAtoms.Count > 2) retVal.Add(a);
            }
            return retVal.ToArray();
        }

        static public Atom[][] HeteroCyclic(Molecule m, string element)
        {
            //if (!cyclesFound) this.GetDFS();
            Atom[][] rings = m.FindRings();
            List<Atom[]> retVal = new List<Atom[]>();
            foreach (Atom[] atoms in rings)
            {
                foreach (Atom a in atoms)
                {
                    if (a.Element.ToString() == element)
                    {
                        retVal.Add(atoms);
                        break;
                    }
                }
            }
            return retVal.ToArray();
        }

        static public Atom[][] HeteroCyclic(Molecule m)
        {
            //if (!cyclesFound) this.GetDFS();
            Atom[][] rings = m.FindRings();
            List<Atom[]> retVal = new List<Atom[]>();
            foreach (Atom[] atoms in rings)
            {
                foreach (Atom a in atoms)
                {
                    if (a.Element != ELEMENTS.C)
                    {
                        retVal.Add(atoms);
                        break;
                    }
                }
            }
            return retVal.ToArray();
        }

        static public string[] PhosphorousFunctionality(Molecule m)
        {
            List<string> retVal = new List<string>();
            System.IO.StringReader reader = new System.IO.StringReader(Properties.Resources.Phosphate_Functional_Groups);
            int[] atoms = null;
            string line = reader.ReadLine();
            while (!String.IsNullOrEmpty(line))
            {
                string[] b = line.Split('\t');
                if (m.FindFunctionalGroup(b[1], ref atoms)) retVal.Add(b[0]);
                line = reader.ReadLine();
            }
            return retVal.ToArray<string>();
        }

        //static public string FunctionalGroups(string smiles, string format)
        //{
        //    Molecule m = new Molecule(smiles);
        //    List<string> retVal = new List<string>();
        //    System.IO.StringReader reader = new System.IO.StringReader(Properties.Resources.Phosphate_Functional_Groups);
        //    int[] atoms = null;
        //    string line = reader.ReadLine();
        //    while (!String.IsNullOrEmpty(line))
        //    {
        //        string[] b = line.Split('\t');
        //        if (m.FindFunctionalGroup(b[1], ref atoms)) retVal.Add(b[0]);
        //        line = reader.ReadLine();
        //    }
        //    if (format == "json")
        //    {
        //        var json = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        return json.Serialize(retVal.ToArray<string>());
        //    }
        //    string val = string.Empty;
        //    foreach (string s in retVal) val = val + "; ";
        //    return val.Remove(val.Length - 1);
        //}

        //static public string[] FunctionalGroups(string smiles)
        //{
        //    Molecule m = new Molecule(smiles);
        //    List<string> retVal = new List<string>();
        //    System.IO.StringReader reader = new System.IO.StringReader(Properties.Resources.Phosphate_Functional_Groups);
        //    int[] atoms = null;
        //    string line = reader.ReadLine();
        //    while (!String.IsNullOrEmpty(line))
        //    {
        //        string[] b = line.Split('\t');
        //        if (m.FindFunctionalGroup(b[1], ref atoms)) retVal.Add(b[0]);
        //        line = reader.ReadLine();
        //    }
        //    return retVal.ToArray<string>();
        //}

        static public string[] AvailablePhosphateFunctionalGroups
        {
            get {
                List<string> groups = new List<string>();
                System.IO.StringReader reader = new System.IO.StringReader(Properties.Resources.Phosphate_Functional_Groups);
                string line = reader.ReadLine();
                while (!String.IsNullOrEmpty(line))
                {
                    string[] b = line.Split('\t');
                    groups.Add(b[0]);
                    line = reader.ReadLine();
                }
                return groups.ToArray();
            }
        }

        //static public string AvailableFunctionalGroups()
        //{
        //    List<FunctionalGroup> groups = new List<FunctionalGroup>();
        //    System.IO.StringReader reader = new System.IO.StringReader(Properties.Resources.Phosphate_Functional_Groups);
        //    string line = reader.ReadLine();
        //    while (!String.IsNullOrEmpty(line))
        //    {
        //        string[] b = line.Split('\t');
        //        groups.Add(new FunctionalGroup(b[0], b[1]));
        //        line = reader.ReadLine();
        //    }
        //    //var json = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    //return json.Serialize(groups);
        //    return string.Empty;
        //}
    }
}
