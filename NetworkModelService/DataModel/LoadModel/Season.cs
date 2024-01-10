using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.LoadModel
{
    public class Season : IdentifiedObject
    {
        private DateTime endDate = new DateTime();
        private DateTime startDate = new DateTime();
        private List<long> schedules = new List<long>();
        public Season(long globalId) : base(globalId)
        {
        }

        public DateTime EndDate { get => endDate; set => endDate = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public List<long> Schedules { get => schedules; set => schedules = value; }




        public override bool Equals(object x)
        {
            if (base.Equals(x))
            {
                Season s = (Season)x;
                return (endDate.Equals(s.EndDate) && startDate.Equals(s.StartDate) && CompareHelper.CompareLists(s.schedules, this.schedules, true));
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
        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.SEASON_ENDDATE:
                case ModelCode.SEASON_STARTDATE:
                case ModelCode.SEASON_SEASONDAYTYPESCHS:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }
        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SEASON_ENDDATE:
                    property.SetValue(endDate);
                    break;
                case ModelCode.SEASON_STARTDATE:
                    property.SetValue(startDate);
                    break;
                case ModelCode.SEASON_SEASONDAYTYPESCHS:
                    property.SetValue(schedules);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }


        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SEASON_ENDDATE:
                    endDate = property.AsDateTime();
                    break;
                case ModelCode.SEASON_STARTDATE:
                    startDate = property.AsDateTime();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion IAccess implementation

        #region IReference implementation

        public override bool IsReferenced
        {
            get
            {
                return schedules.Count != 0 || base.IsReferenced;
            }
        }
        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SEASONDAYTYPESCHEDULE_SEASON:
                    schedules.Add(globalId);
                    break;
                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }
        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (schedules != null && schedules.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.SEASON_SEASONDAYTYPESCHS] = schedules.GetRange(0, schedules.Count);
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SEASONDAYTYPESCHEDULE_SEASON:
                    if (schedules.Contains(globalId))
                    {
                        schedules.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceError, "Entity (GID = 0x{ 0:X16}) doesn't contain reference 0x{ 1:X16}.", GlobalId, globalId);
                    }
                    break;
                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }
        #endregion IReference implementation
    }
}
