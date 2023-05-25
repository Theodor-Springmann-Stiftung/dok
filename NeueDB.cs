namespace Musenalm;

using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

class NeueDBLibrary {
    public List<Akteure> Akteure;
    public List<Exemplare> Exemplare;
    public List<Inhalte> Inhalte;
    public List<Orte> Orte;
    public List<Reihen> Reihen;
    public List<Werke> Werke;
    public List<RELATION_InhalteAkteure> RELATION_InhalteAkteure;
    public List<RELATION_WerkeAkteure> RELATION_WerkeAkteure;
    public List<RELATION_WerkeOrte> RELATION_WerkeOrte;
    public List<RELATION_WerkeReihen> RELATION_WerkeReihen;

    private IEnumerable<DATAFile> _dataFiles;
    private AlteDBLibrary _alteDB;
    private LogSink _logSink;

    public NeueDBLibrary(IEnumerable<DATAFile> files, AlteDBLibrary alteDB) {
        _logSink = LogSink.Instance;
        _dataFiles = files;
        _alteDB = alteDB;
        namesFromREALNAMEN();
        namesFromAlmNeu();
        namesFromInhalte();
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
                    Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus REALNAMEN-Tab"
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

    public void namesFromAlmNeu() {
        var orte = new Dictionary<string, Orte>();
        var names = new Dictionary<string, Akteure>();
        var werkeOrte = new List<RELATION_WerkeOrte>();
        var werkeAkteure = new List<RELATION_WerkeAkteure>();
        var exemplare = new List<Exemplare>();
        var reihen = new HashSet<string>();
        var reihenWerke = new List<RELATION_WerkeReihen>();
        var reihenwerke = new Dictionary<long, List<string>>();
        var werke = new Dictionary<long, Werke>();
        var werkeorte = new Dictionary<long, List<string>>();
        var werkedrucker = new Dictionary<long, List<string>>(); 
        var werkehrsg = new Dictionary<long, List<string>>(); 
        var toparse = _alteDB.AlmNeu;
        var idakt = Akteure.Count + 1;
        Regex rgxround = new Regex(@"(?<=\()[^()]*(?=\))");
        Regex rgxeck = new Regex(@"(?<=\()[^()]*(?=\))");
        var rgxfourendnumbers = new Regex(@"\s?(\d{4},\s?\d{4}|\d{4}\/\d{4}|19\d{2}|18\d{2}|17\d{2}|\[oJ\]|Bd \d \[o\.J\.\]|\[o\.J\.\])(-\d)?(\s?\(\d\))?(\s\(2\.\))?(\s1\su\.\s2)?(\s?\[var\.?\]$)?(\s?1\.)?(\(Titelauflage\))?\s?");
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
                                    Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus Verlegern (AlmNeu)"
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
                                Name = o,
                                Land = "Deutschland",
                                Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus Orten (AlmNeu)"
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
                        reihe = e.Replace(m.Last().ToString(), null);
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

            // Exemplare: Werkbacklink, Biblionr, Autopsie, Status, Anmerkungen(?)
            exemplare.Add(new Exemplare() {
                ID = 0,
                Werk = n.Value.NUMMER,
                BiblioNr = trimOrNull(n.Value.BIBLIONR),
                Autopsie = n.Value.AUTOPSIE,
                Status = status,
            });



            // Werke: ID (NUMMER), TITEL, TitelTranskription, Reihe, Jahr, Ausgabe, Struktur, Nachweis, Anmerkungen
            werke.Add(n.Value.NUMMER, new Musenalm.Werke() {
                ID = n.Value.NUMMER,
                Sortiertitel = n.Value.ALMTITEL,
                TitelTranskription = n.Value.ALMTITEL,
                OrtTranskription = ort,
                Jahr = n.Value.JAHR,
                Nachweis = trimOrNull(n.Value.NACHWEIS),
                Struktur = trimOrNull(n.Value.STRUKTUR),
                Anmerkungen = trimOrNull(n.Value.ANMERKUNGEN)
            });
        }

        // Orte, Reihen sortieren und Nummern vergeben
        var ordorte = orte.Values.OrderBy(x => x.Name);
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
                Sortiertitel = n,
                Reihentitel = String.Join(" ", rname)
            });
            idrh++;
        }

        // Relationen setzen
        foreach (var n in werkeorte) {
            foreach (var m in n.Value) {
                werkeOrte.Add(new Musenalm.RELATION_WerkeOrte() {
                    Werk = n.Key,
                    Beziehung = 2, // Erschienen in
                    Ort = orte[m].ID
                });
            }
        }

        foreach (var n in werkedrucker) {
            foreach (var m in n.Value) {
                werkeAkteure.Add(new Musenalm.RELATION_WerkeAkteure() {
                    Werk = n.Key,
                    Beziehung = 7,
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
                        Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus HRSGREALNAME (AlmNeu Nr. " + n.Key + ")"
                    };
                    Akteure.Add(akteur);
                    idakt++;
                }
                werkeAkteure.Add(new Musenalm.RELATION_WerkeAkteure() {
                    Werk = n.Key,
                    Beziehung = 1,
                    Akteur = akteur.ID
                });
            }
        }

        foreach (var n in reihenwerke) {
            foreach (var m in n.Value) {
                var reihe = Reihen.Where(x => x.Sortiertitel == m).FirstOrDefault();
                if (reihe != null) {
                    reihenWerke.Add(new Musenalm.RELATION_WerkeReihen() {
                        Werk = n.Key,
                        Reihe = reihe.ID
                    });
                }
            }
        }

        Akteure.AddRange(names.Values);
        if (Orte == null) Orte = new List<Orte>();
        Orte.AddRange(orte.Values);
        RELATION_WerkeOrte = werkeOrte;
        RELATION_WerkeAkteure = werkeAkteure;
        RELATION_WerkeReihen = reihenWerke;
        Exemplare = exemplare;
        Werke = werke.Values.ToList();

    }

    private void namesFromInhalte() {
        var toparse = _alteDB.INHTab;
        var inaut = new Dictionary<long, List<string>>();
        var ingra = new Dictionary<long, List<string>>();
        var inhAkteure = new List<RELATION_InhalteAkteure>();
        var idakt = Akteure.Count + 1;
        foreach (var n in toparse) {
            // Graphiker:innen
            if (!String.IsNullOrWhiteSpace(n.Value.AUTORREALNAME)) {
                var composite = n.Value.AUTORREALNAME.Split(new string[] {" u." }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (composite.Length > 1) {
                    ingra.Add(n.Value.INHNR, new List<string>());
                    foreach (var c in composite) {
                        ingra[n.Value.INHNR].Add(c);
                    }
                // Autor:innen
                } else {
                    composite = n.Value.AUTORREALNAME.Split(new string[] {" u." }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    inaut.Add(n.Value.INHNR, new List<string>());
                    foreach (var c in composite) {
                        inaut[n.Value.INHNR].Add(c);
                    }
                }
            }
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
                        Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus HRSGREALNAME (Inhalte INHNR " + n.Key + ")"
                    };
                    Akteure.Add(akteur);
                    idakt++;
                }
                inhAkteure.Add(new Musenalm.RELATION_InhalteAkteure() {
                    Inhalt = n.Key,
                    Beziehung = 4,
                    Akteur = akteur.ID
                });
            }
        }


        foreach (var n in ingra) {
            if (n.Value.Count == 2) {

                 // Zeichner
                var m = n.Value[0];
                var zeichner = this.Akteure.Where(x => x.Sortiername == m).FirstOrDefault();
                if (zeichner == null) {
                    _logSink.LogLine("Name " + m + " nicht gefunden. Herkunft: Inhalte INHNR  " + n.Key);
                    var name = m.Split(',').Reverse();
                    zeichner = new Akteure() {
                        ID = idakt,
                        Sortiername = m,
                        Name = String.Join(" ", name),
                        Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus HRSGREALNAME (Inhalte INHNR " + n.Key + ")"
                    };
                    Akteure.Add(zeichner);
                    idakt++;
                }
                inhAkteure.Add(new Musenalm.RELATION_InhalteAkteure() {
                    Inhalt = n.Key,
                    Beziehung = 5,
                    Akteur = zeichner.ID
                });
                
                // Stecher
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
                    Beziehung = 3,
                    Akteur = stecher.ID
                });
            } else {
                foreach (var m in n.Value) {
                    var akteur = this.Akteure.Where(x => x.Sortiername == m).FirstOrDefault();
                    if (akteur == null) {
                        _logSink.LogLine("Name " + m + " nicht gefunden. Herkunft: Inhalte INHNR  " + n.Key);
                        var name = m.Split(',').Reverse();
                        akteur = new Akteure() {
                            ID = idakt,
                            Sortiername = m,
                            Name = String.Join(" ", name),
                            Anmerkungen = "ÜBERPRÜFEN: Autogeneriert aus HRSGREALNAME (Inhalte INHNR " + n.Key + ")"
                        };
                        Akteure.Add(akteur);
                        idakt++;
                    }
                    inhAkteure.Add(new Musenalm.RELATION_InhalteAkteure() {
                        Inhalt = n.Key,
                        Beziehung = 4,
                        Akteur = akteur.ID
                    });
                }
            }
        }

        RELATION_InhalteAkteure = inhAkteure;
    }

    private void SaveFile(string fileroot) {
        var writer = XmlWriter.Create(fileroot + "Gesamt.xml");
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("","");
        writer.WriteStartDocument();
        writer.WriteStartElement("dataroot");
        saveDocument<Akteure>(writer, Akteure, ns);
        saveDocument<Orte>(writer, Orte, ns);
        saveDocument<Reihen>(writer, Reihen, ns);
        saveDocument<Werke>(writer, Werke, ns);
        saveDocument<RELATION_WerkeAkteure>(writer, RELATION_WerkeAkteure, ns);
        saveDocument<RELATION_WerkeOrte>(writer, RELATION_WerkeOrte, ns);
        saveDocument<RELATION_WerkeReihen>(writer, RELATION_WerkeReihen, ns);
        writer.WriteEndDocument();
        writer.Flush();
        writer.Close();
    }

    public void Save(string fileroot) {
        if (Directory.Exists(fileroot)) Directory.Delete(fileroot, true);
        Directory.CreateDirectory(fileroot);
        saveDocument<Akteure>(fileroot + "Akteure_GEN.xml", this.Akteure);
        saveDocument<Orte>(fileroot + "Orte_GEN.xml", this.Orte);
        saveDocument<Werke>(fileroot + "Werke_GEN.xml", this.Werke);
        saveDocument<Reihen>(fileroot + "Reihen_GEN.xml", this.Reihen);
        saveDocument<RELATION_WerkeOrte>(fileroot + "RelationWerkeOrte_GEN.xml", this.RELATION_WerkeOrte);
        saveDocument<RELATION_WerkeAkteure>(fileroot + "RelationenWerkeAkteure_GEN.xml", this.RELATION_WerkeAkteure);
        saveDocument<RELATION_WerkeReihen>(fileroot + "RelationenWerkeReihe_GEN.xml", this.RELATION_WerkeReihen);
        // Order and Save Exemplare
        var e = this.Exemplare.OrderBy(x => x.Werk);
        var id = 1;
        foreach (var n in e) {
            n.ID = id;
            id ++;
        }

        saveDocument<Exemplare>(fileroot + "Exemplare_GEN.xml", e);
        SaveFile(fileroot);
    }

    private void saveDocument<T>(string filename, IEnumerable<T> coll) {
        var writer = XmlWriter.Create(filename);
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("","");
        writer.WriteStartDocument();
        writer.WriteStartElement("dataroot");
        var akteurS = new XmlSerializer(typeof(T));
        foreach (var n in coll) {
            akteurS.Serialize(writer, n, ns);
        }
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
    [XmlElement]
    public long ID;
    [XmlElement]
    public string? Name;
    [XmlElement]
    public string? OrgName;
    [XmlElement]
    public string Sortiername;
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

    public bool ShouldSerializeName() => !String.IsNullOrWhiteSpace(Name);
    public bool ShouldSerializeOrgName() => !String.IsNullOrWhiteSpace(OrgName);
    public bool ShouldSerializeLebensdaten() => !String.IsNullOrWhiteSpace(Lebensdaten);
    public bool ShouldSerializeBeruf() => !String.IsNullOrWhiteSpace(Beruf);
    public bool ShouldSerializePseudonyme() => !String.IsNullOrWhiteSpace(Pseudonyme);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
    public bool ShouldSerializeGND() => !String.IsNullOrWhiteSpace(GND);
}

