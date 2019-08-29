using System.Collections.Generic;
using System.Linq;

namespace SustainableChemistryWeb.ChemInfo
{
    /*------------------------------------------------------------
    * state.h
    * Definition of an abstract class representing a state of the 
    * matching process between two ARGs.
    * See: argraph.h 
     *
    * Author: P. Foggia
    * $Id: state.h,v 1.3 1998/09/29 09:50:16 foggia Exp $
    *-----------------------------------------------------------------*/
    /*----------------------------------------------------------
    * class State
    * An abstract representation of the SSR current state.
    * NOTE: Respect to pre-2.0 version of the library, class
    *   State assumes explicitly a depth-first search. The
    *   BackTrack method has been added to allow a state 
    *   to clean up things before reverting to its parent. This
    *   can be used, for instance, for sharing resources between
    *   the parent and the child. The BackTrack implementation
    *   can safely assume that at most one AddPair has been
    *   performed on the state.
    ---------------------------------------------------------*/
    public abstract class State2
    {
        public abstract State2 Clone();
        public abstract Molecule GetMolecule1();
        public abstract Molecule GetMolecule2();
        public abstract bool NextPair(ref Atom nextAtom1, ref Atom nextAtom2, Atom prevAtom1 = null, Atom prevAtom2 = null);
        public abstract bool IsFeasiblePair(Atom atom1, Atom atom2);
        public abstract void AddPair(Atom atom1, Atom atom2);
        public abstract bool IsGoal();
        public abstract bool IsDead();
        public abstract int CoreLen();
        public abstract void GetCoreSet(ref Atom[] a1, ref Atom[] a2);
        public abstract void BackTrack();
    }

    public abstract class State
    {

        //  public:
        //virtual ~State() { }
        public abstract Atom[] GetGraph1();
        public abstract Atom[] GetGraph2();
        public abstract bool NextPair(ref int pn1, ref int pn2,
                      int prev_n1, int prev_n2);
        public abstract bool IsFeasiblePair(int n1, int n2);
        public abstract void AddPair(int n1, int n2);
        public abstract bool IsGoal();
        public abstract bool IsDead();
        public abstract int CoreLen();
        public abstract void GetCoreSet(ref int[] c1, ref int[] c2);
        public abstract State Clone();  // Changed clone to Clone for uniformity

        public abstract void BackTrack();
    };

    /*------------------------------------------------------------------
     * * vf2_sub_state.cc
     * * Implementation of the class VF2SubState
     * *
     * * Author: P. Foggia
     * *-----------------------------------------------------------------*/


    /*-----------------------------------------------------------------
     * NOTE: 
     *   The attribute compatibility check (methods CompatibleNode
     *   and CompatibleEdge of ARGraph) is always performed
     *   applying the method to g1, and passing the attribute of
     *   g1 as first argument, and the attribute of g2 as second
     *   argument. This may be important if the compatibility
     *   criterion is not symmetric.
     -----------------------------------------------------------------*/


    /*---------------------------------------------------------
     *   IMPLEMENTATION NOTES:
     * The six vectors core_1, core_2, in_1, in_2, out_1, out_2, 
     * are shared among the instances of this class; they are
     * owned by the instance with core_len==0 (the root of the
     * SSR).
     * In the vectors in_* and out_* there is a value indicating 
     * the level at which the corresponding node became a member
     * of the core or of Tin (for in_*) or Tout (for out_*),
     * or 0 if the node does not belong to the set.
     * This information is used for backtracking.
     * The fields t1out_len etc. also count the nodes in core.
     * The true t1out_len is t1out_len-core_len!
     ---------------------------------------------------------*/


    /*----------------------------------------------------------
     * class VF2SubState
     * A representation of the SSR current state
     * See vf2_sub_state.cc for more details.
     ---------------------------------------------------------*/
    public class FunctionalGroupState : State
    { //typedef ARGraph_impl Graph;

        const int NULL_NODE = -1;
        private int core_len, orig_core_len;
        private int added_node1;
        private int t1both_len, t2both_len, t1in_len, t1out_len,
        t2in_len, t2out_len; // Core nodes are also counted by these...
        private int[] core_1;
        private int[] core_2;
        private int[] in_1;
        private int[] in_2;
        private int[] out_1;
        private int[] out_2;
        private int[] order;

        Molecule g1, g2;
        int n1, n2;

        int share_count;

        /*----------------------------------------------------------
         * Methods of the class VF2SubState
         ---------------------------------------------------------*/

        /*----------------------------------------------------------
         * VF2SubState::VF2SubState(g1, g2, sortNodes)
         * Constructor. Makes an empty state.
         * If sortNodes is true, computes an initial ordering
         * for the nodes based on the frequency of their valence.
         ---------------------------------------------------------*/
        public FunctionalGroupState(Molecule ag1, Molecule ag2, bool sortNodes)
        {
            g1 = ag1;
            g2 = ag2;
            n1 = g1.Atoms.Length;
            n2 = g2.Atoms.Length;

            //if (sortNodes)
            //    order = SortNodesByFrequency(ag1);
            //else
            //    order = NULL;

            core_len = orig_core_len = 0;
            t1both_len = t1in_len = t1out_len = 0;
            t2both_len = t2in_len = t2out_len = 0;

            added_node1 = NULL_NODE;

            core_1 = new int[n1];
            core_2 = new int[n2];
            in_1 = new int[n1];
            in_2 = new int[n2];
            out_1 = new int[n1];
            out_2 = new int[n2];
            //share_count = 0;
            //if (!core_1 || !core_2 || !in_1 || !in_2
            //    || !out_1 || !out_2 || !share_count)
            //    error("Out of memory");

            int i;
            for (i = 0; i < n1; i++)
            {
                core_1[i] = NULL_NODE;
                in_1[i] = 0;
                out_1[i] = 0;
            }
            for (i = 0; i < n2; i++)
            {
                core_2[i] = NULL_NODE;
                in_2[i] = 0;
                out_2[i] = 0;
            }


            share_count = 1;
        }

        public override bool IsGoal()
        {
            return core_len == n1;
        }

