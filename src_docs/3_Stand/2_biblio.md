# GM-Biblio
## Allgemeines
Die Tabelle `GM-BIBLIO` dient zur Dokumentation des Stiftungsbestands. Weiter gibt es in der Tabelle auch bibliografische Einträge für noch nicht im Stiftungsbestand befindliche, aber gesuchte Titel. Es gelten dieselben [Abkürzungen wie in der Musenalm](1_Musenalmanache/1_allgemeines.md#abkürzungen). Ebenso für die [Symbole](1_Musenalmanache/1_allgemeines.md#symbole). Einem gewissen Umfang nach decken sich also die Einträge der Tabellen -->&nbsp;[`AlmNeu`](1_Musenalmanache/2_AlmNeu.md) und `GM-BIBLIO`. Die Tabelle hat zzt. <!-- TODO -->XY Einträge.

## Felder
Feld             |  Datentyp | Bedeutung und Anmerkungen 
----------------:|-----------|--------------------------
`NUMMER` | ulong | Mit 1 beginnende, fortlaufende, eindeutige Nummer des bibliographischen Eintrags (Tabellenschlüssel).
`ORT` | Kurzer Text | Diplomatische Transkription der Orts- und Verlags- und manchmal Vertriebsangabe.<br><br>[:material-file-document:&nbsp;Feldwerte](../../files/feldwerte/AlmNeu_ORT.txt)
`JAHR` | ulong | Jahr, auf welches sich der Musenalmanach bezieht. Das Erscheinungsjahr ist demnach entweder im selben Jahr oder ein Jahr früher, jedoch auch sonst nicht unbedingt im Almanach angegeben.
`HERAUSGEBER` | Kurzer Text | Herausgeber des Buches. (Bei Personen) im Format Nachname, Vorname. Mehrere Werte möglich, getrennt mit `;`, manchmal auch mit `und`. Relativ unstrukturiert, es gibt Angaben mit `u.a.`, Organisationen sind nicht von Personen unterschieden, es gibt Anmerkungen und Ortsangaben.<br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_HERAUSGEBER.txt)
`ALLGEMEINE BEMERKUNG` | Langer Text | Anmerkungen zur Erfassung des bibliografischen Eintrags s.a. --> [Sonderzeichen](1_Musenalmanache/1_allgemeines.md#symbole)<br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_ALLGEMEINE_x0020_BEMERKUNG.txt)
`VORHANDEN ALS`  | Kurzer Text | Werte (normalisiert): Original, Reprint, Fremde Herkunft. Mehrere Werte möglich, Trennung mit `;`. Herkunft des gesichteten Exemplars.
`NACHWEIS` | Kurzer Text | Mehrere Werte möglich, Trennung mit `;`. Quellen für bibliografische Angaben über den Almanach aus bekannten Metadatenwerken. <br><br>[:material-file-document:&nbsp;Feldwerte](../../files/feldwerte/AlmNeu_NACHWEIS.txt)
`AUFGENOMMEN` | Kurzer Text | Datum der Aufnahme.
`BEARBEITET VON` | Kurzer Text | Kürzel des letzten Bearbeiters.
