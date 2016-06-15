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
        List<Tuple<string, string, string>> a = new List<Tuple<string, string, string>>();

        internal void Add(TreeViewItem i1, TreeViewItem i2, TreeViewItem i3) {
            if (!a.Contains(new Tuple<string, string, string>((string)i1.Header, (string)i2.Header, (string)i3.Header)))
            {
                a.Add(new Tuple<string, string, string>((string)i1.Header, (string)i2.Header, (string)i3.Header));
            }

        }
        internal void Add(TreeViewItem i1, TreeViewItem i2)
        {
            if (!a.Contains(new Tuple<string, string, string>("",(string)i1.Header, (string)i2.Header)))
            {
                a.Add(new Tuple<string, string, string>("", (string)i1.Header, (string)i2.Header));
            }
        }

        internal void Remove(TreeViewItem i1)
        {
            a.RemoveAll(item => item.Item3 == (string)i1.Header);
        }

        internal List<Tuple<string, string, string>> Get(TreeViewItem selected)
        {
            List<Tuple<string, string, string>> t = new List<Tuple<string, string, string>>();
            t = a.GetRange(0, a.Count);

            t.RemoveAll(item => item.Item2 != (string)selected.Header);

            return t;
        }
    }
}
