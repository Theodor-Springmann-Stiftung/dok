namespace MusenalmConverter;

using System.Xml.Linq;

public static class XMLHelpers {
    public static void GenerateUniqueTagsValues(IEnumerable<DATAFile> files) {
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

    public static void germanizeRDA(string sourcedir) {
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

    public static void GenerateSchemata(string inputfolder, string outputfile) {
        if (File.Exists(outputfile)) File.Delete(outputfile);
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

        if (res != null) {
            res.Save(outputfile);
        }

    }
}