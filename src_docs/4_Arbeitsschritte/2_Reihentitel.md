# 2. Reihentitel

??? bug "Problem: Reihentitel"
    Das Feld `AlmNeu/REIHENTITEL` enthält zzt. nicht nur Reihentitel, sondern alle möglichen Angaben, darunter Angaben zur Ausgabe oder zum autpsierten Exemplar. Manachmal sind auch mehrere Reihentitel zu einem einzelnen Band angegeben, getrennt mit `/)`. Das Feld muss für die maschinelle Zuordnung eines Bandes zu einer Reihe normalisiert werden, auch zur Ausgabe etwa einer Liste von Reihen.

??? bug "Problem: Anmerkungen Reihentitel"
    Anmerkungen zur Reihe sind zzt. dem ersten Band einer Reihe zugeordnet, obwohl sie zur ganzen Reihe gehören.

??? bug "Problem: Aliaseinräge in der AlmNeu-Tabelle"
    In der Tabelle `AlmNeu` gibt es zzt. Alias-Einträge, markiert mit `s.a` und `s.u.`, um Bände auffindbar zu machen, die anderen Reihen zugeordnet sind. Diese Einträge sind anderer Natur als andere bibliografische und dienen der Auffindbarkeit.

In einer Korrekturversion der DB, die so angelegt ist, dass n:m-Beziehungen zwischen Bänden und Reihen möglich sind und eine getrennte Reihentabelle existiert, müssen entsprechende Einträge in der Tabelle vorgenommen werden, dann werden die einzelnen Bände den Reihen zugeordnet. Die Zuordnung selbst hat unterschiedliche Qualität: manche Bände sind Übersetzungen einzelner Reihen, andere direkte Ausgaben der Reihen. Auch das wird festgehalten, um eine ordentlich Reihentabelle erstellen zu können. Weiter werden Anmerkungen, die zur Reihe gehören, in das Anmerkungsfeld der neuen Reihentabelle kopiert. Weiter können die Alias-Einträge dadurch beseitigt werden, dass ein Band mehreren Reihen zugleich zugeordnet werden kann.

Das Feld `AlmNeu\REIHENTITEL` selbst ist weiter nützlich; es enthält so eine Art Kurz- oder Suchtitel für den einzelnen Band, die langen transkribierten Titel taugen zur Anzeige und schnellen Identifikation nicht.

Es gibt eine Excel-Tabelle, die alle Reihen enthält, so müssen die Titel nicht gesondert angelegt werden.