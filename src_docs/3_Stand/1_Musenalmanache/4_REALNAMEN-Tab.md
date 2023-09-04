# Musenalmanache: `REALNAMEN-Tab`
Die Tabelle `REALNAMEN-Tab` erfasst Normdaten von an Inhalten (Urheber:innen) oder Bänden (Herausgeber:innen) beteiligten Personen. Da die Verweisfelder -->&nbsp;[`AlmNeu/HRSGREALNAME`](2_AlmNeu.md) und -->&nbsp;[`INH-Tab/AUTORREALNAME`](3_INH-Tab.md) nicht mehrere Verweise in einem Feld aufnehmen können, sind n:m-Beziehungen zwischen den Tabellenpaaren durch die (redundante) Eintragung von vorkommenden Personenkombinationen in der `REALNAMEN-Tab` realisiert. Auf diese Weise hat die Tabelle zzt. 5.384 Einträge.

Feld             |  Datentyp | Bedeutung und Anmerkungen 
----------------:|-----------|--------------------------
`REALNAME` | Kurzer Text | Einzigartige Bezeichnung einer Person, Personenname (Tabellenschlüssel). Auch Personenkombinationen möglich s.o. Im Falle einer Kombination bleiben alle anderen Felder des Eintrags leer.<br><br>[:material-file-document:&nbsp;Feldwerte](../../files/feldwerte/REALNAME-Tab_REALNAME.txt)
`Daten`| Kurzer Text | Geburts- und Sterbedatum einer Person.
`Nachweis`| Kurzer Text | Metadatenwerke, welchen die normalisierte Form der Personenschreibweise entnommen wurde. Mehrere Werte möglich, mit `;` getrennt.
`Beitrag` | Kurzer Text | Beruf oder Tätigkeit der Person.
`Pseudonym` | Langer Text | Andere Identitäten, die sich eine Person in bestimmten Zusammenhängen selbst gibt. Mehrere Werte möglich, mit `;` getrennt.