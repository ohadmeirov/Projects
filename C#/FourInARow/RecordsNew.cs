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
    public partial class RecordsNew : Form
    {
        OleDbConnection connectiondb = new OleDbConnection(@"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = Computer\D:\214243016\Program Files\FourInARow\bin\Debug\db1.mdb; security Info=False");
        OleDbCommand com;
        string playerName;
        string str;
        public RecordsNew()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {// קולט את שם המשתמש 
            if (textBox1.Text == "")            
                MessageBox.Show("הכנס שם משתמש", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                this.playerName = textBox1.Text;
                if(IsExist() == false)
                {
                    Game a = new Game(this.playerName);
                    a.Show();
                }
                else
                {
                    DialogResult result = MessageBox.Show("?שם המשתמש קיים כבר במערכת - האם תרצה להמשיך", "כפילות בשם המשתמש", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        Game a = new Game(this.playerName);
                        a.Show();
                    }
                }
            }

        }

        private void RecordsNew_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public bool IsExist()
        {// שואל האם הנתק תקני ואפשר לפתוח אותו
            if (connectiondb.State == ConnectionState.Closed)
                connectiondb.Open();
            string strSql = "Select * FROM Scores WHERE username = '" + this.Name + "'  ";
            OleDbCommand cmd = new OleDbCommand(strSql, connectiondb);
            cmd.CommandType = CommandType.Text;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd); 
            DataTable dt = new DataTable();
            da.Fill(dt);
            int count = Convert.ToInt32(dt.Rows.Count.ToString());
            connectiondb.Close();

            if (count == 0)
                return false;

            return true;
        }








        public void SetData()
        {// הולך לדאטא בייס ומאתחל את שם השחקן + התוצאה שלו
            int p = 0;
            if (connectiondb.State == ConnectionState.Closed)
                connectiondb.Open();
            try
            {
                str = "INSERT INTO Scores (UserName, Points) values (this.name, this.points)";
                com = new OleDbCommand(str, connectiondb);
                com.Parameters.AddWithValue("@UserName", this.playerName);
                com.Parameters.AddWithValue("@Points", this.playerName);
                com.ExecuteNonQuery();
                connectiondb.Close();
                MessageBox.Show("Records Successfuly Inserted");
                showdata();
            }
            catch
            {
                str = "SELECT* FROM Scores WHERE UserName = '" + this.playerName + "'";
                com = new OleDbCommand(str, connectiondb);
                OleDbDataReader reader = com.ExecuteReader();

                while(reader.Read())
                {
                    string s = reader[0].ToString();
                    p = Convert.ToInt32(reader[1].ToString());
                }
                reader.Close();
                if (this.points > p)
                {
                    str = "UPDATE Scores SET Points = " + this.points + " WHERE UserName = '" + this.playerName + "'";
                    com = new OleDbCommand(str, connectiondb);
                    com.ExecuteNonQuery();
                    connectiondb.Close();
                    MessageBox.Show("מספר הנקודות עודכן בהצלחה");
                    showdata();
                }
                else
                    MessageBox.Show("התוצאה האחרונה נמוכה מהתוצאה הקיימת ולכן מספר הנקודות לא עודכן");
            }
        }

        private void showdata()
        {// מציג את הדאטא בייס
            if (connectiondb.State == ConnectionState.Closed)
                connectiondb.Open();
            string strSql = "SELECT * FROM Scores ORDER BY Points DESC";
            OleDbCommand cmd = new OleDbCommand(strSql, connectiondb);
            cmd.CommandType = CommandType.Text;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            connectiondb.Close();
            DataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.Refresh();
        }
    }
}
