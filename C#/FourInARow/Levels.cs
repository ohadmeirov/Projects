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
    public partial class Levels : Form
    {
        string playerName;
        public Levels(string playerName)
        {
            InitializeComponent();
            this.playerName = playerName;
        }

        private void Easy_Click(object sender, EventArgs e)
        {
            Game a = new Game(playerName, 0);
            a.Show();
            this.Close();
        }

        private void Noramal_Click(object sender, EventArgs e)
        {
            Game a = new Game(playerName, 1);
            a.Show();
            this.Close();
        }

        private void Hard_Click(object sender, EventArgs e)
        {
            Game a = new Game(playerName, 2);
            a.Show();
            this.Close();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Levels_Load(object sender, EventArgs e)
        {
            button1.Text = ("Choose level please, " + this.playerName);
        }
    }
}