[XmlRoot("Exemplare")]
public class Exemplare {
    [XmlElement]
    public long ID;
    [XmlElement]
    public long Werk;
    [XmlElement("Biblio-Nr")]
    public string? BiblioNr;
    [XmlElement]
    public bool Autopsie;
    [XmlElement]
    public Status[]? Status;
    [XmlElement]
    public string? URL;
    [XmlElement("Standort und Signatur")]
    public string? StandortSignatur;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeBiblioNr() => !String.IsNullOrWhiteSpace(BiblioNr);
    public bool ShouldSerializeStatus() => Status != null;
    public bool ShouldSerializeURL() => !String.IsNullOrWhiteSpace(URL);
    public bool ShouldSerializeStandortSignatur() => !String.IsNullOrWhiteSpace(StandortSignatur);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("Status")]
public class Status {
    [XmlElement]
    public string? Value;
}

[XmlRoot("Inhalte")]
public class Inhalte {
    [XmlElement]
    public long ID;
    [XmlElement]
    public long Werk;
    [XmlElement]
    public string? Sortiertitel;
    [XmlElement("Titel-Transkription")]
    public string? TitelTranskription;
    [XmlElement("Autor-Transkription")]
    public string? AutorTranskription;
    [XmlElement]
    public string? Paginierung;
    [XmlElement]
    public string? Seite;
    [XmlElement]
    public string? Incipit;
    [XmlElement]
    public string? Anmerkungen;
    [XmlElement]
    public string? Typ;
    [XmlElement]
    public bool? Digitalisat;
    [XmlElement]
    public string? Objektnummer;

