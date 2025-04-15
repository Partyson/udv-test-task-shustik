using ErrorOr;
using udvSummerSchoolTestTask.Dto;
using udvSummerSchoolTestTask.Extensions;
using udvSummerSchoolTestTask.Interfaces;

namespace udvSummerSchoolTestTask.Services;

public class VkService : IVkService
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;
    private readonly ILogger<VkService> logger;
    private readonly IStatisticsRepository statisticsRepository;

    public VkService(HttpClient httpClient, IConfiguration configuration,
        IStatisticsRepository statisticsRepository, ILogger<VkService> logger)
    {
        this.httpClient = httpClient;
        this.configuration = configuration;
        this.statisticsRepository = statisticsRepository;
        this.logger = logger;
    }

    public async Task<ErrorOr<int>> CreateStatistic(string vkUserId, int count)
    {
        logger.LogInformation($"Начало обработки для пользователя @{vkUserId}");
        var query = statisticsRepository.MultipleResultQuery()
            .AndFilter(x => x.VkUserId == vkUserId);
        var statisticWithCurrentVkUserId = await statisticsRepository.SearchAsync(query);
        if (statisticWithCurrentVkUserId.Count != 0)
        {
            logger.LogInformation($"Статистика для пользователя @{vkUserId} уже подсчитана");
            return Error.Conflict("General.Conflict", $"Статистика для пользователя @{vkUserId} уже подсчитана");
        }
        var response = await httpClient.GetFromJsonAsync<VkResponse>(CreateVkUrl(vkUserId, count));
        
        if (response.Response == null)
        {
            logger.LogInformation($"Пользователь @{vkUserId} скрыл свои посты от публичного доступа");
            return Error.Failure("General.NotFound", $"Пользователь @{vkUserId} скрыл свои записи от публичного доступа");
        }

        var statisticEntities = response.Response.Items
            .CreateStatisticEntities(vkUserId)
            .ToList();
        await statisticsRepository.AddRangeAsync(statisticEntities);
        logger.LogInformation($"Успешно обработано и записано в базу данных {statisticEntities.Count} записей для пользователя @{vkUserId}");
        return statisticEntities.Count;
    }

    public async Task<ErrorOr<StatisticResponseDto>> GetStatistic(string vkUserId)
    {
        var query = statisticsRepository.MultipleResultQuery()
            .AndFilter(x => x.VkUserId == vkUserId)
            .OrderBy(x => x.Letter);
        var statisticEntities = await statisticsRepository.SearchAsync(query);
        if (statisticEntities.Count == 0)
        {
            logger.LogInformation($"Не найдена статистика для @{vkUserId}");
            return Error.NotFound("General.NotFound", $"Не найдена статистика для @{vkUserId}");
        }

        return new StatisticResponseDto
        {
            VkUserId = vkUserId,
            Statistic = statisticEntities
                .ToDictionary(x => x.Letter, x => x.Count),
        };
    }

    private string CreateVkUrl(string vkUserId, int count) =>
        $"https://api.vk.com/method/wall.get?domain={vkUserId}&count={count}&access_token={configuration["VkSettings:AccessToken"]}&v={configuration["VkSettings:ApiVersion"]}";
}