        public override bool IsDead()
        {
            return n1 > n2 ||
               t1both_len > t2both_len ||
               t1out_len > t2out_len ||
               t1in_len > t2in_len;
        }

        public override int CoreLen()
        {
            return core_len;
        }

        /*----------------------------------------------------------
         * VF2SubState::VF2SubState(state)
         * Copy constructor. 
         ---------------------------------------------------------*/
        FunctionalGroupState(FunctionalGroupState state)
        {
            g1 = state.g1;
            g2 = state.g2;
            n1 = state.n1;
            n2 = state.n2;

            order = state.order;

            core_len = orig_core_len = state.core_len;
            t1in_len = state.t1in_len;
            t1out_len = state.t1out_len;
            t1both_len = state.t1both_len;
            t2in_len = state.t2in_len;
            t2out_len = state.t2out_len;
            t2both_len = state.t2both_len;

            added_node1 = NULL_NODE;

            core_1 = state.core_1;
            core_2 = state.core_2;
            in_1 = state.in_1;
            in_2 = state.in_2;
            out_1 = state.out_1;
            out_2 = state.out_2;
            share_count = state.share_count;

            ++share_count;

        }


        /*---------------------------------------------------------------
         * VF2SubState::~VF2SubState()
         * Destructor.
         --------------------------------------------------------------*/
        //VF2SubState::~VF2SubState()
        //{
        //    if (--*share_count == 0)
        //    {
        //        delete[] core_1;
        //        delete[] core_2;
        //        delete[] in_1;
        //        delete[] out_1;
        //        delete[] in_2;
        //        delete[] out_2;
        //        delete share_count;
        //        delete[] order;
        //    }
        //}

        public override Atom[] GetGraph1()
        {
            return g1.Atoms;
        }

        public override Atom[] GetGraph2()
        {
            return g2.Atoms;
        }

        /*--------------------------------------------------------------------------
         * bool VF2SubState::NextPair(pn1, pn2, prev_n1, prev_n2)
         * Puts in *pn1, *pn2 the next pair of nodes to be tried.
         * prev_n1 and prev_n2 must be the last nodes, or NULL_NODE (default)
         * to start from the first pair.
         * Returns false if no more pairs are available.
         -------------------------------------------------------------------------*/
        public override bool NextPair(ref int pn1, ref int pn2,
                     int prev_n1, int prev_n2)
        {
            if (prev_n1 == NULL_NODE)
                prev_n1 = 0;
            if (prev_n2 == NULL_NODE)
                prev_n2 = 0;
            else
                prev_n2++;

            if (t1both_len > core_len && t2both_len > core_len)
            {
                while (prev_n1 < n1 &&
                   (core_1[prev_n1] != NULL_NODE || out_1[prev_n1] == 0
                            || in_1[prev_n1] == 0))
                {
                    prev_n1++;
                    prev_n2 = 0;
                }
            }
            else if (t1out_len > core_len && t2out_len > core_len)
            {
                while (prev_n1 < n1 &&
                   (core_1[prev_n1] != NULL_NODE || out_1[prev_n1] == 0))
                {
                    prev_n1++;
                    prev_n2 = 0;
                }
            }
            else if (t1in_len > core_len && t2in_len > core_len)
            {
                while (prev_n1 < n1 &&
                   (core_1[prev_n1] != NULL_NODE || in_1[prev_n1] == 0))
                {
                    prev_n1++;
                    prev_n2 = 0;
                }
            }
            else if (prev_n1 == 0 && order != null)
            {
                int i = 0;
                while (i < n1 && core_1[prev_n1 = order[i]] != NULL_NODE)
                    i++;
                if (i == n1)
                    prev_n1 = n1;
            }
            else
            {
                while (prev_n1 < n1 && core_1[prev_n1] != NULL_NODE)
                {
                    prev_n1++;
                    prev_n2 = 0;
                }
            }



            if (t1both_len > core_len && t2both_len > core_len)
            {
                while (prev_n2 < n2 &&
                   (core_2[prev_n2] != NULL_NODE || out_2[prev_n2] == 0
                            || in_2[prev_n2] == 0))
                {
                    prev_n2++;
                }
            }
            else if (t1out_len > core_len && t2out_len > core_len)
            {
                while (prev_n2 < n2 &&
                   (core_2[prev_n2] != NULL_NODE || out_2[prev_n2] == 0))
                {
                    prev_n2++;
                }
            }
            else if (t1in_len > core_len && t2in_len > core_len)
            {
                while (prev_n2 < n2 &&
                   (core_2[prev_n2] != NULL_NODE || in_2[prev_n2] == 0))
                {
                    prev_n2++;
                }
            }
            else
            {
                while (prev_n2 < n2 && core_2[prev_n2] != NULL_NODE)
                {
                    prev_n2++;
                }
            }


            if (prev_n1 < n1 && prev_n2 < n2)
            {
                pn1 = prev_n1;
                pn2 = prev_n2;
                return true;
            }

            return false;
        }



