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
using System.Windows.Shapes;
using HOIA.Allgemein;

namespace HOIA.Daten
{
    /// <summary>
    /// Interaktionslogik für Maschinen_Window.xaml
    /// </summary>
    public partial class Maschinen_Window : Window
    {
        bool neu;
        private Verwaltung_Aufträge verwaltung_Aufträge;

        public Maschinen_Window()
        {
            InitializeComponent();
            Datagrid_Maschinen_Refresh();

            if (comboBox_Maschinen_Art.Items.Count < 1)
            {

                comboBox_Maschinen_Art.Items.Add(new ComboBoxItem() { Content = "Art" });
                comboBox_Maschinen_Art.SelectedIndex= 0;
                DDataContext d = new DDataContext();

                var k = from t in d.Maschinenart
                        select new { t.Name };
                foreach (var i in k)
                {
                    comboBox_Maschinen_Art.Items.Add(new ComboBoxItem() { Content = i.Name });
                }
               
            }

        }
        public Maschinen_Window(Verwaltung_Aufträge verwaltung_Aufträge)
        {
            InitializeComponent();
            Datagrid_Maschinen_Refresh();

            if (comboBox_Maschinen_Art.Items.Count < 1)
            {

                comboBox_Maschinen_Art.Items.Add(new ComboBoxItem() { Content = "Art" });
                comboBox_Maschinen_Art.SelectedIndex = 0;
                DDataContext d = new DDataContext();

                var k = from t in d.Maschinenart
                        select new { t.Name };
                foreach (var i in k)
                {
                    comboBox_Maschinen_Art.Items.Add(new ComboBoxItem() { Content = i.Name });
                }

            }
            this.verwaltung_Aufträge = verwaltung_Aufträge;
        }

        private void button_Neu_Name_Click(object sender, RoutedEventArgs e)
        {
            neu = true;
            textBox_Name.IsEnabled = true;

            textBox_Name.Text = String.Empty;
            comboBox_Maschinen_Art.IsEnabled = true;
            button_Speichern_Name.IsEnabled = true;
        }
        private void button_Bearbeiten_Name_Click(object sender, RoutedEventArgs e)
        {
            textBox_Name.IsEnabled = true;
            button_Speichern_Name.IsEnabled = true;
            comboBox_Maschinen_Art.IsEnabled = true;
            neu = false;
           

        }
        private void button_Speichern_Name_Click(object sender, RoutedEventArgs e)
        {
            string m_art = ((ComboBoxItem)comboBox_Maschinen_Art.SelectedItem).Content.ToString();
            if ( m_art != "Art")
            {
                DDataContext d = new DDataContext();

                int? a = 
                    (from m in d.Maschinenart
                        where m.Name == m_art
                        select new { m.Id }).First().Id;

                if (neu)
                {
                    Maschine s = new Maschine() { Name = textBox_Name.Text, Id_Maschinenart = a };
                    d.Maschine.InsertOnSubmit(s);
                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Datenübermittlung fehlgeschlagen!", "Nee!!!");
                    }
                    //Fügt jeder neuen Maschine automatisch eine Standard-Kategorie hinzu
                    int? mid = (from x in d.Maschine
                                where x.Name == textBox_Name.Text && x.Id_Maschinenart == a
                                select x).First().Id;

                    d.Kategorie.InsertOnSubmit(new Kategorie { Name = "Standard", Id_Maschine = (int)mid });
                }
                else
                {
                    var k = from t in d.Maschine
                            where t.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_maschinen)
                            select t;
                    k.First().Name = textBox_Name.Text;
                    k.First().Id_Maschinenart = a;


                }
                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Datenübermittlung fehlgeschlagen!", "Nee!!!");
                }
                button_Bearbeiten_Name.IsEnabled = false;
                button_Speichern_Name.IsEnabled = false;
                button_Löschen_Name.IsEnabled = false;

                comboBox_Maschinen_Art.IsEnabled = false;

                textBox_Name.IsEnabled = false;
                textBox_Name.Text = String.Empty;
                Datagrid_Maschinen_Refresh();

            }
            else if (textBox_Name.Text.Length < 1)
            {
                MessageBox.Show("Bitte geben sie einen Namen ein!", "Achtung!");
            }
            else
            {
                MessageBox.Show("Bitte wählen sie eine Art von Maschine aus!","Achtung!");
            }
            //verwaltung_Aufträge.RefreshValues();
           
        }
        private void button_Löschen_Name_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Soll diese Maschine wirklich gelöscht werden?", "Echt?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DDataContext d = new DDataContext();
                if (!neu)
                {
                    var l = from f in d.Kategorie
                            where f.Id_Maschine == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_maschinen)
                            select f;
                    d.Kategorie.DeleteAllOnSubmit(l);
                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Datenübermittlung fehlgeschlagen!", "Nee!!!");
                    }

                    var k = from t in d.Maschine
                            where t.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_maschinen)
                            select t;
                    d.Maschine.DeleteAllOnSubmit(k);
                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Datenübermittlung fehlgeschlagen!", "Nee!!!");
                    }
                }
                button_Bearbeiten_Name.IsEnabled = false;
                button_Speichern_Name.IsEnabled = false;
                button_Löschen_Name.IsEnabled = false;

                comboBox_Maschinen_Art.IsEnabled = false;

                textBox_Name.IsEnabled = false;
                textBox_Name.Text = String.Empty;
            }



            Datagrid_Maschinen_Refresh();
            //verwaltung_Aufträge.RefreshValues();
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBox_Name.Text = String.Empty;

            if (dataGrid_maschinen.SelectedIndex != -1)
            {
                textBox_Name.Text = Erweiterungen.Helper.GetStringFromDataGrid(1, dataGrid_maschinen);
                button_Bearbeiten_Name.IsEnabled = true;
                button_Löschen_Name.IsEnabled = true;

            }
            else
            {
                button_Bearbeiten_Name.IsEnabled = false;
            }
        }
        private void Datagrid_Maschinen_Refresh()
        {
            DDataContext d = new DDataContext();
            var zuo = from r in d.Maschine
                      select new { r.Id, r.Name, Art = r.Maschinenart.Name };
            dataGrid_maschinen.ItemsSource = zuo;
        }
    }
}
