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
    public DateTime Date { get; set; }
    public double Weight { get; set; }
    public double Length { get; set; }
    public double HeadCircumference { get; set; }
    public string ChildId { get; set; }
}

