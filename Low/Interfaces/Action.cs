using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public struct Action
    {
        public ActionType ActionType;
        public double Value;
    }

    public enum ActionType
    {
        Move,
        Rotate
    }
}
