using ErrorOr;
using udvSummerSchoolTestTask.Dto;

namespace udvSummerSchoolTestTask.Interfaces;

public interface IVkService
{
    Task<ErrorOr<List<Guid>>> CreateStatistic(string vkUserId, int count);
    Task<ErrorOr<StatisticResponseDto>> GetStatistic(string vkUserId);
}