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

## Premesse
Con la parola 'roll' si intende un giro delle rotelle della slot machine.<br>
In questa documentazione quando si definisce come un utilizzatore debba interfacciarsi con la classe e vengono nominate delle funzioni queste vengono nominate secondo notazione UML.
## Dipendenze della classe SlotMachine
## Classe Slot
Ogni istanza della classe Slot rappresenta uno degli slot della slot machine ( nel nostro caso 3 ).
### Attributi
```C#
private bool _isLocked;
public bool IsLocked { get => _isLocked; internal set => _isLocked = value; }
```
Definisce se lo slot è bloccato oppure no ( per maggiori informazioni fare riferimento alla documentazione della classe 'SlotMachine' ).<br>
Questo attributo è modificabile direttamente solo dall'assembly di appartenenza ( vedi sezione finale 'specifiche finali' per maggiori informazioni ).
## Costruttore
```C#
public Slot()
```
Costruttore di default: inizializza il campo '_isLockes' a false;
## La classe SlotMachine
### Attributi
```C#
private int _credito;
public int Credito { get => this._credito; }
```
Credito inserito dall'utente.<br>
Il campo non è modificabile da codice all'infuori della classe.
```C#
private int _vincita;
public int Vincita { get => this._vincita; }
```
Vincita totale accumulata dall'utente: una volta ritirata viene aggiunta al credito dell'utente.<br>
Il campo non è modificabile da codice all'infuori della classe.
```C#
private int _rimanenti;
public int Rimanenti { get => this._rimanenti; }
```
Contatore dei tentativi rimanenti ( vedi comportamento specificato nella sezione 'Requisiti dei compito' ); non puà superare il valore 3 ( caratteristica dovuta all'implementazione della classe ).<br>
Il campo non è modificabile da codice all'infuori della classe.
```C#
private char[] _ultimoRoll;
public char[] UltimoRoll { get => this._ultimoRoll; }
```
Array contenente il risultato dell'ultimo roll.<br>
Il campo non è modificabile da codice all'infuori della classe.
```C#
private bool _possoBloccareSlot;
public bool PossoBloccareSlot { get => this._possoBloccareSlot;  }
```
Valore booleano il cui scopo è permettere all'utilizzatore della classe di sapere se nel periodo che intercorre tra un roll ed un altro si possono bloccare degli slot.<br>
Nel caso in cui l'utilizzatore provi a bloccare degli slot quando questo flag è impostato su falso la classe solleva un 'InvalidOperationException'.<br>
Il campo non è modificabile da codice all'infuori della classe.
```C#
private Slot _slot1;
private Slot _slot2;
private Slot _slot3;

public bool Slot1 { get => this._slot1.IsLocked; set => this._slot1.IsLocked = value; }
public bool Slot2 { get => this._slot2.IsLocked; set => this._slot2.IsLocked = value; }
public bool Slot3 { get => this._slot3.IsLocked; set => this._slot3.IsLocked = value; }
```
Campi privati di tipo 'Slot' ( vedi sezione 'dipendenze' ): all'utilizzatore è permesso di modificarli e di verificare il loro valore.<br>
Seppur ogni slot metta a disposizione una property pubblica per vedere se è bloccato la classe SlotMachine non espone puntatori alle sue istanze di Slot: durante la fase di implementazione non si è rivelato necessario farlo.
```C#
private char[] _lettere = { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'Z' };
```
Lettere usate dalla classe per lavorare. Rimane a discrezione dell'utilizzatore se aggiungere ( nel proprio codice ) un metodo che associa ad ogni lettera un altro simbolo per poi usare quelli.<br>
Questo campo è accessibile in sola lettura solo con il metodo pubblico 'OttieniSimboli' ( vedi sezione metodi ).
```C#
private Random _r;
```
Campo privato e non accessibile a codice esterno alla classe: questa variabile è usata per rendere randomici i roll: di norma non sarebbe necessario impostarla come campo, tuttavia la continua generazione di istanze della classe 'Random' ad ogni roll è un peso inutile per il programma, inoltre nel caso di una generazione troppo rapida di variabili di questo tipo c'è il rischio che due ritornino lo stesso valore. 
### Costruttore
```C#
public SlotMachine()
```
Costruttore di default, viene chiamato dal programma per inizializzare i campi della classe.<br>
Il valore del campo '_possoBloccareSlot' è inizializzato a false.

### Uso del costruttore
Per usare il costruttore di default basta usare la sintassi più classica
```C#
SlotMachine machine = new SlotMachine();
```
Oppure si può usare la sintassi in vigore da C# 9 chiamata Target-typed new: con questa sintassi il compilatore risale automaticamente al tipo dichiarato per la variabile, e ne evita la ripetizione nella chiamata al costruttore.
```C#
SlotMachine machine = new();
```

### Property
Per le property dei vari campi controllare la sezione del campo.

## Interfacciarsi con la classe SlotMachine
### Premesse
La classe mette a disposizione dei modesti e riassuntivi commenti XML di documentazione per i metodi esposti ( ove necessario ).<br>
In questa sezione ci si potrebbe riferire ai tentativi come 'se si hanno 3 tentativi", "se si sono appena finiti i tentativi": tenere a mente durante la lettura che quando si sono appena finiti i tentativi questi vengono riportati a 3, e quando si hanno 3 tentativi significa che sono appena finiti poiché l'unico modo per averne 3 è rinunciare a quelli rimanenti o finirli.
### Aggiunta di credito
L'aggiunta del credito è eseguibile dall'esterno tramite la funzione '+ AggiungiCredito(int) : void': l'utilizzatore è incaricato di fornire un valore maggiore di 0, in caso contrario viene sollevata un'eccezione di tipo 'ArgumentException'.
### Esecuzione di un roll
Per eseguire un roll l'utilizzatore può effettuare una chiamata alla funzione '+ Rolla() : char[]': questa ritornerà un array di caratteri con lunghezza 3 contenente il risultato del roll appena fatto ( quel valore sarà disponibile nella property '+ UltimoRoll : char[]' fino al roll successivo ).<br>
Il metodo appena descritto verrà correttamente eseguito solo se al momento della chiamata è disponibile del credito o la chiamata è compresa in uno dei 2 tentativi rimanenti dopo un primo roll: in caso non lo sia l'operazione sarà considerata invalida ed un eccezione di tipo 'InvalidOperationException' verrà sollevata.
### Accettazione del corrente risultato
Nel caso in cui l'utente voglia rinunciare ad 1 o 2 dei tentativi che ha a disposizione dopo il roll iniziale può farlo, ma affinché la rinuncia vada a buon fine bisogna chiamare questo metodo, in modo che la classe ne sia notificata e possa aggiornarsi ( oltre che determinare un'eventuale premio ).<br>
Questo metodo non deve essere chiamato quando sono appena finiti i tentativi: in caso venga chiamato l'operazione sarà considerata invalida ed un eccezione di tipo 'InvalidOperationException' verrà sollevata.
### Bloccare 1 o più slot
Per bloccare uno o più slot è possible usare le rispettive property ( vedi sezione attributi classe ).<br>
Nel caso in cui gli slot vengano bloccati quando non possono essere bloccati la classe ignorerà il loro blocco.

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