# 2. Reihentitel
## Normalisierung der Reihentitel
<div class="task-list-right"></div>
- [ ] in Arbeit
??? bug "Problem: Normalisierung der Reihentitel" 
    Das Feld `AlmNeu/REIHENTITEL` enthält zzt. nicht nur Reihentitel, sondern alle möglichen Angaben, darunter Angaben zur Ausgabe oder zum autpsierten Exemplar. Manachmal sind auch mehrere Reihentitel zu einem einzelnen Band angegeben, getrennt mit `/)`. Das Feld muss für die maschinelle Zuordnung eines Bandes zu einer Reihe normalisiert werden, etwa zur Ausgabe einer Liste von Reihen.

    --> s.a. [:material-file-document:&nbsp;Feldwerte](../files/feldwerte/AlmNeu_REIHENTITEL.txt)

In einer Überarbeitungsversion der DB, `alma\Datenbank\Ueberarbeitungsdatenbank_Aktuelle_Bearbeitung.accdb`, (>Access 2016) die so angelegt ist, dass 

1. n:m-Beziehungen zwischen Bänden und Reihen möglich sind und 
2. eine gesonderte Reihentabelle `Reihen` existiert, 

müssen entsprechende Einträge in der Tabelle vorgenommen werden, dann werden die Bände jeweils ihren Reihen zugeordnet. Die Zuordnung selbst hat unterschiedliche Qualität: manche Bände sind Übersetzungen einzelner 
Reihen, andere direkte Ausgaben der Reihen. Das wird in Beziehungstripeln der Form Subjekt-Prädikat-Objekt erfasst:

    <Band> ist in der Reihe erschienen <Reihe>
    <Band> kann auch als Teil der Reihe gelten <Reihe>
    <Band> ist die dt. Übertragung der Reihe <Reihe>
    <Band> ist die fr. Übertragung der Reihe <Reihe>

!!! info
    Es gibt eine Excel-Tabelle, die alle Reihen enthält, so müssen nur selten Titel gesondert angelegt werden.


## Aliaseinträge in der `AlmNeu`
<div class="task-list-right"></div>
- [ ] in Arbeit
??? bug "Problem: Aliaseinräge in der AlmNeu-Tabelle"
    In der Tabelle `AlmNeu` gibt es zzt. Alias-Einträge, markiert mit `s.a` und `s.u.`, um Bände auffindbar zu machen, die anderen Reihen zugeordnet sind. Diese Einträge sind anderer Natur als die anderen und dienen der Auffindbarkeit.

Aliaseinträge sollen durch die Möglichkeit, einem Band mehrere Reihen zuordnen zu können schließlich überflüssig werden. Der Eintrag der `AlmNeu`

    AlmNeu NUMMER 1015, Feld REIHENTITEL: Taschenbuch für die Gegenden am Niederrhein s. u. Taschenbuch, Bergisches 1804

soll dadurch überflüssig werden, dass dem Band des bergischen Taschenbuchs von 1804 ein weiterer Reihentitel hinzugefügt wird:

    Baende NUMMER 1018, Bergisches Taschenbuch 1804, Reihe(n): 
    (Band) Bergisches Taschenbuch 1804 ist Teil der Reihe Taschenbuch, Bergisches
    (Band) Bergisches Taschenbuch 1804 kann auch als Teil der Reihe gelten Taschenbuch für die Gegenden am Niederrhein

So kann in einem neuen System das Bergische Taschenbuch von 1804 einmal unter dem Reihentitel `Taschenbuch, Bergisches` und einmal unter dem Titel `Taschenbuch für die Gegenden am Niederrhein` gefunden werden.


## Anmerkungen zur Reihe
<div class="task-list-right"></div>
- [ ] in Arbeit
??? bug "Problem: Anmerkungen zur Reihe in `AlmNeu/ANMERKUNGEN`"
    Anmerkungen zur Reihe sind zzt. dem ersten Band einer Reihe im Feld `AlmNeu/ANMERKUNGEN` zugeordnet, obwohl sie für die ganze Reihe gelten.

Weiter werden Anmerkungen, die eigentlich die Reihe kommentieren, aber meistens im  beim ersten Band einer Reihe stehen, in das Feld `Anmerkungen` der neuen Tabelle `Reihen` kopiert.


## Weitere Angaben in `AlmNeu/REIHENTITEL`
<div class="task-list-right"></div>
- [ ] ungeklärt, Fälle werden gesammelt
??? bug "Problem: Weitere Angaben in `AlmNeu/REIHENTITEL`"
    Weiter gibt es im Feld  `AlmNeu/REIHENTITEL` noch einige weitere Angaben außer der Reihentitel, die aus Not an geeigneten Feldern dort hinterlegt wurden.

    Bisher gesammelte Fälle:
    
    - Titelauflage – wie behandeln?
    - Pos. 40/4847 Aglaja (Wien) [var.] – wie behandeln?
    - Pos. 45/4847: Agrionien [oJ] – eckige Klammer einfach löschen?


    --> s.a. [:material-file-document:&nbsp;Feldwerte](../files/feldwerte/AlmNeu_REIHENTITEL.txt)

Bei der Durchsicht der Bände werden Fälle und Beispiele gesammelt; dann kann geschaut werden, wie die Informationen auf andere oder neue Felder verteilt werden können.

!!! info
    Das Feld `AlmNeu/REIHENTITEL` selbst ist weiter nützlich; es enthält so eine Art Kurz- oder Suchtitel für den einzelnen Band, die langen transkribierten Titel taugen zur Anzeige und schnellen Identifikation nicht. Grundsätzlich erfordert die Aufnahme von Normdaten in bibligrafischen Systemen einen Titel, der zur Anzeige in Trefferlisten taugt (--> [Werktitel – Dokumentationsplattform des Standardisierungsausschusses](https://sta.dnb.de/doc/RDA-E-W005))

