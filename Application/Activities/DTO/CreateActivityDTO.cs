using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Activities.DTO;

public class CreateActivityDTO
{
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public string Category { get; set; } = string.Empty;

    //Location props
    [Required]
    public string City { get; set; } = string.Empty;
    [Required]
    public string Venue { get; set; } = string.Empty;
    [Required]
    public double Latitude { get; set; }
    [Required]
    public double Longitude { get; set; }
}
