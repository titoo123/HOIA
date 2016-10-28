using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        public static string STRING_MASCHINE = "Maschine";
        public static string STRING_VERFAHREN = "Verfahren";
        public static string STRING_KATEGORIE = "Kategorie";
        public static string STRING_ALLGEMEIN = "Allgemein";

        public static string STRING_ACHTUNG = "Achtung!";

        public const string STRING_BEARBEITUNG_STATUS = "In Bearbeitung";
        public const string STRING_FREI_STATUS = "Frei";
        public const string STRING_FERTIG_STATUS = "Fertig";
        public const string STRING_ZUGEORDNET_STATUS = "Zugeordnet";
        public const string STRING_GESPEICHERT_STATUS = "Gespeichert";
        public const string STRING_STATUS = "Status";

        //public static string FREIEAUFTRÄGE_STRING = "Freie Aufträge";
        //public static string FERTIGEAUFTRÄGE_STRING = "Bearbeitete Aufträge";
        //public static string WARTENDEAUFTRÄGE_STRING = "Wartende Aufträge";
        //public static string GESPERRTEAUFTRÄGE_STRING = "Gesperrte Aufträge";

        //public static string AUFTRAG_OFFEN_STRING = "Frei";
        //public static string AUFTRAG_WARTEN_STRING = "Warte";
        //public static string AUFTRAG_GESPERRT_STRING = "Gesperrt";

        //public static string EL1_STRING = "EL1";
        //public static string EL2_STRING = "EL2";
        //public static string HGO1_STRING = "HGO1";
        //public static string HGO2_STRING = "HGO2";
        //public static string RM_STRING = "RM";

        public static string PLATZHALTER_STRING = "----------------------";



        public static string GetStringFromDataGrid(int l, DataGrid g)
        {
            return "" + ((TextBlock)g.Columns[l].GetCellContent(g.SelectedItem)).Text;
        }
        public static string GetStringFromDataGrid(int l,int r, DataGrid g)
        {
            return "" + ((TextBlock)g.Columns[l].GetCellContent(g.Items[r])).Text;
        }
        public static int GetIntFromDataGrid(int l, DataGrid g)
        {
            return Convert.ToInt32(GetStringFromDataGrid(l, g));
        }
        public static int GetIntFromDataGrid(int l, int r, DataGrid g)
        {
            return Convert.ToInt32(GetStringFromDataGrid( l, r, g));
        }
        public static double GetDoubleFromDataGrid(int l, DataGrid g)
        {
            return Convert.ToDouble(GetStringFromDataGrid(l, g));
        }

        public static string AddString(string s,string k,int a) {
            return s + "( "+ k +": " + a + " ) ";
        }
        public static string CleanUpCatNumber(string s)
        {
            if (s.Contains(" 1"))
            {
                return CutFreeString(s.Split('1').First());
            }
            else if (s.Contains(" 2"))
            {
                return CutFreeString(s.Split('2').First());
            }
            else if (s.Contains(" 3"))
            {
                return CutFreeString(s.Split('3').First());
            }
            else
            {
                return s;
            }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="d">Was soll zurückgegeben werden? 0 oder 1</param>
        /// <returns></returns>
        public static string CleanUpTheFuckingListViewItem(string s, int d)
        {
            if (s.Contains("("))
            {
                string z = s.Split('(')[1];
                z = z.Split(',')[d];
                if (z.Contains(")"))
                {
                   z = z.Split(')').First();
                }
                z = z.Replace(" ",String.Empty);
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
        internal static string GetComboBoxTextComplete(ComboBox x)
        {
            try
            {
                return x.SelectedItem.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static string GetComboBoxText(ComboBox x) {
            try
            {
                return ((ComboBoxItem)x.SelectedItem).Content.ToString();
            }
            catch (Exception)
            {
                return String.Empty;
            }

        }

        public static string GetComboBoxTextMK2(ComboBox x) {
            try
            {
                string s = x.SelectedItem.ToString();
                if (s.Contains(" "))
                {
                    string[] c = s.Split(' ');
                    return c[1];
                }
                else
                {
                    return s;
                }

            }
            catch (Exception)
            {
                return String.Empty;
            }

        }
        public static DataGridRow GetRow(DataGrid grid, int index)
        {
            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // May be virtualized, bring into view and try again.
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.Items[index]);
                row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }
        public static DataGridCell GetCell(DataGrid grid, DataGridRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    grid.ScrollIntoView(row, grid.Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(row);
                }

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }
        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        public static void ComboBoxSelectValue(ComboBox c,string s)
        {
            c.SelectedIndex = c.Items.IndexOf(s);
        }
        public static void DataGridMakeRowColor(DataGrid dataGrid, int t, SolidColorBrush color)
        {
            DataGridRow row = GetRow(dataGrid, t);

            try
            {
                row.Background = color;
            }
            catch (Exception)
            {

            }
        }
    }
}
