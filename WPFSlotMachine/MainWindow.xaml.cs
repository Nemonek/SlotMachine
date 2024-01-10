using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using SlotMachineLibrary;

namespace WPFSlotMachine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SlotMachine _machine;

        private DispatcherTimer _inputInvalido;
        private Dictionary<char, BitmapSource> _associazioneConSimboliClasse;

        public MainWindow() {
            InitializeComponent();

            this._machine = new();
            this.InizializzaImmagini();
            this._inputInvalido = new() {
                Interval = TimeSpan.FromSeconds(5)
            };
            
            this._inputInvalido.Tick += InputInvalido_RipristinoColoreTextBox;

            BottoneTieni1.IsEnabled = this._machine.PossoBloccareSlot;
            BottoneTieni2.IsEnabled = this._machine.PossoBloccareSlot;
            BottoneTieni3.IsEnabled = this._machine.PossoBloccareSlot;
            PulsanteRinuncia.IsEnabled = this._machine.PossoBloccareSlot;

            GiriRimanenti.Text = $"{this._machine.Rimanenti}";
        }

        private void InizializzaImmagini() {
            this._associazioneConSimboliClasse = new();
            char[] simboliUsatiDallaClasse = this._machine.OttieniSimboli();
            BitmapImage b = new(new Uri(@"./Media/Assets.jpg", UriKind.Relative));
            int larghezzaTotale = b.PixelWidth;
            int altezzaTotale = b.PixelHeight;

            // Il programma si aspetta un file con 4 file da 5 carte.
            int larghezzaCarta = larghezzaTotale / 5;
            int altezzaCarta = altezzaTotale / 4; 

            CroppedBitmap tmp;
            int counter = 0;
            for(int i  = 0; i < 5; i++) {
                for (int j = 0; j < 4; j++)
                {
                    tmp = new(b, new Int32Rect(( larghezzaCarta * i ), ( altezzaCarta * j ), larghezzaCarta, altezzaCarta));
                    this._associazioneConSimboliClasse.Add(simboliUsatiDallaClasse[counter], BitmapFrame.Create(tmp));
                    counter++;
                }
            }
            MostraSlot1.Source = this._associazioneConSimboliClasse['A'];
            MostraSlot2.Source = this._associazioneConSimboliClasse['A'];
            MostraSlot3.Source = this._associazioneConSimboliClasse['A'];

        }

        private void AggiungiCredito_Evento(object sender, RoutedEventArgs e) {
            int.TryParse(QuantoCreditoAggiungere.Text, out int res);
            
            // Se il numero non viene convertito in 'res' si trova -1
            if ( res <= 0 ) {
                QuantoCreditoAggiungere.BorderBrush = Brushes.Red;
                AggiungiCredito_InputInvalidoTextBlock.Visibility = Visibility.Visible;

                if (this._inputInvalido.IsEnabled)
                    this._inputInvalido.Stop();

                this._inputInvalido.Start();
            }
            else {
                this._machine.AggiungiCredito(res);
                AggiornaVisualizzazioneCredito();
            }

            QuantoCreditoAggiungere.Text = "";
        }

        private void AggiungiCredito() {
            int.TryParse(QuantoCreditoAggiungere.Text, out int res);

            // Se il numero non viene convertito in 'res' si trova -1
            if (res <= 0)
            {
                QuantoCreditoAggiungere.BorderBrush = Brushes.Red;
                AggiungiCredito_InputInvalidoTextBlock.Visibility = Visibility.Visible;

                if (this._inputInvalido.IsEnabled)
                    this._inputInvalido.Stop();

                this._inputInvalido.Start();
            }
            else
            {
                // Nel caso l'utente invii entro 5 secondi un valore valido la casella deve tornare normale non appena preme invio o il bottone // strettamente estetico \\
                if(this._inputInvalido.IsEnabled)
                {
                    this._inputInvalido.Stop();
                    InputInvalido_RipristinoColoreTextBox(null, EventArgs.Empty);
                }
                this._machine.AggiungiCredito(res);
                AggiornaVisualizzazioneCredito();
            }

            QuantoCreditoAggiungere.Text = "";
        }

        private void InputInvalido_RipristinoColoreTextBox(object? sender, EventArgs e) {
            QuantoCreditoAggiungere.BorderBrush = Brushes.LightGray;
            AggiungiCredito_InputInvalidoTextBlock.Visibility = Visibility.Collapsed;
        }
        private void AggiornaVisualizzazioneCredito()
        {
            Credito.Text = this._machine.Credito.ToString();
        }
        private void RollaLettere(object sender, RoutedEventArgs e)
        {
            if (this._machine.Credito == 0) {
                MessageBox.Show("Credito insufficiente: impossibile procedre con l'operazione");
                return;
            }
            char[] ret = this._machine.Rolla();
            //PrimaLettera.Text = ret[0].ToString();
            MostraSlot1.Source = this._associazioneConSimboliClasse[ret[0]];
            MostraSlot2.Source = this._associazioneConSimboliClasse[ret[1]];
            MostraSlot3.Source = this._associazioneConSimboliClasse[ret[2]];

            BottoneTieni1.IsEnabled = this._machine.PossoBloccareSlot;
            BottoneTieni2.IsEnabled = this._machine.PossoBloccareSlot;
            BottoneTieni3.IsEnabled = this._machine.PossoBloccareSlot;
            PulsanteRinuncia.IsEnabled = this._machine.PossoBloccareSlot;

            BottoneTieni1.Content = "Blocca";
            BottoneTieni2.Content = "Blocca";
            BottoneTieni3.Content = "Blocca";
            
            GiriRimanenti.Text = this._machine.Rimanenti.ToString();
            AggiornaVisualizzazioneCredito();
        }

        private void TieniUno(object sender, RoutedEventArgs e)
        {
            this._machine.Slot1 = true;
            ((Button)sender).Content = "Sblocca";
        }
        private void TieniDue(object sender, RoutedEventArgs e)
        {
            this._machine.Slot2 = true;
            ((Button)sender).Content = "Sblocca";
        }
        private void TieniTre(object sender, RoutedEventArgs e)
        {
            this._machine.Slot3 = true;
            ((Button)sender).Content = "Sblocca";
        }

        private void QuantoCreditoAggiungere_EnterPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                AggiungiCredito();
        }

        private void AccettaRisultatoEvento(object sender, RoutedEventArgs e)
        {
            this._machine.NotificaRinuncia();

            BottoneTieni1.IsEnabled = this._machine.PossoBloccareSlot;
            BottoneTieni2.IsEnabled = this._machine.PossoBloccareSlot;
            BottoneTieni3.IsEnabled = this._machine.PossoBloccareSlot;
            PulsanteRinuncia.IsEnabled = this._machine.PossoBloccareSlot;

            BottoneTieni1.Content = "Blocca";
            BottoneTieni2.Content = "Blocca";
            BottoneTieni3.Content = "Blocca";
            GiriRimanenti.Text = $"{this._machine.Rimanenti}";
        }
    }
}
