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
    public partial class Form1 : Form
    {
        OleDbConnection connectiondb = new OleDbConnection(@"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = C:\214243016\Program Files\FourInARow\db1.mdb;");
        string playerName;
        
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {// מכניס שמות בודק האם תקני וכו
            if (textBox1.Text == "")
                MessageBox.Show("הכנס שם משתמש", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                this.playerName = textBox1.Text;
                if (IsExist() == false)
                {
                    Main a = new Main(this.playerName);
                    a.Show();
                }
                else
                {
                    DialogResult result = MessageBox.Show("?שם המשתמש קיים כבר במערכת - האם תרצה להמשיך", "כפילות בשם המשתמש", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        Main a = new Main(this.playerName);
                        a.Show();
                    }
                    else
                    {
                        textBox1.Text = "";
                    }
                }
            }
        }
        public bool IsExist()
        {// שואלת האם הנתב חוקי ואפשר לפתוח אותו
            if (connectiondb.State == ConnectionState.Closed)
                connectiondb.Open();
            
            string strSql = "Select * FROM Recordstable WHERE PlayerName = '" + this.playerName + "'  ";
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

        private void Form1_Load(object sender, EventArgs e)
        {// טוען את הדאטה בייס
            // TODO: This line of code loads data into the 'db1DataSet.Recordstable' table. You can move, or remove it, as needed.
            this.recordstableTableAdapter.Fill(this.db1DataSet.Recordstable);
            string g = ("Hello " + this.playerName);
        }
    }
}
