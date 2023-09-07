# Pflichtfelder, wenn verfügbar:
## IDENTIFIKATOREN
vorhanden           -> rdam:P30004 hat Identifikator für eine Manifestation
vorhanden           -> rdai:P40001 hat identifikator für ein Exemplar

## TITELFELDER
vorhanden/generiert -> rdaw:P10223 hat bevorzugten Titel des Werkes
                           + rdaw:P10219 hat Datum eines Werks
                           + rdaw:P10078 hat Expression eines Werks
                           + rdaw:P10218 hat Ursprungsrt eines Werks
                          (+ rdaw:P100003 hat sonstige unterscheidene Eigenschaft eines Werkes)
                        Ein Titel eines Werks, der in einer bestimmten Anwendung oder in einem bestimmten Kontext als Vorzugsform bestimmt wird.
                        DARF fingiert werden. Ist oftmals identisch mit rdam:P30156 hat Haupttitel. 
                        Textwerke ohne Titel: Incipit
vorhanden/generiert -> rdam:P30156 hat Haupttitel
                       Ein Titel einer Manifestation, der in einer bestimmten Anwendung oder in einem bestimmten Kontext als Vorzugsform bestimmt wird. Oftmals identisch mit
                       rdaw:P10223
teilw. vorhanden    -> rdam:P30142 hat Titelzusatz
unstrukturiert      -> rdam:P30203 hat Paralleltitel
Siehe https://sta.dnb.de/doc/RDA-E-W010

Der bevorzugte Titel des Werks muss nur dann als eigenes Datenelement in der zusammengesetzten Beschreibung erfasst werden, wenn er vom Haupttitel der Manifestation abweicht oder wenn ein zusätzliches unterscheidendes Merkmal erfasst werden muss. In allen anderen Fällen übernimmt der Titel, der als Haupttitel der Manifestation erfasst wurde, zugleich auch die Funktion des bevorzugten Titels des Werks. Unabhängig davon ist es möglich, eine Verknüpfung zu dem entsprechenden Normdatensatz für das Werk anzulegen oder einen identischen Haupttitel explizit als bevorzugten Titel des Werks zu erfassen. Wird ein vorliegender Manifestationstitel als bevorzugter Titel des Werks erfasst und enthält der Manifestationstitel einen offensichtlichen Tipp- oder Schreibfehler, so korrigieren Sie diesen bei der Erfassung des bevorzugten Titels des Werks.

## SONST. FELDER
vorhanden           -> rdam:P30105 hat Verantwortlichkeitsangabe, die sich auf den Hapttitel bezieht
vorhanden           -> rdaw:P10065 hat geistige:n Schöpfer:in
unstrukturiert      -> rdam:P30133 hat Ausgabebezeichnung 
unstrukturiert      -> rdam:P30132 hat Ausgabebezeichnung einer näher erläuterten Überarbeitung
vorhanden           -> rdam:P30088 hat Erscheinungsort
vorhanden           -> rdam:P30011 hat Erscheinungsdatum
unstrukturiert      -> rdam:P30176 hat Verlagsnamen
teilw. vorhanden    -> rdam:P30182 hat Umfang einer Manifestation
nicht vorh.         -> rdam:P30169 hat Maße
teilw. vorhanden    -> rdaw:P10222 hat Art des Inhalts 
teilw. vorhanden    -> rdam:P30003 hat Erscheinungsweise

zum letzten Punkt: die DNB nennt mögliche Werte Einzelne Einheit, mehrteilige Monografie, fortlaufende Ressource oder integrierende Ressource

## REIHENANGABEN
nicht vorh          -> rdam:P30014 hat Zählung innerhalb einer Reihe
generiert           -> rdam:30149 chronologische Bezeichnung der ersten Ausgabe einer Folge
generiert           -> rdam:30150 chronologische Bezeichnung der letzten Ausgabe einer Folge
vorhanden           -> rdam:P30157 hat Titel eine Reihe
"jährlich"          -> rdam:P30146 hat Erscheinungsfrequenz

## STRUKTURELLE ANGABEN
generiert, "Text"   -> rdae:P20001 hat Inhaltstyp
idR "Band"          -> rdam:P30001 hat Datenträgertyp, alternativ Online-Resource, Mikroform, Karte, Blatt
idR ohne Hilfmittel -> rdam:P30002 hat Medientyp, alternativ Computermedien, Mikroform
idR "Deutsch"       -> rdae:P20006 hat Sprache der Expression
idR "Lateinisch"    -> rdae:P20065 hat Schrift (bei nicht-lateinischen Schriften)

## PERSONEN & KÖRPERSCHAFTEN
vorhanden           -> rdaa:P50117 hat bevorzugten Namen einer Person
vorhanden           -> rdaa:P50041 hat bevorzugten Namen einer Körperschaft
vorhanden           -> rdaa:P50194 hat Beruf oder Tätigkeit
vorhanden           -> rdaa:P50121 hat Geburtsdatum
vorhanden           -> rdaa:50120 hat Sterbedatum
nicht vorh.         -> NA hat Titel einer Person
generiert           -> rdaa:P55094 hat Identifikator für eine Person
generiert           -> rdaa:P50005 hat Identifikator für eine Körperschaft

