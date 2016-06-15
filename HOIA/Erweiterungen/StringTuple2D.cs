using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HOIA.Erweiterungen
{
    class StringTuple2D
    {
        List<Tuple<string, string>> items = new List<Tuple<string, string>>();

        public List<Tuple<string, string>> Items
        {
            get
            {
                return items;
            }

        }

        internal void Add(string i1, string i2)
        {
            if (!Items.Contains(new Tuple<string, string>(i1, i2)))
            {
                Items.Add(new Tuple<string, string>(i1, i2));
            }

        }
        internal void Add(TreeViewItem i1, TreeViewItem i2)
        {
            if (!Items.Contains(new Tuple<string, string>((string)i1.Header, (string)i2.Header )))
            {
                Items.Add(new Tuple<string, string>((string)i1.Header, (string)i2.Header));
            }

        }
        internal void Remove(string i1)
        {
            Items.RemoveAll(item => item.Item2 == i1);
        }
        internal void Remove(TreeViewItem i1)
        {
            Items.RemoveAll(item => item.Item2 == (string)i1.Header);
        }

        //internal List<Tuple<string, string>> Get(TreeViewItem selected)
        //{
        //    List<Tuple<string, string>> t = new List<Tuple< string, string>>();
        //    t = items.GetRange(0, items.Count);

        //    t.RemoveAll(item => item.Item2 != (string)selected.Header);

        //    return t;
        //}

    }
}
