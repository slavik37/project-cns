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

  /// <summary>
  /// Состояние крича
  /// Каждый крич последовательно меняет состояния при определенном воздействии на него среды
  /// Договор такой: 
  ///  Сначала крич обладает состоянием JUST_ADDED_OR_MOVED
  /// В этом состоянии среда может узнать значения эффекторов, выставленных у крича, 
  ///  и согласно им изменить некоторые его сенсоры (по законам пары крич/среда)
  ///  После этого среда должна вызвать метода EnvironmentAffected(), который переведет крич в состояние ENVIRONMENT_AFFECTED
  /// ENVIRONMENT_AFFECTED
  ///  Как только крича уведомили, что EnvironmentAffected, он должен в свою очередь уведомить об этом ЦНС
  ///  который после этого совершит операции с памятью, проверит предикторы, выставит эффекторы и сделает прогноз.
  /// Крич выставляет состояние CREATURE_REACTED
  /// Теперь среда вызывает метод крича - Advantage() 
  ///  Он по состояниям эффекторов, выставленных ЦНС - крич выставляет свои сенсоры и устанавливает состояние JUST_ADDED_OR_MOVED
  ///  Теперь среда вносит свои коррективы в сенсоры крича и вызывает EnvironmentAffected() 
  ///  и так по кругу
  /// </summary>
  public enum CreatureState { JUST_ADDED_OR_MOVED, ENVIRONMENT_AFFECTED, CREATURE_REACTED }

  /// <summary>
  /// Интерфейс крича  
  /// </summary>
  public interface ICreature
  {
    int SectionsCount();
    ISection GetSection(int index);
    void EnvironmentAffected();
    void Advantage();
    CreatureState GetState();
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

  class Environment 
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
      crt.SetFeedVector(BoundValue.MinValue, BoundValue.MinValue);
      crt.SetTarget(BoundValue.MinValue);
      crt.EnvironmentAffected();  
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
        crt.SetFeedVector(BoundValue.MinValue, BoundValue.MinValue);
        crt.SetTarget(BoundValue.MinValue);
        crt.EnvironmentAffected();       
      }
    }
    private List<ISkotina_10> creatures = new List<ISkotina_10>();  
  }

  public struct Target
  {
    public double angle;
    public double distance;
  }


}

