namespace udvSummerSchoolTestTask.Services;

public static class LetterCounter
{
    public static Dictionary<char, int> CountLetters(IEnumerable<string> texts)
    {
        var combinedText = string.Concat(texts).ToLowerInvariant();
        return combinedText
            .Where(char.IsLetter)
            .GroupBy(c => c)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}