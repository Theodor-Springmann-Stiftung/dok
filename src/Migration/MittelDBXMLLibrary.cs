namespace MusenalmConverter.Migration.MittelDBXML;
using MusenalmConverter.Migration.AlteDBXML;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

public class MittelDBXMLLibrary {
    private static List<string[]> TYP = new List<string[]> {
        new string [] { "Corrigenda" }, 
        new string [] { "Diagramm" }, 
        new string [] { "Gedicht/Lied", "Gedicht" }, 
        new string [] { "Graphik", "Graphik/Tafel" }, 
        new string [] { "Graphik-Verzeichnis", "G-Verz" }, 
        new string [] { "graph. Anleitung" }, 
        new string [] { "graph. Strickanleitung" }, 
        new string [] { "graph. Tanzanleitung" }, 
        new string [] { "Inhaltsverzeichnis", "I-Verz" }, 
        new string [] { "Kalendarium", "Kalender" }, 
        new string [] { "Karte" }, 
        new string [] { "Musikbeigabe" }, 
        new string [] { "Musikbeigaben-Verzeichnis", "MusBB-Verz" }, 
        new string [] { "Motto" }, 
        new string [] { "Prosa" }, 
        new string [] { "Rätsel" }, 
        new string [] { "Sammlung" }, 
        new string [] { "Spiegel" }, 
        new string [] { "szen. Darstellung" }, 
        new string [] { "Tabelle" }, 
        new string [] { "Tafel", "Graphik/Tafel" }, 
        new string [] { "Titel" }, 
        new string [] { "Text" }, 
        new string [] { "Trinkspruch" }, 
        new string [] { "Umschlag" }, 
        new string [] { "Widmung" }
    }; 

    private static string[] PAG = new string[] {
        "ar",
        "röm",
        "ar1",
        "ar2",
        "ar3",
        "ar4",
        "ar5",
        "ar6",
        "ar7",
        "röm1",
        "röm2",
        "röm3",
        "röm4"
    };

    public List<Akteure> Akteure;
    public List<Inhalte> Inhalte;
    public List<Reihen> Reihen;
    public List<Baende> Baende;
    public List<RELATION_InhalteAkteure> RELATION_InhalteAkteure;
    public List<RELATION_BaendeAkteure> RELATION_BaendeAkteure;
    public List<RELATION_BaendeReihen> RELATION_BaendeReihen;

    private IEnumerable<DATAFile> _dataFiles;
    private AlteDBXMLLibrary _alteDB;
    private LogSink _logSink;

    public MittelDBXMLLibrary(IEnumerable<DATAFile> files, AlteDBXMLLibrary alteDB) {
        _logSink = LogSink.Instance;
        _dataFiles = files;
        _alteDB = alteDB;
        namesFromREALNAMEN();
        ParseReihenOrteExemplareBaende();
        ParseInhalte();
    }

    
    // For now only deserializing Baende, Reihen & their relations are supported    
    public MittelDBXMLLibrary(IEnumerable<DATAFile> files) {
        RELATION_BaendeReihen = new ();
        RELATION_BaendeAkteure = new ();
        RELATION_InhalteAkteure = new ();
        Baende = new ();
        Reihen = new ();
        Inhalte = new ();
        Akteure = new ();
        var RELATION_BaendeReihenS = new XmlSerializer(typeof(RELATION_BaendeReihen));
        var BaendeS = new XmlSerializer(typeof(Baende));
        var ReihenS = new XmlSerializer(typeof(Reihen));
        var InhalteS = new XmlSerializer(typeof(Inhalte));
        var AkteureS = new XmlSerializer(typeof(Akteure));
        var RELATION_BaendeAkteureS = new XmlSerializer(typeof(RELATION_BaendeAkteure));
        var RELATION_InhalteAkteureS = new XmlSerializer(typeof(RELATION_InhalteAkteure));
        foreach (var f in files) {
            var elements = f.Document.Root.Elements(f.BaseElementName);
            foreach (var e in elements) {
                if (f.BaseElementName == "_x002A_RELATION_BaendeReihen") {
                    RELATION_BaendeReihen i;
                    using (XmlReader r = e.CreateReader()) {
                        i = (RELATION_BaendeReihen)RELATION_BaendeReihenS.Deserialize(r);
                    }
                    if (i != null) RELATION_BaendeReihen.Add(i);
                }
                else if (f.BaseElementName == "Baende") {
                    Baende i;
                    using (XmlReader r = e.CreateReader()) {
                        i = (Baende)BaendeS.Deserialize(r);
                    }
                    if(i != null) Baende.Add(i);
                }
                else if (f.BaseElementName == "Reihen") {
                    Reihen i;
                    using (XmlReader r = e.CreateReader()) {
                        i = (Reihen)ReihenS.Deserialize(r);
                    }
                    if(i != null) Reihen.Add(i);
                } else if (f.BaseElementName == "Inhalte") {
                    Inhalte i;
                    using (XmlReader r = e.CreateReader()) {
                        i = (Inhalte)InhalteS.Deserialize(r);
                    }
                    if(i != null) Inhalte.Add(i);
                } else if (f.BaseElementName == "Akteure") {
                    Akteure i;
                    using (XmlReader r = e.CreateReader()) {
                        i = (Akteure)AkteureS.Deserialize(r);
                    }
                    if(i != null) Akteure.Add(i);
                } else if (f.BaseElementName == "_x002A_RELATION_BaendeAkteure") {
                    RELATION_BaendeAkteure i;
                    using (XmlReader r = e.CreateReader()) {
                        i = (RELATION_BaendeAkteure)RELATION_BaendeAkteureS.Deserialize(r);
                    }
                    if(i != null) RELATION_BaendeAkteure.Add(i);
                } else if (f.BaseElementName == "_x002A_RELATION_InhalteAkteure") {
                    RELATION_InhalteAkteure i;
                    using (XmlReader r = e.CreateReader()) {
                        i = (RELATION_InhalteAkteure)RELATION_InhalteAkteureS.Deserialize(r);
                    }
                    if(i != null) RELATION_InhalteAkteure.Add(i);
                }
            }
        }
    }

