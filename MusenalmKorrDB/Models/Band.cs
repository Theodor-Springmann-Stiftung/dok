namespace MusenalmKorrDB.Models;

public class Band {
    public int ID { get; set; }
    public string? Sortiertitel { get; set; }
    public string? TitelTranskription { get; set; }
    public string? OrtTranskription { get; set; }
    public int? Jahr { get; set; }
    public string? AusgabeBand { get; set; }
    public string? Struktur { get; set; }
    public string? Nachweis { get; set; }
    public string? Anmerkungen { get; set; }
    public string? AbgeschnittenJahr { get; set; }

    public ICollection<REL_Band_Reihe>? Reihen { get; set; }
    public ICollection<REL_Band_Akteur>? Akteure { get; set; }
    public ICollection<REL_Band_Ort>? Orte { get; set; }
}

public class REL_Band_Reihe {
    public int BandID { get; set; }
    public int VOC_ReiheID {get; set; }
    public int ReiheID { get; set; }
}

public class REL_Band_Akteur {
    public int BandID { get; set; }
    public int VOC_AkteurID { get; set; }
    public int AkteurID { get; set; }
}

public class REL_Band_Ort { 
    public int BandID { get; set; }
    public int VOC_OrtID { get; set; }
    public int OrtID { get; set; }
}