using System.ComponentModel.DataAnnotations;
using BMSAPI.Database.Models;

namespace BMSAPI.Models;

public class DiaperDTO {
    public string Id { get; set; }
    public DateTime DateTime { get; set; }
    public bool Wet { get; set; }

    public bool Poop { get; set; }
    // public Child Child { get; set; }
}

public class GetAllDiapersDTO {
    public string? ChildId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class CreateDiaperDTO {
    [Required] public string DateTime { get; set; }
    [Required] public bool Wet { get; set; }
    [Required] public bool Poop { get; set; }
    [Required] public string ChildId { get; set; }
}

public class UpdateDiaperDTO : CreateDiaperDTO {
    [Required] public string Id { get; set; }
}