using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;
using WebApp.Lib.Data.ViewModels.Base;

namespace WebApp.Lib.Data.ViewModels;

public class AccountsVM : DtosVM<Account>
{
    public IEnumerable<AccountCategory> CategoryList { get; set; } = [];

    public AccountsVM() : base() { }
    public AccountsVM(CRUD crud, IDtoInfo dtoInfo, bool isModal, string path,
        IEnumerable<Account> entries,
        IEnumerable<AccountCategory> categoryList)
        : base(crud, dtoInfo, isModal, path, entries)
    {
        CategoryList = categoryList;
    }
}

public class AccountVM : DtoVM<Account>
{
    public IEnumerable<AccountCategory> CategoryList { get; set; } = [];

    public AccountVM() : base() { }
    public AccountVM(CRUD crud, IDtoInfo dtoInfo, bool isModal, string path,
        Account entry,
        IEnumerable<AccountCategory> categoryList)
        : base(crud, dtoInfo, isModal, path, entry)
    {
        CategoryList = categoryList;
    }
}
