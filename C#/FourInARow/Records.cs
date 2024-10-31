using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace FourInARow
{
    public partial class Records : Form
    {
        OleDbConnection connectiondb = new OleDbConnection(@"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = C:\214243016\Program Files\FourInARow\db1.mdb;");
        string playerName;
        OleDbCommand cmd;
        string str;
        int win = 0;
        int lose = 0;
        public Records(string playerName, int win, int lose)
        {
            InitializeComponent();
            this.playerName = playerName;
            this.win = win;
            this.lose = lose;
            SetData();
            showdata();
        }
        public Records(string playerName)
        {            
            InitializeComponent();
            this.playerName = playerName;
            showdata();
        }

        private void Records_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'db1DataSet.Recordstable' table. You can move, or remove it, as needed.
            this.recordstableTableAdapter.Fill(this.db1DataSet.Recordstable);
            string g = ("Hello " + this.playerName);

           
        }
        public void SetData()
        {
            int p = 0;
            if (connectiondb.State == ConnectionState.Closed)
                connectiondb.Open();
            try
            {                
                str = "INSERT INTO Recordstable (PlayerName, Wins, Loses) values (this.playerName, this.win, this.lose)";
                cmd = new OleDbCommand(str, connectiondb);
                cmd.Parameters.AddWithValue("@PlayerName", this.playerName);
                cmd.Parameters.AddWithValue("@Wins", this.win);
                cmd.Parameters.AddWithValue("@Loses", this.lose);
                cmd.ExecuteNonQuery();
                connectiondb.Close();
                MessageBox.Show("Records Successfuly Inserted");
                showdata();
            }
            catch
            {
                str = "SELECT * FROM Recordstable WHERE PlayerName = '" + this.playerName + "'";
                cmd = new OleDbCommand(str, connectiondb);
                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string s = reader[0].ToString();
                    p = Convert.ToInt32(reader[1].ToString());
                }
                reader.Close();

                if (this.win > p)
                {
                    str = "UPDATE Recordstable SET Wins = '" + this.win + "' WHERE PlayerName = '" + this.playerName + "'";
                    cmd = new OleDbCommand(str, connectiondb);
                    cmd.ExecuteNonQuery();
                    connectiondb.Close();
                    MessageBox.Show("מספר הנצחונות עודכן");
                    showdata();
                }
                else if (this.win == p)
                {
                    str = "UPDATE Recordstable SET Loses = '" + this.lose + "' WHERE PlayerName = '" + this.playerName + "'";
                    cmd = new OleDbCommand(str, connectiondb);
                    cmd.ExecuteNonQuery();
                    connectiondb.Close();
                    showdata();
                }
                else 
                {
                    str = "UPDATE Recordstable SET Loses = '" + this.lose + "' WHERE PlayerName = '" + this.playerName + "'";
                    cmd = new OleDbCommand(str, connectiondb);
                    cmd.ExecuteNonQuery();
                    connectiondb.Close();
                    MessageBox.Show("מספר ההפסדים עודכן");
                    showdata();
                }
            }
        }
        public void showdata()
        {
            if (connectiondb.State == ConnectionState.Closed)
                connectiondb.Open();
            string strSql = "SELECT * FROM Recordstable ORDER BY Wins DESC";
            OleDbCommand cmd = new OleDbCommand(strSql, connectiondb);
            cmd.CommandType = CommandType.Text;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            connectiondb.Close();
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("האם אתה בטוח שברצונך לצאת", "יציאה", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();           
        }
    }  
}
