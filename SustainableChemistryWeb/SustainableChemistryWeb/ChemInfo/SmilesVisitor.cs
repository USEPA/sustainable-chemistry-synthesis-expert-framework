using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SustainableChemistryWeb.ChemInfo
{
    //
    // This smiles parser is based upon an ANTLR 4 visitor developed from the included G4 grammar.
    //
    class SmilesVisitor : smilesBaseVisitor<object>
    {
        Molecule retVal;
        BondType nextBond = BondType.Single;
        Atom last = null;
        System.Collections.Hashtable ringAtoms;
        System.Collections.Hashtable ringbonds;
        string cisTrans;
        bool addExplicitHToNext;

        System.Collections.Generic.List<Bond> doubleBonds;

        public SmilesVisitor()
        {
            retVal = new Molecule();
            ringAtoms = new System.Collections.Hashtable();
            ringbonds = new System.Collections.Hashtable();
            cisTrans = string.Empty;
            doubleBonds = new List<Bond>();
            addExplicitHToNext = false;
        }


        public override object Visit(IParseTree tree)
        {
            for (int i = 0; i < tree.ChildCount; i++)
            {
                IParseTree item = tree.GetChild(i);
                if (typeof(smilesParser.ChainContext).IsAssignableFrom(item.GetType()))
                    VisitChain((smilesParser.ChainContext)item);
                // else throw new System.NotImplementedException("String not a chain.");
            }
            return retVal.Atoms;
        }

        public override object VisitAromatic(smilesParser.AromaticContext context)
        {
            return new Atom(context.GetChild(0).GetText().ToUpper(), AtomType.AROMATIC);
        }

        public override object VisitAtom([NotNull] smilesParser.AtomContext context)
        {
            string symbol = string.Empty;
            Chirality ch = Chirality.UNSPECIFIED;
            int isotope = 0;
            int hCount = 0;
            int charge = 0;
            int atomClass = 0;
            foreach (IParseTree tree in context.children)
            {
                if (typeof(smilesParser.AromaticContext).IsAssignableFrom(tree.GetType()))
                {
                    Atom a = (Atom)VisitAromatic((smilesParser.AromaticContext)tree);
                    a.AtomType = ChemInfo.AtomType.AROMATIC;
                    return a;
                }
                else if (typeof(smilesParser.OrganicContext).IsAssignableFrom(tree.GetType()))
                {
                    Atom a = (Atom)VisitOrganic((smilesParser.OrganicContext)tree);
                    a.AtomType = ChemInfo.AtomType.ORGANIC;
                    return a;
                }
                else if (typeof(smilesParser.HalogenContext).IsAssignableFrom(tree.GetType()))
                {
                    Atom a = (Atom)VisitHalogen((smilesParser.HalogenContext)tree);
                    a.AtomType = ChemInfo.AtomType.ORGANIC;
                    return a;
                }
                else if (typeof(smilesParser.WildcardContext).IsAssignableFrom(tree.GetType()))
                {
                    Atom a = (Atom)VisitWildcard((smilesParser.WildcardContext)tree);
                    a.AtomType = ChemInfo.AtomType.WILDCARD;
                    return a;
                }
                else
                {
                    if (typeof(smilesParser.SymbolContext).IsAssignableFrom(tree.GetType()))
                    {
                        symbol = (string)VisitSymbol((smilesParser.SymbolContext)tree);
                    }
                    if (typeof(smilesParser.ChiralContext).IsAssignableFrom(tree.GetType()))
                    {
                        ch = (Chirality)VisitChiral((smilesParser.ChiralContext)tree);
                    }
                    if (typeof(smilesParser.IsotopeContext).IsAssignableFrom(tree.GetType()))
                    {
                        isotope = (int)VisitIsotope((smilesParser.IsotopeContext)tree);
                    }
                    if (typeof(smilesParser.HcountContext).IsAssignableFrom(tree.GetType()))
                    {
                        hCount = (int)VisitHcount((smilesParser.HcountContext)tree);
                    }
                    if (typeof(smilesParser.ChargeContext).IsAssignableFrom(tree.GetType()))
                    {
                        charge = (int)VisitCharge((smilesParser.ChargeContext)tree);
                    }
                    if (typeof(smilesParser.AtomclassContext).IsAssignableFrom(tree.GetType()))
                    {
                        atomClass = (int)VisitAtomclass((smilesParser.AtomclassContext)tree);
                    }
                }
            }
            if (addExplicitHToNext)
            {
                hCount++;
                this.addExplicitHToNext = false;
            }
            string[] organics = { "B", "C", "N", "O", "S", "P", "F", "Cl", "Br", "I", "X" };
            string[] aromatics = { "b", "c", "n", "o", "p", "s", "se", "as" };
            AtomType type = AtomType.NONE;
            if (organics.Contains(symbol)) type = AtomType.ORGANIC;
            else if (aromatics.Contains(symbol)) type = AtomType.AROMATIC;
            return new Atom(symbol, type, isotope, ch, hCount, charge, atomClass);
        }

        public override object VisitAtomclass([NotNull] smilesParser.AtomclassContext context)
        {
            int.TryParse(context.GetChild(1).GetText(), out int retVal);
            return retVal;
        }

        public override object VisitBond([NotNull] smilesParser.BondContext context)
        {
            if (context.GetText() == "-") return BondType.Single;
            else if (context.GetText() == "=")
            {
                return BondType.Double;
            }
            else if (context.GetText() == "#") return BondType.Triple;
            else if (context.GetText() == "$") return BondType.Any;
            else if (context.GetText() == "\\")
            {
                if (string.IsNullOrEmpty(cisTrans)) cisTrans = context.GetText();
                else
                {
                    if (context.GetText() == cisTrans)
                    {
                        if (((doubleBonds.Count - 1) % 2) == 0)
                            foreach (Bond b in doubleBonds)
                                b.Stereo = BondStereo.trans;
                    }
                    else
                    {
                        if (((doubleBonds.Count - 1) % 2) == 0)
                            foreach (Bond b in doubleBonds)
                                b.Stereo = BondStereo.cis;
                    }
                    cisTrans = string.Empty;
                    doubleBonds.Clear();
                }
            }
            else if (context.GetText() == "/")
            {
                if (string.IsNullOrEmpty(cisTrans)) cisTrans = context.GetText();
                else
                {
                    if (context.GetText() == cisTrans)
                    {
                        if (((doubleBonds.Count - 1) % 2) == 0)
                            foreach (Bond b in doubleBonds)
                                b.Stereo = BondStereo.trans;
                    }
                    else
                    {
                        if (((doubleBonds.Count - 1) % 2) == 0)
                            foreach (Bond b in doubleBonds)
                                b.Stereo = BondStereo.cis;
                    }
                    cisTrans = string.Empty;
                    doubleBonds.Clear();
                }
            }
            return BondType.Single;
        }

        //public override object VisitBracket_atom(smilesParser.Bracket_atomContext context)
        //{
        //    string symbol = string.Empty;
        //    Chirality ch = Chirality.UNSPECIFIED;
        //    int isotope = 0;
        //    int hCount = 0;
        //    int charge = 0;
        //    int atomClass = 0;
        //    foreach (IParseTree tree in context.children)
        //    {
        //        if (typeof(smilesParser.SymbolContext).IsAssignableFrom(tree.GetType()))
        //        {
        //            symbol = (string)VisitSymbol((smilesParser.SymbolContext)tree);
        //        }
        //        if (typeof(smilesParser.ChiralContext).IsAssignableFrom(tree.GetType()))
        //        {
        //            ch = (Chirality)VisitChiral((smilesParser.ChiralContext)tree);
        //        }
        //        if (typeof(smilesParser.IsotopeContext).IsAssignableFrom(tree.GetType()))
        //        {
        //            isotope = (int)VisitIsotope((smilesParser.IsotopeContext)tree);
        //        }
        //        if (typeof(smilesParser.HcountContext).IsAssignableFrom(tree.GetType()))
        //        {
        //            hCount = (int)VisitHcount((smilesParser.HcountContext)tree);
        //        }
        //        if (typeof(smilesParser.ChargeContext).IsAssignableFrom(tree.GetType()))
        //        {
        //            charge = (int)VisitCharge((smilesParser.ChargeContext)tree);
        //        }
        //        if (typeof(smilesParser.AtomclassContext).IsAssignableFrom(tree.GetType()))
        //        {
        //            atomClass = (int)VisitAtomclass((smilesParser.AtomclassContext)tree);
        //        }
        //    }
        //    return new Atom(symbol, AtomType.NONE, isotope, ch, hCount, charge, atomClass);
        //}

        public override object VisitBranch([NotNull] smilesParser.BranchContext context)
        {
            if (context.ChildCount == 3)
            {
                if (typeof(smilesParser.ChainContext).IsAssignableFrom(context.GetChild(1).GetType()))
                    return VisitChain((smilesParser.ChainContext)context.GetChild(1));
                return VisitBranch((smilesParser.BranchContext)context.GetChild(1));
            }
            if (typeof(smilesParser.BondContext).IsAssignableFrom(context.GetChild(1).GetType()))
            {
                if (context.GetChild(1).GetText() == "\\")
                {
                    if (string.IsNullOrEmpty(cisTrans)) cisTrans = "/";
                    else
                    {
                        if (context.GetText() == cisTrans)
                        {
                            if (((doubleBonds.Count - 1) % 2) == 0)
                                foreach (Bond b in doubleBonds)
                                    b.Stereo = BondStereo.cis;
                        }
                        else
                        {
                            if (((doubleBonds.Count - 1) % 2) == 0)
                                foreach (Bond b in doubleBonds)
                                    b.Stereo = BondStereo.trans;
                        }
                        cisTrans = string.Empty;
                        doubleBonds.Clear();
                    }
                }
                else if (context.GetChild(1).GetText() == "/")
                {
                    if (string.IsNullOrEmpty(cisTrans)) cisTrans = "\\";
                    else
                    {
                        if (context.GetText() == cisTrans)
                        {
                            if (((doubleBonds.Count - 1) % 2) == 0)
                                foreach (Bond b in doubleBonds)
                                    b.Stereo = BondStereo.cis;
                        }
                        else
                        {
                            if (((doubleBonds.Count - 1) % 2) == 0)
                                foreach (Bond b in doubleBonds)
                                    b.Stereo = BondStereo.trans;
                        }
                        cisTrans = string.Empty;
                        doubleBonds.Clear();
                    }
                }
                else nextBond = (BondType)VisitBond((smilesParser.BondContext)context.GetChild(1));
                return VisitChain((smilesParser.ChainContext)context.GetChild(2));
            }
            nextBond = (BondType)VisitDot((smilesParser.DotContext)context.GetChild(1));
            return VisitChain((smilesParser.ChainContext)context.GetChild(2));
        }

        public override object VisitBranched_atom([NotNull] smilesParser.Branched_atomContext context)
        {
            foreach (IParseTree tree in context.children)
            {
                if (typeof(smilesParser.AtomContext).IsAssignableFrom(tree.GetType()))
                {
                    Atom current = (Atom)VisitAtom((smilesParser.AtomContext)tree);
                    if (current.Element != ELEMENTS.H)
                    {
                        retVal.AddAtom(current);
                        if (addExplicitHToNext)
                        {
                            current.ExplicitHydrogens++;
                            addExplicitHToNext = false;
                        }
                        if (last != null)
                        {
                            if (last.AtomType == AtomType.AROMATIC && current.AtomType == AtomType.AROMATIC) nextBond = BondType.Aromatic;
                        }
                        Bond b = retVal.AddBond(last, current, nextBond, BondStereo.NotStereoOrUseXYZ, BondTopology.Either, BondReactingCenterStatus.Unmarked);
                        if (!string.IsNullOrEmpty(cisTrans) && nextBond == BondType.Double) doubleBonds.Add(b);
                        nextBond = BondType.Single;
                        last = current;
                    }
                    else
                    {
                        if (last != null) last.ExplicitHydrogens++;
                        else addExplicitHToNext = true;
                    }
                }
                else if (typeof(smilesParser.RingbondContext).IsAssignableFrom(tree.GetType()))
                {
                    int ring = (int)VisitRingbond((smilesParser.RingbondContext)tree);
                    if (ringAtoms.ContainsKey(ring))
                    {
                        BondType nextType = (BondType)ringbonds[ring];
                        if (nextType != BondType.Single && (nextBond == BondType.Single || nextBond == nextType)) nextBond = nextType;
                        Bond b = retVal.AddBond(last, (Atom)ringAtoms[ring], nextBond, BondStereo.NotStereoOrUseXYZ, BondTopology.Either, BondReactingCenterStatus.Unmarked);
                        if (!string.IsNullOrEmpty(cisTrans) && nextBond == BondType.Double) doubleBonds.Add(b);
                        nextBond = BondType.Single;
                        ringAtoms.Remove(ring);
                        ringbonds.Remove(ring);
                    }
                    else
                    {
                        ringAtoms.Add(ring, last);
                        ringbonds.Add(ring, nextBond);
                        nextBond = BondType.Single;
                    }
                }
                else if (typeof(smilesParser.BranchContext).IsAssignableFrom(tree.GetType()))
                {
                    Atom temp = last;
                    VisitBranch((smilesParser.BranchContext)tree);
                    last = temp;
                }
                else if (typeof(smilesParser.BondContext).IsAssignableFrom(tree.GetType()))
                {
                    nextBond = (BondType)VisitBond((smilesParser.BondContext)context.GetChild(1));
                }
                else
                    throw new System.InvalidOperationException("uhoh");
            }
            return null;
        }

        public override object VisitChain([NotNull] smilesParser.ChainContext context)
        {
            foreach (IParseTree tree in context.children)
            {
                if (typeof(smilesParser.ChainContext).IsAssignableFrom(tree.GetType()))
                    VisitChain((smilesParser.ChainContext)tree);
                else if (typeof(smilesParser.Branched_atomContext).IsAssignableFrom(tree.GetType()))
                {
                    VisitBranched_atom((smilesParser.Branched_atomContext)tree);
                }
                else if (typeof(smilesParser.BondContext).IsAssignableFrom(tree.GetType()))
                    nextBond = (BondType)VisitBond((smilesParser.BondContext)tree);
                else if (typeof(smilesParser.DotContext).IsAssignableFrom(tree.GetType()))
                    nextBond = (BondType)VisitDot((smilesParser.DotContext)tree);
                else nextBond = BondType.Any;
            }
            return null;
        }

        public override object VisitCharge([NotNull] smilesParser.ChargeContext context)
        {
            int retVal = 0;
            if (context.GetText() == "+")
                retVal = 1;
            else if (context.GetText() == "++")
                retVal = 2;
            else if (context.GetText() == "-")
                retVal = -1;
            else if (context.GetText() == "--")
                retVal = -2;
            else
                int.TryParse(context.GetText(), out retVal);
            return retVal;
        }

        public override object VisitChiral([NotNull] smilesParser.ChiralContext context)
        {
            Chirality retVal = Chirality.UNSPECIFIED;
            if (context.GetText() == "@") retVal = Chirality.TETRAHEDRAL_COUNTER_CLOCKWISE;
            else if (context.GetText() == "@@") retVal = Chirality.TETRAHEDRAL_CLOCKWISE;
            return retVal;
        }

        public override object VisitDot([NotNull] smilesParser.DotContext context)
        {
            return BondType.Disconnected;
        }

        public override object VisitHcount([NotNull] smilesParser.HcountContext context)
        {
            int retVal = 1;
            if (context.ChildCount == 2)
                int.TryParse(context.GetChild(1).GetText(), out retVal);
            return retVal;
        }

        public override object VisitIsotope(smilesParser.IsotopeContext context)
        {
            int.TryParse(context.GetText(), out int retVal);
            return retVal;
        }

        public override object VisitOrganic([NotNull] smilesParser.OrganicContext context)
        {
            return new Atom(context.GetChild(0).GetText(), AtomType.ORGANIC);
        }

        public override object VisitHalogen([NotNull] smilesParser.HalogenContext context)
        {
            return new Atom(context.GetChild(0).GetText(), AtomType.ORGANIC);
        }

        public override object VisitWildcard([NotNull] smilesParser.WildcardContext context)
        {
            return new Atom(context.GetChild(0).GetText(), AtomType.WILDCARD);
        }

        public override object VisitRingbond([NotNull] smilesParser.RingbondContext context)
        {
            int.TryParse(context.GetText(), out int retVal);
            return retVal;
        }

        public override object VisitSmiles([NotNull] smilesParser.SmilesContext context)
        {
            Molecule mol = new Molecule();
            foreach (IParseTree tree in context.children)
            {
                VisitChain((smilesParser.ChainContext)tree);
            }
            return mol;
        }

        public override object VisitSymbol([NotNull] smilesParser.SymbolContext context)
        {
            return context.GetText();
        }

        //public override Molecule VisitWildcard([NotNull] smilesParser.WildcardContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public override Molecule VisitTerminal(ITerminalNode node)
        //{
        //    throw new NotImplementedException();
        //}
    }

}
