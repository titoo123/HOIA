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
    /// Interaktionslogik für Verfahren_Window.xaml
    /// </summary>
    public partial class Verfahren_Window : Window
    {
        bool v_neu;
        private Verwaltung_Aufträge verwaltung_Aufträge;

        public Verfahren_Window(Verwaltung_Aufträge verwaltung_Aufträge)
        {
            InitializeComponent();
            this.verwaltung_Aufträge = verwaltung_Aufträge;
            RefreshDataGrid();
        }

        private void RefreshDataGrid() {

            //DDataContext d = new DDataContext();

            //var ver = from v in d.Verfahren
            //          select new { v.Id, v.Name, MaschinenArt = v.Maschinenart.Name};
            //dataGrid_Verfahren.ItemsSource = ver;

            //if (comboBox_Maschinenart.Items.Count < 1)
            //{
            //    List<string> m_List = new List<string>();
            //    var mas = (from m in d.Maschinenart
            //              select new { m.Name });
            //    foreach (var i in mas)
            //    {
            //        m_List.Add(i.Name);
            //    }
            //    comboBox_Maschinenart.ItemsSource = m_List;
            //}
        }
        private void button_Neu_Name_Click(object sender, RoutedEventArgs e)
        {
            textBox_Verfahren_Name.IsEnabled = true;
            comboBox_Maschinenart.IsEnabled = true;
            textBox_Verfahren_Beschreibung.IsEnabled = true;

            textBox_Verfahren_Name.Text = String.Empty;
            textBox_Verfahren_Name.Text = String.Empty;
            comboBox_Maschinenart.SelectedIndex = -1;
            button_Verfahren_Speichern.IsEnabled = true;
            v_neu = true;
        }
        private void button_Bearbeiten_Name_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid_Verfahren.SelectedIndex != -1)
            {
                textBox_Verfahren_Name.IsEnabled = true;
                comboBox_Maschinenart.IsEnabled = true;
                textBox_Verfahren_Beschreibung.IsEnabled = true;

                button_Verfahren_Speichern.IsEnabled = true;
                button_Verfahren_Löschen.IsEnabled = true;

                v_neu = false;
            }
            else
            {
                MessageBox.Show("Bitte wählen sie ein Verfahren aus der Liste!","Achtung!");
            }
        }
        private void button_Speichern_Name_Click(object sender, RoutedEventArgs e)
        {
            //if (v_neu)
            //{

            //    if (comboBox_Maschinenart.SelectedIndex != -1)
            //    {
            //        DDataContext d = new DDataContext();
            //        int? ma_id = (from i in d.Maschinenart
            //                      where i.Name == comboBox_Maschinenart.SelectedItem.ToString()
            //                      select i).First().Id;

            //        Verfahren nver = new Verfahren()
            //        {
            //            Name = textBox_Verfahren_Name.Text,
            //            Beschreibung = textBox_Verfahren_Beschreibung.Text,
            //            Id_Maschinenart = ma_id
            //        };
            //        d.Verfahren.InsertOnSubmit(nver);

            //        try
            //        {
            //            d.SubmitChanges();
            //        }
            //        catch (Exception)
            //        {
            //            MessageBox.Show("Keine Verbindung zur Datenbank!", "Fehler!");
            //        }


            //        textBox_Verfahren_Name.IsEnabled = false;
            //        textBox_Verfahren_Beschreibung.IsEnabled = false;
            //        comboBox_Maschinenart.IsEnabled = false;
            //        button_Verfahren_Bearbeiten.IsEnabled = false;
            //        button_Teilschritt_Speichern.IsEnabled = false;
            //        button_Verfahren_Löschen.IsEnabled = false;

            //        textBox_Verfahren_Name.Text = String.Empty;
            //        textBox_Verfahren_Beschreibung.Text = String.Empty;
            //        comboBox_Maschinenart.SelectedIndex = -1;

            //    }
            //    else
            //    {
            //        MessageBox.Show("Bitte wählen sie die Art der Maschine aus!", "Achtung!");
            //    }
              
            //}
            //else
            //{
            //    if (comboBox_Maschinenart.SelectedIndex != -1)
            //    {
            //        DDataContext d = new DDataContext();

            //        Verfahren ver = (from v in d.Verfahren
            //                         where v.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_Verfahren)
            //                         select v).First();
            //        ver.Name = textBox_Verfahren_Name.Text;
            //        ver.Beschreibung = textBox_Verfahren_Beschreibung.Text;

            //        //ComboBoxItem c = comboBox_Maschinenart.SelectedItem.ToString() as ComboBoxItem;
            //        int? ma_id = (from i in d.Maschinenart
            //                      where i.Name == comboBox_Maschinenart.SelectedItem.ToString()
            //                      select i).First().Id;
            //        if (ma_id != null)
            //        {
            //            ver.Id_Maschinenart = ma_id;

            //            try
            //            {
            //                d.SubmitChanges();
            //            }
            //            catch (Exception)
            //            {
            //                MessageBox.Show("Keine Verbindung zur Datenbank!", "Fehler!");
            //            }

            //            textBox_Verfahren_Name.IsEnabled = false;
            //            textBox_Verfahren_Beschreibung.IsEnabled = false;
            //            comboBox_Maschinenart.IsEnabled = false;
            //            button_Verfahren_Bearbeiten.IsEnabled = false;
            //            button_Teilschritt_Speichern.IsEnabled = false;
            //            button_Verfahren_Löschen.IsEnabled = false;

            //            textBox_Verfahren_Name.Text = String.Empty;
            //            textBox_Verfahren_Beschreibung.Text = String.Empty;
            //            comboBox_Maschinenart.SelectedIndex = -1;
            //        }
            //        else
            //        {
            //            MessageBox.Show("Art der Maschine nicht erkannt!", "Fehler!");
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Bitte wählen sie die Art der Maschine aus!", "Achtung!");
            //    }

               

            //}
            //RefreshDataGrid();
            //verwaltung_Aufträge.RefreshValues();
        }
        private void button_Löschen_Name_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid_Verfahren.SelectedIndex != -1)
            {
                DDataContext d = new DDataContext();
                Verfahren ver = (from l in d.Verfahren
                          where l.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_Verfahren)
                          select l).First();
                d.Verfahren.DeleteOnSubmit(ver);

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Keine Verbindung zur Datenbank!", "Fehler!");
                }

            }
            else
            {
                MessageBox.Show("Bitte wählen sie ein zu löschendes Verfahren aus!","Achtung!");
            }
            
            RefreshDataGrid();
            //verwaltung_Aufträge.RefreshValues();
        }
        private void dataGrid_Verfahren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //    if (dataGrid_Verfahren.SelectedIndex != -1)
            //    {
            //        DDataContext d = new DDataContext();

            //        var v = (from f in d.Verfahren
            //                 where f.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_Verfahren)
            //                 select new { f.Name, f.Beschreibung, MaschinenArt = f.Maschinenart.Name }).First();

            //        textBox_Verfahren_Name.Text = v.Name;
            //        textBox_Verfahren_Beschreibung.Text = v.Beschreibung;
            //        if (v.MaschinenArt != null)
            //        {
            //            foreach (var c in comboBox_Maschinenart.Items)
            //            {
            //                if (c.ToString() == v.MaschinenArt)
            //                {
            //                    comboBox_Maschinenart.SelectedItem = c;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            comboBox_Maschinenart.SelectedIndex = -1;
            //        }
            //        button_Verfahren_Bearbeiten.IsEnabled = true;
            //    }
            //    else
            //    {

            //    }


        }
    }
}
