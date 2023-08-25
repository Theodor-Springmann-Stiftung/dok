namespace MusenalmConverter.API;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using MusenalmConverter.API.Models;
using MusenalmConverter.Migration.MittelDBXML;

public class APICaller {
    private static string URI = "https://db.musenalm.de/";
    // 25/08/2023: revoked key
    private static string KEY_IDENTITY = "";
    private static string KEY_CREDENTIAL = "";

    private MittelDBXMLLibrary _data;
    private HttpClient _httpClient;

    public List<Actor> Actors;

    public APICaller(MittelDBXMLLibrary data) {
        _data = data;
        _httpClient = new();
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.BaseAddress = new Uri(URI);

        CreateData();
    }

    private void CreateData() {
        CreateActorData();
    }

    private void CreateActorData() {
        Actors = new();
        foreach (var a in _data.Akteure) {
            if (!String.IsNullOrWhiteSpace(a.NAME) && 
                (_data.RELATION_BaendeAkteure.Where(x => x.AKTEUR == a.ID).Any() ||
                 _data.RELATION_InhalteAkteure.Where(x => x.AKTEUR == a.ID).Any())) {
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
                Actors.Add(new Actor(a.NAME.Trim()) {
                    Pseudonym = ps,
                    Nachweis = nw,
                    Beruf = String.IsNullOrWhiteSpace(a.BERUF) ? null : new LiteralProperty[] { new LiteralProperty( a.BERUF )},
                    Geburtsdatum = geburt == null ? null : new LiteralProperty[]{ geburt },
                    Sterbedatum = tod == null ? null : new LiteralProperty[]{ tod },
                    Anmerkung = String.IsNullOrWhiteSpace(a.ANMERKUNGEN) ? null : new HtmlProperty[] { new HtmlProperty( a.ANMERKUNGEN )},
                    Nummer = new LiteralProperty[] { new LiteralProperty(a.ID.ToString())}
                });
            }
        }
    }

    public async Task PostActorData() {
        var options = new JsonSerializerOptions { WriteIndented = true };
        LogSink.Instance.LogLine("Anzahlt Akteure: " + Actors.Count.ToString());
        ParallelOptions parallelOptions = new() {
            MaxDegreeOfParallelism = 48
        };
 
        await Parallel.ForEachAsync(Actors, parallelOptions, async (a, token) => {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                "api/items?key_identity=" + KEY_IDENTITY + "&key_credential=" + KEY_CREDENTIAL, a);
            response.EnsureSuccessStatusCode();
        });
        LogSink.Instance.LogLine("Finished");
    }

    private string? trimOrNull(string? totrim) {
        if (String.IsNullOrWhiteSpace(totrim)) return null;
        return totrim.Trim();
    }
}