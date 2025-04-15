using udvSummerSchoolTestTask.Dto;
using udvSummerSchoolTestTask.Entities;

namespace udvSummerSchoolTestTask.Extensions;

public static class PostsTextExtensions
{
    internal static IEnumerable<StatisticEntity> CreateStatisticEntities(this IEnumerable<VkPost> vkPosts, string vkUserId)
    {
        var texts = vkPosts.Select(x => x.Text).ToList();
        var combinedText = string.Concat(texts).ToLowerInvariant();
        return combinedText
            .Where(char.IsLetter)
            .GroupBy(c => c)
            .Select(x => new StatisticEntity
            {
                Letter = x.Key,
                Count = x.Count(),
                VkUserId = vkUserId
            });
    }
}