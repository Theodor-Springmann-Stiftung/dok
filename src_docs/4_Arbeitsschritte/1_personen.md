# 1. Personen
## Personenschreibweisen
<div class="task-list-right"></div>
- [x] abgeschlossen
??? bug "Problem: Personenschreibweisen"
    Weil es bisher keine Möglichkeit gab, Personen einzeln Bänden oder Inhalten zuzuweisen, wurden in der `REALNAMEN-Tab` Personenkombinationen angelegt, getrennt durch `u.` oder `;`, um mehrere Personen einer Veröffentlichung oder einem Inhalt zuzuweisen. Weil sich das ändern soll (beispielsweise für eine sortierte Personenliste), muss für jede Person innnerhalb einer Personengruppe ein Eintrag vorhanden sein, der in der Schreibsweise exakt mit der zusammengesetzten Aufnahme übereinstimmt.

Es wurden Listen von Personen ausgegeben, deren zusammengesetzter Eintrag maschinell mit keinem Einzeleintrag in der `REALNAMEN-Tab` identifiziert werden kann. Die Schreibweisen und Einträge wurden in der vorhandenen Access-DB einzeln korrigiert.


## Verwaiste Einträge
<div class="task-list-right"></div>
- [x] abgeschlossen
??? bug "Problem: Verwaiste Einträge"
    In der `INH-Tab` gab es Einträge zu Inhalten, die auf keinen existierenden Band verweisen. Weiter gibt es Personen, denen kein Werk zugeordnet ist.

Die entsprechenden Einträge wurden gelöscht.