using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.LoadModel
{
    public class SeasonDayTypeSchedule : RegularIntervalSchedule
    {
        private long season;
        private long dayType;

        public SeasonDayTypeSchedule(long globalId) : base(globalId)
        {
        }

        public long Season { get => season; set => season = value; }
        public long DayType { get => dayType; set => dayType = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                SeasonDayTypeSchedule x = (SeasonDayTypeSchedule)obj;
                return (x.season == this.season && x.dayType == this.dayType);
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
                case ModelCode.SEASONDAYTYPESCHEDULE_DAYTYPE:
                    property.SetValue(dayType);
                    break;
                case ModelCode.SEASONDAYTYPESCHEDULE_SEASON:
                    property.SetValue(season);
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
                case ModelCode.SEASONDAYTYPESCHEDULE_DAYTYPE:
                case ModelCode.SEASONDAYTYPESCHEDULE_SEASON:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SEASONDAYTYPESCHEDULE_DAYTYPE:
                    dayType = property.AsReference();
                    break;
                case ModelCode.SEASONDAYTYPESCHEDULE_SEASON:
                    season = property.AsReference();
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
            if (dayType != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.SEASONDAYTYPESCHEDULE_DAYTYPE] = new List<long>();
                references[ModelCode.SEASONDAYTYPESCHEDULE_DAYTYPE].Add(dayType);
            }

            if (season != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.SEASONDAYTYPESCHEDULE_SEASON] = new List<long>();
                references[ModelCode.SEASONDAYTYPESCHEDULE_SEASON].Add(season);
            }

            base.GetReferences(references, refType);
        }

        #endregion IReference implementation
    }
}
