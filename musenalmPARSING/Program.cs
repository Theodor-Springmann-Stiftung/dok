
using System.Xml.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using MusenalmConverter;
using MusenalmConverter.Models.AlteDBXML;
using MusenalmConverter.Models.NeueDBXML;
using System.Xml.Serialization;
using System.Xml;

const string DATASOURCEFOLDER = "../daten_2023-05-25/";
const string LOGFILE = "./log.txt";
const string SCHEMASOURCEFOLDER = "../norm/";
const string SCHEMADESTFILE = "../norm/Schema.xsd";
const string OUTFOLDER = "./generated/";


LogSink.Instance.SetFile(LOGFILE);
XMLHelpers.GenerateSchemata(SCHEMASOURCEFOLDER, SCHEMADESTFILE);
var data = DATAFile.GenerateDataFilesDirectory(DATASOURCEFOLDER);
var oldDB = new AlteDBXMLLibrary(data);
var newDB = new NeueDBXMLLibrary(data, oldDB);
newDB.Save(OUTFOLDER, "../" + SCHEMADESTFILE);

public class DATAFile {
    public string BaseElementName { get; private set; }
    public string File { get; private set; }
    public XDocument Document { get; private set; }

    public DATAFile(string name, string file, XDocument doc) {
        BaseElementName = name;
        File = file;
        Document = doc;
    }

    public static IEnumerable<DATAFile> GenerateDataFilesDirectory(string sourcedir) { 
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
}
