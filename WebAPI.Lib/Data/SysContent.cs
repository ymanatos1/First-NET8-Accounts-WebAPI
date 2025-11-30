using WebAPI.Data.Models;

namespace WebAPI.Lib.Data;

public class SysContent
{
    string Name { get; set; } = string.Empty;
    string ReleaseName { get; } = "Nov.2025";
    string DatabaseName { get; set; } = string.Empty;

    public IEnumerable<Account>? Accounts { get; set; }
    public IEnumerable<AccountCategory>? AccountCategories { get; set; }
}

