namespace MusenalmConverter.Models.NeueDBXML;

using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

[XmlRoot("Akteure")]
public class Akteure {
    // [XmlAttribute(Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
    // public string noNamespaceSchemaLocation = "../muster/Akteure.xsd";

    [XmlElement]
    public long ID;
    [XmlElement]
    public string? Name;
    [XmlElement]
    public string? OrgName;
    [XmlElement]
    public string? Sortiername;
    [XmlElement]
    public string? Lebensdaten;
    [XmlElement]
    public string? Beruf;
    [XmlElement]
    public string? Pseudonyme;
    [XmlElement]
    public string? Anmerkungen;
    [XmlElement]
    public string? GND;
    [XmlElement]
    public string? KSortiername;

    public bool ShouldSerializeName() => !String.IsNullOrWhiteSpace(Name);
    public bool ShouldSerializeOrgName() => !String.IsNullOrWhiteSpace(OrgName);
    public bool ShouldSerializeLebensdaten() => !String.IsNullOrWhiteSpace(Lebensdaten);
    public bool ShouldSerializeBeruf() => !String.IsNullOrWhiteSpace(Beruf);
    public bool ShouldSerializePseudonyme() => !String.IsNullOrWhiteSpace(Pseudonyme);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
    public bool ShouldSerializeGND() => !String.IsNullOrWhiteSpace(GND);
    public bool ShouldSerializeSortiername() => !String.IsNullOrWhiteSpace(Sortiername);
    public bool ShouldSerializeksortiername() => !String.IsNullOrWhiteSpace(KSortiername);
}

[XmlRoot("Exemplare")]
public class Exemplare {
    // [XmlAttribute(Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
    // [XmlAttribute("xsi:noNamespaceSchemaLocation")]
    // public string noNamespaceSchemaLocation = "../muster/Exemplare.xsd";

    [XmlElement]
    public long ID;
    [XmlElement]
    public long Band;
    [XmlElement("Biblio-Nr")]
    public string? BiblioNr;
    [XmlElement]
    public bool Gesichtet;
    [XmlElement]
    public Status[]? Status;
    [XmlElement]
    public string? URL;
    [XmlElement("Standort und Signatur")]
    public string? StandortSignatur;
    [XmlElement]
    public string? Anmerkungen;
    [XmlElement]
    public string? Struktur;

    public bool ShouldSerializeBiblioNr() => !String.IsNullOrWhiteSpace(BiblioNr);
    public bool ShouldSerializeStatus() => Status != null;
    public bool ShouldSerializeURL() => !String.IsNullOrWhiteSpace(URL);
    public bool ShouldSerializeStruktur() => !String.IsNullOrWhiteSpace(Struktur);
    public bool ShouldSerializeStandortSignatur() => !String.IsNullOrWhiteSpace(StandortSignatur);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("Status")]
public class Status {
    [XmlElement]
    public string? Value;
}

[XmlRoot("Typ")]
public class Typ {
    [XmlElement]
    public string? Value;
}

[XmlRoot("Paginierung")]
public class Paginierung {
    [XmlElement]
    public string? Value;
}

[XmlRoot("Inhalte")]
public class Inhalte {
    [XmlElement]
    public long ID;
    [XmlElement]
    public long? Band;
    [XmlElement]
    public string? TitelTranskription;
    [XmlElement]
    public string? AutorTranskription;
    [XmlElement]
    public string? Paginierung;
    [XmlElement]
    public string? Seite;
    [XmlElement]
    public string? IncipitTranskription;
    [XmlElement]
    public string? Anmerkungen;
    [XmlElement]
    public Typ[]? Typ;
    [XmlElement]
    public bool? Digitalisat;
    [XmlElement]
    public float? Objektnummer;
    [XmlElement]
    public string? KSortiertitel;

    public bool ShouldSerializeKSortiertitel() => !String.IsNullOrWhiteSpace(KSortiertitel);
    public bool ShouldSerializeTitelTranskription() => !String.IsNullOrWhiteSpace(TitelTranskription);
    public bool ShouldSerializeAutorTranskription() => !String.IsNullOrWhiteSpace(AutorTranskription);
    public bool ShouldSerializePaginierung() => Paginierung != null;
    public bool ShouldSerializeSeite() => !String.IsNullOrWhiteSpace(Seite);
    public bool ShouldSerializeIncipitTranskription() => !String.IsNullOrWhiteSpace(IncipitTranskription);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
    public bool ShouldSerializeTyp() => Typ != null;
    public bool ShouldSerializeObjektnummer() => Objektnummer != null;
}

