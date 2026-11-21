using Microsoft.AspNetCore.Mvc;
using TripLog.Data;
using TripLog.Models;

/// Handles the three-page Add Trip process. Data from pages 1 and 2 is held
/// in TempData until the user clicks Save on page 3.
public class AddController : Controller
{
    private AppDbContext _context;

    public AddController(AppDbContext context)
    {
        _context = context;
    }

    /// Starts a new Add Trip process with a clean TempData.
    [HttpGet]
    public IActionResult Page1()
    {
        TempData.Clear();
        return View(new DestinationViewModel());
    }

    /// Validates page 1, stores it in TempData, and picks the next page
    [HttpPost]
    public IActionResult Page1(DestinationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Data is stored in TempData between "pages"
        TempData["Destination"] = model.Destination;
        TempData["StartDate"] = model.StartDate;
        TempData["EndDate"] = model.EndDate;
        TempData["Accommodation"] = model.Accommodation?.Trim() ?? "";

        // Page 2 only appears if the user entered an accommodation
        if (string.IsNullOrWhiteSpace(model.Accommodation))
        {
            return RedirectToAction("Page3");
        }
        return RedirectToAction("Page2");
    }


    /// Shows page 2 with the accommodation name in the subhead.
    [HttpGet]
    public IActionResult Page2()
    {
        if (TempData.Peek("Destination") == null)
        {
            return RedirectToAction("Index", "Home");   // process was not started
        }

        string? accommodation = TempData.Peek("Accommodation")?.ToString();
        if (string.IsNullOrWhiteSpace(accommodation))
        {
            return RedirectToAction("Page3");           // page 2 not needed
        }

        ViewBag.Subhead = accommodation;
        return View(new AccommodationViewModel());
    }
    /// Stores accommodation details in TempData and moves to page 3.
    [HttpPost]
    public IActionResult Page2(AccommodationViewModel model)
    {
        TempData["AccommodationPhone"] = model.AccommodationPhone ?? "";
        TempData["AccommodationEmail"] = model.AccommodationEmail ?? "";
        return RedirectToAction("Page3");
    }


    /// Shows page 3 with the destination in the subhead.
    [HttpGet]
    public IActionResult Page3()
    {
        string? destination = TempData.Peek("Destination")?.ToString();
        if (destination == null)
        {
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Subhead = destination;
        return View(new ActivitiesViewModel());
    }

    /// Builds the Trip from TempData plus page 3 and saves it.
    [HttpPost]
    public async Task<IActionResult> Page3(ActivitiesViewModel model)
    {
        string? destination = TempData.Peek("Destination")?.ToString();
        if (destination == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var trip = new Trip
        {
            Destination = destination,
            StartDate = DateTime.Parse(TempData.Peek("StartDate")!.ToString()!),
            EndDate = DateTime.Parse(TempData.Peek("EndDate")!.ToString()!),
            Accommodation = new Accommodation
            {
                Name = TempData.Peek("Accommodation")?.ToString(),
                Phone = TempData.Peek("AccommodationPhone")?.ToString(),
                Email = TempData.Peek("AccommodationEmail")?.ToString()
            },
            ThingsToDo = new ThingsToDo
            {
                Activity1 = model.ThingToDo1,
                Activity2 = model.ThingToDo2,
                Activity3 = model.ThingToDo3
            }
        };

        _context.Trips.Add(trip);
        await _context.SaveChangesAsync();

        TempData.Clear();
        TempData["Message"] = $"Trip to {trip.Destination} added.";
        return RedirectToAction("Index", "Home");
    }

    /// Clears TempData and returns to the Home page.
    [HttpPost]
    public IActionResult Cancel()
    {
        TempData.Clear();
        return RedirectToAction("Index", "Home");
    }
}