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
        public static int AZU = 0;

        StringTuple4D sT4D = new StringTuple4D();

        public Verwaltung_Aufträge()
        {
            InitializeComponent();
            //Lädt Aufträge in Liste
            Load_ODLs(listBox_Aufträge);
            //Lädt Daten in Comboboxen
            Load_CBI();

            comboBox_Maschine_Einstellungen.IsEnabled = false;
            comboBox_Status_Einstellungen.IsEnabled = false;
            comboBox_Verfahren_Einstellungen.IsEnabled = false;
            comboBox_Kategorie_Einstellungen.IsEnabled = false;

            button_Kategorie_Entfernen.IsEnabled = false;
            button_Kategorie_Hinzufügen.IsEnabled = false;

        }

        private void Load_CBI()
        {
            DDataContext d = new DDataContext();

            var kat = (from k in d.Kategorie
                       select new { k.Name }).Distinct();
            foreach (var item in kat)
            {
                comboBox_Kategorien_Suche.Items.Add(item.Name);
            }

            var ver = from v in d.Verfahren
                      select v;
            foreach (var item in ver)
            {
                comboBox_Verfahren_Suche.Items.Add(item.Name);
            }

            var mas = from m in d.Maschine
                      select m;
            foreach (var item in mas)
            {
                comboBox_Maschine_Suche.Items.Add(item.Name);
            }

        }

        private void comboBox_Status_Suche_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex != -1)
            {
                Load_ODLs(listBox_Aufträge);
            }

        }

        private void comboBox_Kategorien_Suche_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex != -1)
            {
                Load_ODLs(listBox_Aufträge);
            }
        }

        private void comboBox_Verfahren_Suche_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex != -1)
            {
                Load_ODLs(listBox_Aufträge);
            }
        }

        private void comboBox_Maschine_Suche_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex != -1)
            {
                Load_ODLs(listBox_Aufträge);
            }
        }


        //MouseUp Events
        private void listBox_Aufträge_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //Wenn kein Auftrag ausgewählt ist
            if (listBox_Aufträge.SelectedIndex != -1)
            {
                //Lade Auftragsdaten aus Datenbank
                Load_Auftragsdaten_From_DB(sender);

                button_Prozess_Schritt_Neu.IsEnabled = true;
                dataGrid_Prozess_Schritt_Refresh();

                AZU = 0;

                comboBox_Maschine_Einstellungen.IsEnabled = false;
                comboBox_Status_Einstellungen.IsEnabled = false;
                comboBox_Kategorie_Einstellungen.IsEnabled = false;
                comboBox_Verfahren_Einstellungen.IsEnabled = false;

                button_Kategorie_Hinzufügen.IsEnabled = false;
                button_Kategorie_Entfernen.IsEnabled = false;

                comboBox_Verfahren_Load();
                comboBox_Kategorie_Load();
                dataGrid_Kategorien_Load();

                Helper.ComboBoxSelectValue(comboBox_Maschine_Einstellungen, Helper.STRING_MASCHINE);
                Helper.ComboBoxSelectValue(comboBox_Status_Einstellungen, Helper.STRING_STATUS);
                Helper.ComboBoxSelectValue(comboBox_Verfahren_Einstellungen, Helper.STRING_VERFAHREN);
                Helper.ComboBoxSelectValue(comboBox_Kategorie_Einstellungen, Helper.STRING_KATEGORIE);


                textBox_Anlasstemp.Text = string.Empty;
                textBox_SProgramm.Text = string.Empty;

                textBox_Anlasstemp.Visibility = Visibility.Hidden;
                textBox_SProgramm.Visibility = Visibility.Hidden;
                label_Anlasstemp.Visibility = Visibility.Hidden;
                label_SProgramm.Visibility = Visibility.Hidden;

            }
            else
            {
                button_Prozess_Schritt_Neu.IsEnabled = false;


            }



        }

        //Sonstige Funktionen
        private void Load_ODLs(ListBox lBox)
        {
            DDataContext d = new DDataContext();

            string status = String.Empty;
            string kategorie = String.Empty;
            string verfahren = String.Empty;
            string maschine = String.Empty;

            try
            {
                status = Helper.GetComboBoxTextMK2(comboBox_Status_Suche);
            }
            catch (Exception)
            {
            }
            try
            {
                kategorie = Helper.GetComboBoxTextMK2(comboBox_Kategorien_Suche);
            }
            catch (Exception)
            {
            }
            try
            {
                verfahren = Helper.GetComboBoxTextMK2(comboBox_Verfahren_Suche);
            }
            catch (Exception)
            {
            }
            try
            {
                maschine = Helper.GetComboBoxTextMK2(comboBox_Maschine_Suche);
            }
            catch (Exception)
            {
            }

            var query = (from a in d.View_Verwaltung_Aufträge_Suche
                             // group a by a.ODL
                         select a);//.GroupBy(a => a.ODL);
            //var query =
            //    from a in d.Auftrag

            //    join z in d.Auftrags_Zuordnung on a.Id equals z.Id_Auftrag
            //    into zGroup
            //    from zG in zGroup.DefaultIfEmpty()

            //    join n in d.MN_Auftrag_Kategorie on zG.Id equals n.Id_Auftrags_Zuorndung
            //    into nGroup
            //    from nG in nGroup.DefaultIfEmpty()

            //    join k in d.Kategorie on nG.Id_Kategorie equals k.Id
            //    into kGroup
            //    from kG in kGroup.DefaultIfEmpty()

            //    join m in d.Maschine on zG.Id_Maschine equals m.Id
            //    into mGroup
            //    from mG in mGroup.DefaultIfEmpty()

            //    join v in d.Verfahren on zG.Id_Verfahren equals v.Id
            //    into vGroup
            //    from vG in vGroup.DefaultIfEmpty()

            //        // where kG.Name == kategorie && a.Status == status && vG.Name == verfahren && mG.Name == maschine

            //    select new { a.ODL, a.Status, Kategorie = kG.Name, Verfahren = vG.Name, Maschine = mG.Name };

            if (status != "Status")
            {
                query = from q in query
                        where q.Status == status
                        select q;
            }
            if (kategorie != "Kategorie")
            {
                query = from w in query
                        where w.Kategorie == kategorie
                        select w;
            }
            if (verfahren != "Verfahren")
            {
                query = from r in query
                        where r.Verfahren == verfahren
                        select r;
            }
            if (maschine != "Maschine")
            {
                query = from t in query
                        where t.Maschine == maschine
                        select t;
            }

            var fquery = (from g in query
                          select new { g.ODL }).Distinct();

            try
            {
                lBox.Items.Clear();


                foreach (var item in fquery)
                {

                    lBox.Items.Add(item.ODL);
                }

            }
            catch (Exception)
            {
            }

            listBox_change_weight();
        }

        private void Load_Auftragsdaten_From_DB(object sender)
        {
            //Lädt Daten in den Detailbereich
            if (((ListBox)sender).SelectedIndex != -1)
            {
                ODL = (string)((ListBox)sender).SelectedItem;
                frame.Refresh();

            }
        }
        private void comboBoxes_Choose_Data()
        {
            //Lädt Daten in den Detailbereich


            if (AZU != 0)
            {
                DDataContext d = new DDataContext();

                var auf = from x in d.Auftrags_Zuordnung
                          where x.Id == AZU
                          select x;
                var vef = from z in d.Verfahren
                          where z.Id == auf.First().Id_Verfahren
                          select z;
                var mas = from g in d.Maschine
                          where g.Id == auf.First().Id_Maschine
                          select g;

                //Alle comboBox_Status_Einstellungen Elemente auswählen
                foreach (ComboBoxItem cItem in comboBox_Status_Einstellungen.Items)
                {
                    if (cItem.Content.ToString() == auf.First().Status)
                    {
                        comboBox_Status_Einstellungen.SelectedItem = cItem;
                        break;
                    }
                }
                
                if (mas.Count() > 0)
                {
                    //Alle comboBox_Maschine_Einstellungen Elemente auswählen
                    foreach (string cItem in comboBox_Maschine_Einstellungen.Items)
                    {
                        if (cItem == mas.First().Name)
                        {
                            comboBox_Maschine_Einstellungen.SelectedItem = cItem;
                            break;
                        }
                    }
                }

                //Alle comboBox_Verfahren_Einstellungen Elemente auswählen
                if (vef.Count() > 0)
                {
                    try
                    {
                        foreach (string cItem in comboBox_Verfahren_Einstellungen.Items)
                        {
                            if (cItem == vef.First().Name)
                            {
                                comboBox_Verfahren_Einstellungen.SelectedItem = cItem;
                                break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                }


            }
            //ComboBox 'Kategorie' auswählen
            comboBox_Kategorie_Einstellungen.SelectedIndex = 0;


        }

        private void comboBox_Maschine_Einstellungen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //comboBox_Verfahren_Load();
            try
            {
                //ComboBox Maschine Einstellungen - Nichts gewählt?
                //Etwas
                if (comboBox_Maschine_Einstellungen.SelectedIndex != -1)
                {
                    //ComboBox Maschine Einstellungen - Standardwert gewählt?
                    //Nein
                    if (comboBox_Maschine_Einstellungen.SelectedIndex != 0)
                    {
                        //ComboBox Verfahren Einstellungen - Nichts gewählt?
                        //Ja
                        if (comboBox_Verfahren_Einstellungen.SelectedIndex == -1)
                        {
                            //Setze Wert auf Standard
                            comboBox_Verfahren_Einstellungen.SelectedIndex = 0;
                            //comboBox_Verfahren_Load();
                        }

                        DDataContext d = new DDataContext();
                        //Datenbankabfrage Maschineneinstellungen

                        //Normaler Ablauf
                        comboBox_Verfahren_Einstellungen.IsEnabled = true;

                        var azu = from y in d.Auftrags_Zuordnung
                                  where y.Id == AZU
                                  select y;

                        if (azu.Count() > 0)
                        {
                            string m = comboBox_Maschine_Einstellungen.SelectedItem.ToString();

                            //Wenn etwas ausgewählt wird
                            if (m != Helper.STRING_MASCHINE)
                            {
                                azu.First().Id_Maschine = (from n in d.Maschine where n.Name == m select n).First().Id;

                                //Nur für ausblenden/einblenden von Eigenschaften
                                var mas = from z in d.Maschine
                                          where z.Id == azu.First().Id_Maschine
                                          select z;
                                Switch_Maschine_Einstellungen_Maschine(mas.First());
                                //Läd die Verfahren wenn eine Maschine ausgewählt ist
                                comboBox_Verfahren_Load();
                                try
                                {
                                    Helper.ComboBoxSelectValue(comboBox_Verfahren_Einstellungen, azu.First().Verfahren.Name);
                                }
                                catch (Exception)
                                {
                                    comboBox_Verfahren_Einstellungen.SelectedIndex = 0;
                                }


                                comboBox_Kategorie_Einstellungen.IsEnabled = true;
                                button_Kategorie_Hinzufügen.IsEnabled = true;
                                button_Kategorie_Entfernen.IsEnabled = true;

                                ////Extrafelder

                                //    textBox_Anlasstemp.Text = azu.First().Temperatur.ToString();
                                //    textBox_SProgramm.Text = azu.First().Programm.ToString();

                                //    //textBox_Anlasstemp.Text = string.Empty;
                                //    //textBox_SProgramm.Text = string.Empty;



                            }
                            //Wenn "Maschine" ausgewählt wurde
                            else
                            {
                                //Läd die Verfahren wenn eine Maschine ausgewählt ist
                                comboBox_Verfahren_Load();
                                comboBox_Verfahren_Einstellungen.SelectedIndex = 0;

                                comboBox_Kategorie_Einstellungen.IsEnabled = false;
                                button_Kategorie_Hinzufügen.IsEnabled = false;
                                button_Kategorie_Entfernen.IsEnabled = false;
                            }


                        }

                        try
                        {
                            d.SubmitChanges();
                        }
                        catch (Exception)
                        {
                        }
                        comboBox_Kategorie_Load();


                    }
                    //ComboBox Maschine Einstellungen - Standardwert gewählt?
                    //Ja          
                    else
                    {
                        comboBox_Verfahren_Einstellungen.IsEnabled = false;
                    }
                }
                //ComboBox Maschine Einstellungen - Nichts gewählt?
                //Nichts
                //
                else
                {
                    comboBox_Maschine_Einstellungen.SelectedIndex = 0;
                    Switch_Maschine_Einstellungen_Maschine(new Maschine() { Temperatur = false, Programm = false });
                }

            }
            catch (Exception)
            {

            }
            dataGrid_Prozess_Schritt_Refresh();
        }

        private void comboBox_Verfahren_Einstellungen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex != 0 && ((ComboBox)sender).SelectedIndex != -1)
            {
                DDataContext d = new DDataContext();

                var azu = from y in d.Auftrags_Zuordnung
                          where y.Id == AZU
                          select y;

                if (azu.Count() > 0)
                {
                    string s = Helper.GetComboBoxTextComplete(comboBox_Verfahren_Einstellungen);
                    azu.First().Id_Verfahren = (from n in d.Verfahren where n.Name == s select n).First().Id;
                }


                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                }
            }
            dataGrid_Prozess_Schritt_Refresh();
        }

        private void comboBox_Status_Einstellungen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string s = Helper.GetComboBoxText(comboBox_Status_Einstellungen);

            if (s == Helper.STRING_ZUGEORDNET_STATUS && (comboBox_Verfahren_Einstellungen.SelectedIndex == 0
                    ))
            {
                MessageBox.Show("Bitte wählen sie eine Maschine und ein Verfahren!");
                comboBox_Status_Einstellungen.SelectedIndex = 1;
            }
            else
            if (((ComboBox)sender).SelectedIndex != 0 && ((ComboBox)sender).SelectedIndex != -1)
            {
                if (AZU != 0)
                {
                    //Erstelle Datenbankverbindung
                    DDataContext d = new DDataContext();
                    var auf = from x in d.Auftrags_Zuordnung
                              where x.Id == AZU
                              select x;
                    auf.First().Status = s;

                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {
                    }
                }

            }
            else if (s == "Status")
            {
                try
                {
                    DDataContext d = new DDataContext();
                    var auf = from x in d.Auftrags_Zuordnung
                              where x.Id == AZU
                              select x;
                    auf.First().Status = Helper.STRING_FREI_STATUS;

                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {
                    }
                }
                catch (Exception)
                {

                }

            }

        }

        private void comboBox_Kategorie_Einstellungen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBox_Maschine_Load()
        {
            try
            {
                //Erstelle Datenbankverbindung
                DDataContext d = new DDataContext();

                //Fülle Maschinen ComboBox
                var mas = from x in d.Maschine
                          select x;

                comboBox_Maschine_Einstellungen.Items.Clear();
                comboBox_Maschine_Einstellungen.Items.Add("Maschine");
                foreach (var item in mas)
                {
                    comboBox_Maschine_Einstellungen.Items.Add(item.Name);
                }
            }
            catch (Exception)
            {
            }

        }

        private void comboBox_Verfahren_Load()
        {
            try
            {
                string s = Helper.GetComboBoxTextMK2(comboBox_Maschine_Einstellungen);
                if (s.Length > 0)
                {
                    //Erstelle Datenbankverbindung
                    DDataContext d = new DDataContext();
                    //Fülle Verfahren

                    var ver = from x in d.View_Verfahren_der_Maschinen
                              where x.Name == s
                              select x.Name_Verfahren;

                    comboBox_Verfahren_Einstellungen.Items.Clear();
                    comboBox_Verfahren_Einstellungen.Items.Add("Verfahren");
                    foreach (string item in ver)
                    {
                        comboBox_Verfahren_Einstellungen.Items.Add(item);
                    }

                }

            }
            catch (Exception)
            {
            }

        }

        private void comboBox_Kategorie_Load()
        {
            try
            {
                //Erstelle Datenbankverbindung
                DDataContext d = new DDataContext();
                var kat = from x in d.Kategorie
                          where x.Id_Maschine == null
                          select x;
                string n = comboBox_Maschine_Einstellungen.SelectedItem.ToString();
                var kam = from y in d.Kategorie
                          where y.Id_Maschine == (int?)(from b in d.Maschine where b.Name == n select b).First().Id
                          select y;
                comboBox_Kategorie_Einstellungen.Items.Clear();
                comboBox_Kategorie_Einstellungen.Items.Add("Kategorie");
                foreach (var item in kat.Concat(kam))
                {
                    comboBox_Kategorie_Einstellungen.Items.Add(item.Name);
                }

            }
            catch (Exception)
            {
            }
        }

        private void button_Kategorie_Hinzufügen_Click(object sender, RoutedEventArgs e)
        {
            string ks = comboBox_Kategorie_Einstellungen.SelectedItem.ToString();
            if (ks != Helper.STRING_KATEGORIE)
            {
                DDataContext d = new DDataContext();
                //Auftrag ID
                //int? a = (from u in d.Auftrag
                //          where u.ODL == ODL
                //          select u).First().Id;
                //Auftragszuordung ID
                var z = (from o in d.Auftrags_Zuordnung
                         where o.Id == AZU
                         select o);
                //Kategorie ID
                int? k = (from t in d.Kategorie
                          where t.Name == comboBox_Kategorie_Einstellungen.SelectedItem.ToString()
                          select t).First().Id;
                if (z.Count() > 0)
                {

                    //NM Kategorie 
                    var kfa = from x in d.MN_Auftrag_Kategorie
                              where x.Id_Kategorie == k && x.Id_Auftrags_Zuorndung == z.First().Id
                              select x;

                    if (kfa.Count() == 0)
                    {
                        MN_Auftrag_Kategorie akt = new MN_Auftrag_Kategorie() { Id_Kategorie = k, Id_Auftrags_Zuorndung = z.First().Id };
                        d.MN_Auftrag_Kategorie.InsertOnSubmit(akt);
                    }
                    else
                    {
                        MessageBox.Show("Diese Kategorie wurde bereits diesem Auftrag zugeordnet!", Helper.STRING_ACHTUNG);
                    }
                }
                //else
                //{
                //    Auftrags_Zuordnung auftrags_Zuordung = new Auftrags_Zuordnung() { Id_Auftrag = a };
                //    d.Auftrags_Zuordnung.InsertOnSubmit(auftrags_Zuordung);
                //    try
                //    {
                //        d.SubmitChanges();
                //    }
                //    catch (Exception)
                //    {

                //    }
                //    //NM Kategorie 
                //    var kfa = from x in d.MN_Auftrag_Kategorie
                //              where x.Id_Kategorie == k && x.Id_Auftrags_Zuorndung == auftrags_Zuordung.Id
                //              select x;

                //    if (kfa.Count() == 0)
                //    {
                //        MN_Auftrag_Kategorie akt = new MN_Auftrag_Kategorie() { Id_Kategorie = k, Id_Auftrags_Zuorndung = auftrags_Zuordung.Id };
                //        d.MN_Auftrag_Kategorie.InsertOnSubmit(akt);
                //    }
                //    else
                //    {
                //        MessageBox.Show("Diese Kategorie wurde bereits diesem Auftrag zugeordnet!", Helper.STRING_ACHTUNG);
                //    }
                //}
                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                }
                dataGrid_Kategorien_Load();
            }

        }

        private void button_Kategorie_Entfernen_Click(object sender, RoutedEventArgs e)
        {
            DDataContext d = new DDataContext();
            ////Auftrag ID
            //int? a = (from u in d.Auftrag
            //          where u.ODL == ODL
            //          select u).First().Id;
            //Auftragszuordung ID
            int? z = (from o in d.Auftrags_Zuordnung
                      where o.Id_Auftrag == AZU
                      select o).First().Id;
            //Kategorie ID
            int? k = (from t in d.Kategorie
                      where t.Name == Helper.GetStringFromDataGrid(1, dataGrid_Kategorien)
                      select t).First().Id;
            //NM Kategorie 
            var kfa = from x in d.MN_Auftrag_Kategorie
                      where x.Id_Kategorie == k && x.Id_Auftrags_Zuorndung == z
                      select x;

            if (kfa.Count() != 0)
            {
                d.MN_Auftrag_Kategorie.DeleteAllOnSubmit(kfa);
            }


            try
            {
                d.SubmitChanges();
            }
            catch (Exception)
            {
            }
            dataGrid_Kategorien_Load();
        }

        private void dataGrid_Kategorien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dataGrid_Kategorien_Load()
        {

            DDataContext d = new DDataContext();
            List<Kategorie> kList = new List<Kategorie>();


            var kfa = from x in d.MN_Auftrag_Kategorie
                      where x.Id_Auftrags_Zuorndung == AZU
                      select x;

            if (kfa.Count() > 0)
            {
                kList.Clear();
                foreach (var item in kfa)
                {
                    var k = from y in d.Kategorie
                            where y.Id == item.Id_Kategorie
                            select y;
                    kList.AddRange(k);
                }

                dataGrid_Kategorien.ItemsSource = kList;
            }
            else
            {
                dataGrid_Kategorien.ItemsSource = null;
            }




        }

        private void label_Set_Values()
        {

            //label_Statistik.Content = string.Empty;

            //if (comboBox_Maschine_Suche.SelectedIndex != -1)
            //{
            //    DDataContext d = new DDataContext();

            //    int? a = (from y in d.Auftrag
            //              where y.ODL == listBox_Aufträge.SelectedItem.ToString()
            //              select y).First().Id;

            //    var mli = from x in d.Material
            //              where x.Id_Auftrag == a
            //              select x;

            //    foreach (var item in mli)
            //    {
            //        var mnv = from z in d.MN_Verfahren_Material
            //                  where z.Id_Material == item.Id
            //                  select z;

            //    }


            //}

            label_Statistik.Content = "0";
        }

        void Switch_Maschine_Einstellungen_Maschine(Maschine m)
        {
            if (m.Temperatur == true)
            {
                Switch_Maschinen_Einstellungen_Anlasstemp(true);
            }
            else
            {
                Switch_Maschinen_Einstellungen_Anlasstemp(false);
            }
            if (m.Programm == true)
            {
                Switch_Maschinen_Einstellungen_SProgramm(true);
            }
            else
            {
                Switch_Maschinen_Einstellungen_SProgramm(false);
            }
        }
        void Switch_Maschinen_Einstellungen_Anlasstemp(bool b)
        {

            if (b)
            {
                label_Anlasstemp.Visibility = Visibility.Visible;
                textBox_Anlasstemp.Visibility = Visibility.Visible;

            }
            else
            {
                label_Anlasstemp.Visibility = Visibility.Hidden;
                textBox_Anlasstemp.Visibility = Visibility.Hidden;
                textBox_Anlasstemp.Text = string.Empty;

            }

        }
        void Switch_Maschinen_Einstellungen_SProgramm(bool b)
        {

            if (b)
            {
                label_SProgramm.Visibility = Visibility.Visible;
                textBox_SProgramm.Visibility = Visibility.Visible;
            }
            else
            {
                label_SProgramm.Visibility = Visibility.Hidden;
                textBox_SProgramm.Visibility = Visibility.Hidden;
                textBox_SProgramm.Text = string.Empty;
            }

        }

        private void textBox_SProgramm_TextChanged(object sender, TextChangedEventArgs e)
        {
            DDataContext d = new DDataContext();

            try
            {
                if (AZU != 0)
                {
                    Auftrags_Zuordnung a = (from x in d.Auftrags_Zuordnung
                                            where x.Id == AZU
                                            select x).First();
                    int v;
                    if (Int32.TryParse(textBox_SProgramm.Text, out v))
                    {
                        a.Programm = v;
                        d.SubmitChanges();
                    }
                }


            }
            catch (Exception)
            {
                MessageBox.Show("Fehler! Bitte geben sie eine ganze Zahl ein!");
            }
        }
        private void textBox_Anlasstemp_TextChanged(object sender, TextChangedEventArgs e)
        {
            DDataContext d = new DDataContext();

            try
            {
                if (AZU != 0)
                {
                    Auftrags_Zuordnung a = (from x in d.Auftrags_Zuordnung
                                            where x.Id == AZU
                                            select x).First();
                    int v;
                    if (Int32.TryParse(textBox_Anlasstemp.Text, out v))
                    {
                        a.Temperatur = v;
                        d.SubmitChanges();
                    }
                }


            }
            catch (Exception)
            {
                MessageBox.Show("Fehler! Bitte geben sie eine ganze Zahl ein!");
            }
        }

        private void listBox_change_weight()
        {
            DDataContext d = new DDataContext();
            int i = 0;
            try
            {

                for (int j = 0; j < listBox_Aufträge.Items.Count; j++)
                {
                    string s = listBox_Aufträge.Items.GetItemAt(j).ToString();
                    var tes = from x in d.View_Auftrag_Gesamtgewicht
                              where x.ODL == s
                              select x;
                    if (tes.Count() > 0)
                    {
                        i = i + Convert.ToInt32(tes.First().Gesamtgewicht);
                    }

                }

            }
            catch (Exception)
            {
                i = 0;
            }
            try
            {
                ListBoxItem_TotalWeight.Content = "Total: " + i + " Kg";
            }
            catch (Exception)
            {

            }

        }

        private void dataGrid_Prozess_Schritt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_Prozess_Schritt.SelectedIndex != -1)
            {
                button_Prozess_Schritt_Bearbeiten.IsEnabled = true;
                AZU = 0;
                comboBox_Maschine_Einstellungen.IsEnabled = false;
                comboBox_Status_Einstellungen.IsEnabled = false;
                comboBox_Kategorie_Einstellungen.IsEnabled = false;
                comboBox_Verfahren_Einstellungen.IsEnabled = false;

                button_Kategorie_Hinzufügen.IsEnabled = false;
                button_Kategorie_Entfernen.IsEnabled = false;

                //comboBox_Verfahren_Load();
                //comboBox_Kategorie_Load();
                //dataGrid_Kategorien_Load();

                //Helper.ComboBoxSelectValue(comboBox_Maschine_Einstellungen, Helper.STRING_MASCHINE);
                //Helper.ComboBoxSelectValue(comboBox_Status_Einstellungen, Helper.STRING_STATUS);
                //Helper.ComboBoxSelectValue(comboBox_Verfahren_Einstellungen, Helper.STRING_VERFAHREN);
                //Helper.ComboBoxSelectValue(comboBox_Kategorie_Einstellungen, Helper.STRING_KATEGORIE);
                comboBox_Maschine_Einstellungen.SelectedIndex = 0;
                comboBox_Status_Einstellungen.SelectedIndex = 0;
                comboBox_Verfahren_Einstellungen.SelectedIndex = 0;
                comboBox_Kategorie_Einstellungen.SelectedIndex = 0;

                textBox_Anlasstemp.Text = string.Empty;
                textBox_SProgramm.Text = string.Empty;

                textBox_Anlasstemp.Visibility = Visibility.Hidden;
                textBox_SProgramm.Visibility = Visibility.Hidden;
                label_Anlasstemp.Visibility = Visibility.Hidden;
                label_SProgramm.Visibility = Visibility.Hidden;

            }
            else
            {
                button_Prozess_Schritt_Bearbeiten.IsEnabled = false;
            }


            dataGrid_Kategorien_Load();
        }

        private void button_Prozess_Schritt_Neu_Click(object sender, RoutedEventArgs e)
        {
            DDataContext d = new DDataContext();
            Auftrag a = (from y in d.Auftrag
                         where y.ODL == ODL
                         select y).First();
            //int c = (from x in d.Auftrags_Zuordnung
            //         where x.Id_Auftrag == a.Id
            //         select x).Count();

            if (true)
            {
                if (ODL != string.Empty)
                {




                    int c = (from x in d.Auftrags_Zuordnung
                             where x.Id_Auftrag == a.Id
                             select x).Count();

                    Auftrags_Zuordnung azu = new Auftrags_Zuordnung() { Id_Auftrag = a.Id, Schritt = c + 1 };
                    d.Auftrags_Zuordnung.InsertOnSubmit(azu);

                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Fehler! Keine Verbindung zur Datenbank!");
                    }
                    dataGrid_Prozess_Schritt_Refresh();
                }
                else
                {
                    button_Prozess_Schritt_Neu.IsEnabled = false;
                }
            }

        }
        private void dataGrid_Prozess_Schritt_Refresh()
        {

            DDataContext d = new DDataContext();

            //Leere Prozessschrittliste
            dataGrid_Prozess_Schritt.Items.Clear();
            try
            {
                int s = (from y in d.Auftrag
                         where y.ODL == ODL
                         select y).First().Id;

                var dvp = from x in d.Auftrags_Zuordnung
                          where x.Id_Auftrag == s
                          select x;

                foreach (var item in dvp)
                {
                    dataGrid_Prozess_Schritt.Items.Add(item);
                }

            }
            catch (Exception)
            {
            }
        }

        private void button_Prozess_Schritt_Bearbeiten_Click(object sender, RoutedEventArgs e)
        {

            if (dataGrid_Prozess_Schritt.SelectedIndex != -1)
            {
                try
                {
                    AZU = Helper.GetIntFromDataGrid(0, dataGrid_Prozess_Schritt);
                    comboBox_Maschine_Einstellungen.IsEnabled = true;
                    comboBox_Status_Einstellungen.IsEnabled = true;
                    comboBox_Maschine_Load();

                    comboBoxes_Choose_Data();

                    //Extraboxen
                    DDataContext d = new DDataContext();
                    var exb = from x in d.Auftrags_Zuordnung
                              where x.Id == AZU
                              select x;
                    textBox_Anlasstemp.Text = exb.First().Temperatur.ToString();
                    textBox_SProgramm.Text = exb.First().Programm.ToString();
                }
                catch (Exception)
                {


                }
            }
        }

        private void button_Prozess_Schritt_Speichern_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
