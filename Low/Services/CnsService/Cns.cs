using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace CnsService
{
    public class Cns : ICns
    {
        public void AddSensor(string sensorName, double minValue, double maxValue)
        {
            throw new NotImplementedException();
        }

        public void AddTargetSensor(string sensorName, double minValue, double maxValue)
        {
            throw new NotImplementedException();
        }

        public void AddEffector(string effectorName, double minValue, double maxValue)
        {
            throw new NotImplementedException();
        }
    }
}
