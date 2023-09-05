# Umgestaltung der Datanbank(en): Ziele & Fortschritt
Die Umgestaltung der ganzen Sammlungsdokumentation hat grundsätzlich zwei verschiedene Seiten:

1. Das Arbeiten, Recherchieren und Bereitstellen der Daten (für Editor:innen der Datenbank). Nach dieser Seite sind von besonderem Interesse die Datenintegrität, langfristige Speicherung und Sicherung, Kompatibilität mit zukünftigen Technologien und Standards und nicht zuletzt Nutzerfreundlichkeit.
2. Die Präsentation der Daten für Interessierte, i.d.R. Forscher und Antiquare. Bibliographische Daten sollen in angemessener Weise präsentiert werden, mit Sortier- und Recherchemöglichkeiten nach Reihen, Personen, Bänden, einer zuverlässigen Suchfunktion und der Möglichkeit zum Herunterladen digitaler Reproduktionen. 

## Unterschiede in formaler Erfassung zwischen Almanachen & `GM-BIBLIO`
Grundsätzlich ist die Präsentation der Daten für die **Buch-Datenbank** [`GM-BIBLIO`](3_Stand/2_biblio.md) nicht so wichtig, da sie keine öffentliche Seite hat und mit ihr nicht über einen gegliederten Zugang, sondern hauptsächlich über die Volltextsuche interagiert wird. Damit wird die fehlende Atomisierung beispielsweise von Herausgeber- oder Autorangaben eher zur Frage der Standardisierung und Kompatibilität (mit externen Datenbanken), und bereitet nur wenig Einschränkungen der Funktionalität.

!!! info "Grenzen formaler Gliederung"
    Grundsätzlich wird nach der gewünschten Funktion und Form der Interaktion mit der Datenbank gefragt, um zu entscheiden, wie weit atomisiert und formal strukturiert die Daten gehalten und gepflegt werden müssen. Es gilt: so formal aufbereitet wie für die gewünschte Funktionalität möglich, aber auch nicht mehr als nötig.

    Vorgeschlagene Basisfunktionalität der Datenbanken kann sein, eine bibliographisch korrekte und vollständige Angabe zu jeweils einem Eintrag generieren zu können, um das Buch oder den Band eindeutig zu identifizieren.

Für die `GM-BIBLIO` reicht es wahrscheinlich, zusätzlich festzuhalten, dass das Buch, falls vorhanden, an seinem Standort auffindbar sein muss.

Die [**Almanache**](3_Stand/1_Musenalmanache/1_allgemeines.md) sind da anders: da der Zugang zu den Daten sich ändert, so ändern sich auch Fragen der Speicherung und Atomisierung. Eine Sortierung und Anzeige der Daten nach Herausgebern oder Urhebern beispielsweise erfordert eine Normalisierung der Schreibweisen. Ähnlich eine Darstellung nach Reihen oder Auflistung nach Jahren. Weiter steckt in der Musenalm *wissenschaftliche* Arbeit: die Felder sind nicht notwendig nur Dokumentation, sondern speichern Forschungsergebnisse.

## Musenalm: Allgemeine Anforderungen
- Als bibliografisches Nachschlagewerk müssen eindeutige Identifikatoren für Reihen und Bände geschaffen werden.
- Es muss möglich werden, anhand der Daten präzise, eindeutige bibliographische Angaben zu generieren. <!-- TODO --> harvard AMA etc.

## Musenalm: Gewünschte Funktionen der Seitendarstellung
- Volltextsuche
- Anzeige nach Reihen, Bänden und Personen gegliedert
- Bereitstellung von Scans und Inhalten
- Redaktionelle Bearbeitungsmöglichkeiten von Seiten & Dokumentation
- Auffindbarkeit der Seiten über einschlägige Suchmaschinen

## Musenalm: Anforderungen interner Dokumentation
- Nachhaltige Datenspeicherung, -aufbewahrung und -sicherung
- Möglichkeiten zur genauen Recherche und Überarbeitung der Daten
- Möglichkeiten der Integration der Datenbanken in bereits existierende Bibliotheks- und Nachschlagewerke
- Möglichkeiten zur Übergabe des Datenbanksystems an »professionelle« Akteure, Bibliothekare, eingelernte Personen
- Möglichkeiten zum Anschluss aller Sammlungen, Musenalm-, BIBLIO- und Teppichsammlung
- Benutzerfreundlichkeit, dazu gehört:
    * Bedienung von überall über den Browser
    * Eine Ansprechende und einfache Oberfläche zur Bearbeitung der Datensätze

## Arbeitsschritte & Vorgehen

