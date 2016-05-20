using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HOIA.Erweiterungen
{
    public class Maschine
    {
        string name;
        string tag;

        bool sublevel;

        TreeViewItem item;
        Extended_TreeView t;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t">Parent Treeview</param>
        /// <param name="name">Name</param>
        /// <param name="sublevel">Ob noch Unterkategorien existieren</param>
        public Maschine(Extended_TreeView t, string name, bool sublevel ) {

            this.t = t;
            this.tag = name;
            this.name = name;
            this.sublevel = sublevel;

            t.Items.Add(new TreeViewItem() { Header = name, Tag = name });

            item = t.TreeViewGetNode_ByText(name);
            AddInformation();
        }

        public Maschine(Extended_TreeView t, string name, bool sublevel, IQueryable<string> fre)
        {

            this.t = t;
            this.tag = name;
            this.name = name;
            this.sublevel = sublevel;

            t.Items.Add(new TreeViewItem() { Header = name, Tag = name });

            item = t.TreeViewGetNode_ByText(name);
            t.CreateChilds(fre, name);
            AddInformation();

            t.TreeViewGetNode_ByText(Helper.FREIEAUFTRÄGE_STRING).IsExpanded = true;
        }

        void AddInformation()
        {
            if ((string)item.Tag == Helper.FREIEAUFTRÄGE_STRING)
            {
                item.Header = (string)item.Header + " ( " + item.Items.Count + " )";
            }

        }
        internal void RefreshInformation()
        {
            if ((string)item.Tag == Helper.FREIEAUFTRÄGE_STRING)
            {
                item.Header = Helper.CleanUpString((string)item.Header) + " ( " + item.Items.Count + " )";
            }
        }
    }
}
