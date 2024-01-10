using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using FTN.Services.NetworkModelService.DataModel.LoadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class SwitchSchedule : SeasonDayTypeSchedule
    {
        private long switchId = 0;
        public SwitchSchedule(long globalId) : base(globalId)
        {
        }

        public long SwitchId { get => switchId; set => switchId = value; }


        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                SwitchSchedule x = (SwitchSchedule)obj;
                return (x.switchId == this.switchId);
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
            switch (property.Id)
            {
                case ModelCode.SWITCHSCHEDULE_SWITCH:
                    property.SetValue(switchId);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }
        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.SWITCHSCHEDULE_SWITCH:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }
        public override void SetProperty(Property property)
        {
            switch(property.Id)
            {
                case ModelCode.SWITCHSCHEDULE_SWITCH:
                    switchId = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion IAccess implementation
        #region IReference implementation
        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if(switchId != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.SWITCHSCHEDULE_SWITCH] = new List<long>();
                references[ModelCode.SWITCHSCHEDULE_SWITCH].Add(switchId);
            }
        }
        #endregion IReference implementation

    }
}
