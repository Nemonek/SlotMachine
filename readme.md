# Piredda.Riccardo.4i.rubricaWPF

## Requisiti del compito
* Creazione di una libreria che implementi la classe 'SlotMachine' con il seguente comportamento:
  * ogni moneta inserita dà diritto ad una partita nella quale si girano le 3 rotelle della slot facendo apparire 3 simboli ( la classe dovrà lavorare usando le lettere dell'alfabeto italiano ).
  * all'utente devono essere forniti 3 tentativi per far girare le rotelle della slot: 
    * il primo costa 1 credito;
    * i successivi due sono gratuiti, e l'utente deve poter decidere se tenere uno o più simboli di quelli ottenuti dopo il primo;
    * l'utente non deve essere obbligato ad usare tutti e 3 i tentativi: deve essere in grado di accettare il risultato del primo o del secondo ( il risultato del terzo giro deve essere per forza accettato --> l'utente a quel punto non ha scelta );
    * se l'utente accetta il primo o il secondo risultato sta rinunciando ai tentativi rimanenti senza possibilità di tornare indietro.
  * usare i seguenti criteri per stabilire un'eventuale vincita:
    * per una coppia l'utente vince 1 moneta;
    * per un tris di lettere uguali ( es: | L | L | L | vincita di 10 monete) l'utente vince un numero di monete pari alla posizione in ordine alfabetico della lettera;
    * se ci sono tre Z si tratta di jackpot e l'utente vince 100 monete.
* creazione di un programma console che faccia uso della classe 'SlotMachine';
* creazione di un programma WPF ( Windows Presentation Foundation ) che faccia uso della classe.

## Linee guida generali adottate
* Una libreria deve essere completamente indipendente dal programma che la andrà ad usare: essa **non** deve interfacciarsi con l'utilizzatore, ma deve fornire all'utilizzatore i metodi perché lui sia in grado di interfacciarsi con la classe. In parole brevi: **non** è accettabile che la classe dipenda dall'utilizzatore.

## Lo standard Microsoft per la nomenclatura dei campi privati di una classe
Secondo la convenzione diffusa ed usata da Microsoft i nomi dei campi privati di una classe devono iniziare con il trattino basso (underscore _) ed il nome effettivo del campo deve iniziare con una lettera minuscola (lower case).
Per quanto riguarda le proprietà associate ai vari campi, queste devono iniziare con lettera maiuscola (upper case).

## Accortezze in fase di compilazione
1) Per non dover copiare a mano l'immagine usata nel programma WPF nella directory di output del file eseguibile inserire nel file NomeProgetto.csproj le seguenti righe di codice:
    ```XML
    <ItemGroup>
      <None Update="Media\Assets.png">
        <CopyToOutputDirectory>Always</CopyToOutputDirectory>
      </None>
    </ItemGroup>
    ```
    Questo permetterà al compilatore di capire l'azione che deve eseguire sul file in fase di compilazione.
2) Ad ogni modifica del file CSV il programma ha bisogno di essere ricompilato, in modo che la nuova versione del file sia copiata nella cartella dell'eseguibile.