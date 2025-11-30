using Newtonsoft.Json;

namespace WebAPI.Data.Models;

public class AccountCategory : Dto
{
    [JsonIgnore]
    public virtual IEnumerable<Account>? Accounts { get; set; } = null;

}
