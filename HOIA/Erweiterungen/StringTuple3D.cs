using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HOIA.Erweiterungen
{
    public class StringTuple3D
    {
        List<Tuple<string, string, string>> items = new List<Tuple<string, string, string>>();

        public List<Tuple<string, string, string>> Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
            }
        }

        internal void Add(string i1, string i2, string i3)
        {
            if (!Items.Contains(new Tuple<string, string,string>(i1, i2, i3)))
            {
                Items.Add(new Tuple<string, string, string>(i1, i2, i3));
            }

        }
        internal void Add(TreeViewItem i1, TreeViewItem i2, TreeViewItem i3) {
            if (!Items.Contains(new Tuple<string, string, string>((string)i1.Header, (string)i2.Header, (string)i3.Header)))
            {
                Items.Add(new Tuple<string, string, string>((string)i1.Header, (string)i2.Header, (string)i3.Header));
            }

        }
        internal void Add(TreeViewItem i1, TreeViewItem i2)
        {
            if (!Items.Contains(new Tuple<string, string, string>("",(string)i1.Header, (string)i2.Header)))
            {
                Items.Add(new Tuple<string, string, string>("", (string)i1.Header, (string)i2.Header));
            }
        }

        internal void Remove(TreeViewItem i1)
        {
            Items.RemoveAll(item => item.Item1 == (string)i1.Header);
        }
        internal void Remove(string i1)
        {
            Items.RemoveAll(item => item.Item1 == i1);
        }
        internal List<Tuple<string, string, string>> Get(TreeViewItem selected)
        {
            List<Tuple<string, string, string>> t = new List<Tuple<string, string, string>>();
            t = Items.GetRange(0, Items.Count);

            t.RemoveAll(item => item.Item2 != (string)selected.Header);

            return t;
        }

        internal void Refresh(Tuple<string, string, string> its)
        {
            this.Remove(its.Item1);
            items.Add(its);
        }
    }
}