## SONSTIGES
vorhanden           -> Themenbeziehung (bisher nicht Standartisiert)

Verhältnisse rdam:P30135 Manifestation-verkörpertes Werk, rdam:P30139 Manifestation-verkörperte Expression sind sinnvoll für eigenständige Erhebungen.
Zudem sind sie immer von der Manifestation aus gedacht.
Identifikatoren für Werke, Expression nur bei eigenständiger Beschreibung, wir benutzen allerdings eine zusammengesetzte Beschreibung.

# Umgang mit alten Tabellenfeldern
## REALNAME-Tab -> Realnamen werden zu Personen (rdac:C100004) und Körperschaften (rdac:100005)
### Fällt weg
(ID)            -> Fällt weg -> Upload zu Dokumentationszwecken
(ORGANISATION)  -> Fällt weg

### MUSS:
REALNAME        -> rdaa:P50117 hat bevorzugten Namen einer Person; rdaa:P50041 hat bevorzugten Namen einer Körperschaft

### MUSS, falls verfügbar:
Daten           -> rdaa:P50121 hat Geburtsdatum & rdaa:50120 hat Sterbedatum; rdaa:P50037 Gründungsdatum & rdaa:P50038 Auflösungsdatum
Nachweis        -> rdaa:P50368 ist Person beschrieben in; rdaa:P50369 ist Körperschaft beschrieben in
Beitrag         -> rdaa:P50194 hat Beruf oder Tätigkeit; fällt weg bei Körperschaften
Pseudonym       -> Split ;,/),/, dann rdaa:P50428 hat andere Identität einer Person; rdaa:P50025 hat abweichenden Namen einer Körperschaft

### KANN:                
-               -> rdaa:P50395 hat Anmerkung zur Person; rdaa:P50393 hat Anmerkung zur Körperschaft
-               -> rdaa:P55094 hat Identifikator für eine Person; rdaa:P50005 hat Identifikator für eine Körperschaft
-               -> rdaa:P50103 hat abweichenden Namen einer Person
-               -> rdaa:P50031 hat Ort, der mit einer Körperschaft in Verbindung steht
-               -> rdaa:P51106 hat Günderin/Gründer einer Körperschaft

## INH-Tab -> Wird zu musenalm:Inhalt (zusammengesetzte Beschreibung)
### MUSS:
ID              -> rdam:P30020 ist Teil von (Manifestation)
INHNR           -> rdam:30004 hat Identifikator für eine Manifestation
-               -> rdam:P30156 hat Haupttitel: Titel ODER Incipit ODER (Stiche) generierter Titel in [] = rdaw:P10223 hat bevorzugten Titel des Werkes
-               -> rdae:P20001 hat Inhaltstyp
-               -> rdam:P30001 hat Datenträgertyp
-               -> rdam:P30002 hat Medientyp
-               -> rdae:P20006 hat Sprache der Expression
-               -> rdae:P20065 hat Schrift (bei nicht-lateinischen Schriften)

### MUSS, WENN VERFÜGBAR:
AUTOR           -> rdam:P30105 hat Verantwortlichkeitsangabe, die sich auf einen Haupttitel bezieht
TITEL           -> rdam:P30106 hat Gesamttitelangabe & rdam:P30156 hat Hat Haupttitel evtl. mit []
INCIPIT         -> falls kein rdam:P301106 hat Gesamttitelangabe, dann rdam:P30156 hat Haupttitel, dann auch rdam:P30063 Anmerkung zum Titel: Incipit, außerdem in jedem Fall musenalm:incipit
SEITE           -> rdam:P30182 hat Umfang einer Manifestation (beginnend mit "S.")
PAG             -> mit Komma nach rdam:P30182
OBJZAEHL        -> rdaw:P10012 hat Zählung eines Teils od. rdam:30014 Zählung innerhalb einer Reihe
? OBJEKT         -> rdam:P30335 hat Kategorie einer Manifestation s.a. Content Type
AUTORREALNAME   -> rdaw:P10065 hat geistige Schöpferin/geistigen Schöpfer eines Werkes
? BILD          -> rdam:P30016 hat elektronische Reproduktion einer Manifestation mit Verweis; Digitalisat als Ja/Nein zu Archivzwecken

### KANN:
ANMERKINH       -> rdam:P30137 hat Anmerkung zur Manifestation
-               -> rdam:P30063 hat Anmerkung zum Titel einer Manifestation
-               -> rdam:P30061 hat Anmerkung zum Umfang einer Manifestation