        /*---------------------------------------------------------------
         * bool VF2SubState::IsFeasiblePair(node1, node2)
         * Returns true if (node1, node2) can be added to the state
         * NOTE: 
         *   The attribute compatibility check (methods CompatibleNode
         *   and CompatibleEdge of ARGraph) is always performed
         *   applying the method to g1, and passing the attribute of
         *   g1 as first argument, and the attribute of g2 as second
         *   argument. This may be important if the compatibility
         *   criterion is not symmetric.
         --------------------------------------------------------------*/
        public override bool IsFeasiblePair(int node1, int node2)
        {
            //assert(node1 < n1);
            //assert(node2 < n2);
            //assert(core_1[node1] == NULL_NODE);
            //assert(core_2[node2] == NULL_NODE);

            if (!g1.CompatibleNode(g1.GetNodeAttr(node1), g2.GetNodeAttr(node2)))
                return false;

            //if (!g1.CompatibleNode(g1.GetNodeDegree(node1), g2.GetNodeDegree(node2)))
            //    return false;

            int i, other1, other2;
            Bond attr1 = null;
            int termout1 = 0, termout2 = 0, termin1 = 0, termin2 = 0, new1 = 0, new2 = 0;

            // Check the 'out' edges of node1
            for (i = 0; i < g1.OutEdgeCount(node1); i++)
            {
                other1 = g1.GetOutEdge(node1, i, ref attr1);
                if (core_1[other1] != NULL_NODE)
                {
                    other2 = core_1[other1];
                    if (!g2.HasEdge(node2, other2) ||
                        !g1.CompatibleEdge(attr1, g2.GetEdgeAttr(node2, other2)))
                        return false;
                }
                else
                {
                    if (in_1[other1] != 0)
                        termin1++;
                    if (out_1[other1] != 0)
                        termout1++;
                    if (in_1[other1] == 0 && out_1[other1] == 0)
                        new1++;
                }
            }

            // Check the 'in' edges of node1
            for (i = 0; i < g1.InEdgeCount(node1); i++)
            {
                other1 = g1.GetInEdge(node1, i, ref attr1);
                if (core_1[other1] != NULL_NODE)
                {
                    other2 = core_1[other1];
                    if (!g2.HasEdge(other2, node2) ||
                        !g1.CompatibleEdge(attr1, g2.GetEdgeAttr(other2, node2)))
                        return false;
                }
                else
                {
                    if (in_1[other1] != 0)
                        termin1++;
                    if (out_1[other1] != 0)
                        termout1++;
                    if (in_1[other1] == 0 && out_1[other1] == 0)
                        new1++;
                }
            }


            // Check the 'out' edges of node2
            for (i = 0; i < g2.OutEdgeCount(node2); i++)
            {
                other2 = g2.GetOutEdge(node2, i);
                if (core_2[other2] != NULL_NODE)
                {
                    other1 = core_2[other2];
                    if (!g1.HasEdge(node1, other1))
                        return false;
                }
                else
                {
                    if (in_2[other2] != 0)
                        termin2++;
                    if (out_2[other2] != 0)
                        termout2++;
                    if (in_2[other2] == 0 && out_2[other2] == 0)
                        new2++;
                }
            }

            // Check the 'in' edges of node2
            for (i = 0; i < g2.InEdgeCount(node2); i++)
            {
                other2 = g2.GetInEdge(node2, i);
                if (core_2[other2] != NULL_NODE)
                {
                    other1 = core_2[other2];
                    if (!g1.HasEdge(other1, node1))
                        return false;
                }
                else
                {
                    if (in_2[other2] != 0)
                        termin2++;
                    if (out_2[other2] != 0)
                        termout2++;
                    if (in_2[other2] == 0 && out_2[other2] == 0)
                        new2++;
                }
            }

            return termin1 <= termin2 && termout1 <= termout2 && new1 <= new2;
        }



        /*--------------------------------------------------------------
         * void VF2SubState::AddPair(node1, node2)
         * Adds a pair to the Core set of the state.
         * Precondition: the pair must be feasible
         -------------------------------------------------------------*/
        public override void AddPair(int node1, int node2)
        {
            //assert(node1 < n1);
            //assert(node2 < n2);
            //assert(core_len < n1);
            //assert(core_len < n2);

            core_len++;
            added_node1 = node1;

            if (in_1[node1] == 0)
            {
                in_1[node1] = core_len;
                t1in_len++;
                if (out_1[node1] != 0)
                    t1both_len++;
            }
            if (out_1[node1] == 0)
            {
                out_1[node1] = core_len;
                t1out_len++;
                if (in_1[node1] != 0)
                    t1both_len++;
            }

            if (in_2[node2] == 0)
            {
                in_2[node2] = core_len;
                t2in_len++;
                if (out_2[node2] != 0)
                    t2both_len++;
            }
            if (out_2[node2] == 0)
            {
                out_2[node2] = core_len;
                t2out_len++;
                if (in_2[node2] != 0)
                    t2both_len++;
            }

            core_1[node1] = node2;
            core_2[node2] = node1;


            int i, other;
            for (i = 0; i < g1.InEdgeCount(node1); i++)
            {
                other = g1.GetInEdge(node1, i);
                if (in_1[other] == 0)
                {
                    in_1[other] = core_len;
                    t1in_len++;
                    if (out_1[other] != 0)
                        t1both_len++;
                }
            }

            for (i = 0; i < g1.OutEdgeCount(node1); i++)
            {
                other = g1.GetOutEdge(node1, i);
                if (out_1[other] == 0)
                {
                    out_1[other] = core_len;
                    t1out_len++;
                    if (in_1[other] != 0)
                        t1both_len++;
                }
            }

            for (i = 0; i < g2.InEdgeCount(node2); i++)
            {
                other = g2.GetInEdge(node2, i);
                if (in_2[other] == 0)
                {
                    in_2[other] = core_len;
                    t2in_len++;
                    if (out_2[other] != 0)
                        t2both_len++;
                }
            }

            for (i = 0; i < g2.OutEdgeCount(node2); i++)
            {
                other = g2.GetOutEdge(node2, i);
                if (out_2[other] == 0)
                {
                    out_2[other] = core_len;
                    t2out_len++;
                    if (in_2[other] != 0)
                        t2both_len++;
                }
            }

        }



        /*--------------------------------------------------------------
         * void VF2SubState::GetCoreSet(c1, c2)
         * Reads the core set of the state into the arrays c1 and c2.
         * The i-th pair of the mapping is (c1[i], c2[i])
         --------------------------------------------------------------*/
        public override void GetCoreSet(ref int[] c1, ref int[] c2)
        {
            List<int> temp1 = new List<int>();
            List<int> temp2 = new List<int>();
            for (int i = 0; i < n1; i++)
                if (core_1[i] != NULL_NODE)
                {
                    temp1.Add(i);
                    temp2.Add(core_1[i]);
                }
            c1 = temp1.ToArray<int>();
            c2 = temp2.ToArray<int>();
        }


        /*----------------------------------------------------------------
         * Clones a VF2SubState, allocating with new the clone.
         --------------------------------------------------------------*/
        public override State Clone()
        {
            return new FunctionalGroupState(this);
        }

