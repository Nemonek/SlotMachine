
using System.Runtime.CompilerServices;

namespace SlotMachineLibrary;
public class SlotMachine
{
    private int _credito;
    private int _vincita;
    private int _rimanenti;
    private char[] _ultimoRoll;
    private bool _possoBloccareSlot;
    private Slot _slot1;
    private Slot _slot2;
    private Slot _slot3;
    private char[] _lettere = { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'Z' };
    private Random _r;   // impostata come campo poichè la continua creazione di istanze della classe Random rischia di fargli produrre lo stesso risultato

    /* PROPERTY */
    public int Credito { get => this._credito; }
    public int Vincita { get => this._vincita; }
    public int Rimanenti { get => this._rimanenti; }
    public char[] UltimoRoll { get => this._ultimoRoll; }
    public bool PossoBloccareSlot { get => this._possoBloccareSlot;  }

    // Chi usa la classe può vedere quali simboli sono usati dalla medesima per operare
    public char[] OttieniSimboli() => this._lettere;
    
    // Chi usa la classe può impostare lo stato degli slot a piacimento.
    // Controlla possibilità di errore: gli slot possono essere bloccati anche se il flag è = false
    public bool Slot1 { get => this._slot1.IsLocked; set => this._slot1.IsLocked = value; }
    public bool Slot2 { get => this._slot2.IsLocked; set => this._slot2.IsLocked = value; }
    public bool Slot3 { get => this._slot3.IsLocked; set => this._slot3.IsLocked = value; }


    public SlotMachine()
    {
        this._credito = 0;
        this._vincita = 0;
        this._rimanenti = 3;
        this._ultimoRoll = new char[3];
        this._possoBloccareSlot = false;
        this._slot1 = new();
        this._slot2 = new();
        this._slot3 = new();

        this._r = new Random();
    }
    /// <summary>
    /// Permette l'aggiunta di credito.
    /// </summary>
    /// <param name="n">Numero maggiore di 0 che rappresenta quanto credito verrà aggiunto.</param>
    /// <exception cref="ArgumentException">Sollevata quando viene passato come parametro un valore minore o uguale di 0.</exception>
    public void AggiungiCredito(int n)
    {
        if (n <= 0)
            throw new ArgumentException();

        this._credito += n;
    }
    /*
     Il player può fare 3 giri spendendo 1 credito:
     al primo non può decidere se tenere qualcosa, al 2 ed al 3 si; può anche rinunciare ai due giri.

        Se degli slot sono bloccati e può rollare, quindi il contatore è = 2 o a 1 rolla solo quelli sbloccati, se non ce ne sono sbloccati agisce autonomamente
    e li rirolla tutti.
     */

    /// <summary>
    /// Esegue un roll se possibile.
    /// </summary>
    /// <returns>char[3] contente il risultato del roll.</returns>
    /// <exception cref="InvalidOperationException">Se non c'è credito l'operazione è considerata invalida.</exception>
    public char[] Rolla() {

        // Controllare controllo: this._rimanenti == 0 potrebbe non servire 
        if ( this._rimanenti == 3 || this._rimanenti == 0 ) {
            if (this._credito == 0)
                throw new InvalidOperationException();
            this._rimanenti--;
            this._credito--;
            this._possoBloccareSlot = true;

            char[] retVal = new char[] {
                this._lettere.GetRandomElement(this._r),
                this._lettere.GetRandomElement(this._r),
                this._lettere.GetRandomElement(this._r)
            };
            this._ultimoRoll = retVal;

            return retVal;
        }
        else {

            this._rimanenti--;
            
            char[] retVal = new char[] {
                (this._slot1.IsLocked) ? this._ultimoRoll[0] :  this._lettere.GetRandomElement(this._r),
                (this._slot2.IsLocked) ? this._ultimoRoll[1] :  this._lettere.GetRandomElement(this._r),
                (this._slot3.IsLocked) ? this._ultimoRoll[2] :  this._lettere.GetRandomElement(this._r)
            };

            this._ultimoRoll = retVal;

            this._slot1.IsLocked = false;
            this._slot2.IsLocked = false;
            this._slot3.IsLocked = false;

            // this._rimanenti è a 0, quindi questo è l'ultimo roll disponibile: l'utente non può non ottenere il premio (se c'è un premio)
            if (this._rimanenti == 0)
            {
                this._rimanenti = 3;
                this._vincita += DeterminaPremio();
                this._possoBloccareSlot = false;
            }

            return retVal;
        }


    }
    private int DeterminaPremio()
    {
        int check = new HashSet<char>(this._ultimoRoll).Count;
        if (check == 1) return this._lettere.IndexOf(this._ultimoRoll[0]);

        if (check == 2) return 1;

        else
        { // check = 3
            int n = this._lettere.IndexOf(this._ultimoRoll[0]);

            if (this._ultimoRoll[0] == 'z' && this._ultimoRoll[1] == 'z' && this._ultimoRoll[2] == 'z') return 100;

            // PROBLEMA: IN CASO DI CAMBIO DELLA LUNGHEZZA DELL'ARRAY DI LETTERE IL PROGRAMMA CRASHA SE NON SI CAMBIA ANCHE QUA
            if (n + 2 >= 20) return 0;

            if (this._lettere[n + 1] == this._ultimoRoll[1] && this._lettere[n + 2] == this._ultimoRoll[2]) return 50;
            return 0;
        }
    }
/// <summary>
/// Notifica la classe che l'utente ha rinunciato ad uno o due dei suoi tentativi.
/// </summary>
/// <exception cref="InvalidOperationException">Se chiamato quando l'utente ha 3 tentativi l'operazione è considerata invalida.</exception>
    public void NotificaRinuncia() {
        if (this._rimanenti == 3) throw new InvalidOperationException();
        this._vincita += DeterminaPremio();
        this._rimanenti = 3;
        this._possoBloccareSlot = false;
    }

}

public static class ArrayHelper {

    public static T GetRandomElement<T>(this T[] array, Random r) {
        return array[r.Next(0, array.Length)];
    }

    public static int IndexOf(this char[] input, char c)
    {
        int l = input.Length;
        for (int i = 0; i < l; i++)
            if (c == input[i]) return i;

        return -1;
    }

}
