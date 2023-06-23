namespace Musenalm;

using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

class NeueDBLibrary {
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
    public List<Exemplare> Exemplare;
    public List<Inhalte> Inhalte;
    public List<Orte> Orte;
    public List<Reihen> Reihen;
    public List<Baende> Baende;
    public List<RELATION_InhalteAkteure> RELATION_InhalteAkteure;
    public List<RELATION_BaendeAkteure> RELATION_BaendeAkteure;
    public List<RELATION_BaendeOrte> RELATION_BaendeOrte;
    public List<RELATION_BaendeReihen> RELATION_BaendeReihen;

    private IEnumerable<DATAFile> _dataFiles;
    private AlteDBLibrary _alteDB;
    private LogSink _logSink;

    public NeueDBLibrary(IEnumerable<DATAFile> files, AlteDBLibrary alteDB) {
        _logSink = LogSink.Instance;
        _dataFiles = files;
        _alteDB = alteDB;
        namesFromREALNAMEN();
        ParseReihenOrteExemplareBaende();
        ParseInhalte();
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
                var name = n.REALNAME.Split(',').Reverse();
                names.Add(n.REALNAME.Trim(), new Akteure() {
                    Name = String.Join(" ", name).Trim(),
                    Sortiername = n.REALNAME.Trim(),
                    Lebensdaten = trimOrNull(n.Daten),
                    Beruf = trimOrNull(n.Beitrag),
                    Pseudonyme = trimOrNull(n.Pseudonym),
                    KSortiername = n.REALNAME.Trim(),
                });
            }
        }
        
        var orderednames = names.OrderBy(x => x.Key).Select(x => x.Value).ToList();

        foreach (var n in notfound) {
            _logSink.LogLine("Name " + n.Item1.Trim() + " nicht gefunden. Herkunft: REALNAME-Tab " + n.Item2.REALNAME);
            if (!names.ContainsKey(n.Item1.Trim())) {
                var name = n.Item1.Split(',').Reverse();
                var akteur = new Akteure() {
                    Name = String.Join(" ", name),
                    Sortiername = n.Item1.Trim(),
                    Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus REALNAMEN-Tab",
                    KSortiername = n.Item1.Trim(),
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
        var orte = new Dictionary<string, Orte>();
        var names = new Dictionary<string, Akteure>();
        var werkeOrte = new List<RELATION_BaendeOrte>();
        var werkeAkteure = new List<RELATION_BaendeAkteure>();
        var exemplare = new List<Exemplare>();
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
            var ort = n.Value.ORT;

            if (!String.IsNullOrWhiteSpace(ort)) {
                var matches = rgxround.Matches(ort);

                // Drucker
                if (matches != null && matches.Any()) {
                    werkedrucker.Add(n.Value.NUMMER, new List<string>());
                    foreach (var m in matches) {
                        ort = ort.Replace("(" + m.ToString() + ")", null);
                        var splittednames = m.ToString().Split(new string[] {"/", ";"}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        foreach (var na in splittednames) {
                            if (!names.ContainsKey(na)) {
                                names.Add(na, new Musenalm.Akteure() {
                                    ID = idakt,
                                    OrgName = na,
                                    Sortiername = na,
                                    Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus Verlegern (AlmNeu)",
                                    KSortiername = na
                                });
                                idakt++;
                            }
                            werkedrucker[n.Value.NUMMER].Add(na);
                        }
                    }
                }

                // Orte
                var splittedplaces = ort.Split(new string[]{" u.", " und", ";", ","}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (splittedplaces != null && splittedplaces.Any()) {
                    werkeorte.Add(n.Value.NUMMER, new List<string>());
                    foreach (var o in splittedplaces) {
                        if (!orte.ContainsKey(o)) {
                            orte.Add(o, new Musenalm.Orte {
                                ID = 0,
                                Sortiername = o,
                                Land = "Deutschland",
                                Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus Orten (AlmNeu)",
                                KSortiername = o
                            });
                        }
                        werkeorte[n.Value.NUMMER].Add(o);
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
                    var rname = reihe.Split(',').Reverse();
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

            // Exemplare: Bandbacklink, Biblionr, Gesichtet, Status, Anmerkungen(?)
            exemplare.Add(new Exemplare() {
                ID = 0,
                Band = n.Value.NUMMER,
                BiblioNr = trimOrNull(n.Value.BIBLIONR),
                Gesichtet = n.Value.AUTOPSIE,
                Status = status,
            });

            // Ausgabe
            var ausg = 1;
            if (!String.IsNullOrWhiteSpace(n.Value.REIHENTITEL)) {
                var m = rgxausgabe.Matches(n.Value.REIHENTITEL);
                if (m != null && m.Count > 0) ausg = Int32.Parse(m.Last().ToString());
            }

            // Baende: ID (NUMMER), TITEL, TitelTranskription, Reihe, Jahr, Ausgabe, Struktur, Nachweis, Anmerkungen
            werke.Add(n.Value.NUMMER, new Musenalm.Baende() {
                ID = n.Value.NUMMER,
                Sortiertitel = trimOrNull(n.Value.ALMTITEL),
                TitelTranskription = trimOrNull(n.Value.ALMTITEL),
                OrtTranskription = ort,
                Jahr = n.Value.JAHR,
                Ausgabe = ausg,
                Nachweis = trimOrNull(n.Value.NACHWEIS),
                Struktur = trimOrNull(n.Value.STRUKTUR),
                Anmerkungen = trimOrNull(n.Value.ANMERKUNGEN),
                KSortiertitel = trimOrNull(n.Value.ALMTITEL),
                AbgeschnittenJahr = abschnitte.ContainsKey(n.Value.NUMMER) ? abschnitte[n.Value.NUMMER] : null
            });
        }

        // Orte, Reihen sortieren und Nummern vergeben
        var ordorte = orte.Values.OrderBy(x => x.Sortiername);
        var idort = 1;
        foreach (var n in ordorte) {
            n.ID = idort;
            idort++;
        }

        var ordrh = reihen.OrderBy(x => x);
        var idrh = 1;
        foreach (var n in ordrh) {
            var rname = n.Split(',').Reverse();
            if (Reihen == null) Reihen = new List<Reihen>();
            Reihen.Add(new Musenalm.Reihen() {
                ID = idrh,
                Sortiername = n,
                Name = String.Join(" ", rname),
                KSortiername = n
            });
            idrh++;
        }

        // Relationen setzen
        foreach (var n in werkeorte) {
            foreach (var m in n.Value) {
                werkeOrte.Add(new Musenalm.RELATION_BaendeOrte() {
                    Band = n.Key,
                    Beziehung = 2, // Erschienen in
                    Ort = orte[m].ID
                });
            }
        }

        foreach (var n in werkedrucker) {
            foreach (var m in n.Value) {
                werkeAkteure.Add(new Musenalm.RELATION_BaendeAkteure() {
                    Band = n.Key,
                    Beziehung = 6,
                    Akteur = names[m].ID
                });
            }
        }

        foreach (var n in werkehrsg) {
            foreach (var m in n.Value) {
                var akteur = this.Akteure.Where(x => x.Sortiername == m).FirstOrDefault();
                if (akteur == null) {
                    _logSink.LogLine("Name " + m + " nicht gefunden. Herkunft: AlmNeu Nr. " + n.Key);
                    var name = m.Split(',').Reverse();
                    akteur = new Akteure() {
                        ID = idakt,
                        Sortiername = m,
                        Name = String.Join(" ", name),
                        Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus HRSGREALNAME (AlmNeu Nr. " + n.Key + ")",
                        KSortiername = m
                    };
                    Akteure.Add(akteur);
                    idakt++;
                }
                werkeAkteure.Add(new Musenalm.RELATION_BaendeAkteure() {
                    Band = n.Key,
                    Beziehung = 5,
                    Akteur = akteur.ID
                });
            }
        }

        foreach (var n in reihenwerke) {
            foreach (var m in n.Value) {
                var reihe = Reihen.Where(x => x.Sortiername == m).FirstOrDefault();
                if (reihe != null) {
                    reihenBaende.Add(new Musenalm.RELATION_BaendeReihen() {
                        Band = n.Key,
                        Reihe = reihe.ID
                    });
                }
            }
        }

        Akteure.AddRange(names.Values);
        if (Orte == null) Orte = new List<Orte>();
        Orte.AddRange(orte.Values);
        RELATION_BaendeOrte = werkeOrte;
        RELATION_BaendeAkteure = werkeAkteure;
        RELATION_BaendeReihen = reihenBaende;
        Exemplare = exemplare;
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
                Band = n.Value.ID,
                TitelTranskription = n.Value.TITEL,
                AutorTranskription = n.Value.AUTOR,
                IncipitTranskription = n.Value.INCIPIT,
                Anmerkungen = n.Value.ANMERKINH,
                Objektnummer = zaehler,
                Seite = seite,
                Paginierung = pag,
                Typ = intyp.ContainsKey(n.Value.INHNR) ? intyp[n.Value.INHNR].ToArray() : null, 
                Digitalisat = n.Value.BILD
            });
        }


        // Relationen setzen
        foreach (var n in inaut) {
            foreach (var m in n.Value) {
                var akteur = this.Akteure.Where(x => x.Sortiername == m).FirstOrDefault();
                if (akteur == null) {
                    _logSink.LogLine("Name " + m + " nicht gefunden. Herkunft: Inhalte INHNR  " + n.Key);
                    var name = m.Split(',').Reverse();
                    akteur = new Akteure() {
                        ID = idakt,
                        Sortiername = m,
                        Name = String.Join(" ", name),
                        Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus HRSGREALNAME (Inhalte INHNR " + n.Key + ")",
                        KSortiername = m
                    };
                    Akteure.Add(akteur);
                    idakt++;
                }
                inhAkteure.Add(new Musenalm.RELATION_InhalteAkteure() {
                    Inhalt = n.Key,
                    Beziehung = 1,
                    Akteur = akteur.ID
                });
            }
        }


        foreach (var n in ingra) {
            if (n.Value.Count == 2) {

                 // Zeichner:innen
                var m = n.Value[0];
                var zeichner = this.Akteure.Where(x => x.Sortiername == m).FirstOrDefault();
                if (zeichner == null) {
                    _logSink.LogLine("Name " + m + " nicht gefunden. Herkunft: Inhalte INHNR  " + n.Key);
                    var name = m.Split(',').Reverse();
                    zeichner = new Akteure() {
                        ID = idakt,
                        Sortiername = m,
                        Name = String.Join(" ", name),
                        Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus HRSGREALNAME (Inhalte INHNR " + n.Key + ")",
                        KSortiername = m
                    };
                    Akteure.Add(zeichner);
                    idakt++;
                }
                inhAkteure.Add(new Musenalm.RELATION_InhalteAkteure() {
                    Inhalt = n.Key,
                    Beziehung = 3,
                    Akteur = zeichner.ID
                });
                
                // Stecher:innen
                m = n.Value[1];
                var stecher = this.Akteure.Where(x => x.Sortiername == m).FirstOrDefault();
                if (stecher == null) {
                    _logSink.LogLine("Name " + m + " nicht gefunden. Herkunft: Inhalte INHNR  " + n.Key);
                    var name = m.Split(',').Reverse();
                    stecher = new Akteure() {
                        ID = idakt,
                        Sortiername = m,
                        Name = String.Join(" ", name),
                        Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus HRSGREALNAME (Inhalte INHNR " + n.Key + ")"
                    };
                    Akteure.Add(stecher);
                    idakt++;
                }
                inhAkteure.Add(new Musenalm.RELATION_InhalteAkteure() {
                    Inhalt = n.Key,
                    Beziehung = 4,
                    Akteur = stecher.ID
                });
            } else {

                // Autor:innen
                foreach (var m in n.Value) {
                    var akteur = this.Akteure.Where(x => x.Sortiername == m).FirstOrDefault();
                    if (akteur == null) {
                        _logSink.LogLine("Name " + m + " nicht gefunden. Herkunft: Inhalte INHNR  " + n.Key);
                        var name = m.Split(',').Reverse();
                        akteur = new Akteure() {
                            ID = idakt,
                            Sortiername = m,
                            Name = String.Join(" ", name),
                            Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus HRSGREALNAME (Inhalte INHNR " + n.Key + ")",
                            KSortiername = m
                        };
                        Akteure.Add(akteur);
                        idakt++;
                    }
                    inhAkteure.Add(new Musenalm.RELATION_InhalteAkteure() {
                        Inhalt = n.Key,
                        Beziehung = 1,
                        Akteur = akteur.ID
                    });
                }
            }
        }
        
        RELATION_InhalteAkteure = inhAkteure;
        Inhalte = inhalte;
    }

    public void Save(string fileroot, string schemafile) {
        if (Directory.Exists(fileroot)) Directory.Delete(fileroot, true);
        Directory.CreateDirectory(fileroot);
        // Order and Save Exemplare
        var e = this.Exemplare.OrderBy(x => x.Band);
        var id = 1;
        foreach (var n in e) {
            n.ID = id;
            id ++;
        }
        SaveFile(fileroot, schemafile);
    }

    private void SaveFile(string fileroot, string schemafile) {
        var writer = XmlWriter.Create(fileroot + "Gesamt.xml", new XmlWriterSettings() {
            Indent = true,
            NewLineOnAttributes = false,
            NewLineHandling = NewLineHandling.None, 
        });
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("", "");
        // ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        writer.WriteStartDocument();
        writer.WriteStartElement("dataroot");
        writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
        writer.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, schemafile);
        saveDocument<Akteure>(writer, Akteure, ns);
        saveDocument<Orte>(writer, Orte, ns);
        saveDocument<Reihen>(writer, Reihen, ns);
        saveDocument<Baende>(writer, Baende, ns);
        saveDocument<Inhalte>(writer, Inhalte, ns);
        saveDocument<RELATION_BaendeAkteure>(writer, RELATION_BaendeAkteure, ns);
        saveDocument<RELATION_BaendeOrte>(writer, RELATION_BaendeOrte, ns);
        saveDocument<RELATION_BaendeReihen>(writer, RELATION_BaendeReihen, ns);
        saveDocument<RELATION_InhalteAkteure>(writer, RELATION_InhalteAkteure, ns);
        saveDocument<Exemplare>(writer, Exemplare, ns);
        writer.WriteEndDocument();
        writer.Flush();
        writer.Close();
    }

    private void saveDocument<T>(XmlWriter w, IEnumerable<T> coll, XmlSerializerNamespaces ns) {
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

[XmlRoot("Akteure")]
public class Akteure {
    // [XmlAttribute(Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
    // public string noNamespaceSchemaLocation = "../muster/Akteure.xsd";

    [XmlElement]
    public long ID;
    [XmlElement]
    public string? Name;
    [XmlElement]
    public string? OrgName;
    [XmlElement]
    public string? Sortiername;
    [XmlElement]
    public string? Lebensdaten;
    [XmlElement]
    public string? Beruf;
    [XmlElement]
    public string? Pseudonyme;
    [XmlElement]
    public string? Anmerkungen;
    [XmlElement]
    public string? GND;
    [XmlElement]
    public string? KSortiername;

    public bool ShouldSerializeName() => !String.IsNullOrWhiteSpace(Name);
    public bool ShouldSerializeOrgName() => !String.IsNullOrWhiteSpace(OrgName);
    public bool ShouldSerializeLebensdaten() => !String.IsNullOrWhiteSpace(Lebensdaten);
    public bool ShouldSerializeBeruf() => !String.IsNullOrWhiteSpace(Beruf);
    public bool ShouldSerializePseudonyme() => !String.IsNullOrWhiteSpace(Pseudonyme);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
    public bool ShouldSerializeGND() => !String.IsNullOrWhiteSpace(GND);
    public bool ShouldSerializeSortiername() => !String.IsNullOrWhiteSpace(Sortiername);
    public bool ShouldSerializeksortiername() => !String.IsNullOrWhiteSpace(KSortiername);
}

[XmlRoot("Exemplare")]
public class Exemplare {
    // [XmlAttribute(Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
    // [XmlAttribute("xsi:noNamespaceSchemaLocation")]
    // public string noNamespaceSchemaLocation = "../muster/Exemplare.xsd";

    [XmlElement]
    public long ID;
    [XmlElement]
    public long Band;
    [XmlElement("Biblio-Nr")]
    public string? BiblioNr;
    [XmlElement]
    public bool Gesichtet;
    [XmlElement]
    public Status[]? Status;
    [XmlElement]
    public string? URL;
    [XmlElement("Standort und Signatur")]
    public string? StandortSignatur;
    [XmlElement]
    public string? Anmerkungen;
    [XmlElement]
    public string? Struktur;

    public bool ShouldSerializeBiblioNr() => !String.IsNullOrWhiteSpace(BiblioNr);
    public bool ShouldSerializeStatus() => Status != null;
    public bool ShouldSerializeURL() => !String.IsNullOrWhiteSpace(URL);
    public bool ShouldSerializeStruktur() => !String.IsNullOrWhiteSpace(Struktur);
    public bool ShouldSerializeStandortSignatur() => !String.IsNullOrWhiteSpace(StandortSignatur);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("Status")]
public class Status {
    [XmlElement]
    public string? Value;
}

[XmlRoot("Typ")]
public class Typ {
    [XmlElement]
    public string? Value;
}

[XmlRoot("Paginierung")]
public class Paginierung {
    [XmlElement]
    public string? Value;
}

[XmlRoot("Inhalte")]
public class Inhalte {
    [XmlElement]
    public long ID;
    [XmlElement]
    public long? Band;
    [XmlElement]
    public string? TitelTranskription;
    [XmlElement]
    public string? AutorTranskription;
    [XmlElement]
    public string? Paginierung;
    [XmlElement]
    public string? Seite;
    [XmlElement]
    public string? IncipitTranskription;
    [XmlElement]
    public string? Anmerkungen;
    [XmlElement]
    public Typ[]? Typ;
    [XmlElement]
    public bool? Digitalisat;
    [XmlElement]
    public float? Objektnummer;
    [XmlElement]
    public string? KSortiertitel;

    public bool ShouldSerializeKSortiertitel() => !String.IsNullOrWhiteSpace(KSortiertitel);
    public bool ShouldSerializeTitelTranskription() => !String.IsNullOrWhiteSpace(TitelTranskription);
    public bool ShouldSerializeAutorTranskription() => !String.IsNullOrWhiteSpace(AutorTranskription);
    public bool ShouldSerializePaginierung() => Paginierung != null;
    public bool ShouldSerializeSeite() => !String.IsNullOrWhiteSpace(Seite);
    public bool ShouldSerializeIncipitTranskription() => !String.IsNullOrWhiteSpace(IncipitTranskription);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
    public bool ShouldSerializeTyp() => Typ != null;
    public bool ShouldSerializeObjektnummer() => Objektnummer != null;
}

[XmlRoot("Orte")]
public class Orte {
    [XmlElement]
    public long ID;
    [XmlElement]
    public string? Sortiername;
    [XmlElement]
    public string? KSortiername;
    [XmlElement]
    public string? Land;
    [XmlElement]
    public string? GeoNames;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeSortiername() => !String.IsNullOrWhiteSpace(Sortiername);
    public bool ShouldSerializeKSortiername() => !String.IsNullOrWhiteSpace(KSortiername);
    public bool ShouldSerializeLand() => !String.IsNullOrWhiteSpace(Land);
    public bool ShouldSerializeGeoNames() => !String.IsNullOrWhiteSpace(GeoNames);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("Reihen")]
public class Reihen {
    [XmlElement]
    public long ID;
    [XmlElement]
    public string? Name;
    [XmlElement]
    public string? Sortiername;
    [XmlElement]
    public string? KSortiername;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeReihentitel() => !String.IsNullOrWhiteSpace(Name);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
    public bool ShouldSerializeSortiertitel() => !String.IsNullOrWhiteSpace(Sortiername);
    public bool ShouldSerializeKSortiertitel() => !String.IsNullOrWhiteSpace(KSortiername);
}

[XmlRoot("Baende")]
public class Baende {
    [XmlElement]
    public long ID;
    [XmlElement]
    public string? Sortiertitel;
    [XmlElement("Titel-Transkription")]
    public string? TitelTranskription;
    [XmlElement("Ort-Transkription")]
    public string? OrtTranskription;
    [XmlElement]
    public long? Jahr = null;
    [XmlElement]
    public int? Ausgabe;
    [XmlElement]
    public string? Struktur;
    [XmlElement]
    public string? Nachweis;
    [XmlElement]
    public string? Anmerkungen;
    [XmlElement]
    public string? KSortiertitel;
    [XmlElement]
    public string? AbgeschnittenJahr;

    public bool ShouldSerializeJahr() => Jahr != null;
    public bool ShouldSerializeAusgabe() => Ausgabe != null;
    public bool ShouldSerializeSortiertitel() => !String.IsNullOrWhiteSpace(Sortiertitel);
    public bool ShouldSerializeKSortiertitel() => !String.IsNullOrWhiteSpace(KSortiertitel);
    public bool ShouldSerializeTitelTranskription() => !String.IsNullOrWhiteSpace(TitelTranskription);
    public bool ShouldSerializeOrtTranskription() => !String.IsNullOrWhiteSpace(OrtTranskription);
    public bool ShouldSerializeStruktur() => !String.IsNullOrWhiteSpace(Struktur);
    public bool ShouldSerializeNachweis() => !String.IsNullOrWhiteSpace(Nachweis);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
    public bool ShouldSerializeAbgeschnittenJahr() => !String.IsNullOrWhiteSpace(AbgeschnittenJahr);
}

[XmlRoot("*RELATION_InhalteAkteure")]
public class RELATION_InhalteAkteure {
    [XmlElement]
    public long Inhalt;
    [XmlElement]
    public long Beziehung;
    [XmlElement]
    public long Akteur;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("*RELATION_BaendeAkteure")]
public class RELATION_BaendeAkteure {
    [XmlElement]
    public long Band;
    [XmlElement]
    public long Beziehung;
    [XmlElement]
    public long Akteur;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("*RELATION_BaendeReihen")]
public class RELATION_BaendeReihen {
    [XmlElement]
    public long Band;
    [XmlElement]
    public long Reihe;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("*RELATION_BaendeOrte")]
public class RELATION_BaendeOrte {
    [XmlElement]
    public long Band;
    [XmlElement]
    public long Beziehung;
    [XmlElement]
    public long Ort;
    [XmlElement]
    public string Anmerkungen;

    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}