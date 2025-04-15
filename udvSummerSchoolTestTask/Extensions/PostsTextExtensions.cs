using udvSummerSchoolTestTask.Dto;

namespace udvSummerSchoolTestTask.Extensions;

public static class PostsTextExtensions
{
    internal static Dictionary<char, int> CountLetters(this IEnumerable<VkPost> vkPosts)
    {
        var texts = vkPosts.Select(x => x.Text).ToList();
        var combinedText = string.Concat(texts).ToLowerInvariant();
        return combinedText
            .Where(char.IsLetter)
            .GroupBy(c => c)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}