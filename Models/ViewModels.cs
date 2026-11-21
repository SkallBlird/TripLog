// Name: Your Name
// Assignment 02 - Trips Log
using System.ComponentModel.DataAnnotations;

namespace TripLog.Models
{
    ///Page 1: destination, dates, and optional accommodation name.
    public class DestinationViewModel
    {
        [Required(ErrorMessage = "Please enter a destination.")]
        public string Destination { get; set; } = "";

        [Required(ErrorMessage = "Please enter a start date.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Please enter an end date.")]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        // Optional: if filled in, the user goes to Page 2
        public string? Accommodation { get; set; }
    }

    ///Page 2: accommodation details (all optional).
    public class AccommodationViewModel
    {
        [Display(Name = "Phone")]
        public string? AccommodationPhone { get; set; }

        [Display(Name = "Email")]
        public string? AccommodationEmail { get; set; }
    }

    ///Page 3: things to do (all optional).
    public class ActivitiesViewModel
    {
        [Display(Name = "Thing To Do 1")]
        public string? ThingToDo1 { get; set; }

        [Display(Name = "Thing To Do 2")]
        public string? ThingToDo2 { get; set; }

        [Display(Name = "Thing To Do 3")]
        public string? ThingToDo3 { get; set; }
    }
}