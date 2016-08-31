using HOIA.Erweiterungen;
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
    /// Interaktionslogik für Freie_Aufträge.xaml
    /// </summary>
    public partial class Verwaltung_Aufträge : Page
    {
        public static string ODL = String.Empty;

        StringTuple4D sT4D = new StringTuple4D();
        
        public Verwaltung_Aufträge()
        {
            InitializeComponent();

            Load_Order_ODLs(listBox_Aufträge);
            
        }

        //Verwaltung der Daten
        private void Button_Maschinen_Verwalten_Click(object sender, RoutedEventArgs e)
        {
           Daten.Maschinen_Window mwi = new Daten.Maschinen_Window(this);
            mwi.Show();
        }

        private void Button_Verfahren_Verwalten_Click(object sender, RoutedEventArgs e)
        {
            Daten.Verfahren_Window vwi = new Daten.Verfahren_Window(this);
            vwi.Show();
        }

        private void Button_Kategorie_Verwalten_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Maschine.SelectedIndex != -1)
            {
                Daten.Kategorie_Window kwi = new Daten.Kategorie_Window(listBox_Maschine.SelectedValue.ToString(),this);
                kwi.Show();
            }
            else
            {
                Daten.Kategorie_Window kwi = new Daten.Kategorie_Window(this);
                kwi.Show();
            }

        }

        //Pfeilbuttons zum zuweisen
        private void Button_Von_Auftrag_Zu_Maschine_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Aufträge.SelectedIndex != -1 && listBox_Maschine.SelectedIndex != -1)
            {
                //Übergibt Daten aus Listboxes
                string auf_odl = listBox_Aufträge.SelectedItem.ToString();
                string auf_mas = listBox_Maschine.SelectedItem.ToString();

                //Fügt Datensatz hinzu
                //ODL - Maschine
                sT4D.Add(auf_odl, auf_mas, String.Empty,String.Empty);
                //Löscht Eintrag aus Auftragsliste
                listBox_Aufträge.Items.Remove(auf_odl);
                //Leert Liste
                listBox_Aufträge_in_Maschine.Items.Clear();

                foreach (var chosen_item in sT4D.Items)
                {
                        if (
                            chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
                        &&  chosen_item.Item3 == String.Empty
                        &&  chosen_item.Item4 == String.Empty
                        )
                        {
                            listBox_Aufträge_in_Maschine.Items.Add(chosen_item.Item1);
                        }
                }

            }

        }

        private void Button_Von_Maschine_Zu_Auftrag_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Aufträge_in_Maschine.SelectedIndex != -1)
            {
                string auf_odl = listBox_Aufträge_in_Maschine.SelectedItem.ToString();
                listBox_Aufträge.Items.Add(auf_odl);
                listBox_Aufträge_in_Maschine.Items.Remove(auf_odl);
                sT4D.RemoveByFirst(auf_odl);

                //Leert Liste
                listBox_Aufträge_in_Maschine.Items.Clear();

                foreach (var chosen_item in sT4D.Items)
                {
                    if (
                            chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
                        &&  chosen_item.Item3 == String.Empty
                        &&  chosen_item.Item4 == String.Empty
                        )
                    {
                        listBox_Aufträge_in_Maschine.Items.Add(chosen_item.Item1);
                    }
                }
            }

        }

        private void Button_Von_Verfahren_Zu_Kategorie_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Verfahren.SelectedIndex != -1
               && listBox_Aufträge_in_Verfahren.SelectedIndex != -1
               && listBox_Kategorie.SelectedIndex != -1)
            {
                string auf_odl = listBox_Aufträge_in_Verfahren.SelectedItem.ToString();
                string auf_kat = listBox_Kategorie.SelectedItem.ToString();
                string auf_mas = listBox_Maschine.SelectedItem.ToString();

                //listBox_Aufträge_in_Maschine.Items.Add(auf_odl);
                listBox_Aufträge_in_Verfahren.Items.Remove(auf_odl);

                sT4D.RemoveByFirst(auf_odl);

                sT4D.Add(auf_odl, auf_mas, auf_kat, String.Empty);

                //Leert Liste
                listBox_Aufträge_in_Kategorie.Items.Clear();

                foreach (var chosen_item in sT4D.Items)
                {
                    if (
                        chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
                    &&  chosen_item.Item3 == (string)listBox_Kategorie.SelectedItem
                    &&  chosen_item.Item4 == String.Empty
                    )
                    {
                        listBox_Aufträge_in_Kategorie.Items.Add(chosen_item.Item1);
                    }
                }
            }
        }

        private void Button_Von_Kategorie_Zu_Verfahren_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Kategorie.SelectedIndex != -1 && listBox_Aufträge_in_Kategorie.SelectedIndex != -1
                && listBox_Verfahren.SelectedIndex != -1)
            {
                string auf_odl = listBox_Aufträge_in_Kategorie.SelectedItem.ToString();
                string auf_mas = listBox_Maschine.SelectedItem.ToString();
                string auf_kat = listBox_Kategorie.SelectedItem.ToString();
                string auf_ver = listBox_Verfahren.SelectedItem.ToString();


                listBox_Aufträge_in_Verfahren.Items.Add(auf_odl);
                listBox_Aufträge_in_Kategorie.Items.Remove(auf_odl);

                sT4D.RemoveByFirst(auf_odl);
                //Fügt Datensatz hinzu
                //ODL - Maschine
                sT4D.Add(auf_odl, auf_mas, auf_kat, auf_ver);



                //Leert Liste
                listBox_Aufträge_in_Verfahren.Items.Clear();

                foreach (var chosen_item in sT4D.Items)
                {
                    if (   chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
                        && chosen_item.Item3 == (string)listBox_Kategorie.SelectedItem
                        && chosen_item.Item4 == (string)listBox_Verfahren.SelectedItem
                        )
                    {
                        listBox_Aufträge_in_Verfahren.Items.Add(chosen_item.Item1);
                    }
                }
            }
        }

        private void Button_Von_Maschine_Zu_Kategorie_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Kategorie.SelectedIndex != -1 && listBox_Aufträge_in_Maschine.SelectedIndex != -1)
            {                
                //Übergibt Daten aus Listboxes
                string auf_odl = listBox_Aufträge_in_Maschine.SelectedItem.ToString();
                string auf_mas = listBox_Maschine.SelectedItem.ToString();
                string auf_kat = listBox_Kategorie.SelectedItem.ToString();

                //Löscht Auftrag aus Zuordnungsliste
                sT4D.RemoveByFirst(auf_odl);
                //Fügt Datensatz hinzu
                //ODL - Maschine - Kategorie
                sT4D.Add(auf_odl, auf_mas, auf_kat,String.Empty);
                //Löscht Eintrag aus Maschinenliste
                listBox_Aufträge_in_Maschine.Items.Remove(auf_odl);
                //Leert Liste
                listBox_Aufträge_in_Kategorie.Items.Clear();

                foreach (var chosen_item in sT4D.Items)
                {
                    if (    chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
                        &&  chosen_item.Item3 == (string)listBox_Kategorie.SelectedItem
                        &&  chosen_item.Item4 == String.Empty
                        //&&  chosen_item.Item4 == (string)listBox_Verfahren.SelectedItem
                        )
                    {
                        listBox_Aufträge_in_Kategorie.Items.Add(chosen_item.Item1);
                    }
                }
            }
        }

        private void Button_Von_Kategorie_Zu_Maschine_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Maschine.SelectedIndex !=-1 
                && listBox_Aufträge_in_Kategorie.SelectedIndex !=-1 
                && listBox_Kategorie.SelectedIndex != -1)
            {
                string auf_odl = listBox_Aufträge_in_Kategorie.SelectedItem.ToString();
                string auf_mas = listBox_Maschine.SelectedItem.ToString();

                //listBox_Aufträge_in_Maschine.Items.Add(auf_odl);
                listBox_Aufträge_in_Kategorie.Items.Remove(auf_odl);

                sT4D.RemoveByFirst(auf_odl);

                sT4D.Add(auf_odl, auf_mas, String.Empty, String.Empty);

                //Leert Liste
                listBox_Aufträge_in_Maschine.Items.Clear();

                foreach (var chosen_item in sT4D.Items)
                {
                    if (
                        chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
                    &&  chosen_item.Item3 == String.Empty
                    &&  chosen_item.Item4 == String.Empty
                    )
                    {
                        listBox_Aufträge_in_Maschine.Items.Add(chosen_item.Item1);
                    }
                }
            }
        }
        
        //ListBox Änderungen
        private void listBox_Maschine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Leert Liste
            listBox_Aufträge_in_Maschine.Items.Clear();

            //Sucht passende Einträge
            foreach (var chosen_item in sT4D.Items)
            {

                    if (
                        chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
                    &&  chosen_item.Item3 == String.Empty
                    &&  chosen_item.Item4 == String.Empty
                    )
                    {
                        listBox_Aufträge_in_Maschine.Items.Add(chosen_item.Item1);
                    }


            }

            //Sucht Kategorien für Maschine
            if (listBox_Maschine.SelectedIndex != -1)
            {
                
                DDataContext d = new DDataContext();

                var s = from m in d.Kategorie
                        where m.Maschine.Name == listBox_Maschine.SelectedValue.ToString()
                        select m;
                List<string> l_s = new List<string>();
                foreach (var item in s)
                {
                    l_s.Add(item.Name);
                }

                listBox_Kategorie.ItemsSource = l_s;
                //Sucht Maschine
                Maschine mas = (from k in d.Maschine
                                where k.Name == listBox_Maschine.SelectedValue.ToString()
                                select k).First();
                //Sucht Maschinenart
                Maschinenart mar = (from a in d.Maschinenart
                          where a.Id == mas.Id_Maschinenart
                          select a).First();
                //Sucht passende Verfahren
                listBox_Verfahren.ItemsSource = (from f in d.Verfahren
                          where f.Id_Maschinenart == mar.Id
                          select f.Name).ToList();


                //Erneuert Aufträge in Kategorie
                listBox_Kategorie.SelectedIndex = -1;
                listBox_Verfahren.SelectedIndex = -1;
                //Leert andere Listen
                listBox_Aufträge_in_Verfahren.Items.Clear();
                listBox_Aufträge_in_Kategorie.Items.Clear();
            }
        }

        private void listBox_Kategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Leert Liste
            listBox_Aufträge_in_Kategorie.Items.Clear();
            //Leert andere Listen
            listBox_Aufträge_in_Verfahren.Items.Clear();
            //Sucht passende Einträge
            if (listBox_Kategorie.SelectedIndex != -1)
            {

                foreach (var chosen_item in sT4D.Items)
                {
                    if (
                            chosen_item.Item3 == (string)listBox_Kategorie.SelectedItem
                        &&  chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
                        &&  chosen_item.Item4 == String.Empty
                        )
                    {
                        listBox_Aufträge_in_Kategorie.Items.Add(chosen_item.Item1);
                    }
                }


            }
        }

        private void listBox_Verfahren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Leert Liste
            listBox_Aufträge_in_Verfahren.Items.Clear();
           // listBox_Aufträge_in_Kategorie.Items.Clear();

            //Sucht passende Einträge
            if (listBox_Verfahren.SelectedIndex != -1)
            {
                foreach (var chosen_item in sT4D.Items)
                {
                    if (
                            chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
                        &&  chosen_item.Item3 == (string)listBox_Kategorie.SelectedItem
                        &&  chosen_item.Item4 == (string)listBox_Verfahren.SelectedItem
                        )
                    {
                        listBox_Aufträge_in_Verfahren.Items.Add(chosen_item.Item1);
                    }
                }
                
            }
        }

        //MouseUp Events
        private void listBox_Aufträge_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangeValues(sender);
        }

        private void listBox_Aufträge_in_Maschine_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangeValues(sender);
        }

        private void listBox_Aufträge_in_Kategorie_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangeValues(sender);
        }

        private void listBox_Aufträge_in_Verfahren_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangeValues(sender);
        }

        //ComboBox Funktionen
        private void comboBox_Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Load_Order_ODLs(listBox_Aufträge);
        }

        //Sonstige Funktionen
        private void Load_Order_ODLs(ListBox lBox) {

            DDataContext d = new DDataContext();

            //Freie Aufträge
            var o = from a in d.Auftrag
                    where a.Status == ((ComboBoxItem)comboBox_Status.SelectedItem).Content.ToString()
                    select new { a.ODL };
            if (lBox != null)
            {
                try
                {
                    lBox.Items.Clear();


                    foreach (var item in o)
                    {
                        // auftrag_ODL_List.Add(item.ODL);
                        lBox.Items.Add(item.ODL);
                    }

                    //Maschinen
                    var e = from m in d.Maschine
                            select new { m.Name };
                    List<string> l_m = new List<string>();
                    foreach (var item in e)
                    {
                        l_m.Add(item.Name);
                    }

                    listBox_Maschine.ItemsSource = l_m;
                }
                catch (Exception)
                {
                }
            }

        }

        private void Information_Loader(string s)
        {
            //string kg = " Kg";
            //string abmessung = " mm";

            //DDataContext d = new DDataContext();

            //var drt = from h in d.Auftrag
            //          where h.ODL == s
            //          select h;
            //if (drt.Count() > 0)
            //{
            //    Auftrag a = drt.First();

            //    //Allgemeine Daten
            //    textBox_Walzung.Text = a.Walzung;
            //    textBox_Lagerort.Text = a.Lagerort;
            //    textBox_Verarbeitung.Text = a.Verarbeitung;
            //    textBox_Auftrag.Text = a.AuftragsNr + "/" + a.Position;
            //    textBox_ODL.Text = a.ODL;
            //    textBox_ADatum.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(a.Auftragsdatum.ToString()));
            //    textBox_Status.Text = a.Status;
            //    textBox_LTermin.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(a.Liefertermin.ToString()));

            //    //Beschreibung Material
            //    textBox_Abm1.Text = a.Abmessung1.ToString() + abmessung;
            //    textBox_Abm2.Text = a.Abmessung2.ToString() + abmessung;
            //    textBox_Art.Text = a.Art;
            //    textBox_Stahlsorte.Text = a.Stahlsorte;
            //    textBox_FLänge.Text = a.FLänge.ToString() + abmessung;
            //    textBox_WLänge.Text = a.WLänge.ToString() + abmessung;
            //    textBox_Charge.Text = a.Charge;
            //    //Gesamtmenge berechnen
            //    var ert = from l in d.Material
            //              where l.Id_Auftrag == drt.First().Id
            //              select l;
            //    textBox_Gesamtmenge.Text = ert.Count().ToString();
            //    //Gesamtgewicht berechnen
            //    int? geg = (from m in d.Material
            //                where m.Id_Auftrag == a.Id
            //                select m.Gewicht).Sum();
            //    textBox_Gesamtgewicht.Text = geg.ToString() + kg;
            //    //Zugehörige Materialien
            //    var mat = from m in d.Material
            //              where m.Id_Auftrag == a.Id
            //              select m;
            //    int z = 1;
            //    foreach (Material m in mat)
            //    {
            //        m.Display_Position = z++; ;
            //    }
            //    ListView_Material.ItemsSource = mat;

            //    //Messwerte
            //    textBox_C.Text = a.C.ToString();
            //    textBox_Mn.Text = a.Mn.ToString();
            //    textBox_Si.Text = a.Si.ToString();
            //    textBox_P.Text = a.P.ToString();
            //    textBox_S.Text = a.S.ToString();
            //    textBox_Cr.Text = a.Cr.ToString();
            //    textBox_Ni.Text = a.Ni.ToString();
            //    textBox_Mo.Text = a.Mo.ToString();

            //    //Ergänzungen
            //    textBox_TecAnmerkungen.Text = a.TechnischeAnmerkungen;
            //    textBox_IntAnmerkungen.Text = a.Bemerkungen;
            //    textBox_Sägeprogramm.Text = a.SägeProgramm.ToString();
            //    textBox_Anlasstemp.Text = a.Anlasstemparartur.ToString();
            //    //Status aktualisieren
            //    switch (textBox_Status.Text)
            //    {
            //        case "Frei":
            //            radiobutton_Frei.IsChecked = true;
            //            break;
            //        case "Warten":
            //            radiobutton_Warten.IsChecked = true;
            //            break;
            //        case "Gesperrt":
            //            radiobutton_Gesperrt.IsChecked = true;
            //            break;
            //        default:
            //            break;
            //    }

            //}
        }

        private void ChangeValues(object sender)
        {
            //Lädt Daten in den Detailbereich
            if (((ListBox)sender).SelectedIndex != -1)
            {
                ODL = (string)((ListBox)sender).SelectedItem;
                frame.Refresh();

            }
        }

    }
}
