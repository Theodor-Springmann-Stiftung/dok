using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MusenalmAPI.Models;

public class Band {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long ID { get; set; }
    public string? TitelTranskription { get; set; }
    public string? OrtTranskription { get; set; }
    public long? Jahr { get; set; }
    public string? AusgabeBand { get; set; }
    public string? Struktur { get; set; }
    public string? Nachweis { get; set; }
    public string? Anmerkungen { get; set; }
    public string? AbgeschnittenJahr { get; set; }

    public ICollection<REL_Band_Reihe>? Reihen { get; set; }
    public ICollection<REL_Band_Akteur>? Akteure { get; set; }
    public ICollection<REL_Band_Ort>? Orte { get; set; }
    public ICollection<Exemplar>? Exemplare { get; set; }
    public ICollection<Inhalt>? Inhalte { get; set; }
}

public class REL_Band_Reihe {
    public long ID { get; set; }
    public long BandID { get; set; }
    public long VOC_ReiheID {get; set; }
    public long ReiheID { get; set; }
    public string? Anmerkungen { get; set; }

    public Band? Band { get; set; }
    public VOC_Reihe? VOC_Reihe { get; set; }
    public Reihe? Reihe { get; set; }
}

public class REL_Band_Akteur {
    public long ID { get; set;}
    public long BandID { get; set; }
    public long VOC_AkteurID { get; set; }
    public long AkteurID { get; set; }
    public string? Anmerkungen { get; set; }

    public Band? Band { get; set; }
    public VOC_Akteur? VOC_Akteur { get; set; }
    public Akteur? Akteur { get; set; }
}


public class REL_Band_Ort {
    public long ID { get; set; }
    public long BandID { get; set; }
    public long VOC_OrtID { get; set; }
    public long OrtID { get; set; }
    public string? Anmerkungen { get; set; }

    public Band? Band { get; set; }
    public VOC_Ort? Beziehung { get; set; }
    public Ort? Ort { get; set; }
}