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
    /// Interaktionslogik für Bestimmungsort.xaml
    /// </summary>
    public partial class Bestimmungsort : Window
    {
        private string auftragsNr;
        private HOIA.Bestimmungsort bestimmungsort;

        public Bestimmungsort()
        {
            InitializeComponent();
        }

        public Bestimmungsort(HOIA.Bestimmungsort bestimmungsort, string auftragsNr)
        {
            InitializeComponent();
            this.bestimmungsort = bestimmungsort;
            this.auftragsNr = auftragsNr;

            textBox_Name.Text = bestimmungsort.Name;
            textBox_Anschrift.Text = bestimmungsort.Anschrift;
            textBox_PLZ.Text = bestimmungsort.PLZ;
            textBox_Stadt.Text = bestimmungsort.Stadt;
            textBox_Land.Text = bestimmungsort.Land;

            this.Title = this.Title + " " + auftragsNr;
        }
    }
}
