# Musenalmanache: `AlmNeu`
Feld             |  Bedeutung und Anmerkungen 
----------------:|---------------------------
`NUMMER`         | Fortlaufende, eindeutige Nummer des Bandes.
`BIBLIO-NR`      | Nummer, unter welcher der Band in der Tabelle `BIBLIO-GM` erfasst ist. Leer, falls nicht.
`ALM-TITEL`      | Diplomatische Transkription des Gesamttitelangabe des Titelblatts mit Zeilenumbrüchen (`/`). Leer, falls kein Band vorgelegen hat.
`REIHENTITEL`    | Kompliziertestes Feld. Zuerst gibt es die Reihe(n) an, unter welche ein Band eingeordnet werden kann und nachfolgend das Jahr des Bandes. Weiterhin wird angegeben, ob ein Band außerdem einer anderssprachigen Reihe zuzuordnen ist (als Übersetzung). Weiter enthält es Ausgabevermerke (wie) und bei Mehrfacherfassung ein `[var]`, `[var.2]` u.s.w. Weiter sind einige Datensätze sonst leer und dienen nur der Recherche, denn der Reihentitel verweist mit `s.a.` oder `s.u.` auf entsprechend andere Datensätze, unter welchen man gewünschte Informationen finden kann.
`ORT`            | Diplomatische Transkription der Orts- und Verlagsangabe.
`JAHR`           | Jahr, auf welches sich der Musenalmanach bezieht. Das Erscheinungsjahr ist demnach entweder im selben Jahr oder ein Jahr früher, jedoch auch sonst nicht unbedingt im Almanach angegeben.
`HERAUSGEBER`    | Diplomatische Transkription der Verantwortlichkeitsangabe.    
`ANMERKUNGEN`    | Anmerkugen zur Erfassung des Almanachs / Sonderzeichen s.u.
`AUTOPSIE`       | Wahr, falls ein Exemplar des Almanachs gesichtet wurde.       
`VORHANDEN`      | Wahr, falls der Almanach im Bestand der Stiftung ist.
`VORHANDEN ALS`  | Mögliche Werte: Original, Reprint, Fremde Herkunft. Mehrere Werte möglich. Herkunft des gesichteten Exemplars.
`NACHWEIS`       | Quellen für bibliografische Angaben über den Almanach aus bekannten Metadatenwerken.
`STRUKTUR`       | Beschreibt Aufbau und Struktur eines Almanachs.
`NORM`           | ~ markiert erfasste Bände. Ursprünglich gedacht als Dokumentation des Soll-Zustands eines Almanachs kaum genutzt.
`VOLLST. ERFASST`| Wahr, wenn der Almanach vollständig erfasst wurde.
`HRSGREALNAME`   | Verweis auf die `REALNAMEN-Tab`. Gibt den Norm-Datensatz an, unter welchem der/die Herausgeber:in erfasst ist und identifiziert ihn/sie eindeutig.
`BEARBEITET AM`  | Datum der letzten Bearbeitung.
`BEARBEITET VON` | Kürzel des letzten Bearbeiters.
`ID`             | Unbenutzt.