using System.Text.Json.Serialization;

namespace WebAPI.Data.Models;

public class AccountCategory : Dto
{
    [JsonIgnore]
    public virtual List<Account>? Accounts { get; set; } = null;

}
