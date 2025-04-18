﻿using EntityFrameworkCore.Repository;
using udvSummerSchoolTestTask.DataBases;
using udvSummerSchoolTestTask.Entities;
using udvSummerSchoolTestTask.Interfaces;

namespace udvSummerSchoolTestTask.Repositories;

public class StatisticsRepository (ApplicationDbContext context) : Repository<StatisticEntity>(context), IStatisticsRepository;