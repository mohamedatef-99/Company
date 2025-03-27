using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Company.PL.Models;
using Company.PL.Services;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Company.PL.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IScopedSerivces scopedSerivces01;
    private readonly IScopedSerivces scopedSerivces02;
    private readonly ITransientService transientService01;
    private readonly ITransientService transientService02;
    private readonly ISingletionService singletionService01;
    private readonly ISingletionService singletionService02;

    public HomeController(ILogger<HomeController> logger,
        IScopedSerivces scopedSerivces01,
        IScopedSerivces scopedSerivces02,
        ITransientService transientService01,
        ITransientService transientService02,
        ISingletionService singletionService01,
        ISingletionService singletionService02
        )
    {
        _logger = logger;
        this.scopedSerivces01 = scopedSerivces01;
        this.scopedSerivces02 = scopedSerivces02;
        this.transientService01 = transientService01;
        this.transientService02 = transientService02;
        this.singletionService01 = singletionService01;
        this.singletionService02 = singletionService02;
    }

    public string TestLifeTime()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append($"scopedService01 :: {scopedSerivces01.GetGuid()}\n");
        builder.Append($"scopedService01 :: {scopedSerivces02.GetGuid()}\n\n");
        builder.Append($"transentService :: {transientService01.GetGuid()}\n");
        builder.Append($"transentService :: {transientService02.GetGuid()}\n\n");
        builder.Append($"singletonService :: {singletionService01.GetGuid()}\n");
        builder.Append($"singletonService :: {singletionService02.GetGuid()}\n\n");

        return builder.ToString();
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
