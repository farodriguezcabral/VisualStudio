using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Task_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        Drawer d = new Drawer();
        private void Form1_Load(object sender, EventArgs e)
        {
            Dictionary<string, int> drawers = d.getDrawers();//get list of available drawers
            foreach (KeyValuePair<string, int> kvp in drawers)//populate Drawers listbox
            {
                Drawers.Items.Add(kvp.Key);
            }   
        }

        private void Drawers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Drawers.SelectedItem != null)
            {
                string curItem = Drawers.SelectedItem.ToString();//store selected item by user
                List<string> fields = d.getFields(curItem);//get fields assocciated to selected drawer
                Fields.DataSource = fields;//populate fields listbox
                fields = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Fields_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        }
    }