        /*----------------------------------------------------------------
         * Undoes the changes to the shared vectors made by the 
         * current state. Assumes that at most one AddPair has been
         * performed.
         ----------------------------------------------------------------*/
        public override void BackTrack()
        {
            //assert(core_len - orig_core_len <= 1);
            //assert(added_node1 != NULL_NODE);

            if (orig_core_len < core_len)
            {
                int i, node2;

                if (in_1[added_node1] == core_len)
                    in_1[added_node1] = 0;
                for (i = 0; i < g1.InEdgeCount(added_node1); i++)
                {
                    int other = g1.GetInEdge(added_node1, i);
                    if (in_1[other] == core_len)
                        in_1[other] = 0;
                }

                if (out_1[added_node1] == core_len)
                    out_1[added_node1] = 0;
                for (i = 0; i < g1.OutEdgeCount(added_node1); i++)
                {
                    int other = g1.GetOutEdge(added_node1, i);
                    if (out_1[other] == core_len)
                        out_1[other] = 0;
                }

                node2 = core_1[added_node1];

                if (in_2[node2] == core_len)
                    in_2[node2] = 0;
                for (i = 0; i < g2.InEdgeCount(node2); i++)
                {
                    int other = g2.GetInEdge(node2, i);
                    if (in_2[other] == core_len)
                        in_2[other] = 0;
                }

                if (out_2[node2] == core_len)
                    out_2[node2] = 0;
                for (i = 0; i < g2.OutEdgeCount(node2); i++)
                {
                    int other = g2.GetOutEdge(node2, i);
                    if (out_2[other] == core_len)
                        out_2[other] = 0;
                }

                core_1[added_node1] = NULL_NODE;
                core_2[node2] = NULL_NODE;

                core_len = orig_core_len;
                added_node1 = NULL_NODE;
            }

        }
    }

    /*----------------------------------------------------------
     * class VF2SubState
     * A representation of the SSR current state
     * See vf2_sub_state.cc for more details.
     ---------------------------------------------------------*/
    //public class VF2SubState : State
    //{ //typedef ARGraph_impl Graph;

    //    const int NULL_NODE = -1;
    //    private int core_len, orig_core_len;
    //    private int added_node1;
    //    private int t1both_len, t2both_len, t1in_len, t1out_len,
    //    t2in_len, t2out_len; // Core nodes are also counted by these...
    //    private int[] core_1;
    //    private int[] core_2;
    //    private int[] in_1;
    //    private int[] in_2;
    //    private int[] out_1;
    //    private int[] out_2;
    //    private int[] order;

    //    Molecule functionalGroup, testMolecule;
    //    int numAtomsinFunctionalGroup, numAtomsInTestMolecule;

    //    int share_count;

    //    /*----------------------------------------------------------
    //     * Methods of the class VF2SubState
    //     ---------------------------------------------------------*/

    //    /*----------------------------------------------------------
    //     * VF2SubState::VF2SubState(g1, g2, sortNodes)
    //     * Constructor. Makes an empty state.
    //     * If sortNodes is true, computes an initial ordering
    //     * for the nodes based on the frequency of their valence.
    //     ---------------------------------------------------------*/
    //    public VF2SubState(Molecule groupToFind, Molecule moleculeToSearch, bool sortNodes)
    //    {
    //        functionalGroup = groupToFind;
    //        testMolecule = moleculeToSearch;
    //        numAtomsinFunctionalGroup = functionalGroup.Atoms.Length;
    //        numAtomsInTestMolecule = testMolecule.Atoms.Length;

    //        //if (sortNodes)
    //        //    order = SortNodesByFrequency(ag1);
    //        //else
    //        //    order = NULL;

    //        core_len = orig_core_len = 0;
    //        t1both_len = t1in_len = t1out_len = 0;
    //        t2both_len = t2in_len = t2out_len = 0;

    //        added_node1 = NULL_NODE;

    //        core_1 = new int[numAtomsinFunctionalGroup];
    //        core_2 = new int[numAtomsInTestMolecule];
    //        in_1 = new int[numAtomsinFunctionalGroup];
    //        in_2 = new int[numAtomsInTestMolecule];
    //        out_1 = new int[numAtomsinFunctionalGroup];
    //        out_2 = new int[numAtomsInTestMolecule];
    //        //share_count = 0;
    //        //if (!core_1 || !core_2 || !in_1 || !in_2
    //        //    || !out_1 || !out_2 || !share_count)
    //        //    error("Out of memory");

    //        int i;
    //        for (i = 0; i < numAtomsinFunctionalGroup; i++)
    //        {
    //            core_1[i] = NULL_NODE;
    //            in_1[i] = 0;
    //            out_1[i] = 0;
    //        }
    //        for (i = 0; i < numAtomsInTestMolecule; i++)
    //        {
    //            core_2[i] = NULL_NODE;
    //            in_2[i] = 0;
    //            out_2[i] = 0;
    //        }


    //        share_count = 1;
    //    }

    //    public override bool IsGoal()
    //    {
    //        return core_len == numAtomsinFunctionalGroup;
    //    }

    //    public override bool IsDead()
    //    {
    //        return numAtomsinFunctionalGroup > numAtomsInTestMolecule ||
    //           t1both_len > t2both_len ||
    //           t1out_len > t2out_len ||
    //           t1in_len > t2in_len;
    //    }

    //    public override int CoreLen()
    //    {
    //        return core_len;
    //    }

    //    /*----------------------------------------------------------
    //     * VF2SubState::VF2SubState(state)
    //     * Copy constructor. 
    //     ---------------------------------------------------------*/
    //    VF2SubState(VF2SubState state)
    //    {
    //        functionalGroup = state.functionalGroup;
    //        testMolecule = state.testMolecule;
    //        numAtomsinFunctionalGroup = state.numAtomsinFunctionalGroup;
    //        numAtomsInTestMolecule = state.numAtomsInTestMolecule;

    //        order = state.order;

