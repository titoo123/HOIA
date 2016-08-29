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

namespace HOIA.Daten
{
    /// <summary>
    /// Interaktionslogik für Kategorie_Window.xaml
    /// </summary>
    public partial class Kategorie_Window : Window
    {
        bool neu;
        public Kategorie_Window()
        {
            InitializeComponent();
            if (comboBox_Maschine.Items.Count < 1)
            {

                comboBox_Maschine.Items.Add(new ComboBoxItem() { Content = "Maschine" });
                comboBox_Maschine.SelectedIndex = 0;
                DDataContext d = new DDataContext();

                var k = from t in d.Maschine
                        select new { t.Name };
                foreach (var i in k)
                {
                    comboBox_Maschine.Items.Add(new ComboBoxItem() { Content = i.Name });
                }
            }
        }
        public Kategorie_Window(string maschine)
        {
            InitializeComponent();
            if (comboBox_Maschine.Items.Count < 1)
            {

                comboBox_Maschine.Items.Add(new ComboBoxItem() { Content = "Maschine" });
                comboBox_Maschine.SelectedIndex = 0;
                DDataContext d = new DDataContext();

                var k = from t in d.Maschine
                        select new { t.Name };
                foreach (var i in k)
                {
                    comboBox_Maschine.Items.Add(new ComboBoxItem() { Content = i.Name });
                }

                if (maschine.Length > 0)
                {
                        foreach (var c in comboBox_Maschine.Items)
                        {
                            if (((ComboBoxItem)c).Content.ToString() == maschine)
                            {
                                comboBox_Maschine.SelectedItem = c;
                            }
                        }
                }
     
            }
        }

        private void button_Neu_Name_Click(object sender, RoutedEventArgs e)
        {
            neu = true;
            textBox_Name.IsEnabled = true;

            textBox_Name.Text = String.Empty;
            comboBox_Maschine.IsEnabled = true;
            button_Speichern_Name.IsEnabled = true;
        }

        private void button_Bearbeiten_Name_Click(object sender, RoutedEventArgs e)
        {
            textBox_Name.IsEnabled = true;
            button_Speichern_Name.IsEnabled = true;
            comboBox_Maschine.IsEnabled = true;
            neu = false;
        }

        private void button_Speichern_Name_Click(object sender, RoutedEventArgs e)
        {
            string m_art = ((ComboBoxItem)comboBox_Maschine.SelectedItem).Content.ToString();
            if (m_art != "Maschine")
            {
                DDataContext d = new DDataContext();

                int a =
                    (from m in d.Maschine
                     where m.Name == m_art
                     select new { m.Id }).First().Id;

                if (neu)
                {
                    Kategorie s = new Kategorie() { Name = textBox_Name.Text, Id_Maschine = a };
                    d.Kategorie.InsertOnSubmit(s);
                }
                else
                {
                    var k = from t in d.Kategorie
                            where t.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_Kategorien)
                            select t;
                    k.First().Name = textBox_Name.Text;
                    k.First().Id_Maschine = a;


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

                //comboBox_Maschine.IsEnabled = false;

                textBox_Name.IsEnabled = false;
                textBox_Name.Text = String.Empty;
                Datagrid_Kategorien_Refresh();
            }
            else if (textBox_Name.Text.Length < 1)
            {
                MessageBox.Show("Bitte geben sie einen Namen ein!", "Achtung!");
            }
            else
            {
                MessageBox.Show("Bitte wählen sie eine Art von Maschine aus!", "Achtung!");
            }

        }

        private void button_Löschen_Name_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Soll diese Maschine wirklich gelöscht werden?", "Echt?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DDataContext d = new DDataContext();
                if (!neu)
                {
                    var k = from t in d.Maschine
                            where t.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_Kategorien)
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

                //comboBox_Maschine.IsEnabled = false;

                textBox_Name.IsEnabled = false;
                textBox_Name.Text = String.Empty;
            }



            Datagrid_Kategorien_Refresh();
        }

        private void comboBox_Maschine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBox_Name.Text = String.Empty;
            
            Datagrid_Kategorien_Refresh();
           
        }
        private void Datagrid_Kategorien_Refresh()
        {
            DDataContext d = new DDataContext();
            var zuo = from r in d.Kategorie
                      where r.Maschine.Name == ((ComboBoxItem)comboBox_Maschine.SelectedItem).Content.ToString()
                      select new { r.Id, r.Name };
            dataGrid_Kategorien.ItemsSource = zuo;
        }

        private void dataGrid_Kategorien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBox_Name.Text = String.Empty;

            if (dataGrid_Kategorien.SelectedIndex != -1)
            {
                textBox_Name.Text = Erweiterungen.Helper.GetStringFromDataGrid(1, dataGrid_Kategorien);
                button_Bearbeiten_Name.IsEnabled = true;
                button_Löschen_Name.IsEnabled = true;

            }
            else
            {
                button_Bearbeiten_Name.IsEnabled = false;
            }
        }
    }
}
