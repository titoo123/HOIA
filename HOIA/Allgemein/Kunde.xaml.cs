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

namespace HOIA.Allgemein
{
    /// <summary>
    /// Interaktionslogik für Kunde.xaml
    /// </summary>
    public partial class Kunde : Window
    {
        private string auftragsNr;
        private HOIA.Kunde kunde;

        public Kunde()
        {
            InitializeComponent();
        }

        public Kunde(HOIA.Kunde kunde, string auftragsNr)
        {
            InitializeComponent();
            this.kunde = kunde;
            this.auftragsNr = auftragsNr;

            textBox_Name.Text = kunde.Name;
            textBox_Anschrift.Text = kunde.Anschrift;
            textBox_PLZ.Text = kunde.PLZ;
            textBox_Stadt.Text = kunde.Stadt;
            textBox_Land.Text = kunde.Land;

            this.Title = this.Title + " " + auftragsNr;
        }

    }
}
