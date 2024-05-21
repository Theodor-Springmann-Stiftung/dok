# 3. Verleger und Orte
## Angaben in `AlmNeu/ORT`
<div class="task-list-right"></div>
- [x] abgeschlossen
??? bug "Problem: Unstrukturierte Angaben in `AlmNeu/ORT`"
    Das Feld  `AlmNeu/ORT` enthält zzt. meist den (Erscheinungs?)ort der Publikation, außerdem den Verlag in Klammern, manchmal auch der Vertrieb an anderem Ort. Mehrere Werte sind möglich, meist mit `;` getrennt. Eine vollständige bibliografische Angabe erfordert aber das saubere Trennen beider Angaben vgl. [--> 2.8.2 Erscheinungsort & 2.8.4 Verlagsname im Standardelemente-Set der DNB](https://wiki.dnb.de/pages/viewpage.action?pageId=114430616&preview=/114430616/112493888/Standardelemente-Set_Titeldaten.pdf)

    --> s.a. [:material-file-document:&nbsp;Feldwerte](../files/feldwerte/AlmNeu_ORT.txt)

Es muss noch erhoben werden, wie gut sich Verleger und Ort maschinell separieren lassen. Ansonsten muss in der Überarbeitungsversion der DB, `alma\Datenbank\Ueberarbeitungsdatenbank_Aktuelle_Bearbeitung.accdb`, (>Access 2016) die so angelegt ist, dass 

1. n:m-Beziehungen zwischen Bänden und Personen/Organisationen möglich sind und 
2. sich in der (bisherigen) Personentabelle `Akteure` auch Organisationen erfassen lassen, 

die Verleger aus dem Feld Ort ausgeschnitten und an anderer Stelle dem Band zugewiesen werden. Das Verältnis zwischen Bänden und Akteuren kann in Beziehungstripeln nach dem Schema Subjekt-Prädikat-Objekt erfasst werden:

    <Band> wurde geschaffen von <Akteur>
    <Band> wurde geschrieben von <Akteur>
    <Band> wurde gezeichnet von <Akteur>
    <Band> wurde gestochen von <Akteur>
    <Band> wurde herausgegeben von <Akteur>
    <Band> wurde verlegt von <Akteur>
    <Band> wurde gedruckt von <Akteur>
    <Band> wurde vertrieben von <Akteur>

Aus
    
    AlmNeu NUMMER 2, Feld ORT: Zürich u. Leipzig (Füßli u. Sohn); (Comm. J. B. Schiegg)

wird somit

    Baende NUMMER 2, Akteur(e): 
    (Band) Alruna 1807 wurde verlegt von Füßli u. Sohn
    (Band) Alruna 1807 wurde vertrieben von J. B. Schiegg

    Baende NUMEMR 2, ORT:
    Zürich; Leipzig

Eventuell müssen hierfür Akteure in der Tabelle `Akteure` angelegt werden mit dem Feld `Akteure\ORGANISATION` gesetzt, handelt es sich um einen Verlag.
