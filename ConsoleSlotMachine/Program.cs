using SlotMachineLibrary;

namespace ConsoleSlotMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SlotMachine machine = new();
            Console.WriteLine("Benvenuto alla slot machine!");
            bool flag = true;
            string input = "";
            while (flag) {
                Console.WriteLine("La preghiamo di selezionare un operazione da fare:");
                Console.WriteLine("1) Aggiungere credito");
                Console.WriteLine("2) Esegui un roll");
                Console.WriteLine("e) Exit");
                ScriviInGialloNoCapo("Input: ");
                input = Console.ReadLine()!;

                switch (input)
                {
                    case "1":
                        Console.Write("Inserire un numero maggiore di 0: ");
                        if (!int.TryParse(Console.ReadLine(), out int res) && res <= 0)
                            ScriviInRosso("Errore: input invalido; riprovare!");

                        else {
                            machine.AggiungiCredito(res);
                            ScriviInVerde($"Credito aggiornato: {machine.Credito}");
                        }
                        Attendi();
                        break;

                    case "2":
                        if (machine.Credito == 0) {
                            ScriviInRosso($"Attenzione: il suo credito è insufficiente. Credito: {machine.Credito}");
                            Attendi();
                            break;

                        }
                        char[] risultato = machine.Rolla();
                        ScriviInVerde($"Il risultato del roll è: |{risultato[0]} - {risultato[1]} - {risultato[2]}|");
                        Console.Write($"Lo vuole tenere o vuole riprovare? Le rimangono {machine.Rimanenti}/3. Digiti S/s per tenere il corrente risultato, N/n per riprovare.");
                        string i = Console.ReadLine()!.ToLower();

                        while ((i != "s" && i != "n") || i.Length > 1) {
                            ScriviInRosso("Input Invalido: riprovare!");
                            i = Console.ReadLine()!.ToLower();
                        }
                        if (i == "s") {
                            machine.NotificaRinuncia();
                            ScriviInVerde($"Ha appena accettato il risultato |{risultato[0]} - {risultato[1]} - {risultato[2]}|.");
                            Console.WriteLine("Eventuali vincite sono state registrate:");
                            Console.WriteLine($"Credito attuale: {machine.Credito}");
                            Console.WriteLine($"Vincita attuale: {machine.Vincita}");
                            Attendi();
                        }
                        else {
                            string i2 = "";
                            ScriviInVerde("Ha deciso di riprovare:");
                            Console.WriteLine("Ha la possibilità di tenere uno, due, tutti o nessuno del risultato corrente.");
                            Console.WriteLine("Digiti un numero da 1 a 3 per tenere uno slot (Attenzione: le verrà chiesta conferma della selezione e potrà decidere di tenere più slot.)");
                            Console.WriteLine("Digiti 0 per non tenere nessuno slot.");

                            bool flag2 = true;

                            while( flag2 ) {
                                i2 = Console.ReadLine()!.ToLower();
                                while( i2 != "0" && i2 != "1" && i2 != "2" && i2 != "3" ) {
                                    ScriviInRosso("Errore: input non valido. Se non vuole tenere il risultato di nessuno slot digiti 0.");
                                }
                            } 
                        }
                        break;

                    case "e":
                        flag = false;
                        ScriviInRosso("Arrivederci!");
                        break;

                    default:
                        ScriviInRosso("Comando invalido!");
                        break;
                }
                Console.Clear();
            }
        }

        static void ScriviInRosso(string s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        static void ScriviInVerde(string s)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(s);
            Console.ResetColor();
        }
        static void ScriviInGiallo(string s)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(s);
            Console.ResetColor();
        }
        static void ScriviInGialloNoCapo(string s)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(s);
            Console.ResetColor();
        }
        static void Attendi()
        {
            ScriviInGiallo("Premere un qualsiasi tasto per continuare.");
            Console.ReadKey();
        }
    }
}