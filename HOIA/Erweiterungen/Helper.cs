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

namespace HOIA.Erweiterungen
{
    class Helper
    {
        public static string HAUBENOFEN_ART = "ho";
        public static string INDUKTIONSANLAGEN_ART = "ia";
        public static string FREIEAUFTRÄGE_STRING = "Freie Aufträge";
        public static string FERTIGEAUFTRÄGE_STRING = "Fertige Aufträge";
        public static string AUFTRAG_OFFEN_STRING = "Frei";

        public static string EL1_STRING = "EL1";
        public static string EL2_STRING = "EL2";
        public static string HGO1_STRING = "HGO1";
        public static string HGO2_STRING = "HGO2";
        public static string RM_STRING = "RM";

        public static string GetStringFromDataGrid(int l, DataGrid g)
        {
            return "" + ((TextBlock)g.Columns[l].GetCellContent(g.SelectedItem)).Text;
        }
        public static int GetIntFromDataGrid(int l, DataGrid g)
        {
            return Convert.ToInt32(GetStringFromDataGrid(l, g));
        }
        public static double GetDoubleFromDataGrid(int l, DataGrid g)
        {
            return Convert.ToDouble(GetStringFromDataGrid(l, g));
        }

        public static string AddString(string s,string k,int a) {
            return s + "( "+ k +": " + a + " ) ";
        }
        public static string CleanUpString(string s)
        {
            if (s.Contains("("))
            {
                return CutFreeString(s.Split('(').First());
            }
            else
            {
                return s;
            }

        }
        public static string CleanUpTheFuckingListViewItem(string s)
        {
            if (s.Contains("("))
            {
                string z = s.Split('(')[1];
                z = z.Split(',').First();
                return z;
            }
            else
            {
                return s;
            }

        }
        public static string CutFreeString( string s) {

            return s.Replace(" ", "");
        }

        internal static string AddString(string s, string k, int a, string n)
        {
            return s + " ( " + k + ": " + a + " "+ n + " ) ";
        }
    }
}
