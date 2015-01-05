using System.Linq;
using Core.ViewOnly.Base;
using Core.Views;

namespace Core.ViewOnly.Impl
{
    public class HealthViewRepository : ViewRepository<HealthView>, IHealthViewRepository
    {
        public HealthView[] HeathData()
        {
            return Fetch(new Sql("Select top 60 * FROM HealthView  order by RecordTime, HealthType")).ToArray();
        }
    }
}