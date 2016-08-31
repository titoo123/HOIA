using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HOIA.Erweiterungen
{
    public class StringTuple4D
    {
        List<Tuple<string, string, string, string>> items = new List<Tuple<string, string, string, string>>();

        public List<Tuple<string, string, string, string>> Items
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
        internal void Add(string i1, string i2, string i3, string i4)
        {
            if (!Items.Contains(new Tuple<string, string,string, string>(i1, i2, i3, i4)))
            {
                Items.Add(new Tuple<string, string, string,string>(i1, i2, i3, i4));
            }

        }
        internal void RemoveByFirst(string i1)
        {
            Items.RemoveAll(item => item.Item1 == i1);
        }
        internal List<Tuple<string, string, string, string>> Get(TreeViewItem selected)
        {
            List<Tuple<string, string, string, string>> t = new List<Tuple<string, string, string, string>>();
            t = Items.GetRange(0, Items.Count);

            t.RemoveAll(item => item.Item2 != (string)selected.Header);

            return t;
        }
        internal void Refresh(Tuple<string, string, string, string> its)
        {
            this.RemoveByFirst(its.Item1);
            items.Add(its);
        }
    }
}
