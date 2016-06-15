using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HOIA.Erweiterungen
{
    class Tuple2D
    {
        List<Tuple<string, string>> a = new List<Tuple<string, string>>();

        internal void Add(TreeViewItem i1, TreeViewItem i2)
        {
            if (!a.Contains(new Tuple<string, string>((string)i1.Header, (string)i2.Header )))
            {
                a.Add(new Tuple<string, string>((string)i1.Header, (string)i2.Header));
            }

        }

        internal void Remove(TreeViewItem i1)
        {
            a.RemoveAll(item => item.Item2 == (string)i1.Header);
        }

        internal List<Tuple<string, string>> Get(TreeViewItem selected)
        {
            List<Tuple<string, string>> t = new List<Tuple< string, string>>();
            t = a.GetRange(0, a.Count);

            t.RemoveAll(item => item.Item2 != (string)selected.Header);

            return t;
        }
    }
}
