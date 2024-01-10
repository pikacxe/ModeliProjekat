namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    using FTN.Common;

    /// <summary>
    /// ProfileConverter has methods for populating
    /// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
    /// </summary>
    public static class ProfileConverter
    {

        #region Populate ResourceDescription
        public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
        {
            if ((cimIdentifiedObject != null) && (rd != null))
            {
                if (cimIdentifiedObject.MRIDHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
                }
                if (cimIdentifiedObject.NameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
                }
                if (cimIdentifiedObject.AliasNameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cimIdentifiedObject.AliasName));
                }
            }
        }

        public static void PopulatePowerSystemResourceProperties(FTN.PowerSystemResource cimPowerSystemResource, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimPowerSystemResource != null) && (rd != null))
            {
                ProfileConverter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);
            }
        }


        public static void PopulateEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimEquipment != null) && (rd != null))
            {
                ProfileConverter.PopulatePowerSystemResourceProperties(cimEquipment, rd, importHelper, report);
                
                if (cimEquipment.AggregateHasValue)
                {
                   rd.AddProperty(new Property(ModelCode.EQUIPMENT_AGGREGATE, cimEquipment.Aggregate));
                }
                if (cimEquipment.NormallyInServiceHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EQUIPMENT_NORMALLYINSERVICE, cimEquipment.NormallyInService));
                }
            }
        }

        public static void PopulateConductingEquipmentProperties(FTN.ConductingEquipment cimConductingEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimConductingEquipment != null) && (rd != null))
            {
                ProfileConverter.PopulateEquipmentProperties(cimConductingEquipment, rd, importHelper, report);
            }
        }

        public static void PopulateSwitchProperties(FTN.Switch cimSwitch, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSwitch != null) && (rd != null))
            {
                ProfileConverter.PopulateConductingEquipmentProperties(cimSwitch, rd, importHelper, report);

                if (cimSwitch.NormalOpenHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_NORMALOPEN, cimSwitch.NormalOpen));
                }
                if (cimSwitch.RatedCurrentHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_RATEDCURRENT, cimSwitch.RatedCurrent));
                }
                if (cimSwitch.RetainedHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_RETAINED, cimSwitch.Retained));
                }
                if (cimSwitch.SwitchOnCountHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_SWITCHONCOUNT, cimSwitch.SwitchOnCount));
                }
                if (cimSwitch.SwitchOnDateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_SWITCHONDATE, cimSwitch.SwitchOnDate));
                }
            }
        }

        public static void PopulateProtectedSwitchProperties(FTN.ProtectedSwitch cimProtectedSwitch, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimProtectedSwitch != null) && (rd != null))
            {
                ProfileConverter.PopulateSwitchProperties(cimProtectedSwitch, rd, importHelper, report);

                if (cimProtectedSwitch.BreakingCapacityHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PROTSWITCH_BREAKINGCAPACITY, cimProtectedSwitch.BreakingCapacity));
                }
            }
        }   

        public static void PopulateLoadBreakSwitchProperties(FTN.LoadBreakSwitch cimLoadBreakSwitch, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimLoadBreakSwitch != null) && (rd != null))
            {
                ProfileConverter.PopulateProtectedSwitchProperties(cimLoadBreakSwitch, rd, importHelper, report);
            }
        }

        public static void PopulateBreakerProperties(FTN.Breaker cimBreaker, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimBreaker != null) && (rd != null))
            {
                ProfileConverter.PopulateProtectedSwitchProperties(cimBreaker, rd, importHelper, report);
                
                if(cimBreaker.InTransitTimeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BREAKER_INTRANSITTIME, cimBreaker.InTransitTime));
                }
            }

        }

        public static void PopulateRecloserProperties(FTN.Recloser cimRecloser, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRecloser != null) && (rd != null))
            {
                ProfileConverter.PopulateProtectedSwitchProperties(cimRecloser, rd, importHelper, report);
            }
        }

        public static void PopulateBasicIntervalScheduleProperties(FTN.BasicIntervalSchedule cimBasicIntervalSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimBasicIntervalSchedule != null) && (rd != null))
            {
                ProfileConverter.PopulateIdentifiedObjectProperties(cimBasicIntervalSchedule, rd);

                if (cimBasicIntervalSchedule.StartTimeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTSCHEDULE_STARTTIME, cimBasicIntervalSchedule.StartTime));
                }
            }
        }

        public static void PopulateRegularIntervalScheduleProperties(FTN.RegularIntervalSchedule cimRegularIntervalSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRegularIntervalSchedule != null) && (rd != null))
            {
                ProfileConverter.PopulateBasicIntervalScheduleProperties(cimRegularIntervalSchedule, rd, importHelper, report);
            }
        }

        public static void PopulateSeasonDayTypeScheduleProperties(FTN.SeasonDayTypeSchedule cimSeasonDayTypeSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSeasonDayTypeSchedule != null) && (rd != null))
            {
                ProfileConverter.PopulateRegularIntervalScheduleProperties(cimSeasonDayTypeSchedule, rd, importHelper, report);
                if (cimSeasonDayTypeSchedule.DayTypeHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimSeasonDayTypeSchedule.DayType.ID);
                    if(gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimSeasonDayTypeSchedule.GetType().ToString()).Append(" rdfID = \"").Append(cimSeasonDayTypeSchedule.ID);
                        report.Report.Append("\" - Failed to set reference to DayType: rdfID \"").Append(cimSeasonDayTypeSchedule.DayType.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.SEASONDAYTYPESCHEDULE_DAYTYPE, gid));
                }
                if (cimSeasonDayTypeSchedule.SeasonHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimSeasonDayTypeSchedule.Season.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimSeasonDayTypeSchedule.GetType().ToString()).Append(" rdfID = \"").Append(cimSeasonDayTypeSchedule.ID);
                        report.Report.Append("\" - Failed to set reference to Season: rdfID \"").Append(cimSeasonDayTypeSchedule.Season.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.SEASONDAYTYPESCHEDULE_SEASON, gid));
                }
            }
        }

        public static void PopulateSwitchScheduleProperties(FTN.SwitchSchedule cimSwitchSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSwitchSchedule != null) && (rd != null))
            {
                ProfileConverter.PopulateSeasonDayTypeScheduleProperties(cimSwitchSchedule, rd, importHelper, report);

                if (cimSwitchSchedule.SwitchHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimSwitchSchedule.Switch.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimSwitchSchedule.GetType().ToString()).Append(" rdfID = \"").Append(cimSwitchSchedule.ID);
                        report.Report.Append("\" - Failed to set reference to Switch: rdfID \"").Append(cimSwitchSchedule.Switch.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.SWITCHSCHEDULE_SWITCH, gid));
                }
            }
        }

        public static void PopulateRegularTimePointProperties(FTN.RegularTimePoint cimRegularTimePoint, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRegularTimePoint != null) && (rd != null))
            {
                ProfileConverter.PopulateIdentifiedObjectProperties(cimRegularTimePoint, rd);

                if (cimRegularTimePoint.SequenceNumberHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULARTIMEPOINT_SEQNUMBER, cimRegularTimePoint.SequenceNumber));
                }
                if (cimRegularTimePoint.Value1HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULARTIMEPOINT_VALUE1, cimRegularTimePoint.Value1));
                }
                if (cimRegularTimePoint.Value2HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULARTIMEPOINT_VALUE2, cimRegularTimePoint.Value2));
                }
                if (cimRegularTimePoint.IntervalScheduleHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimRegularTimePoint.IntervalSchedule.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimRegularTimePoint.GetType().ToString()).Append(" rdfID = \"").Append(cimRegularTimePoint.ID);
                        report.Report.Append("\" - Failed to set reference to IntervalSchedule: rdfID \"").Append(cimRegularTimePoint.IntervalSchedule.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.REGULARTIMEPOINT_INTERVALSCH, gid));
                }
            }
        }

        public static void PopulateDayTypeProperties(FTN.DayType cimDayType, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimDayType != null) && (rd != null))
            {
                ProfileConverter.PopulateIdentifiedObjectProperties(cimDayType, rd);
            }
        }   

        public static void PopulateSeasonProperties(FTN.Season cimSeason, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSeason != null) && (rd != null))
            {
                ProfileConverter.PopulateIdentifiedObjectProperties(cimSeason, rd);
                if (cimSeason.EndDateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SEASON_ENDDATE, cimSeason.EndDate));
                }
                if (cimSeason.StartDateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SEASON_STARTDATE, cimSeason.StartDate));
                }
            }
        }

        #endregion Populate ResourceDescription
    }
}
