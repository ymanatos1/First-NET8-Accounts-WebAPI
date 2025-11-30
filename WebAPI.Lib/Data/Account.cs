using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebAPI.Data.Models;

public class Account : DtoWithActive
{
    [Required]
    [Username(ErrorMessage =
        "Name must start with a letter and contain only letters, digits, - or _. Max 20 characters.")]
    public new string Name { get; set; } = string.Empty;

    [Required]
    public int CategoryId {  get; set; }
    [JsonIgnore]
    public virtual AccountCategory? Category { get; set; } = null;

}
