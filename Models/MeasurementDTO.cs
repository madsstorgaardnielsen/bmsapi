using System.ComponentModel.DataAnnotations;
using BMSAPI.Database.Models;

namespace BMSAPI.Models;

public class MeasurementDTO {
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public double Weight { get; set; }
    public double Length { get; set; }
    public double HeadCircumference { get; set; }
    public string ChildId { get; set; }
}

public class CreateMeasurementDTO {
    [Required] public DateTime Date { get; set; }
    [Required] public double Weight { get; set; }
    [Required] public double Length { get; set; }
    [Required] public double HeadCircumference { get; set; }
    [Required] public string ChildId { get; set; }
}