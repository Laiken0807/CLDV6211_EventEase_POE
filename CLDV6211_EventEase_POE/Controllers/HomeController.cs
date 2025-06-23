using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CLDV6211_EventEase_POE.Data;
using CLDV6211_EventEase_POE.Models;

namespace CLDV6211_EventEase_POE.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CLDV6211_EventEase_POEContext _context;

    public HomeController(ILogger<HomeController> logger, CLDV6211_EventEase_POEContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Load Eventease items from DB
        var events = await _context.eventeases.ToListAsync();
        return View(events); // Pass them to the view
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
