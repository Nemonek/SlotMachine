using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SlotMachineLibrary;

namespace WPFSlotMachine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SlotMachine m;
        private int[] _tieni;
        public MainWindow()
        {
            InitializeComponent();
            this.m = new();
            this._tieni = new int[3];
        }

        private void AggiungiCredito(object sender, RoutedEventArgs e)
        {
            int ToAdd =  int.Parse(QuantoCreditoAggiungere.Text);
            m.AggiungiCredito(ToAdd);
            Credito.Text = m.SaldoGiocatore.ToString();
        }

        private void RollaLettere(object sender, RoutedEventArgs e)
        {
            if (m.SaldoGiocatore == 0) return;
            
            char[] roll;
            if (m.Counter < 2 && (this._tieni[0] != -1 || this._tieni[1] != -1 || this._tieni[2] != -1))
                roll = m.EseguiRollMantenendo(this._tieni);
            else
                roll = m.EseguiRoll();

            GiriRimanenti.Text = m.Counter.ToString();


            PrimaLettera.Text = roll[0].ToString();
            SecondaLettera.Text = roll[1].ToString();
            TerzaLettera.Text = roll[2].ToString();
            Credito.Text = m.SaldoGiocatore.ToString(); ;
            Premio.Text = m.UltimoRisultato.ToString();
            this._tieni[0] = -1;
            this._tieni[1] = -1;
            this._tieni[2] = -1;
        }

        private void TieniUno(object sender, RoutedEventArgs e)
        {
            this._tieni[0] = 0;
        }
        private void TieniDue(object sender, RoutedEventArgs e)
        {
            this._tieni[1] = 1;
        }
        private void TieniTre(object sender, RoutedEventArgs e)
        {
            this._tieni[2] = 2;
        }
    }
}
