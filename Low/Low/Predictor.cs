using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Low
{
  class Predictor
  {
    public Predictor(Section mySect, Sensor mySens)
    {
      this.mySect = mySect;
      this.mySens = mySens;
      this.entries = new List<PredictorEntry>();
      this.predictedValue = mySens.CurrentValue;
      this.predictedForTick = int.MaxValue;

      List<Interval> intervals = new List<Interval>();
      intervalsCount = mySect.GetEffectorsCount() + mySect.GetSensorsCount() + mySect.GetTSensorsCount();

      for (int i = 0; i < intervalsCount; ++i)
        intervals.Add(
          new Interval()
          );

      PredictorEntry pe = new PredictorEntry();
      pe.intervals = intervals;
      pe.predictor = new CP_Const(mySens.CurrentValue);

      entries.Add(pe);
    }

    public void DoPrediction()
    {
      if (this.predictedForTick == mySect.CurrentTick)
        return;

      List<double> intervals = new List<double>();
      for (int i = 0; i < mySect.GetEffectorsCount(); ++i)
        intervals.Add(mySect.GetEffectorValue(i));
      for (int i = 0; i < mySect.GetSensorsCount(); ++i)
        intervals.Add(mySect.GetSensorValue(i));
      for (int i = 0; i < mySect.GetTSensorsCount(); ++i)
        intervals.Add(mySect.GetTSensorValue(i));
      var query = entries.Where(entry =>
      {
        for (int i = 0; i < intervalsCount; ++i)
          if (entry.intervals[i] != intervals[i])
            return false;
        return true;
      }
      );

      List<ConcreatePredictor> predictors = new List<ConcreatePredictor>();
      foreach (PredictorEntry pe in query)
        predictors.Add(pe.predictor);

      if (predictors.Count != 1)
        throw new Exception("Ошибка {0C168423-33BB-458E-984F-C22F149B07AA}");

      predictedForTick = mySect.CurrentTick;
      predictedValue = predictors[0].DoPrediction();
    }

    public bool CheckPrediction()
    {
      if (predictedForTick != mySect.CurrentTick - 1)
        throw new Exception("Ошибка {501B9974-B906-4FB8-856F-2E48E8111DC6}");
      if (predictedValue != mySens.CurrentValue)
      {
        РаботыЕслиПредикторНеПравильноПредсказалъ();
        return false;
      }
      return true;
    }

    //focus here
    void РаботыЕслиПредикторНеПравильноПредсказалъ()
    {
      
    }

    private double predictedValue;
    private double predictedForTick;
    private int intervalsCount;
    private Section mySect;
    private Sensor mySens;
    private List<PredictorEntry> entries;
  }

  class PredictorEntry
  {
    public List<Interval> intervals;
    public ConcreatePredictor predictor;
  }

  class Interval
  {
    public Interval()
    {
      upper.operation = IntervalOp.Eternity;
      upper.value = BoundValue.MinValue;
      lower.operation = IntervalOp.Eternity;
      lower.value = BoundValue.MinValue;
    }
    IntervalBound upper;
    IntervalBound lower;

    public static bool operator ==(Interval pe, double d)
    {
      //достаточно одного интервала, 
      //т.к. +беск и -беск достигаются IntervalOp.MoreOrEqual или IntervalOp.LessOrEqual
      if (pe.lower.operation == IntervalOp.Eternity)
        return true;

      if (
          (
           (pe.lower.operation == IntervalOp.Equal && pe.lower.value == d) ||
           (pe.lower.operation == IntervalOp.MoreOrEqual && pe.lower.value >= d) ||
           (pe.lower.operation == IntervalOp.More && pe.lower.value > d)
          )
          &&
          (
           (pe.upper.operation == IntervalOp.Equal && pe.upper.value == d) ||
           (pe.upper.operation == IntervalOp.LessOrEqual && pe.upper.value <= d) ||
           (pe.upper.operation == IntervalOp.Less && pe.upper.value < d)
          )
         )
        return true;
      else
        return false;
    }

    public static bool operator !=(Interval pe, double d)
    {
      return !(pe == d);
    }

    public override int GetHashCode()
    {
      return lower.value.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (obj.GetType() != typeof(Interval))
        return false;

      return this == (Interval)obj;
    }
  }

  struct IntervalBound
  {
    public IntervalOp operation;
    public double value;
  }

  enum IntervalOp
  {
    Eternity,
    Less,
    More,
    LessOrEqual,
    MoreOrEqual,
    Equal
  }

  abstract class ConcreatePredictor
  {
    public abstract double DoPrediction();
  }

  class CP_Const : ConcreatePredictor
  {
    public CP_Const(double value)
    {
      fValue = value;
    }

    public override double DoPrediction()
    {
      return fValue;
    }

    double fValue;
  }
}
