using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MusenalmAPI.Models;

public class Inhalt {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long ID { get; set; }
    public long? BandID { get; set; }
    public string? TitelTranskription { get; set; }
    public string? AutorTranskription { get; set; }
    public long? VOC_PaginierungID { get; set; }
    public string? Seite { get; set; }
    public string? IncipitTranskription { get; set; }
    public string? Anmerkungen { get; set; }
    public bool? Digitalisat { get; set; }
    public float? Objektnummer { get; set; }

    public ICollection<REL_Inhalt_Typ>? Typen { get; set; }
    public ICollection<REL_Inhalt_Akteur>? Akteure { get; set; }
    public VOC_Paginierung? Paginierung { get; set; }
    public Band? Band { get; set; }
}

public class VOC_Typ {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long ID { get; set; }
    public string? Name { get; set; }
}

public class REL_Inhalt_Typ {
    public long ID { get; set; }
    public long InhaltID { get; set; }
    public long VOC_TypID { get; set; }
    public string? Anmerkungen { get; set; }

    public Inhalt? Inhalt { get; set; }
    public VOC_Typ? Typ { get; set; }
}

public class REL_Inhalt_Akteur {
    public long ID { get; set; }
    public long InhaltID { get; set; }
    public long VOC_AkteurID { get; set; }
    public long AkteurID { get; set; }
    public string? Anmerkungen { get; set; }

    public Inhalt? Inhalt { get; set; }
    public VOC_Akteur? VOC_Akteur { get; set; }
    public Akteur? Akteur { get; set; }
}

public class VOC_Paginierung {
    [DatabaseGenerated(DatabaseGeneratedOption.None)] 
    public long ID { get; set; }
    public string? Name { get; set; }
}