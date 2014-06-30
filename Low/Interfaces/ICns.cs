using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ICns
    {
        void AddSensor(string sensorName, double minValue, double maxValue);
        void AddTargetSensor(string sensorName, double minValue, double maxValue);
        void AddEffector(string effectorName, double minValue, double maxValue);
    }
}
