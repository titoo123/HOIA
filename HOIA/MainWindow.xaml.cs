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

namespace HOIA
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Erweiterungen.TreeViewHelper.ExpandAll(treeView, true);
        }

        private void H_Zuordnen_Selected(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri(@"Haubenofen\Freie_Aufträge.xaml", UriKind.Relative);
        }
        
        private void MenuItem_Maschinenarten_Click(object sender, RoutedEventArgs e)
        {
            Daten.Maschinenarten_Window maWerte = new Daten.Maschinenarten_Window();
            maWerte.Show();
        }

        private void TechnologieVorlage_Selected(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri(@"Daten\TVorlagen.xaml", UriKind.Relative);
        }

        private void Auftrags_Zuordnung_Selected(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri(@"Allgemein\Freie_Aufträge.xaml", UriKind.Relative);
        }

        private void IA_Verfahren_Selected(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri(@"Daten\IA_Verfahren.xaml", UriKind.Relative);
        }

        private void MenuItem_Maschinen_Click(object sender, RoutedEventArgs e)
        {
            Daten.Maschinen_Window mwi = new Daten.Maschinen_Window();
            mwi.Show();
        }

        private void MenuItem_Maschinenkategorie_Click(object sender, RoutedEventArgs e)
        {
            Daten.Kategorie_Window kwi = new Daten.Kategorie_Window();
            kwi.Show();
        }
    }
}
