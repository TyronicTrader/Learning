using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace DBStuff
{
    public partial class frmDBStuff : Form
    {
        ConnectToSQLite dbcon = new ConnectToSQLite();
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteDataAdapter sda;
        SQLiteCommandBuilder scb;
        DataSet ds;
        DataTable dt;

        public frmDBStuff()
        {
            InitializeComponent();
            fillRawDataGrid();
            highlightMonthCalendar();
            FillTreeView();
        }

        private void frmDBStuff_Load(object sender, EventArgs e)
        {
            //fillRawDataGrid();
            //highlightMonthCalendar();
            //FillTreeView();
        }

        private void highlightMonthCalendar()
        {
            string boldDatesQuery = "select Not_DATETIME from NOTES";
            sda = new SQLiteDataAdapter(boldDatesQuery, dbcon.con);
            sda.Fill(ds, "boldDates");
            try
            {
                foreach (DataRow dr in ds.Tables["boldDates"].Rows)
                {
                    monthCalendar1.AddBoldedDate(DateTime.Parse(dr["Not_DATETIME"].ToString()));
                    monthCalendar1.UpdateBoldedDates();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ds.Tables.Remove("boldDates");
        }

        #region ABC Treeview example

        
        private class ItemInfo
        {
            public int ID;
            public int ParentID;
            public string Name;
        }

        private void FillTreeView()
        {
            var items = new List<ItemInfo>()
            {
                new ItemInfo(){ID = 1, ParentID = 4, Name = "A"},
                new ItemInfo(){ID = 2, ParentID = 1, Name = "A1"},
                new ItemInfo(){ID = 3, ParentID = 0, Name = "B"},
                new ItemInfo(){ID = 4, ParentID = 0, Name = "C"},
                new ItemInfo(){ID = 5, ParentID = 1, Name = "A2"},
                new ItemInfo(){ID = 6, ParentID = 3, Name = "B1"},
            };

            FillNode(items, null);
        }

        private void FillNode(List<ItemInfo> items, TreeNode node)
        {
            var parentID = node != null ? (int)node.Tag : 0;

            var nodesCollection = node != null ? node.Nodes : treeView1.Nodes;

            foreach (var item in items.Where(i => i.ParentID == parentID))
            {
                var newNode = nodesCollection.Add(item.Name, item.Name);
                newNode.Tag = item.ID;

                FillNode(items, newNode);
            }
        }
        

        #endregion




        public void fillRawDataGrid()
        {
            try
            {
                ds = new DataSet();
                ds.Tables.Clear();
                dt = new DataTable();
                dt.Clear();

                #region notes
                ///Section 1
                ///this section we create datasets using separate dataAdapters
                ///when you fill the dataset, it can be filled in serial (similar to datatable) or as an array (each adapter with separate reference)
                ///try taking the sda2.Fill(ds, "NOTETYPES"); and then change to sda2.Fill(ds, [2]);
                //string Query1 = "select * from NOTES";
                //SQLiteDataAdapter sda1 = new SQLiteDataAdapter(Query1, dbcon.con);
                //sda1.Fill(ds, "NOTES");
                //dataGridView1.DataSource = ds.Tables["NOTES"];

                //string Query2 = "select * from NOTETYPES";
                //SQLiteDataAdapter sda2 = new SQLiteDataAdapter(Query2, dbcon.con);
                //sda2.Fill(ds, "NOTETYPES");
                //dataGridView2.DataSource = ds.Tables["NOTETYPES"];

                //string Query3 = "select * from NOTEMEDIA";
                //SQLiteDataAdapter sda3 = new SQLiteDataAdapter(Query3, dbcon.con);
                //sda3.Fill(ds, "NOTEMEDIA");
                //dataGridView3.DataSource = ds.Tables["NOTEMEDIA"];
                //ds.Clear();

                ///Section 1.5
                /// DataTable below is populated by Query4 which is a combination of info from within the same tables used above
                //dt = new DataTable();
                //string Query4 = "select * from NOTEMEDIA, NOTES, USER, NOTETYPES where NOTEMEDIA.Nmd_Not_ID = 1";
                //SQLiteDataAdapter sda4 = new SQLiteDataAdapter(Query4, dbcon.con);
                //sda4.Fill(dt);
                //dataGridView4.DataSource = dt;

                ///Section 2
                ///Here you will notice we are re-using the same initial adapter for this form class which is
                /// acceptable as long as you understand the adapter connection/query will be replaced with the new data
                #endregion

                string Query1 = "select * from NOTES";
                sda = new SQLiteDataAdapter(Query1, dbcon.con);
                sda.Fill(ds, "NOTES");  //sda puts the results of the query into the dataset array[0] as reference name "NOTES"
                sda.Fill(dt);           //datatable does not do a referencable array of data like a dataset
                sda.Fill(ds, "DataSetAsDataTable");     
                dataGridView1.DataSource = ds.Tables["NOTES"];

                string Query2 = "select * from NOTETYPES";
                sda = new SQLiteDataAdapter(Query2, dbcon.con);
                sda.Fill(ds, "NOTETYPES");
                sda.Fill(dt);
                sda.Fill(ds, "DataSetAsDataTable");
                //ds.Tables.Remove("NOTETYPES");
                dataGridView2.DataSource = ds.Tables["NOTETYPES"];

                string Query3 = "select * from NOTEMEDIA";
                sda = new SQLiteDataAdapter(Query3, dbcon.con);
                sda.Fill(ds, "NOTEMEDIA");
                sda.Fill(dt);
                sda.Fill(ds, "DataSetAsDataTable");
                dataGridView3.DataSource = ds.Tables["NOTEMEDIA"];

                dataGridView4.DataSource = dt;
                            //when reusing the same dataset reference name to put in the information
                            //it will build the dataset like a datatable in a way
                            //Datasets will have referencable tables (datatable info)
                dataGridView5.DataSource = ds.Tables["DataSetAsDataTable"];
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
