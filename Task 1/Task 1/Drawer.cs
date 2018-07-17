using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Collections;
using System.Data;
namespace Task_1
{
    class Drawer
    {
    
        private Dictionary<string, int> availableDrawers;//dictionary will store a list of available drawers as wells as their drawerId
        private Dictionary<int, List<string>> availableFields;//dictionary will store a list containing all the fields that correspond to a specific drawerId
   
        public Drawer() {
            //get the list of drawers and fields once the Drawer object is created
            availableDrawers = storeDrawers();
            availableFields = storeFields();
        }

        private Dictionary<string, int> storeDrawers()
        {
            Dictionary<string, int> dataRead = new Dictionary<string, int>();//Dictionary key = name of drawer, value = drawerId
            SqlConnection conn;
            using (conn = new SqlConnection())
            {
                conn.ConnectionString = "Data Source=CALYPSO\\SQLEXPRESS;User ID=click; Password=scan;Initial Catalog=clickscan";//Server, login credentials, and database to look at
                conn.Open();//open the connection
                SqlDataReader data;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT drawerid, dname From cs_drawers";
                data = cmd.ExecuteReader();
                while (data.Read())
                {//Iterate through results from query
                    IDataRecord record = (IDataRecord)data;
                    dataRead.Add(String.Format("{0}", record[1]), record.GetInt32(0)); 
                }
                data.Close();
                conn.Close();
            }
            return dataRead;
        }

        public Dictionary<int, List<string>> storeFields()
        {
            Dictionary<int, List<string>> storedFields = new Dictionary<int, List<string>>();//Dictionary key = drawerId, value = list of fields associated to drawerId
            SqlDataReader reader;

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "Data Source=CALYPSO\\SQLEXPRESS;User ID=click; Password=scan;Initial Catalog=clickscan";//Server, login credentials, and database to look at
                conn.Open();//open the connection

                SqlCommand cmd = new SqlCommand();//create a new SQL command

                cmd.Connection = conn;
                cmd.CommandText = "SELECT drawerid, flddesc FROM cs_fields"; ;
                reader = cmd.ExecuteReader();//execute command
                while (reader.Read())
                {//Iterate through results from query
                    IDataRecord record = (IDataRecord)reader;
                    List<string> fields;
                    int key = record.GetInt32(0);
                    if (!storedFields.TryGetValue(key, out fields))
                    {
                        fields = new List<string>();
                        storedFields.Add(key, fields);
                    }
                    fields.Add(String.Format("{0}", record[1]));
                }


                reader.Close();
                conn.Close();
                return storedFields;
            }

        }

        public Dictionary<string, int> getDrawers()
        {
            return availableDrawers;//return list of available drawers
        }

        public List<string> getFields(string selection)
        {
            int value = 0;
            int drawerId = 0;
            if (availableDrawers.TryGetValue(selection, out value))//get drawer id for selected item
            {
                drawerId = value;
            }
            List<string> v = null;
            List<string> fields = null;
            if(availableFields.TryGetValue(drawerId, out v))//get list of fields associated to selected drawer
            {
                fields = v;
            }
            return fields;
        }

    }

}