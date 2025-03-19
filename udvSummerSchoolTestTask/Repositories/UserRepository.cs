using EntityFrameworkCore.Repository;
using udvSummerSchoolTestTask.DataBases;
using udvSummerSchoolTestTask.Entities;
using udvSummerSchoolTestTask.Interfaces;

namespace udvSummerSchoolTestTask.Repositories;

public class UserRepository(ApplicationDbContext context) : Repository<UserEntity>(context), IUserRepository;