    //        core_len = orig_core_len = state.core_len;
    //        t1in_len = state.t1in_len;
    //        t1out_len = state.t1out_len;
    //        t1both_len = state.t1both_len;
    //        t2in_len = state.t2in_len;
    //        t2out_len = state.t2out_len;
    //        t2both_len = state.t2both_len;

    //        added_node1 = NULL_NODE;

    //        core_1 = state.core_1;
    //        core_2 = state.core_2;
    //        in_1 = state.in_1;
    //        in_2 = state.in_2;
    //        out_1 = state.out_1;
    //        out_2 = state.out_2;
    //        share_count = state.share_count;

    //        ++share_count;

    //    }


    //    /*---------------------------------------------------------------
    //     * VF2SubState::~VF2SubState()
    //     * Destructor.
    //     --------------------------------------------------------------*/
    //    //VF2SubState::~VF2SubState()
    //    //{
    //    //    if (--*share_count == 0)
    //    //    {
    //    //        delete[] core_1;
    //    //        delete[] core_2;
    //    //        delete[] in_1;
    //    //        delete[] out_1;
    //    //        delete[] in_2;
    //    //        delete[] out_2;
    //    //        delete share_count;
    //    //        delete[] order;
    //    //    }
    //    //}

    //    public override Atom[] GetGraph1()
    //    {
    //        return functionalGroup.Atoms;
    //    }

    //    public override Atom[] GetGraph2()
    //    {
    //        return testMolecule.Atoms;
    //    }

    //    /*--------------------------------------------------------------------------
    //     * bool VF2SubState::NextPair(pn1, pn2, prev_n1, prev_n2)
    //     * Puts in *pn1, *pn2 the next pair of nodes to be tried.
    //     * prev_n1 and prev_n2 must be the last nodes, or NULL_NODE (default)
    //     * to start from the first pair.
    //     * Returns false if no more pairs are available.
    //     -------------------------------------------------------------------------*/
    //    public override bool NextPair(ref int pn1, ref int pn2,
    //                 int prev_n1, int prev_n2)
    //    {
    //        if (prev_n1 == NULL_NODE)
    //            prev_n1 = 0;
    //        if (prev_n2 == NULL_NODE)
    //            prev_n2 = 0;
    //        else
    //            prev_n2++;

    //        if (t1both_len > core_len && t2both_len > core_len)
    //        {
    //            while (prev_n1 < numAtomsinFunctionalGroup &&
    //               (core_1[prev_n1] != NULL_NODE || out_1[prev_n1] == 0
    //                        || in_1[prev_n1] == 0))
    //            {
    //                prev_n1++;
    //                prev_n2 = 0;
    //            }
    //        }
    //        else if (t1out_len > core_len && t2out_len > core_len)
    //        {
    //            while (prev_n1 < numAtomsinFunctionalGroup &&
    //               (core_1[prev_n1] != NULL_NODE || out_1[prev_n1] == 0))
    //            {
    //                prev_n1++;
    //                prev_n2 = 0;
    //            }
    //        }
    //        else if (t1in_len > core_len && t2in_len > core_len)
    //        {
    //            while (prev_n1 < numAtomsinFunctionalGroup &&
    //               (core_1[prev_n1] != NULL_NODE || in_1[prev_n1] == 0))
    //            {
    //                prev_n1++;
    //                prev_n2 = 0;
    //            }
    //        }
    //        else if (prev_n1 == 0 && order != null)
    //        {
    //            int i = 0;
    //            while (i < numAtomsinFunctionalGroup && core_1[prev_n1 = order[i]] != NULL_NODE)
    //                i++;
    //            if (i == numAtomsinFunctionalGroup)
    //                prev_n1 = numAtomsinFunctionalGroup;
    //        }
    //        else
    //        {
    //            while (prev_n1 < numAtomsinFunctionalGroup && core_1[prev_n1] != NULL_NODE)
    //            {
    //                prev_n1++;
    //                prev_n2 = 0;
    //            }
    //        }



    //        if (t1both_len > core_len && t2both_len > core_len)
    //        {
    //            while (prev_n2 < numAtomsInTestMolecule &&
    //               (core_2[prev_n2] != NULL_NODE || out_2[prev_n2] == 0
    //                        || in_2[prev_n2] == 0))
    //            {
    //                prev_n2++;
    //            }
    //        }
    //        else if (t1out_len > core_len && t2out_len > core_len)
    //        {
    //            while (prev_n2 < numAtomsInTestMolecule &&
    //               (core_2[prev_n2] != NULL_NODE || out_2[prev_n2] == 0))
    //            {
    //                prev_n2++;
    //            }
    //        }
    //        else if (t1in_len > core_len && t2in_len > core_len)
    //        {
    //            while (prev_n2 < numAtomsInTestMolecule &&
    //               (core_2[prev_n2] != NULL_NODE || in_2[prev_n2] == 0))
    //            {
    //                prev_n2++;
    //            }
    //        }
    //        else
    //        {
    //            while (prev_n2 < numAtomsInTestMolecule && core_2[prev_n2] != NULL_NODE)
    //            {
    //                prev_n2++;
    //            }
    //        }


    //        if (prev_n1 < numAtomsinFunctionalGroup && prev_n2 < numAtomsInTestMolecule)
    //        {
    //            pn1 = prev_n1;
    //            pn2 = prev_n2;
    //            return true;
    //        }

    //        return false;
    //    }



    //    /*---------------------------------------------------------------
    //     * bool VF2SubState::IsFeasiblePair(node1, node2)
    //     * Returns true if (node1, node2) can be added to the state
    //     * NOTE: 
    //     *   The attribute compatibility check (methods CompatibleNode
    //     *   and CompatibleEdge of ARGraph) is always performed
    //     *   applying the method to g1, and passing the attribute of
    //     *   g1 as first argument, and the attribute of g2 as second
    //     *   argument. This may be important if the compatibility
    //     *   criterion is not symmetric.
    //     --------------------------------------------------------------*/
    //    public override bool IsFeasiblePair(int node1, int node2)
    //    {
    //        //assert(node1 < n1);
    //        //assert(node2 < n2);
    //        //assert(core_1[node1] == NULL_NODE);
    //        //assert(core_2[node2] == NULL_NODE);

