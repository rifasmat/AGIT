using Rifa.Models;

namespace Rifa.Services.Planning
{
    public interface IPlanningService
    {
        object ProcessPlanning(ProductionDay data);
        List<object> GetHistory();
        object GetDetail(string planningCd);
        object UpdateStatus(string planningCd, bool isActive);
    }
}