namespace MusenalmKorrDB.Models;

public class Inhalt {
    public long ID { get; set; }
    public int? Band { get; set; }
    public string? TitelTranskription { get; set; }
    public string? AutorTranskription { get; set; }
    public int? PaginierungID { get; set; }
    public string? Seite { get; set; }
    public string? IncipitTranskription { get; set; }
    public string? Anmerkungen { get; set; }
    public bool? Digitalisat { get; set; }
    public float? Objektnummer { get; set; }

    public ICollection<REL_Inhalt_Typ>? Typen { get; set; }
    public ICollection<REL_Inhalt_Akteur>? Akteure { get; set; }
}

public class Typ {
    public int ID { get; set; }
    public string? Name { get; set; }
}

public class REL_Inhalt_Typ {
    public int InhaltID { get; set; }
    public int TypID { get; set; }
}

public class REL_Inhalt_Akteur {
    public int InhaltID { get; set; }
    public int VOC_AkteurID { get; set; }
    public int AkteurID { get; set; }
}

public class Paginierung { 
    public int ID { get; set; }
    public string? Name { get; set; }
}