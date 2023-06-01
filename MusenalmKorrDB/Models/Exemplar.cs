namespace MusenalmKorrDB.Models;

public class Exemplar {
    public int ID { get; set; }
    public int? BandID { get; set; }
    public string? BiblioNr { get; set; }
    public bool Gesichtet { get; set; }
    public string? URL { get; set; }
    public string? StandortSignatur { get; set; }
    public string? Anmerkungen { get; set; }
    public string? Struktur { get; set; }

    public ICollection<REL_Exemplar_Status>? Status { get; set; }
}

public class Status {
    public int ID { get; set; }
    public string? Name { get; set; }
}

public class REL_Exemplar_Status {
    public int ExemplarID { get; set; }
    public int StatusID { get; set; }
}