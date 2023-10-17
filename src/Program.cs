
using MusenalmConverter;
using System.Xml.Linq;
using System.Xml;
using MusenalmConverter.Migration.AlteDBXML;
using MusenalmConverter.Migration.MittelDBXML;
using System.Text;
using MusenalmConverter.API;

const string OLD_DATADIR = "./_input/data/";
const string MID_DATADIR = "./_input/data_uebertragung/";
const string NORMDIR = "./_input/norm/";
const string MITTELDIR = "./_input/mittel";
const string RDADIR = "./_input/RDA/xml/termList";
const string DESTDIR = "./_output/";
const string LOGFILE = "./_log.txt";
const string REIHENFILE = "./reihen.txt";

// LOGGING + DESTDIR
if (Directory.Exists(DESTDIR)) Directory.Delete(DESTDIR, true);
Directory.CreateDirectory(DESTDIR);
var log = LogSink.Instance;
log.SetFile(LOGFILE);

// Getting the OLD DATA
// var data = getDATA();
// var oldDB = new AlteDBXMLLibrary(data);

// // DATA Migration
// var mscheme = unifySchemata(MITTELDIR);
// var mDB = new MittelDBXMLLibrary(data, oldDB);
// mDB.Save(DESTDIR, mscheme);

// // MIGRATION DATA EDITING
var mdata = getDATA_MID();
var mscheme = unifySchemata(MITTELDIR);
var mlib = new MittelDBXMLLibrary(mdata);
mlib.transforms_nachweis_anmerkungen();
mlib.Save(DESTDIR, mscheme);

// // // API Calls
// var APIC = new APICaller(mDB);
// APIC.CreateActorData();
// APIC.CreateReihenData();
// APIC.PostReihenData().Wait();
// APIC.PostActorData().Wait();
// APIC.CreateBaendeData();
// APIC.PostBaendeData().Wait();

