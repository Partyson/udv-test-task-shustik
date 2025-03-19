using EntityFrameworkCore.Repository;
using udvSummerSchoolTestTask.DataBases;
using udvSummerSchoolTestTask.Entities;
using udvSummerSchoolTestTask.Interfaces;

namespace udvSummerSchoolTestTask.Repositories;

public class LetterRepository (ApplicationDbContext context) : Repository<LetterEntity>(context), ILetterRepository;