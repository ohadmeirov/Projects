using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FourInARow
{
    public partial class Main : Form
    {
        string playerName;
        public Main(string playerName)
        {
            InitializeComponent();
            this.playerName = playerName;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            label1.Text = ("Hello " + this.playerName + "!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Levels a = new Levels(this.playerName);
            a.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Instructions a = new Instructions();
            a.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("האם אתה בטוח שברצונך לצאת", "יציאה", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Records a = new Records(this.playerName);
            a.Show();
        }
    }
}