IEnumerable<DATAFile> getDATA_OLD() {
    var sourcedir = OLD_DATADIR;
    var xmls = Directory
        .EnumerateFiles(sourcedir, "*", SearchOption.AllDirectories)
        .Where(s => s.EndsWith(".xml"))
        .ToList();

    return xmls.Select(f => {
        var document = XDocument.Load(f, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
        var name = f.Split('/').Last();
        name = name.Substring(0, name.Length-4);
        return new DATAFile(name, f, document);
    });
}



IEnumerable<DATAFile> getDATA_MID() {
    var sourcedir = MID_DATADIR;
    var xmls = Directory
        .EnumerateFiles(sourcedir, "*", SearchOption.AllDirectories)
        .Where(s => s.EndsWith(".xml"))
        .ToList();

    return xmls.Select(f => {
        var document = XDocument.Load(f, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
        var name = document.Root.Elements()!.First()!.Name.ToString();
        return new DATAFile(name, f, document);
    });
}

void generateUniqueTagsValues(IEnumerable<DATAFile> files) {
    if (files == null || !files.Any()) return;
    var results = DESTDIR + "generateUniqueTagsValues/";
    if (Directory.Exists(results))  Directory.Delete(results, true);
    Directory.CreateDirectory(results);

    foreach (var f in files) {
        Dictionary<string, Dictionary<string, int>> elementsvalues = new Dictionary<string, Dictionary<string, int>>();
        foreach (var d in f.Document.Root.Descendants(f.BaseElementName)) {
            foreach (var e in d.Elements()) {
                var c = e.Value.ToString();
                var n = e.Name.ToString();
                if (!elementsvalues.ContainsKey(n)) elementsvalues.Add(n, new Dictionary<string, int>());
                if (!elementsvalues[n].ContainsKey(c)) elementsvalues[n].Add(c, 0);
                elementsvalues[n][c] += 1;
            }
        }

        foreach (var e in elementsvalues) {
            TextWriter tw = new StreamWriter(results + f.BaseElementName + "_" + e.Key + ".txt");
            var ordered = e.Value.OrderBy(x => x.Key);
            foreach (var v in ordered) {
                tw.WriteLine(v.Key + " " + v.Value);
            }
            tw.Close();
        }
    }
}

XDocument? unifySchemata(string inputfolder) {
    var xsds = Directory
            .EnumerateFiles(inputfolder, "*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".xsd"))
            .ToList();
    var files = new List<(string, XElement, XDocument, XElement)>();
    foreach (var f in xsds) {
        var doc = XDocument.Load(f, LoadOptions.SetLineInfo | LoadOptions.PreserveWhitespace);
        var element = doc.Root.Elements().Where(x => x.Attribute("name") != null && x.Attribute("name")!.Value != "dataroot").FirstOrDefault();
        var dataroot = doc.Root.Elements().Where(x => x.Attribute("name") != null && x.Attribute("name")!.Value == "dataroot").FirstOrDefault();
        if (element != null) files.Add((element.Attribute("name")!.Value, element, doc, dataroot));
    }

    XDocument res = null;
    XElement s = null;

    foreach (var e in files) {
        if (res == null || s == null) {
            res = e.Item3;
            s = e.Item4.Descendants(e.Item4.GetNamespaceOfPrefix("xsd") + "sequence").First();
            continue;
        } else {
            var seqences = e.Item4.Descendants(e.Item4.GetNamespaceOfPrefix("xsd") + "sequence").First().Elements();
            s.Add(seqences);
            res.Root!.Add(e.Item2);
        }
    }

   return res;

}

void germanizeRDA(string sourcedir, bool html) {
    var xmls = Directory
            .EnumerateFiles(sourcedir, "*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".xml") && !s.EndsWith("DEUTSCH.xml"))
            .ToList();
    foreach (var f in xmls) {
        var document = XDocument.Load(f);
        document.Descendants().Where(x =>
            !x.Elements().Any() &&
            x.HasAttributes &&
            x.Attribute(x.GetNamespaceOfPrefix("xml") + "lang") != null &&
            x.Attribute(x.GetNamespaceOfPrefix("xml") + "lang")?.Value != "de")
            .Remove();
        var fn = f.Split('\\').ToList();
        var nfn = "GER_" + fn.Last();
        fn.RemoveAt(fn.Count - 1);
        fn.Add(nfn);
        document.Save(String.Join('\\', fn), SaveOptions.None);

        if (html) {
            var sb = new StringBuilder();
            sb.Append("<html><body>");
            var depr = false;
            var intro = false;
            foreach (var n in document.Root.Nodes()) {
                switch (n.NodeType) {
                    case XmlNodeType.Comment:
                        var comment = (XComment)n;
                        if (comment.Value.Trim().StartsWith("Element Set:")) {
                            sb.Append("<h1>" + comment.Value.Split(":").Last() + "</h1>");
                        } else if (comment.Value.ToUpper().Contains("DEPRECATED")) {
                            depr = true;
                        }
                        break;
                    case XmlNodeType.Element:
                        if (depr) {
                            depr = false;
                            continue;
                        }
                        if (!intro) {
                            sb.Append("<table>");
                            intro = true;
                        }
                        var element = (XElement)n;
                        if (element.Name == element.GetNamespaceOfPrefix("rdf") + "Description") {
                            var id = element.Attribute(element.GetNamespaceOfPrefix("rdf") + "about")?.Value.Split("/").TakeLast(2).ToList();
                            var name = "rdf" + String.Join(':', id);
                            var label = element.Element(element.GetNamespaceOfPrefix("rdfs") + "label")?.Value;
                            var def = element.Element(element.GetNamespaceOfPrefix("skos") + "definition")?.Value;
                            if (!String.IsNullOrWhiteSpace(name) && 
                                !String.IsNullOrWhiteSpace(label) && 
                                !String.IsNullOrWhiteSpace(def)) {
                                sb.Append("<tr>");
                                sb.Append("<td><em>" + name + "</em></td>");
                                sb.Append("<td>" + label + "</td>");
                                sb.Append("<td>" + def + "</td>");
                                sb.Append("</tr>");
                            }
                        } else if(element.Name == element.GetNamespaceOfPrefix("skos") + "Concept") {
                            var name = element.Attribute(element.GetNamespaceOfPrefix("rdf") + "about")?.Value.Split("/").Last();
                            var label = element.Element(element.GetNamespaceOfPrefix("skos") + "prefLabel")?.Value;
                            var def = element.Element(element.GetNamespaceOfPrefix("skos") + "definition")?.Value;
                            if (!String.IsNullOrWhiteSpace(name) && 
                                !String.IsNullOrWhiteSpace(label) && 
                                !String.IsNullOrWhiteSpace(def)) {
                                sb.Append("<tr>");
                                sb.Append("<td>" + name + "</td>");
                                sb.Append("<td><em>" + label + "</em></td>");
                                sb.Append("<td>" + def + "</td>");
                                sb.Append("</tr>");
                            }
                        }
                        break;
                }
            }
            sb.Append("</table>");
            sb.Append("</html></body>");
            File.WriteAllText(f + ".html", sb.ToString());
        }
    }
}

// void exportReihen(NeueDBXMLLibrary library, string outpath) {
//     var rs = library.Reihen.OrderBy(x => x.Sortiername).ThenBy(x => x.ID);
//     var bs = library.Baende.ToDictionary(x => x.ID);
//     var reld = library.RELATION_BaendeReihen.ToLookup(x => x.Reihe);
//     var sb = new StringBuilder();
//     foreach (var r in rs) {
//         sb.AppendLine(r.Sortiername);
//     }

//     System.IO.File.WriteAllText(outpath, sb.ToString());
// }

public class DATAFile {
    public string BaseElementName { get; private set; }
    public string File { get; private set; }
    public XDocument Document { get; private set; }

    public DATAFile(string name, string file, XDocument doc) {
        BaseElementName = name;
        File = file;
        Document = doc;
    } 
}