    public bool ShouldSerializeSortiertitel() => !String.IsNullOrWhiteSpace(Sortiertitel);
    public bool ShouldSerializeTitelTranskription() => !String.IsNullOrWhiteSpace(TitelTranskription);
    public bool ShouldSerializeAutorTranskription() => !String.IsNullOrWhiteSpace(AutorTranskription);
    public bool ShouldSerializePaginierung() => !String.IsNullOrWhiteSpace(Paginierung);
    public bool ShouldSerializeSeite() => !String.IsNullOrWhiteSpace(Seite);
    public bool ShouldSerializeIncipit() => !String.IsNullOrWhiteSpace(Incipit);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
    public bool ShouldSerializeTyp() => !String.IsNullOrWhiteSpace(Typ);
    public bool ShouldSerializeObjektnummer() => !String.IsNullOrWhiteSpace(Objektnummer);
}

[XmlRoot("Orte")]
public class Orte {
    [XmlElement]
    public long ID;
    [XmlElement]
    public string? Name;
    [XmlElement]
    public string? Land;
    [XmlElement]
    public string? GeoNames;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeName() => !String.IsNullOrWhiteSpace(Name);
    public bool ShouldSerializeLand() => !String.IsNullOrWhiteSpace(Land);
    public bool ShouldSerializeGeoNames() => !String.IsNullOrWhiteSpace(GeoNames);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("Reihen")]
