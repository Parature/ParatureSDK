using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class TimezonesList : PagedData.PagedData
    {
        public List<Timezone> Timezones = new List<Timezone>();
    }
}