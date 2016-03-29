namespace PtpChat.Main.UserInterface.Subforms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Forms;

    using BrightIdeasSoftware;

    using PtpChat.Base.Classes;

    public partial class NodesForm : Form
    {
        public PTPClient ptpClient;

        public NodesForm(PTPClient ptpclient)
        {
            //object list
            this.InitializeComponent();
            this.SetupColumns();

            this.RefreshNodes(ptpclient);


            //data grid view
            var nodes = ptpclient?.GetNodes().ToList();

            //dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSize = true;

           // //Initialize and add a text box column.
           //var column = new DataGridViewTextBoxColumn { DataPropertyName = "IPAddress", Name = "IP" };
           // dataGridView1.Columns.Add(column);

           // column = new DataGridViewTextBoxColumn { DataPropertyName = "Port", Name = "Port" };
           // dataGridView1.Columns.Add(column);

           // column = new DataGridViewTextBoxColumn { DataPropertyName = "AddedOn", Name = "Added" };
           // dataGridView1.Columns.Add(column);

           // column = new DataGridViewTextBoxColumn { DataPropertyName = "LastSeen", Name = "Last Seen", };
           // dataGridView1.Columns.Add(column);

            //create a list of classes containing the data we want in the view
        }

        public void RefreshNodes(PTPClient ptpclient)
        {
            if (ptpclient == null) return;

            this.ptpClient = ptpclient;
            var nodes = ptpclient.GetNodes().ToList();

            this.objList_Nodes.SetObjects(nodes);


            var bindingList = new BindingList<Node>(nodes);
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;
            dataGridView1.Refresh();
        }

        private void SetupColumns()
        {
            //this.objList_Nodes.CustomSorter = delegate (OLVColumn column, SortOrder order)
            //{
            //    this.objList_Nodes.ListViewItemSorter = new ColumnComparer(this.olvCol_IsStartUpNode, SortOrder.Descending, column, order);
            //};

            // i dont know why the order of these string matter, but it does, so dont mess with them
            this.olvCol_IsStartUpNode.MakeGroupies(new[] { true, false }, new[] { "Clients", "Unknown", "Servers" });

            this.olvCol_Added.GroupKeyGetter = delegate (object rowObject)
            {
                Node node = (Node)rowObject;
                return node.LastRecieve.Value != DateTime.MinValue ? new DateTime(node.LastRecieve.Value.Year, node.LastRecieve.Value.Month, 1) : new DateTime(6, 6, 6);
            };
            this.olvCol_Added.GroupKeyToTitleConverter = groupKey => ((DateTime)groupKey).ToString("MMMM yyyy");


            this.olvCol_LastRecieve.GroupKeyGetter = delegate (object rowObject)
            {
                Node node = (Node)rowObject;
                return node.LastRecieve != DateTime.MinValue ? new DateTime(node.LastRecieve.Value.Year, node.LastRecieve.Value.Month, 1) : new DateTime(6, 6, 6);
            };
            this.olvCol_LastRecieve.GroupKeyToTitleConverter = groupKey => ((DateTime)groupKey).ToString("MMMM yyyy");
        }
    }
}