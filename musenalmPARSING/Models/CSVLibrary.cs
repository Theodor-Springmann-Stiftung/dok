namespace MusenalmConverter.Models.CSV;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

[XmlRoot("Akteure")]
public class Akteure {
    // [XmlAttribute(Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
    // public string noNamespaceSchemaLocation = "../muster/Akteure.xsd";

    public long ID;
    public string? NAME;
    public bool? ORGANISATION = false;
    public string? BERUF;
    public string? PSEUDONYM;
    public string? ANMERKUNGEN;
    public string? GND;
    public string? Geburtsdatum;
    public string? Sterbedatum;
}

[XmlRoot("Status")]
public class Status {
    public string? Value;
}

[XmlRoot("Typ")]
public class Typ {
    public string? Value;
}

[XmlRoot("Paginierung")]
public class Paginierung {
    public string? Value;
}

[XmlRoot("Inhalte")]
public class Inhalte {
    public long ID;
    public long? BAND;
    public string? TITEL;
    public string? AUTOR;
    public string? PAGINIERUNG;
    public string? SEITE;
    public string? INCIPIT;
    public string? ANMERKUNGEN;
    public Typ[]? TYP;
    public bool? DIGITALISAT;
    public float? OBJEKTNUMMER;

    public bool ShouldSerializeTITEL() => !String.IsNullOrWhiteSpace(TITEL);
    public bool ShouldSerializeAUTOR() => !String.IsNullOrWhiteSpace(AUTOR);
    public bool ShouldSerializePAGINIERUNG() => !String.IsNullOrWhiteSpace(PAGINIERUNG);
    public bool ShouldSerializeSEITE() => !String.IsNullOrWhiteSpace(SEITE);
    public bool ShouldSerializeINCIPIT() => !String.IsNullOrWhiteSpace(INCIPIT);
    public bool ShouldSerializeANMERKUNGEN() => !String.IsNullOrWhiteSpace(ANMERKUNGEN);
    public bool ShouldSerializeTYP() => TYP != null;
    public bool ShouldSerializeOBJEKTNUMMER() => OBJEKTNUMMER != null;
}

[XmlRoot("Reihen")]
public class Reihen {
    public long ID;
    public string? NAME;
    public string? SORTIERNAME;
    public string? ANMERKUNGEN;

    public bool ShouldSerializeNAME() => !String.IsNullOrWhiteSpace(NAME);
    public bool ShouldSerializeANMERKUNGEN() => !String.IsNullOrWhiteSpace(ANMERKUNGEN);
    public bool ShouldSerializeSORTIERNAME() => !String.IsNullOrWhiteSpace(SORTIERNAME);
}

[XmlRoot("Baende")]
public class Baende {
    public long ID;
    [XmlElement("BIBLIO-ID")]
    public string? BIBLIOID;
    public string? SORTIERTITEL;
    public string? TITEL;
    public string? HERAUSGEBER;
    public string? ORT;
    public long? JAHR = null;
    public string? STRUKTUR;
    public string? NACHWEIS;
    public string? NORM;
    public bool AUTOPSIE;
    public string? ANMERKUNGEN;
    [XmlElement("REIHENTITEL-ALT")]
    public string? REIHENTITELALT;
    public Status[]? STATUS;

    public bool ShouldSerializeJAHR() => JAHR != null;
    public bool ShouldSerializeSORTIERTITEL() => !String.IsNullOrWhiteSpace(SORTIERTITEL);
    public bool ShouldSerializeHERAUSGEBER() => !String.IsNullOrWhiteSpace(HERAUSGEBER);
    public bool ShouldSerializeTITEL() => !String.IsNullOrWhiteSpace(TITEL);
    public bool ShouldSerializeORT() => !String.IsNullOrWhiteSpace(ORT);
    public bool ShouldSerializeSTRUKTUR() => !String.IsNullOrWhiteSpace(STRUKTUR);
    public bool ShouldSerializeNACHWEIS() => !String.IsNullOrWhiteSpace(NACHWEIS);
    public bool ShouldSerializeNORM() => !String.IsNullOrWhiteSpace(NORM);

    public bool ShouldSerializeANMERKUNGEN() => !String.IsNullOrWhiteSpace(ANMERKUNGEN);
    public bool ShouldSerializeSTATUS() => STATUS != null;
}

[XmlRoot("*RELATION_InhalteAkteure")]
public class RELATION_InhalteAkteure {
    public long INHALT;
    public long BEZIEHUNG;
    public long AKTEUR;
    public string? ANMERKUNG;
    public bool? ERSCHLOSSEN = false;
    public bool? UNSICHER = false;
    public bool ShouldSerializeANMERKUNG() => !String.IsNullOrWhiteSpace(ANMERKUNG);
}

[XmlRoot("*RELATION_BaendeAkteure")]
public class RELATION_BaendeAkteure {
    public long BAND;
    public long BEZIEHUNG;
    public long AKTEUR;
    public string? ANMERKUNG;
    public bool? ERSCHLOSSEN = false;
    public bool? UNSICHER = false;
    public bool ShouldSerializeANMERKUNG() => !String.IsNullOrWhiteSpace(ANMERKUNG);
}

[XmlRoot("*RELATION_BaendeReihen")]
public class RELATION_BaendeReihen {
    public long BAND;
    public long BEZIEHUNG;
    public long REIHE;
    public string? ANMERKUNG;
    public bool? ERSCHLOSSEN = false;
    public bool? UNSICHER = false;
    public bool ShouldSerializeANMERKUNG() => !String.IsNullOrWhiteSpace(ANMERKUNG);
}