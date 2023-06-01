namespace MusenalmKorrDB.Models;

using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

public class Akteur {
    public long ID { get; set; }
    public string? Name { get; set; }
    public string? OrgName { get; set; }
    public string? Sortiername { get; set; }
    public string? Lebensdaten { get; set; }
    public string? Beruf { get; set; }
    public string? Pseudonyme { get; set; }
    public string? Anmerkungen { get; set; }
    public string? GND { get; set; }
}

public class VOC_Akteur { 
    public long ID { get; set; }
    public string? Beziehung { get; set; }
}