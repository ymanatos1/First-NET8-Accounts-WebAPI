using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI.Data.Models;

public class Account : DtoWithActive
{
    [Required]
    public int CategoryId {  get; set; }
    [JsonIgnore]
    public virtual AccountCategory? Category { get; set; } = null;

}
