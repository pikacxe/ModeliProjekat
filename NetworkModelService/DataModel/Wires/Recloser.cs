using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class Recloser : ProtectedSwitch
    {
        public Recloser(long globalId) : base(globalId)
        {
        }
    }
}
