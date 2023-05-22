
using System.Xml.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using Musenalm;
using System.Xml.Serialization;
using System.Xml;

const string DATASOURCE = "./daten_2023-05-19/";


var data = getDATA();
var oldDB = new AlteDBLibrary(data);
var newDB = new NeueDBLibrary(data, oldDB);
newDB.Save("./generated/");

IEnumerable<DATAFile> getDATA() {    
    var sourcedir = DATASOURCE;
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

void generateNames(IEnumerable<DATAFile> files) {
    if (files == null || !files.Any()) return;
    var almneu = files.Where(n => n.BaseElementName == "AlmNeu").First();
    var realnamen = files.Where(n => n.BaseElementName == "REALNAME-Tab").First();
    var inhalte = files.Where(n => n.BaseElementName == "INH-TAB").First();
    var library = new Library();
    var akteurdocument = new XDocument();
    var akteurroot = new XElement("dataroot");
    var ortedocument = new XDocument();
    var orteroot = new XElement("dataroot");
    ortedocument.Add(orteroot);
    akteurdocument.Add(akteurroot);

    // REALNAMEN-Tab
    var names = new Dictionary<string, XElement>();
    foreach(var e in realnamen.Document.Root.Descendants(realnamen.BaseElementName)) {
        if (e.Element("REALNAME") != null) {
            var name = e.Element("REALNAME").Value;
            names.Add(name.Trim(), e);
        }
    }


    // AlmNeu
    library.AlmNeuDrucker = new Dictionary<string, string[]>();
    var foundnames = new Dictionary<string, int>();
    library.AlmNeuPlaces = new Dictionary<string, string[]>();
    var foundplaces = new HashSet<string>();
    Regex rgx = new Regex(@"(?<=\()[^()]*(?=\))");
    var orte = almneu.Document.Root.Descendants("ORT");
    var id = 0;
    foreach (var o in orte) {
        var nummer = o.ElementsBeforeSelf("NUMMER").First().Value;
        var val = o.Value;
        var matches = rgx.Matches(val);
        
        if (matches != null && matches.Any()) {
            library.AlmNeuDrucker.Add(nummer, new string[] {});
            foreach (var m in matches) {
                val = val.Replace("(" + m.ToString() + ")", null);
                var splitted = m.ToString().Split(new string[] {"/", ";"}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var so in splitted) {
                    if (!String.IsNullOrWhiteSpace(so)) {
                        var name = so.Trim();
                        library.AlmNeuDrucker[nummer].Append(name);
                        if (!foundnames.ContainsKey(name)) {
                            foundnames.Add(name, id);
                            id++;
                        }
                    }
                }
            }
        }

        var fo = val.Split(new string[]{" u.", " und", ";", ","}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (fo != null && fo.Any()) {
            library.AlmNeuPlaces.Add(nummer, new string[] {});
            foreach (var so in fo) {
                library.AlmNeuPlaces[nummer].Append(so);
                if (!foundplaces.Contains(so)) foundplaces.Add(so);
            }
        }
    }

    foreach (var n in foundnames) {
        var element = new XElement("Akteure");
        element.Add(new XElement("ID", n.Value));
        element.Add(new XElement("OrgName", n.Key));
        element.Add(new XElement("Sortiername", n.Key));
        element.Add(new XElement("Beruf", "Drucker/Verleger"));
        library.Names.Add(n.Key, (element, n.Value));
    }

    id = 1;
    library.Places = new Dictionary<string, (XElement, int)>();
    foreach (var o in foundplaces) {
        var element = new XElement("Orte");
        element.Add(new XElement("ID", id));
        element.Add(new XElement("Name", o));
        element.Add(new XElement("Land", "Deutschland"));
        id++;
    }

    foreach (var e in library.Names) akteurroot.Add(e.Value.Item1);
    if (!Directory.Exists("./generated")) Directory.CreateDirectory("./generated");
    akteurdocument.Save("./generated/generated_Akteure.xml");
}

void generateUniqueTagsValues(IEnumerable<DATAFile> files) {
    if (files == null || !files.Any()) return;
    var results = "./generateUniqueTagsValues/";
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
            foreach (var v in e.Value) {
                tw.WriteLine(v.Key + " " + v.Value);
            }
            tw.Close();
        }
    }
}

void germanizeRDA() {
    var sourcedir = "./RDA";
    var xmls = Directory
            .EnumerateFiles(sourcedir, "*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".xml") && !s.EndsWith("DEUTSCH.xml"))
            .ToList();
    foreach (var f in xmls) {
        var document = XDocument.Load(f);
        document.Descendants().Where(x =>
            x.HasAttributes &
            x.Attribute(x.GetNamespaceOfPrefix("xml") + "lang") != null &&
            x.Attribute(x.GetNamespaceOfPrefix("xml") + "lang")?.Value != "de")
            .Remove();
        document.Save(f.Substring(0, f.Length-4) + "DEUTSCH.xml", SaveOptions.None);
    }
}

class Library {
    public Dictionary<string, (XElement, int)>? Names { get; set; }
    public Dictionary<string, (XElement, int)>? Places { get; set; }

    public Dictionary<string, string[]>? AlmNeuDrucker { get; set; }
    public Dictionary<string, string[]>? AlmNeuHrsg { get; set; }
    public Dictionary<string, string[]>? AlmNeuPlaces { get; set; }
}

class DATAFile {
    public string BaseElementName { get; private set; }
    public string File { get; private set; }
    public XDocument Document { get; private set; }

    public DATAFile(string name, string file, XDocument doc) {
        BaseElementName = name;
        File = file;
        Document = doc;
    } 
}
