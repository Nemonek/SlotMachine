using SlotMachineLibrary;

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
    char[] roll = m.EseguiRoll();
    Console.WriteLine($"Risultato del roll: |{roll[0]} - {roll[1]} - {roll[2]}|");
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