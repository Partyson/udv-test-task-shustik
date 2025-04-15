namespace udvSummerSchoolTestTask.Dto;

public class StatisticResponseDto
{
    public string VkUserId { get; set; }
    public Dictionary<char, int> Statistic { get; set; }
    
}