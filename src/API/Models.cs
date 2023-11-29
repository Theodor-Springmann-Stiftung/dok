namespace MusenalmConverter.API.Models;
using System.Text.Json.Serialization;
using MusenalmConverter.Migration.MittelDBXML;

public interface APICONSTANTS {
    public string URI { get; set; }
    // 25/08/2023: revoked key
    public string KEY_IDENTITY { get; set; }
    public string KEY_CREDENTIAL { get; set; }

    public Resource PERSON_RESOURCE_CLASS { get; set; }
    public Resource PERSON_RESOURCE_TEMPLATE { get; set; } 
    public Resource[] MUSENALM_PERSON_SETS { get; set; }

    public Resource REIHENTITEL_RESOURCE_CLASS { get; set; }
    public Resource REIHENTITEL_RESOURCE_TEMPLATE { get; set; }
    public Resource[] MUSENALM_REIHENTITEL_SETS { get; set; }

    public Resource BAND_RESOURCE_CLASS { get; set; }
    public Resource BAND_RESOURCE_TEMPLATE { get; set; }
    public Resource[] MUSENALM_BAND_SETS { get; set; }

    public Resource CORPORATE_RESOURCE_CLASS { get; set; }
    public Resource CORPORATE_RESOURCE_TEMPLATE { get; set; }
    public Resource[] MUSENALM_CORPORATE_SETS { get; set; }

    public Resource INHALT_RESOURCE_CLASS { get; set; }
    public Resource INHALT_RESOURCE_TEMPLATE { get; set; }
    public Resource[] MUSENALM_INHALT_SETS { get; set; }
}

public class SERVERCONSTANTS : APICONSTANTS {
    public string URI { get; set; } = "https://omeka.musenalm.de/";
    // 25/08/2023: revoked key
    public string KEY_IDENTITY { get; set; } = "";
    public string KEY_CREDENTIAL { get; set; } = "";

    // Person resource classes: enter the appropriate class ids and resource template ids
    // for the person resource class and the person resource template. Also the collections
    // (= item sets) to which a person should be added.

    // This info differs from installation to installation. You can find the ids in the
    // url of the templates and classes in the webinterface of your installation.
    public Resource PERSON_RESOURCE_CLASS { get; set; } = new Resource(106);
    public Resource PERSON_RESOURCE_TEMPLATE { get; set; } = new Resource(5);
    public Resource[] MUSENALM_PERSON_SETS { get; set; } = [
        new Resource(4),
        new Resource(3)
    ];

    public Resource REIHENTITEL_RESOURCE_CLASS { get; set; } = new Resource(112);
    public Resource REIHENTITEL_RESOURCE_TEMPLATE { get; set; } = new Resource(3);
    public Resource[] MUSENALM_REIHENTITEL_SETS { get; set; } = [
        new Resource(6)
    ];

    public Resource BAND_RESOURCE_CLASS { get; set; } = new Resource(119);
    public Resource BAND_RESOURCE_TEMPLATE { get; set; } = new Resource(2);
    public Resource[] MUSENALM_BAND_SETS { get; set; } = [
        new Resource(1)
    ];

    public Resource CORPORATE_RESOURCE_CLASS { get; set; } = new Resource(106);
    public Resource CORPORATE_RESOURCE_TEMPLATE { get; set; } = new Resource(4);
    public Resource[] MUSENALM_CORPORATE_SETS { get; set; } = [
        new Resource(5),
        new Resource(3)
    ];

    public Resource INHALT_RESOURCE_CLASS { get; set; } = new Resource(120);
    public Resource INHALT_RESOURCE_TEMPLATE { get; set; } = new Resource(6);
    public Resource[] MUSENALM_INHALT_SETS { get; set; } = [
        new Resource(2)
    ];
}

// // Testdata for local isntallation
// public class LOCALHOSTCONSTANTS : APICONSTANTS {
//     public string URI { get; set; } = "http://localhost";
//     public string KEY_IDENTITY { get; set; } = "aABTqSpbdYYXac9NF8hX9a2nA90FfQvM";
//     public string KEY_CREDENTIAL { get; set; } = "2ptD5CmClTh3QXq3WOUWBYsnWlKuBroF";

//     public Resource PERSON_RESOURCE_CLASS { get; set; } = new Resource(114);
//     public Resource PERSON_RESOURCE_TEMPLATE { get; set; } = new Resource(3);
//     public Resource[] MUSENALM_PERSON_SETS { get; set; } = new Resource[] {
//         new Resource(3),
//         new Resource(9),
//         new Resource(10)
//     };

//     public Resource REIHENTITEL_RESOURCE_CLASS { get; set; } = new Resource(112);
//     public Resource REIHENTITEL_RESOURCE_TEMPLATE { get; set; } = new Resource(5);
//     public Resource[] MUSENALM_REIHENTITEL_SETS { get; set; } = new Resource[] {
//         new Resource(1)
//     };

//     public Resource BAND_RESOURCE_CLASS { get; set; } = new Resource(119);
//     public Resource BAND_RESOURCE_TEMPLATE { get; set; } = new Resource(6);
//     public Resource[] MUSENALM_BAND_SETS { get; set; } = new Resource[] {
//         new Resource(8)
//     };

