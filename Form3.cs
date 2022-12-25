using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameLife
{
    public partial class Form3 : Form
    {
        int count;
        bool infcountofmoves=true;
        Form1 Parent;
        public Form3()
        {
            InitializeComponent();

            checkBox1.Checked = infcountofmoves;
            if(count>0)
                textBox1.Text = count.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Parent = this.Owner as Form1;
            Parent.infCountOfMoves = checkBox1.Checked;
            int tcount;
            if (int.TryParse(textBox1.Text, out tcount) && tcount >0)
                count = tcount;

            Parent.countOfMoves = count;
            this.Close();
        }
    }
}
