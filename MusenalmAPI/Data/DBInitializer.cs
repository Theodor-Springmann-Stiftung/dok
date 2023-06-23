namespace MusenalmAPI.Data;
using MusenalmAPI.Models;


public static class DbInitializer {

    const string DATASOURCEFOLDER = "../daten_2023-05-25/";

    private static Dictionary<string, long> Typen = new Dictionary<string, long> {
        { "Corrigenda", 1},
        { "Diagramm", 2},
        { "Gedicht/Lied", 3},
        { "Graphik", 4 }, 
        { "Graphik-Verzeichnis", 5},
        { "graph. Anleitung", 6}, 
        { "graph. Strickanleitung", 7}, 
        { "graph. Tanzanleitung", 8}, 
        { "Inhaltsverzeichnis", 9},
        { "Kalendarium", 10},
        { "Karte", 11}, 
        { "Musikbeigabe", 12}, 
        { "Musikbeigaben-Verzeichnis", 13},
        { "Motto", 14}, 
        { "Prosa", 15}, 
        { "Rätsel", 16}, 
        { "Sammlung", 17}, 
        { "Spiegel", 18}, 
        { "szen. Darstellung", 19}, 
        { "Tabelle", 20}, 
        { "Tafel", 21},
        { "Titel", 22}, 
        { "Text", 23}, 
        { "Trinkspruch", 24}, 
        { "Umschlag", 25}, 
        { "Widmung", 26 } 
    }; 

    private static Dictionary<string, long> Paginierungen = new Dictionary<string, long>() {
        { "ar", 1},
        { "röm", 2},
        { "ar1", 3},
        { "ar2", 4},
        { "ar3", 5},
        { "ar4", 6},
        { "ar5", 7},
        { "ar6", 8},
        { "ar7", 9},
        { "röm1", 10},
        { "röm2", 11},
        { "röm3", 12},
        { "röm4", 13}
    };


    private static readonly Dictionary<string, long> Statuses = new Dictionary<string, long>() {
        { "Original vorhanden" , 1 },
        { "Reprint vorhanden", 2 },
        { "Fremde Herkunft", 3 },
    };

    private static readonly List<VOC_Akteur> VOC_Akteure = new List<VOC_Akteur>() {
        new VOC_Akteur() { ID = 1, Beziehung = "wurde geschaffen von" },
        new VOC_Akteur() { ID = 2, Beziehung = "wurde geschrieben von" },
        new VOC_Akteur() { ID = 3 , Beziehung = "wurde gezeichnet von" },
        new VOC_Akteur() { ID = 4, Beziehung = "wurde gestochen von" },
        new VOC_Akteur() { ID = 5, Beziehung = "wurde herausgegeben von" },
        new VOC_Akteur() { ID = 6, Beziehung = "wurde verlegt von" },
        new VOC_Akteur() { ID = 7, Beziehung = "wurde gedruckt von"}
    };

    private static readonly List<VOC_Ort> VOC_Orte = new List<VOC_Ort>() {
        new VOC_Ort() { ID = 1, Beziehung = "ist erschienen in" },
        new VOC_Ort() { ID = 2, Beziehung = "ist verlegt in"}
    };

    private static readonly List<VOC_Reihe> VOC_Reihen = new List<VOC_Reihe>() {
        new VOC_Reihe() { ID = 1, Beziehung = "ist erschienen als Teil der Reihe" }
    };

