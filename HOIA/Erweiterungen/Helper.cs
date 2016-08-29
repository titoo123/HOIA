﻿using System;
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
        public static string FERTIGEAUFTRÄGE_STRING = "Bearbeitete Aufträge";
        public static string WARTENDEAUFTRÄGE_STRING = "Wartende Aufträge";
        public static string GESPERRTEAUFTRÄGE_STRING = "Gesperrte Aufträge";

        public static string AUFTRAG_OFFEN_STRING = "Frei";
        public static string AUFTRAG_WARTEN_STRING = "Warte";
        public static string AUFTRAG_GESPERRT_STRING = "Gesprrt";

        public static string EL1_STRING = "EL1";
        public static string EL2_STRING = "EL2";
        public static string HGO1_STRING = "HGO1";
        public static string HGO2_STRING = "HGO2";
        public static string RM_STRING = "RM";

        public static string PLATZHALTER_STRING = "----------------------";

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
    }
}
