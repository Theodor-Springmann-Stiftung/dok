# Musenalmanache: `AlmNeu`
Die Tabelle `AlmNeu` erfasst einzelne Bände (im Sinne von: Bücher) der Almanache, entweder als bibliografische Angabe nach einem Zeugen oder als Angaben in diplomatischer Umschrift nach einem gesichteten boolean ulong Exemplar. Ausnahmen s.u. `REIHENTITEL`. Zurzeit hat die Tabelle etwa 4.847 Einträge. Die Felder der Tabelle lauten:

## Felder
Feld             |  Datentyp | Bedeutung und Anmerkungen 
----------------:|:----------|:-------------------------
`NUMMER` | ulong | Mit 1 beginnende, fortlaufende, eindeutige Nummer des Bandes (Tabellenschlüssel).
`BIBLIO-NR` | ulong | Nummer, unter welcher der Band in der Biblio-Tabelle (-->&nbsp;[`GM-BIBLIO\NUMMER`](../2_biblio.md)) erfasst ist. Leer, falls nicht.
`ALM-TITEL` | Kurzer Text | Diplomatische Transkription des Gesamttitelangabe des Titelblatts mit Zeilenumbrüchen (`/`). Leer, falls kein Band vorgelegen hat.
`REIHENTITEL` | Kurzer Text | Gibt Reihe(n) an, unter welche ein Band eingeordnet werden kann und nachfolgend das Jahr des Bandes. <br><br>Weiterhin wird angegeben, ob ein Band außerdem einer anderssprachigen Reihe zuzuordnen ist (als Übersetzung, oftmals aber nicht immer getrennt mit `/)`). <br><br>Weiter enthält es Ausgabevermerke (wie `(1)` oder `Titelauflage` <!-- TODO -->) und bei Mehrfacherfassung ein `[var]`, `[var.2]` u.s.w. <br><br>Weiter sind einige Datensätze bis auf dieses Feld leer und dienen nur der Recherche, denn der Reihentitel verweist mit `s.a.` oder `s.u.` auf entsprechend andere Datensätze, unter welchen man gewünschte Informationen finden kann.<br><br>[:material-file-document:&nbsp;Feldwerte](../../files/feldwerte/AlmNeu_REIHENTITEL.txt)
`ORT` | Kurzer Text | Diplomatische Transkription der Orts- und Verlags- und manchmal Vertriebsangabe.<br><br>[:material-file-document:&nbsp;Feldwerte](../../files/feldwerte/AlmNeu_ORT.txt)
`JAHR` | ulong | Jahr, auf welches sich der Musenalmanach bezieht. Das Erscheinungsjahr ist demnach entweder im selben Jahr oder ein Jahr früher, jedoch auch sonst nicht unbedingt im Almanach angegeben.
`HERAUSGEBER` | Kurzer Text | Diplomatische Transkription der Verantwortlichkeitsangabe.    
`ANMERKUNGEN` | Langer Text | Anmerkungen zur Erfassung des Almanachs s.a. --> [Sonderzeichen](1_allgemeines.md#symbole)<br><br>Im ersten Band einer Reihe finden sich Angaben zu Herausgeber, Ort, Verlag und Erscheinungsfolge.<br><br>[:material-file-document:&nbsp;Feldwerte](../../files/feldwerte/AlmNeu_ANMERKUNGEN.txt)
`AUTOPSIE` | boolean | Wahr, falls mind. ein Exemplar des Almanachs gesichtet wurde.       
`VORHANDEN` | boolean | Wahr, falls der Almanach im Bestand der Stiftung ist.
`VORHANDEN ALS`  | Kurzer Text | Werte (normalisiert): Original, Reprint, Fremde Herkunft. Mehrere Werte möglich, Trennung mit `;`. Herkunft des gesichteten Exemplars.
`NACHWEIS` | Kurzer Text | Mehrere Werte möglich, Trennung mit `;`. Quellen für bibliografische Angaben über den Almanach aus bekannten Metadatenwerken. <br><br>[:material-file-document:&nbsp;Feldwerte](../../files/feldwerte/AlmNeu_NACHWEIS.txt)
`STRUKTUR` | Kurzer Text | Beschreibt Aufbau und Struktur eines Almanachs s.a. --> [Abkürzungen](1_allgemeines.md#abkürzungen)<br><br>[:material-file-document:&nbsp;Feldwerte](../../files/feldwerte/AlmNeu_STRUKTUR.txt)
`NORM` | Kurzer Text | `~` (Tilde) markiert erfasste Bände. Ursprünglich gedacht als Dokumentation des Soll-Zustands eines Almanachs, jedoch kaum genutzt.<br><br>[:material-file-document:&nbsp;Feldwerte](../../files/feldwerte/AlmNeu_NORM.txt)
`VOLLST. ERFASST`| boolean | Wahr, wenn der Almanach vollständig erfasst, d.h. seine Inhalte in die [`INH-Tab`](3_INH-Tab.md) eingetragen wurde.
`HRSGREALNAME` | -->&nbsp;`HRSGREALNAME-Tab/REALNAME` | Verweis auf [`REALNAME-Tab`](4_REALNAMEN-Tab.md). Gibt den Norm-Datensatz an, unter welchem der/die Herausgeber:in des Bandes erfasst ist und identifiziert ihn/sie eindeutig. Mehrere Werte möglich, denn in der REALNAMEN-Tab sind alle vorkommenden Kombinationen als gesonderte Datensätze eingetragen. Trennung mit `;` (Grafik) oder `u.` (Text).
`BEARBEITET AM` | Datum | Datum der letzten Bearbeitung.
`BEARBEITET VON` | Kurzer Text | Kürzel des letzten Bearbeiters.
`ID` | ? | Unbenutzt.