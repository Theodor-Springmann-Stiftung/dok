using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MusenalmAPI.Models;

[Index(nameof(Sortiername))]
public class Reihe {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long ID { get; set; }
    public string? Name { get; set; }
    public string? Sortiername { get; set; }
    public string? Anmerkungen { get; set; }

    public ICollection<REL_Band_Reihe>? REL_Baende_Reihen { get; set; }
}

public class VOC_Reihe : TVOC { }