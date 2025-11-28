using System.ComponentModel.DataAnnotations;

namespace WebAPI.Data.Models;

public class Dto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string AsString()
    {
        if (Id <= 0)
            return $"{Name}".Trim();
        else if (Name == null || Name == "")
            return $"with Id #{Id}".Trim();
        else
            return $"#{Id} - {Name}".Trim();
    }

}


public interface IHasActive
{
    bool IsActive { get; }
}

public class DtoWithActive : Dto, IHasActive
{
    [Required]
    public bool IsActive { get; set; }

}
