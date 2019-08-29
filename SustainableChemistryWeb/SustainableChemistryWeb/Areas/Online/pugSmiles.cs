namespace SustainableChemistryWeb.Areas.Online
{

    public class Rootobject
    {
        public Propertytable PropertyTable { get; set; }
    }

    public class Propertytable
    {
        public Property1[] Properties { get; set; }
    }

    public class Property1
    {
        public int CID { get; set; }
        public string CanonicalSMILES { get; set; }
    }
}
