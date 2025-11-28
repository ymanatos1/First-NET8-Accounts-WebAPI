using System.IO;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;
using WebApp.Lib.Data.ViewModels.Base;

namespace WebApp.Lib.Data.ViewModels;


public class AccountCategoriesVM : DtosVM<AccountCategory>
{
    public AccountCategoriesVM() { }
    public AccountCategoriesVM(CRUD crud, IDtoInfo dtoInfo, bool isModal, string path, 
        IEnumerable<AccountCategory> entries)
        : base(crud, dtoInfo, isModal, path, entries)
    { }
}

public class AccountCategoryVM : DtoVM<AccountCategory>
{
    public AccountCategoryVM() { }
    public AccountCategoryVM(CRUD crud, IDtoInfo dtoInfo, bool isModal, string path,
        AccountCategory entry)
        : base(crud, dtoInfo, isModal, path, entry)
    { }
}
