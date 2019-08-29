using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.ChemInfo
{
    class ForceDirectedGraph
    {
        Molecule molecule;

        public ForceDirectedGraph(Molecule m)
        {
            molecule = m;
        }

        // Molecule Boundaries for drawing algorithm

        public System.Drawing.Rectangle GetLocationBounds()
        {
            int top = molecule.Atoms[0].Location2D.Y;
            int bottom = molecule.Atoms[0].Location2D.Y;
            int left = molecule.Atoms[0].Location2D.X;
            int right = molecule.Atoms[0].Location2D.X;
            foreach (Atom a in molecule.Atoms)
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
                return molecule.Location;
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
                foreach (Atom a in molecule.Atoms)
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
            foreach (Atom a in molecule.Atoms)
            {
                sumX = sumX + a.Location2D.X;
                sumY = sumY + a.Location2D.Y;
            }
            int offsetX = sumX / molecule.Atoms.Length;
            int offsetY = sumY / molecule.Atoms.Length;
            foreach (Atom a in molecule.Atoms)
            {
                System.Drawing.Point p = new System.Drawing.Point(a.Location2D.X - offsetX, a.Location2D.Y - offsetY);
                a.Location2D = p;
            }
        }

        public void CenterMolecule(System.Drawing.Rectangle rect)
        {
            this.CenterMolecule();
            foreach (Atom a in molecule.Atoms)
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
            foreach (Atom a in molecule.Atoms)
            {
                a.Location2D = new System.Drawing.Point(random.Next(Size.Width), random.Next(Size.Height));
            }
        }

        double GetNextBondLength(Atom a1, Atom a2, Atom a3)
        {
            Bond b1 = molecule.GetBond(a1, a2);
            Bond b2 = molecule.GetBond(a2, a3);
            if (b1.BondType == BondType.Single && b2.BondType == BondType.Single)
                return Math.Sqrt(Math.Pow(100 * b1.BondLength, 2) + Math.Pow(100 * b1.BondLength, 2));
            return -1;
        }

        //protected void SetHydrogens()
        //{
        //    for (int i = molecule.Atoms.Length - 1; i >= 0; i--)
        //    {
        //        molecule.Atoms.AddRange(molecule.Atoms[i].SetHydrogens());
        //        molecule.Atoms.Remove(molecule.Atoms[i].RemoveOneHydrogen());
        //    }
        //}

        //protected void ResetHydrogens()
        //{
        //    for (int i = molecule.Atoms.Length - 1; i >= 0; i--)
        //    {
        //        Atom[] hydrogens = molecule.Atoms[i].RemoveHydrogens();
        //        foreach (Atom a in hydrogens)
        //        {
        //            molecule.Atoms.Remove(a);
        //        }
        //    }
        //}

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
            m_OptimalDistance = Math.Sqrt(this.GetArea() / molecule.Atoms.Length);
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


        public void Calculate()
        {
            RandomLocateAtoms();
            //SetHydrogens();
            Temperature = InitialTemperature;
            int maxIter = 500;
            for (int i = 0; i < maxIter; i++)
            {
                IterateFGD(i > 25);
                Temperature *= (1.0 - (double)i / (double)maxIter);
            }
            //ResetHydrogens();

        }

        protected void IterateFGD(bool addBonds)
        {
            CalculateOptimalDistanceBetweenVertices();
            foreach (Atom v in molecule.Atoms)
            {
                v.DeltaX = 0.0;
                v.DeltaY = 0.0;
            }
            foreach (Atom v in molecule.Atoms)
            {
                foreach (Atom u in molecule.Atoms)
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

            foreach (Atom v in molecule.Atoms)
            {
                foreach (Atom u in molecule.Atoms)
                {
                    Bond b = molecule.GetBond(u, v);
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
                foreach (Atom v in molecule.Atoms)
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
                    foreach (SustainableChemistryWeb.ChemInfo.Bond bond in v.BondedAtoms)
                    {
                        if (bond.BondType == BondType.Double)
                        {

                        }
                    }
                }
            }
            foreach (Atom v in molecule.Atoms)
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

        //public int GetBondAngle(Atom atom1, Atom atom2)
        //{
        //    foreach (Bond b in atom1.BondedAtoms)
        //    {
        //        if (b.ConnectedAtom == atom2) return b.Angle;
        //    }
        //    foreach (Bond b in atom2.BondedAtoms)
        //    {
        //        if (b.ConnectedAtom == atom1) return (b.Angle + 180) % 360;
        //    }
        //    return -1;
        //}

        //public int SetBondAngle(Atom atom1, Atom atom2, int angle)
        //{
        //    foreach (Bond b in atom1.BondedAtoms)
        //    {
        //        if (b.ConnectedAtom == atom2)
        //        {
        //            b.Angle = angle;
        //            //b.SetBondededAtomLocation();
        //            return angle;
        //        }
        //    }
        //    foreach (Bond b in atom2.BondedAtoms)
        //    {
        //        if (b.ConnectedAtom == atom1)
        //        {
        //            b.Angle = (angle + 180) % 360;
        //            //b.SetParentAtomLocation();
        //            return (angle + 180) % 360;
        //        }
        //    }
        //    return 0;
        //}

    }
}
