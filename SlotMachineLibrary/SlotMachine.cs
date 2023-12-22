
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
    private char[] _lettere = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'Z' };
    private Random _r;   // impostata come campo poichè la continua creazione di istanze della classe Random rischia di fargli produrre lo stesso risultato

    public int Credito { get => this._credito; }
    public int Vincita { get => this._vincita; }
    public int Rimanenti { get => this._rimanenti; }
    public char[] UltimoRoll { get => this._ultimoRoll; }
    public bool PossoBloccareSlot { get => this._possoBloccareSlot;  }

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


    public char[] Rolla() {

        if (this._credito == 0)
            throw new InvalidOperationException();

        if ( this._rimanenti == 3 || this._rimanenti == 0 ) {

            if (this._rimanenti == 0) {
                this._rimanenti = 3;
            }
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
            // this._rimanenti andrà a 0, quindi questo è l'ultimo roll disponibile: l'utente non può non ottenere il premio (se c'è un premio)
            if (this._rimanenti == 1)
                this._vincita += DeterminaPremio();

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

            if (n + 2 > 21) return 0;

            if (this._lettere[n + 1] == this._ultimoRoll[1] && this._lettere[n + 2] == this._ultimoRoll[2]) return 50;
            return 0;
        }
    }

    public void NotificaRinuncia() {
        this._vincita += DeterminaPremio();
        this._rimanenti = 3;
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
