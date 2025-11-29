using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace Taschenrechner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        public MainWindow()
        {
            InitializeComponent();
        }

        private double ersterWert;
        private string operatorZeichen;
        private bool isNewNumber = true;


        // Methode bzw. Logik für Zahlen-Buttons: 
        private void ZahlenButtons(object sender, RoutedEventArgs e)
        {
            Button taste = (Button)sender;
            string ziffer = (string)taste.Content;

            if (isNewNumber || Display.Text == "0")
            {
                if (!string.IsNullOrWhiteSpace(operatorZeichen) && Display.Text.Contains(operatorZeichen)) // Prüft auf gesetzten Operator und startet die Eingabe der zweiten Zahl.
                {
                    Display.Text += ziffer; // Start der 2. Zahl -> anhängen.
                }
                else
                {             
                    Display.Text = ziffer; // Start der 1. Zahl -> ersetzen.
                }

                isNewNumber = false;
            }
            else
            {
                Display.Text += ziffer;
            }
        }

        // Methode bzw. Logik für Komma-Button:
        private void KommaButton(object sender, RoutedEventArgs e)
        {
            Button taste = (Button)sender;
            string komma = (string)taste.Content;

            if (!Display.Text.Contains(","))
            {
                if (isNewNumber)
                {
                    Display.Text = "0" + komma;
                    isNewNumber = false;
                }
                else
                {
                    Display.Text += komma;
                }
            }
        }

        // Methode bzw. Logik für AllClear-Button:
        private void ACButton(object sender, RoutedEventArgs e)
        {
            Display.Text = "0"; // Display zurücksetzen auf Startwert "0".

            isNewNumber = true; // Erwartet eine neue Zahl bzw. die "0" wird ersetzt.

            ersterWert = 0; // Löscht alle gespeicherten Zwischenergebnisse.

            operatorZeichen = ""; // Stellt sicher, dass kein alter Operator mehr aktiv ist.
        }

        // Methode bzw. Logik für Backspace-Button:
        private void BackspaceButton(object sender, RoutedEventArgs e)
        {
            if (Display.Text.Length > 1)
            {
                Display.Text = Display.Text.Substring(0, Display.Text.Length - 1); // Entfernt das letzte Zeichen.
            }
            else
            {
                Display.Text = "0"; // Nur ein Zeichen oder 0 übrig -> Display wird auf Startwert "0" gesetzt.
            }

            isNewNumber = false;
        }

        // Methode bzw. Logik für die Operatoren-Buttons:
        private void OperatorenButtons(object sender, RoutedEventArgs e)
        {
            Button taste = (Button)sender;
            string neuerOperator = (string)taste.Content;


            if (!string.IsNullOrWhiteSpace(operatorZeichen) && !isNewNumber)
            {
                FühreBerechnungAus();
            }
            else
            {
                ersterWert = Convert.ToDouble(Display.Text.Replace(",", "."));
            }

            operatorZeichen = neuerOperator;

            isNewNumber = true;

            // Zeigt das Zwischenergebnis und den neuen Operator an.
            Display.Text = ersterWert.ToString(CultureInfo.CurrentCulture) + operatorZeichen;
        }

        // Methode bzw. Logik für das Gleichheitszeichen-Button:
        private void GleichheitszeichenButton(object sender, RoutedEventArgs e)
        {
            // Fehlerprüfung hinzugefügt, da der Benutzer jederzeit "=" klicken kann.
            if (string.IsNullOrWhiteSpace(operatorZeichen))
            {
                return;
            }

            FühreBerechnungAus();

            // Nach "=" ist die Kette beendet und Operator löschen.
            operatorZeichen = "";
        }

        private void FühreBerechnungAus()
        {

            double zweiterWert;
            double ergebnis = 0;


            // Zweiten Wert ermitteln:
            int operatorIndex = Display.Text.IndexOf(operatorZeichen);
            string zweiteZahlAlsString = Display.Text.Substring(operatorIndex + operatorZeichen.Length);

            if (string.IsNullOrWhiteSpace(zweiteZahlAlsString))
            {
                zweiterWert = ersterWert;
            }
            else
            {
                zweiterWert = Convert.ToDouble(zweiteZahlAlsString.Replace(",", "."));
            }

            // Berechnung:
            switch (operatorZeichen)
            {
                case "+":
                    ergebnis = ersterWert + zweiterWert;
                    break;
                case "-":
                    ergebnis = ersterWert - zweiterWert;
                    break;
                case "x":
                    ergebnis = ersterWert * zweiterWert;
                    break;
                case "/":
                    if (zweiterWert != 0)
                    {
                        ergebnis = ersterWert / zweiterWert;
                    }
                    else
                    {
                        Display.Text = "Error: Division durch Null";
                        isNewNumber = true;
                        operatorZeichen = "";
                        return;
                    }
                    break;
                default:
                    ergebnis = zweiterWert;
                    break;
            }

            Display.Text = ergebnis.ToString("N", CultureInfo.CurrentCulture); // Zeigt das berechnete Zwischenergebnis im de-DE-Format (Komma) an.
            ersterWert = ergebnis; // Speichert das Ergebnis als neuen Startwert für die fortlaufende Rechnung (Kettenrechnung).
            isNewNumber = true; // Setzt den Zustand zurück, damit die nächste Zahl das Zwischenergebnis überschreibt.
        }
    }   
}