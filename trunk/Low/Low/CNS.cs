using System;
using System.Collections.Generic;
using System.Text;

namespace Low
{
  public interface ISection
  {
    string GetSectionsName();
    int GetEffectorsCount();
    int GetSensorsCount();
    int GetGoalSensorsCount();
    double GetEffector(int index);
    string GetEffectorsName(int index);
    double GetSensor(int index);
    string GetSensorsName(int index);
    double GetGoalSensor(int index);
    string GetGoalSensorsName(int index);
    void SetEffector(int index, double value);
  }

  public interface ICreature
  {
    int SectionsCount();
    ISection GetSection(int index);
    void DoPrediction();
    void Advantage();
    void CheckPrediction();
    void React();
  }

  public class CNS
  {
    public CNS(ICreature cr)
    {
      myCr = cr;

      //формировать секции
      int seCount = cr.SectionsCount();
      for (int seIndex = 0; seIndex < seCount; ++seIndex)
      {
        ISection sec = cr.GetSection(seIndex);
        Section section = new Section(sec, this);
        sections.Add(section);
      }
    }

    /// <summary>
    /// L003 Общий алгоритм
    /// </summary>
    public void React()
    {
      //L004
      if (!L003_learningComplete)
        ReactLearning();
      else
        ReactDefault();
    }

    /// <summary>
    /// L003 индикатор окончания этапа обучения
    /// </summary>
    private bool L003_learningComplete = false;

    /// <summary>
    /// L004 Алгоритм этапа обучения
    /// </summary>
    private void ReactLearning()
    {
      int section = 0;
      int effIndex = 0;

      ISection sec = myCr.GetSection(section);
      sec.SetEffector(effIndex, BoundValue.MaxValue);      
    }
    //private

    /// <summary>
    /// L005 Алгоритм этапа реакция 
    /// </summary>
    private void ReactDefault()
    {
    }

    public void DoPrediction()
    {
      foreach (Section s in sections)
        s.DoPrediction();
    }

    public void Advantage()
    {
      ++fCurrentTick;
      foreach (Section s in sections)
        s.Advantage();
    }

    public void CheckPrediction()
    {
      foreach (Section s in sections)
        s.CheckPredictors();
    }

    private ICreature myCr;
    private List<Section> sections = new List<Section>();

    private double fCurrentTick = 0;
    public double CurrentTick
    {
      get { return fCurrentTick; }
    }
  }

  class Section
  {
    public Section(ISection sec, CNS cns)
    {
      myCns = cns;
      mySec = sec;

      //получить эффекторы
      int effCount = sec.GetEffectorsCount();
      effsMem = new List<double>[effCount];
      for (int effInd = 0; effInd < effCount; ++effInd)
      {
        effsMem[effInd] = new List<double>();        
        Effector eff = new Effector(effInd, this);
        effs.Add(eff);
      }

      //получить сенсоры
      int sensCount = sec.GetSensorsCount();
      sensorsMem = new List<double>[sensCount];
      for (int sensInd = 0; sensInd < sensCount; ++sensInd)
      {
        sensorsMem[sensInd] = new List<double>();        
        Sensor sens = new Sensor(sensInd, this, false);
        sensors.Add(sens);
      }

      //получить целевые сенсоры
      int tSensCount = sec.GetGoalSensorsCount();
      tSensorsMem = new List<double>[tSensCount];
      for (int tSensInd = 0; tSensInd < tSensCount; ++tSensInd)
      {
        tSensorsMem[tSensInd] = new List<double>();        
        Sensor tSens = new Sensor(tSensInd, this, true);
        tSensors.Add(tSens);
      }
    }

    public string SectionsName { get { return mySec.GetSectionsName(); } }
    public string GetSensorsName(int index) { return mySec.GetSensorsName(index); }
    public string GetGoalSensorsName(int index) { return mySec.GetGoalSensorsName(index); }
    public string GetEffectorsName(int index) { return mySec.GetEffectorsName(index); }

    public void Advantage()
    {
      //обновить эффекторы
      int effCount = mySec.GetEffectorsCount();
      for (int effInd = 0; effInd < effCount; ++effInd)      
        effsMem[effInd].Add(mySec.GetEffector(effInd));

      //обновить сенсоры
      int sensCount = mySec.GetSensorsCount();
      for (int sensInd = 0; sensInd < sensCount; ++sensInd)      
        sensorsMem[sensInd].Add(mySec.GetSensor(sensInd));
      
      //обновить целевые сенсоры
      int tSensCount = mySec.GetGoalSensorsCount();
      for (int tSensInd = 0; tSensInd < tSensCount; ++tSensInd)      
        tSensorsMem[tSensInd].Add(mySec.GetGoalSensor(tSensInd));              
    }

    public void DoPrediction()
    {
      foreach (Sensor sn in sensors)
        sn.DoPrediction();
      foreach (Sensor sn in tSensors)
        sn.DoPrediction();
    }

    public void CheckPredictors()
    {
      foreach (Sensor sn in sensors)
        sn.CheckPrediction();
      foreach (Sensor sn in tSensors)
        sn.CheckPrediction();
    }

    public void SetEffectorValue(int index, double value)
    {
      mySec.SetEffector(index, value);
    }

    private CNS myCns;
    private ISection mySec;
        
    private List<double>[] sensorsMem;
    public List<Sensor> sensors = new List<Sensor>();
    public double GetSensorValue(int index) { return mySec.GetSensor(index); }
        
    private List<double>[] tSensorsMem;
    public List<Sensor> tSensors = new List<Sensor>();
    public double GetTSensorValue(int index) { return mySec.GetGoalSensor(index); }

    private List<double>[] effsMem;
    public List<Effector> effs = new List<Effector>();
    public double GetEffectorValue(int index) { return mySec.GetEffector(index); }

    public double CurrentTick { get { return myCns.CurrentTick; } }
  }

  abstract class Cell
  {
    public Cell(int index, Section mySec)
    {
      this.index = index;
      this.mySec = mySec;
    }

    public abstract double CurrentValue { get; }
    public abstract string Name { get; }

    public readonly int index;
    public Section mySec;
  }

  class Sensor : Cell
  {
    public Sensor(int index, Section mySec, bool thisIsTarget)
      : base(index, mySec)
    {
      this.thisIsTarget = thisIsTarget;
      pm = new Predictor(mySec, this);
    }

    //этап первый - сделать предикцию
    public void DoPrediction()
    {
      pm.DoPrediction();
    }

    //этап второй - сравнить, произвести запись в память
    public void CheckPrediction()
    {
      pm.CheckPrediction();
    }

    public override double CurrentValue
    {
      get
      {
        if (thisIsTarget)
          return mySec.GetTSensorValue(index);
        else
          return mySec.GetSensorValue(index);
      }
    }

    public override string Name
    {
      get
      {
        return mySec.SectionsName + " " + (thisIsTarget ? mySec.GetGoalSensorsName(index) : mySec.GetSensorsName(index));
      }
    }

    public override string ToString()
    {
      return Name + ": " + CurrentValue.ToString();
    }

    private bool thisIsTarget;
    private Predictor pm;
  }

  class Effector : Cell
  {
    public Effector(int index, Section mySec)
      : base(index, mySec) { }

    public override double CurrentValue
    {
      get { return mySec.GetEffectorValue(index); }
    }

    public override string Name
    {
      get
      {
        return mySec.SectionsName + " " + mySec.GetEffectorsName(index);
      }
    }

    public override string ToString()
    {
      return Name + ": " + CurrentValue.ToString();
    }
  }
}
