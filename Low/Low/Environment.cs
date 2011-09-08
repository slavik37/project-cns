using System;
using System.Collections.Generic;
using System.Text;

namespace Low
{
  class Environment: IEnvironment_10 
  {
    /// <summary>
    /// Добавить создание к среде. Нельзя после старта.
    /// </summary>
    /// <param name="crt"></param>
    public void AddCreature(ISkotina_10 crt)
    {
      if (started)
        throw new Exception("Среда уже сконструирована и запущена");
      
      creatures.Add(crt);
      crt.SetFeedVector(0.0, 0.0);
      crt.SetTarget(0.0);
      crt.React();
      crt.DoPrediction();      
    }
    /// <summary>
    /// Запустить среду.
    /// </summary>
    public void Start()
    {
      started = true;
    }
    /// <summary>
    /// Индикатор, что среда создана и запущена.
    /// </summary>
    public bool Started
    { get { return started; } }
    private bool started = false;

    public void AdvantageMoment()
    {
      foreach (ISkotina_10 crt in creatures)
      {
        crt.Advantage();
        crt.SetFeedVector(0.0, 0.0);
        crt.SetTarget(0.0);
        crt.CheckPrediction();
        crt.React();
        crt.DoPrediction();        
      }
    }

    private List<ISkotina_10> creatures = new List<ISkotina_10>();  
  }

  public struct Target
  {
    public double angle;
    public double distance;
  }

  public interface IEnvironment_10
  {    
  }

  /// <summary>
  /// Конкретная скотина
  /// </summary>
  public interface ISkotina_10 : ICreature
  {    
    /// <summary>
    /// Среда задает данной скотине вектор на ближайшую еду
    /// </summary>
    /// <param name="angle">Угол</param>
    /// <param name="distance">Дистанция</param>
    void SetFeedVector(double angle, double distance);
    
    /// <summary>
    /// Среда задает уровень сытости скотины в зависимости от текущего уровня
    /// </summary>
    /// <param name="value">Уровень</param>
    void SetTarget(double value);
  }
}

