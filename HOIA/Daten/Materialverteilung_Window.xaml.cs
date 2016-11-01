using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace HOIA.Daten
{
    /// <summary>
    /// Interaktionslogik für Materialverteilung_Window.xaml
    /// </summary>
    public partial class Materialverteilung_Window : Page
    {
        List<int> idList = new List<int>();

        public Materialverteilung_Window()
        {
            InitializeComponent();

            comboBox_Aufträge_Hinzufügen_Von(comboBox_Auftrag, comboBox_Verfahren_Von);

            comboBox_Load_Verfahren_Von(comboBox_Verfahren_Von);
            comboBox_Load_Gespeicherte_Verfahren(comboBox_Gespeicherte_Verfahren);

        }

        private void comboBox_Verfahren_Von_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                comboBox_Auftrag.SelectedIndex = 0;
                comboBox_Aufträge_Hinzufügen_Von(comboBox_Auftrag, comboBox_Verfahren_Von);

                Refresh_Datagrid_von();
            }
            catch (Exception)
            {

            }


        }

        private void comboBox_Auftrag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Refresh_Datagrid_von();
            }
            catch (Exception)
            {
            }

        }

        private void comboBox_Gespeicherte_Verfahren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string s = Erweiterungen.Helper.GetComboBoxTextComplete(comboBox_Gespeicherte_Verfahren);
            if (!s.Contains("Gespeicherte Verfahren"))
            {
                //Leert Liste
                idList.Clear();
                //string[] srs = s.Split(' ');
                //srs[0] Verfahren-Name
                //srs[1] Datum

                DDataContext d = new DDataContext();

                var nmi = from x in d.View_Prozess_Anwendung_Material
                          where (x.Name == s)
                          select x;

                foreach (var item in nmi)
                {
                    idList.Add(item.Id_Material_OTab);

                }

                button_Freigeben.IsEnabled = true;
                button_Löschen.IsEnabled = true;



            }
            else
            {
                button_Freigeben.IsEnabled = false;
                button_Löschen.IsEnabled = false;
                idList.Clear();
            }
            Refresh_DataGrid_zu();
        }
        
        private void Refresh_Datagrid_von()
        {
            //Wenn kein Auftrag gewählt wurde, aber ein Verfahren
            if ((comboBox_Verfahren_Von.SelectedIndex != -1 || comboBox_Verfahren_Von.SelectedIndex != 0) && (comboBox_Auftrag.SelectedIndex == 0 || comboBox_Auftrag.SelectedIndex == -1))
            {
                //Leere Materiallsite
                dataGrid_Material_Von.Items.Clear();
                //Fülle Materialliste
                dataGrid_Hinzufügen_Material(string.Empty);


            }
            //Es wurde ein Verfahren und ein Auftrag gewählt
            else if ((comboBox_Verfahren_Von.SelectedIndex != -1 || comboBox_Verfahren_Von.SelectedIndex != 0) && (comboBox_Auftrag.SelectedIndex != 0 && comboBox_Auftrag.SelectedIndex != -1))
            {
                //Leere Materiallsite
                dataGrid_Material_Von.Items.Clear();
                //Fülle Materialliste
                dataGrid_Hinzufügen_Material((string)comboBox_Auftrag.SelectedItem);

            }
            else
            {
                //Leere Materiallsite
                dataGrid_Material_Von.Items.Clear();
            }

        }

        private void Refresh_DataGrid_zu()
        {
            DDataContext d = new DDataContext();

            try
            {
                if (dataGrid_Material_Zu.Items.Count > 0)
                {
                    dataGrid_Material_Zu.Items.Clear();
                }

                foreach (int i in idList)
                {
                    Material fel = (from y in d.Material
                                    where y.Id == i
                                    select y).First();
                    dataGrid_Material_Zu.Items.Add(fel);
                }
            }
            catch (Exception)
            {
            }

            label_Set_Weight_By_DataGrid(idList, label_Material_zu_Statistik);
        }




        private void button_Sende_Einen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DDataContext d = new DDataContext();

                int i = Erweiterungen.Helper.GetIntFromDataGrid(0, dataGrid_Material_Von);

                string s = (from x in d.Material
                            where x.Id == i
                            select x).First().Status;

                if (s == Erweiterungen.Helper.STRING_FREI_STATUS)
                {
                    //Fügt MaterialId zu Liste hinzu
                    if (!idList.Contains(i))
                    {
                        idList.Add(i);
                    }
                    else
                    {
                        MessageBox.Show("Dieses Material wurde bereits in ein Verfahren übertragen!");
                    }

                    //Synchronisiert Daten
                    Refresh_Datagrid_von();
                    Refresh_DataGrid_zu();
                }



            }
            catch (Exception)
            {
            }

        }

        private void button_Sende_Alle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DDataContext d = new DDataContext();

                for (int i = 0; i < dataGrid_Material_Von.Items.Count; i++)
                {
                    int j = (Erweiterungen.Helper.GetIntFromDataGrid(0, i, dataGrid_Material_Von));

                    string s = (from x in d.Material
                                where x.Id == j
                                select x).First().Status;

                    if (s == Erweiterungen.Helper.STRING_FREI_STATUS)
                    {
                        if (!idList.Contains(j))
                        {
                            idList.Add(j);
                        }
                    }
                }
                Refresh_Datagrid_von();
                Refresh_DataGrid_zu();
            }
            catch (Exception)
            {
            }
        }

        private void button_Zurück_Einen_Click(object sender, RoutedEventArgs e)
        {
            DDataContext d = new DDataContext();
            int i = Convert.ToInt32( Erweiterungen.Helper.GetStringFromDataGrid(0,dataGrid_Material_Zu.SelectedIndex,dataGrid_Material_Zu));

            Material m = (from x in d.Material
                          where x.Id == i
                          select x).First();
            m.Status = Erweiterungen.Helper.STRING_FREI_STATUS;

            try
            {
                d.SubmitChanges();
                idList.Remove(i);
            }
            catch (Exception)
            {
                MessageBox.Show("Fehler! Material konnte nicht entfernt werden!");
            }

            Refresh_Datagrid_von();
            Refresh_DataGrid_zu();

        }

        private void button_Zurück_Alle_Click(object sender, RoutedEventArgs e)
        {
            DDataContext d = new DDataContext();

            var m = from x in d.Material
                    where idList.Contains(x.Id)
                    select x;
            foreach (Material item in m)
            {
                item.Status = Erweiterungen.Helper.STRING_FREI_STATUS;
            }

            try
            {
                d.SubmitChanges();
                idList.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("Fehler! Material konnte nicht entfernt werden!");
            }

            Refresh_Datagrid_von();
            Refresh_DataGrid_zu();
        }




        private void comboBox_Load_Verfahren_Von(ComboBox comboBox_Verfahren_Von)
        {
            if (comboBox_Verfahren_Von.Items.Count < 2)
            {
                DDataContext d = new DDataContext();

                int int_HGO = (from y in d.Maschinenart
                               where y.Name.Contains("Haubenofen")
                               select y).First().Id;

                var mas = from y in d.Maschine
                          where y.Id_Maschinenart == int_HGO
                          select y;

                foreach (var ytem in mas)
                {
                    var zov = from x in d.Verfahren
                              where x.Id_Maschine == ytem.Id
                              select x;

                    foreach (var item in zov)
                    {
                        comboBox_Verfahren_Von.Items.Add(item.Name);
                    }

                }

            }

        }

        private void comboBox_Aufträge_Hinzufügen_Von(ComboBox comboBox_Auftrag_Von, ComboBox comboBox_Verfahren_Von)
        {
            DDataContext d = new DDataContext();

            var amv = from x in d.View_Auftrag_AZU_Verfahren
                      where x.Status_AZU == Erweiterungen.Helper.STRING_ZUGEORDNET_STATUS && x.Name == Erweiterungen.Helper.GetComboBoxTextMK2(comboBox_Verfahren_Von)
                      select x;

            comboBox_Auftrag_Von.Items.Clear();
            comboBox_Auftrag_Von.Items.Add("Auftrag");
            foreach (var item in amv)
            {
                comboBox_Auftrag_Von.Items.Add(item.ODL);
            }
            comboBox_Auftrag_Von.SelectedIndex = 0;
        }

        private void comboBox_Load_Gespeicherte_Verfahren(ComboBox c)
        {
            DDataContext d = new DDataContext();

            var zov = from x in d.Verfahren_Prozess_Anwendung
                      where x.Status == Erweiterungen.Helper.STRING_GESPEICHERT_STATUS
                      select x;

            c.Items.Clear();
            c.Items.Add(new ComboBoxItem() { Content = "Gespeicherte Verfahren" });
            try
            {
                c.SelectedIndex = 0;
            }
            catch (Exception)
            {
            }


            foreach (var item in zov)
            {
                c.Items.Add(item.Name);
            }

        }


        /// <summary>
        /// Berechnet das Gewicht der in der Liste verbleibenden Materialen
        /// </summary>
        /// 
        private void label_Set_Weight_By_DataGrid(DataGrid dg, Label lb) {
            try
            {
                DDataContext d = new DDataContext();

                int w = 0;
                for (int i = 0; i < dg.Items.Count; i++)
                {
                    int z = Erweiterungen.Helper.GetIntFromDataGrid(0, i, dg);
                    //Bildet Summe des Gewichts
                    w = w + Convert.ToInt32((from x in d.Material
                                             where x.Id == z
                                             select x.Gewicht).Sum());
                }


                lb.Content = "Total: " + w + " Kg";

        }
            catch (Exception)
            {
                lb.Content = "Total: 0 Kg";
            }
}
        private void label_Set_Weight_By_DataGrid(List<int> dg, Label lb)
        {
            try
            {
                DDataContext d = new DDataContext();

                int w = 0;
                for (int i = 0; i < dg.Count; i++)
                {
                   // int z = Erweiterungen.Helper.GetIntFromDataGrid(0, i, dg);
                    //Bildet Summe des Gewichts
                    w = w + Convert.ToInt32((from x in d.Material
                                             where x.Id == dg[i]
                                             select x.Gewicht).Sum());
                }


                lb.Content = "Total: " + w + " Kg";

            }
            catch (Exception)
            {
                lb.Content = "Total: 0 Kg";
            }
        }


        private void dataGrid_Hinzufügen_Material(string odl)
        {

            try
            {
                if (odl == string.Empty)
                {
                    DDataContext d = new DDataContext();
                    var aft = from y in d.View_Auftrag_AZU_Verfahren
                              where

                              y.Status_AZU == Erweiterungen.Helper.STRING_ZUGEORDNET_STATUS &&
                              y.Name == Erweiterungen.Helper.GetComboBoxTextMK2(comboBox_Verfahren_Von)

                              select y;

                    foreach (var item in aft)
                    {
                        dataGrid_Hinzufügen_Material(item.ODL);
                    }
                }
                else
                {
                    DDataContext d = new DDataContext();

                    int? aId = (from y in d.View_Auftrag_AZU_Verfahren
                                where

                                y.ODL == odl &&
                                y.Status_AZU == Erweiterungen.Helper.STRING_ZUGEORDNET_STATUS &&
                                y.Name == Erweiterungen.Helper.GetComboBoxTextMK2(comboBox_Verfahren_Von)

                                select y).First().Id;

                    var ama = from x in d.Material
                              where //x.Status == Erweiterungen.Helper.STRING_FREI_STATUS &&
                              x.Id_Auftrag == aId
                              select x;

                    foreach (var item in ama)
                    {
                        int t = dataGrid_Material_Von.Items.Add(item);

                        if (item.Status == Erweiterungen.Helper.STRING_FREI_STATUS)
                        {
                            Erweiterungen.Helper.DataGridMakeRowColor(dataGrid_Material_Von, t, Brushes.White);
                        }
                        //Wenn das Material in der aktuellen Glühung verteilt wird
                        if (idList.Contains(item.Id))
                        {
                            Erweiterungen.Helper.DataGridMakeRowColor(dataGrid_Material_Von, t, Brushes.Orange);
                        }
                        //Wenn das Material bereits an einer anderen Stelle verteilt wurde
                        switch (item.Status)
                        {
                            case Erweiterungen.Helper.STRING_ZUGEORDNET_STATUS:
                                Erweiterungen.Helper.DataGridMakeRowColor(dataGrid_Material_Von, t, Brushes.Red);
                                break;
                            //case Erweiterungen.Helper.STRING_FREI_STATUS:
                            //    Erweiterungen.Helper.DataGridMakeRowColor(dataGrid_Material_Von, t, Brushes.White);
                            //    break;
                            case Erweiterungen.Helper.STRING_FERTIG_STATUS:
                                Erweiterungen.Helper.DataGridMakeRowColor(dataGrid_Material_Von, t, Brushes.Green);
                                break;
                            case null:
                                Erweiterungen.Helper.DataGridMakeRowColor(dataGrid_Material_Von, t, Brushes.White);
                                break;
                            default:
                                break;
                        }


                    }
                    
                    
                }
                label_Set_Weight_By_DataGrid(dataGrid_Material_Von, label_Material_von_Statistik);

            }
            catch (Exception)
            {

            }

        }



        private void button_Freigeben_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Material wurde erfolgreich freigeben!");
        }
        private void button_Neu_Click(object sender, RoutedEventArgs e)
        {
            dataGrid_Material_Zu.IsEnabled = true;

            button_Sende_Einen.IsEnabled = true;
            button_Sende_Alle.IsEnabled = true;
            button_Zurück_Einen.IsEnabled = true;
            button_Zurück_Alle.IsEnabled = true;

            button_Speichern.IsEnabled = true;
            button_Freigeben.IsEnabled = false;
            //button_Löschen.IsEnabled = true;

            comboBox_Gespeicherte_Verfahren.SelectedIndex = 0;
            idList.Clear();
            dataGrid_Material_Zu.Items.Clear();


        }
        private void button_Löschen_Click(object sender, RoutedEventArgs e)
        {
            DDataContext d = new DDataContext();

            //Finde Prozess - und lösche den
            var fps = from x in d.Verfahren_Prozess_Anwendung
                      where x.Name == Erweiterungen.Helper.GetComboBoxTextComplete(comboBox_Gespeicherte_Verfahren)
                      select x;
            d.Verfahren_Prozess_Anwendung.DeleteAllOnSubmit(fps);

            var bla = from y in d.View_Prozess_Anwendung_Material
                      where y.Id == fps.First().Id
                      select y;
            //Setzte Materialstatus zurück
            foreach (var item in bla)
            {
                Material m = (from z in d.Material
                              where z.Id == item.Id_Material_OTab
                              select z).First();
                m.Status = Erweiterungen.Helper.STRING_FREI_STATUS;
            }
            try
            {
                d.SubmitChanges();
            }
            catch (Exception)
            {

            }
            comboBox_Load_Gespeicherte_Verfahren(comboBox_Gespeicherte_Verfahren);
        }
        private void button_Speichern_Click(object sender, RoutedEventArgs e)
        {
            if (idList.Count > 0)
            {
                if (comboBox_Verfahren_Von.SelectedIndex != 0 && comboBox_Verfahren_Von.SelectedIndex != -1)
                {
                    DDataContext d = new DDataContext();

                    Verfahren vef = (from x in d.Verfahren
                                     where x.Name == Erweiterungen.Helper.GetComboBoxTextMK2(comboBox_Verfahren_Von)
                                     select x).First();

                    string ds = DateTime.Now.ToLongDateString() + "," + DateTime.Now.ToLongTimeString();
                    Verfahren_Prozess_Anwendung vmm = new Verfahren_Prozess_Anwendung()
                    {
                        Name = vef.Name + " " + ds,
                        Id_Verfahren = vef.Id,
                        Status = Erweiterungen.Helper.STRING_GESPEICHERT_STATUS
                    };

                    d.Verfahren_Prozess_Anwendung.InsertOnSubmit(vmm);

                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {
                    }
                    Verfahren_Prozess_Anwendung nvm = (from k in d.Verfahren_Prozess_Anwendung
                                                       where k.Name == (vef.Name + " " + ds)
                                                       select k).First();
                    foreach (var item in idList)
                    {
                        //Suche passendes Material
                        Material material = (from x in d.Material
                                             where x.Id == item
                                             select x).First();

                        MN_Verfahren_Prozess_Material vpm = new MN_Verfahren_Prozess_Material()
                        {
                            Id_Verfahren_Prozess = nvm.Id,
                            Id_Material = material.Id
                        };

                        d.MN_Verfahren_Prozess_Material.InsertOnSubmit(vpm);

                        material.Status = Erweiterungen.Helper.STRING_ZUGEORDNET_STATUS;

                    }
                    button_Freigeben.IsEnabled = true;
                    try
                    {
                        d.SubmitChanges();
                        MessageBox.Show("Erfolgreich gespeichert!");

                        comboBox_Load_Gespeicherte_Verfahren(comboBox_Gespeicherte_Verfahren);

                        Erweiterungen.Helper.ComboBoxSelectValue(comboBox_Gespeicherte_Verfahren, vef.Name + " " + ds);

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Speichern nicht erfolgreich! Bitte wenden sie sich an einen Administrator!");
                    }

                }
            }
            else
            {
                MessageBox.Show("Die Liste ist leer! Bitte fügen sie vor dem speichern Material hinzu!");
            }


        }

    }
}
