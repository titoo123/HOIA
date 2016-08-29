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
    public partial class Freie_Aufträge : Page
    {
        StringTuple3D sT3D = new StringTuple3D();

        List<string> auftrag_ODL_List = new List<string>();

        public Freie_Aufträge()
        {
            InitializeComponent();

            DDataContext d = new DDataContext();

            //Freie Aufträge
            var o = from a in d.Auftrag
                    where a.Status == "Frei"
                    select new { a.ODL };

            foreach (var item in o)
            {
                auftrag_ODL_List.Add(item.ODL);
                listBox_Aufträge.Items.Add(item.ODL);
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_Aufträge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        private void Button_Maschinen_Verwalten_Click(object sender, RoutedEventArgs e)
        {
           Daten.Maschinen_Window mwi = new Daten.Maschinen_Window();
            mwi.Show();
        }

        private void Button_Verfahren_Verwalten_Click(object sender, RoutedEventArgs e)
        {
            Daten.Verfahren_Window vwi = new Daten.Verfahren_Window();
            vwi.Show();
        }

        private void Button_Kategorie_Verwalten_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Maschine.SelectedIndex != -1)
            {
                Daten.Kategorie_Window kwi = new Daten.Kategorie_Window(listBox_Maschine.SelectedValue.ToString());
                kwi.Show();
            }
            else
            {
                Daten.Kategorie_Window kwi = new Daten.Kategorie_Window();
                kwi.Show();
            }

        }

        private void Button_Von_Auftrag_Zu_Maschine_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Aufträge.SelectedIndex != -1 && listBox_Maschine.SelectedIndex != -1)
            {
                string auf_odl = listBox_Aufträge.SelectedItem.ToString();
                string auf_mas = listBox_Maschine.SelectedItem.ToString();

                //Fügt Datensatz hinzu
                //ODL - Maschine
                sT3D.Add(auf_odl, auf_mas, String.Empty);
                //Löscht Eintrag aus Auftragsliste
                listBox_Aufträge.Items.Remove(auf_odl);
                //Leert Liste
                listBox_Aufträge_in_Maschine.Items.Clear();

                foreach (var chosen_item in sT3D.Items)
                {
                        if ((string)chosen_item.Item2 == (string)listBox_Maschine.SelectedItem)
                        {
                            listBox_Aufträge_in_Maschine.Items.Add(chosen_item.Item1);
                        }
                }


                //DDataContext d = new DDataContext();

                //Auftrags_Zuordnung azu = new Auftrags_Zuordnung() { };
            }

        }

        private void Button_Von_Maschine_Zu_Auftrag_Click(object sender, RoutedEventArgs e)
        {
            listBox_Aufträge.Items.Add((string)listBox_Aufträge_in_Maschine.SelectedItem);

        }

        private void Button_Von_Verfahren_Zu_Kategorie_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Von_Kategorie_Zu_Verfahren_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Von_Maschine_Zu_Kategorie_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Von_Kategorie_Zu_Maschine_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listBox_Maschine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Leert Liste
            listBox_Aufträge_in_Maschine.Items.Clear();

            //Sucht passende Einträge
            foreach (var chosen_item in sT3D.Items)
            {

                    if ((string)chosen_item.Item2 == (string)listBox_Maschine.SelectedItem)
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
            }
        }
    }
}
