using System.Text.RegularExpressions;

namespace MusenalmConverter.Migration.CSV;

public class CSVOrte {
    public string Namen { get; set; }
    public string Baende { get; set; }
}

public class CSVVerleger {
    public string Namen { get; set; }
    public string Baende { get; set; }
}

public class CSVParser<T> {
    public static List<T> Parse(string[] header, string path) {
        var lines = File.ReadAllLines(path);
        var data = lines.Select(line => {
            string[] data = [];
            string token = "";
            bool inQuote = false;
            foreach (var c in line) {
                if (c == '"') {
                    inQuote = !inQuote;
                    continue;
                }
                if (c == ';' && !inQuote) {
                    if (!String.IsNullOrWhiteSpace(token)) data = data.Append(token).ToArray();
                    token = "";
                    continue;
                }
                token += c;
            }

            if (!String.IsNullOrWhiteSpace(token)) data = data.Append(token).ToArray();

            if (data.Length != header.Length) {
                throw new Exception("CSV file is not valid: Number of columns does not match header: " + string.Join(", ", data) + "Zeile: " + line);
            }
            return data; 
        }).ToList();
        var result = new List<T>();
        foreach (var row in data) {
            var item = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties();
            for (var i = 0; i < header.Length; i++) {
                var property = properties.FirstOrDefault(p => p.Name == header[i]);
                if (property != null) {
                    property.SetValue(item, Convert.ChangeType(row[i].Trim(), property.PropertyType));
                }
            }
            result.Add(item);
        }
        return result;
    }
}

public class CSVLibrary {
    public List<CSVOrte> Orte;
    public List<CSVVerleger> Verleger;

    public CSVLibrary(string[] paths) {
        foreach (var p in paths) {
            var path = new FileInfo(p);
            if (!path.Exists) {
                throw new FileNotFoundException($"File {p} not found");
            } else if (path.Extension != ".csv" && path.Extension != ".CSV") {
                throw new Exception($"File {p} is not a CSV file");
            } else if (!path.Name.StartsWith("orte") && !path.Name.StartsWith("verleger")) {
                throw new Exception($"Datei {p} muss mit 'verleger' oder 'orte' beginnen.");
            }

            if (path.Name.StartsWith("orte")) {
                if (Orte == null) {
                    Orte = CSVParser<CSVOrte>.Parse(new string[] { "Namen", "Baende" }, p);
                } else {
                    Orte.AddRange(CSVParser<CSVOrte>.Parse(new string[] { "Namen", "Baende" }, p));
                }
            } else {
                if (Verleger == null) {
                    Verleger = CSVParser<CSVVerleger>.Parse(new string[] { "Namen", "Baende" }, p);
                } else {
                    Verleger.AddRange(CSVParser<CSVVerleger>.Parse(new string[] { "Namen", "Baende" }, p));
                }
            }
        }
    }
}