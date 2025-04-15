using ErrorOr;
using udvSummerSchoolTestTask.Dto;

namespace udvSummerSchoolTestTask.Interfaces;

public interface IVkService
{
    Task<ErrorOr<int>> CreateStatistic(string vkUserId, int count);
    Task<ErrorOr<StatisticResponseDto>> GetStatistic(string vkUserId);
}