public class Reihen {
    [XmlElement]
    public long ID;
    [XmlElement]
    public string? Reihentitel;
    [XmlElement]
    public string? Sortiertitel;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeReihentitel() => !String.IsNullOrWhiteSpace(Reihentitel);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
    public bool ShouldSerializeSortiertitel() => !String.IsNullOrWhiteSpace(Sortiertitel);
}

[XmlRoot("Werke")]
public class Werke {
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

    public bool ShouldSerializeJahr() => Jahr != null;
    public bool ShouldSerializeAusgabe() => Ausgabe != null;
    public bool ShouldSerializeSortiertitel() => !String.IsNullOrWhiteSpace(Sortiertitel);
    public bool ShouldSerializeTitelTranskription() => !String.IsNullOrWhiteSpace(TitelTranskription);
    public bool ShouldSerializeOrtTranskription() => !String.IsNullOrWhiteSpace(OrtTranskription);
    public bool ShouldSerializeStruktur() => !String.IsNullOrWhiteSpace(Struktur);
    public bool ShouldSerializeNachweis() => !String.IsNullOrWhiteSpace(Nachweis);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("*RELATION_Inhalte-Akteure")]
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

[XmlRoot("*RELATION_Werke-Akteure")]
public class RELATION_WerkeAkteure {
    [XmlElement]
    public long Werk;
    [XmlElement]
    public long Beziehung;
    [XmlElement]
    public long Akteur;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("*RELATION_Werke-Reihen")]
public class RELATION_WerkeReihen {
    [XmlElement]
    public long Werk;
    [XmlElement]
    public long Reihe;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("*RELATION_Werke-Orte")]
public class RELATION_WerkeOrte {
    [XmlElement]
    public long Werk;
    [XmlElement]
    public long Beziehung;
    [XmlElement]
    public long Ort;
    [XmlElement]
    public string Anmerkungen;

    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}