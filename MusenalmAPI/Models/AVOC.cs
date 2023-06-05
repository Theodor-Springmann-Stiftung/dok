using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MusenalmAPI.Models;


// Beziehungen-Vokabluar: bezogen auf Tabellen
[Index(nameof(Name))]
public abstract class TVOC {
    public long ID { get; set; }
    public string? Name { get; set; }
    public string Tabelle { get; set; }
}

// Feld-Vokabular: Bezogen auf Felder
[Index(nameof(Name))]
public abstract class FVOC {
    public long ID { get; set; }
    public string? Name { get; set; }
    public string Feld { get; set; }
}