    public void transforms_nachweis_anmerkungen() {
        List<(Baende, Reihen)> br = new();
        var rella = this.RELATION_BaendeReihen.ToLookup(x => x.REIHE);
        var bla = this.Baende.ToDictionary(x => x.ID);
        foreach (var  r in this.Reihen) {
            if (rella.Contains(r.ID)) {
                var b = rella[r.ID].Select(x => bla[x.BAND]).OrderBy(x => x.JAHR);
                if (String.IsNullOrWhiteSpace(r.Anmerkungen)) {
                    r.Anmerkungen = b.First()?.ANMERKUNGEN;
                    b.First().ANMERKUNGEN = string.Empty;
                }
                r.NACHWEIS = b.First()?.NACHWEIS;
            }
        }
    }
    
    private void namesFromREALNAMEN() {
        var notfound = new List<(string, REALNAMETab)>();
        var names = new Dictionary<string, Akteure>();
        var hashset = _alteDB.REALNAMETab.Select(x => x.REALNAME).ToHashSet();
        var toparse = _alteDB.REALNAMETab;
        foreach (var n in toparse) {
            var composite = n.REALNAME.Split(new string[] {";", " u."}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (composite.Length > 1) {
                var nf = composite.Where(x => !hashset.Contains(x));
                if (nf != null && nf.Any()) notfound.AddRange(nf.Select(x => (x, n)));
            } else {
                names.Add(n.REALNAME.Trim(), new Akteure() {
                    NAME = n.REALNAME.Trim(),
                    NACHWEIS = trimOrNull(n.Nachweis),
                    LEBENSDATEN = trimOrNull(n.Daten),
                    BERUF = trimOrNull(n.Beitrag),
                    PSEUDONYM = trimOrNull(n.Pseudonym),
                });
            }
        }
        
        var orderednames = names.OrderBy(x => x.Key).Select(x => x.Value).ToList();

        foreach (var n in notfound) {
            _logSink.LogLine("Name " + n.Item1.Trim() + " nicht gefunden. Herkunft: REALNAME-Tab " + n.Item2.REALNAME);
            if (!names.ContainsKey(n.Item1.Trim())) {
                var name = n.Item1.Split(',').Reverse();
                var akteur = new Akteure() {
                    NAME = String.Join(" ", name),
                    ANMERKUNGEN = "ÜBERPRÜFEN: Autogeneriert aus REALNAMEN-Tab",
                };
                orderednames.Add(akteur);
                names.Add(n.Item1.Trim(), akteur);
            }
        }

        if (Akteure == null) Akteure = new List<Akteure>();
        int id = 1;
        foreach (var n in orderednames) {
            n.ID = id;
            Akteure.Add(n);
            id++;
        }


    }

    public void ParseReihenOrteExemplareBaende() {
        var names = new Dictionary<string, Akteure>();
        var werkeAkteure = new List<RELATION_BaendeAkteure>();
        var reihen = new HashSet<string>();
        var reihenBaende = new List<RELATION_BaendeReihen>();
        var reihenwerke = new Dictionary<long, List<string>>();
        var abschnitte = new Dictionary<long, string>();
        var werke = new Dictionary<long, Baende>();
        var werkeorte = new Dictionary<long, List<string>>();
        var werkedrucker = new Dictionary<long, List<string>>(); 
        var werkehrsg = new Dictionary<long, List<string>>(); 
        var toparse = _alteDB.AlmNeu;
        var idakt = Akteure.Count + 1;
        Regex rgxnormabschnitt = new Regex(@"\s?\d{4}\s?");
        Regex rgxround = new Regex(@"(?<=\()[^()]*(?=\))");
        Regex rgxeck = new Regex(@"(?<=\()[^()]*(?=\))");
        Regex rgxfourendnumbers = new Regex(@"\s?(\d{4},\s?\d{4}|\d{4}\/\d{4}|19\d{2}|18\d{2}|17\d{2}|\[oJ\]|Bd \d \[o\.J\.\]|\[o\.J\.\])(-\d$)?(\s?\(\d\))?(\s\(2\.\))?(\s1\su\.\s2)?(\s?\[var\.?\]$)?(\s?1\.)?(\(Titelauflage\))?\s?");
        Regex rgxausgabe = new Regex(@"(?<=-)\d(?=\s?$)");
        foreach (var n in toparse) {

            // Verleger
            var ort = n.Value.ORT;
            if (!String.IsNullOrWhiteSpace(ort)) {
                var matches = rgxround.Matches(ort);

                if (matches != null && matches.Any()) {
                    werkedrucker.Add(n.Value.NUMMER, new List<string>());
                    foreach (var m in matches) {
                        ort = ort.Replace("(" + m.ToString() + ")", null);
                        var splittednames = m.ToString().Split(new string[] {"/", ";"}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        foreach (var na in splittednames) {
                            if (!names.ContainsKey(na)) {
                                names.Add(na, new Akteure() {
                                    ID = idakt,
                                    ORGANISATION = true,
                                    NAME = na,
                                    ANMERKUNGEN = "ÜBERPRÜFEN: Autogeneriert aus Verlegern (AlmNeu)",
                                });
                                idakt++;
                            }
                            werkedrucker[n.Value.NUMMER].Add(na);
                        }
                    }
                }
            }

            // Reihen
            if (!String.IsNullOrWhiteSpace(n.Value.REIHENTITEL)) {
                reihenwerke.Add(n.Value.NUMMER, new List<string>());
                var composite = n.Value.REIHENTITEL.Split("/)", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var e in composite) {
                    var reihe = e;
                    var m = rgxfourendnumbers.Matches(e);
                    if (m != null && m.Any())  {
                        var abschnitt = m.Last().ToString();
                        var normabs = rgxnormabschnitt.Match(abschnitt);
                        if (normabs.Length < abschnitt.Length) {
                            if (!abschnitte.ContainsKey(n.Value.NUMMER)) abschnitte.Add(n.Value.NUMMER, abschnitt);
                            else abschnitte[n.Value.NUMMER] += abschnitt;
                        }
                        
                        reihe = e.Replace(abschnitt, null);
                    }
                    if (!reihen.Contains(reihe)) reihen.Add(reihe);
                    reihenwerke[n.Value.NUMMER].Add(reihe);
                }
            }

            // Herausgaber
            if (!String.IsNullOrWhiteSpace(n.Value.HRSGREALNAME)) {
                var composite = n.Value.HRSGREALNAME.Split(new string[] {";", " u."}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                werkehrsg.Add(n.Value.NUMMER, new List<string>());
                foreach (var c in composite) {
                    werkehrsg[n.Value.NUMMER].Add(c);
                }
            }
            
            var vorh = trimOrNull(n.Value.VORHANDENALS);
            Status[]? status = null;
            if (vorh != null) {
                if (vorh == "Original") status = new Status[] { new Status() { Value = "Original vorhanden" }};
                if (vorh == "Reprint") status = new Status[] { new Status() { Value = "Reprint vorhanden" }};;
                if (vorh == "fremde Herkunft") status = new Status[] { new Status() { Value = "Fremde Herkunft" }};;
                if (vorh == "Reprint u. fremde Herkunft") status = new Status[] { 
                    new Status() { Value = "Reprint vorhanden" },
                    new Status() { Value = "Fremde Herkunft" },
                };
            }

            // Ausgabe
            var ausg = 1;
            if (!String.IsNullOrWhiteSpace(n.Value.REIHENTITEL)) {
                var m = rgxausgabe.Matches(n.Value.REIHENTITEL);
                if (m != null && m.Count > 0) ausg = Int32.Parse(m.Last().ToString());
            }

            // Baende: ID (NUMMER), TITEL, TitelTranskription, Reihe, Jahr, Ausgabe, Struktur, Nachweis, Anmerkungen
            werke.Add(n.Value.NUMMER, new Baende() {
                ID = n.Value.NUMMER,
                SORTIERTITEL = trimOrNull(n.Value.ALMTITEL),
                TITEL = trimOrNull(n.Value.ALMTITEL),
                ORT = trimOrNull(n.Value.ORT),
                NORM = trimOrNull(n.Value.NORM),
                HERAUSGEBER = trimOrNull(n.Value.HERAUSGEBER),
                JAHR = n.Value.JAHR,
                NACHWEIS = trimOrNull(n.Value.NACHWEIS),
                STRUKTUR = trimOrNull(n.Value.STRUKTUR),
                ANMERKUNGEN = trimOrNull(n.Value.ANMERKUNGEN),
                REIHENTITELALT = trimOrNull(n.Value.REIHENTITEL),
                STATUS = status,
                AUTOPSIE = n.Value.AUTOPSIE,
                BIBLIOID = trimOrNull(n.Value.BIBLIONR),
                ERFASST = n.Value.VOLSTAENDIGERFASST,
                VORHANDEN = n.Value.VORHANDEN
            });
        }
        

        // REIHEN SORTIEREN
        var ordrh = reihen.OrderBy(x => x);
        var idrh = 1;
        foreach (var n in ordrh) {
            var rname = n.Split(',').Reverse();
            if (Reihen == null) Reihen = new List<Reihen>();
            Reihen.Add(new Reihen() {
                ID = idrh,
                SORTIERNAME = n,
                NAME = String.Join(" ", rname)
            });
            idrh++;
        }

        // RELATIONEN SETZEN
        foreach (var n in werkedrucker) {
            foreach (var m in n.Value) {
                werkeAkteure.Add(new RELATION_BaendeAkteure() {
                    BAND = n.Key,
                    BEZIEHUNG = 6,
                    AKTEUR = names[m].ID
                });
            }
        }

        foreach (var n in werkehrsg) {
            foreach (var m in n.Value) {
                var akteur = this.Akteure.Where(x => x.NAME == m).FirstOrDefault();
                if (akteur == null) {
                    _logSink.LogLine("Name " + m + " nicht gefunden. Herkunft: AlmNeu Nr. " + n.Key);
                    akteur = new Akteure() {
                        ID = idakt,
                        NAME = m,
                        ANMERKUNGEN = "ÜBERPRÜFEN: Autogeneriert aus HRSGREALNAME (AlmNeu Nr. " + n.Key + ")",
                    };
                    Akteure.Add(akteur);
                    idakt++;
                }
                werkeAkteure.Add(new RELATION_BaendeAkteure() {
                    BAND = n.Key,
                    BEZIEHUNG = 5,
                    AKTEUR = akteur.ID
                });
            }
        }

        foreach (var n in reihenwerke) {
            foreach (var m in n.Value) {
                var reihe = Reihen.Where(x => x.SORTIERNAME == m).FirstOrDefault();
                if (reihe != null) {
                    reihenBaende.Add(new RELATION_BaendeReihen() {
                        BAND = n.Key,
                        BEZIEHUNG = 1,
                        REIHE = reihe.ID
                    });
                }
            }
        }

        Akteure.AddRange(names.Values);
        RELATION_BaendeAkteure = werkeAkteure;
        RELATION_BaendeReihen = reihenBaende;
        Baende = werke.Values.ToList();
    }

    private void ParseInhalte() {
        var toparse = _alteDB.INHTab;
        var inaut = new Dictionary<long, List<string>>();
        var ingra = new Dictionary<long, List<string>>();
        var intyp = new Dictionary<long, List<Typ>>();
        var inpag = new Dictionary<long, List<Paginierung>>();
        var inhAkteure = new List<RELATION_InhalteAkteure>();
        var inhalte = new List<Inhalte>();
        var idakt = Akteure.Count + 1;
        foreach (var n in toparse) {
            // Seite
            string? seite = n.Value.SEITE != null ? n.Value.SEITE.Split(".").First() : null;

            // Wird auf einen Band verwiesen, den es gar nicht gibt?
            var band = Baende.Where(x => x.ID == n.Value.ID);
            if (band == null || !band.Any()) {
                _logSink.LogLine("Band " + n.Value.ID + " existiert nicht. Herkunft: INHNR " + n.Value.INHNR);
                continue;
            }

            // Typ
            if (n.Value.OBJEKT != null) {
                var tcomposite = n.Value.OBJEKT.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var typ in TYP) {
                    foreach (var tc in tcomposite) {
                        foreach (var s in typ) {
                            if (tc.Contains(s)) {
                                if (!intyp.ContainsKey(n.Value.INHNR)) intyp.Add(n.Value.INHNR, new List<Typ>());
                                intyp[n.Value.INHNR].Add(new Typ() { Value = typ.First() });
                                break;
                            }
                        }
                    }
                }
            }
            
            //Paginierung
            string? pag = null;
            if (!String.IsNullOrWhiteSpace(n.Value.PAG)) {
                foreach (var p in PAG) {
                    if (n.Value.PAG.Contains(p)) {
                        pag = p;
                    }
                }
                if (pag == null) {
                    _logSink.LogLine("Paginierung " + n.Value.PAG + ", INHNR " + n.Value.INHNR + " unzulässig.");
                }
            }
            
            // Graphiker:innen
            if (!String.IsNullOrWhiteSpace(n.Value.AUTORREALNAME)) {
                var composite = n.Value.AUTORREALNAME.Split(new string[] {" u.", " u " }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (composite.Length > 1) {
                    ingra.Add(n.Value.INHNR, new List<string>());
                    foreach (var c in composite) {
                        ingra[n.Value.INHNR].Add(c);
                    }
                // Autor:innen
                } else {
                    composite = n.Value.AUTORREALNAME.Split(new string[] {";" }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    inaut.Add(n.Value.INHNR, new List<string>());
                    foreach (var c in composite) {
                        inaut[n.Value.INHNR].Add(c);
                    }
                }
            }

            float zaehler;
            if (!float.TryParse(n.Value.OBJZAEHL, NumberStyles.Any, CultureInfo.InvariantCulture, out zaehler)) {
                _logSink.LogLine("Objektzähler " + n.Value.OBJZAEHL + " unzulässig. Herkunft INHNR " + n.Value.INHNR);
            }

            inhalte.Add(new Inhalte() {
                ID = n.Value.INHNR,
                BAND = n.Value.ID,
                TITEL = n.Value.TITEL,
                AUTOR = n.Value.AUTOR,
                INCIPIT = n.Value.INCIPIT,
                ANMERKUNGEN = n.Value.ANMERKINH,
                OBJEKTNUMMER = zaehler,
                SEITE = seite,
                PAGINIERUNG = pag,
                TYP = intyp.ContainsKey(n.Value.INHNR) ? intyp[n.Value.INHNR].ToArray() : null, 
                DIGITALISAT = n.Value.BILD
            });
        }


        // Relationen setzen
        foreach (var n in inaut) {
            foreach (var m in n.Value) {
                var akteur = this.Akteure.Where(x => x.NAME == m).FirstOrDefault();
                if (akteur == null) {
                    _logSink.LogLine("Name " + m + " nicht gefunden. Herkunft: Inhalte INHNR  " + n.Key);
                    akteur = new Akteure() {
                        ID = idakt,
                        NAME = m,
                        ANMERKUNGEN = "ÜBERPRÜFEN: Autogeneriert aus HRSGREALNAME (Inhalte INHNR " + n.Key + ")",
                    };
                    Akteure.Add(akteur);
                    idakt++;
                }
                inhAkteure.Add(new RELATION_InhalteAkteure() {
                    INHALT = n.Key,
                    BEZIEHUNG = 1,
                    AKTEUR = akteur.ID
                });
            }
        }


        foreach (var n in ingra) {
            if (n.Value.Count == 2) {

                 // Zeichner:innen
                var m = n.Value[0];
                var zeichner = this.Akteure.Where(x => x.NAME == m).FirstOrDefault();
                if (zeichner == null) {
                    _logSink.LogLine("Name " + m + " nicht gefunden. Herkunft: Inhalte INHNR  " + n.Key);
                    var name = m.Split(',').Reverse();
                    zeichner = new Akteure() {
                        ID = idakt,
                        NAME = m,
                        ANMERKUNGEN = "ÜBERPRÜFEN: Autogeneriert aus HRSGREALNAME (Inhalte INHNR " + n.Key + ")",
                    };
                    Akteure.Add(zeichner);
                    idakt++;
                }
                inhAkteure.Add(new RELATION_InhalteAkteure() {
                    INHALT = n.Key,
                    BEZIEHUNG = 3,
                    AKTEUR = zeichner.ID
                });
                
                // Stecher:innen
                m = n.Value[1];
                var stecher = this.Akteure.Where(x => x.NAME == m).FirstOrDefault();
                if (stecher == null) {
                    _logSink.LogLine("Name " + m + " nicht gefunden. Herkunft: Inhalte INHNR  " + n.Key);
                    var name = m.Split(',').Reverse();
                    stecher = new Akteure() {
                        ID = idakt,
                        NAME = m,
                        ANMERKUNGEN = "ÜBERPRÜFEN: Autogeneriert aus HRSGREALNAME (Inhalte INHNR " + n.Key + ")"
                    };
                    Akteure.Add(stecher);
                    idakt++;
                }
                inhAkteure.Add(new RELATION_InhalteAkteure() {
                    INHALT = n.Key,
                    BEZIEHUNG = 4,
                    AKTEUR = stecher.ID
                });
            } else {

                // Autor:innen
                foreach (var m in n.Value) {
                    var akteur = this.Akteure.Where(x => x.NAME == m).FirstOrDefault();
                    if (akteur == null) {
                        _logSink.LogLine("Name " + m + " nicht gefunden. Herkunft: Inhalte INHNR  " + n.Key);
                        var name = m.Split(',').Reverse();
                        akteur = new Akteure() {
                            ID = idakt,
                            NAME = m,
                            ANMERKUNGEN = "ÜBERPRÜFEN: Autogeneriert aus HRSGREALNAME (Inhalte INHNR " + n.Key + ")",
                        };
                        Akteure.Add(akteur);
                        idakt++;
                    }
                    inhAkteure.Add(new RELATION_InhalteAkteure() {
                        INHALT = n.Key,
                        BEZIEHUNG = 1,
                        AKTEUR = akteur.ID
                    });
                }
            }
        }
        
        RELATION_InhalteAkteure = inhAkteure;
        Inhalte = inhalte;
    }

    public void Save(string fileroot, XDocument? schemafile) {
        SaveFile(fileroot, schemafile);
    }

    public void SaveFile(string fileroot, XDocument? schemafile) {
        var writer = XmlWriter.Create(fileroot + "MDB.xml", new XmlWriterSettings() {
            Indent = true,
            NewLineOnAttributes = false,
            NewLineHandling = NewLineHandling.None, 
        });
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("", "");
        // ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        writer.WriteStartDocument();
        writer.WriteStartElement("dataroot");
        if (schemafile != null) {
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "MittelXMLSchema.xsd");
            schemafile.Save(fileroot + "MittelXMLSchema.xsd");
        }
        saveDocument<Akteure>(writer, Akteure, ns);
        saveDocument<Reihen>(writer, Reihen, ns);
        saveDocument<Baende>(writer, Baende, ns);
        saveDocument<Inhalte>(writer, Inhalte, ns);
        saveDocument<RELATION_BaendeAkteure>(writer, RELATION_BaendeAkteure, ns);
        saveDocument<RELATION_BaendeReihen>(writer, RELATION_BaendeReihen, ns);
        saveDocument<RELATION_InhalteAkteure>(writer, RELATION_InhalteAkteure, ns);
        writer.WriteEndDocument();
        writer.Flush();
        writer.Close();
    }

    private void saveDocument<T>(XmlWriter w, IEnumerable<T> coll, XmlSerializerNamespaces ns) {
        if (coll == null || !coll.Any()) return;
        var akteurS = new XmlSerializer(typeof(T));
        foreach (var n in coll) {
            akteurS.Serialize(w, n, ns);
        }
    }

    private string? trimOrNull(string? totrim) {
        if (String.IsNullOrWhiteSpace(totrim)) return null;
        return totrim.Trim();
    }
}