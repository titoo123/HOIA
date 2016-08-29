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

namespace HOIA.Haubenofen
{
    /// <summary>
    /// Interaktionslogik für TVorlagen.xaml
    /// </summary>
    public partial class TVorlagen : Page
    {
        bool tv_neu;
        bool ts_neu;
        public TVorlagen()
        {
            InitializeComponent();

            if (comboBox_Funktion.Items.Count == 0)
            {
                List<String> funktionen = new List<string>(new String[] { "Kühlen", "Warten", "Rezirkulierung", "50 Prozent", "Kühlkl.", "" });

                comboBox_Funktion.ItemsSource = funktionen;
            }
            treeView_TVorlagen_Load();
            button_TV_Neu.IsEnabled = true;

        }

        private void treeView_TVorlagen_Load()
        {
            //DDataContext d = new DDataContext();
            //var glv = from l in d.Verfahren
            //          where l.Art == Erweiterungen.Helper.HAUBENOFEN_ART
            //          select new { l.Name };

            //List<TreeViewItem> t = new List<TreeViewItem>();
            //foreach (var item in glv)
            //{
            //    t.Add(new TreeViewItem() { Header = item.Name });
            //}
            //treeView_TVorlagen.ItemsSource = t;
        }
        private void dataGrid_Funktionen_Load(Verfahren v)
        {
            DDataContext d = new DDataContext();

            var tie = from m in d.Teilschritt
                      where m.Id_Verfahren == v.Id
                      select new { m.Id, m.Zieltemperatur, m.Delta, m.Funktion };

            dataGrid_Funktionen.ItemsSource = tie;
        }
        private void treeView_TVorlagen_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            button_TV_Bearbeiten.IsEnabled = false;
            button_TV_Löschen.IsEnabled = false;
            button_TV_Speichern.IsEnabled = false;

            if (treeView_TVorlagen.SelectedValue != null)
            {
                TreeViewItem i = (TreeViewItem)treeView_TVorlagen.SelectedItem;

                DDataContext d = new DDataContext();
                var sel = from c in d.Verfahren
                          where c.Name == (String)i.Header
                          select c;

                if (sel.Count() > 0)
                {
                    Verfahren v = sel.First();

                    textBox_Name.Text = v.Name;
                    textBox_Beschreibung.Text = v.Beschreibung;

                    dataGrid_Funktionen_Load(v);

                    button_Funktion_Neu.IsEnabled = true;
                    button_TV_Löschen.IsEnabled = true;
                    button_TV_Bearbeiten.IsEnabled = true;
                    dataGrid_Funktionen.IsEnabled = true;

                    button_TV_Speichern.IsEnabled = false;
                    textBox_Beschreibung.IsEnabled = false;
                    textBox_Name.IsEnabled = false;

                }

            }
            else
            {
                MessageBox.Show("Kein gültiges Objekt ausgewählt!", "Nee!");
            }
        }

        private void button_TV_Neu_Click(object sender, RoutedEventArgs e)
        {
            //Technologievorlage
            tv_neu = true;
            button_TV_Bearbeiten_Click(sender, e);

            textBox_Name.Text = String.Empty;
            textBox_Beschreibung.Text = String.Empty;
            //Teilschritte
            dataGrid_Funktionen.ItemsSource = null;
            dataGrid_Funktionen.IsEnabled = true;


        }