## AlmNeu -> Wird zus. mit GM-BIBLIO zu tssbiblio:eintrag (zusammengesetzte Beschreibung) + rdac: Exemplar
BEARBEITET AM   -> Fällt weg
BEARBEITET VON  -> Fällt weg
NUMMER          -> rdam:30004 hat Identifikator für eine Manifestation
BIBLIO-NR       -> rdae:30004 hat Identifikator für ein Exemplar
ALM-TITEL       -> rdam:P30106 hat Gesamttitelangabe & rdam:P30156 hat Haupttitel. Evtl. Hauptitel = Kurztitel (GM-BIBLIO) od. P10223 hat bevorzugten Titel des Werkes = Kurztitel
REIHENTITEL     -> rdam:P30157 hat Titel eine Reihe (Nomen)
ORT             -> rdam:P30088 hat Erscheinungsort und rdam:P30176 hat Verlagsnamen
JAHR            -> rdam:P30011 hat Erscheinungsdatum
HERAUSGEBER     -> rdam:P30105 hat Verantwortlichkeitsangabe, die sich auf den Hapttitel bezieht

ANMERKUNGEN     -> rdam:P30137 hat Anmerkung zur Manifestation
AUTOPSIE        -> musenalm:gesichtet, interner Bearbeitungsvermerk
VORHANDEN       -> Fällt weg (alle Einträge mit einem Exemplar sind vorhanden) Upload zu Dokumentationszwecken
VORHANDEN ALS   -> Fällt weg s.o. Upload zu Dokumentationszwecken
? NACHWEIS      -> rdaw:P10406 hat konsultierte Quelle s.u. ToDo
STRUKTUR        -> rdam:P30182 hat Umfang einer Manifestation
NORM            -> Fällt weg; merge mit rdam:P30182 hat Umfang einer Manifestation
VOLLST. ERFASST -> musenalm:erfasst, interner Bearbeitungsvermerk
HRSGREALNAME    -> rdaw:P10046 hat herausgebenden Akteur

## GM-BIBLIO -> Wird ggf. zus. mit AlmNeu zu tssbiblio:eintrag (zusammengesetzte Beschreibung) + rdac: Exemplar
NUMMER          -> rdae:30004 hat Identifikator für ein Exemplar
? EINGETRAGEN?  -> musenalm:eingetragen, interner Verarbeitungsvermerk
? GESUCHT?      -> musenalm:gesucht, interner Verarbeitungsvermerk
AUTOR           -> rdaw:P10065 hat geistige:n Schöpfer:in
HERAUSGEBER     -> rdaw:P10046 hat herausgebenden Akteur
TITEL           -> rdam:P30156 hat Haupttitel & rdaw:P10223 hat bevorzugten Titel des Werkes, bei Almanachen anders
UNTERTITEL      -> rdam:P30142 hat Titelzusatz
KURZTITEL       -> rdam:P30131 hat Kurztitel
ERSCHIENEN IN   -> ? Fällt weg, fast unbenutzt; Upload zu Dokumentationszwecken
ORT             -> rdam:P30088 hat Erscheinungsort evtl. Normdaten
JAHR            -> rdam:P30011 hat Erscheinungsdatum evtl. Normdaten
? NACHWEIS      -> rdaw:P10406 hat konsultierte Quelle s.u. ToDo
ZUGEHOERIG      -> Fällt weg (realisiert durch Sammlungen)
VORHANDENALS    -> Fällt weg; Upload zu Dokumentationszwecken
INHALT          -> rdaw:P10004 hat Kategorie eines Werkes
ZUSTAND         -> tssbiblio:zustand at Erhaltungszustand
NOTIZ AEUSS.    -> tssbiblio:anmzustand hat Anmerkung zum Erhaltungszustand
NOTIZ INHALT    -> rdai:P40011 hat Anmerkung zum Umfang eines Exemplars
ALLGEMEINE BEMERKUNG; -> rdam:P30137 hat Anmerkung zur Manifestation
STANDORT;       -> rdai:P40162 hat Ort eines Exemplars
? UNKLAR        -> Fällt weg


# Felder ToDo
Sucheinstiege?
FEI-Fingerprint
Gliederung in Normdaten, Titeldaten, Anmerkungen, Bearbeitungsvermerke, Tabellenfelder
rdaw:P10118 ist Werk beschrieben in vs rdam:P30254 "ist Manifestation beschrieben in"
rdaw:P10406 "hat konsultierte Quelle"

rdaa:P51112
	
"is person described with metadata by"

rdaa:P51110
	
"is corporate body described with metadata by"

	
rdaw:P10623
	
"is work described with metadata by" [@en; no @de]


# Import der Daten in eine neue Omeka-Installation
1. Entsprechende Module aktivieren
2. RDA-Ontologien importieren (über Menüpunkt Ontologien)
3. Eigene Ontologien importieren (über Menüpunkt Custom Ontology)
4. Eigenes individuelles Vokabular importieren
5. Resourcenvorlagen importieren, dabei
    5a. Datentypen von Listen anpassen
    5b. Filter von Nachschlagefeldern anpassen
6. Inverse Eigenschaften importieren (über Menüpunkt Module->Reciprocal)