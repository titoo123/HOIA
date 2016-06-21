﻿using System;
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
        public static List<String> maschines = new List<string>();

        string name;
        string tag;

        int jobWeight;

        bool sublevel;

        TreeViewItem item;
        Extended_TreeView t;

        StringTuple2D processingJobList = new StringTuple2D();

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

            t.Items.Add(new TreeViewItem() { Header = name, Tag = name });
            maschines.Add(name);

            Item = t.TreeViewGetNode_ByText(name);
            RefreshValue();

        }

        public Maschine(Extended_TreeView t, string name, bool sublevel, IQueryable<string> fre)
        {

            this.t = t;
            this.tag = name;
            this.name = name;
            this.sublevel = sublevel;

            t.Items.Add(new TreeViewItem() { Header = name, Tag = name });
            maschines.Add(name);

            Item = t.TreeViewGetNode_ByText(name);
            t.CreateChilds(fre, name);

            RefreshValue();
            t.TreeViewGetNode_ByText(Helper.FREIEAUFTRÄGE_STRING).IsExpanded = true;
        }

        internal void RefreshValue()
        {
            if (sublevel)
            {
                //item.Header = Helper.CleanUpString((string)item.Header) + " ( " + GetJobWeight() + " Kg )";

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




            }
            else
            {   //Anzahl freie Aufträge
                if ((string)Item.Tag == Helper.FREIEAUFTRÄGE_STRING)
                {
                    Item.Header = Helper.CleanUpString((string)Item.Header) + " ( " + Item.Items.Count + " )";
                }else if ((string)Item.Tag == Helper.RM_STRING)
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

        internal void RefreshJobWeight()
        {
            //if (!name.Contains(Helper.AUFTRAG_OFFEN_STRING) && !name.Contains(Helper.FERTIGEAUFTRÄGE_STRING))
            //{
            //    item.Header = Helper.CleanUpString((string)item.Header) + " ( " + jobWeight.ToString() + " Kg )";
            //}

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">ODL</param>
        /// <param name="z">Prozess</param>
        internal void AddJobToList(string s,string z)
        {
            processingJobList.Add(s,z);

            //Added Gewicht auf Hauptknoten
            DDataContext d = new DDataContext();
            int? gewicht = 0;
          
                var gew = from g in d.Auftrag
                          where g.ODL == s
                          select g;

                foreach (var m in gew)
                {
                    int? mag = (from k in d.Material
                                where k.Id_Auftrag == m.Id
                                select k.Gewicht).Sum();
                    gewicht = gewicht + mag;
                }
            
            item.Header = Helper.CleanUpString((string)item.Header) + " ( " + gewicht + " Kg )";
        }

        /// <summary>
        /// Fügt Elmente von Maschineenlist in Übersichtslist
        /// </summary>
        /// <param name="l">Listview</param>
        /// <param name="_tag">Header des Treeviewitems</param>
        /// <returns></returns>
        public ListView ReplaceListViewItems(ListView l, string _tag)
        {
            if (_tag != null)
            {
                //Leert Liste 
                //l.Items.Clear();
                foreach (TreeViewItem i in t.Items)
                {
                    //Level 1
                    //Fügt Maschinenjobs/Aufträge in Liste
                    //Beim Linksklicken auf die Maschinen
                    if (_tag.Contains((string)i.Tag) || _tag.Contains("RM"))
                    {
                        if (_tag.Contains(this.Tag))
                        {
                            if (processingJobList.Items != null)
                            {
                                foreach (var j in processingJobList.Items)
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
            }

            return l;
        }

    }
}

