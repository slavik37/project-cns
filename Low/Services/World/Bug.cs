using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.Practices.ServiceLocation;
using Action = Interfaces.Action;

namespace WorldService
{
    public class Bug : ICreature
    {
        private Dictionary<string, Leg> _legs;
        private const string LeftFront = "LeftFront";
        private const string RightFront = "RightFront";
        private const string LeftBack = "LeftBack";
        private const string RightBack = "RightBack";

        private ICns _cns;

        public Bug()
        {
            _legs = new Dictionary<string, Leg>
            {
                {LeftFront, new Leg(LeftFront, 0.0, 0.0)},
                {RightFront, new Leg(RightFront, 0.0, 0.0)},
                {LeftBack, new Leg(LeftBack, 0.0, 0.0)},
                {RightBack, new Leg(RightBack, 0.0, 0.0)}
            };

            var cnsService =
                (ICnsService)
                    ServiceLocator.Current.GetService(typeof(ICnsService));

            _cns = cnsService.CreateCnc();

            foreach (var leg in _legs)
            {
                _cns.AddSensor(leg.Value.GetHorizontalAngleSensorName(), -100.0, 100.0);
                _cns.AddSensor(leg.Value.GetVerticalAngleSensorName(), -100.0, 100.0);
                _cns.AddSensor(leg.Value.GetTouchSensorName(), 0.0, 1.0);
                _cns.AddEffector(leg.Value.GetFirstHorEffectorName(), 0.0, 100.0);
                _cns.AddEffector(leg.Value.GetSecondHorEffectorName(), 0.0, 100.0);
                _cns.AddEffector(leg.Value.GetFirstVerEffectorName(), 0.0, 100.0);
                _cns.AddEffector(leg.Value.GetSecondVerEffectorName(), 0.0, 100.0);
            }

            _cns.AddSensor("TireSens", 0.0, 1000.0);
            _cns.AddTargetSensor("HungSens", 0.0, 1000.0);
            _cns.AddSensor("FoodDistSens", 0.0, 1000.0);
            _cns.AddSensor("FoodAngleSens", 0.0, Math.PI);
        }

        public List<Action> GetActions()
        {
            throw new NotImplementedException();
        }

        public void UpdateSensors()
        {
            throw new NotImplementedException();
        }
    }

    public class Leg
    {
        private readonly string _name;
        private double _horizontalAngle;
        private double _verticalAngle;


        public string GetHorizontalAngleSensorName()
        {
            return _name + "HorSens";
        }

        public string GetVerticalAngleSensorName()
        {
            return _name + "VerSens";
        }

        public string GetTouchSensorName()
        {
            return _name + "TouchSens";
        }

        public string GetFirstHorEffectorName()
        {
            return _name + "HorEffFirst";
        }

        public string GetSecondHorEffectorName()
        {
            return _name + "HorEffSecond";
        }

        public string GetFirstVerEffectorName()
        {
            return _name + "VerEffFirst";
        }

        public string GetSecondVerEffectorName()
        {
            return _name + "VerEffSecond";
        }

        public Leg(string name, double horizontalAngle, double verticalAngle)
        {
            _name = name;
            _horizontalAngle = horizontalAngle;
            _verticalAngle = verticalAngle;
        }
    }
}
