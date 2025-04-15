using ErrorOr;
using udvSummerSchoolTestTask.Dto;
using udvSummerSchoolTestTask.Entities;
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

    public async Task<ErrorOr<List<Guid>>> CreateStatistic(string vkUserId, int count)
    {
        logger.LogInformation($"Начало обработки для пользователя @{vkUserId}");
        var accessToken = configuration["VkSettings:AccessToken"];
        var apiVersion = configuration["VkSettings:ApiVersion"];
        var url = $"https://api.vk.com/method/wall.get?domain={vkUserId}&count={count}&access_token={accessToken}&v={apiVersion}";
        var response = await httpClient.GetFromJsonAsync<VkResponse>(url);
        
        if (response.Response == null)
        {
            logger.LogInformation($"Посты пользователя @{vkUserId} не найдены на странице в вк");
            return Error.Failure("General.NotFound", "Посты не найдены на странице в вк");
        }
        
        var statisticEntities = response.Response.Items.CountLetters()
            .Select(x => new StatisticEntity
            {
                Letter = x.Key,
                Count = x.Value,
                VkUserId = vkUserId
            }).ToList();
        await statisticsRepository.AddRangeAsync(statisticEntities);
        logger.LogInformation($"Успешно обработано и записано в базу данных {statisticEntities.Count} записей для пользователя @{vkUserId}");
        return statisticEntities.Select(x => x.Id).ToList();
    }

    public async Task<ErrorOr<StatisticResponseDto>> GetStatistic(string vkUserId)
    {
        var query = statisticsRepository.MultipleResultQuery()
            .AndFilter(x => x.VkUserId == vkUserId);
        var statisticEntities = await statisticsRepository.SearchAsync(query);
        if (statisticEntities.Count == 0)
        {
            logger.LogInformation($"Не найдена статистика для @{vkUserId}");
            return Error.NotFound("General.NotFound", $"Не найдена статистика для @{vkUserId}");
        }

        return new StatisticResponseDto
        {
            Statistics = statisticEntities
                .OrderBy(x => x.Letter)
                .ToDictionary(x => x.Letter, x => x.Count),
        };
    }
}