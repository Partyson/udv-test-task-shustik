namespace udvSummerSchoolTestTask.Entities;

public class UserEntity : BaseEntity
{
    public Guid Id { get; set; }
    public List<LetterEntity> Letters { get; set; } = [];
}