//     public Resource CORPORATE_RESOURCE_CLASS { get; set; } = new Resource(108);
//     public Resource CORPORATE_RESOURCE_TEMPLATE { get; set; } = new Resource(4);
//     public Resource[] MUSENALM_CORPORATE_SETS { get; set; } = new Resource[] {
//         new Resource(2),
//         new Resource(7),
//         new Resource(10)
//     };

//     public Resource ITEM_RESOURCE_CLASS { get; set; } = new Resource(113);
//     public Resource ITEM_RESOURCE_TEMPLATE { get; set; } = new Resource(7);
//     public Resource[] MUSENALM_ITEM_SETS { get; set; } = new Resource[] {
//         new Resource(5)
//     };
// }

public class Property {
    [property: JsonPropertyName("property_id")] 
    public string? PropertyID { get; set; } = "auto";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("property_value")]
    public string? PropertyValue { get; set; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("is_public")]
    public bool? IsPublic { get; set; } = null;
}

public class LiteralProperty : Property {
    [property: JsonPropertyName("type")] 
    public string Type { get; } = "literal";

    [property: JsonPropertyName("@value")] 
    public string Value { get;  set; } = string.Empty;

    public LiteralProperty(string _value) {
        Value = _value;
    }

    public LiteralProperty() { }
}

public class HtmlProperty : Property {
    [property: JsonPropertyName("type")] 
    public string Type { get; } = "html";

    [property: JsonPropertyName("@type")] 
    public string HtmlType { get; } = "http://www.w3.org/1999/02/22-rdf-syntax-ns#HTML";

    [property: JsonPropertyName("@value")] 
    public string Value { get; set; } = string.Empty;

    public HtmlProperty(string _value) {
        Value = _value;
    }

    public HtmlProperty() { }
}

public class ItemProperty : Property {
    [property: JsonPropertyName("type")] 
    public string Type { get; } = "resource:item";

    [property: JsonPropertyName("value_resource_name")] 
    public string ValueResourceName { get; set; } = "items";

    [property: JsonPropertyName("value_resource_id")] 
    public int ID { get; set; }

    public ItemProperty(int id) {
        ID= id;
    }

    public ItemProperty() { }
}

public class BooleanProperty : Property {
    [property: JsonPropertyName("type")] 
    public string Type { get; } = "boolean";

    [property: JsonPropertyName("@value")] 
    public bool Value { get; set; }

    public BooleanProperty(bool value) {
        Value = value;
    }

    public BooleanProperty() { }
}

public class Resource {
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("o:id")]
    public int? ID { get; set; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("@id")]
    public string? IDURI { get; set; } = null;

    public Resource(int _id)  {
        ID = _id;
    }

    public Resource() { }
}

public class Date {
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("@value")]
    public string? Value { get; set; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("@type")]
    public string? Type { get; set; } = null;
}

public class Item : Resource {
    [property: JsonPropertyName("o:resource_class")]
    public Resource ResourceClass { get; set; }

    [property: JsonPropertyName("o:resource_template")]
    public Resource RersourceTemplate { get; set; }

    [property: JsonPropertyName("o:item_set")]
    public Resource[] ItemSet { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("is_public")]
    public bool? IsPublic { get; set; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("@type")]
    public string[]? Type { get; set; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("o:title")]
    public string? Title { get; set; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("@o:created")]
    public string? Created { get; set; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("@o:modified")]
    public string? Modified { get; set; } = null;


    public Item(Resource _resource_class, Resource _resource_template, Resource[] _item_set) {
        ResourceClass = _resource_class;
        RersourceTemplate = _resource_template;
        ItemSet = _item_set;
    }

    public Item() { }
}

public class Person : Item {
    [property: JsonPropertyName("rdaa:P50111")]
    public LiteralProperty[] Name { get; set; }

