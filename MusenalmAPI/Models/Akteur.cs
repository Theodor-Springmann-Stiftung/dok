namespace MusenalmAPI.Models;

using System.ComponentModel.DataAnnotations.Schema;

public class Akteur {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long ID { get; set; }
    public string? Name { get; set; }
    public string? OrgName { get; set; }
    public string? Sortiername { get; set; }
    public string? Lebensdaten { get; set; }
    public string? Beruf { get; set; }
    public string? Pseudonyme { get; set; }
    public string? Anmerkungen { get; set; }
    public string? GND { get; set; }
}

public class VOC_Akteur {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long ID { get; set; }
    public string? Beziehung { get; set; }
}