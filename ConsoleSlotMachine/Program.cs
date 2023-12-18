using SlotMachineLibrary;

// Problema nella libreria: il punteggio del round n viene accreditato solo al round n+1

SlotMachine m = new();
string input;
Console.WriteLine("Benvenuto!");
while (true)
{
    Console.WriteLine("Seleziona l'operazione da fare:");
    Console.WriteLine("1) Aggiungere monete");
    Console.WriteLine("2) Rollare");
    Console.WriteLine("3) Verificare credito");
    Console.WriteLine("4) Esci");
    input = Console.ReadLine()!;
    if (input == "4") break;     // C'è bisogno di questo controllo poichè break nello switch fa uscire dal singolo caso.
    switch(input)
    {
        case "1":
            AggiungiMonete();
            break;

        case "2":
            rolla();
            break;

        case "3":
            Console.WriteLine($"Credito attuale: {m.SaldoGiocatore}.");
            break;

        default:
            Console.WriteLine("Errore: comando non riconosciuto.");
            break;
    }
}

void rolla()
{
    if(m.SaldoGiocatore == 0)
    {
        Console.WriteLine("Errore: il saldo è vuoto! Inserire dei crediti prima di giocare.");
        return;
    }
    Console.WriteLine($"Saldo giocatore: {m.SaldoGiocatore}");
    char[] roll = m.EseguiRoll();
    string tieni;
    Console.WriteLine($"Risultato del roll: |{roll[0]} - {roll[1]} - {roll[2]}|");

    Console.WriteLine("Lei ha ora la possibilità di tenere una o più lettere tra quelle uscite.");
    Console.WriteLine("Per tenere delle lettere digiti il loro numero: la prima è 1, la seconda è 2 ecc; per tenere più lettere separi i numeri con uno spazio, per uscire digiti a.");
    
    while (m.Counter < 2)
    {
        tieni = Console.ReadLine()!;
        int[] par = { -1, -1, -1 };
        if (tieni == "a") return;
        string[] s = tieni.Split(' ');
        if(s.Length > 3)
        {
            Console.WriteLine("Input invalido: possibilità persa!");
            return;
        }
        foreach(string str in s)
        {
            if(int.TryParse(str, out int res) && res-1 < 3 && res-1 >= 0)
            {
                par[res-1] = res-1;
            }
            else
            {
                Console.WriteLine("Input invalido: possibilità persa!");
                return;
            }
        }
        roll = m.EseguiRollMantenendo(par);
        Console.WriteLine($"Risultato del roll: |{roll[0]} - {roll[1]} - {roll[2]}|");
        par[0] = -1;
        par[1] = -1;
        par[2] = -1;
    }
    Console.WriteLine($"Saldo giocatore: {m.SaldoGiocatore}");

}

void AggiungiMonete()
{
    Console.WriteLine($"Credito attuale: {m.SaldoGiocatore}");
    Console.Write("Quanto credito vuole aggiungere? inserisca un numero: ");
    string n = Console.ReadLine()!;
    while(!int.TryParse(n, out int res) || res <= 0)
    {
        if (n == "a")
            return;
        Console.WriteLine("Errore: inserire un numero valido o digitare 'a' per annullare");
        n = Console.ReadLine()!;
    }

    // A questo punto sappiamo già che il numero è valido
    m.AggiungiCredito(int.Parse(n));
}