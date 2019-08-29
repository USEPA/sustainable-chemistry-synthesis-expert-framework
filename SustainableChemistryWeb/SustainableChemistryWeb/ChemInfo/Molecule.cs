using System;
using System.Collections.Generic;
using System.Linq;

namespace SustainableChemistryWeb.ChemInfo
{
    struct FunctionalGroupAtoms
    {
        public Atom[] Atoms;
        public string FunctionalGroup;

        public FunctionalGroupAtoms(string group, Atom[] atoms)
        {
            FunctionalGroup = group;
            Atoms = atoms;
        }
    }


    [Serializable]
    [Newtonsoft.Json.JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)]
    public class Molecule
    {
        List<Atom> m_Atoms;
        List<List<Atom>> cycles;
        List<FunctionalGroupAtoms> groupAtoms;
        List<Atom[]> paths;
        bool[,] touching;
        bool[,] fused;
        bool ringsFound;
        bool pathsFound;
        System.Collections.Hashtable atomsCount;
        //FunctionalGroupCollection m_FunctionalGroups;
        bool aromaticitySet;
        bool[] isRingAromatic;
        bool[] isRingHeterocyclic;
        bool[] isRingHeterocyclicAromatic;

        public Molecule()
        {
            m_Atoms = new List<Atom>();
            //m_FunctionalGroups = new FunctionalGroupCollection();
            groupAtoms = new List<FunctionalGroupAtoms>();
            atomsCount = new System.Collections.Hashtable();
            aromaticitySet = false;
            Aromatic = false;
            ringsFound = false;
            pathsFound = false;
        }

        public Molecule(string smiles)
        {
            m_Atoms = new List<Atom>();
            atomsCount = new System.Collections.Hashtable();
            aromaticitySet = false;
            Aromatic = false;
            ringsFound = false;
            pathsFound = false;
            smilesLexer lexer = new smilesLexer(new Antlr4.Runtime.AntlrInputStream(smiles));
            smilesParser parser = new smilesParser(new Antlr4.Runtime.CommonTokenStream(lexer));
            try
            {
                this.m_Atoms.AddRange((Atom[])new SmilesVisitor().Visit(parser.smiles()));
            }
            catch (SystemException)
            {
                this.m_Atoms.Clear();
            }
            groupAtoms = new List<FunctionalGroupAtoms>();
            this.FindRings();
            //groupAtoms = new List<Atom[]>();
            Smiles = smiles;
        }

        [Newtonsoft.Json.JsonProperty]
        public bool Aromatic { get; internal set; }
        [Newtonsoft.Json.JsonProperty]
        public bool Heterocyclic { get; internal set; }
        [Newtonsoft.Json.JsonProperty]
        public bool HeterocyclicAromatic { get; internal set; }
        [Newtonsoft.Json.JsonProperty]
        public string Smiles { get; }

        [Newtonsoft.Json.JsonProperty]
        public Atom[] Atoms
        {
            get
            {
                return m_Atoms.ToArray<Atom>();
            }
        }

        [Newtonsoft.Json.JsonProperty]
        public double MolecularWeight
        {
            get
            {
                double retval = 0.0;
                foreach (Atom a in m_Atoms)
                    retval = retval + a.AtomicMass;
                return retval;
            }
        }

        public Atom GetNextAtom(Atom a)
        {
            if (a == m_Atoms[m_Atoms.Count - 1]) return null;
            return m_Atoms[m_Atoms.IndexOf(a) + 1];
        }

        public void AddAtom(Atom a)
        {
            if (m_Atoms.Contains(a)) return;
            m_Atoms.Add(a);
            IncrementAtomCount(a);
            ringsFound = false;
            pathsFound = false;
        }

        void IncrementAtomCount(Atom a)
        {
            if (atomsCount.Contains(a.Element))
            {
                atomsCount[a.Element] = ((int)atomsCount[a.Element]) + 1;
                return;
            }
            atomsCount.Add(a.Element, 1);
        }

        public Bond AddBond(Atom atomOne, Atom atomTwo, BondType type, BondStereo stereo, BondTopology topology, BondReactingCenterStatus rcStatus)
        {
            if (atomOne != null)
            {
                atomOne.AddConnectedAtom(atomTwo);
                atomTwo.AddConnectedAtom(atomOne);
                ringsFound = false;
                pathsFound = false;
                return atomOne.AddBond(atomTwo, type, stereo, topology, rcStatus);
            }
            return null;
        }


        /// <summary>
        /// Returns the <see cref="ChemInfo.Bond"/> between two <see cref="ChemInfo.Atom"/> objects.
        /// </summary>
        /// <param name="atom1">One <see cref="ChemInfo.Atom"/></param> object to locate the <see cref="ChemInfo.Bond"/>.
        /// <param name="atom2">The other <see cref="ChemInfo.Atom"/></param> object attached by the <see cref="ChemInfo.Bond"/>.</param>
        /// <returns><see cref="ChemInfo.Bond"/> between the two <see cref="ChemInfo.Atom"/> objects. If there is no bond bewteen the atoms, it returns null.</returns>
        public Bond GetBond(Atom atom1, Atom atom2)
        {
            foreach (Bond b in atom1.BondedAtoms)
            {
                if (b.ConnectedAtom == atom2) return b;
            }
            foreach (Bond b in atom2.BondedAtoms)
            {
                if (b.ConnectedAtom == atom1) return b;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal Atom[][] FindRings()
        {
            if (ringsFound) return ConvertToArrayArray(cycles);
            Stack<Atom> myStack = new Stack<Atom>();
            foreach (Atom a in m_Atoms) a.Visited = false;
            cycles = new List<List<Atom>>();
            foreach (Atom a in m_Atoms)
            {
                FindRings(a, a, myStack);
            }
            ExtractRings();
            FusedRings();
            TestHeterocyclic();
            TestAromaticity();
            TestHeterocyclicAromaticity();
            ringsFound = true;
            return ConvertToArrayArray(cycles);
        }

        public void FindAllPaths()
        {
            if (pathsFound) return;
            paths = new List<Atom[]>();
            Stack<Atom> stack = new Stack<Atom>();
            Atom[] ends = EndAtoms();
            for (int i = 0; i < ends.Length - 1; i++)
            {
                for (int j = i + 1; j < ends.Length; j++)
                {
                    stack.Clear();
                    foreach (Atom a in m_Atoms) a.Visited = false;
                    FindPath(ends[i], ends[j], null, stack);
                }
            }
            pathsFound = true;
        }

        public Atom[] EndAtoms()
        {
            List<Atom> atoms = new List<ChemInfo.Atom>();
            foreach (Atom a in m_Atoms) if (a.Degree == 1) atoms.Add(a);
            return atoms.ToArray();
        }

        public void FindPath(Atom current, Atom target, Atom parent, Stack<Atom> stack)
        {
            current.Visited = true;
            stack.Push(current);
            if (current == target)
            {
                //we're done... return the path
                Atom[] a = stack.ToArray<Atom>();
                paths.Add(a.Reverse().ToArray());
                return;
            }
            else
            {
                foreach (Atom next in current.ConnectedAtoms)
                {
                    if (next != parent)
                    {
                        if (!next.Visited) FindPath(next, target, current, stack);
                        //
                    }
                }
            }
            stack.Pop();
        }

        public Atom[] FindLongestpath()
        {
            if (!pathsFound) FindAllPaths();
            // paths will be 0 if all atoms are in a ring...
            if (paths.Count == 0) return new Atom[0];
            paths.Sort(CompareArrayByCount);
            paths.Reverse();
            return paths[0];
        }

        void FindRings(Atom current, Atom parent, Stack<Atom> stack)
        {
            stack.Push(current);
            foreach (Atom next in current.ConnectedAtoms)
            {
                if (next != parent)
                {
                    if (stack.Contains(next))
                    {
                        List<Atom> cycle = new List<Atom>();
                        foreach (Atom a in stack)
                        {
                            cycle.Add(a);
                            if (a == next) break;
                        }
                        cycles.Add(cycle);
                        stack.Pop();
                        return;
                    }
                    else if (!next.Visited)
                    {
                        FindRings(next, current, stack);
                    }
                }
            }
            if (stack.Count != 0) stack.Pop();
            current.Visited = true;
        }

        void BreadthFirstSearch(Atom current, Atom parent, Queue<Atom> queue)
        {
            //current.Visited = true;
            queue.Enqueue(current);
            List<Atom> path = new List<ChemInfo.Atom>();
            while (queue.Count > 0)
            {
                Atom v1 = queue.Dequeue();
                path.Add(v1);
                foreach (Atom a in v1.ConnectedAtoms)
                {
                    if (!a.Visited)
                    {
                        queue.Enqueue(a);
                        //a.Visited = true;
                    }
                }
            }
            current.Visited = true;
        }

        Atom[] DepthFirstSearch(Atom current, Atom parent, Stack<Atom> stack)
        {
            current.Visited = true;
            stack.Push(current);
            if (current.ConnectedAtoms.Length == 1)
            {
                //we're done... return the path
                Atom[] a = stack.ToArray<Atom>();
                return (a.Reverse().ToArray());
            }
            else
            {
                foreach (Atom next in current.ConnectedAtoms)
                {
                    if (next != parent)
                    {
                        if (!next.Visited) DepthFirstSearch(next, current, stack);
                        //
                    }
                }
            }
            stack.Pop();
            return null;
        }

        T[][] ConvertToArrayArray<T>(List<List<T>> lists)
        {
            T[][] retVal = new T[lists.Count][];
            for (int i = 0; i < lists.Count; i++)
            {
                retVal[i] = lists[i].ToArray<T>();
            }
            return retVal;
        }

        List<T> Matches<T>(List<T> list1, List<T> list2)
        {
            List<T> retVal = new List<T>();
            foreach (T val in list1)
            {
                int location = list2.IndexOf(val);
                if (location >= 0)
                {
                    retVal.Add(val);
                }
            }
            return retVal;
        }

        void ExtractRings()
        {
            cycles.Sort(CompareListsByLength);
            for (int i = 0; i < cycles.Count; i++)
            {
                for (int j = i + 1; j < cycles.Count; j++)
                {
                    List<Atom> m = Matches(cycles[j], cycles[i]);
                    if (m.Count > 2)
                    {
                        if (m.Count == cycles[i].Count)
                        {
                            BondCollection start = m[0].BondedAtoms;
                            int first = cycles[j].IndexOf(m[0]);
                            int last = cycles[j].IndexOf(m[m.Count - 1]);
                            if (last < first)
                            {
                                cycles[j].RemoveRange(first, last - first);
                            }
                            else
                            {
                                if (cycles[j][0] == m[0])
                                {
                                    last = cycles[j].IndexOf(m[1]);
                                    int count = cycles[j].Count - last;
                                    cycles[j].RemoveRange(last + 1, count - 1);
                                }
                                else
                                {
                                    List<int> ints = new List<int>();
                                    foreach (Atom a in m)
                                    {
                                        ints.Add(m_Atoms.IndexOf(a) + 1);
                                    }
                                    cycles[j].RemoveRange(cycles[j].IndexOf(m[1]), m.Count - 2);
                                }
                            }
                        }
                        else
                        {
                            BondCollection start = m[0].BondedAtoms;
                            BondCollection end = m[m.Count - 1].BondedAtoms;
                            Bond bridge = null;
                            foreach (Bond n in end)
                            {
                                if (start.Contains(n)) bridge = n;
                            }
                            if (bridge != null)
                            {
                                for (int k = 1; k < m.Count - 1; k++)
                                {
                                    cycles[j].Remove(m[k]);
                                }
                                cycles[j].Insert(cycles[j].IndexOf(m[0]) + 1, bridge.ConnectedAtom);
                            }
                        }
                    }
                }
            }
        }

        void FusedRings()
        {
            touching = new bool[this.cycles.Count, this.cycles.Count];
            fused = new bool[this.cycles.Count, this.cycles.Count];
            for (int i = 0; i < this.cycles.Count; i++)
            {
                for (int j = i + 1; j < this.cycles.Count; j++)
                {
                    int common = 0;
                    foreach (Atom a in this.cycles[j])
                    {
                        if (this.cycles[i].Contains(a)) common = common + 1;
                    }
                    if (common == 1) touching[i, j] = true;
                    if (common == 2) fused[i, j] = true;
                }
            }
        }
        //C1=NC=CC=C1

        void TestHeterocyclic()
        {
            isRingHeterocyclic = new bool[cycles.Count];
            for (int i = 0; i < cycles.Count; i++)
            {
                isRingHeterocyclic[i] = false;
                foreach (Atom a in cycles[i])
                {
                    if (a.Element != ELEMENTS.C)
                    {
                        isRingHeterocyclic[i] = true;
                        Heterocyclic = true;
                    }
                }
            }
        }

        void TestAromaticity()
        {
            //if weve already done this, don't do it again.
            if (this.aromaticitySet) return;
            this.aromaticitySet = true;
            if (this.cycles.Count == 0) Aromatic = false;
            isRingAromatic = new bool[cycles.Count];
            for (int i = 0; i < cycles.Count; i++)
            {
                int numPi = 0;
                isRingAromatic[i] = false;
                if (cycles[i][0].AtomType == ChemInfo.AtomType.AROMATIC)
                {
                    isRingAromatic[i] = true;
                    Aromatic = true;
                }
                else
                {
                    foreach (Atom a in cycles[i])
                    {
                        numPi = numPi + a.NumPiElectrons;
                    }
                }
                if ((numPi - 2) % 4 == 0)
                {
                    isRingAromatic[i] = true;
                    Aromatic = true;
                    foreach (Atom a in cycles[i])
                    {
                        a.AtomType = AtomType.AROMATIC;
                        foreach (Bond b in a.BondedAtoms)
                        {
                            if (cycles[i].Contains(b.ConnectedAtom))
                                b.BondType = BondType.Aromatic;
                        }
                    }
                }
            }
        }

        void TestHeterocyclicAromaticity()
        {
            isRingHeterocyclicAromatic = new bool[cycles.Count];
            if (!this.Aromatic || !this.Heterocyclic)
            {
                HeterocyclicAromatic = false;
                return;
            }
            for (int i = 0; i < cycles.Count; i++)
            {
                isRingHeterocyclicAromatic[i] = false;
                if (isRingAromatic[i] && isRingHeterocyclic[i])
                {
                    this.HeterocyclicAromatic = true;
                    isRingHeterocyclicAromatic[i] = true;
                }
            }
        }

        public bool RingContains(Atom[] indices)
        {
            foreach (List<Atom> atoms in cycles)
            {
                bool contained = true;
                foreach (Atom a in indices)
                    if (!atoms.Contains(a)) contained = false;
                if (contained) return true;
            }
            return false;
        }

        // VF2 Implementation

        public bool FunctionalGroupsFound { get; set; } = false;

        [Newtonsoft.Json.JsonProperty]
        public string[] FunctionalGroups
        {
            get
            {
                List<string> groups = new List<string>();
                foreach (FunctionalGroupAtoms g in groupAtoms)
                {
                    if (!groups.Contains(g.FunctionalGroup)) groups.Add(g.FunctionalGroup);
                }
                return groups.ToArray<string>();
            }
        }

        public bool FindFunctionalGroup(string smarts)
        {
            Molecule m = new Molecule(smarts);
            int pn = 0;
            int[] matches = null;
            int[] atoms = null;
            return this.Match(ref pn, ref matches, ref atoms, new FunctionalGroupState(m, this, false));
        }

        public bool FindFunctionalGroup(SustainableChemistryWeb.Models.FunctionalGroup group)
        {
            if (this.Atoms.Length == 0) return false;
            Molecule m = new Molecule(group.Smarts);
            bool retVal = false;
            int pn = 0;
            int[] matches = null;
            int[] at = null;
            foreach (Atom a in m_Atoms) a.inGroup = false;
            bool matched = this.Match(ref pn, ref matches, ref at, new FunctionalGroupState(m, this, false));
            while (matched)
            {
                Atom[] a = null;
                if (at != null)
                {
                    a = new Atom[at.Length];
                    for (int i = 0; i < at.Length; i++)
                    {
                        a[i] = this.Atoms[at[i]];
                        a[i].inGroup = true;
                    }
                }
                if (group.Name == "LACTONE")
                {
                    Atom[] atoms = { m_Atoms[at[0]], m_Atoms[at[1]] };
                    if (this.RingContains(atoms))
                    {
                        //groupAtoms.AddAtoms(at);
                        retVal = true;
                    }
                }
                else if (group.Name == "LACTAM")
                {
                    Atom[] atoms = { m_Atoms[at[0]], m_Atoms[at[1]] };
                    if (this.RingContains(atoms))
                    {
                        //group.AddAtoms(at);
                        retVal = true;
                    }
                }
                else
                {
                    //group.AddAtoms(at);
                    retVal = true;
                }
                matched = this.Match(ref pn, ref matches, ref at, new FunctionalGroupState(m, this, false));
            }
            if (retVal)
            {
                Atom[] atoms = new Atom[at.Length];
                for (int i = 0; i < at.Length; i++) atoms[i] = m_Atoms[at[i]];
                this.groupAtoms.Add(new FunctionalGroupAtoms(group.Name, atoms));
            }
            return retVal;
        }

        //public bool FindFunctionalGroup(FunctionalGroup group)
        //{
        //    bool retVal = false;
        //    Molecule m = new Molecule(group.Smart);
        //    int pn = 0;
        //    int[] matches = null;
        //    int[] at = null;
        //    foreach (Atom a in m_Atoms) a.inGroup = false;
        //    bool matched = this.Match(ref pn, ref matches, ref at, new FunctionalGroupState(m, this, false));
        //    while (matched)
        //    {                
        //        Atom[] a = null;
        //        if (at != null)
        //        {
        //            a = new Atom[at.Length];
        //            for (int i = 0; i < at.Length; i++)
        //            {
        //                a[i] = this.Atoms[at[i]];
        //                a[i].inGroup = true;
        //            }
        //        }
        //        if (group.Name == "LACTONE")
        //        {
        //            Atom[] atoms = { m_Atoms[at[0]], m_Atoms[at[1]] };
        //            if (this.RingContains(atoms))
        //            {
        //                //groupAtoms.AddAtoms(at);
        //                retVal = true;
        //            }
        //        }
        //        else if (group.Name == "LACTAM")
        //        {
        //            Atom[] atoms = { m_Atoms[at[0]], m_Atoms[at[1]] };
        //            if (this.RingContains(atoms))
        //            {
        //                //group.AddAtoms(at);
        //                retVal = true;
        //            }
        //        }
        //        else
        //        {
        //            //group.AddAtoms(at);
        //            retVal = true;
        //        }
        //        matched = this.Match(ref pn, ref matches, ref at, new FunctionalGroupState(m, this, false));
        //    }
        //    if (retVal)
        //    {
        //        Atom[] atoms = new Atom[at.Length];
        //        for (int i = 0; i < at.Length; i++) atoms[i] = m_Atoms[at[i]];
        //        this.groupAtoms.Add(new FunctionalGroupAtoms(group.Name, atoms));
        //    }
        //    return retVal;
        //}


        internal bool Match(ref int pn, ref int[] c1, ref int[] c2, State s)
        {
            if (s.IsGoal())
            {
                pn = s.CoreLen();
                s.GetCoreSet(ref c1, ref c2);
                return true;
            }

            if (s.IsDead())
                return false;

            int n1 = -1;
            int n2 = -1;
            bool found = false;
            while (!found && s.NextPair(ref n1, ref n2, n1, n2))
            {
                if (s.IsFeasiblePair(n1, n2))
                {
                    State s1 = s.Clone();
                    s1.AddPair(n1, n2);
                    found = Match(ref pn, ref c1, ref c2, s1);
                    s1.BackTrack();
                    //delete s1;
                }
            }
            return found;
        }

        internal bool Match(Stack<Atom> atomsInGroup, Stack<Atom> matchedInMolecule, State2 s)
        {
            if (s.IsGoal())
            {
                //pn = s.CoreLen();
                //s.GetCoreSet(ref c1, ref c2);
                return true;
            }

            Atom n1 = null;
            Atom n2 = null;
            bool found = false;
            while (!found && s.NextPair(ref n1, ref n2, n1, n2))
            {
                if (s.IsFeasiblePair(n1, n2))
                {
                    State2 s1 = s.Clone();
                    s1.AddPair(n1, n2);
                    found = Match(atomsInGroup, matchedInMolecule, s1);
                    s1.BackTrack();
                    //delete s1;
                }
            }
            return found;
        }

        internal bool Match(Atom currentInGroup, Atom currentInMolecule, Stack<Atom> atomsInGroup, Stack<Atom> matchedInMolecule, Molecule groupToMatch)
        {
            if (atomsInGroup.Count == groupToMatch.Atoms.Length)
            {
                //pn = s.CoreLen();
                //s.GetCoreSet(ref c1, ref c2);
                return true;
            }

            Atom currentGroupParent = currentInGroup;
            currentInGroup = groupToMatch.GetNextAtom(currentInGroup);
            Atom currentMoleculeParent = currentInMolecule;
            while (currentInGroup != null)
            {
                currentInMolecule = m_Atoms[0];
                while (currentInMolecule != null)
                {
                    foreach (Atom next in currentInGroup.ConnectedAtoms)
                    {
                        if (next != currentGroupParent)
                        {
                            foreach (Atom nextInMolecule in currentInMolecule.ConnectedAtoms)
                            {
                                if (nextInMolecule != currentMoleculeParent)
                                {
                                    if (next.Element == nextInMolecule.Element)
                                    {
                                        Bond b1 = groupToMatch.GetBond(next, currentInGroup);
                                        Bond b2 = this.GetBond(nextInMolecule, currentInMolecule);
                                        if (b1.BondType == b2.BondType)
                                        {
                                            atomsInGroup.Push(currentInGroup);
                                            matchedInMolecule.Push(currentInMolecule);
                                            if (Match(next, currentInGroup, atomsInGroup, matchedInMolecule, groupToMatch)) return true;
                                            matchedInMolecule.Pop();
                                            atomsInGroup.Pop();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    currentMoleculeParent = currentInMolecule;
                    currentInMolecule = this.GetNextAtom(currentInMolecule);
                }
                currentMoleculeParent = null;
                currentGroupParent = currentInGroup;
                currentInGroup = groupToMatch.GetNextAtom(currentInGroup);
            }
            return false;
        }

        internal Atom NextConnectedAtom(Atom atom, Atom currentConnectedAtom)
        {
            if (currentConnectedAtom == null) return atom.ConnectedAtoms[0];
            for (int i = 0; i < atom.ConnectedAtoms.Length - 1; i++)
                if (atom.ConnectedAtoms[i] == currentConnectedAtom) return atom.ConnectedAtoms[i + 1];
            return null;
        }

        internal bool HasEdge(int n1, int n2)
        {
            Atom[] atoms = this.m_Atoms[n1].ConnectedAtoms;
            foreach (Atom a in atoms)
            {
                if (this.m_Atoms.IndexOf(a) > -1) return true;
            }
            return false;
        }

        internal bool HasEdge(Atom n1, Atom n2)
        {
            return n1.ConnectedAtoms.Contains(n2);
        }

        internal Bond GetEdgeAttr(int n1, int n2)
        {
            return this.GetBond(m_Atoms[n1], m_Atoms[n2]);
        }

        internal Bond GetEdgeAttr(Atom n1, Atom n2)
        {
            return this.GetBond(n1, n2);
        }

        internal int InEdgeCount(int node)
        {
            return m_Atoms[node].ConnectedAtoms.Length;
        }

        internal int InEdgeCount(Atom node)
        {
            return node.ConnectedAtoms.Length;
        }

        internal int OutEdgeCount(int node)
        {
            return m_Atoms[node].ConnectedAtoms.Length;
        }

        internal int OutEdgeCount(Atom node)
        {
            return node.ConnectedAtoms.Length;
        }

        internal int EdgeCount(int node)
        {
            return m_Atoms[node].ConnectedAtoms.Length;
        }

        internal int EdgeCount(Atom node)
        {
            return node.ConnectedAtoms.Length;
        }

        internal int GetInEdge(int node, int i)
        {
            Atom a = m_Atoms[node].ConnectedAtoms[i];
            return m_Atoms.IndexOf(a);
        }

        internal Atom GetInEdge(Atom node, int i)
        {
            return node.ConnectedAtoms[i];
        }

        internal int GetInEdge(int node, int i, ref Bond pattr)
        {
            Atom a = m_Atoms[node].ConnectedAtoms[i];
            pattr = this.GetBond(m_Atoms[node], a);
            return m_Atoms.IndexOf(a);
        }

        internal Atom GetInEdge(Atom node, int i, ref Bond pattr)
        {
            Atom a = node.ConnectedAtoms[i];
            pattr = this.GetBond(node, a);
            return a;
        }

        internal int GetOutEdge(int node, int i)
        {
            Atom a = m_Atoms[node].ConnectedAtoms[i];
            return m_Atoms.IndexOf(a);
        }

        internal Atom GetOutEdge(Atom node, int i)
        {
            return node.ConnectedAtoms[i];
        }

        internal int GetOutEdge(int node, int i, ref Bond pattr)
        {
            Atom a = m_Atoms[node].ConnectedAtoms[i];
            pattr = this.GetBond(m_Atoms[node], a);
            return m_Atoms.IndexOf(a);
        }

        internal Atom GetOutEdge(Atom node, int i, ref Bond pattr)
        {
            Atom a = node.ConnectedAtoms[i];
            pattr = this.GetBond(node, a);
            return a;
        }

        internal bool CompatibleNode(ELEMENTS attr1, ELEMENTS attr2)
        {
            return (attr1 == attr2);
        }

        internal bool CompatibleNode(int attr1, int attr2)
        {
            if (attr1 == 1) return true;
            return (attr1 == attr2);
        }

        internal bool CompatibleNode(Atom atom1, Atom atom2)
        {
            if (atom2.inGroup) return false;
            if (atom1.Element == ELEMENTS.WILD_CARD) return true;
            if (atom2.Element == ELEMENTS.WILD_CARD) return true;
            if (atom1.Element == ELEMENTS.Halogen)
            {
                if (atom2.Element == ELEMENTS.F) return true;
                if (atom2.Element == ELEMENTS.Cl) return true;
                if (atom2.Element == ELEMENTS.Br) return true;
                if (atom2.Element == ELEMENTS.I) return true;
                if (atom2.Element == ELEMENTS.At) return true;
                return false;
            }
            if (atom1.Element != atom2.Element) return false;
            if (atom1.ExplicitHydrogens > atom2.NumHydrogens) return false;
            if (atom1.Degree == 1) return true;
            if (atom1.Degree != atom2.Degree) return false;
            return true;
        }

        internal bool CompatibleEdge(Bond attr1, Bond attr2)
        {
            if (attr2 == null) return false;
            if (attr1.BondType != attr2.BondType) return false;
            return true;
        }

        internal Atom GetNodeAttr(int i)
        {
            return m_Atoms[i];
        }

        internal int GetNodeDegree(int i)
        {
            return m_Atoms[i].Degree;
        }


        int CompareArrayByCount<T>(T[] array1, T[] array2)
        {
            return array1.Length - array2.Length;
        }

        int CompareListsByLength<T>(List<T> list1, List<T> list2)
        {
            return list1.Count - list2.Count;
        }

        void CanonizerWeinginger()
        {
            // Step 1. Set Initial Invariants and go to Step 3.
            // The initial invariants are handled by the comparer.
            // Step 3. Sort the vector.
            m_Atoms.Sort(AtomInvariantComparerWeinginger);

            // Step 4. RankException Atomic Vector 
            int highestRank = this.RankAtomsWeinginger();
            // Step 6. Save partition as symmetry class.
            foreach (Atom a in this.m_Atoms) a.WeiningerSymmetryClass = a.WeiningerRank;
            // If the number of ranks is smaller than the number of atoms, break ties...
            // Step 5. If not invariant, go to Step 2.
            // Step 7. If highest rank is smaller than the number of nodes, break ties and go to Step 2.
            // if the number of ranks equals the number of atoms, we're done.
            while (highestRank < this.m_Atoms.Count)
            {
                this.m_Atoms.Sort(WeiningerProductOfPrimesComparer);
                highestRank = this.RankAtomsWeinginger();
            }
        }

        int AtomInvariantComparerWeinginger(Atom atom1, Atom atom2)
        {
            // Number of connections is the first test.
            if (atom1.WeiningerInvariant.NumberOfConnections != atom2.WeiningerInvariant.NumberOfConnections) return atom1.WeiningerInvariant.NumberOfConnections - atom2.WeiningerInvariant.NumberOfConnections;
            // Followed by the number of non-hydrogen bonds.
            if (atom1.WeiningerInvariant.NumberOfNonHydrogenBonds != atom2.WeiningerInvariant.NumberOfNonHydrogenBonds) return atom1.WeiningerInvariant.NumberOfNonHydrogenBonds - atom2.WeiningerInvariant.NumberOfNonHydrogenBonds;
            //Then atomic number        
            if (atom1.WeiningerInvariant.AtomicNumber != atom2.WeiningerInvariant.AtomicNumber) return atom1.WeiningerInvariant.AtomicNumber - atom2.WeiningerInvariant.AtomicNumber;
            // Sign of charge, and then charge, but why both. I'm guessing 1980s computer issues that Moore's Law solved.
            // This can be done in one sort, which the same results, especially since the comparer returns an integer.
            if (atom1.WeiningerInvariant.Charge != atom2.WeiningerInvariant.Charge) return atom1.WeiningerInvariant.Charge - atom2.WeiningerInvariant.Charge;
            // Next is number of attached hydrogens
            if (atom1.WeiningerInvariant.NumberOfAttachedHydrogens != atom2.WeiningerInvariant.NumberOfAttachedHydrogens) return atom1.WeiningerInvariant.NumberOfAttachedHydrogens - atom2.WeiningerInvariant.NumberOfAttachedHydrogens;
            return 0;
        }

        int WeiningerProductOfPrimesComparer(Atom atom1, Atom atom2)
        {
            if (atom1.WeiningerRank != atom2.WeiningerRank) return atom1.WeiningerRank - atom2.WeiningerRank;
            return atom1.WeiningerProductOfPrimes - atom2.WeiningerProductOfPrimes;
        }

        int RankAtomsWeinginger()
        {
            int currentRank = 1;
            m_Atoms[0].WeiningerRank = currentRank;
            for (int i = 1; i < m_Atoms.Count; i++)
            {
                if (m_Atoms[i = 1].WeiningerInvariant != m_Atoms[i].WeiningerInvariant) currentRank++;
                else if (m_Atoms[i - 1].WeiningerProductOfPrimes != m_Atoms[i].WeiningerProductOfPrimes) currentRank++;
                m_Atoms[i].WeiningerRank = currentRank;
            }
            return currentRank;
        }


        // Molecule Boundaries for drawing algorithm

        public System.Drawing.Rectangle GetLocationBounds()
        {
            int top = m_Atoms[0].Location2D.Y;
            int bottom = m_Atoms[0].Location2D.Y;
            int left = m_Atoms[0].Location2D.X;
            int right = m_Atoms[0].Location2D.X;
            foreach (Atom a in m_Atoms)
            {
                if (top > a.Location2D.Y) top = a.Location2D.Y;
                if (bottom < a.Location2D.Y) bottom = a.Location2D.Y;
                if (left > a.Location2D.X) left = a.Location2D.X;
                if (right < a.Location2D.X) right = a.Location2D.X;
            }
            m_Location = new System.Drawing.Point(left, top);
            m_Size = new System.Drawing.Size(right - left, bottom - top);
            return new System.Drawing.Rectangle(left, top, right - left, bottom - top);
        }

        System.Drawing.Point m_Location;
        System.Drawing.Size m_Size = new System.Drawing.Size(1100, 850);
        public System.Drawing.Point Location
        {
            get
            {
                return m_Location;
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return m_Size;
            }
        }

        public System.Drawing.Point[] AtomLocations
        {
            get
            {
                List<System.Drawing.Point> locations = new List<System.Drawing.Point>();
                foreach (Atom a in this.m_Atoms)
                {
                    locations.Add(a.Location2D);
                }
                return locations.ToArray();
            }
        }

        public void CenterMolecule()
        {
            int sumX = 0;
            int sumY = 0;
            foreach (Atom a in this.m_Atoms)
            {
                sumX = sumX + a.Location2D.X;
                sumY = sumY + a.Location2D.Y;
            }
            int offsetX = sumX / this.m_Atoms.Count;
            int offsetY = sumY / this.m_Atoms.Count;
            foreach (Atom a in this.m_Atoms)
            {
                System.Drawing.Point p = new System.Drawing.Point(a.Location2D.X - offsetX, a.Location2D.Y - offsetY);
                a.Location2D = p;
            }
        }

        public void CenterMolecule(System.Drawing.Rectangle rect)
        {
            this.CenterMolecule();
            foreach (Atom a in this.m_Atoms)
            {
                System.Drawing.Point p = new System.Drawing.Point(a.Location2D.X + rect.X + rect.Width / 2, a.Location2D.Y + rect.Y + rect.Height / 2);
                a.Location2D = p;
            }

        }

        // General Force Directed Graph Atom Location methods

        int GetArea()
        {
            return Size.Height * Size.Width;
        }

        double DistanceBetweenAtoms(Atom a1, Atom a2)
        {
            return Math.Sqrt(DistanceBetweenAtomsSquared(a1, a2));
        }

        double DistanceBetweenAtomsSquared(Atom a1, Atom a2)
        {
            int delX = a1.Location2D.X - a2.Location2D.X;
            int delY = a1.Location2D.Y - a2.Location2D.Y;
            return Math.Pow(delX, 2) + Math.Pow(delY, 2);
        }

        void RandomLocateAtoms()
        {
            System.Random random = new Random();
            foreach (Atom a in m_Atoms)
            {
                a.Location2D = new System.Drawing.Point(random.Next(Size.Width), random.Next(Size.Height));
            }
        }

        double GetNextBondLength(Atom a1, Atom a2, Atom a3)
        {
            Bond b1 = GetBond(a1, a2);
            Bond b2 = GetBond(a2, a3);
            if (b1.BondType == BondType.Single && b2.BondType == BondType.Single)
                return Math.Sqrt(Math.Pow(100 * b1.BondLength, 2) + Math.Pow(100 * b1.BondLength, 2));
            return -1;
        }

        protected void SetHydrogens()
        {
            for (int i = this.m_Atoms.Count - 1; i >= 0; i--)
            {
                m_Atoms.AddRange(this.m_Atoms[i].SetHydrogens());
                m_Atoms.Remove(m_Atoms[i].RemoveOneHydrogen());
            }
        }

        protected void ResetHydrogens()
        {
            for (int i = this.m_Atoms.Count - 1; i >= 0; i--)
            {
                Atom[] hydrogens = m_Atoms[i].RemoveHydrogens();
                foreach (Atom a in hydrogens)
                {
                    this.m_Atoms.Remove(a);
                }
            }
        }

        protected void SetUnboundedPairs()
        {

        }

        // Fruchterman and Reingold (1991) parameters

        double m_OptimalDistance = 0;
        public double OptimalDistanceBetweenVertices
        {
            get
            {
                return m_OptimalDistance;
            }
        }

        public double CalculateOptimalDistanceBetweenVertices()
        {
            m_OptimalDistance = Math.Sqrt(this.GetArea() / this.m_Atoms.Count);
            return m_OptimalDistance;
        }

        double RepulsiveMiutiplier { get; set; } = 0.2;
        double AttractiveMultiplier { get; set; } = 15;

        // Fruchterman and Reingold (1991) Force Calculations
        double AttractiveForce(double distance)
        {
            return this.AttractiveMultiplier * Math.Pow(distance, 2) / m_OptimalDistance;
        }

        double RepulsiveForce(double distance)
        {
            return this.RepulsiveMiutiplier * Math.Pow(m_OptimalDistance, 2) / distance;
        }

        // Fruchterman and Reingold (1991) annealing temperature
        public double InitialTemperature
        {
            get
            {
                return Math.Min(Size.Height, Size.Width) / 10;
            }
        }

        double Temperature
        {
            get;
            set;
        }


        // Fraczek (2016) force parameters
        double m_cRep = 5625;
        public double cRep
        {
            get
            {
                return m_cRep;
            }
            set
            {
                m_cRep = value;
            }
        }

        double m_cBond = 10;
        public double cBond
        {
            get
            {
                return m_cBond;
            }
            set
            {
                m_cBond = value;
            }
        }

        // Fraczek (2016) force calculcations
        double FraczekRepulsiveForce(Atom a1, Atom a2)
        {
            return m_cRep / DistanceBetweenAtoms(a1, a2);
        }

        double FraczekRepulsiveForce(double distance)
        {
            return m_cRep / distance;
        }


        double BondAngleForce(Bond b1, Bond b2)
        {
            double distance = Math.Sqrt(Math.Pow(b1.BondLength, 2) + Math.Pow(b1.BondLength, 2));
            double bondLength = double.MaxValue;
            if (b1.ParentAtom == b2.ParentAtom) bondLength = DistanceBetweenAtoms(b1.ConnectedAtom, b2.ConnectedAtom);
            else if (b1.ParentAtom == b2.ConnectedAtom) bondLength = DistanceBetweenAtoms(b1.ConnectedAtom, b2.ParentAtom);
            else if (b1.ConnectedAtom == b2.ParentAtom) bondLength = DistanceBetweenAtoms(b1.ParentAtom, b2.ConnectedAtom);
            else bondLength = DistanceBetweenAtoms(b1.ParentAtom, b2.ParentAtom);
            return 0.3 / (bondLength - distance);
        }


        public void ForceDirectedGraph()
        {
            RandomLocateAtoms();
            SetHydrogens();
            Temperature = InitialTemperature;
            int maxIter = 500;
            for (int i = 0; i < maxIter; i++)
            {
                IterateFGD(i > 25);
                Temperature *= (1.0 - (double)i / (double)maxIter);
            }
            ResetHydrogens();

        }

        protected void IterateFGD(bool addBonds)
        {
            CalculateOptimalDistanceBetweenVertices();
            foreach (Atom v in m_Atoms)
            {
                v.DeltaX = 0.0;
                v.DeltaY = 0.0;
            }
            foreach (Atom v in m_Atoms)
            {
                foreach (Atom u in m_Atoms)
                {
                    if (u != v)
                    {
                        double distance = DistanceBetweenAtoms(u, v);
                        if (distance < 0) continue;
                        if (distance < 1) distance = 1;
                        double delX = v.Location2D.X - u.Location2D.X;
                        double delY = v.Location2D.Y - u.Location2D.Y;
                        v.DeltaX = v.DeltaX + ((delX / distance) * this.RepulsiveForce(distance));
                        v.DeltaY = v.DeltaY + ((delY / distance) * this.RepulsiveForce(distance));
                    }
                }
            }

            foreach (Atom v in m_Atoms)
            {
                foreach (Atom u in m_Atoms)
                {
                    Bond b = GetBond(u, v);
                    if (b != null)
                    {
                        double distance = DistanceBetweenAtoms(u, v);
                        if (distance < 10) distance = 10;
                        double delX = v.Location2D.X - u.Location2D.X;
                        double delY = v.Location2D.Y - u.Location2D.Y;
                        double deltaX = ((double)delX / (double)distance) * this.AttractiveForce(distance);
                        double deltaY = ((double)delY / (double)distance) * this.AttractiveForce(distance);
                        v.DeltaX = v.DeltaX - deltaX;
                        v.DeltaY = v.DeltaY - deltaY;
                        u.DeltaX = u.DeltaX + deltaX;
                        u.DeltaY = u.DeltaY + deltaY;
                    }
                }
            }

            // This adds the Angle force to fix bond angles from Fraczek 2016
            if (addBonds)
            {
                foreach (Atom v in this.m_Atoms)
                {
                    foreach (Atom u in v.ConnectedAtoms)
                    {
                        if (u.ConnectedAtoms.Length < 4)
                        {
                            foreach (Atom a in u.ConnectedAtoms)
                            {
                                if (a != v)
                                {
                                    double distance = this.GetNextBondLength(v, u, a);
                                    if (distance < 10) distance = 10;
                                    double delX = v.Location2D.X - a.Location2D.X;
                                    double delY = v.Location2D.Y - a.Location2D.Y;
                                    double deltaX = 0.5 * (delX / distance) * AttractiveForce(distance);
                                    double deltaY = 0.5 * (delY / distance) * AttractiveForce(distance);
                                    v.DeltaX = v.DeltaX - deltaX;
                                    v.DeltaY = v.DeltaY - deltaY;
                                    a.DeltaX = a.DeltaX + deltaX;
                                    a.DeltaY = a.DeltaY + deltaY;
                                }
                            }
                        }

                        else if (u.ConnectedAtoms.Length == 4)
                        {
                            foreach (Atom a in u.ConnectedAtoms)
                            {
                                if (a != v)
                                {
                                    double distance = this.GetNextBondLength(v, u, a);
                                    if (distance < 10) distance = 10;
                                    double delX = v.Location2D.X - u.Location2D.X;
                                    double delY = v.Location2D.Y - u.Location2D.Y;
                                    double deltaX = 0.75 * ((double)delX / (double)distance) * AttractiveForce(distance);
                                    double deltaY = 0.75 * ((double)delY / (double)distance) * AttractiveForce(distance);
                                    v.DeltaX = v.DeltaX - deltaX;
                                    v.DeltaY = v.DeltaY - deltaY;
                                    u.DeltaX = u.DeltaX + deltaX;
                                    u.DeltaY = u.DeltaY + deltaY;
                                }
                            }
                        }
                    }
                    foreach (ChemInfo.Bond bond in v.BondedAtoms)
                    {
                        if (bond.BondType == BondType.Double)
                        {

                        }
                    }
                }
            }
            foreach (Atom v in m_Atoms)
            {
                double displacement = Math.Sqrt((v.DeltaX * v.DeltaX) + (v.DeltaY * v.DeltaY));
                double x = v.Location2D.X;
                x = x + ((v.DeltaX / displacement) * Math.Min(displacement, Temperature));
                x = Math.Min(Size.Width, Math.Max(0, x));
                double y = v.Location2D.Y;
                y = y + ((v.DeltaY / displacement) * Math.Min(displacement, Temperature));
                y = Math.Min(Size.Height, Math.Max(0, y));
                v.Location2D = new System.Drawing.Point((int)x, (int)y);
            }
        }
    }
}