[XmlRoot("Orte")]
public class Orte {
    [XmlElement]
    public long ID;
    [XmlElement]
    public string? Sortiername;
    [XmlElement]
    public string? KSortiername;
    [XmlElement]
    public string? Land;
    [XmlElement]
    public string? GeoNames;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeSortiername() => !String.IsNullOrWhiteSpace(Sortiername);
    public bool ShouldSerializeKSortiername() => !String.IsNullOrWhiteSpace(KSortiername);
    public bool ShouldSerializeLand() => !String.IsNullOrWhiteSpace(Land);
    public bool ShouldSerializeGeoNames() => !String.IsNullOrWhiteSpace(GeoNames);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("Reihen")]
public class Reihen {
    [XmlElement]
    public long ID;
    [XmlElement]
    public string? Name;
    [XmlElement]
    public string? Sortiername;
    [XmlElement]
    public string? KSortiername;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeReihentitel() => !String.IsNullOrWhiteSpace(Name);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
    public bool ShouldSerializeSortiertitel() => !String.IsNullOrWhiteSpace(Sortiername);
    public bool ShouldSerializeKSortiertitel() => !String.IsNullOrWhiteSpace(KSortiername);
}

[XmlRoot("Baende")]
public class Baende {
    [XmlElement]
    public long ID;
    [XmlElement]
    public string? Sortiertitel;
    [XmlElement("Titel-Transkription")]
    public string? TitelTranskription;
    [XmlElement("Ort-Transkription")]
    public string? OrtTranskription;
    [XmlElement]
    public long? Jahr = null;
    [XmlElement]
    public int? Ausgabe;
    [XmlElement]
    public string? Struktur;
    [XmlElement]
    public string? Nachweis;
    [XmlElement]
    public string? Anmerkungen;
    [XmlElement]
    public string? KSortiertitel;
    [XmlElement]
    public string? AbgeschnittenJahr;

    public bool ShouldSerializeJahr() => Jahr != null;
    public bool ShouldSerializeAusgabe() => Ausgabe != null;
    public bool ShouldSerializeSortiertitel() => !String.IsNullOrWhiteSpace(Sortiertitel);
    public bool ShouldSerializeKSortiertitel() => !String.IsNullOrWhiteSpace(KSortiertitel);
    public bool ShouldSerializeTitelTranskription() => !String.IsNullOrWhiteSpace(TitelTranskription);
    public bool ShouldSerializeOrtTranskription() => !String.IsNullOrWhiteSpace(OrtTranskription);
    public bool ShouldSerializeStruktur() => !String.IsNullOrWhiteSpace(Struktur);
    public bool ShouldSerializeNachweis() => !String.IsNullOrWhiteSpace(Nachweis);
    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
    public bool ShouldSerializeAbgeschnittenJahr() => !String.IsNullOrWhiteSpace(AbgeschnittenJahr);
}

[XmlRoot("*RELATION_InhalteAkteure")]
public class RELATION_InhalteAkteure {
    [XmlElement]
    public long Inhalt;
    [XmlElement]
    public long Beziehung;
    [XmlElement]
    public long Akteur;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("*RELATION_BaendeAkteure")]
public class RELATION_BaendeAkteure {
    [XmlElement]
    public long Band;
    [XmlElement]
    public long Beziehung;
    [XmlElement]
    public long Akteur;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("*RELATION_BaendeReihen")]
public class RELATION_BaendeReihen {
    [XmlElement]
    public long Band;
    [XmlElement]
    public long Reihe;
    [XmlElement]
    public string? Anmerkungen;

    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}

[XmlRoot("*RELATION_BaendeOrte")]
public class RELATION_BaendeOrte {
    [XmlElement]
    public long Band;
    [XmlElement]
    public long Beziehung;
    [XmlElement]
    public long Ort;
    [XmlElement]
    public string Anmerkungen;

    public bool ShouldSerializeAnmerkungen() => !String.IsNullOrWhiteSpace(Anmerkungen);
}