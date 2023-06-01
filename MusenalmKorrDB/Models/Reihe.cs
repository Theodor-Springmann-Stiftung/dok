namespace MusenalmKorrDB.Models;

public class Reihe {
    public int ID { get; set; }
    public string? Name { get; set; }
    public string? Sortiername { get; set; }
    public string? Anmerkungen { get; set; }
}

public class VOC_Reihe {
    public int ID { get; set; }
    public string? Beziehung { get; set; }
}