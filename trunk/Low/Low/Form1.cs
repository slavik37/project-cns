using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Low
{
  public partial class Form1 : Form
  {
    Environment env = new Environment();

    public Form1()
    {
      InitializeComponent();      
      
      //Создать и запустить среду
      env = new Environment();
      env.AddCreature(bug1);
      cnsView1.MyCNS = bug1.GetMyCNS();
      env.Start();
    }

    private void btStepFrw_Click(object sender, EventArgs e)
    {
      env.AdvantageMoment();
      bug1.Refresh();
    }

    private void trackBar1_Scroll(object sender, EventArgs e)
    {
      BoundValue bv = new BoundValue("", trackBar1.Value - 100);
      tbValue.Text = trackBar1.Value.ToString();
      tbOptimazedVal.Text = s0.NormalizeModal(bv).ToString();
    }
  }
}
