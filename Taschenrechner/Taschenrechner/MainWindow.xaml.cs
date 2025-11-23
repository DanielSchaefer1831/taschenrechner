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
        private bool isNewNumber = true;
        private double ersterWert = 0;
        private string operatorZeichen = "0";

        public MainWindow()
        {
            InitializeComponent();
        }

        // Methode für Klicks auf Zahlen- und Komma-Buttons:
        private void ZahlenButtonGeklickt(object sender, RoutedEventArgs e)
        {
            Button taste = (Button)sender;

            string ziffer = (string)taste.Content;

            if (ziffer == ",")
            {
                if (!Display.Text.Contains(","))
                {
                    Display.Text += ziffer;
                }
            }
            else
            {
                if (Display.Text == "0")
                {
                    Display.Text = ziffer;
                }
                else
                {
                    Display.Text += ziffer;
                }
            }
        }

        // Methode für Klick auf AC-Button:
        private void AC_Button(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
            isNewNumber = true;
        }

        // Methode, um die letzte eingegebene Ziffer/Komma zu löschen:
        private void LoeschenButton(object sender, RoutedEventArgs e)
        {
            if (Display.Text.Length > 1)
            {
                Display.Text = Display.Text.Substring(0, Display.Text.Length - 1);
            }
            else
            {
                Display.Text = "0";
            }
        }

        // Methode für Operatoren-Logik:
        private void OperatorenButtons(object sender, RoutedEventArgs e)
        {
            Button taste = (Button)sender;
            string geklickterOperator = (string)taste.Content;

            ersterWert = Convert.ToDouble(Display.Text.Replace(",", "."));

            operatorZeichen = geklickterOperator;

            isNewNumber = true;

            Display.Text = ersterWert + operatorZeichen;
        }

        // Methode für Gleichheitszeichen-Logik:
        private void GleichheitszeichenButton(object sender, RoutedEventArgs e)
        {
            double zweiterWert;
            double ergebnis = 0;

            
            // Zweiter Wert nach dem Operator:
            int operatorIndex = Display.Text.LastIndexOf(operatorZeichen);

            string anzeigeTextOhneOperator;
            if (operatorIndex > 0 && operatorZeichen != "")
            {
                anzeigeTextOhneOperator = Display.Text.Substring(operatorIndex + operatorZeichen.Length);
            }
            else
            {
                anzeigeTextOhneOperator = Display.Text;
            }


            // Zweite Zahl in einen Double konvertieren:
            if (string.IsNullOrWhiteSpace(anzeigeTextOhneOperator))
            {
                zweiterWert = ersterWert;
            }
            else
            {
                zweiterWert = Convert.ToDouble(anzeigeTextOhneOperator.Replace(",", "."));
            }



            // BERECHNUNG:
            switch(operatorZeichen)
            {
                case "+":
                    ergebnis = ersterWert + zweiterWert;
                    break;

                case "-":
                    ergebnis = ersterWert - zweiterWert;
                    break;

                case "*":
                    ergebnis = ersterWert * zweiterWert;
                    break;

                case "/":
                    if (zweiterWert != 0)
                    {
                        ergebnis = ersterWert / zweiterWert;
                    }
                    else
                    {
                        Display.Text = "Error! 0 kann man nicht teilen!";
                        isNewNumber = true;
                        return;
                    }
                    break;
                
                default: // Wenn man keinen Operator eingesetzt hat:
                    ergebnis = zweiterWert;
                    break;
            }

            Display.Text  = ergebnis.ToString(new CultureInfo("de-DE"));

            ersterWert = ergebnis;

            isNewNumber = true;
            operatorZeichen = "";
        }
    }   
}