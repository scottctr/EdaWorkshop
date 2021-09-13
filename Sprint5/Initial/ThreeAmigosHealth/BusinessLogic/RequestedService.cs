using System.ComponentModel;

namespace BusinessLogic
{
    public enum RequestedService
    {
        [Description("Long-Term Care")]
        LongTermCare,
        
        [Description("Sleep Study")]
        SleepStudy,

        [Description("Fertility Assistance")]
        FertilityAssistance
    }
}