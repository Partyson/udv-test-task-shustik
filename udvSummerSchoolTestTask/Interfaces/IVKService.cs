using ErrorOr;
namespace udvSummerSchoolTestTask.Interfaces;

public interface IVkService
{
    Task<ErrorOr<List<Guid>>> CreateStatistic(string vkUserId, int count);
}