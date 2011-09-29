using System;
using System.Collections.Generic;
using System.Text;

namespace Low
{ 
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

      reactAlg = new ReactAlg(sections);   
    }

    /// <summary>
    /// Влияние среды произошло
    /// Перейти в следующий момент времени
    /// </summary>
    public void NotifyEnvironmentAffected()
    {
      if (myCr.GetState() != CreatureState.ENVIRONMENT_AFFECTED)
        throw new Exception("Невыполнение кричем условий последовательности вызова {59BB9671-9861-4238-A276-278298A120CB}");

      //{{ пропустить операции с памятью и проверку прогноза в первый момент жизни существования крича
      if (CurrentTick > 0) 
      {
        //установить память        
        foreach (Section s in sections)
          s.Advantage();
      
        //Проверить правильность прогноза значениев сенсоров
        bool result = true;
        foreach (Section s in sections)
        {
          if (!s.CheckPredictors())
            result = false;
        }
        //оповестить алгоритм реакции,как отработали предикторы      
        reactAlg.NotifyPredictorsResult(result);
      }
      //}}пропустить операции с памятью и проверку прогноза в первый момент жизни существования крича

      //L003 Алгоритм реакции в зависимости от текущего этапа
      reactAlg.Execute();

      //Прогнозировать значения сенсоров    
      foreach (Section s in sections)
        s.DoPrediction();

      ++fCurrentTick;
    }

    //L003 Алгоритм реакции в зависимости от текущего этапа
    private ReactAlg reactAlg;    
    /// <summary>
    /// L003 Алгоритм реакции в зависимости от текущего этапа
    /// </summary>
    private class ReactAlg
    {
      public ReactAlg(List<Section> sections)
      {
        this.sections = sections;
        curState = new StateGenesis(sections);
      }
      public void Execute()
      {
        curState.Execute();
      }      
      public void NotifyPredictorsResult(bool result)
      {
        predictorsWorkedWell = result;
        if (result)
        { //если прогноз верен
          if (curState is StateGenesis)
          { //если текущее состояние ЦНС стартовое
            curState = new StateLearning(sections, IsPredictorsWorkedWell, NotifyLearningIsDone);//перейти в состояние обучения
            return;
          }
        }
        else
        { //если прогноз неверен
        }
      }
      private bool predictorsWorkedWell = false;
      private bool IsPredictorsWorkedWell() { return predictorsWorkedWell; }
      //оповестить, что этап обучения закончен
      private void NotifyLearningIsDone()
      {
        curState = new StateNormal(sections);
        curState.Execute();
      }

      private List<Section> sections;
            
      //сосояние ЦНС L003
      private State curState;
      //базовый класс для состояния ЦНС L003
      private abstract class State
      {
        protected List<Section> sections;
        public State(List<Section> sections)
        {
          this.sections = sections;
        }
        public abstract void Execute();
      }      
      //стартовое состояние L003
      private class StateGenesis : State
      {
        public StateGenesis(List<Section> sections) : base(sections) { }
        public override void Execute()
        { 
          //ничего не делать в стартовом состоянии
        }
      }
      //этап обучения L004
      private class StateLearning : State
      {
        public StateLearning(List<Section> sections, BoolDelegate IsPredictorsWorkedWell, VoidDelegate NotifyLearningIsDone)
          : base(sections) 
        {
          this.NotifyLearningIsDone = NotifyLearningIsDone;
          this.IsPredictorsWorkWell = IsPredictorsWorkedWell;
        }       
        public override void Execute()
        {
          //{создать список диапозонов значений, которые были присвоены эффекторам
          if (L004_effectorsValues == null)
          { //эффекторы еще не были ни разу установлены        
            L004_effectorsValues = new List<EffInterval>();
            foreach (Section s in sections)
            {
              for (int i = 0; i < s.GetEffectorsCount(); ++i)
              {
                double eff = s.GetEffectorValue(i);                
                EffInterval effValue = new EffInterval(s, i,  eff, eff);
                L004_effectorsValues.Add(effValue);
              }
            }
          }
          //}создать список диапозонов значений, которые были присвоены эффекторам
                    
          
          //GEN 
          //можно определить приоритет обучения эффекторов
          if (L004_effectorsValues.Count == 0) return;          
          if (L004_currentEffector == null)          
            L004_currentEffector = L004_effectorsValues[0];
          
          //если предикторы отработали нормально, можно изменить эффектор
          if (IsPredictorsWorkWell())
          {
            if (L004_currentEffector.SetNextValue())
              return;

            //установить следующий эффектор
            for (int i = 0; i < L004_effectorsValues.Count; ++i)
            {
              EffInterval effInt = L004_effectorsValues[i];
              if (effInt.IsDone()) continue;
              else
              {
                L004_currentEffector = effInt;
                break;
              }
            }
            if (L004_currentEffector.SetNextValue())
              return;

            //если все эффекторы перебраны - перейти в штатный режим
            NotifyLearningIsDone();
          }
          else //****предикторы отработали неправильно
          {
            
          }

          //ISection sec = myCr.GetSection(section);
          //sec.SetEffector(effIndex, BoundValue.MaxValue);  
        }

        private BoolDelegate IsPredictorsWorkWell;
        private VoidDelegate NotifyLearningIsDone;

        // Диапазон значений, в которые были установлены эффекторы    
        private List<EffInterval> L004_effectorsValues = null;
        //Текущий проверяемый эффектор
        private EffInterval L004_currentEffector = null;        
        
        // Интервал значений эффектора        
        private class EffInterval
        {
          /// <summary>
          /// Заданные значения эффектора
          /// </summary>
          /// <param name="mySec">Секция эффектора</param>
          /// <param name="effectorIndex">Индекс эффектора</param>
          /// <param name="bottom">Нижнее значение</param>
          /// <param name="top">Верхнее значение</param>
          public EffInterval(Section mySec, int effectorIndex, double bottom, double top)
          {
            this.bottom = bottom;
            this.top = top;
            this.mySec = mySec;
            this.effectorIndex = effectorIndex;
          }
          public double top;
          public double bottom;
          /// <summary>
          /// Установить эффектор в следующее значение
          /// </summary>
          /// <returns>False - если все значения уже перебраты</returns>
          public bool SetNextValue()
          {
            //GEN можно определить направление и значение эффектора на этапе обучения

            if (done) return false;

            const double learningEffectorDeltaValue = BoundValue.MaxValueModal / 200;

            if (top < BoundValue.MaxValue)
            {
              if (top + learningEffectorDeltaValue > BoundValue.MaxValue)              
                top = BoundValue.MaxValue;
              else 
                top += learningEffectorDeltaValue;

              mySec.SetEffectorValue(effectorIndex, top);
              return true;
            }

            if (bottom < BoundValue.MinValue)
            {
              if (bottom - learningEffectorDeltaValue > BoundValue.MinValue)
                bottom = BoundValue.MinValue;
              else
                bottom -= learningEffectorDeltaValue;

              mySec.SetEffectorValue(effectorIndex, bottom);
              return true;
            }

            done = true;
            return false;
          }
          public bool IsDone() { return done; }
          //если уже перебраты все значения эффектора
          bool done = false;
          private int effectorIndex;
          private Section mySec;
        };
      }
      //штатное состояние L005
      private class StateNormal : State
      {
        public StateNormal(List<Section> sections) : base(sections) { }
        public override void Execute()
        {
        }
      }
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

    public bool CheckPredictors()
    {
      bool result = true;
      foreach (Sensor sn in sensors)
      {
        if (!sn.CheckPrediction())
          result = false; //если хоть один сенсор спрогнозирован неправильно - вся секция неправильная
      }
      foreach (Sensor sn in tSensors)
      {
        if (!sn.CheckPrediction())
          result = false; //если хоть один сенсор спрогнозирован неправильно - вся секция неправильная
      }
      return result;
    }

    private CNS myCns;
    private ISection mySec;
        
    private List<double>[] sensorsMem;
    private List<Sensor> sensors = new List<Sensor>();
    public int GetSensorsCount() { return mySec.GetSensorsCount(); }
    public double GetSensorValue(int index) { return mySec.GetSensor(index); }
        
    private List<double>[] tSensorsMem;
    private List<Sensor> tSensors = new List<Sensor>();
    public int GetTSensorsCount() { return mySec.GetGoalSensorsCount(); }
    public double GetTSensorValue(int index) { return mySec.GetGoalSensor(index); }

    private List<double>[] effsMem;
    private List<Effector> effs = new List<Effector>();
    public int GetEffectorsCount() { return mySec.GetEffectorsCount(); }
    public double GetEffectorValue(int index) { return mySec.GetEffector(index); }
    public void SetEffectorValue(int index, double value) { mySec.SetEffector(index, value); }

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
    }

    //этап первый - сделать предикцию
    public void DoPrediction()
    {
      if (pm == null)
        pm = new Predictor(mySec, this);
      pm.DoPrediction();
    }

    //этап второй - сравнить, произвести запись в память
    public bool CheckPrediction()
    {
      return pm.CheckPrediction();
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
