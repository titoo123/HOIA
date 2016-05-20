using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HOIA.Erweiterungen
{
    public class TreeViewHelper
    {
        public static void CreateChild(TreeViewItem t, string s)
        {
            t.Items.Add(new TreeViewItem() { Header = s });
        }
        public static List<TreeViewItem> CreateChilds(List<string> l)
        {
            List<TreeViewItem> t = new List<TreeViewItem>();
            foreach (var i in l)
            {
                t.Add(new TreeViewItem() { Header = i });
            }
            return t;
        }
        public static TreeViewItem TreeViewGetNode_ByTag(TreeView t, string s)
        {
            foreach (TreeViewItem tre in t.Items)
            {
                if (((String)tre.Tag).Contains(s))
                {
                    return tre;
                }
                if (tre.HasItems)
                {
                    TreeViewItem t1 = GetNode_ByTag(tre, s);
                    if (t1 != null)
                    {
                        return t1;
                    }
                }
            }
            return null;
        }
        private static TreeViewItem GetNode_ByTag(TreeViewItem t, string s)
        {
            if (((String)t.Tag).Contains(s))
            {
                return t;
            }

            foreach (TreeViewItem tre in t.Items)
            {
                if (t != null)
                {
                    if (((String)tre.Tag).Contains(s))
                    {
                        return tre;
                    }
                    if (tre.HasItems)
                    {
                        TreeViewItem t1 = GetNode_ByTag(tre, s);
                        if (t1 != null)
                        {
                            return t1;
                        }

                    }
                }

            }
            return null;
        }
        public static TreeViewItem TreeViewGetNode_ByText(TreeView t, string s)
        {
            foreach (TreeViewItem tre in t.Items)
            {
                if (((String)tre.Header).Contains(s))
                {
                    return tre;
                }
                if (tre.HasItems)
                {
                    TreeViewItem t1 = GetNode_ByText(tre, s);
                    if (t1 != null)
                    {
                        return t1;
                    }
                }
            }
            return null;
        }
        private static TreeViewItem GetNode_ByText(TreeViewItem t, string s)
        {
            if (((String)t.Header).Contains(s))
            {
                return t;
            }

            foreach (TreeViewItem tre in t.Items)
            {
                if (t != null)
                {
                    if (((String)tre.Header).Contains(s))
                    {
                        return tre;
                    }
                    if (tre.HasItems)
                    {
                        TreeViewItem t1 = GetNode_ByText(tre, s);
                        if (t1 != null)
                        {
                            return t1;
                        }

                    }
                }

            }
            return null;
        }
        internal static void CreateChilds(TreeViewItem i, IQueryable<object> hoa)
        {
            foreach (var item in hoa)
            {
                CreateChild(i, TreeViewHelper.CleanUp4List(item.ToString()));
            }
        }
        internal static void CreateChilds(TreeView t, IQueryable<object> hoa, string v)
        {
            TreeViewItem i = TreeViewGetNode_ByText(t, v);
            if (i != null)
            {
                CreateChilds(i, hoa);
            }

        }
        internal static bool InDb(TreeViewItem i)
        {
            DDataContext d = new DDataContext();

            var kjh = from l in d.Verfahren
                      select l;

            if (i != null)
            {
                foreach (var item in kjh)
                {
                    if ((string)i.Header == item.Name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        internal static bool InDb(string j)
        {
            string i = Helper.CleanUpString (j);
            DDataContext d = new DDataContext();

            var kjh = from l in d.Verfahren
                      select l;

            if (i != null)
            {
                foreach (var item in kjh)
                {
                    if (i == item.Name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal static string HitTreeView(TreeView t, MouseButtonEventArgs e)
        {

            DependencyObject uie = t.InputHitTest(e.GetPosition(t)) as DependencyObject;
            TextBlock b = uie as TextBlock;
            string s = String.Empty;

            if (b != null)
            {
                s = b.DataContext.ToString();

                // TreeViewItem ti = TreeViewGetNode_ByText(t, s);
            }

            return s;

        }
        internal static TreeViewItem HitTreeView(TreeView t, MouseEventArgs e)
        {

            DependencyObject uie = t.InputHitTest(e.GetPosition(t)) as DependencyObject;
            TextBlock b = uie as TextBlock;
            string s = String.Empty;
            TreeViewItem ti = null;

            if (b != null)
            {
                s = b.DataContext.ToString();

                ti = TreeViewGetNode_ByText(t, s);
            }

            return ti;

        }
        internal static void CloseAll(TreeView t, TreeViewItem i, int v)
        {
            if (GetNodeLevel(t, i) != v)
            {
                foreach (TreeViewItem item in t.Items)
                {
                    if (item.Header != i.Header && (string)item.Header != Helper.FREIEAUFTRÄGE_STRING)
                    {
                        item.IsExpanded = false;
                    }
                    else
                    {
                        item.IsExpanded = true;
                    }
                }
            }

        }
        public static void ExpandAll(TreeView t, bool expand)
        {
            foreach (TreeViewItem i in t.Items)
            {
                if ((string)i.Header != Helper.FREIEAUFTRÄGE_STRING)
                {
                    i.IsExpanded = expand;
                }


            }
        }
        public static void ExpandAll(TreeViewItem t, bool expand)
        {
            t.IsExpanded = expand;
            if (t.HasItems)
            {

                foreach (TreeViewItem item in t.Items)
                {
                    item.IsExpanded = expand;
                }
            }

        }
        public static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }
        public static int GetNodeLevel(TreeView t, TreeViewItem i)
        {

            int index = 0;
            int indexOfSelectedNode = 0;

            if (t.Items.Count >= 0)
            {
                if (i != null)
                {
                    index++;

                    TreeViewItem item = i;

                    ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(item);

                    while (parent != null && parent.GetType() == typeof(TreeViewItem))
                    {
                        index++;

                        parent = ItemsControl.ItemsControlFromItemContainer(parent);

                    }

                    indexOfSelectedNode = index;

                }

            }

            return index;
        }
        public static int GetTreeLevel(TreeView t)
        {
            int index = 0;
            int indexOfSelectedNode = 0;

            if (t.Items.Count >= 0)

            {
                if (t.SelectedValue != null)
                {
                    index++;

                    TreeViewItem item = t.SelectedItem as TreeViewItem;

                    ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(item);

                    while (parent != null && parent.GetType() == typeof(TreeViewItem))
                    {
                        index++;

                        parent = ItemsControl.ItemsControlFromItemContainer(parent);

                    }

                    indexOfSelectedNode = index;

                }

            }

            //MessageBox.Show(indexOfSelectedNode.ToString());
            return indexOfSelectedNode;
        }
        public static string CleanUp4List(string s)
        {
            s = s.Replace("{ Name = ", "");
            s = s.Replace(" }", "");
            return s;
        }

        public static TreeViewItem GetParent(TreeViewItem t) {
            ItemsControl parent = GetSelectedTreeViewItemParent(t);
            TreeViewItem t_parent = parent as TreeViewItem;
            return t_parent;
        }

        public static ItemsControl GetSelectedTreeViewItemParent(TreeViewItem item)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(item);
            while (!(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as ItemsControl;
        }
        
    }


}
