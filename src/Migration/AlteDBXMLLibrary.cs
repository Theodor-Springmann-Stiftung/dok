namespace MusenalmConverter.Migration.AlteDBXML;

using System.Xml;
using System.Xml.Serialization;

public class AlteDBXMLLibrary {
    public List<REALNAMETab> REALNAMETab;
    public Dictionary<long, INHTab> INHTab;
    public Dictionary<long, AlmNeu> AlmNeu;
    public Dictionary<long, GMBIBLIO> GMBIBLIO;

    public AlteDBXMLLibrary(IEnumerable<DATAFile> files) {
        REALNAMETab = new List<REALNAMETab>();
        INHTab = new Dictionary<long, INHTab>();
        AlmNeu = new Dictionary<long, AlmNeu>();
        GMBIBLIO = new();
        var realnameS = new XmlSerializer(typeof(REALNAMETab));
        var inhalteS = new XmlSerializer(typeof(INHTab));
        var almS = new XmlSerializer(typeof(AlmNeu));
        var gmbiblioS = new XmlSerializer(typeof(GMBIBLIO));
        foreach (var f in files) {
            var elements = f.Document.Root.Elements(f.BaseElementName);
            foreach (var e in elements) {
                if (f.BaseElementName == "REALNAME-Tab") {
                    REALNAMETab i;
                    using (XmlReader r = e.CreateReader()) {
                        i = (REALNAMETab)realnameS.Deserialize(r);
                    }
                    if (i != null) REALNAMETab.Add(i);
                }
                else if (f.BaseElementName == "INH-TAB") {
                    INHTab i;
                    using (XmlReader r = e.CreateReader()) {
                        i = (INHTab)inhalteS.Deserialize(r);
                    }
                    if (i != null) INHTab.Add(i.INHNR, i);
                }
                else if (f.BaseElementName == "GM-BIBLIO") {
                    GMBIBLIO i;
                    using (XmlReader r = e.CreateReader()) {
                        i = (GMBIBLIO)gmbiblioS.Deserialize(r);
                    }
                    if(i != null) GMBIBLIO.Add(i.NUMMER, i);
                }
                else if (f.BaseElementName == "AlmNeu") {
                    AlmNeu i;
                    using (XmlReader r = e.CreateReader()) {
                        i = (AlmNeu)almS.Deserialize(r);
                    }
                    if(i != null) AlmNeu.Add(i.NUMMER, i);
                }
            }
        }
    }
}