    [property: JsonPropertyName("rdaa:P50428")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? Pseudonym { get; set; }

    [property: JsonPropertyName("rdaa:P50103")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? AbweichenderName { get; set; }

    [property: JsonPropertyName("rdaa:P50113")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HtmlProperty[]? BiogrAngaben { get; set; }

    [property: JsonPropertyName("rdaa:P50368")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? Nachweis { get; set; }

    [property: JsonPropertyName("rdaa:P50104")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? Beruf { get; set; }

    [property: JsonPropertyName("rdaa:P50121")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? Geburtsdatum { get; set; }

    [property: JsonPropertyName("rdaa:P50120")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? Sterbedatum { get; set; }

    [property: JsonPropertyName("rdaa:P50395")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HtmlProperty[]? Anmerkung { get; set; }

    [property: JsonPropertyName("musenalmdepr:nummer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? Nummer { get; set; }
    
    public Person(APICONSTANTS c, string name) : base(c.PERSON_RESOURCE_CLASS, c.PERSON_RESOURCE_TEMPLATE, c.MUSENALM_PERSON_SETS) {
        Name = [new LiteralProperty(name)];
    }

    public Person() : base () { }
}

public class Coroprate : Item {
    [property: JsonPropertyName("rdaa:P50032")]
    public LiteralProperty[] Name { get; set; }

    [property: JsonPropertyName("rdaa:P50025")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? AbweichenderName { get; set; }

    [property: JsonPropertyName("rdaa:P50369")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? Nachweis { get; set; }

    [property: JsonPropertyName("rdaa:P50395")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HtmlProperty[]? Anmerkung { get; set; }
    
    public Coroprate(APICONSTANTS c, string name) : base(c.CORPORATE_RESOURCE_CLASS, c.CORPORATE_RESOURCE_TEMPLATE, c.MUSENALM_CORPORATE_SETS) {
        Name = [new LiteralProperty(name)];
    }

    public Coroprate() : base () { }
}


public class Reihentitel : Item {
    [property: JsonPropertyName("rdax:P00019")]
    public LiteralProperty[] Name { get; set; }

    [property: JsonPropertyName("rdan:P80178")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? Nachweis { get; set; }

    [property: JsonPropertyName("rdan:P80071")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HtmlProperty[]? Anmerkung { get; set; }

    [property: JsonPropertyName("musenalmdepr:nummer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? Nummer { get; set; }

    public Reihentitel(APICONSTANTS c, string name) : base(c.REIHENTITEL_RESOURCE_CLASS, c.REIHENTITEL_RESOURCE_TEMPLATE, c.MUSENALM_REIHENTITEL_SETS) {
        Name = [new LiteralProperty(name)];
    }

    public Reihentitel() : base() { }
}

public class Band : Item {
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30106")]
    public LiteralProperty[]? Titel { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30157")]
    public ItemProperty[]? Reihe { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("musenalm:altreihe")]
    public ItemProperty[]? AltReihe { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30204")]
    public ItemProperty[]? ParallelReihe { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30117")]
    public LiteralProperty[]? Herausgeberangabe { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdaw:P10046")]
    public ItemProperty[]? Herausgeber { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30088")]
    public LiteralProperty[]? Ort { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30083")]
    public ItemProperty[]? Verlag { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30080")]
    public ItemProperty[]? Vertrieb { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30011")]
    public LiteralProperty[]? Jahr { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30133")]
    public LiteralProperty[]? Ausgabezeichnung { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30111")]
    public LiteralProperty[]? Veröffentlichungsangabe { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30168")]
    public LiteralProperty[]? Erscheinungsfrequenz { get; set; } = new LiteralProperty[] { new LiteralProperty("Jährlich") };

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30182")]
    public LiteralProperty[]? Struktur { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30254")]
    public LiteralProperty[]? Nachweis { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("musenalm:gesichtet")]
    public BooleanProperty[]? Gesichtet { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("musenalm:erfasst")]
    public BooleanProperty[]? Erfasst { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30137")]
    public HtmlProperty[]? Anmerkung { get; set; }

    [property: JsonPropertyName("tssbiblio:biblioid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? BiblioID { get; set; }

    [property: JsonPropertyName("musenalmdepr:nummer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? Nummer { get; set; }

    [property: JsonPropertyName("musenalmdepr:norm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? Norm { get; set; }

    [property: JsonPropertyName("musenalmdepr:reihentitel")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? ReihentitelALT { get; set; }

    [property: JsonPropertyName("musenalmdepr:vorhandenals")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? VorhandenAls { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdaw:P30033")]
    public ItemProperty[]? Inhalte { get; set; }


    public Band(APICONSTANTS c, string titel) : base(c.BAND_RESOURCE_CLASS, c.BAND_RESOURCE_TEMPLATE, c.MUSENALM_BAND_SETS) {
        Titel = [new LiteralProperty(titel)];
    }

    public Band() : base() { }

}

public class Inhalt : Item {

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30020")]
    public ItemProperty[]? Band { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdaw:10012")]
    public LiteralProperty[]? Objektnummer { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdaw:P10004")]
    public LiteralProperty[]? Typ { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("musenalm:pag")]
    public LiteralProperty[]? Paginierung { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("musenalm:seite")]
    public LiteralProperty[]? Seite { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30156")]
    public LiteralProperty[]? Titel { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("musenalm:incipit")]
    public LiteralProperty[]? Incipit { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30117")]
    public ItemProperty[]? Urheberangabe { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P10065")]
    public ItemProperty[]? Urheber { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P10061")]
    public ItemProperty[]? Autor { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30070")]
    public ItemProperty[]? Kupferstecher { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P10058")]
    public ItemProperty[]? Zeichner { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [property: JsonPropertyName("rdam:P30137")]
    public HtmlProperty[]? Anmerkung { get; set; }

    [property: JsonPropertyName("rdai:P40088")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BooleanProperty[]? Digitalisat { get; set; }

    [property: JsonPropertyName("musenalmdepr:nummer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LiteralProperty[]? Nummer { get; set; }

    public Inhalt(APICONSTANTS c, string titel) : base(c.INHALT_RESOURCE_CLASS, c.INHALT_RESOURCE_TEMPLATE, c.MUSENALM_INHALT_SETS) {
        Titel = [new LiteralProperty(titel)];
    }

    public Inhalt() : base() { }

}
