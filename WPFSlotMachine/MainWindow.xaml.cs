using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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

        public MainWindow() {
            InitializeComponent();

            this._machine = new();
            this._inputInvalido = new() {
                Interval = TimeSpan.FromSeconds(5)
            };
            
            this._inputInvalido.Tick += InputInvalido_RipristinoColoreTextBox;

            BottoneTieni1.IsEnabled = this._machine.PossoBloccareSlot;
            BottoneTieni2.IsEnabled = this._machine.PossoBloccareSlot;
            BottoneTieni3.IsEnabled = this._machine.PossoBloccareSlot;

            GiriRimanenti.Text = $"{this._machine.Rimanenti}";
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
                AggiornaCredito();
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
                AggiornaCredito();
            }

            QuantoCreditoAggiungere.Text = "";
        }

        private void InputInvalido_RipristinoColoreTextBox(object? sender, EventArgs e) {
            QuantoCreditoAggiungere.BorderBrush = Brushes.LightGray;
            AggiungiCredito_InputInvalidoTextBlock.Visibility = Visibility.Collapsed;
        }
        private void AggiornaCredito()
        {
            Credito.Text = this._machine.Credito.ToString();
        }
        private void RollaLettere(object sender, RoutedEventArgs e)
        {
            if(this._machine.Credito == 0) {
                MessageBox.Show("Credito insufficiente: impossibile procedre con l'operazione");
                return;
            }
            char[] ret = this._machine.Rolla();
            PrimaLettera.Text = ret[0].ToString();
            SecondaLettera.Text = ret[1].ToString();
            TerzaLettera.Text = ret[2].ToString();

            BottoneTieni1.IsEnabled = this._machine.PossoBloccareSlot;
            BottoneTieni2.IsEnabled = this._machine.PossoBloccareSlot;
            BottoneTieni3.IsEnabled = this._machine.PossoBloccareSlot;

            GiriRimanenti.Text = this._machine.Rimanenti.ToString();
        }

        private void TieniUno(object sender, RoutedEventArgs e)
        {
            this._machine.Slot1 = true;
        }
        private void TieniDue(object sender, RoutedEventArgs e)
        {
            this._machine.Slot2 = true;
        }
        private void TieniTre(object sender, RoutedEventArgs e)
        {
            this._machine.Slot3 = true;
        }

        private void QuantoCreditoAggiungere_EnterPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                AggiungiCredito();
        }
    }
}
