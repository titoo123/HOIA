using HOIA.Erweiterungen;
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
        private List<Auftragskategorie> kList = new List<Auftragskategorie>();

        private string treeView_Tag;

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

        }
        /// <summary>
        /// Läd Werte
        /// </summary>
        private void TreeView_H_Zuordnen_HOWerte_Refresh()
        {
     
            kList.Add(new Auftragskategorie(treeView_H_Zuordnen_HOWerte, Helper.FREIEAUFTRÄGE_STRING));
            kList.Add(new Auftragskategorie(treeView_H_Zuordnen_HOWerte, Helper.FERTIGEAUFTRÄGE_STRING));
            kList.Add(new Auftragskategorie(treeView_H_Zuordnen_HOWerte, Helper.WARTENDEAUFTRÄGE_STRING));
            kList.Add(new Auftragskategorie(treeView_H_Zuordnen_HOWerte, Helper.GESPERRTEAUFTRÄGE_STRING));

            RefreshFreieAufträge();
        }
        /// <summary>
        /// Linksklick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_Item_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Extended_TreeView exT = treeView_H_Zuordnen_HOWerte;

            //Tag des angeklickenten Elements
            string tag = TreeViewHelper.HitTreeView(exT, e);
            //Gibt Variable global weiter
            treeView_Tag = tag;

            //Leert Liste 
            ListView_Aufträge.Items.Clear();

            ////Öffnet Knoten
            //if (TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, TreeViewHelper.TreeViewGetNode_ByTag(treeView_H_Zuordnen_HOWerte, tag)) == 1)
            //{
            //   // TreeViewHelper.ExpandAll(treeView_H_Zuordnen_HOWerte, false);
            //}

            //foreach (var m in mList)
            //{
            //    m.ReplaceListViewItems(ListView_Aufträge, tag);
            //}

            if (tag != null)
            {   //exT_i1 = Extended Treeview Item der Stufe 1
                foreach (TreeViewItem exT_i1 in exT.Items)
                {

                    //Level 1 und 2 
                    //Freie Aufträge und Kategorien wie "Bearbeitete Aufträge"
                    //exT_i2 = Extended Treeview Item der Stufe 2
                    foreach (TreeViewItem exT_i2 in exT_i1.Items)
                    {
                        if (tag.Contains(Helper.CleanUpString(exT_i2.Header.ToString())))
                        {
                            if (
                             (TreeViewHelper.GetNodeLevel(exT, exT_i2) == 2
                             && (
                             ((string)exT_i1.Tag).Contains(Helper.FREIEAUFTRÄGE_STRING)
                             //||
                             //((string)item.Tag).Contains(Helper.RM_STRING)
                             ))
                             )
                            {
                                lMousePressed = true;
                                treeViewItem = exT_i2;
                                item_Click(exT_i2);
                            }
                        }
                        //Level 3 
                        //RM und Maschinen wie EL1
                        //exT_i3 = Extended Treeview Item der Stufe 3
                        foreach (TreeViewItem exT_i3 in exT_i2.Items)
                        {
                            if (tag.Contains(exT_i3.Tag.ToString()))
                            {
                                if (TreeViewHelper.GetNodeLevel(exT, exT_i3) == 3)
                                {
                                    lMousePressed = true;
                                    treeViewItem = exT_i3;
                                    item_Click(exT_i3);
                                }
                            }
                            if (exT_i3.HasItems)
                            {   //Level 4
                                //ODL Nummern in den Maschinen
                                foreach (TreeViewItem exT_i4 in exT_i3.Items)
                                {
                                    if (TreeViewHelper.GetNodeLevel(exT, exT_i4) == 4)
                                    {
                                        if (tag.Contains(exT_i4.Tag.ToString()))
                                        {
                                            lMousePressed = true;
                                            treeViewItem = exT_i4;
                                            item_Click(exT_i4);
                                        }

                                    }
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
            //string header = TreeViewHelper.HitTreeView(treeView_H_Zuordnen_HOWerte, e);
            //if (header != null)
            //{
            //    TreeViewItem selectedItem = TreeViewHelper.TreeViewGetNode_ByText(treeView_H_Zuordnen_HOWerte, header);
            //    //ContextMenu contextMenu = new ContextMenu();

            //    MenuItem m = new MenuItem();
            //    m.Header = "Freigeben";

            //    //m.Click += Move;
            //    if (selectedItem != null)
            //    {
            //        if (selectedItem.Focus())
            //        {
            //            if (selectedItem.Header == null)
            //                return;

            //            if (
            //                (
            //                TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, selectedItem) == 2
            //                &&
            //                (
            //                (string)TreeViewHelper.GetParent(selectedItem).Header != Helper.FREIEAUFTRÄGE_STRING
            //                &&
            //                (string)TreeViewHelper.GetParent(selectedItem).Header != Helper.RM_STRING)
            //                )

            //                || (TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, selectedItem) == 1
            //                && (string)selectedItem.Header == Helper.RM_STRING)
            //                )
            //            {

            //                //selectedItem.ContextMenu = contextMenu;

            //                //contextMenu.Items.Add(m);
            //                //selectedItem.ContextMenu.IsOpen = true;


            //            }
            //        }
            //    }
            //}


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
                TreeViewItem selectedItemParentParent = TreeViewHelper.GetParent(selectedItemParent);

                if (selectedItem != null)
                {
                    if (selectedItem.Focus())
                    {
                        if (selectedItem.Header == null)
                            return;

                        if (
                            (

                            TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, selectedItem) == 3
                            &&
                            (
                            (string)selectedItemParent.Tag != Helper.FREIEAUFTRÄGE_STRING
                            &&
                            (string)selectedItemParent.Tag != Helper.RM_STRING)
                            )

                            || (TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, selectedItem) == 2
                            && (string)selectedItem.Tag == Helper.RM_STRING)
                            )
                        {
                            if (selectedItem.Items.Count > 0)
                            {
                                if (MessageBox.Show("Wollen sie wirklich die " + selectedItem.Items.Count + " Aufträge von " + selectedItem.Tag + " in die Liste verschieben?", "Verschieben?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    AddToListView(selectedItem, selectedItemParent, selectedItemParentParent);

                                }
                            }

                        }
                        else if (TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, selectedItem) == 4)
                        {
                            AddToListView(selectedItem, selectedItemParent, selectedItemParentParent);
                        }
                    }
                }
            }


        }
        private void treeView_H_Zuordnen_HOWerte_MouseUp(object sender, MouseButtonEventArgs e)
        {
            lMousePressed = false;
            TreeViewItem treeViewItem_catched = TreeViewHelper.VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;

            if (treeViewItem_catched != null)
            {
                string treeViewItem_Tag = treeViewItem_catched.Tag.ToString();
                Extended_TreeView exT = treeView_H_Zuordnen_HOWerte;

                if (treeViewItem != null)
                {
                    if (
                        (//Freie Aufträge
                        TreeViewHelper.GetNodeLevel(exT, treeViewItem_catched) == 1 && (treeViewItem_Tag.Contains(Helper.FREIEAUFTRÄGE_STRING) || treeViewItem_Tag.Contains(Helper.RM_STRING))
                        ||//RM
                        TreeViewHelper.GetNodeLevel(exT, treeViewItem_catched) == 2 && (treeViewItem_Tag.Contains(Helper.FREIEAUFTRÄGE_STRING) || treeViewItem_Tag.Contains(Helper.RM_STRING))
                        )
                        ||
                        TreeViewHelper.InDb(treeViewItem_Tag)
                        &&
                        treeViewItem_Tag != treeViewItem.Tag.ToString()
                        )
                    {
                        //TreeViewItem t_parent = TreeViewHelper.GetParent(t);

                        //Fügt Knoten hinzu
                        treeViewItem_catched.Items.Add(new TreeViewItem() { Header = treeViewItem.Header, Tag = treeViewItem.Tag });
                        //Öffnet Knoten
                        TreeViewHelper.ExpandAll(treeViewItem_catched, true);
                        //Löscht knoten aus Liste
                        if (treeViewItem.Parent is TreeViewItem)
                        {
                            (treeViewItem.Parent as TreeViewItem).Items.Remove(treeViewItem);
                        }

                        //foreach (Maschine m in mList)
                        //{
                        //    m.RefreshValue();
                        //}
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

            //if (lMousePressed)
            //{
            //    TreeViewItem i = TreeViewHelper.HitTreeView(treeView_H_Zuordnen_HOWerte, e);
            //    if (i != null && TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, i) == 1 )
            //    {
            //        i.IsExpanded = true;
            //        //TreeViewHelper.CloseAll(treeView_H_Zuordnen_HOWerte, i, 2);
            //        TreeViewHelper.OpenOrCloseAllOnLevel(treeView_H_Zuordnen_HOWerte, i, 3);
                    
            //    }

            //}

        }






        /// <summary>
        /// Auswahl Auftragsliste ändert sich
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_Aufträge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (ListView_Aufträge.SelectedIndex != -1)
            //{
            //    item_Click(Helper.CleanUpTheFuckingListViewItem(ListView_Aufträge.SelectedItem.ToString(), 0));
            //}
        }
        private void ListView_Aufträge_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (ListView_Aufträge.Items.Count > 0)
            //{
            //    var tmp = ListView_Aufträge.SelectedItems[0];
            //    if (MessageBox.Show("Wollen sie wirklich diesen Auftrag: " + tmp.ToString() + " aus den aktiven Aufträgen entfernen?", "Entfernen?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //    {
            //        //Auftrag
            //        string s1 = Helper.CleanUpTheFuckingListViewItem(ListView_Aufträge.SelectedItem.ToString(), 0);
            //        //Kategorie
            //        string s2 = Helper.CleanUpTheFuckingListViewItem(ListView_Aufträge.SelectedItem.ToString(), 1);
            //        //Ausgewähltes TreeViewItem
            //        TreeViewItem i = treeView_H_Zuordnen_HOWerte.SelectedItem as TreeViewItem;

            //        //Löscht Auftrag aus der Liste der Maschine
            //        foreach (var m in mList)
            //        {
            //            m.DeleteJobFormList(s1);
            //        }

            //        //Löscht Auftrag aus der eigentlichen Liste
            //        ListView_Aufträge.Items.RemoveAt(ListView_Aufträge.SelectedIndex);

            //        //Fügt Knoten wieder an der richtigen Stelle hinzu
            //        foreach (TreeViewItem t in i.Items)
            //        {
            //            if ((string)t.Tag == s2)
            //            {
            //                t.Items.Add(new TreeViewItem() { Header = s1, Tag = s1 });
            //            }

            //        }

            //    }
            //}
        }


        private void Information_Loader(string s)
        {
            //string kg = " Kg";
            //string abmessung = " mm";

            //DDataContext d = new DDataContext();

            //var drt = from h in d.Auftrag
            //          where h.ODL == s
            //          select h;
            //if (drt.Count() > 0)
            //{
            //    Auftrag a = drt.First();

            //    //Allgemeine Daten
            //    textBox_Walzung.Text = a.Walzung;
            //    textBox_Lagerort.Text = a.Lagerort;
            //    textBox_Verarbeitung.Text = a.Verarbeitung;
            //    textBox_Auftrag.Text = a.AuftragsNr + "/" + a.Position;
            //    textBox_ODL.Text = a.ODL;
            //    textBox_ADatum.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(a.Auftragsdatum.ToString()));
            //    textBox_Status.Text = a.Status;
            //    textBox_LTermin.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(a.Liefertermin.ToString()));

            //    //Beschreibung Material
            //    textBox_Abm1.Text = a.Abmessung1.ToString() + abmessung;
            //    textBox_Abm2.Text = a.Abmessung2.ToString() + abmessung;
            //    textBox_Art.Text = a.Art;
            //    textBox_Stahlsorte.Text = a.Stahlsorte;
            //    textBox_FLänge.Text = a.FLänge.ToString() + abmessung;
            //    textBox_WLänge.Text = a.WLänge.ToString() + abmessung;
            //    textBox_Charge.Text = a.Charge;
            //    //Gesamtmenge berechnen
            //    var ert = from l in d.Material
            //              where l.Id_Auftrag == drt.First().Id
            //              select l;
            //    textBox_Gesamtmenge.Text = ert.Count().ToString();
            //    //Gesamtgewicht berechnen
            //    int? geg = (from m in d.Material
            //                where m.Id_Auftrag == a.Id
            //                select m.Gewicht).Sum();
            //    textBox_Gesamtgewicht.Text = geg.ToString() + kg;
            //    //Zugehörige Materialien
            //    var mat = from m in d.Material
            //              where m.Id_Auftrag == a.Id
            //              select m;
            //    int z = 1;
            //    foreach (Material m in mat)
            //    {
            //        m.Display_Position = z++; ;
            //    }
            //    ListView_Material.ItemsSource = mat;

            //    //Messwerte
            //    textBox_C.Text = a.C.ToString();
            //    textBox_Mn.Text = a.Mn.ToString();
            //    textBox_Si.Text = a.Si.ToString();
            //    textBox_P.Text = a.P.ToString();
            //    textBox_S.Text = a.S.ToString();
            //    textBox_Cr.Text = a.Cr.ToString();
            //    textBox_Ni.Text = a.Ni.ToString();
            //    textBox_Mo.Text = a.Mo.ToString();

            //    //Ergänzungen
            //    textBox_TecAnmerkungen.Text = a.TechnischeAnmerkungen;
            //    textBox_IntAnmerkungen.Text = a.Bemerkungen;
            //    textBox_Sägeprogramm.Text = a.SägeProgramm.ToString();
            //    textBox_Anlasstemp.Text = a.Anlasstemparartur.ToString();
            //    //Status aktualisieren
            //    switch (textBox_Status.Text)
            //    {
            //        case "Frei":
            //            radiobutton_Frei.IsChecked = true;
            //            break;
            //        case "Warten":
            //            radiobutton_Warten.IsChecked = true;
            //            break;
            //        case "Gesperrt":
            //            radiobutton_Gesperrt.IsChecked = true;
            //            break;
            //        default:
            //            break;
            //    }

            //}
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
            ////Ändert Wichtigkeit in Datenbank
            //if (textBox_ODL.Text.Length > 0)
            //{
            //    DDataContext d = new DDataContext();
            //    var auf = from v in d.Auftrag
            //              where v.ODL == textBox_ODL.Text
            //              select v;
            //    Auftrag a = auf.First();

            //    if (!(a.Wichtig == null))
            //    {
            //        a.Wichtig = !a.Wichtig;
            //    }
            //    else
            //    {
            //        a.Wichtig = true;
            //    }


            //    try
            //    {
            //        d.SubmitChanges();
            //    }
            //    catch (Exception)
            //    {
            //        MessageBox.Show("Wichtigkeit des Auftrages konnte leider nicht bearbeitet werden!\nKeine Verbindung zur Datenbank!", "Fehler!");
            //    }

            //    //Ändert Wichtigkeit in Liste
            //    foreach (Maschine m in mList)
            //    {
            //        m.RefreshJobInList(textBox_ODL.Text);
            //    }

            //    //Leert Liste 
            //    ListView_Aufträge.Items.Clear();

            //    foreach (var m in mList)
            //    {
            //        m.ReplaceListViewItems(ListView_Aufträge, treeView_Tag);
            //    }
            //}
        }

        private void button_AS400_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddToListView(TreeViewItem selectedItem, TreeViewItem selectedItemParent, TreeViewItem selectedItemParentParent)
        {
            //foreach (var m in mList)
            //{
            //    if (m.Tag == (string)selectedItem.Tag)
            //    {
            //        foreach (TreeViewItem n in selectedItem.Items)
            //        {
            //            //Ob Auftrag wichtig ist?
            //            DDataContext d = new DDataContext();
            //            var wic = from l in d.Auftrag
            //                      where l.Wichtig == true && l.ODL == (string)n.Tag
            //                      select l;

            //            if (wic.Count() > 0)
            //            {
            //                //Fügt Aufträge in Werteliste ein
            //                m.AddJobToList((string)n.Tag, (string)TreeViewHelper.GetParent(n).Tag, true);
            //            }
            //            else
            //            {
            //                //Fügt Aufträge in Werteliste ein
            //                m.AddJobToList((string)n.Tag, (string)TreeViewHelper.GetParent(n).Tag, false);
            //            }

            //        }
            //        //Löscht Items aus der TreeView
            //        selectedItem.Items.Clear();
            //    }
            //    //Für RM
            //    if (selectedItemParent != null)
            //    {
            //        if (m.Tag == (string)selectedItemParent.Tag)
            //        {
            //            foreach (TreeViewItem n in selectedItem.Items)
            //            {
            //                //Ob Auftrag wichtig ist?
            //                DDataContext d = new DDataContext();
            //                var wic = from l in d.Auftrag
            //                          where l.Wichtig == true && l.ODL == (string)n.Tag
            //                          select l;

            //                if (wic.Count() > 0)
            //                {
            //                    //Fügt Aufträge in Werteliste ein
            //                    m.AddJobToList((string)n.Tag, (string)TreeViewHelper.GetParent(n).Tag, true);
            //                }
            //                else
            //                {
            //                    //Fügt Aufträge in Werteliste ein
            //                    m.AddJobToList((string)n.Tag, (string)TreeViewHelper.GetParent(n).Tag, false);
            //                }


            //            }
            //            //Löscht Items aus der TreeView
            //            selectedItem.Items.Clear();

            //        }
            //        if (selectedItemParentParent != null)
            //        {
            //            if (m.Tag == (string)selectedItemParentParent.Tag)
            //            {

            //                //Ob Auftrag wichtig ist?
            //                DDataContext d = new DDataContext();
            //                var wic = from l in d.Auftrag
            //                          where l.Wichtig == true && l.ODL == (string)selectedItem.Tag
            //                          select l;

            //                if (wic.Count() > 0)
            //                {
            //                    //Fügt Aufträge in Werteliste ein
            //                    m.AddJobToList((string)selectedItem.Tag, (string)TreeViewHelper.GetParent(selectedItem).Tag, true);
            //                }
            //                else
            //                {
            //                    //Fügt Aufträge in Werteliste ein
            //                    m.AddJobToList((string)selectedItem.Tag, (string)TreeViewHelper.GetParent(selectedItem).Tag, false);
            //                }


            //                //Löscht Items aus der TreeView
            //                selectedItemParent.Items.Remove(selectedItem);
            //            }
            //        }
            //    }


            //}

        }

        private void RefreshFreieAufträge() {
            foreach (TreeViewItem item in treeView_H_Zuordnen_HOWerte.Items)
            {
                if ((string)item.Tag == Helper.FREIEAUFTRÄGE_STRING)
                {
                    item.Header = Helper.CleanUpString((string)item.Header) + " ( " + item.Items.Count + " )";
                }
            }

        }
    }

}
