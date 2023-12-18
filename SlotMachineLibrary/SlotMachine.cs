
using System.Runtime.CompilerServices;

namespace SlotMachineLibrary;
public class SlotMachine
{
    private char[] _lettere = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'Z' };

    private int _saldoGiocatore;
    private int _ultimoRisultato;
    public int UltimoRisultato { get => this._ultimoRisultato; }

    public int SaldoGiocatore { get => this._saldoGiocatore; private set => this._saldoGiocatore = value; }

    private char[] _ultimoRoll;
    public char[] UltimoRoll { get => this._ultimoRoll; private set => this._ultimoRoll = value; }

    private int _counter;
    public int Counter { get => this._counter;  }
    public SlotMachine() {
        this._saldoGiocatore = 0;
        this._counter = 2;
        this._ultimoRisultato = 0;
    }

    public void AggiungiCredito(int n)
    {
        if (n <= 0) throw new();

        this._saldoGiocatore += n;
    }

    public char[] EseguiRollMantenendo(int[] input)
    {
        if (this._saldoGiocatore == 0) throw new();

        Random r = new();

        this._ultimoRoll[0] = (input[0] == -1) ? this._lettere[r.Next(0, 21)] : this._ultimoRoll[0];
        this._ultimoRoll[1] = (input[1] == -1) ? this._lettere[r.Next(0, 21)] : this._ultimoRoll[1];
        this._ultimoRoll[2] = (input[2] == -1) ? this._lettere[r.Next(0, 21)] : this._ultimoRoll[2];

        this._counter++;

        return this._ultimoRoll;
    }

    public char[] EseguiRoll()
    {
        if (this._saldoGiocatore == 0) throw new();
        this._saldoGiocatore--;
        this._counter = 0;
        Random r = new();
        char[] roll = { this._lettere[r.Next(0, 21)], this._lettere[r.Next(0, 21)], this._lettere[r.Next(0, 21)] };
        this._ultimoRoll = roll;
        int ultimo = DeterminaPremio();
        this._ultimoRisultato = ultimo;
        this._saldoGiocatore+= ultimo;

        return this._ultimoRoll;
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
}

public static class ArrayHelper
{
    public static int IndexOf(this char[] input, char c)
    {
        int l = input.Length;
        for (int i = 0; i < l; i++)
            if (c == input[i]) return i;

        return -1;
    }
}
