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
    public class Extended_TreeView : TreeView
    {
        public static void CreateChild(TreeViewItem t, string s)
        {
            t.Items.Add(new TreeViewItem() { Header = s, Tag = s });
        }
        public static List<TreeViewItem> CreateChilds(List<string> l)
        {
            List<TreeViewItem> t = new List<TreeViewItem>();
            foreach (var i in l)
            {
                t.Add(new TreeViewItem() { Header = i, Tag = i });
            }
            return t;
        }
        public TreeViewItem TreeViewGetNode_ByText( string s)
        {
            foreach (TreeViewItem tre in this.Items)
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
        internal void CreateChilds( IQueryable<object> hoa, string v)
        {
            TreeViewItem i = TreeViewGetNode_ByText(v);
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

        internal void CreateChilds(List<string> einträge, string name)
        {
            TreeViewItem i = TreeViewGetNode_ByText(name);
            if (i != null)
            {
                CreateChilds(i, einträge);
            }
        }

        private void CreateChilds(TreeViewItem i, List<string> einträge)
        {
            foreach (var item in einträge)
            {
                CreateChild(i, TreeViewHelper.CleanUp4List(item.ToString()));
            }
        }

        internal string HitTreeView( MouseButtonEventArgs e)
        {
            DependencyObject uie = this.InputHitTest(e.GetPosition(this)) as DependencyObject;
            TextBlock b = uie as TextBlock;
            string s = String.Empty;

            if (b != null)
            {
                s = b.DataContext.ToString();
            }

            return s;

        }
        internal TreeViewItem HitTreeView(MouseEventArgs e)
        {

            DependencyObject uie = this.InputHitTest(e.GetPosition(this)) as DependencyObject;
            TextBlock b = uie as TextBlock;
            string s = String.Empty;
            TreeViewItem ti = null;

            if (b != null)
            {
                s = b.DataContext.ToString();

                ti = this.TreeViewGetNode_ByText(s);
            }

            return ti;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i">Item</param>
        /// <param name="v">Level</param>
        /// <param name="a">Ausnahme</param>
        internal void CloseAllInLevel(TreeViewItem i, int v, string a)
        {
            if (GetNodeLevel(i) != v)
            {
                foreach (TreeViewItem item in this.Items)
                {
                    if (item.Header != i.Header && (string)item.Header != a)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expand">Bool Wahlvariable</param>
        /// <param name="a">Ausnahme</param>
        public void ExpandOrCloseAll( bool expand, string a)
        {
                foreach (TreeViewItem item in this.Items)
                {
                    item.IsExpanded = expand;
                }

        }
        public DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }
        public int GetNodeLevel( TreeViewItem i)
        {

            int index = 0;
            int indexOfSelectedNode = 0;

            if (this.Items.Count >= 0)
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
        public int GetTreeLevel()
        {
            int index = 0;
            int indexOfSelectedNode = 0;

            if (this.Items.Count >= 0)

            {
                if (this.SelectedValue != null)
                {
                    index++;

                    TreeViewItem item = this.SelectedItem as TreeViewItem;

                    ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(item);

                    while (parent != null && parent.GetType() == typeof(TreeViewItem))
                    {
                        index++;

                        parent = ItemsControl.ItemsControlFromItemContainer(parent);

                    }

                    indexOfSelectedNode = index;

                }

            }
            return indexOfSelectedNode;
        }
        public static string CleanUp4List(string s)
        {
            s = s.Replace("{ Name = ", "");
            s = s.Replace(" }", "");
            return s;
        }

        public static TreeViewItem GetParent(TreeViewItem t)
        {
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

        public void Move(ListView l)
        {
            TreeViewItem t = SelectedItem as TreeViewItem;
            if (t != null)
            {
                l.Items.Add((string)t.Header);
            }



            //TreeViewItem newChild = new TreeViewItem();
            //TreeViewItem selected = new TreeViewItem();
            //// Unboxing
            //MenuItem menuItem = sender as MenuItem;
            //newChild.Header = menuItem.Header;

            //selected = (TreeViewItem)this.SelectedItem;

            //foreach (var item in a.Get(selected))
            //{
            //    if (!l.Items.Contains(item))
            //    {
            //        l.Items.Add(item);
            //    }

            //}
        }
    }
}
