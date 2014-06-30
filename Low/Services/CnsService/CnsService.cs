using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace CnsService
{
    public class CnsService : ICnsService
    {
        public ICns CreateCnc()
        {
            return new Cns();
        }
    }
}
