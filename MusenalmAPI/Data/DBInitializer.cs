namespace MusenalmAPI.Data;
using MusenalmAPI.Models;


public static class DbInitializer {

    const string DATASOURCEFOLDER = "../daten_2023-05-25/";

    private static List<string> Typen = new List<string> {
         "Corrigenda",
         "Diagramm" ,
         "Gedicht/Lied",
         "Graphik", 
         "Graphik-Verzeichnis",
         "graph. Anleitung", 
         "graph. Strickanleitung", 
         "graph. Tanzanleitung", 
         "Inhaltsverzeichnis",
         "Kalendarium",
         "Karte", 
         "Musikbeigabe", 
         "Musikbeigaben-Verzeichnis",
         "Motto", 
         "Prosa", 
         "Rätsel", 
         "Sammlung", 
         "Spiegel", 
         "szen. Darstellung", 
         "Tabelle", 
         "Tafel",
         "Titel", 
         "Text", 
         "Trinkspruch", 
         "Umschlag", 
         "Widmung" 
    }; 

    private static List<string> Paginierungen = new List<string>() {
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


    private static readonly List<string> Statuses = new List<string>() {
        "Original vorhanden",
        "Reprint vorhanden",
        "Fremde Herkunft"
    };

    private static readonly List<string> VOC_Akteure = new List<string>() {
        "wurde geschaffen von",
        "wurde geschrieben von",
        "wurde gezeichnet von",
        "wurde gestochen von",
        "wurde herausgegeben von",
        "wurde verlegt von",
        "wurde gedruckt von"
    };

    private static readonly List<string> VOC_Orte = new List<string>() {
        "ist erschienen in",
        "ist verlegt in"
    };

    private static readonly List<string> VOC_Reihen = new List<string>() {
        "ist erschienen als Teil der Reihe"
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
        context.VOC_Akteure.AddRange(VOC_Akteure.Select(x => new VOC_Akteur() { Name = x }));
        context.VOC_Orte.AddRange(VOC_Orte.Select(x => new VOC_Ort() { Name = x }));
        context.VOC_Reihen.AddRange(VOC_Reihen.Select(x => new VOC_Reihe() { Name = x }));
        context.VOC_Statuses.AddRange(Statuses.Select(x => new VOC_Status() { Name = x }));
        context.VOC_Paginierungen.AddRange(Paginierungen.Select((x, y) => new VOC_Paginierung() { Name = x }));
        context.VOC_Typen.AddRange(Typen.Select((x, y) => new VOC_Typ() { Name = x }));
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

        var reihenbez = context.VOC_Reihen.First();
        context.REL_Baende_Reihen.AddRange(nDB.RELATION_BaendeReihen.Select(x => 
            new REL_Band_Reihe() {
                BandID = x.Band,
                ReiheID = x.Reihe,
                VOC_ReiheID = reihenbez.ID,
                Anmerkungen = x.Anmerkungen
            }
        ));
        context.SaveChanges();
        
        context.REL_Baende_Akteue.AddRange(nDB.RELATION_BaendeAkteure.Select(x => { 
            var aB = VOC_Akteure[(int)x.Beziehung - 1];
            var nB = context.VOC_Akteure.SingleOrDefault(x => x.Name == aB).ID;
            return new REL_Band_Akteur() {
                BandID = x.Band,
                AkteurID = x.Akteur,
                VOC_AkteurID = nB,
                Anmerkungen = x.Anmerkungen
            };
        }));
         context.SaveChanges();

        context.REL_Baende_Orte.AddRange(nDB.RELATION_BaendeOrte.Select((x, y) => { 
            var aB = VOC_Orte[(int)x.Beziehung - 1];
            var nB = context.VOC_Orte.SingleOrDefault(x => x.Name == aB).ID;
            return new REL_Band_Ort() {
                BandID = x.Band,
                OrtID = x.Ort,
                VOC_OrtID = nB,
                Anmerkungen = x.Anmerkungen
            };
        }));
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
                    VOC_StatusID = context.VOC_Statuses.Where(x => x.Name == status.Value).First().ID,
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
                VOC_PaginierungID = x.Paginierung != null ? context.VOC_Paginierungen.Where(y => y.Name == x.Paginierung).First().ID : null
            }
        ));
        context.SaveChanges();

        context.REL_Inhalte_Typen.AddRange(nDB.Inhalte.Where(x => x.Typ != null && x.Typ.Any()).SelectMany(x => {
            List<REL_Inhalt_Typ> t = new List<REL_Inhalt_Typ>();
            foreach (var typ in x.Typ!) {
                t.Add(new REL_Inhalt_Typ() { 
                    VOC_TypID = context.VOC_Typen.Where(y => y.Name == typ.Value).First().ID,
                    InhaltID = x.ID
                });
            }
            return t;
        }));
        context.SaveChanges();

        context.REL_Inhalte_Akteure.AddRange(nDB.RELATION_InhalteAkteure.Select(x => {
            var aB = VOC_Akteure[(int)x.Beziehung - 1];
            var nB = context.VOC_Akteure.SingleOrDefault(x => x.Name == aB).ID;
            return new REL_Inhalt_Akteur() {
                AkteurID = x.Akteur,
                InhaltID = x.Inhalt,
                VOC_AkteurID = nB,
                Anmerkungen = x.Anmerkungen
            };
        }));
        context.SaveChanges();

        LogSink.Instance.WriteToConsole(true);

    }
}