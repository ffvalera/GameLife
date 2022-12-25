using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace GameLife
{
    public partial class Form2 : Form
    {
        bool[] Born;
        bool[] Live;
        Field field;
        public Form2(ref Field field_)
        {
            InitializeComponent();
            field = field_;
            Born = field.burn;
            Live = field.live;

            for (int i = 0; i < Born.Length; i++)
            {
                if (Born[i])
                    BornBox.SetItemChecked(i, true);
            }
            for (int i = 0; i < Live.Length; i++)
            {
                if (Live[i])
                    LiveBox.SetItemChecked(i, true);
            }
        }

        private void Ok_Click(object sender, EventArgs e)
       {
            Born = Born.Select(x => false).ToArray();
            Live = Live.Select(x => false).ToArray();
            foreach (int i in BornBox.CheckedIndices)
            {
                if(BornBox.GetItemCheckState(i) == CheckState.Checked)
                    Born[i] = true;
            }            
            foreach (int i in LiveBox.CheckedIndices)
            {
                if (LiveBox.GetItemCheckState(i) == CheckState.Checked)
                    Live[i] = true;
            }
            field.burn = Born;
            field.live = Live;
            this.Close();
        }
    }
}
