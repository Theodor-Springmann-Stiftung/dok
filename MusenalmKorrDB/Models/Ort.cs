namespace MusenalmKorrDB.Models;

public class Ort {
    public int ID { get; set; }
    public string? Sortiername { get; set; }
    public string? Land { get; set; }
    public string? GeoNames { get; set; }
    public string? Anmerkungen { get; set; }
}

public class VOC_Ort {
    public int ID { get; set; }
    public string? Beziehung { get; set; }
}