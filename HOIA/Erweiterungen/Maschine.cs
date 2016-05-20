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

        Tuple l = new Tuple();

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
            RefreshInformation();
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
            RefreshInformation();

            t.TreeViewGetNode_ByText(Helper.FREIEAUFTRÄGE_STRING).IsExpanded = true;
        }

        internal void RefreshInformation()
        {
            //Anzahl freier Aufträge
            if ((string)item.Tag == Helper.FREIEAUFTRÄGE_STRING)
            {
                item.Header = Helper.CleanUpString((string)item.Header) + " ( " + item.Items.Count + " )";
            }
            if ((string)item.Tag == Helper.EL1_STRING)
            {
                item.Header = Helper.CleanUpString((string)item.Header) + " ( " + item.Items.Count + " Kg )";
            }
        }

        internal void MakeValue() {

            if (sublevel)
            {

            }
            else
            {   //Anzahl freie Aufträge
                if ((string)item.Tag == Helper.FREIEAUFTRÄGE_STRING)
                {
                    item.Header = Helper.CleanUpString((string)item.Header) + " ( " + item.Items.Count + " )";
                }
                else
                {
                    string gewicht = String.Empty;
                    //List<TreeViewItem> childs = new List<TreeViewItem>();
                    DDataContext d = new DDataContext();
                    foreach (TreeViewItem i in item.Items)
                    {
                        var gew = from g in d.Auftrag
                                  where g.ODL == (string)item.Tag
                                  select g;
                        foreach (var m in gew)
                        {
                            var mag = (from k in d.Material
                                       where k.Id_Auftrag == m.Id
                                       select k.Gewicht).Sum();

                        }
                    }
                       
                    //string gewicht = item.Tag as string;
                }



            }
        }
    }
}
