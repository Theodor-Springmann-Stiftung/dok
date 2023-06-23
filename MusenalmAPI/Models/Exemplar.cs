using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MusenalmAPI.Models;

public class Exemplar {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long ID { get; set; }
    public long? BandID { get; set; }
    public string? BiblioNr { get; set; }
    public bool Gesichtet { get; set; }
    public string? URL { get; set; }
    public string? StandortSignatur { get; set; }
    public string? Anmerkungen { get; set; }
    public string? Struktur { get; set; }

    public ICollection<REL_Exemplar_Status>? Status { get; set; }
    public Band? Band { get; set; }
}

public class VOC_Status {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long ID { get; set; }
    public string? Name { get; set; }
}

public class REL_Exemplar_Status {
    public long ID { get; set; }
    public long ExemplarID { get; set; }
    public long VOC_StatusID { get; set; }
    public string? Anmerkungen { get; set; }
    
    public VOC_Status? Status { get; set; }
    public Exemplar? Exemplar { get; set; }
}