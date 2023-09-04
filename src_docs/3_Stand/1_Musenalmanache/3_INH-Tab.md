# Musenalmanache: `INH-Tab`
Die Tabelle `INH-Tab` erfasst Beiträge einzelner katalogisierter Bände Seiten- oder Beitragsweise. Inhalte können nicht nach der Seitenzahl sortiert werden, sondern benötigen einen extra Zähler `OBJZAEHL` da ein Band mehrere verschiedene Seitenzählungen enthalten kann. Die Tabelle enthält zzt. etwa 130325 Einträge. Die Felder der Tabelle lauten:

<!-- TODO: Seiten- oder Beitragsweise??
    Auch bekannte nicht erfasste Inhalte?  -->

Feld             |  Datentyp | Bedeutung und Anmerkungen 
----------------:|-----------|--------------------------
`ID` | ulong | Band, in welchem der Inhalt zu finden ist (`AlmNeu/NUMMER`). 
`INHNR`| ulong | Mit 1 beginnende, fortlaufende, eindeutige Nummer des Inhalts.
`AUTORREALNAME`| -->&nbsp;`REALNAME-Tab/REALNAME` | Verweis auf [`REALNAME-Tab`](4_REALNAMEN-Tab.md). Gibt den Norm-Datensatz an, unter welchem der/die Urheber:in des Inhalts erfasst ist und identifiziert ihn/sie eindeutig. Mehrere Werte möglich, denn in der REALNAMEN-Tab sind alle vorkommenden Kombinationen als gesonderte Datensätze eingetragen. Trennung mit `;` (Grafik) oder `u.` (Text). Verfasser, Dichter, Zeichner, Stecher des Inhalts. 
`AUTOR` | Kurzer Text | Diplomatische Transkription der Autorangabe(n)
`TITEL` | Langer Text | Diplomatische Transkription der Gesamttitelangabe mit Zeilenumbrüchen (`/`). Leer, falls es sich um eine Zeichnung, Kalendarium o.ä. handelt.
`INCIPIT` | Langer Text | Diplomatische Transkription des Incipits mit Zeilenumbrüchen (`/`) bei Textbeiträgen, selbst wenn ein Titel eingetragen wurde.
`SEITE` | ufloat | Diplomatische Transkription der Seitenangabe, auf welcher der Inhalt zu finden ist. Zurzeit istbei mehreren Inhalten auf einer Seite mit der Nachkommastelle die Reihenfolge der Inhalte festgehalten, das muss aber nicht weiter erhoben werden.
`OBJZAEHL` | ufloat | Nummer des Beitrags innerhalb des Bandzusammenhangs. Wid benötigt, um eine eindeutige Reihenfolge herzustellen. Kommazahlen bezeichnen meist zusammengehörige, mehrseitige Inhalte. 
`ANMERKINH` | Langer Text | Anmerkungen zur Erfassung des Inhalts s.a. --> [Sonderzeichen](1_allgemeines.md#symbole)<br><br>[:material-file-document:&nbsp; Feldwerte](../../files/feldwerte/INH-TAB_ANMERKINH.txt)
`PAG` | Kurzer Text | Mögliche Werte (normalisiert): ar, röm, ar1-7, röm1-7. Gibt die Art der Paginierung einer Seite an.
`OBJEKT` | Kurzer Text | Möglich Werte (normalisiert): Corrigenda, Diagramm, Gedicht/Lied, Graphik, Graphik-Verzeichnis, graph. Anleitung, graph. Strickanleitung, graph. Tanzanleitung, Inhaltsverzeichnis, Kalendarium, Karte, Musikbeigabe, Musikbeigaben-Verzeichnis, Motto, Prosa, Rätsel, Sammlung, Spiegel, szen. Darstellung, Tabelle, Tafel, Titel, Text, Trinkspruch, Umschlag, Widmung. Mehrere Werte möglich. Gibt das Genre eines Inhalts an.
`BILD` | boolean | Wahr, ein Digitalisat des Inhalts verfügbar ist -->&nbsp;[Bilder](1_allgemeines.md#bilder).