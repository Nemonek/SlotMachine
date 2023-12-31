﻿using SlotMachineLibrary;
using System.Reflection.Emit;

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
                Console.WriteLine("3) Verifica credito");
                Console.WriteLine("4) Verifica vincita");
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
                        // Credito insufficiente
                        if (machine.Credito == 0) {
                            ScriviInRosso($"Attenzione: il suo credito è insufficiente. Credito: {machine.Credito}");
                            Attendi();
                            break;
                        }

                        LabelEstrazione:
                        char[] risultato = machine.Rolla();
                        ScriviInVerde($"Il risultato del roll è: |{risultato[0]} - {risultato[1]} - {risultato[2]}|");

                        // Questo controllo funziona grazie ad un "artificio" particolare:
                        // quando si raggiunge l'ultimo tentativo a disposizione e si decide di usarlo si viene rimandati al label soprastante ( LabelEstrazione ): in quel punto, poichè quello è l'ultimo tentativo
                        // disponibile il contatore della classe vale 1, poi, quando verrà chiamata la funzione Rolla() questo raggiungerà il valore 0, e sarà subito rimesso a 3, poichè la classe non può permettersi
                        // di rimanere indietro nei conteggi per favorire l'UI ( se il valore 0 venisse portato a 3 non al giro in cui si raggiunge 0, ma al giro successivo, basterebbe, per l'UI controllare quando
                        // ci si trova di fronte ad uno 0, tuttavia, non è accettabile che la classe dipenda dall'interazione con l'UI per operare ).
                        // In conclusione, ad ogni singolo ciclo
                        if (machine.Rimanenti == 3)
                        {
                            ScriviInRosso("Tentativi esauriti.");
                            Console.WriteLine("Eventuali vincite sono state registrate:");
                            Console.WriteLine($"Credito attuale: {machine.Credito}");
                            Console.WriteLine($"Vincita attuale: {machine.Vincita}");
                            Attendi();
                            Console.Clear();
                            break;
                        }
                        
                        Console.Write($"Lo vuole tenere o vuole riprovare? Le rimangono {machine.Rimanenti}/3 tentativi. Digiti S/s per riprovare, N/n per accettare il corrente risultato.");

                        string i = Console.ReadLine()!.ToLower();

                        // Input invalido
                        while ((i != "s" && i != "n") || i.Length > 1) {
                            ScriviInRosso("Input Invalido: riprovare!");
                            i = Console.ReadLine()!.ToLower();
                        }
                        // Rinuncia a tenere le lettere e accetta questo risultato
                        if (i == "n") {
                            machine.NotificaRinuncia();
                            ScriviInVerde($"Ha appena accettato il risultato |{risultato[0]} - {risultato[1]} - {risultato[2]}|.");
                            Console.WriteLine("Eventuali vincite sono state registrate:");
                            Console.WriteLine($"Credito attuale: {machine.Credito}");
                            Console.WriteLine($"Vincita attuale: {machine.Vincita}");
                            Attendi();
                        }
                        // Vuole riprovare
                        else {
                            string i2 = "";
                            ScriviInVerde("Ha deciso di riprovare:");
                            Console.WriteLine("Ha la possibilità di tenere uno, due, tutti o nessuno del risultato corrente.");
                            Console.WriteLine("Digiti un numero da 1 a 3 per tenere uno slot. Per tenere più slot digiti i numeri separati da uno spazio.");
                            Console.WriteLine("Digiti 0 per non tenere nessuno slot.");
                            i2 = Console.ReadLine()!;

                            // Se l'input è invalido si continua a chiederlo
                            // A livello di efficenza sarebbe meglio che la funzione di verifica della validita modificasse un array con i valori che trova:
                            // quando ne trova uno invalido continua a ciclare all'infinito, mentre, quando trova un input valido ed il programma procede potrebbe
                            // accedere direttamente all'array senza ripetere le operazioni di parse e lo split.
                            // Purtroppo questo non si può fare pk violerebbe il principio di separazione dei compiti: la funzione o controlla l'input, o lo elabora ed estrapola i dati.
                            while(InputInvalido(i2))
                            {
                                ScriviInRosso("Errore: input invalido!");
                                i2 = Console.ReadLine()!;
                            }

                            string[] strings = i2.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            int tmp;
                            foreach (string s in strings)
                            {
                                tmp = int.Parse(s);
                                if (tmp == 1)
                                    machine.Slot1 = true;
                                else if (tmp == 2)
                                    machine.Slot2 = true;
                                else 
                                    machine.Slot3 = true;
                            }

                            goto LabelEstrazione;

                        }
                        break;

                    case "3":
                        Console.WriteLine($"Credito: {machine.Credito}");
                        Attendi();
                        break;

                    case "4":
                        Console.WriteLine($"Vincita attuale: {machine.Vincita}");
                        Attendi();
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

        static bool InputInvalido(string input)
        {
            string[] s = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in s)
                // Se l'input non è convertibile è invalido.
                if (!int.TryParse(str, out int n))
                    return true;
                // Se l'input viene convertito a intero deve rientrare in un range specifico.
                else if (n - 1 < -1 || n - 1 > 3)
                    return true;
        
            return false;

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