    //        if (!functionalGroup.CompatibleNode(functionalGroup.GetNodeAttr(node1), testMolecule.GetNodeAttr(node2)))
    //            return false;

    //        int i, other1, other2;
    //        Bond attr1 = null;
    //        int termout1 = 0, termout2 = 0, termin1 = 0, termin2 = 0, new1 = 0, new2 = 0;

    //        // Check the 'out' edges of node1
    //        for (i = 0; i < functionalGroup.OutEdgeCount(node1); i++)
    //        {
    //            other1 = functionalGroup.GetOutEdge(node1, i, ref attr1);
    //            if (core_1[other1] != NULL_NODE)
    //            {
    //                other2 = core_1[other1];
    //                if (!testMolecule.HasEdge(node2, other2) ||
    //                    !functionalGroup.CompatibleEdge(attr1, testMolecule.GetEdgeAttr(node2, other2)))
    //                    return false;
    //            }
    //            else
    //            {
    //                if (in_1[other1] != 0)
    //                    termin1++;
    //                if (out_1[other1] != 0)
    //                    termout1++;
    //                if (in_1[other1] == 0 && out_1[other1] == 0)
    //                    new1++;
    //            }
    //        }

    //        // Check the 'in' edges of node1
    //        for (i = 0; i < functionalGroup.InEdgeCount(node1); i++)
    //        {
    //            other1 = functionalGroup.GetInEdge(node1, i, ref attr1);
    //            if (core_1[other1] != NULL_NODE)
    //            {
    //                other2 = core_1[other1];
    //                if (!testMolecule.HasEdge(other2, node2) ||
    //                    !functionalGroup.CompatibleEdge(attr1, testMolecule.GetEdgeAttr(other2, node2)))
    //                    return false;
    //            }
    //            else
    //            {
    //                if (in_1[other1] != 0)
    //                    termin1++;
    //                if (out_1[other1] != 0)
    //                    termout1++;
    //                if (in_1[other1] == 0 && out_1[other1] == 0)
    //                    new1++;
    //            }
    //        }


    //        // Check the 'out' edges of node2
    //        for (i = 0; i < testMolecule.OutEdgeCount(node2); i++)
    //        {
    //            other2 = testMolecule.GetOutEdge(node2, i);
    //            if (core_2[other2] != NULL_NODE)
    //            {
    //                other1 = core_2[other2];
    //                if (!functionalGroup.HasEdge(node1, other1))
    //                    return false;
    //            }
    //            else
    //            {
    //                if (in_2[other2] != 0)
    //                    termin2++;
    //                if (out_2[other2] != 0)
    //                    termout2++;
    //                if (in_2[other2] == 0 && out_2[other2] == 0)
    //                    new2++;
    //            }
    //        }

    //        // Check the 'in' edges of node2
    //        for (i = 0; i < testMolecule.InEdgeCount(node2); i++)
    //        {
    //            other2 = testMolecule.GetInEdge(node2, i);
    //            if (core_2[other2] != NULL_NODE)
    //            {
    //                other1 = core_2[other2];
    //                if (!functionalGroup.HasEdge(other1, node1))
    //                    return false;
    //            }
    //            else
    //            {
    //                if (in_2[other2] != 0)
    //                    termin2++;
    //                if (out_2[other2] != 0)
    //                    termout2++;
    //                if (in_2[other2] == 0 && out_2[other2] == 0)
    //                    new2++;
    //            }
    //        }

    //        return termin1 <= termin2 && termout1 <= termout2 && new1 <= new2;
    //    }



    //    /*--------------------------------------------------------------
    //     * void VF2SubState::AddPair(node1, node2)
    //     * Adds a pair to the Core set of the state.
    //     * Precondition: the pair must be feasible
    //     -------------------------------------------------------------*/
    //    public override void AddPair(int node1, int node2)
    //    {
    //        //assert(node1 < n1);
    //        //assert(node2 < n2);
    //        //assert(core_len < n1);
    //        //assert(core_len < n2);

    //        core_len++;
    //        added_node1 = node1;

    //        if (in_1[node1] == 0)
    //        {
    //            in_1[node1] = core_len;
    //            t1in_len++;
    //            if (out_1[node1] != 0)
    //                t1both_len++;
    //        }
    //        if (out_1[node1] == 0)
    //        {
    //            out_1[node1] = core_len;
    //            t1out_len++;
    //            if (in_1[node1] != 0)
    //                t1both_len++;
    //        }

    //        if (in_2[node2] == 0)
    //        {
    //            in_2[node2] = core_len;
    //            t2in_len++;
    //            if (out_2[node2] != 0)
    //                t2both_len++;
    //        }
    //        if (out_2[node2] == 0)
    //        {
    //            out_2[node2] = core_len;
    //            t2out_len++;
    //            if (in_2[node2] != 0)
    //                t2both_len++;
    //        }

    //        core_1[node1] = node2;
    //        core_2[node2] = node1;


    //        int i, other;
    //        for (i = 0; i < functionalGroup.InEdgeCount(node1); i++)
    //        {
    //            other = functionalGroup.GetInEdge(node1, i);
    //            if (in_1[other] == 0)
    //            {
    //                in_1[other] = core_len;
    //                t1in_len++;
    //                if (out_1[other] != 0)
    //                    t1both_len++;
    //            }
    //        }

    //        for (i = 0; i < functionalGroup.OutEdgeCount(node1); i++)
    //        {
    //            other = functionalGroup.GetOutEdge(node1, i);
    //            if (out_1[other] == 0)
    //            {
    //                out_1[other] = core_len;
    //                t1out_len++;
    //                if (in_1[other] != 0)
    //                    t1both_len++;
    //            }
    //        }

    //        for (i = 0; i < testMolecule.InEdgeCount(node2); i++)
    //        {
    //            other = testMolecule.GetInEdge(node2, i);
    //            if (in_2[other] == 0)
    //            {
    //                in_2[other] = core_len;
    //                t2in_len++;
    //                if (out_2[other] != 0)
    //                    t2both_len++;
    //            }
    //        }

