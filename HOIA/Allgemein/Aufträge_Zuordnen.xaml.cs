﻿using HOIA.Erweiterungen;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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



namespace HOIA.Allgemein
{
    /// <summary>
    /// Interaktionslogik für H_Zuordnen.xaml
    /// </summary>
    public partial class Aufträge_Zuordnen : Page
    {
        private TreeViewItem treeViewItem;
        private bool lMousePressed = false;
        private List<Maschine> mList = new List<Maschine>();

        /// <summary>
        /// Konstruktor Klasse
        /// </summary>
        public Aufträge_Zuordnen()
        {
            InitializeComponent();
            //Laden aller Maschinen
            TreeView_H_Zuordnen_HOWerte_Refresh();
            //Eventzuordnung der TreeViewItems
            treeView_H_Zuordnen_HOWerte.AddHandler(TreeViewItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(treeView_Item_MouseLeftButtonDown));
            treeView_H_Zuordnen_HOWerte.AddHandler(TreeViewItem.PreviewMouseDoubleClickEvent, new MouseButtonEventHandler(treeViewItem_MouseDoubleClick));
            //Eventzuordung der ListViewItems
            ListView_Aufträge.AddHandler(ListViewItem.PreviewMouseDoubleClickEvent, new MouseButtonEventHandler(ListView_Aufträge_MouseDoubleClick));
            //ListView_Material.AlternationCount = ListView_Material.AlternationCount + 2;
        }
        /// <summary>
        /// Läd Werte
        /// </summary>
        private void TreeView_H_Zuordnen_HOWerte_Refresh()
        {
            DDataContext d = new DDataContext();
            //Verfahren
            //Haubenofen
            var hoa = from r in d.Verfahren
                      where r.Art == Helper.HAUBENOFEN_ART
                      select r.Name;
            //Induktionsanlagen
            var iaa = from r in d.Verfahren
                      where r.Art == Helper.INDUKTIONSANLAGEN_ART
                      select r.Name;
            //Freie Aufträge
            var fre = from f in d.Auftrag
                      where f.Status == Helper.AUFTRAG_OFFEN_STRING
                      select f.ODL;
            mList.Add(new Maschine(treeView_H_Zuordnen_HOWerte, Helper.FREIEAUFTRÄGE_STRING, false, fre));

            mList.Add(new Maschine(treeView_H_Zuordnen_HOWerte, Helper.EL1_STRING, true, iaa));
            mList.Add(new Maschine(treeView_H_Zuordnen_HOWerte, Helper.EL2_STRING, true, iaa));
            mList.Add(new Maschine(treeView_H_Zuordnen_HOWerte, Helper.HGO1_STRING, true, hoa));
            mList.Add(new Maschine(treeView_H_Zuordnen_HOWerte, Helper.HGO2_STRING, true, hoa));
            mList.Add(new Maschine(treeView_H_Zuordnen_HOWerte, Helper.RM_STRING, false));
            mList.Add(new Maschine(treeView_H_Zuordnen_HOWerte, Helper.FERTIGEAUFTRÄGE_STRING, false));
        }
        /// <summary>
        /// Linksklick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_Item_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            //Tag des angeklickenten Elements
            string tag = TreeViewHelper.HitTreeView(treeView_H_Zuordnen_HOWerte, e);

            //Leert Liste 
            ListView_Aufträge.Items.Clear();
            //Öffnet Knoten
            if (TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, TreeViewHelper.TreeViewGetNode_ByTag(treeView_H_Zuordnen_HOWerte, tag)) == 1)
            {
                TreeViewHelper.ExpandAll(treeView_H_Zuordnen_HOWerte, false);
            }

            foreach (var m in mList)
            {
                m.ReplaceListViewItems(ListView_Aufträge, tag);
            }

