# Musenalmanache: Allgemeines und Vorgehen
## Zum Gegenstand: Musenalmanache
Die Sammlung von Musenalmanachen erfasst derzeit etwa 4500 Bände von meist jährlich erscheinenden Periodika mit Kalendarium, Text-, Noten- und/oder Bildbeiträgen aus dem 18. und 19. Jahrhundert. Dabei erfassen die Daten nicht nur Almanache, die dem Bestand der Sammlung der TSS ausmachen, sondern versteht sich vor allem als *Bibliografie* überhaupt existierender Almanache. Die wichtigsten Zeugen für die Bibliografie sind unter anderen:

Goedecke

<!-- TODO -->

Die Erfassug von Musenalmanachen erfordert drei Ebenen einer  hierarchischen Gliederung:

`Reihen`
:   Reihen sind Zusammenfassungen jährlich erscheinender Almanache, meist werden Reihentitel aus den Titelangaben einzelner Bäne abstrahiert. Manche Reihen sind offensichtlich Übersetzungen französischer Titel. In der Access-DB sind Reihen bisher nicht gesondert, sondern im Feld `REIHENTITEL` in der Tabelle `AlmNeu` dokumentiert.

`Bände`
:   Einzelne Ausgaben, konkrete Manifestatationen von Almanachen. Nicht nur gesichtete, sondern auch nur bezeugte Ausgaben. In der Tabelle `AlmNeu` sind Bände als Einträge erfasst.

`Inhalte`
:   Einzelne Beiträge eines Bandes, wie Kalendarium, Gedicht, Titel, Stich u.s.w. In der Tabelle `INH-TAB` als Einträge erfasst.

## Bilder
Die Bilder sind als JPG und mit Wasserzeichen bzw. Erkennungsnummern versehen im Ordner `alma/Scans/` gespeichert. Die Benennung der Unterordner folgt der Angabe in Tabelle/Feld `AlmNeu/NUMMER`, die der Dateien der Angabe in Tabelle/Feld `INH-Tab/OBJZAEHL`. Der Ordner ist etwa 7.25 GB groß.

## Reihen-Datei
Zusätzlich zur Access-Tabelle s.u. gibt es noch eine Excel-Liste aller Reihentitel.

## Abkürzungen

## Symbole

## Access-Datei
Die Daten werden in einer Access-Datei gehalten und eingetragen (Pfad: `alma/Datenbank/Datenbank_Aktuelle_Bearbeitung.mdb`). Zusätzlich zu den beiden oben genannten Tabellen ist die Tabelle `REALNAMEN-Tab` relevant, die Normdaten für Personennamen enthält.
