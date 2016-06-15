using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        List<string> a;

        public string Tag
        {
            get
            {
                return tag;
            }

            set
            {
                tag = value;
            }
        }

        public TreeViewItem Item
        {
            get
            {
                return item;
            }

            set
            {
                item = value;
            }
        }

        //Tuple l = new Tuple();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t">Parent Treeview</param>
        /// <param name="name">Name</param>
        /// <param name="sublevel">Ob noch Unterkategorien existieren</param>
        public Maschine(Extended_TreeView t, string name, bool sublevel)
        {

            this.t = t;
            this.tag = name;
            this.name = name;
            this.sublevel = sublevel;

            a = new List<string>();

            t.Items.Add(new TreeViewItem() { Header = name, Tag = name });

            Item = t.TreeViewGetNode_ByText(name);
            RefreshValue();

        }

        public Maschine(Extended_TreeView t, string name, bool sublevel, IQueryable<string> fre)
        {

            this.t = t;
            this.tag = name;
            this.name = name;
            this.sublevel = sublevel;

            a = new List<string>();

            t.Items.Add(new TreeViewItem() { Header = name, Tag = name });

            Item = t.TreeViewGetNode_ByText(name);
            t.CreateChilds(fre, name);
            //RefreshInformation();
            RefreshValue();
            t.TreeViewGetNode_ByText(Helper.FREIEAUFTRÄGE_STRING).IsExpanded = true;
        }

        internal void RefreshValue()
        {

            if (sublevel)
            {

                DDataContext d = new DDataContext();

                foreach (TreeViewItem j in Item.Items)
                {
                    int? gewicht = 0;
                    foreach (TreeViewItem i in j.Items)
                    {
                        var gew = from g in d.Auftrag
                                  where g.ODL == (string)i.Tag
                                  select g;

                        foreach (var m in gew)
                        {
                            int? mag = (from k in d.Material
                                        where k.Id_Auftrag == m.Id
                                        select k.Gewicht).Sum();
                            gewicht = gewicht + mag;
                        }
                    }
                    j.Header = Helper.CleanUpString((string)j.Header) + " ( " + gewicht + " Kg )";
                }

                //item.Header = Helper.CleanUpString((string)item.Header) + " ( " + gewicht + " Kg )";
            }
            else
            {   //Anzahl freie Aufträge
                if ((string)Item.Tag == Helper.FREIEAUFTRÄGE_STRING)
                {
                    Item.Header = Helper.CleanUpString((string)Item.Header) + " ( " + Item.Items.Count + " )";
                }

                if ((string)Item.Tag == Helper.RM_STRING)
                {
                    //string gewicht = String.Empty;
                    int? gewicht = 0;
                    //List<TreeViewItem> childs = new List<TreeViewItem>();
                    DDataContext d = new DDataContext();

                    foreach (TreeViewItem i in Item.Items)
                    {
                        var gew = from g in d.Auftrag
                                  where g.ODL == (string)i.Tag
                                  select g;

                        foreach (var m in gew)
                        {
                            int? mag = (from k in d.Material
                                        where k.Id_Auftrag == m.Id
                                        select k.Gewicht).Sum();
                            gewicht = gewicht + mag;
                        }
                    }
                    Item.Header = Helper.CleanUpString((string)Item.Header) + " ( " + gewicht + " Kg )";
                }






            }
        }

        internal void AddJobToList(string s)
        {
            this.a.Add(s);
        }
        internal List<string> GetJobList()
        {
            return this.a;
        }
        public ListView ReplaceListViewItems(ListView l, string _tag)
        {
            if (_tag != null)
            {
                //Leert Liste 
                //l.Items.Clear();
                foreach (TreeViewItem item in t.Items)
                {
                    //Level 1
                    //Fügt Maschinenjobs/Aufträge in Liste
                    //Beim Linksklicken auf die Maschinen
                    if (_tag == (string)item.Tag || _tag.Contains("RM"))
                    {
                        if (_tag.Contains(this.Tag))
                        {

                            foreach (string j in this.a)
                            {
                                if (!l.Items.Contains(j))
                                {
                                    l.Items.Add(j);
                                }

                            }

                        }
                    }
                }
            }

            return l;
        }

    }
}

