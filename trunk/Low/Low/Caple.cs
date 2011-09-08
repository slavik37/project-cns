using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Low
{
  public partial class Caple : Control, ICreature 
  {
    CNS MyCNS;
    CapleLeg leg;

    public Caple()
    {
      leg = new CapleLeg();
      MyCNS = new CNS(this);
      InitializeComponent();
    }

    public CNS GetMyCNS()
    {
      return MyCNS;
    }

    protected override void OnPaint(PaintEventArgs pe)
    {
      int lineWidth_ = 3;
      SolidBrush sb = new SolidBrush(Color.Black);
      if ((Width - lineWidth_ * 2 <= 0) || (Height - lineWidth_ * 2 <= 0))
      {
        pe.Graphics.FillRectangle(sb, 0, 0, Width, Height);
        base.OnPaint(pe);
        return;
      }

      SolidBrush rsb = new SolidBrush(Color.Red);
      ISection l = leg;

      double halfwidth = (Width - lineWidth_) / 10.0 / 2.0;
      double halfheight = (Height - lineWidth_) / 10.0 / 2.0;
      double cx = (l.GetSensor(1) / 100.0 + 1.0) * (double)((Width - halfwidth * 2.0 - lineWidth_ * 2.0) * 0.5);
      double cy = (l.GetSensor(3) / 100.0 + 1.0) * (double)((Height - halfheight * 2.0 - lineWidth_ * 2.0) * 0.5);


      pe.Graphics.FillRectangle(sb, 0, 0, Width, lineWidth_);
      pe.Graphics.FillRectangle(sb, Width - lineWidth_, 0, Width, Height);
      pe.Graphics.FillRectangle(sb, 0, Height - lineWidth_, Width, Height);
      pe.Graphics.FillRectangle(sb, 0, 0, lineWidth_, Height);

      int x = (int)Math.Round(cx + (double)halfwidth + (double)lineWidth_);
      int y = (int)Math.Round(cy + (double)halfheight + (double)lineWidth_);

      pe.Graphics.FillRectangle(rsb,
        (int)(cx  + lineWidth_), (int)(cy + lineWidth_),
        (int)(halfwidth * 2), (int)(halfheight * 2));
      base.OnPaint(pe);
    }

    //ICreature        
    #region ICreature Members
    public int SectionsCount()
    {
      return 1;
    }

    public ISection GetSection(int index)
    {
      if (index == 0) return leg;
      return null;
    }

    public void Advantage()
    {      
      leg.Advantage();      
    }
        
    public void React()
    {
      MyCNS.React();
      Refresh();
    }
    
    public void DoPrediction()
    {
      throw new NotImplementedException();
    }

    public void CheckPrediction()
    {
      throw new NotImplementedException();
    }
    #endregion


    
  }

  class CapleLeg : ISection
  {
    public CapleLeg()
    {
      rightSensor = new BoundValue("RS");
      leftSensor = new BoundValue("LS");
      upSensor = new BoundValue("US");
      downSensor = new BoundValue("DS");

      rightEff = new BoundValue("RE");
      leftEff = new BoundValue("LE");
      upEff = new BoundValue("UE");
      downEff = new BoundValue("DE");

      pl = new BoundValue("PL");
    }    
    int ISection.GetEffectorsCount()
    {
      return 4;
    }
    int ISection.GetSensorsCount()
    {
      return 5;
    }
    int ISection.GetGoalSensorsCount()
    {
      return 1;
    }
    
    double ISection.GetEffector(int index)
    {
      return GetEff(index).Value;
    }

    void ISection.SetEffector(int index, double value)
    {
      GetEff(index).Value = value;
    }

    double ISection.GetSensor(int index)
    {
      switch (index)
      {
        case right:
          {
            return rightSensor.Value;
          }
        case left:
          {
            return leftSensor.Value;
          }
        case up:
          {
            return upSensor.Value;
          }
        case down:
          {
            return downSensor.Value;
          }
        case touch:
          {
            if (downSensor.Value == BoundValue.MaxValue) return BoundValue.MaxValue;
            else return BoundValue.MinValue;
          }
        default:
          {
            throw new ArgumentOutOfRangeException();
          }
      }
    }

    double ISection.GetGoalSensor(int index)
    {
      if (index != 0) throw new ArgumentOutOfRangeException();
      return pl.Value;
    }

    BoundValue GetEff(int index)
    {
      switch (index)
      {
        case right:
          {
            return rightEff;
          }
        case left:
          {
            return leftEff;
          }
        case up:
          {
            return upEff;
          }
        case down:
          {
            return downEff;
          }
        default:
          {
            throw new ArgumentOutOfRangeException();
          }
      }
    }

    public void Advantage()
    {
      double dx = rightEff.Value - leftEff.Value;
      if (dx >= 0)
      {
        rightSensor.Value += dx * des;
        leftSensor.Value = -rightSensor.Value;
      }
      else
      {
        double curLeft = leftSensor.Value;

        leftSensor.Value += dx * des;
        rightSensor.Value = -leftSensor.Value;

        // PL Calculate
        if (downSensor.Value == BoundValue.MaxValue /*Touch*/)
          curLeft = leftSensor.Value - curLeft;
        pl.Value += curLeft * des;
      }
      double dy = upEff.Value - downEff.Value;
      if (dy >= 0)
      {
        upSensor.Value += dy * des;
        downSensor.Value = -upSensor.Value;
      }
      else
      {
        downSensor.Value += dy * des;
        upSensor.Value = -downSensor.Value;
      }      
    }

    const double des = 0.01;
    
    const int right = 0;
    const int left = 1;
    const int up = 2;
    const int down = 3;
    const int touch = 4;

    BoundValue rightSensor;
    BoundValue leftSensor;
    BoundValue upSensor;
    BoundValue downSensor;

    BoundValue rightEff;
    BoundValue leftEff;
    BoundValue upEff;
    BoundValue downEff;

    BoundValue pl;

    string ISection.GetSectionsName()
    {
      throw new NotImplementedException();
    }

    string ISection.GetEffectorsName(int index)
    {
      throw new NotImplementedException();
    }

    string ISection.GetSensorsName(int index)
    {
      throw new NotImplementedException();
    }

    string ISection.GetGoalSensorsName(int index)
    {
      throw new NotImplementedException();
    }
  }
}
