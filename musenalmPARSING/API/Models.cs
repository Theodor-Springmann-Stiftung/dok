namespace MusenalmConverter.API.Models;
using System.Text.Json.Serialization;

public class LiteralProperty {
    public string property_id { get; } = "auto";
    public string type { get; } = "literal";
    [property: JsonPropertyName("@value")] 
    public string value { get; private set; } = string.Empty;

    public LiteralProperty(string _value) {
        value = _value;
    }
}

public class HtmlProperty {
    public string property { get; } = "auto";
    public string type { get; } = "html";
    [property: JsonPropertyName("@type")] 
    public string htmltype { get; } = "http://www.w3.org/1999/02/22-rdf-syntax-ns#HTML";
    [property: JsonPropertyName("@value")] 
    public string value { get; private set; } = string.Empty;

    public HtmlProperty(string _value) {
        value = _value;
    }
}

public class Resource {
    [property: JsonPropertyName("o:id")]
    public int id { get; private set; }

    public Resource(int _id)  {
        id = _id;
    }
}

public class Item {
    [property: JsonPropertyName("o:resource_class")]
    public Resource resource_class { get; private set; }
    [property: JsonPropertyName("o:resource_template")]
    public Resource resource_template { get; private set; }
    [property: JsonPropertyName("o:item_set")]
    public Resource[] item_set { get; private set; }

    public Item(Resource _resource_class, Resource _resource_template, Resource _item_set) {
        resource_class = _resource_class;
        resource_template = _resource_template;
        item_set = new Resource[] { _item_set };
    }
}

public class Actor : Item {
    [property: JsonPropertyName("rdaa:P50111")]
    public LiteralProperty[] Name { get; private set; }
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
    
    public Actor(string name) : base(new Resource(108), new Resource(6), new Resource(8)) {
        Name = new LiteralProperty[] { new LiteralProperty(name) };
    }
}