        private void button_TV_Bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            button_TV_Speichern.IsEnabled = true;
            textBox_Beschreibung.IsEnabled = true;
            textBox_Name.IsEnabled = true;
        }

        private void button_TV_Speichern_Click(object sender, RoutedEventArgs e)
        {
            //DDataContext d = new DDataContext();
            //var sol = from l in d.Verfahren
            //          where l.Name == textBox_Name.Text
            //          select l;

            //if (sol.Count() == 0 || !tv_neu)
            //{

            //    if (tv_neu)
            //    {
            //        Verfahren v = new Verfahren() { Name = textBox_Name.Text, Beschreibung = textBox_Beschreibung.Text, Art = Erweiterungen.Helper.HAUBENOFEN_ART };

            //        d.Verfahren.InsertOnSubmit(v);

            //    }
            //    else
            //    {
            //        var sel = from j in d.Verfahren
            //                  where j.Name == textBox_Name.Text
            //                  select j;

            //        if (sel.Count() > 0)
            //        {
            //            Verfahren v = sel.First();
            //            v.Name = textBox_Name.Text;
            //            v.Beschreibung = textBox_Beschreibung.Text;

            //        }
            //    }

            //    try
            //    {
            //        d.SubmitChanges();
            //    }
            //    catch (Exception)
            //    {
            //        MessageBox.Show("Keine Verbindung zur Datenbank!", "Nee!");
            //    }


            //    treeView_TVorlagen_Load();

            //    button_TV_Speichern.IsEnabled = false;
            //    button_TV_Bearbeiten.IsEnabled = false;
            //    button_TV_Löschen.IsEnabled = false;

            //    textBox_Beschreibung.IsEnabled = false;
            //    textBox_Name.IsEnabled = false;
            //    tv_neu = false;
            //}
            //else
            //{
            //    MessageBox.Show("Technologie Name bereits vergeben!", "Nee!");
            //}

        }

        private void button_TV_Löschen_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Wollen sie diese Technologie Vorlage wirklich löschen?", "Wirklich?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DDataContext d = new DDataContext();

                var sel = from j in d.Verfahren
                          where j.Name == textBox_Name.Text
                          select j;

                if (sel.Count() > 0)
                {
                    var fdg = from l in d.Teilschritt
                              where l.Id_Verfahren == sel.First().Id
                              select l;
                    d.Teilschritt.DeleteAllOnSubmit(fdg);

                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Keine Verbindung zur Datenbank!", "Nee!");
                    }
                    dataGrid_Funktionen_Load(sel.First());
                }
                
                d.Verfahren.DeleteAllOnSubmit(sel);

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Keine Verbindung zur Datenbank!", "Nee!");
                }
                treeView_TVorlagen_Load();

                button_Funktion_Speichern.IsEnabled = false;

                textBox_Zieltemperatur.Text = String.Empty;
                textBox_Delta.Text = String.Empty;

                comboBox_Funktion.SelectedItem = "";
            }
        }

        private void dataGrid_Funktionen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_Funktionen.SelectedItem != null)
            {
                button_Funktion_Bearbeiten.IsEnabled = true;
                button_Funktion_Löschen.IsEnabled = true;

                textBox_Zieltemperatur.Text = Erweiterungen.Helper.GetStringFromDataGrid(1, dataGrid_Funktionen);
                textBox_Delta.Text = Erweiterungen.Helper.GetStringFromDataGrid(2, dataGrid_Funktionen);
                comboBox_Funktion.SelectedValue = Erweiterungen.Helper.GetStringFromDataGrid(3, dataGrid_Funktionen);
            }




        }

        private void button_Funktion_Bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            textBox_Delta.IsEnabled = true;
            textBox_Zieltemperatur.IsEnabled = true;
            comboBox_Funktion.IsEnabled = true;

            button_Funktion_Speichern.IsEnabled = true;
        }

        private void button_Funktion_Speichern_Click(object sender, RoutedEventArgs e)
        {
            int temp = 0;
            int delta = 0;

            if (Int32.TryParse(textBox_Zieltemperatur.Text,out temp) && Int32.TryParse( textBox_Delta.Text, out delta))
            {
                DDataContext d = new DDataContext();

                var ftz = from b in d.Verfahren
                          where b.Name == textBox_Name.Text
                          select b;

                if (ts_neu)
                {
                    Teilschritt ts = new Teilschritt()
                    {
                        Delta = delta,
                        Funktion = (String)comboBox_Funktion.SelectedItem,
                        Zieltemperatur = temp,
                        Id_Verfahren = ftz.First().Id
                    };

                    d.Teilschritt.InsertOnSubmit(ts);
                }
                else
                {
                    var dfg = from r in d.Teilschritt
                              where r.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_Funktionen)
                              select r;

                    Teilschritt t = dfg.First();
                    t.Delta = delta;
                    t.Zieltemperatur = temp;
                    t.Funktion = (String)comboBox_Funktion.SelectedValue;
                }

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Keine Verbindung zur Datenbank!", "Nee!");
                }
                dataGrid_Funktionen_Load(ftz.First());

                textBox_Zieltemperatur.IsEnabled = false;
                textBox_Delta.IsEnabled = false;
                comboBox_Funktion.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Bitte überprüfen sie ihre Eingaben!", "Nee!");
            }
        }

        private void button_Funktion_Löschen_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Wollen sie diesen Teilschritt wirklich löschen?", "Wirklich?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DDataContext d = new DDataContext();

                var sel = from j in d.Teilschritt
                          where j.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_Funktionen)
                          select j;

                d.Teilschritt.DeleteAllOnSubmit(sel);

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Keine Verbindung zur Datenbank!", "Nee!");
                }
                var ftz = from b in d.Verfahren
                          where b.Name == textBox_Name.Text
                          select b;
                dataGrid_Funktionen_Load(ftz.First());

                textBox_Zieltemperatur.IsEnabled = false;
                textBox_Delta.IsEnabled = false;
                comboBox_Funktion.IsEnabled = false;

                button_Funktion_Löschen.IsEnabled = false;
                button_Funktion_Speichern.IsEnabled = false;
                button_Funktion_Bearbeiten.IsEnabled = false;

                textBox_Delta.Text = String.Empty;
                textBox_Zieltemperatur.Text = String.Empty;

            }
        }

        private void button_Funktion_Neu_Click(object sender, RoutedEventArgs e)
        {
            ts_neu = true;
            button_Funktion_Bearbeiten_Click(sender, e);

            textBox_Zieltemperatur.Text = String.Empty;
            textBox_Delta.Text = String.Empty;
            comboBox_Funktion.SelectedItem = "";
        }

        private void dataGrid_Funktionen_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime) || e.Column.Header.ToString() == "Datum")
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";

            if (e.Column.Header.ToString() == "Id" || e.Column.Header.ToString() == "Matten")
            {
                e.Column.MinWidth = 0;
                e.Column.Width = 0;
            }


        }
    
    }
}
