using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class ProtectedSwitch : Switch
    {
        private float breakingCapacity;

        public ProtectedSwitch(long globalId) : base(globalId)
        {
        }

        public float BreakingCapacity { get => breakingCapacity; set => breakingCapacity = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                ProtectedSwitch protectedSwitch = (ProtectedSwitch)obj;
                return (protectedSwitch.breakingCapacity == this.breakingCapacity);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess implementation
        public override void GetProperty(Property property)
        {
            switch(property.Id)
            {
                case ModelCode.PROTSWITCH_BREAKINGCAPACITY:
                    property.SetValue(breakingCapacity);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.PROTSWITCH_BREAKINGCAPACITY:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void SetProperty(Property property)
        {
            switch(property.Id)
            {
                case ModelCode.PROTSWITCH_BREAKINGCAPACITY:
                    breakingCapacity = property.AsFloat();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion IAccess implementation
    }
}
