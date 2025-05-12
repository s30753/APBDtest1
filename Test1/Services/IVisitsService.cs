using Test1.Models;

namespace Test1.Services;

public interface IVisitsService
{
    Task<Visit> GetVisitByIdAsync(int visit_id);
    Task<int> AddVisitAsync(Visit visit);
}