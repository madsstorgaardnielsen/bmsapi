using System.ComponentModel.DataAnnotations;
using BMSAPI.Database.Models;

namespace BMSAPI.Models;

public class ChildDTO {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Birthdate { get; set; }
    public List<UserDTO> Parents { get; set; }
}

public class SimpleChildDTO {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Birthdate { get; set; }
}

public class CreateChildDTO {
    [Required] public string ParentOneId { get; set; }

    public string ParentTwoId { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public string Birthdate { get; set; }

    [Required] [DataType(DataType.Text)] public string Name { get; set; }
}

public class UpdateChildDTO {
    [Required] [DataType(DataType.Text)] public string Id { get; set; }

    public string ParentOneId { get; set; }

    public string ParentTwoId { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Birthdate { get; set; }

    [Required] [DataType(DataType.Text)] public string Name { get; set; }
}

public class ChildFeedingProfileDTO {
    public SimpleChildDTO Child { get; set; }
    public SimpleFeedingProfileDTO FeedingProfile { get; set; }
}