    //        for (i = 0; i < testMolecule.OutEdgeCount(node2); i++)
    //        {
    //            other = testMolecule.GetOutEdge(node2, i);
    //            if (out_2[other] == 0)
    //            {
    //                out_2[other] = core_len;
    //                t2out_len++;
    //                if (in_2[other] != 0)
    //                    t2both_len++;
    //            }
    //        }

    //    }



    //    /*--------------------------------------------------------------
    //     * void VF2SubState::GetCoreSet(c1, c2)
    //     * Reads the core set of the state into the arrays c1 and c2.
    //     * The i-th pair of the mapping is (c1[i], c2[i])
    //     --------------------------------------------------------------*/
    //    public override void GetCoreSet(ref int[] c1, ref int[] c2)
    //    {
    //        List<int> temp1 = new List<int>();
    //        List<int> temp2 = new List<int>();
    //        int i;
    //        for (i = 0; i < numAtomsinFunctionalGroup; i++)
    //            if (core_1[i] != NULL_NODE)
    //            {
    //                temp1.Add(i);
    //                temp2.Add(core_1[i]);
    //            }
    //        c1 = temp1.ToArray<int>();
    //        c2 = temp2.ToArray<int>();
    //    }


    //    /*----------------------------------------------------------------
    //     * Clones a VF2SubState, allocating with new the clone.
    //     --------------------------------------------------------------*/
    //    public override State Clone()
    //    {
    //        return new VF2SubState(this);
    //    }

    //    /*----------------------------------------------------------------
    //     * Undoes the changes to the shared vectors made by the 
    //     * current state. Assumes that at most one AddPair has been
    //     * performed.
    //     ----------------------------------------------------------------*/
    //    public override void BackTrack()
    //    {
    //        //assert(core_len - orig_core_len <= 1);
    //        //assert(added_node1 != NULL_NODE);

    //        if (orig_core_len < core_len)
    //        {
    //            int i, node2;

    //            if (in_1[added_node1] == core_len)
    //                in_1[added_node1] = 0;
    //            for (i = 0; i < functionalGroup.InEdgeCount(added_node1); i++)
    //            {
    //                int other = functionalGroup.GetInEdge(added_node1, i);
    //                if (in_1[other] == core_len)
    //                    in_1[other] = 0;
    //            }

    //            if (out_1[added_node1] == core_len)
    //                out_1[added_node1] = 0;
    //            for (i = 0; i < functionalGroup.OutEdgeCount(added_node1); i++)
    //            {
    //                int other = functionalGroup.GetOutEdge(added_node1, i);
    //                if (out_1[other] == core_len)
    //                    out_1[other] = 0;
    //            }

    //            node2 = core_1[added_node1];

    //            if (in_2[node2] == core_len)
    //                in_2[node2] = 0;
    //            for (i = 0; i < testMolecule.InEdgeCount(node2); i++)
    //            {
    //                int other = testMolecule.GetInEdge(node2, i);
    //                if (in_2[other] == core_len)
    //                    in_2[other] = 0;
    //            }

    //            if (out_2[node2] == core_len)
    //                out_2[node2] = 0;
    //            for (i = 0; i < testMolecule.OutEdgeCount(node2); i++)
    //            {
    //                int other = testMolecule.GetOutEdge(node2, i);
    //                if (out_2[other] == core_len)
    //                    out_2[other] = 0;
    //            }

    //            core_1[added_node1] = NULL_NODE;
    //            core_2[node2] = NULL_NODE;

    //            core_len = orig_core_len;
    //            added_node1 = NULL_NODE;
    //        }

    //    }
    //}

    //public class VF2SubState2 : State2
    //{
    //    Molecule m1, m2;
    //    Atom addedAtom1;
    //    Atom addedAtom2;
    //    bool nodesSorted;
    //    Stack<Atom> core_1;
    //    Stack<Atom> core_2;
    //    List<Atom> terminal_1;
    //    List<Atom> terminal_2;
    //    //Stack<Atom> in_1;
    //    //Stack<Atom> in_2;
    //    //Stack<Atom> out_1;
    //    //Stack<Atom> out_2;
    //    //Stack<Atom> t_1;
    //    //Stack<Atom> t_2;
    //    //int t1both_len, t2both_len;

    //    public VF2SubState2(Molecule molecule1, Molecule molecule2, bool SortNodes)
    //    {
    //        m1 = molecule1;
    //        m2 = molecule2;
    //        //t1both_len = 0;
    //        //t2both_len = 0;
    //        core_1 = new Stack<Atom>();
    //        core_2 = new Stack<Atom>();
    //        terminal_1 = new List<Atom>();
    //        terminal_2 = new List<Atom>();
    //        //in_1 = new Stack<Atom>();
    //        //in_2 = new Stack<Atom>();
    //        //out_1 = new Stack<Atom>();
    //        //out_2 = new Stack<Atom>();
    //        //t_1 = new Stack<Atom>();
    //        //t_2 = new Stack<Atom>();
    //        addedAtom1 = null;
    //        nodesSorted = SortNodes;
    //    }

    //    public VF2SubState2(VF2SubState2 s)
    //    {
    //        VF2SubState2 vf2 = (VF2SubState2)s;
    //        m1 = s.GetMolecule1();
    //        m2 = s.GetMolecule2();
    //        //t1both_len = vf2.t1both_len;
    //        //t2both_len = vf2.t2both_len;
    //        core_1 = vf2.core_1;
    //        core_2 = vf2.core_2;
    //        terminal_1 = vf2.terminal_1;
    //        terminal_2 = vf2.terminal_2;
    //        //in_1 = vf2.in_1;
    //        //in_2 = vf2.in_2;
    //        //out_1 = vf2.out_1;
    //        //out_2 = vf2.out_2;
    //        //t_1 = vf2.t_1;
    //        //t_2 = vf2.t_2;
    //        addedAtom1 = vf2.addedAtom1;
    //        addedAtom2 = vf2.addedAtom2;
    //        nodesSorted = vf2.nodesSorted;
    //    }

    //    public override State2 Clone()
    //    {
    //        return new VF2SubState2(this);
    //    }

