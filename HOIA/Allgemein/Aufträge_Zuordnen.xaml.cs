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
        Erweiterungen.Tuple a = new Erweiterungen.Tuple();


        public Aufträge_Zuordnen()
        {
            InitializeComponent();
            TreeView_H_Zuordnen_HOWerte_Refresh();
            treeView_H_Zuordnen_HOWerte.AddHandler(TreeViewItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(treeView_Item_MouseLeftButtonDown));
            treeView_H_Zuordnen_HOWerte.AddHandler(TreeViewItem.PreviewMouseRightButtonDownEvent, new MouseButtonEventHandler(treeView_Item_MouseRightButtonDown));

        }
        private void TreeView_H_Zuordnen_HOWerte_Refresh()
        {
            DDataContext d = new DDataContext();
            //Verfahren
            //Haubenofen
            var hoa = from r in d.Verfahren
                      where r.Art == Helper.HAUBENOFEN_ART
                      select new { r.Name };
            //Induktionsanlagen
            var iaa = from r in d.Verfahren
                      where r.Art == Helper.INDUKTIONSANLAGEN_ART
                      select new { r.Name };
            //Freie Aufträge
            var fre = from f in d.Auftrag
                      where f.Status == Helper.AUFTRAG_OFFEN_STRING
                      select f.ODL;

            FillTreeView(treeView_H_Zuordnen_HOWerte);
            
            TreeViewHelper.CreateChilds(treeView_H_Zuordnen_HOWerte, fre, Helper.FREIEAUFTRÄGE_STRING);

            TreeViewHelper.CreateChilds(treeView_H_Zuordnen_HOWerte, iaa, Helper.EL1_STRING);
            TreeViewHelper.CreateChilds(treeView_H_Zuordnen_HOWerte, iaa, Helper.EL2_STRING);

            TreeViewHelper.CreateChilds(treeView_H_Zuordnen_HOWerte, hoa, Helper.HGO1_STRING);
            TreeViewHelper.CreateChilds(treeView_H_Zuordnen_HOWerte, hoa, Helper.HGO2_STRING);

            TreeViewItem i = TreeViewHelper.TreeViewGetNode_ByText(treeView_H_Zuordnen_HOWerte, Helper.FREIEAUFTRÄGE_STRING);
            i.IsExpanded = true;
            // Helper.ExpandAll(treeView_H_Zuordnen_HOWerte, true);

        }
        private void FillTreeView(TreeView treeView_H_Zuordnen_HOWerte)
        {
            List<string> l = new List<string>();

            l.Add(Helper.FREIEAUFTRÄGE_STRING);

            l.Add(Helper.EL1_STRING);
            l.Add(Helper.EL2_STRING);
            l.Add(Helper.HGO1_STRING);
            l.Add(Helper.HGO2_STRING);
            l.Add(Helper.RM_STRING);

            treeView_H_Zuordnen_HOWerte.ItemsSource = TreeViewHelper.CreateChilds(l);

        }

        private void treeView_Item_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            string header = TreeViewHelper.HitTreeView(treeView_H_Zuordnen_HOWerte, e);

            
            if (TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, TreeViewHelper.TreeViewGetNode_ByText(treeView_H_Zuordnen_HOWerte, header)) == 1 )
            {
                TreeViewHelper.ExpandAll(treeView_H_Zuordnen_HOWerte, false);
            }

            if (header != null)
            {
                foreach (TreeViewItem item in treeView_H_Zuordnen_HOWerte.Items)
                {
                    //Level 1 und 2
                    foreach (TreeViewItem i in item.Items)
                    {
                        if (header == i.Header.ToString())
                        {
                            if ((TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, i) == 2 
                             && (item.Header.ToString() == Helper.FREIEAUFTRÄGE_STRING || item.Header.ToString() == Helper.RM_STRING)))
                            {
                                lMousePressed = true;
                                treeViewItem = i;
                                treeViewItem_Click(i);
                               // DragDropEffects d = DragDrop.DoDragDrop(i,)
                            }
                        }
                        //Level 3
                        foreach (TreeViewItem a in i.Items)
                        {
                            if (header == a.Header.ToString())
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
                ContextMenu contextMenu = new ContextMenu();

                MenuItem m = new MenuItem();
                m.Header = "Freigeben";

                m.Click += OptionClick;
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

                            selectedItem.ContextMenu = contextMenu;

                            contextMenu.Items.Add(m);
                            selectedItem.ContextMenu.IsOpen = true;


                        }
                    }
                }
            }


        }
        private void treeView_H_Zuordnen_HOWerte_MouseUp(object sender, MouseButtonEventArgs e)
        {   
            lMousePressed = false;
            TreeViewItem t = TreeViewHelper.VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem != null)
            {
                if ((TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, t) == 1 && (t.Header.ToString() == Helper.FREIEAUFTRÄGE_STRING || t.Header.ToString() == Helper.RM_STRING)
                    || TreeViewHelper.GetNodeLevel(treeView_H_Zuordnen_HOWerte, t) == 2 && (t.Header.ToString() == Helper.FREIEAUFTRÄGE_STRING || t.Header.ToString() == Helper.RM_STRING))
                    || TreeViewHelper.InDb(t)
                    && t.Header != treeViewItem.Header)
                {
                    TreeViewItem t_parent = TreeViewHelper.GetParent(t);
                    
                    //Fügt Knoten hinzu
                    t.Items.Add(new TreeViewItem() { Header = treeViewItem.Header });
                    //Öffnet Knoten
                    TreeViewHelper.ExpandAll(t, true);
                    //Löscht knoten aus Liste
                    if (treeViewItem.Parent is TreeViewItem)
                    {
                        (treeViewItem.Parent as TreeViewItem).Items.Remove(treeViewItem);
                        a.Remove(treeViewItem);
                    }
                    //Fügt Daten für Liste hinzu
                    if (t_parent != null)
                    {
                        a.Add(t_parent, t, treeViewItem);
                    }
                    else
                    {
                        a.Add(t, treeViewItem);
                    }

                }

            }
            treeViewItem = null;

            e.Handled = true;
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

        void OptionClick(object sender, RoutedEventArgs e)
        {
            TreeViewItem newChild = new TreeViewItem();
            TreeViewItem selected = new TreeViewItem();
            // Unboxing
            MenuItem menuItem = sender as MenuItem;
            newChild.Header = menuItem.Header;

            selected = (TreeViewItem)treeView_H_Zuordnen_HOWerte.SelectedItem;

            foreach (var item in a.Get(selected))
            {
                if ( !ListView_Aufträge.Items.Contains(item) )
                {
                    ListView_Aufträge.Items.Add(item);
                }

            }

        }


    }
}
