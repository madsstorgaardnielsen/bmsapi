using System.ComponentModel.DataAnnotations;
using BMSAPI.Database.Models;

namespace BMSAPI.Models;

public class DiaperDTO {
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public bool Wet { get; set; }
    public bool Poop { get; set; }
    // public Child Child { get; set; }
}

public class CreateDiaperDTO {
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }
    [Required] public bool Wet { get; set; }
    [Required] public bool Poop { get; set; }
    [Required] public string ChildId { get; set; }
}

public class UpdateDiaperDTO : CreateDiaperDTO {
    [Required]
    public string Id { get; set; }
}