namespace MusenalmConverter.API;

using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using MusenalmConverter.API.Models;
using MusenalmConverter.Migration.MittelDBXML;

public class APICaller {


    // TODO:
    // Körperschaften
    // Reihe und Bände verlinken
    private APICONSTANTS SERVERDATA = new LOCALHOSTCONSTANTS();
    
    private MittelDBXMLLibrary _data;
    private HttpClient _httpClient;

    public List<Person> Persons;
    public List<Reihentitel> Reihen;
    public List<Band> Baende;

    public ConcurrentDictionary<string, Person> rPersons;
    public ConcurrentDictionary<string, Reihentitel> rReihen;
    public ConcurrentDictionary<string, Band> rBaende;

    public APICaller(MittelDBXMLLibrary data) {
        _data = data;
        _httpClient = new();
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.BaseAddress = new Uri(SERVERDATA.URI);
    }

    public void CreateActorData() {
        Persons = new();
        foreach (var a in _data.Akteure) {
            if (!String.IsNullOrWhiteSpace(a.NAME) && 
                (_data.RELATION_BaendeAkteure.Where(x => x.AKTEUR == a.ID).Any() ||
                 _data.RELATION_InhalteAkteure.Where(x => x.AKTEUR == a.ID).Any())) {
                if (!a.ORGANISATION.HasValue || !a.ORGANISATION.Value) {
                    // Personen
                    var lebensdaten = trimOrNull(a.LEBENSDATEN);
                    LiteralProperty? geburt = null, tod = null;
                    if (lebensdaten != null) {
                        var splitlebensdaten = lebensdaten.Split(new string[] { "-", "–"}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        geburt = splitlebensdaten.First() == "?" ? null : new LiteralProperty(splitlebensdaten.First());
                        if (splitlebensdaten.Length > 1) {
                            var todesd = String.Concat(splitlebensdaten.TakeLast(splitlebensdaten.Length-1));
                            tod = todesd == "?" ? null : new LiteralProperty(todesd);
                        }
                    }
                    LiteralProperty[]? nw = null;
                    var nachweis = trimOrNull(a.NACHWEIS);
                    if (!String.IsNullOrWhiteSpace(nachweis)) {
                        var splitnachweis = nachweis.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        if (splitnachweis != null && splitnachweis.Any()) {
                            nw = splitnachweis.Select(x => new LiteralProperty(x)).ToArray();
                        }
                    }
                    LiteralProperty[]? ps = null;
                    var pseudonym = trimOrNull(a.PSEUDONYM);
                    if (!String.IsNullOrWhiteSpace(pseudonym)) {
                        var splitps = pseudonym.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        if (splitps != null && splitps.Any()) {
                            ps = splitps.Select(x => new LiteralProperty(x)).ToArray();
                        }
                    }
                    Persons.Add(new Person(SERVERDATA, a.NAME.Trim()) {
                        Pseudonym = ps,
                        Nachweis = nw,
                        Beruf = String.IsNullOrWhiteSpace(a.BERUF) ? null : new LiteralProperty[] { new LiteralProperty( a.BERUF )},
                        Geburtsdatum = geburt == null ? null : new LiteralProperty[]{ geburt },
                        Sterbedatum = tod == null ? null : new LiteralProperty[]{ tod },
                        Anmerkung = String.IsNullOrWhiteSpace(a.ANMERKUNGEN) ? null : new HtmlProperty[] { new HtmlProperty( a.ANMERKUNGEN )},
                        Nummer = new LiteralProperty[] { new LiteralProperty(a.ID.ToString())}
                    });
                } else {
                    // Körperschaften

                }
            }
        }
    }

    public void CreateReihenData() {
        Reihen = new();
        foreach (var r in _data.Reihen) {
            Reihen.Add(new Reihentitel(SERVERDATA, r.NAME.Trim()) {
                Anmerkung = String.IsNullOrWhiteSpace(r.ANMERKUNGEN) ? null : new HtmlProperty[] { new HtmlProperty(r.ANMERKUNGEN) },
                Nummer = new LiteralProperty[] { new LiteralProperty(r.ID.ToString()) }
            });
        }
    }

    public void CreateBaendeData() {
        Baende = new();
        foreach (var b in _data.Baende) {
            var akt = _data.RELATION_BaendeAkteure.Where(x => x.BAND == b.ID);
            var rei = _data.RELATION_BaendeReihen.Where(x => x.BAND == b.ID);

            var rn = rei.Select(x => rReihen[x.REIHE.ToString()]?.ID).ToArray();
            var hrsg = akt.Where( x => x.BEZIEHUNG == 5).Select(x => rPersons[x.AKTEUR.ToString()]?.ID).ToArray();

            Baende.Add(new Band(SERVERDATA, b.TITEL) {
                Reihe = rn == null || !rn.Any() ? null : rn.Where(x => x != null).Select(x => new ItemProperty((int)x)).ToArray(),
                Herausgeber = hrsg == null || !hrsg.Any() ? null : hrsg.Where(x => x != null).Select(x => new ItemProperty((int)x)).ToArray(),
                Norm = String.IsNullOrWhiteSpace(b.NORM) ? null : new LiteralProperty[] { new LiteralProperty(b.NORM) },
                Nachweis = String.IsNullOrWhiteSpace(b.NACHWEIS) ? null : b.NACHWEIS.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => new LiteralProperty(x)).ToArray(),
                Nummer = new LiteralProperty[] { new LiteralProperty(b.ID.ToString()) },
                Herausgeberangabe = String.IsNullOrWhiteSpace(b.HERAUSGEBER) ? null : new LiteralProperty[] { new LiteralProperty(b.HERAUSGEBER) },
                Ort = String.IsNullOrWhiteSpace(b.ORT) ? null : b.ORT.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => new LiteralProperty(x)).ToArray(),
                Jahr = b.JAHR == null ? null : new LiteralProperty[] { new LiteralProperty(b.JAHR.ToString()) },
                Gesichtet = new BooleanProperty[] { new BooleanProperty(b.AUTOPSIE) },
                Erfasst = new BooleanProperty[] { new BooleanProperty(b.ERFASST) },
                Anmerkung = String.IsNullOrWhiteSpace(b.ANMERKUNGEN) ? null : b.ANMERKUNGEN.Split("/)", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => new HtmlProperty(x)).ToArray(),
                Struktur = String.IsNullOrWhiteSpace(b.STRUKTUR) ? null : new LiteralProperty[] { new LiteralProperty(b.STRUKTUR) },
                VorhandenAls = b.STATUS == null ? null : b.STATUS.Select(x => new LiteralProperty(x.Value)).ToArray(),
                Vorhanden = new BooleanProperty[] { new BooleanProperty(b.VORHANDEN) },
                ReihentitelALT = String.IsNullOrWhiteSpace(b.REIHENTITELALT) ? null : new LiteralProperty[] { new LiteralProperty(b.REIHENTITELALT) },
            });
        }
    }

    public async Task PostActorData() {
        var options = new JsonSerializerOptions { WriteIndented = true };
        LogSink.Instance.LogLine("Anzahl Personen: " + Persons.Count.ToString());
        ParallelOptions parallelOptions = new() {
            MaxDegreeOfParallelism = 128
        };
    

        rPersons = new();
        await Parallel.ForEachAsync(Persons, parallelOptions, async (a, token) => {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                "api/items?key_identity=" + SERVERDATA.KEY_IDENTITY + "&key_credential=" + SERVERDATA.KEY_CREDENTIAL, a
            );
            response.EnsureSuccessStatusCode();
            var opts = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
            };
            opts.Converters.Add(new AutoNumberToStringConverter());
            Person? ca = JsonSerializer.Deserialize<Person>(response.Content.ReadAsStream(), opts);
            rPersons.GetOrAdd(ca.Nummer.First().Value, ca);
        });
        LogSink.Instance.LogLine("Finished");
    }

    public async Task PostReihenData() {
        var options = new JsonSerializerOptions { WriteIndented = true };
        LogSink.Instance.LogLine("Anzahl Reihen: " + Reihen.Count.ToString());
        ParallelOptions parallelOptions = new() {
            MaxDegreeOfParallelism = 128
        };
    

        rReihen = new();
        await Parallel.ForEachAsync(Reihen, parallelOptions, async (a, token) => {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                "api/items?key_identity=" + SERVERDATA.KEY_IDENTITY + "&key_credential=" + SERVERDATA.KEY_CREDENTIAL, a
            );
            try {
                response.EnsureSuccessStatusCode();
            }
            catch {
                var r = await response.RequestMessage.Content.ReadAsStringAsync();
                Console.WriteLine(r);
            }
            var opts = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
            };
            opts.Converters.Add(new AutoNumberToStringConverter());
            Reihentitel? ca = JsonSerializer.Deserialize<Reihentitel>(response.Content.ReadAsStream(), opts);
            rReihen.GetOrAdd(ca.Nummer.First().Value, ca);
        });
        LogSink.Instance.LogLine("Finished");
    }

    public async Task PostBaendeData() {
        var options = new JsonSerializerOptions { WriteIndented = true };
        LogSink.Instance.LogLine("Anzahl Bände: " + Baende.Count.ToString());
        ParallelOptions parallelOptions = new() {
            MaxDegreeOfParallelism = 128
        };
    

        rBaende = new();
        await Parallel.ForEachAsync(Baende, parallelOptions, async (a, token) => {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                "api/items?key_identity=" + SERVERDATA.KEY_IDENTITY + "&key_credential=" + SERVERDATA.KEY_CREDENTIAL, a
            );
            response.EnsureSuccessStatusCode();
            var opts = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
            };
            opts.Converters.Add(new AutoNumberToStringConverter());
            Band? ca = JsonSerializer.Deserialize<Band>(response.Content.ReadAsStream(), opts);
            rBaende.GetOrAdd(ca.Nummer.First().Value, ca);
        });
        LogSink.Instance.LogLine("Finished");
    }

    private string? trimOrNull(string? totrim) {
        if (String.IsNullOrWhiteSpace(totrim)) return null;
        return totrim.Trim();
    }
}