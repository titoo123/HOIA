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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HOIA.Allgemein
{
    /// <summary>
    /// Interaktionslogik für Details_Auftrag.xaml
    /// </summary>
    public partial class Details_Auftrag : Page
    {
        public Details_Auftrag()
        {
            InitializeComponent();

        }


        private void Button_Speichern_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_Wichtig_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_kunde_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_Bestimmungsort_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_AS400_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void ListView_Aufträge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        public void LoadData(string ODL) {
            string kg = " Kg";
            string abmessung = " mm";

            DDataContext d = new DDataContext();

            var drt = from h in d.Auftrag
                      where h.ODL == ODL
                      select h;
            if (drt.Count() > 0)
            {
                Auftrag a = drt.First();

                //Allgemeine Daten
                textBox_Walzung.Text = a.Walzung;
                textBox_Lagerort.Text = a.Lagerort;
                textBox_Verarbeitung.Text = a.Verarbeitung;
                textBox_Auftrag.Text = a.AuftragsNr + "/" + a.Position;
                textBox_ODL.Text = a.ODL;
                textBox_ADatum.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(a.Auftragsdatum.ToString()));
                textBox_Status.Text = a.Status;
                textBox_LTermin.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(a.Liefertermin.ToString()));

                //Beschreibung Material
                textBox_Abm1.Text = a.Abmessung1.ToString() + abmessung;
                textBox_Abm2.Text = a.Abmessung2.ToString() + abmessung;
                textBox_Art.Text = a.Art;
                textBox_Stahlsorte.Text = a.Stahlsorte;
                textBox_FLänge.Text = a.FLänge.ToString() + abmessung;
                textBox_WLänge.Text = a.WLänge.ToString() + abmessung;
                textBox_Charge.Text = a.Charge;
                //Gesamtmenge berechnen
                var ert = from l in d.Material
                          where l.Id_Auftrag == drt.First().Id
                          select l;
                textBox_Gesamtmenge.Text = ert.Count().ToString();
                //Gesamtgewicht berechnen
                int? geg = (from m in d.Material
                            where m.Id_Auftrag == a.Id
                            select m.Gewicht).Sum();
                textBox_Gesamtgewicht.Text = geg.ToString() + kg;
                //Zugehörige Materialien
                var mat = from m in d.Material
                          where m.Id_Auftrag == a.Id
                          select m;
                int z = 1;
                foreach (Material m in mat)
                {
                    m.Display_Position = z++; ;
                }
                ListView_Material.ItemsSource = mat;

                //Messwerte
                textBox_C.Text = a.C.ToString();
                textBox_Mn.Text = a.Mn.ToString();
                textBox_Si.Text = a.Si.ToString();
                textBox_P.Text = a.P.ToString();
                textBox_S.Text = a.S.ToString();
                textBox_Cr.Text = a.Cr.ToString();
                textBox_Ni.Text = a.Ni.ToString();
                textBox_Mo.Text = a.Mo.ToString();

                //Ergänzungen
                textBox_TecAnmerkungen.Text = a.TechnischeAnmerkungen;
                textBox_IntAnmerkungen.Text = a.Bemerkungen;
                textBox_Sägeprogramm.Text = a.SägeProgramm.ToString();
                textBox_Anlasstemp.Text = a.Anlasstemparartur.ToString();
                //Status aktualisieren
                switch (textBox_Status.Text)
                {
                    case "Frei":
                        radiobutton_Frei.IsChecked = true;
                        break;
                    case "Warten":
                        radiobutton_Warten.IsChecked = true;
                        break;
                    case "Gesperrt":
                        radiobutton_Gesperrt.IsChecked = true;
                        break;
                    default:
                        break;
                }

            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData(Verwaltung_Aufträge.ODL);
        }
    }
}
