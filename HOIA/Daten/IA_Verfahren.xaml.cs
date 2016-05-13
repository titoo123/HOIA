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

namespace HOIA.Daten
{
    /// <summary>
    /// Interaktionslogik für IA_Verfahren.xaml
    /// </summary>
    public partial class IA_Verfahren : Page
    {
        bool neu;
        public IA_Verfahren()
        {
            InitializeComponent();
            Datagrid_IA_Werte_Refresh();
        }
       
        private void Datagrid_IA_Werte_Refresh()
        {
            DDataContext d = new DDataContext();
            var zuo = from r in d.Verfahren
                      where r.Art == Erweiterungen.Helper.INDUKTIONSANLAGEN_ART
                      select new { r.Id, r.Name };
            dataGrid_hoWerte.ItemsSource = zuo;
        }

        private void button_Neu_Name_Click(object sender, RoutedEventArgs e)
        {
            neu = true;
            textBox_Name.IsEnabled = true;
            textBox_Name.Text = String.Empty;

            button_Speichern_Name.IsEnabled = true;
        }

        private void button_Bearbeiten_Name_Click(object sender, RoutedEventArgs e)
        {
            textBox_Name.IsEnabled = true;
            button_Speichern_Name.IsEnabled = true;
            neu = false;


        }

        private void button_Speichern_Name_Click(object sender, RoutedEventArgs e)
        {
            DDataContext d = new DDataContext();

            if (neu)
            {
                Verfahren s = new Verfahren() { Art = Erweiterungen.Helper.INDUKTIONSANLAGEN_ART, Name = textBox_Name.Text };
                d.Verfahren.InsertOnSubmit(s);
            }
            else
            {
                var k = from t in d.Verfahren
                        where t.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_hoWerte)
                        select t;
                k.First().Name = textBox_Name.Text;


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

            textBox_Name.IsEnabled = false;
            textBox_Name.Text = String.Empty;
            Datagrid_IA_Werte_Refresh();
        }

        private void button_Löschen_Name_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Soll dieses Verfahren wirklich gelöscht werden?", "Echt?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DDataContext d = new DDataContext();
                if (!neu)
                {
                    var k = from t in d.Verfahren
                            where t.Id == Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_hoWerte)
                            select t;
                    d.Verfahren.DeleteAllOnSubmit(k);
                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Datenübermittlung fehlgeschlagen!", "Nee!!!");
                    }
                }
            }



            Datagrid_IA_Werte_Refresh();
        }

        private void dataGrid_hoWerte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBox_Name.Text = String.Empty;

            if (dataGrid_hoWerte.SelectedIndex != -1)
            {
                textBox_Name.Text = Erweiterungen.Helper.GetStringFromDataGrid(1, dataGrid_hoWerte);
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
