using HOIA.Erweiterungen;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        //Erweiterungen.Tuple a = new Erweiterungen.Tuple();

        List<Maschine> mList = new List<Maschine>();

        public Aufträge_Zuordnen()
        {
            InitializeComponent();

            TreeView_H_Zuordnen_HOWerte_Refresh();

            treeView_H_Zuordnen_HOWerte.AddHandler(TreeViewItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(treeView_Item_MouseLeftButtonDown));
            //treeView_H_Zuordnen_HOWerte.AddHandler(TreeViewItem.PreviewMouseRightButtonDownEvent, new MouseButtonEventHandler(treeView_Item_MouseRightButtonDown));
            treeView_H_Zuordnen_HOWerte.AddHandler(TreeViewItem.PreviewMouseDoubleClickEvent, new MouseButtonEventHandler(treeViewItem_MouseDoubleClick));
            //AddHeaderContent();

        }
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

        }

        private void treeView_Item_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            string tag = TreeViewHelper.HitTreeView(treeView_H_Zuordnen_HOWerte, e);

            
            if (TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, TreeViewHelper.TreeViewGetNode_ByTag(treeView_H_Zuordnen_HOWerte, tag)) == 1 )
            {
                TreeViewHelper.ExpandAll(treeView_H_Zuordnen_HOWerte, false);
            }

            if (tag != null)
            {
                foreach (TreeViewItem item in treeView_H_Zuordnen_HOWerte.Items)
                {
                    //Level 1 und 2
                    foreach (TreeViewItem i in item.Items)
                    {
                        if (tag.Contains(Helper.CleanUpString( i.Header.ToString())))
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
                                treeViewItem_Click(i);
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
                                    treeViewItem_Click(a);
                                }
                            }
                        }

                    }

                }

            }

        }
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

        private void treeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            MessageBox.Show("","");
            treeView_H_Zuordnen_HOWerte.Move(ListView_Aufträge);

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
                        TreeViewItem t_parent = TreeViewHelper.GetParent(t);

                        //Fügt Knoten hinzu
                        t.Items.Add(new TreeViewItem() { Header = treeViewItem.Header , Tag = treeViewItem.Tag });
                        //Öffnet Knoten
                        TreeViewHelper.ExpandAll(t, true);
                        //Löscht knoten aus Liste
                        if (treeViewItem.Parent is TreeViewItem)
                        {
                            (treeViewItem.Parent as TreeViewItem).Items.Remove(treeViewItem);
                            //a.Remove(treeViewItem);
                        }

                        foreach (Maschine m in mList)
                        {
                            m.RefreshInformation();
                        }
                    }

                }
                treeViewItem = null;

                e.Handled = true;
            }

        }

        
        private void treeViewItem_Click(TreeViewItem ti)
        {

            DDataContext d = new DDataContext();

            var drt = from h in d.Auftrag
                      where h.ODL == ti.Header.ToString()
                      select h;
            if (drt.Count() > 0)
            {
                Auftrag a = drt.First();

                label_ODL.Content = "ODL: \t" + a.ODL;
            }

        }
   
        private void treeView_H_Zuordnen_HOWerte_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (lMousePressed)
            {
                TreeViewItem i = TreeViewHelper.HitTreeView(treeView_H_Zuordnen_HOWerte, e);
                if (i != null && TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte,i) == 1)
                {
                    i.IsExpanded = true;
                    TreeViewHelper.CloseAll(treeView_H_Zuordnen_HOWerte,i, 2);
                }

            }

        }

  

        void AddHeaderContent() {

            DDataContext d = new DDataContext();
            var ver = from z in d.Verfahren
                      select z.Name;

            foreach (TreeViewItem t in treeView_H_Zuordnen_HOWerte.Items)
            {
                string s = (string)t.Header;
                if (s.Contains(Helper.FREIEAUFTRÄGE_STRING))
                {
                    t.Header = Helper.AddString((string)t.Header, "Anzahl", 3);
                }
                if (s.Contains(Helper.EL1_STRING))
                {
                    t.Header = Helper.AddString((string)t.Header, "Gewicht", 3, "Kg");
                }
                if (s.Contains(Helper.EL2_STRING))
                {
                    t.Header = Helper.AddString((string)t.Header, "Gewicht", 3, "Kg");
                }
                if (s.Contains(Helper.HGO1_STRING))
                {
                    t.Header = Helper.AddString((string)t.Header, "Gewicht", 3, "Kg");
                }
                if (s.Contains(Helper.HGO2_STRING))
                {
                    t.Header = Helper.AddString((string)t.Header, "Gewicht", 3, "Kg");
                }
                if (s.Contains(Helper.RM_STRING))
                {
                    t.Header = Helper.AddString((string)t.Header, "Gewicht", 3, "Kg");
                }

                //Teile Gewichte für die einzelnen Verfahren hinzu
                foreach (TreeViewItem c in t.Items)
                {
                    if (ver.Contains((string)c.Header))
                    {
                        c.Header = Helper.AddString((string)c.Header, "Gewicht", 3, "Kg");
                    }
                }



            }

        }

    }
}
