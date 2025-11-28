using WebAPI.Data.Models;
using WebAPI.Data.Models.Db;
using WebAPI.Data.Services.Base;
using WebAPI.Lib.Data.Services;
using WebAPI.Lib.WebAPI.Query;

namespace WebAPI.Data.Services;

public class AccountCategoryService : DtoApiServiceBase<AccountCategory>, IAccountCategoryService
{
    public override string DtoName { get; } = "Account Category";
    public override string DtoNamePlural { get; } = "Account Categories";
    public override string DtoPath { get; } = "AccountCategories"; // DtoNamePlural.Replace(" ", "");

    public bool DBSetHasAny(int id) { return base.DbSetHasAny(id); }


    public AccountCategoryService(IAccountsDbContext? db = null, ILogger? logger = null)
        : base(db, logger, db?.AccountCategories ?? null)
    {
    }

    //public new async Task<IEnumerable<AccountCategory>> Get(DtoQueryParameters? q = null)
    //{
    //    return await base.Get(q);
    //}

    //public new async Task<AccountCategory> GetById(int id)
    //{
    //    return await base.GetById(id);
    //}

    ////public async Task<int> Add(ModelStateDictionary ModelState, Account account)
    //public new async Task<int> Add(bool isValid, AccountCategory entry)
    //{
    //    return await base.Add(isValid, entry);
    //}

    ////public async Task<int> Update(ModelStateDictionary ModelState, int id, Account account)
    //public new async Task<int> Update(bool isValid, int id, AccountCategory entry)
    //{
    //    return await base.Update(isValid, id, entry);
    //}

    //public new async Task<int> RemoveById(int id)
    //{
    //    return await base.RemoveById(id);
    //}

}
