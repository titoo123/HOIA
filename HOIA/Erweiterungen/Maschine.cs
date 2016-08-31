using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HOIA.Erweiterungen
{
    /// <summary>
    /// Diese Klasse stellt ein virtuelle Maschine dar, die mehrere Prozesse besitzt und verschiedene Jobs übergeben bekommen kann
    /// </summary>
    public class Maschine
    {
        //Stellt die Liste aller im Programm vorhandener Maschinen dar
        //Ist hilfreich wenn Aufzählungen benötigt werden
        public static List<String> maschines = new List<string>();

        string name;
        string tag;
        string auftragskategorie;

        int katAnzahl;
        bool sublevel;

        TreeViewItem item;
        Extended_TreeView exT;

        StringTuple4D processingJobList = new StringTuple4D();

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
        /// Konstruktor
        /// </summary>
        /// <param name="t">Parent Treeview</param>
        /// <param name="name">Name</param>
        /// <param name="sublevel">Ob noch Unterkategorien existieren</param>
        public Maschine(Extended_TreeView exT, string name)
        {


            this.exT = exT;
            this.tag = name;
            this.name = name;
            //this.sublevel = sublevel;

            this.katAnzahl = 0;

            exT.Items.Add(new TreeViewItem() { Header = name, Tag = name });

            Item = exT.TreeViewGetNode_ByText(name);
            RefreshValue();
            exT.TreeViewGetNode_ByText(Helper.FREIEAUFTRÄGE_STRING).IsExpanded = true;
        }

        public Maschine(TreeViewItem t, Extended_TreeView exT, string auftragskategorie, string name, bool sublevel)
        {

            this.auftragskategorie = auftragskategorie;
            this.exT = exT;
            this.tag = name;
            this.name = name;
            this.sublevel = sublevel;

            this.katAnzahl = 0;

            t.Items.Add(new TreeViewItem() { Header = name, Tag = name });
            item = exT.TreeViewGetNode_ByTextAndByKategorie(name, auftragskategorie);
            // maschines.Add(name);

            //Item = t.TreeViewGetNode_ByText(name);
            RefreshValue();
            exT.TreeViewGetNode_ByText(Helper.FREIEAUFTRÄGE_STRING).IsExpanded = true;
        }

        /// <summary>
        /// Bildet Konstruktor der Maschine
        /// </summary>
        /// <param name="t">TreeView</param>
        /// <param name="name">Name des Hauptknotens</param>
        /// <param name="sublevel">Bestitzt der Hauptknoten Unterkategorien?</param>
        /// <param name="fre">Liste der Namen der Unterknoten</param>
        /// <param name="katAnzahl">Anzahl der Unterkategorien die die Unterknoten haben</param>
        public Maschine(TreeViewItem t, Extended_TreeView exT, string auftragskategorie, string name, bool sublevel, IQueryable<string> fre, int katAnzahl)
        {
            this.auftragskategorie = auftragskategorie;
            this.exT = exT;
            this.tag = name;
            this.name = name;
            this.sublevel = sublevel;
            this.katAnzahl = katAnzahl;

            t.Items.Add(new TreeViewItem() { Header = name, Tag = name });
            //maschines.Add(name);
            item = exT.TreeViewGetNode_ByTextAndByKategorie(name, auftragskategorie);
            //item = exT.TreeViewGetNode_ByText(name);


            //Fügt Kategorien hinzu. Zum Beispiel +QT 1
            if (katAnzahl > 1)
            {
                List<string> einträge = new List<string>();

                foreach (string s in fre)
                {
                    for (int i = 1; i < katAnzahl + 1; i++)
                    {

                        einträge.Add(s + " " + i);

                    }
                }
                foreach (string e in einträge)
                {
                    item.Items.Add(new TreeViewItem() { Header = e, Tag = e });
                }


            }
            else
            {
                foreach (string e in fre)
                {
                    item.Items.Add(new TreeViewItem() { Header = e, Tag = e });
                }

            }

            RefreshValue();
            exT.TreeViewGetNode_ByText(Helper.FREIEAUFTRÄGE_STRING).IsExpanded = true;
        }

        private void RefreshValue(TreeViewItem item)
        {
            TreeViewItem ti = item;
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




            }
            else
            {
                ////Anzahl freie Aufträge
                //if ((string)Item.Tag == Helper.FREIEAUFTRÄGE_STRING)
                //{
                //    Item.Header = Helper.CleanUpString((string)Item.Header) + " ( " + Item.Items.Count + " )";
                //}
                //else
                if ((string)Item.Tag == Helper.RM_STRING)
                {
                    int? gewicht = 0;
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

        /// <summary>
        /// Gewicht der Haupknoten für Elemente der List
        /// </summary>
        /// <param name="s">ODL</param>
        /// <param name="z">Prozess</param>
        internal void AddJobToList(string s, string z, bool wichtig)
        {
            //if (wichtig)
            //{
            //    processingJobList.Add(s, z, "!");
            //}
            //else
            //{
            //    processingJobList.Add(s, z, " ");
            //}


            //Added Gewicht auf Hauptknoten
            DDataContext d = new DDataContext();
            int? gewicht = 0;

            foreach (var a in processingJobList.Items)
            {
                var gew = from g in d.Auftrag
                          where g.ODL == a.Item1
                          select g;
                //Sammelt gewicht der Materialen des Auftrags 
                foreach (var m in gew)
                {
                    int? mag = (from k in d.Material
                                where k.Id_Auftrag == m.Id
                                select k.Gewicht).Sum();
                    gewicht = gewicht + mag;
                }
            }

            item.Header = Helper.CleanUpString((string)item.Header) + " ( " + gewicht + " Kg )";
        }

        internal void DeleteJobFormList(string s)
        {
            processingJobList.RemoveByFirst(s);
        }

        internal void RefreshJobInList(string s)
        {
            Tuple<string, string, string> items = null;

            foreach (var i in processingJobList.Items)
            {
                if (i.Item1 == s)
                {
                    DDataContext d = new DDataContext();
                    var wic = from r in d.Auftrag
                              where r.ODL == s
                              select r;
                    if ((bool)wic.First().Wichtig)
                    {
                        items = Tuple.Create(i.Item1, i.Item2, "!");
                    }
                    else
                    {
                        items = Tuple.Create(i.Item1, i.Item2, " ");
                    }

                }
            }
            if (items != null)
            {
                //processingJobList.Refresh(items);
            }

        }

        /// <summary>
        /// Fügt Elemente von Maschinenlist in Übersichtslist
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
                foreach (TreeViewItem i in exT.Items)
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

