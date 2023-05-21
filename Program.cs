using System.Xml.Linq;
using System.Linq;

const string DATASOURCE = "./daten_2023-05-19/";


var data = getDATA();
generateNames(data);

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

    var names = new Dictionary<string, XElement>();
    foreach(var e in realnamen.Document.Root.Descendants(realnamen.BaseElementName)) {
        if (e.Element("REALNAME") != null) {
            var name = e.Element("REALNAME").Value;
            names.Add(name.Trim(), e);
        }
    }

    var notfound = new List<(string, XElement)>();
    var keystodelete = new List<string>();
    foreach (var e in names) {
        var compositenames = e.Key.Split(" u.");
        if (compositenames.Length > 1) {
            var c = compositenames.Where(x => !String.IsNullOrEmpty(x.Trim()) && !names.ContainsKey(x.Trim()));
            if (c != null && c.Any()) notfound.AddRange(c.Select(x => (x.Trim(), e.Value)));
            keystodelete.Add(e.Key);
        }

        compositenames = e.Key.Split(";");
        if (compositenames.Length > 1) {
            var c = compositenames.Where(x => !String.IsNullOrEmpty(x.Trim()) && !names.ContainsKey(x.Trim()));
            if (c != null && c.Any()) notfound.AddRange(c.Select(x => (x.Trim(), e.Value)));
            keystodelete.Add(e.Key);
        }
    }

    foreach (var k in keystodelete) names.Remove(k);
    var uniquenames = names.OrderBy(x => x.Key).ToList().ToList();
    notfound = notfound.DistinctBy(x => x.Item1).ToList();
    foreach (var n in notfound) uniquenames.Add(new KeyValuePair<string, XElement>(n.Item1, n.Item2));

    var generatedelements = new List<XElement>();
    var id = 1;
    foreach (var n in uniquenames) {
        var element = new XElement("Akteure");
        element.Add(new XElement("ID", id));
        var realname = n.Key.Split(',').Reverse();
        element.Add(new XElement("Name", String.Join(" ", realname).Trim()));
        var lebensdaten = n.Value.Element("Daten");
        if (lebensdaten != null && !String.IsNullOrWhiteSpace(lebensdaten.Value))
            element.Add(new XElement("Lebensdaten", lebensdaten.Value.Trim()));
        var beruf = n.Value.Element("Beitrag");
        if (beruf != null && !String.IsNullOrWhiteSpace(beruf.Value))
            element.Add(new XElement("Beruf", "<div>" + beruf.Value.Trim() + "</div>"));
        var pseudonyme = n.Value.Element("Pseudonyme");
        if (pseudonyme != null && !String.IsNullOrWhiteSpace(pseudonyme.Value))
            element.Add(new XElement("Pseudonyme", pseudonyme.Value));
        element.Add(new XElement("Sortiername", n.Key.Trim()));
        var nachweis = n.Value.Element("Nachweis");
        if (nachweis != null && !String.IsNullOrWhiteSpace(nachweis.Value))
            element.Add(new XElement("Nachweis", nachweis.Value));
        id++;

        generatedelements.Add(element);
    }

    var document = new XDocument();
    var root = new XElement("dataroot");
    foreach (var e in generatedelements) root.Add(e);
    document.Add(root);
    if (!Directory.Exists("./generated")) Directory.CreateDirectory("./generated");
    document.Save("./generated/generated_Akteure.xml");
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
