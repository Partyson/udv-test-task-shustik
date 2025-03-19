namespace udvSummerSchoolTestTask.Entities;

public class LetterEntity : BaseEntity
{
    public char Letter { get; set; }
    public int Count { get; set; }
    public UserEntity User { get; set; }
}