    //    public override Molecule GetMolecule1() { return m1; }
    //    public override Molecule GetMolecule2() { return m2; }
    //    public override bool NextPair(ref Atom nextAtom1, ref Atom nextAtom2, Atom prevAtom1 = null, Atom prevAtom2 = null)
    //    {

    //        if (prevAtom1 == null)
    //        {
    //            prevAtom1 = m1.Atoms[0];
    //        }
    //        if (prevAtom2 == null)
    //        {
    //            prevAtom2 = m2.Atoms[0];
    //        }
    //        else prevAtom2 = m2.GetNextAtom(prevAtom2);

    //        if (terminal_1.Count > core_1.Count)
    //        {
    //            while (prevAtom1 != null && (core_1.Contains(prevAtom1) || !terminal_1.Contains(prevAtom1)))
    //            {
    //                prevAtom1 = m1.GetNextAtom(prevAtom1);
    //                prevAtom2 = m2.Atoms[0];
    //            }
    //        }
    //        else
    //        {
    //            while (prevAtom1 != null && core_1.Contains(prevAtom1))
    //            {
    //                prevAtom1 = m1.GetNextAtom(prevAtom1);
    //                prevAtom2 = m2.Atoms[0];
    //            }
    //        }

    //        if(terminal_2.Count > core_2.Count)
    //        {
    //            while (prevAtom2 != null && (core_2.Contains(prevAtom2) || !terminal_2.Contains(prevAtom2)))
    //            {
    //                prevAtom2 = m2.GetNextAtom(prevAtom2);
    //            }
    //        }
    //        else
    //        {
    //            while (prevAtom2 != null && core_2.Contains(prevAtom1))
    //                prevAtom2 = m2.GetNextAtom(prevAtom2);
    //        }

    //        if (prevAtom1 != null && prevAtom2 != null)
    //        {
    //            nextAtom1 = prevAtom1;
    //            nextAtom2 = prevAtom2;
    //            return true;
    //        }
    //        return false;
    //    }

    //    public override bool IsFeasiblePair(Atom atom1, Atom atom2)
    //    {
    //        if (atom1.Element != atom2.Element) return false;
    //        if (atom1.ConnectedAtoms.Length == 1) return true;

    //        //int i, other1, other2;
    //        //Bond attr1 = null;
    //        //int termout1 = 0, termout2 = 0, termin1 = 0, termin2 = 0, new1 = 0, new2 = 0;

    //        //// Check the 'out' edges of node1
    //        //for (i = 0; i < g1.OutEdgeCount(node1); i++)
    //        //{
    //        //    other1 = g1.GetOutEdge(node1, i, ref attr1);
    //        //    if (core_1[other1] != NULL_NODE)
    //        //    {
    //        //        other2 = core_1[other1];
    //        //        if (!g2.HasEdge(node2, other2) ||
    //        //            !g1.CompatibleEdge(attr1, g2.GetEdgeAttr(node2, other2)))
    //        //            return false;
    //        //    }
    //        //    else
    //        //    {
    //        //        if (in_1[other1] != 0)
    //        //            termin1++;
    //        //        if (out_1[other1] != 0)
    //        //            termout1++;
    //        //        if (in_1[other1] == 0 && out_1[other1] == 0)
    //        //            new1++;
    //        //    }
    //        //}


    //        foreach (Atom a1 in atom1.ConnectedAtoms)
    //        {
    //            foreach (Atom a2 in atom2.ConnectedAtoms)
    //            {
    //                if (a1.Element == a2.Element)
    //                {
    //                    if (a1.GetBond(atom1).BondType == a2.GetBond(atom2).BondType) return true;
    //                }
    //            }
    //        }
    //        return false;


    //        //// Check the 'out' edges of node2
    //        //for (i = 0; i < g2.OutEdgeCount(node2); i++)
    //        //{
    //        //    other2 = g2.GetOutEdge(node2, i);
    //        //    if (core_2[other2] != NULL_NODE)
    //        //    {
    //        //        other1 = core_2[other2];
    //        //        if (!g1.HasEdge(node1, other1))
    //        //            return false;
    //        //    }
    //        //    else
    //        //    {
    //        //        if (in_2[other2] != 0)
    //        //            termin2++;
    //        //        if (out_2[other2] != 0)
    //        //            termout2++;
    //        //        if (in_2[other2] == 0 && out_2[other2] == 0)
    //        //            new2++;
    //        //    }
    //        //}
    //        //return termin1 <= termin2 && termout1 <= termout2 && new1 <= new2;
    //    }

    //    public override void AddPair(Atom atom1, Atom atom2)
    //    {
    //        core_1.Push(atom2);
    //        core_2.Push(atom1);
    //        terminal_1.AddRange(atom1.ConnectedAtoms);
    //        terminal_2.AddRange(atom2.ConnectedAtoms);
    //    }

    //    public override bool IsGoal()
    //    {
    //        return core_1.Count == m1.Atoms.Length;
    //    }

    //    public override bool IsDead()
    //    {
    //        return m1.Atoms.Length > m2.Atoms.Length;
    //        //bool b2 = t_1.Count > t_2.Count;
    //        //bool b3 = out_1.Count > out_2.Count;
    //        //bool b4 = in_1.Count > in_2.Count;
    //        //return b1 || b2 || b3 || b4;
    //    }

    //    public override int CoreLen()
    //    {
    //        return core_1.Count;
    //    }

    //    public override void GetCoreSet(ref Atom[] c1, ref Atom[] c2)
    //    {
    //        c1 = core_2.ToArray<Atom>();
    //        c2 = core_1.ToArray<Atom>(); 
    //    }

    //    public override void BackTrack()
    //    {
    //        Atom a1 = core_1.Pop();
    //        Atom a2 = core_2.Pop();
    //        foreach (Atom a in a2.ConnectedAtoms)
    //        {
    //            terminal_1.Remove(a);
    //        }
    //        foreach (Atom a in a1.ConnectedAtoms)
    //        {
    //            terminal_2.Remove(a);
    //        }
    //    }
    //}
}