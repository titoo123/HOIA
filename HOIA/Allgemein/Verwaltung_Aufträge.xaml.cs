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

            Load_ODLs(listBox_Aufträge);

            comboBox_Maschine_Einstellungen.IsEnabled = false;
            comboBox_Status_Einstellungen.IsEnabled = false;
            comboBox_Verfahren_Einstellungen.IsEnabled = false;
            comboBox_Kategorie_Wählen.IsEnabled = false;
            //Load_Maschinen(listBox_Maschine);

        }

        ////ComboBox Funktionen
        //private void comboBox_Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    Load_ODLs(listBox_Aufträge);
        //}

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

        ////Pfeilbuttons zum zuweisen
        //private void Button_Von_Auftrag_Zu_Maschine_Click(object sender, RoutedEventArgs e)
        //{
        //    if (listBox_Aufträge.SelectedIndex != -1 && listBox_Maschine.SelectedIndex != -1)
        //    {
        //        //Übergibt Daten aus Listboxes
        //        string auf_odl = listBox_Aufträge.SelectedItem.ToString();
        //        string auf_mas = listBox_Maschine.SelectedItem.ToString();

        //        //Fügt Datensatz hinzu
        //        //ODL - Maschine
        //        sT4D.Add(auf_odl, auf_mas, String.Empty,String.Empty);
        //        //Löscht Eintrag aus Auftragsliste
        //        listBox_Aufträge.Items.Remove(auf_odl);
        //        //Leert Liste
        //        listBox_Aufträge_in_Maschine.Items.Clear();

        //        foreach (var chosen_item in sT4D.Items)
        //        {
        //                if (
        //                    chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
        //                &&  chosen_item.Item3 == String.Empty
        //                &&  chosen_item.Item4 == String.Empty
        //                )
        //                {
        //                    listBox_Aufträge_in_Maschine.Items.Add(chosen_item.Item1);
        //                }
        //        }

        //    }

        //}

        //private void Button_Von_Maschine_Zu_Auftrag_Click(object sender, RoutedEventArgs e)
        //{
        //    if (listBox_Aufträge_in_Maschine.SelectedIndex != -1)
        //    {
        //        string auf_odl = listBox_Aufträge_in_Maschine.SelectedItem.ToString();
        //        listBox_Aufträge.Items.Add(auf_odl);
        //        listBox_Aufträge_in_Maschine.Items.Remove(auf_odl);
        //        sT4D.RemoveByFirst(auf_odl);

        //        //Leert Liste
        //        listBox_Aufträge_in_Maschine.Items.Clear();

        //        foreach (var chosen_item in sT4D.Items)
        //        {
        //            if (
        //                    chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
        //                &&  chosen_item.Item3 == String.Empty
        //                &&  chosen_item.Item4 == String.Empty
        //                )
        //            {
        //                listBox_Aufträge_in_Maschine.Items.Add(chosen_item.Item1);
        //            }
        //        }
        //    }

        //}

        //private void Button_Von_Verfahren_Zu_Kategorie_Click(object sender, RoutedEventArgs e)
        //{
        //    if (listBox_Verfahren.SelectedIndex != -1
        //       && listBox_Aufträge_in_Verfahren.SelectedIndex != -1
        //       && listBox_Kategorie.SelectedIndex != -1)
        //    {
        //        string auf_odl = listBox_Aufträge_in_Verfahren.SelectedItem.ToString();
        //        string auf_kat = listBox_Kategorie.SelectedItem.ToString();
        //        string auf_mas = listBox_Maschine.SelectedItem.ToString();

        //        //listBox_Aufträge_in_Maschine.Items.Add(auf_odl);
        //        listBox_Aufträge_in_Verfahren.Items.Remove(auf_odl);

        //        sT4D.RemoveByFirst(auf_odl);

        //        sT4D.Add(auf_odl, auf_mas, auf_kat, String.Empty);

        //        //Leert Liste
        //        listBox_Aufträge_in_Kategorie.Items.Clear();

        //        foreach (var chosen_item in sT4D.Items)
        //        {
        //            if (
        //                chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
        //            &&  chosen_item.Item3 == (string)listBox_Kategorie.SelectedItem
        //            &&  chosen_item.Item4 == String.Empty
        //            )
        //            {
        //                listBox_Aufträge_in_Kategorie.Items.Add(chosen_item.Item1);
        //            }
        //        }
        //    }
        //}

        //private void Button_Von_Kategorie_Zu_Verfahren_Click(object sender, RoutedEventArgs e)
        //{
        //    if (listBox_Kategorie.SelectedIndex != -1 && listBox_Aufträge_in_Kategorie.SelectedIndex != -1
        //        && listBox_Verfahren.SelectedIndex != -1)
        //    {
        //        string auf_odl = listBox_Aufträge_in_Kategorie.SelectedItem.ToString();
        //        string auf_mas = listBox_Maschine.SelectedItem.ToString();
        //        string auf_kat = listBox_Kategorie.SelectedItem.ToString();
        //        string auf_ver = listBox_Verfahren.SelectedItem.ToString();


        //        listBox_Aufträge_in_Verfahren.Items.Add(auf_odl);
        //        listBox_Aufträge_in_Kategorie.Items.Remove(auf_odl);

        //        sT4D.RemoveByFirst(auf_odl);
        //        //Fügt Datensatz hinzu
        //        //ODL - Maschine
        //        sT4D.Add(auf_odl, auf_mas, auf_kat, auf_ver);



        //        //Leert Liste
        //        listBox_Aufträge_in_Verfahren.Items.Clear();

        //        foreach (var chosen_item in sT4D.Items)
        //        {
        //            if (   chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
        //                && chosen_item.Item3 == (string)listBox_Kategorie.SelectedItem
        //                && chosen_item.Item4 == (string)listBox_Verfahren.SelectedItem
        //                )
        //            {
        //                listBox_Aufträge_in_Verfahren.Items.Add(chosen_item.Item1);
        //            }
        //        }
        //    }
        //}

        //private void Button_Von_Maschine_Zu_Kategorie_Click(object sender, RoutedEventArgs e)
        //{
        //    if (listBox_Kategorie.SelectedIndex != -1 && listBox_Aufträge_in_Maschine.SelectedIndex != -1)
        //    {                
        //        //Übergibt Daten aus Listboxes
        //        string auf_odl = listBox_Aufträge_in_Maschine.SelectedItem.ToString();
        //        string auf_mas = listBox_Maschine.SelectedItem.ToString();
        //        string auf_kat = listBox_Kategorie.SelectedItem.ToString();

        //        //Löscht Auftrag aus Zuordnungsliste
        //        sT4D.RemoveByFirst(auf_odl);
        //        //Fügt Datensatz hinzu
        //        //ODL - Maschine - Kategorie
        //        sT4D.Add(auf_odl, auf_mas, auf_kat,String.Empty);
        //        //Löscht Eintrag aus Maschinenliste
        //        listBox_Aufträge_in_Maschine.Items.Remove(auf_odl);
        //        //Leert Liste
        //        listBox_Aufträge_in_Kategorie.Items.Clear();

        //        foreach (var chosen_item in sT4D.Items)
        //        {
        //            if (    chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
        //                &&  chosen_item.Item3 == (string)listBox_Kategorie.SelectedItem
        //                &&  chosen_item.Item4 == String.Empty
        //                //&&  chosen_item.Item4 == (string)listBox_Verfahren.SelectedItem
        //                )
        //            {
        //                listBox_Aufträge_in_Kategorie.Items.Add(chosen_item.Item1);
        //            }
        //        }
        //    }
        //}

        //private void Button_Von_Kategorie_Zu_Maschine_Click(object sender, RoutedEventArgs e)
        //{
        //    if (listBox_Maschine.SelectedIndex !=-1 
        //        && listBox_Aufträge_in_Kategorie.SelectedIndex !=-1 
        //        && listBox_Kategorie.SelectedIndex != -1)
        //    {
        //        string auf_odl = listBox_Aufträge_in_Kategorie.SelectedItem.ToString();
        //        string auf_mas = listBox_Maschine.SelectedItem.ToString();

        //        //listBox_Aufträge_in_Maschine.Items.Add(auf_odl);
        //        listBox_Aufträge_in_Kategorie.Items.Remove(auf_odl);

        //        sT4D.RemoveByFirst(auf_odl);

        //        sT4D.Add(auf_odl, auf_mas, String.Empty, String.Empty);

        //        //Leert Liste
        //        listBox_Aufträge_in_Maschine.Items.Clear();

        //        foreach (var chosen_item in sT4D.Items)
        //        {
        //            if (
        //                chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
        //            &&  chosen_item.Item3 == String.Empty
        //            &&  chosen_item.Item4 == String.Empty
        //            )
        //            {
        //                listBox_Aufträge_in_Maschine.Items.Add(chosen_item.Item1);
        //            }
        //        }
        //    }
        //}

        ////ListBox Änderungen
        //private void listBox_Maschine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //Leert Liste
        //    listBox_Aufträge_in_Maschine.Items.Clear();

        //    //Sucht passende Einträge
        //    foreach (var chosen_item in sT4D.Items)
        //    {

        //            if (
        //                chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
        //            &&  chosen_item.Item3 == String.Empty
        //            &&  chosen_item.Item4 == String.Empty
        //            )
        //            {
        //                listBox_Aufträge_in_Maschine.Items.Add(chosen_item.Item1);
        //            }


        //    }

        //    //Sucht Kategorien für Maschine
        //    if (listBox_Maschine.SelectedIndex != -1)
        //    {
        //        Load_Kategorien(listBox_Maschine);
        //        Load_Verfahren(listBox_Maschine);


        //        //Erneuert Aufträge in Kategorie
        //        listBox_Kategorie.SelectedIndex = -1;
        //        listBox_Verfahren.SelectedIndex = -1;
        //        //Leert andere Listen
        //        listBox_Aufträge_in_Verfahren.Items.Clear();
        //        listBox_Aufträge_in_Kategorie.Items.Clear();
        //    }
        //}

        //private void listBox_Kategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //Leert Liste
        //    listBox_Aufträge_in_Kategorie.Items.Clear();
        //    //Leert andere Listen
        //    listBox_Aufträge_in_Verfahren.Items.Clear();
        //    //Sucht passende Einträge
        //    if (listBox_Kategorie.SelectedIndex != -1)
        //    {

        //        foreach (var chosen_item in sT4D.Items)
        //        {
        //            if (
        //                    chosen_item.Item3 == (string)listBox_Kategorie.SelectedItem
        //                &&  chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
        //                &&  chosen_item.Item4 == String.Empty
        //                )
        //            {
        //                listBox_Aufträge_in_Kategorie.Items.Add(chosen_item.Item1);
        //            }
        //        }


        //    }
        //}

        //private void listBox_Verfahren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //Leert Liste
        //    listBox_Aufträge_in_Verfahren.Items.Clear();
        //    // listBox_Aufträge_in_Kategorie.Items.Clear();

        //    //Sucht passende Einträge
        //    if (listBox_Verfahren.SelectedIndex != -1)
        //    {
        //        foreach (var chosen_item in sT4D.Items)
        //        {
        //            if (
        //                    chosen_item.Item2 == (string)listBox_Maschine.SelectedItem
        //                && chosen_item.Item3 == (string)listBox_Kategorie.SelectedItem
        //                && chosen_item.Item4 == (string)listBox_Verfahren.SelectedItem
        //                )
        //            {
        //                listBox_Aufträge_in_Verfahren.Items.Add(chosen_item.Item1);
        //            }
        //        }

        //    }
        //}

        //MouseUp Events
        private void listBox_Aufträge_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (listBox_Aufträge.SelectedIndex != -1)
            {
                //Lade Auftragsdaten aus Datenbank
                Load_Auftragsdaten_From_DB(sender);

                comboBox_Maschine_Einstellungen.IsEnabled = true;
                comboBox_Status_Einstellungen.IsEnabled = true;

                comboBox_Maschine_Load();
                comboBox_Verfahren_Load();

            }
            else
            {
                comboBox_Maschine_Einstellungen.IsEnabled = false;
                comboBox_Status_Einstellungen.IsEnabled = false;
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
                status = Helper.GetComboBoxText(comboBox_Status_Suche);
            }
            catch (Exception)
            {
            }
            try
            {
                kategorie = Helper.GetComboBoxText(comboBox_Kategorien_Suche);
            }
            catch (Exception)
            {
            }
            try
            {
                verfahren = Helper.GetComboBoxText(comboBox_Verfahren_Suche);
            }
            catch (Exception)
            {
            }
            try
            {
                maschine = Helper.GetComboBoxText(comboBox_Maschine_Suche);
            }
            catch (Exception)
            {
            }

            var query =
                from a in d.Auftrag

                join z in d.Auftrags_Zuordnung on a.Id equals z.Id_Auftrag
                into zGroup
                from zG in zGroup.DefaultIfEmpty()

                join n in d.MN_Auftrag_Kategorie on zG.Id equals n.Id_Auftrags_Zuorndung
                into nGroup
                from nG in nGroup.DefaultIfEmpty()

                join k in d.Kategorie on nG.Id_Kategorie equals k.Id
                into kGroup
                from kG in kGroup.DefaultIfEmpty()

                join m in d.Maschine on zG.Id_Maschine equals m.Id
                into mGroup
                from mG in mGroup.DefaultIfEmpty()

                join v in d.Verfahren on zG.Id_Verfahren equals v.Id
                into vGroup
                from vG in vGroup.DefaultIfEmpty()

                    // where kG.Name == kategorie && a.Status == status && vG.Name == verfahren && mG.Name == maschine

                select new { a.ODL, a.Status, Kategorie = kG.Name, Verfahren = vG.Name, Maschine = mG.Name };

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


            // MessageBox.Show(""+ query.Count());
            try
            {
                lBox.Items.Clear();


                foreach (var item in query)
                {
                    // auftrag_ODL_List.Add(item.ODL);
                    lBox.Items.Add(item.ODL);
                }
                //Load_Maschinen();
            }
            catch (Exception)
            {
            }


        }


        private void Load_Auftragsdaten_From_DB(object sender)
        {
            //Lädt Daten in den Detailbereich
            if (((ListBox)sender).SelectedIndex != -1)
            {
                ODL = (string)((ListBox)sender).SelectedItem;
                frame.Refresh();

                DDataContext d = new DDataContext();

                var auf = from x in d.Auftrag
                          where x.ODL == ODL
                          select x;
                if (auf.Count() > 0)
                {
                    foreach (ComboBoxItem cItem in comboBox_Status_Einstellungen.Items)
                    {
                        if (cItem.Content.ToString() == auf.First().Status)
                        {
                            comboBox_Status_Einstellungen.SelectedItem = cItem;
                            break;
                        }
                    }
                    var azu = from y in d.Auftrags_Zuordnung
                              where y.Id_Auftrag == auf.First().Id
                              select y;
                    if (azu.Count() > 0)
                    {
                        var vef = from z in d.Verfahren
                                  where z.Id == azu.First().Id_Verfahren
                                  select z;
                        var mas = from g in d.Maschine
                                  where g.Id == azu.First().Id_Maschine
                                  select g;
                        if (vef.Count() > 0)
                        {
                            foreach (ComboBoxItem cItem in comboBox_Verfahren_Einstellungen.Items)
                            {
                                if (cItem.Content.ToString() == vef.First().Name)
                                {
                                    comboBox_Verfahren_Einstellungen.SelectedItem = cItem;
                                    break;
                                }
                            }
                        }

                        if (mas.Count() > 0)
                        {
                            foreach (ComboBoxItem cItem in comboBox_Maschine_Einstellungen.Items)
                            {
                                if (cItem.Content.ToString() == mas.First().Name)
                                {
                                    comboBox_Maschine_Einstellungen.SelectedItem = cItem;
                                    break;
                                }
                            }
                        }

                    }
                }



            }
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

                        comboBox_Verfahren_Einstellungen.IsEnabled = true;

                        DDataContext d = new DDataContext();
                        var auf = from x in d.Auftrag
                                  where x.ODL == ODL
                                  select x;
                        var azu = from y in d.Auftrags_Zuordnung
                                  where y.Id_Auftrag == auf.First().Id
                                  select y;
                        if (azu.Count() > 0)
                        {
                            azu.First().Id_Maschine = (from n in d.Maschine where n.Name == Helper.GetComboBoxText(comboBox_Maschine_Einstellungen) select n).First().Id;
                        }
                        else
                        {
                            Auftrags_Zuordnung a = new Auftrags_Zuordnung()
                            {
                                Id_Maschine =
                                (from n in d.Maschine where n.Name == Helper.GetComboBoxText(comboBox_Maschine_Einstellungen) select n).First().Id
                            };
                            d.Auftrags_Zuordnung.InsertOnSubmit(a);
                        }

                        try
                        {
                            d.SubmitChanges();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    //ComboBox Maschine Einstellungen - Standardwert gewählt?
                    //Ja          
                    else
                    {
                        // comboBox_Verfahren_Load();
                        comboBox_Verfahren_Einstellungen.IsEnabled = false;
                    }
                }
                //ComboBox Maschine Einstellungen - Nichts gewählt?
                //Nichts
                else
                {
                    comboBox_Maschine_Einstellungen.SelectedIndex = 0;
                    //comboBox_Verfahren_Einstellungen.SelectedIndex = 0;

                }

            }
            catch (Exception)
            {

            }

        }

        private void comboBox_Verfahren_Einstellungen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex != 0 && ((ComboBox)sender).SelectedIndex != -1)
            {
                DDataContext d = new DDataContext();
                var auf = from x in d.Auftrag
                          where x.ODL == ODL
                          select x;
                var azu = from y in d.Auftrags_Zuordnung
                          where y.Id_Auftrag == auf.First().Id
                          select y;
                if (azu.Count() > 0)
                {
                    azu.First().Id_Verfahren = (from n in d.Verfahren where n.Name == Helper.GetComboBoxText(comboBox_Verfahren_Einstellungen) select n).First().Id;
                }
                else
                {
                    Auftrags_Zuordnung a = new Auftrags_Zuordnung()
                    {
                        Id_Maschine =
                        (from n in d.Maschine where n.Name == Helper.GetComboBoxText(comboBox_Maschine_Einstellungen) select n).First().Id
                    };
                    d.Auftrags_Zuordnung.InsertOnSubmit(a);
                }

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                }
            }
        }

        private void comboBox_Status_Einstellungen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex != 0 && ((ComboBox)sender).SelectedIndex != -1)
            {
                //Erstelle Datenbankverbindung
                DDataContext d = new DDataContext();
                var auf = from x in d.Auftrag
                          where x.ODL == ODL
                          select x;
                auf.First().Status = Helper.GetComboBoxText(comboBox_Status_Einstellungen);

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                }
            }

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
                //Erstelle Datenbankverbindung
                DDataContext d = new DDataContext();
                //Fülle Verfahren
                var ver = from y in d.Verfahren
                          where y.Maschine == null || y.Id_Maschine == (int?)(from b in d.Maschine where b.Name == Helper.GetComboBoxText(comboBox_Maschine_Einstellungen) select b).First().Id
                          select y;
                comboBox_Verfahren_Einstellungen.Items.Clear();
                comboBox_Verfahren_Einstellungen.Items.Add("Verfahren");
                foreach (var item in ver)
                {
                    comboBox_Verfahren_Einstellungen.Items.Add(item.Name);
                }
            }
            catch (Exception)
            {
            }

        }

        //public void RefreshValues() {
        //    Load_ODLs(listBox_Aufträge);
        //    Load_Maschinen(listBox_Maschine);
        //    Load_Kategorien(listBox_Maschine);
        //}

    }
}
