using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class Breaker : ProtectedSwitch
    {
        private float inTransitTime;
        public Breaker(long globalId) : base(globalId)
        {
        }

        public float InTransitTime { get => inTransitTime; set => inTransitTime = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Breaker breaker = (Breaker)obj;
                return (breaker.inTransitTime == this.inTransitTime);
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

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.BREAKER_INTRANSITTIME:
                    property.SetValue(inTransitTime);
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
                case ModelCode.BREAKER_INTRANSITTIME:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void SetProperty(Property property)
        {
            switch(property.Id)
            {
                case ModelCode.BREAKER_INTRANSITTIME:
                    inTransitTime = property.AsFloat();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
    }
}
