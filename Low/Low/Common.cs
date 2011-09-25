using System;
using System.Collections.Generic;
using System.Text;

namespace Low
{
  public delegate bool BoolDelegate();
  public delegate void VoidDelegate();

  public class BoundValue
  {
    public const double MinValue = -100.0;
    public const double MaxValue = 100.0;
    public const double MaxValueModal = MaxValue - MinValue;

    private double value_ = MinValue;
    
    public BoundValue(string name)
    {
      fName = name;      
    }

    public BoundValue(string name, double value)
    {
      fName = name;
      Value = value;
    }
    
    public double Value
    {
      get { return value_; }
      set
      {
        if (value > MaxValue || value < MinValue)
          throw new ArgumentOutOfRangeException();          
        value_ = value;
      }
    }

    public double ValueModal
    {
      get { return value_ - MinValue; }
      set
      {
        if (value > MaxValueModal || value < 0)
          throw new ArgumentOutOfRangeException();
        value_ = MinValue + value;
      }
    }

    private string fName = "";
    public string Name { get { return fName; } }
  }      
}