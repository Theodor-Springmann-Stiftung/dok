using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MusenalmAPI.Models;

[Index(nameof(Sortiername))]
public class Ort {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long ID { get; set; }
    public string? Sortiername { get; set; }
    public string? Land { get; set; }
    public string? GeoNames { get; set; }
    public string? Anmerkungen { get; set; }
}

public class VOC_Ort : TVOC { }