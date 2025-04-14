using ErrorOr;
using udvSummerSchoolTestTask.Entities;
using udvSummerSchoolTestTask.Interfaces;

namespace udvSummerSchoolTestTask.Services;

public class VkService : IVkService
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;
    private readonly ILogger<VkService> logger;
    private readonly IStatisticsRepository statisticsRepository;

    public VkService(HttpClient httpClient, IConfiguration configuration, IStatisticsRepository statisticsRepository, ILogger<VkService> logger)
    {
        this.httpClient = httpClient;
        this.configuration = configuration;
        this.statisticsRepository = statisticsRepository;
        this.logger = logger;
    }

    public async Task<ErrorOr<List<Guid>>> CreateStatistic(string vkUserId, int count)
    {
        logger.LogInformation($"Начало обработки для пользователя @{vkUserId}");
        var accessToken = configuration["VkSettings:AccessToken"];
        var url = $"https://api.vk.com/method/wall.get?owner_id=-{vkUserId}&count={count}&access_token={accessToken}";
        var response = await httpClient.GetFromJsonAsync<List<string>>(url);
        if (response.Count == 0)
        {
            logger.LogInformation($"Посты пользователя @{vkUserId} не найдены");
            return Error.Failure("General.Failure", "Посты не найдены");
        }
        var statistic = LetterCounter.CountLetters(response);
        var statisticEntities = statistic.Select(x => new StatisticEntity
        {
            Letter = x.Key,
            Count = x.Value,
            VkUserId = vkUserId
        }).ToList();
        await statisticsRepository.AddRangeAsync(statisticEntities);
        logger.LogInformation($"Успешно обработано и записано в базу данных {statisticEntities.Count} записей для пользователя @{vkUserId}");
        return statisticEntities.Select(x => x.Id).ToList();
    }
}