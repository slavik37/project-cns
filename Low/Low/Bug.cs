using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Low
{
  public partial class Bug : Control, ISkotina_10 
  {
    s0 s0lu;
    s0 s1ru;
    s1 s2rd;
    s2 s3ld;
    sS0 s4S0;
    sS1 s5S1;

    CNS myCNS;

    public Bug()
    {
      InitializeComponent();
      s0lu = new s0("lu");
      s1ru = new s0("ru");
      s2rd = new s1("rd");
      s3ld = new s2("ld");
      s2rd.SetTwin(s3ld.mySection);
      s3ld.SetTwin(s2rd.mySection);
      s4S0 = new sS0("ad");
      s5S1 = new sS1("G");
      myCNS = new CNS(this);
    }

    public CNS GetMyCNS()
    {
      return myCNS;
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
      ISection l = s0lu;

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
        (int)(cx + lineWidth_), (int)(cy + lineWidth_),
        (int)(halfwidth * 2), (int)(halfheight * 2));      
      base.OnPaint(pe);
    }

    #region ICreature Members

    public int SectionsCount() { return 6; }

    public ISection GetSection(int index)
    {
      switch (index)
      {
        case 0: return s0lu;
        case 1: return s1ru;
        case 2: return s2rd;
        case 3: return s3ld;
        case 4: return s4S0;
        case 5: return s5S1;
        default: throw new ArgumentOutOfRangeException();
      }
    }

    public void DoPrediction()
    {
      myCNS.DoPrediction();
    }

    public void Advantage()
    {      
      s0lu.Advantage();
      s1ru.Advantage();
      s2rd.Advantage();
      s3ld.Advantage();
      myCNS.Advantage();
    }

    public void CheckPrediction()
    {
      myCNS.CheckPrediction();
    }

    public void React()
    {
      myCNS.React();
    }
    #endregion  

    #region ISkotina_10 Members

    public void SetFeedVector(double angle, double distance)
    {
      Target trg = new Target();
      trg.angle = angle;
      trg.distance = distance;
      s4S0.SetTarget(trg);
    }

    public void SetTarget(double value)
    {
      s5S1.SetTarget(value);
    }

    #endregion
  }

  public class s0: ISection 
  {
    public enum s0eff { l1, l2, r1, r2, u1, u2, d1, d2 };
    public enum s0sens { l, r, u, d, touch, tire };
    private BoundValue[] Effs = new[] { new BoundValue("LE1", BoundValue.MinValue), new BoundValue("LE2", BoundValue.MinValue), 
      new BoundValue("RE1", BoundValue.MinValue), new BoundValue("RE2", BoundValue.MinValue),
      new BoundValue("UE1", BoundValue.MinValue), new BoundValue("UE2", BoundValue.MinValue),
      new BoundValue("DE1", BoundValue.MinValue), new BoundValue("DE2", BoundValue.MinValue)};

    private BoundValue[] Sens = new[] { 
      new BoundValue("LS", BoundValue.MinValue + BoundValue.MaxValueModal / 2.0), 
      new BoundValue("RS", BoundValue.MinValue + BoundValue.MaxValueModal / 2.0), 
      new BoundValue("US", BoundValue.MinValue), 
      new BoundValue("DS", BoundValue.MaxValue), 
        new BoundValue("Stouch", BoundValue.MaxValue), 
          new BoundValue("Stire", BoundValue.MaxValue), 
    };
      
    string sectionName;

    public s0(string name)
    {
      sectionName = name;      
    }

    #region ISection Members

    public string GetSectionsName() { return sectionName; }

    public int GetEffectorsCount() { return Effs.Length; }

    public int GetSensorsCount() { return Sens.Length; }

    public int GetGoalSensorsCount() { return 0; }
    
    public double GetEffector(int index)
    {
      return Effs[index].Value;
    }

    public double GetSensor(int index)
    {
      return Sens[index].Value;
    }

    public double GetGoalSensor(int index)
    {
      throw new ArgumentOutOfRangeException();      
    }

    public string GetEffectorsName(int index)
    {
      return Effs[index].Name;
    }

    public string GetSensorsName(int index)
    {
      return Sens[index].Name;
    }

    public string GetGoalSensorsName(int index)
    {
      throw new ArgumentOutOfRangeException();
    }

    public void SetEffector(int index, double value)
    {
      Effs[index].Value = value;
    }
    #endregion

    public void Advantage()
    {
      //РАСЧЕТ
      double leftComponent = Effs[(int)s0eff.l1].ValueModal + Effs[(int)s0eff.l2].ValueModal;
      double rightComponent = Effs[(int)s0eff.r1].ValueModal + Effs[(int)s0eff.r2].ValueModal;
      double upComponent = Effs[(int)s0eff.u1].ValueModal + Effs[(int)s0eff.u2].ValueModal;
      double downComponent = Effs[(int)s0eff.d1].ValueModal + Effs[(int)s0eff.d2].ValueModal;

      const double koeff = 0.005;
      double curTire = Sens[(int)s0sens.tire].ValueModal / BoundValue.MaxValueModal;

      double leftAdd = koeff * leftComponent * (0.9 * curTire + 0.1); //90% зависит от tire
      double rightAdd = koeff * rightComponent * (0.9 * curTire + 0.1);
      double upAdd = koeff * upComponent * (0.9 * curTire + 0.1);
      double downAdd = koeff * downComponent * (0.9 * curTire + 0.1);

      //min = 0 max = 1;
      double newTire = (NormalizeModal(Effs[(int)s0eff.l1]) + NormalizeModal(Effs[(int)s0eff.l2]) +
        NormalizeModal(Effs[(int)s0eff.r1]) + NormalizeModal(Effs[(int)s0eff.r2]) +
        NormalizeModal(Effs[(int)s0eff.u1]) + NormalizeModal(Effs[(int)s0eff.u2]) +
        NormalizeModal(Effs[(int)s0eff.d1]) + NormalizeModal(Effs[(int)s0eff.d2])) / 8;

      newTire /= 1000;  //1000 моментов времени с максимальным напряжением

      //ОБНОВЛЕНИЕ      
      if (Sens[(int)s0sens.tire].ValueModal - newTire >= 0)
        Sens[(int)s0sens.tire].ValueModal -= newTire;
      else Sens[(int)s0sens.tire].ValueModal = 0;

      if (leftAdd >= rightAdd)
        UpdateSensor(leftAdd, rightAdd, ref Sens[(int)s0sens.l], ref Sens[(int)s0sens.r]);      
      else
        UpdateSensor(rightAdd, leftAdd, ref Sens[(int)s0sens.r], ref Sens[(int)s0sens.l]);

      if (upAdd >= downAdd)
        UpdateSensor(upAdd, downAdd, ref Sens[(int)s0sens.u], ref Sens[(int)s0sens.d]);
      else
        UpdateSensor(downAdd, upAdd, ref Sens[(int)s0sens.d], ref Sens[(int)s0sens.u]);

      if (Sens[(int)s0sens.d].Value == BoundValue.MaxValue)
        Sens[(int)s0sens.touch].Value = BoundValue.MaxValue;
      else
        Sens[(int)s0sens.touch].Value = BoundValue.MinValue;
    }

    void UpdateSensor(double a, double b, ref BoundValue sa, ref BoundValue sb)
    {
      a -= b;
      if (a > 0)
      {
        if (sa.Value + a > BoundValue.MaxValue)
          sa.Value = BoundValue.MaxValue;
        else
          sa.Value += a;
        sb.ValueModal = BoundValue.MaxValueModal - sa.ValueModal;
      }
    }

    public static double NormalizeModal(BoundValue val)
    {
      // =(-1600/(A101-125)-12,8)/51,2
      const double onePercent = (BoundValue.MaxValue - BoundValue.MinValue)/100.0;
      double percents = val.ValueModal / onePercent;
      return val.ValueModal * (-1600.0 / (percents - 125.0) - 12.8) / 51.2;      
    }
  }

  public class s1 : ISection //mixed section 1
  {
    public s0 mySection;
    private s0 twinSection;
    public s1(string name) { mySection = new s0(name); }
    public void SetTwin(s0 twin) { twinSection = twin; }
       
    /*public      
      BoundValue
      aLE1,
      aRE1,
      aDE1, aDE2,
      aLS, aUS, aDS,

      bLE2,
      bRE2,
      bUE1, bUE2,
      bRS,

      aStouch,
      aStire,
      bStouch,
      bStire;*/

    #region ISection Members

    public string GetSectionsName() { return mySection.GetSectionsName(); }
    public int GetEffectorsCount() { return 8; }
    public int GetSensorsCount() { return 8; }
    public int GetGoalSensorsCount() { return 0; }

    public double GetEffector(int index)
    {
      switch (index)
      {
        case (int)s0.s0eff.l1: 
        case (int)s0.s0eff.r1: 
        case (int)s0.s0eff.d1: 
        case (int)s0.s0eff.d2: return mySection.GetEffector(index);
        case (int)s0.s0eff.l2: 
        case (int)s0.s0eff.r2: 
        case (int)s0.s0eff.u1: 
        case (int)s0.s0eff.u2: return twinSection.GetEffector(index);
        default: throw new ArgumentOutOfRangeException();
      }
    }

    public string GetEffectorsName(int index)
    {
      switch (index)
      {
        case (int)s0.s0eff.l1:
        case (int)s0.s0eff.r1:
        case (int)s0.s0eff.d1:
        case (int)s0.s0eff.d2: return mySection.GetEffectorsName(index);
        case (int)s0.s0eff.l2:
        case (int)s0.s0eff.r2:
        case (int)s0.s0eff.u1:
        case (int)s0.s0eff.u2: return twinSection.GetEffectorsName(index);
        default: throw new ArgumentOutOfRangeException();
      }
    }

    public double GetSensor(int index)
    {
      switch (index)
      {
        case (int)s0.s0sens.l: 
        case (int)s0.s0sens.u: 
        case (int)s0.s0sens.d:         
        case (int)s0.s0sens.touch:
        case (int)s0.s0sens.tire: return mySection.GetSensor(index);
        
        case (int)s0.s0sens.r:
        case 6: return twinSection.GetSensor((int)s0.s0sens.touch);
        case 7: return twinSection.GetSensor((int)s0.s0sens.tire);
        
        default: throw new ArgumentOutOfRangeException();
      }
    }

    public string GetSensorsName(int index)
    {
      switch (index)
      {
        case (int)s0.s0sens.l:
        case (int)s0.s0sens.u:
        case (int)s0.s0sens.d:
        case (int)s0.s0sens.touch:
        case (int)s0.s0sens.tire: return mySection.GetSensorsName(index);

        case (int)s0.s0sens.r:
        case 6:
        case 7: return twinSection.GetSensorsName(index);

        default: throw new ArgumentOutOfRangeException();
      }
    }

    public double GetGoalSensor(int index)
    {
      throw new ArgumentOutOfRangeException();
    }

    public string GetGoalSensorsName(int index)
    {
      throw new ArgumentOutOfRangeException();
    }

    public void SetEffector(int index, double value)
    {
      switch (index)
      {
        case (int)s0.s0eff.l1: 
        case (int)s0.s0eff.r1: 
        case (int)s0.s0eff.d1: 
        case (int)s0.s0eff.d2: mySection.SetEffector(index, value); break;
        case (int)s0.s0eff.l2: 
        case (int)s0.s0eff.r2: 
        case (int)s0.s0eff.u1: 
        case (int)s0.s0eff.u2: twinSection.SetEffector(index, value); break;
        default: throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    public void Advantage()
    {
      mySection.Advantage();
    }
  }

  public class s2 : ISection //mixed section 2
  {
    public s0 mySection;
    private s0 twinSection;    
    public s2(string name) { mySection = new s0(name); }
    public void SetTwin(s0 twin) { twinSection = twin; }

    /* public
       BoundValue
       aLE2,
       aRE2,
       aUE1, aUE2,
       aRS,

       bLE1,
       bRE1,
       bDE1, bDE2,
       bLS, bUS, bDS;*/

    #region ISection Members

    public string GetSectionsName() { return mySection.GetSectionsName(); }
    public int GetEffectorsCount() { return 8; }
    public int GetSensorsCount() { return 4; }
    public int GetGoalSensorsCount() { return 0; }

    public double GetEffector(int index)
    {
      switch (index)
      {
        case (int)s0.s0eff.l2: 
        case (int)s0.s0eff.r2: 
        case (int)s0.s0eff.u1: 
        case (int)s0.s0eff.u2: return mySection.GetEffector(index);
        case (int)s0.s0eff.l1: 
        case (int)s0.s0eff.r1: 
        case (int)s0.s0eff.d1: 
        case (int)s0.s0eff.d2: return twinSection.GetEffector(index);
        default: throw new ArgumentOutOfRangeException();
      }
    }

    public string GetEffectorsName(int index)
    {
      switch (index)
      {
        case (int)s0.s0eff.l2:
        case (int)s0.s0eff.r2:
        case (int)s0.s0eff.u1:
        case (int)s0.s0eff.u2: return mySection.GetEffectorsName(index);
        case (int)s0.s0eff.l1:
        case (int)s0.s0eff.r1:
        case (int)s0.s0eff.d1:
        case (int)s0.s0eff.d2: return twinSection.GetEffectorsName(index);
        default: throw new ArgumentOutOfRangeException();
      }
    }

    public double GetSensor(int index)
    {
      switch (index)
      {
        case (int)s0.s0sens.r: return mySection.GetSensor(index);
        
        case (int)s0.s0sens.l: 
        case (int)s0.s0sens.u: 
        case (int)s0.s0sens.d: return twinSection.GetSensor(index);
        default: throw new ArgumentOutOfRangeException();
      }
    }

    public string GetSensorsName(int index)
    {
      switch (index)
      {
        case (int)s0.s0sens.r: return mySection.GetSensorsName(index);

        case (int)s0.s0sens.l:
        case (int)s0.s0sens.u:
        case (int)s0.s0sens.d: return twinSection.GetSensorsName(index);
        default: throw new ArgumentOutOfRangeException();
      }
    }

    public double GetGoalSensor(int index)
    {
      throw new NotImplementedException();
    }

    public string GetGoalSensorsName(int index)
    {
      throw new NotImplementedException();
    }

    public void SetEffector(int index, double value)
    {
      switch (index)
      {
        case (int)s0.s0eff.l2: 
        case (int)s0.s0eff.r2: 
        case (int)s0.s0eff.u1: 
        case (int)s0.s0eff.u2: mySection.SetEffector(index, value); break;
        case (int)s0.s0eff.l1: 
        case (int)s0.s0eff.r1: 
        case (int)s0.s0eff.d1: 
        case (int)s0.s0eff.d2: twinSection.SetEffector(index, value); break;
        default: throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    public void Advantage()
    {
      mySection.Advantage();
    }
  }

  public class sS0 : ISection
  {
    private
      BoundValue
      Sangle, Sdistance;
    string sectionName;

    public sS0(string name)
    {
      sectionName = name;
      Sangle = new BoundValue("Sangle");
      Sdistance = new BoundValue("Sdistance");
    }

    #region ISection Members

    public string GetSectionsName() { return sectionName; }
    public int GetEffectorsCount() { return 0; }
    public int GetSensorsCount() { return 2; }
    public int GetGoalSensorsCount() { return 0; }
    public double GetEffector(int index)
    { throw new NotImplementedException(); }
    public string GetEffectorsName(int index)
    { throw new NotImplementedException(); }
    
    public double GetSensor(int index)
    {
      switch (index)
      {
        case 0: return Sangle.Value;
        case 1: return Sdistance.Value;
        default: throw new NotImplementedException();
      }
    }

    public string GetSensorsName(int index)
    {
      switch (index)
      {
        case 0: return Sangle.Name;
        case 1: return Sdistance.Name;
        default: throw new NotImplementedException();
      }
    }

    public double GetGoalSensor(int index)
    { throw new NotImplementedException(); }
    public string GetGoalSensorsName(int index)
    { throw new NotImplementedException(); }
    public void SetEffector(int index, double value)
    { throw new NotImplementedException(); }

    #endregion

    public void SetTarget(Target tg)
    {
      Sangle.Value = tg.angle;
      Sdistance.Value = tg.distance;
    }
  }

  public class sS1 : ISection
  {
    private
      BoundValue
      Starget;
    string sectionName;
    public sS1(string name)
    {
      sectionName = name;
      Starget = new BoundValue("Starget", BoundValue.MaxValue);
    }

    #region ISection Members

    public string GetSectionsName() { return sectionName; }
    public int GetEffectorsCount() { return 0; }
    public int GetSensorsCount() { return 0; }
    public int GetGoalSensorsCount() { return 1; }
    public double GetEffector(int index)
    { throw new NotImplementedException(); }
    public double GetSensor(int index)
    { throw new NotImplementedException(); }
    public double GetGoalSensor(int index)
    {
      return Starget.Value;
    }

    public string GetEffectorsName(int index)
    { throw new NotImplementedException(); }
    public string GetSensorsName(int index)
    { throw new NotImplementedException(); }
    public string GetGoalSensorsName(int index)
    {
      return Starget.Name;
    }

    public void SetEffector(int index, double value)
    { throw new NotImplementedException(); }
    #endregion

    public void SetTarget(double tg)
    {
      Starget.Value = tg;
    }
  }
}