            if (tag != null)
            {
                foreach (TreeViewItem item in treeView_H_Zuordnen_HOWerte.Items)
                {

                    //Level 1 und 2
                    foreach (TreeViewItem i in item.Items)
                    {
                        if (tag.Contains(Helper.CleanUpString(i.Header.ToString())))
                        {
                            if (
                             (TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, i) == 2
                             && (
                             ((string)item.Tag).Contains(Helper.FREIEAUFTRÄGE_STRING)
                             ||
                             ((string)item.Tag).Contains(Helper.RM_STRING)
                             ))
                             )
                            {
                                lMousePressed = true;
                                treeViewItem = i;
                                item_Click(i);
                            }
                        }
                        //Level 3
                        foreach (TreeViewItem a in i.Items)
                        {
                            if (tag.Contains(a.Tag.ToString()))
                            {
                                if (TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, a) == 3)
                                {
                                    lMousePressed = true;
                                    treeViewItem = a;
                                    item_Click(a);
                                }
                            }
                        }

                    }

                }

            }

        }
        /// <summary>
        /// Rechtsklick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_Item_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            string header = TreeViewHelper.HitTreeView(treeView_H_Zuordnen_HOWerte, e);
            if (header != null)
            {
                TreeViewItem selectedItem = TreeViewHelper.TreeViewGetNode_ByText(treeView_H_Zuordnen_HOWerte, header);
                //ContextMenu contextMenu = new ContextMenu();

                MenuItem m = new MenuItem();
                m.Header = "Freigeben";

                //m.Click += Move;
                if (selectedItem != null)
                {
                    if (selectedItem.Focus())
                    {
                        if (selectedItem.Header == null)
                            return;

                        if (
                            (
                            TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, selectedItem) == 2
                            &&
                            (
                            (string)TreeViewHelper.GetParent(selectedItem).Header != Helper.FREIEAUFTRÄGE_STRING
                            &&
                            (string)TreeViewHelper.GetParent(selectedItem).Header != Helper.RM_STRING)
                            )

                            || (TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, selectedItem) == 1
                            && (string)selectedItem.Header == Helper.RM_STRING)
                            )
                        {

                            //selectedItem.ContextMenu = contextMenu;

                            //contextMenu.Items.Add(m);
                            //selectedItem.ContextMenu.IsOpen = true;


                        }
                    }
                }
            }


        }
        /// <summary>
        /// Maus Doppelklick auf TreeViewItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            string header = TreeViewHelper.HitTreeView(treeView_H_Zuordnen_HOWerte, e);
            if (header != null)
            {
                TreeViewItem selectedItem = TreeViewHelper.TreeViewGetNode_ByText(treeView_H_Zuordnen_HOWerte, header);
                TreeViewItem selectedItemParent = TreeViewHelper.GetParent(selectedItem);
                //TreeViewItem selectedItemGrandParent = TreeViewHelper.GetParent(selectedItemParent);

                if (selectedItem != null)
                {
                    if (selectedItem.Focus())
                    {
                        if (selectedItem.Header == null)
                            return;

                        if (
                            (

                            TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, selectedItem) == 2
                            &&
                            (
                            (string)selectedItemParent.Tag != Helper.FREIEAUFTRÄGE_STRING
                            &&
                            (string)selectedItemParent.Tag != Helper.RM_STRING)
                            )

                            || (TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, selectedItem) == 1
                            && (string)selectedItem.Tag == Helper.RM_STRING)
                            )
                        {

                            if (MessageBox.Show("Wollen sie wirklich die " + selectedItem.Items.Count + " Aufträge von " + selectedItem.Tag + " in die Liste verschieben?", "Verschieben?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {

                                foreach (var m in mList)
                                {
                                    if (m.Tag == (string)selectedItem.Tag)
                                    {
                                        foreach (TreeViewItem n in selectedItem.Items)
                                        {   //Fügt Aufträge in Werteliste ein
                                            m.AddJobToList((string)n.Tag, (string)TreeViewHelper.GetParent(n).Tag);
                                        }
                                        //Löscht Items aus der TreeView
                                        selectedItem.Items.Clear();
                                    }
                                    if (selectedItemParent != null)
                                    {
                                        if (m.Tag == (string)selectedItemParent.Tag)
                                        {
                                            foreach (TreeViewItem n in selectedItem.Items)
                                            {   //Fügt Aufträge in Werteliste ein
                                                m.AddJobToList((string)n.Tag, (string)TreeViewHelper.GetParent(n).Tag);

                                            }
                                            //Löscht Items aus der TreeView
                                            selectedItem.Items.Clear();

                                        }
                                    }

                                }




                            }

                        }
                    }
                }
            }

            //treeView_H_Zuordnen_HOWerte.Move(ListView_Aufträge);

        }
        private void treeView_H_Zuordnen_HOWerte_MouseUp(object sender, MouseButtonEventArgs e)
        {
            lMousePressed = false;
            TreeViewItem t = TreeViewHelper.VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;

            if (t != null)
            {
                string h = t.Tag.ToString();
                Extended_TreeView tv = treeView_H_Zuordnen_HOWerte;

                if (treeViewItem != null)
                {
                    if (
                        (
                        TreeViewHelper.GetNodeLevel(tv, t) == 1 && (h.Contains(Helper.FREIEAUFTRÄGE_STRING) || h.Contains(Helper.RM_STRING))
                        ||
                        TreeViewHelper.GetNodeLevel(tv, t) == 2 && (h.Contains(Helper.FREIEAUFTRÄGE_STRING) || h.Contains(Helper.RM_STRING))
                        )
                        ||
                        TreeViewHelper.InDb(h)
                        &&
                        h != treeViewItem.Tag.ToString()
                        )
                    {
                        //TreeViewItem t_parent = TreeViewHelper.GetParent(t);

                        //Fügt Knoten hinzu
                        t.Items.Add(new TreeViewItem() { Header = treeViewItem.Header, Tag = treeViewItem.Tag });
                        //Öffnet Knoten
                        TreeViewHelper.ExpandAll(t, true);
                        //Löscht knoten aus Liste
                        if (treeViewItem.Parent is TreeViewItem)
                        {
                            (treeViewItem.Parent as TreeViewItem).Items.Remove(treeViewItem);
                        }

                        foreach (Maschine m in mList)
                        {
                            m.RefreshValue();
                        }
                    }

                }
                treeViewItem = null;

                e.Handled = true;
            }

        }
        /// <summary>
        /// Item wird angeklickt
        /// </summary>
        /// <param name="i"></param>
        private void item_Click(TreeViewItem i)
        {
            Information_Loader(i.Header.ToString());
        }
        private void item_Click(string i)
        {
            Information_Loader(i);
        }
        /// <summary>
        /// Maus bewegt sich
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_H_Zuordnen_HOWerte_MouseMove(object sender, MouseEventArgs e)
        {

            if (lMousePressed)
            {
                TreeViewItem i = TreeViewHelper.HitTreeView(treeView_H_Zuordnen_HOWerte, e);
                if (i != null && TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, i) == 1)
                {
                    i.IsExpanded = true;
                    TreeViewHelper.CloseAll(treeView_H_Zuordnen_HOWerte, i, 2);
                }

            }

        }
        /// <summary>
        /// Auswahl Auftragsliste ändert sich
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_Aufträge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView_Aufträge.SelectedIndex != -1)
            {
                item_Click(Helper.CleanUpTheFuckingListViewItem(ListView_Aufträge.SelectedItem.ToString(), 0));
            }
        }
        private void ListView_Aufträge_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListView_Aufträge.Items.Count > 0)
            {
                var tmp = ListView_Aufträge.SelectedItems[0];
                if (MessageBox.Show("Wollen sie wirklich diesen Auftrag: " + tmp.ToString() + " aus den aktiven Aufträgen entfernen?", "Entfernen?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    //Auftrag
                    string s1 = Helper.CleanUpTheFuckingListViewItem(ListView_Aufträge.SelectedItem.ToString(), 0);
                    //Kategorie
                    string s2 = Helper.CleanUpTheFuckingListViewItem(ListView_Aufträge.SelectedItem.ToString(), 1);
                    //Ausgewähltes TreeViewItem
                    TreeViewItem i = treeView_H_Zuordnen_HOWerte.SelectedItem as TreeViewItem;

                    //Löscht Auftrag aus der Liste der Maschine
                    foreach (var m in mList)
                    {
                        m.DeleteJobFormList(s1);
                    }

                    //Löscht Auftrag aus der eigentlichen Liste
                    ListView_Aufträge.Items.RemoveAt(ListView_Aufträge.SelectedIndex);

                    //Fügt Knoten wieder an der richtigen Stelle hinzu
                    foreach (TreeViewItem t in i.Items)
                    {
                        if ((string)t.Tag == s2)
                        {
                            t.Items.Add(new TreeViewItem() { Header = s1, Tag = s1 });
                        }

                    }

                }
            }
        }


        private void Information_Loader(string s)
        {

            DDataContext d = new DDataContext();

            var drt = from h in d.Auftrag
                      where h.ODL == s
                      select h;
            if (drt.Count() > 0)
            {
                Auftrag a = drt.First();

                //Allgemeine Daten
                textBox_Walzung.Text = a.Walzung;
                textBox_Lagerort.Text = a.Lagerort;
                textBox_Verarbeitung.Text = a.Verarbeitung;
                textBox_Auftrag.Text = a.AuftragsNr + "/" + a.Position;
                textBox_ODL.Text = a.ODL;
                textBox_ADatum.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(a.Auftragsdatum.ToString()));
                textBox_Status.Text = a.Status;
                textBox_LTermin.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(a.Liefertermin.ToString()));

                //Beschreibung Material
                textBox_Abm1.Text = a.Abmessung1.ToString();
                textBox_Abm2.Text = a.Abmessung2.ToString();
                textBox_Art.Text = a.Art;
                textBox_Stahlsorte.Text = a.Stahlsorte;
                textBox_FLänge.Text = a.FLänge.ToString();
                textBox_WLänge.Text = a.WLänge.ToString();
                textBox_Charge.Text = a.Charge;
                //Gesamtmenge berechnen
                var ert = from l in d.Material
                          where l.Id_Auftrag == drt.First().Id
                          select l;
                textBox_Gesamtmenge.Text = ert.Count().ToString();
                //Gesamtgewicht berechnen
                int? geg = (from m in d.Material
                            where m.Id_Auftrag == a.Id
                            select m.Gewicht).Sum();
                textBox_Gesamtgewicht.Text = geg.ToString();
                //Zugehörige Materialien
                var mat = from m in d.Material
                          where m.Id_Auftrag == a.Id
                          select m;
                int z = 1;
                foreach (Material m in mat)
                {
                    m.Display_Position = z++; ;
                }
                ListView_Material.ItemsSource = mat;

                //Messwerte
                textBox_C.Text = a.C.ToString();
                textBox_Mn.Text = a.Mn.ToString();
                textBox_Si.Text = a.Si.ToString();
                textBox_P.Text = a.P.ToString();
                textBox_S.Text = a.S.ToString();
                textBox_Cr.Text = a.Cr.ToString();
                textBox_Ni.Text = a.Ni.ToString();
                textBox_Mo.Text = a.Mo.ToString();

                //Ergänzungen
                textBox_TecAnmerkungen.Text = a.TechnischeAnmerkungen;
                textBox_IntAnmerkungen.Text = a.Bemerkungen;
                textBox_Sägeprogramm.Text = a.SägeProgramm.ToString();
                textBox_Anlasstemp.Text = a.Anlasstemparartur.ToString();
                //Status aktualisieren
                switch (textBox_Status.Text)
                {
                    case "Frei":
                        radiobutton_Frei.IsChecked = true;
                        break;
                    case "Warten":
                        radiobutton_Warten.IsChecked = true;
                        break;
                    case "Gesperrt":
                        radiobutton_Gesperrt.IsChecked = true;
                        break;
                    default:
                        break;
                }

            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;


            if (button.Content != null)
            {
                textBox_Status.Text = button.Content.ToString();
            }

        }

        private void button_kunde_Click(object sender, RoutedEventArgs e)
        {
            DDataContext d = new DDataContext();

            var auf = from v in d.Auftrag
                      where v.ODL == textBox_ODL.Text
                      // && v.AuftragsNr == textBox_Auftrag.Text
                      select v;
            var kun = from s in d.Kunde
                      where s.Id == auf.First().Id_Kunde
                      select s;
            if (auf.Count() > 0 && kun.Count() > 0)
            {
                Window wKunde = new Kunde(kun.First(), auf.First().AuftragsNr);
                wKunde.Show();
            }

        }

        private void button_Bestimmungsort_Click(object sender, RoutedEventArgs e)
        {
            DDataContext d = new DDataContext();

            var auf = from v in d.Auftrag
                      where v.ODL == textBox_ODL.Text
                      // && v.AuftragsNr == textBox_Auftrag.Text
                      select v;
            var kun = from s in d.Bestimmungsort
                      where s.Id == auf.First().Id_Bestimmungsort
                      select s;
            if (auf.Count() > 0 && kun.Count() > 0)
            {
                Window wBestimmungsort = new Bestimmungsort(kun.First(), auf.First().AuftragsNr);
                wBestimmungsort.Show();
            }

        }

        private void Button_Speichern_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_ODL.Text.Length > 0)
            {
                DDataContext d = new DDataContext();
                var auf = from v in d.Auftrag
                          where v.ODL == textBox_ODL.Text
                          select v;
                Auftrag a = auf.First();

                a.Status = textBox_Status.Text;
                a.TechnischeAnmerkungen = textBox_TecAnmerkungen.Text;
                a.Bemerkungen = textBox_IntAnmerkungen.Text;

                int z;
                if (Int32.TryParse(textBox_Anlasstemp.Text, out z))
                {
                    a.Anlasstemparartur = z;
                }
                else
                {
                    MessageBox.Show("Bitte überprüfen sie die Anlasstemperatur!\nSie ist keine Zahl!", "Fehler!");
                }

                int u;
                if (Int32.TryParse(textBox_Sägeprogramm.Text, out u))
                {
                    a.SägeProgramm = u;
                }
                else
                {
                    MessageBox.Show("Bitte überprüfen sie das Sägeprogramm!\nEs ist keine Zahl!", "Fehler!");
                }

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Auftrag konnte leider nicht bearbeitet werden!\nKeine Verbindung zur Datenbank!", "Fehler!");
                }
            }
        }

        private void button_Wichtig_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_ODL.Text.Length > 0)
            {
                DDataContext d = new DDataContext();
                var auf = from v in d.Auftrag
                          where v.ODL == textBox_ODL.Text
                          select v;
                Auftrag a = auf.First();

                if (!(a.Wichtig == null ))
                {
                    a.Wichtig = !a.Wichtig;
                }
                else
                {
                    a.Wichtig = true;
                }


                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Wichtigkeit des Auftrages konnte leider nicht bearbeitet werden!\nKeine Verbindung zur Datenbank!", "Fehler!");
                }
            }
        }
    }
}
