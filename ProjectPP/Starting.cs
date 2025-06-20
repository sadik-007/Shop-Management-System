using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPP
{
    public partial class Starting: Form
    {
        public Starting()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void salesManToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void customerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 customer = new Form1();
            customer.Show();
            this.Hide();
        }

        private void logInAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminLog admin = new AdminLog();
            admin.Show();
            this.Hide();
        }
    }
}
