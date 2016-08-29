using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HOIA.Erweiterungen
{
    class Auftragskategorie
    {
        public static List<Auftragskategorie> auftragskategorien = new List<Erweiterungen.Auftragskategorie>();

        Extended_TreeView t;
        List<Maschine> m = new List<Maschine>();
        string name;
        TreeViewItem item;

        public Auftragskategorie(Extended_TreeView t, string name)
        {
            //this.name = name;
            //t.Items.Add(new TreeViewItem() { Header = name, Tag = name });
            //item = t.TreeViewGetNode_ByText(name);

            //DDataContext d = new DDataContext();
            ////Verfahren
            ////Haubenofen
            //var hoa = from r in d.Verfahren
            //          where r.Art == Helper.HAUBENOFEN_ART
            //          select r.Name;
            ////Induktionsanlagen
            //var iaa = from r in d.Verfahren
            //          where r.Art == Helper.INDUKTIONSANLAGEN_ART
            //          select r.Name;
            ////Freie Aufträge
            //var fre = from f in d.Auftrag
            //          where f.Status == Helper.AUFTRAG_OFFEN_STRING
            //          select f.ODL;

            //if (name != Helper.FREIEAUFTRÄGE_STRING)
            //{
            //    m.Add(new Maschine(item, t, name, Helper.EL1_STRING, true, iaa, 3));
            //    m.Add(new Maschine(item, t, name, Helper.EL2_STRING, true, iaa, 2));
            //    m.Add(new Maschine(item, t, name, Helper.HGO1_STRING, true, hoa, 1));
            //    m.Add(new Maschine(item, t, name, Helper.HGO2_STRING, true, hoa, 1));
            //    m.Add(new Maschine(item, t, name, Helper.RM_STRING, false));
            //}
            //else
            //{
            //    foreach (string a in fre)
            //    {
            //        item.Items.Add(new TreeViewItem() { Header = a, Tag = a});
            //    }

            //}

            //auftragskategorien.Add(this);
        }

        internal void Count() {

        }
    }
}