    public static void Initialize(MusenalmContext context)
    {
        if (context.REL_Inhalte_Akteure.Any()) return;
        MusenalmConverter.LogSink.Instance.WriteToConsole(false);
        var data = DATAFile.GenerateDataFilesDirectory(DATASOURCEFOLDER);
        var aDB = new MusenalmConverter.Models.AlteDBXML.AlteDBXMLLibrary(data);
        var nDB = new MusenalmConverter.Models.NeueDBXML.NeueDBXMLLibrary(data, aDB);

        LogSink.Instance.WriteToConsole(false);

        // Vokabular und feste Typen
        context.VOC_Akteure.AddRange(VOC_Akteure);
        context.VOC_Orte.AddRange(VOC_Orte);
        context.VOC_Reihen.AddRange(VOC_Reihen);
        context.Statuses.AddRange(Statuses.Select(x => new VOC_Status() { ID = x.Value, Name = x.Key}));
        context.Paginierungen.AddRange(Paginierungen.Select((x, y) => new VOC_Paginierung() { ID = x.Value, Name = x.Key }));
        context.Typen.AddRange(Typen.Select((x, y) => new VOC_Typ() { ID = x.Value, Name = x.Key}));
        context.SaveChanges();

        context.Reihen.AddRange(nDB.Reihen.Select( x => 
            new Reihe() {
                ID = x.ID,
                Name = x.Name,
                Sortiername = x.Sortiername,
                Anmerkungen = x.Anmerkungen
            }
        ));

        context.Akteure.AddRange(nDB.Akteure.Select(x =>
            new Akteur() {
                ID = x.ID,
                Name = x.Name,
                OrgName = x.OrgName,
                Sortiername = x.Sortiername,
                Lebensdaten = x.Lebensdaten,
                Beruf = x.Beruf,
                Pseudonyme = x.Pseudonyme,
                Anmerkungen = x.Anmerkungen,
                GND = x.GND
            }
        ));

        context.Orte.AddRange(nDB.Orte.Select(x =>
            new Ort() {
                ID = x.ID,
                Sortiername = x.Sortiername,
                Land = x.Land,
                GeoNames = x.GeoNames,
                Anmerkungen = x.Anmerkungen
            }
        ));
        context.SaveChanges();

        foreach (var x in nDB.Baende) {
            context.Baende.Add( new Band() {
                ID = x.ID,
                TitelTranskription = x.TitelTranskription,
                OrtTranskription = x.OrtTranskription,
                Jahr = x.Jahr,
                AusgabeBand = x.Ausgabe.ToString(),
                Struktur = x.Struktur,
                Nachweis = x.Nachweis,
                Anmerkungen = x.Anmerkungen,
                AbgeschnittenJahr = x.AbgeschnittenJahr
            });
        }
        context.SaveChanges();

        context.REL_Baende_Reihen.AddRange(nDB.RELATION_BaendeReihen.Select(x => 
            new REL_Band_Reihe() {
                BandID = x.Band,
                ReiheID = x.Reihe,
                VOC_ReiheID = 1,
                Anmerkungen = x.Anmerkungen
            }
        ));
         context.SaveChanges();
        
        context.REL_Baende_Akteue.AddRange(nDB.RELATION_BaendeAkteure.Select(x => 
            new REL_Band_Akteur() {
                BandID = x.Band,
                AkteurID = x.Akteur,
                VOC_AkteurID = x.Beziehung,
                Anmerkungen = x.Anmerkungen
            }
        ));
         context.SaveChanges();

        context.REL_Baende_Orte.AddRange(nDB.RELATION_BaendeOrte.Select((x, y) => 
            new REL_Band_Ort() {
                BandID = x.Band,
                OrtID = x.Ort,
                VOC_OrtID = x.Beziehung,
                Anmerkungen = x.Anmerkungen
            }
        ));
        context.SaveChanges();

        context.Exemplare.AddRange(nDB.Exemplare.Select(x => 
            new Exemplar() {
                ID = x.ID,
                BandID = x.Band,
                BiblioNr = x.BiblioNr,
                Gesichtet = x.Gesichtet,
                URL = x.URL,
                StandortSignatur = x.StandortSignatur,
                Anmerkungen = x.Anmerkungen,
                Struktur = x.Struktur
            }
        ));
        context.SaveChanges();

        context.REL_Exemplare_Statuses.AddRange(nDB.Exemplare.Where(x => x.Status != null && x.Status.Any()).SelectMany(x => {
            var s = new List<REL_Exemplar_Status>();
            foreach (var status in x.Status!) {
                s.Add(new REL_Exemplar_Status() {
                    VOC_StatusID = Statuses[status.Value],
                    ExemplarID = x.ID
                });
            }
            return s;
        }));
        context.SaveChanges();

        context.Inhalte.AddRange(nDB.Inhalte.Select(x => 
            new Inhalt() {
                ID = x.ID,
                BandID = x.Band,
                TitelTranskription = x.TitelTranskription,
                AutorTranskription = x.AutorTranskription,
                IncipitTranskription = x.IncipitTranskription,
                Seite = x.Seite,
                Anmerkungen = x.Anmerkungen,
                Digitalisat = x.Digitalisat,
                Objektnummer = x.Objektnummer,
                VOC_PaginierungID = x.Paginierung != null ? Paginierungen[x.Paginierung!] : null
            }
        ));
        context.SaveChanges();

        context.REL_Inhalte_Typen.AddRange(nDB.Inhalte.Where(x => x.Typ != null && x.Typ.Any()).SelectMany(x => {
            List<REL_Inhalt_Typ> t = new List<REL_Inhalt_Typ>();
            foreach (var typ in x.Typ!) {
                t.Add(new REL_Inhalt_Typ() { 
                    VOC_TypID = Typen[typ.Value],
                    InhaltID = x.ID
                });
            }
            return t;
        }));
        context.SaveChanges();

        context.REL_Inhalte_Akteure.AddRange(nDB.RELATION_InhalteAkteure.Select(x => 
            new REL_Inhalt_Akteur() {
                AkteurID = x.Akteur,
                InhaltID = x.Inhalt,
                VOC_AkteurID = x.Beziehung,
                Anmerkungen = x.Anmerkungen
            }
        ));
        context.SaveChanges();

        LogSink.Instance.WriteToConsole(true);

    }
}