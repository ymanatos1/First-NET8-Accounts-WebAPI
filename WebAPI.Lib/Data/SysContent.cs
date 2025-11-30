using System.ComponentModel.DataAnnotations;
using WebAPI.Data.Models;

namespace WebAPI.Lib.Data;

public class SysContent
{
    [Required]
    public string Name { get; } = "WebAPi";
    [Required]
    public string ReleaseName { get; } = "1.Dec.2025";
    [Required]
    public string DatabaseName { get; } = string.Empty;

    public IEnumerable<Account>? Accounts { get; }
    public IEnumerable<AccountCategory>? AccountCategories { get; }

    public SysContent(IEnumerable<Account>? accounts, IEnumerable<AccountCategory>? accountCategories)
    {
        Accounts = accounts;
        AccountCategories = accountCategories;
    }
}

