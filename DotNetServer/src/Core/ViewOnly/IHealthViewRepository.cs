using Core.Views;

namespace Core.ViewOnly
{
    public interface IHealthViewRepository : IViewRepository<HealthView>
    {
        HealthView[] HeathData();
    }
}