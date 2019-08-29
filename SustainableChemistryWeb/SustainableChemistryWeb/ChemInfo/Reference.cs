using System;
using System.Collections.Generic;
using System.Linq;

namespace SustainableChemistryWeb.ChemInfo
{
    [Serializable]
    [Newtonsoft.Json.JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)]
    public class Reference
    {
        List<string> m_authors;

        public Reference(string data)
        {
        }

        public Reference(Int64 functionalGroup, Int64 reactionName, string data)
        {
            FunctionalGroupIndex = (int)functionalGroup;
            ReactionNameIndex = (int)reactionName;
            m_authors = new List<string>();
            string[] lines = data.Replace("\r", string.Empty).Split('\n');
            RISData = data.TrimStart(' ', '\r', '\n');
            foreach (string line in lines)
            {

                if (line.StartsWith("TY")) Type = line.Substring(6);
                if (String.IsNullOrEmpty(Title) && line.StartsWith("TI")) Title = line.Substring(6);
                if (String.IsNullOrEmpty(Title) && line.StartsWith("T1")) Title = line.Substring(6);
                if (line.StartsWith("AU")) m_authors.Add(line.Substring(6));
                if (line.StartsWith("AB")) Abstract = line.Substring(6);
                if (line.StartsWith("AD")) AuthorAddress = line.Substring(6);
                if (line.StartsWith("JO")) Journal = line.Substring(6);
                if (line.StartsWith("VO")) Volume = line.Substring(6);
                if (line.StartsWith("IS")) Issue = line.Substring(6);
                if (line.StartsWith("SP")) StartPage = line.Substring(6);
                if (line.StartsWith("EP")) EndPage = line.Substring(6);
                if (line.StartsWith("DA")) Date = line.Substring(6);
                if (line.StartsWith("UR")) URL = line.Substring(6);
                if (line.StartsWith("DO")) doi = line.Substring(6);
                if (line.StartsWith("PY")) PY = line.Substring(6);
            }
        }

        public string RISData { get; }
        public string FunctionalGroup { get; }
        public int FunctionalGroupIndex { get; }
        public string ReactionName { get; }
        public int ReactionNameIndex { get; }
        public string Type { get; }
        public string Title { get; }
        public string[] Authors
        {
            get
            {
                return m_authors.ToArray<string>();
            }
        }
        public string Abstract { get; }
        public string AuthorAddress { get; }
        public string Journal { get; }
        public string Volume { get; }
        public string Issue { get; }
        public string StartPage { get; }
        public string EndPage { get; }
        public string Date { get; }
        public string URL { get; }
        [Newtonsoft.Json.JsonProperty]
        public string doi { get; }
        public string PY { get; }
        [Newtonsoft.Json.JsonProperty]
        public string Text
        {
            get
            {
                return this.ToString();
            }
        }

        override public string ToString()
        {
            string retVal = String.Empty;
            foreach (string author in this.Authors)
            {
                retVal = retVal + author + ", ";
            }
            retVal = retVal.Remove(retVal.Length - 2, 2) + " (" + PY + "). \"" + Title + ".\" " + Journal + " " + Volume + ": " + StartPage + "-" + EndPage + ".";
            return retVal;

            // CitePro is here: https://github.com/fouke-boss/citeproc-dotnet
            // citation styples are here: http://citationstyles.org/developers/
            // Load a style from disk(or use one of the overloads for reading
            //from a stream, a text reader or an xml reader).
            // var style = File.Load("harvard-cite-them-right.csl");

            // var style = CiteProc.File.Load(new System.IO.StringReader(Properties.Resources.HarvardCiteThemRight));

            // // Compile the style to get a processor instance.
            // var processor = CiteProc.Processor.Compile(style);

            // CiteProc.Data.DataProvider dataProvider = new CiteProc.Data.DataProvider();
            // dataProvider.GetVariables

            // // Data of the referenced items (books, articles, etc.) is accessed
            // // through the IDataProvider interface. CiteProc.NET comes with a
            // // default implementation of this interface that supports the
            // // CSL JSON format.
            // processor.DataProviders = CiteProc.v10.
            //     //Load("items.json", CiteProc.Data.DataFormat.Json);

            // // Now, you are ready to render citations and bibliographies using
            // // the selected style:
            // var entries = processor.GenerateBibliography();

            // // The result is an instance of a CiteProc.Formatting.Run class.
            // // This instance can then be converted to the desired format. CiteProc
            // // supports plain text and HTML out-of-the-box; the CiteProc.WpfDemo
            // // project contains an example of how to show the result in a WPF
            // // TextBlock. Other formats can be added easily.

            // // Austen, J. (1995) Pride and Prejudice. New York, NY: Dover Publications.
            // var plainText = entries.First().ToPlainText();

            // // Austen, J. (1995) <i>Pride and Prejudice</i>. New York, NY: Dover Publications.
            // var html = entries.First().ToHtml();

        }
    }
}
