# GM-Biblio
## Allgemeines
Die Tabelle `GM-BIBLIO` dient zur Dokumentation des Stiftungsbestands. Weiter gibt es in der Tabelle auch bibliografische Einträge für noch nicht im Stiftungsbestand befindliche, aber gesuchte Titel. Es gelten dieselben [Abkürzungen wie in der Musenalm](1_Musenalmanache/1_allgemeines.md#abkürzungen). Ebenso für die [Symbole](1_Musenalmanache/1_allgemeines.md#symbole). Einem gewissen Umfang nach decken sich also die Einträge der Tabellen -->&nbsp;[`AlmNeu`](1_Musenalmanache/2_AlmNeu.md) und `GM-BIBLIO`. Die Tabelle hat zzt. <!-- TODO -->XY Einträge.

## Tabellenfelder
<div class="sortable-table"></div>
Feld             |  Datentyp | Bedeutung und Anmerkungen 
----------------:|-----------|--------------------------
`NUMMER` | ulong | Mit 1 beginnende, fortlaufende, eindeutige Nummer des bibliographischen Eintrags (Tabellenschlüssel).
`ORT` | Kurzer Text | Ortsangabe, in Klammern der Verlag.<br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_ORT.txt)
`JAHR` | ulong | (Erscheinungs)jahr
`HERAUSGEBER` | Kurzer Text | Herausgeber des Buches. (Bei Personen) im Format Nachname, Vorname. Mehrere Werte möglich, getrennt mit `;`, manchmal auch mit `und`. Relativ unstrukturiert, es gibt Angaben mit `u.a.`, Organisationen sind nicht von Personen unterschieden, es gibt Anmerkungen und Ortsangaben.<br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_HERAUSGEBER.txt)
`AUTOR` | Kurzer Text | Autor des Buches, im Format Nachname, Vorname. Mehrere Werte möglich, getrennt mit `;`, selten mit `und`. manchmal gibt es Anmerkungen mit Rollenangaben wie `Illustr.` oder `Vorw.`.<br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_AUTOR.txt)
`ALLGEMEINE BEMERKUNG` | Langer Text | Anmerkungen zur Erfassung des bibliografischen Eintrags s.a. --> [Sonderzeichen](1_Musenalmanache/1_allgemeines.md#symbole)<br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_ALLGEMEINE_x0020_BEMERKUNG.txt)
`NACHWEIS` | Kurzer Text | Mehrere Werte möglich, Trennung mit `;`. Quellen für bibliografische Angaben über das Buch aus bekannten Metadatenwerken. Deckt sich mit [`AlmNeu/NACHWEIS`](1_Musenalmanache/2_AlmNeu.md) <br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_NACHWEIS.txt)
`AUFGENOMMEN` | Kurzer Text | Datum der Aufnahme.
`BEARBEITET VON` | Kurzer Text | Kürzel des letzten Bearbeiters.
`GEÄNDERT` | Datum | Datum der letzten Änderung.
`EINGETRAGEN?` | boolean | TODO
`UNKLAR` | boolean | TODO
`GESUCHT?` | boolean | TODO
`ERSCHIENEN IN` | Langer Text | Nicht so wirklich gepflegt, nur etwa 29 sinnvolle Angaben. Angabe über Reihe oder Sammelband in welcher ein gesuchter Text erschienen ist. <br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_ERSCHIENEN_x0020_IN.txt)
`INHALT` | Kurzer Text | Angabe der Kategorie, der Gattung oder des Genres eines Buches. Kann sicher normalisiert werden.<br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_INHALT.txt)
`KURZTITEL` | Langer Text | Angabe eines Kurztitels. Deckt sich bei Almanachen oft mit dem Feld, könnte als Vereinfachung dessen gelten, [`AlmNeu/REIHENTITEL`](1_Musenalmanache/2_AlmNeu.md)<br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_KURZTITEL.txt)
`TITEL` | Langer Text | Titel des Buches mit angehängtem Jahr.<br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_TITEL.txt)
`UNTERTITEL` | Langer Text | Untertitel des Buches, oftmals eine diplomatische Transkription der Gesamttitelangabe mit Zeilenbrüchen oder Virgeln `/`. Deckt sich bei Almanachen mit [`AlmNeu/TITEL`](1_Musenalmanache/2_AlmNeu.md)<br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_UNTERTITEL.txt)
`VORHANDEN ALS` | Kurzer Text | Mögliche Werte (normalisiert): (1., 2., 3.) Exemplar, fremde Herkunft, Fotokopie, Mangel, Neudruck, Nicht vorhanden, Original, Papiere, Reprint, unklar Sammelband, DVD, anders. Deckt sich bei Almanachen mit [`AlmNeu/VORHANDEN ALS`](1_Musenalmanache/2_AlmNeu.md)<br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_VORHANDEN_x0020_ALS.txt)
`ZUGEHÖRIG` | Kurzer Text | Mögliche Werte (normalisiert): Almanach-Smlg. 7138x, Auktionen 2x, Bad. Landesbibliothek, Baden 300, Hebel-Archiv, Kalender-Smlg., Karlsruhe, Kultur 5, Lenz-Archiv, Medizin, Nachschlag, Zeitschrift, Zschokke-Smlg. Organisiert die `GM-BIBLIO` in unterschiedliche Sammlungen.
`STANDORT` | Kurzer Text | Gibt den Standort eines Exemplars an TODO
`ZUSTAND` | Kurzer Text | Gibt den Zustand eines Exemplars nach einem eigenen Klassifikationssystem an.TODO<br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_ZUSTAND.txt)
`NOTIZ ÄUSSERES` | Langer Text | Anmerkungen zu Zustand und Mängel des Exemplars.<br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_NOTIZ_x0020_ÄUSSERES.txt)
`NOTIZ INHALT` | Langer Text | Anmerkungen zum Umfang und Art des Exemplars/Buches. Vermischtes, nicht viele Eintragungen.<br><br>[:material-file-document:&nbsp;Feldwerte](../files/feldwerte/GM-BIBLIO_NOTIZ_x0020_INHALT.txt)