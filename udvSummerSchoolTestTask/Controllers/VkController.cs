using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;
using udvSummerSchoolTestTask.Extensions;
using udvSummerSchoolTestTask.Interfaces;
using udvSummerSchoolTestTask.Services;

namespace udvSummerSchoolTestTask.Controllers;

[ApiController]
public class VkController : ControllerBase
{
    private readonly IVkService vkService;
    private readonly IUnitOfWork unitOfWork;


    public VkController(IVkService vkService, IUnitOfWork unitOfWork)
    {
        this.vkService = vkService;
        this.unitOfWork = unitOfWork;
    }

    [HttpPost("write-statistic")]
    public async Task<ActionResult> WriteStatistic([FromQuery] string vkUserId, [FromQuery] int count = 5)
    {
        var result = await vkService.CreateStatistic(vkUserId, count);
        await unitOfWork.SaveChangesAsync();
        if (result.IsError)
            return Problem(result.FirstError.Description, statusCode: result.FirstError.Type.ToStatusCode());
        return Ok($"Кол-во новых записей в базе данных: {result.Value}");
    }

    [HttpGet("get-statistic")]
    public async Task<ActionResult> GetStatistic(string vkUserId)
    {
        var result = await vkService.GetStatistic(vkUserId);
        if (result.IsError)
            return Problem(result.FirstError.Description, statusCode: result.FirstError.Type.ToStatusCode());
        return Ok(result.Value);
    }
}