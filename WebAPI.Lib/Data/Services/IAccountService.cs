using System.Collections.Generic;
using WebAPI.Data.Models;
using WebAPI.Lib.WebAPI.Query;

namespace WebAPI.Lib.Data.Services
{
    public interface IDtoInfo
    {
        string DtoName { get; }
        string DtoNamePlural { get; }
        string DtoPath { get; }
    }


    public interface IDtoService<T, TT> : IDtoInfo
        where T : Dto, new()
        where TT : DtoQueryParameters
    {
        bool DBSetHasAny(int id);

        //Task<IEnumerable<T>> Get(DtoWithActiveQueryParameters? q = null);
        Task<IEnumerable<T>> Get(TT? q = null);
        Task<T> GetById(int id);
        //Task<int> Add(ModelStateDictionary ModelState, T entry);
        Task<int> Add(bool isValid, T entry);
        //Task<int> Update(ModelStateDictionary ModelState, int id, T entry);
        Task<int> Update(bool isValid, int id, T entry);
        //Task<int> Patch(int id, JsonPatchDocument<T> patchDoc);
        Task<int> RemoveById(int id);
    }

    public interface IAccountService : IDtoService<Account, DtoWithActiveQueryParameters>
    {
        /*
        string DtoName { get; }
        string DtoNamePlural { get; }

        Task<IEnumerable<Account>> Get(DtoWithActiveQueryParameters? q = null);
        Task<Account> GetById(int id);
        //Task<int> Add(ModelStateDictionary ModelState, Account account);
        Task<int> Add(bool isValid, Account account);
        //Task<int> Update(ModelStateDictionary ModelState, int id, Account account);
        Task<int> Update(bool isValid, int id, Account account);
        //Task<int> Patch(int id, JsonPatchDocument<Account> patchDoc);
        Task<int> RemoveById(int id);
        */
    }

    public interface IAccountCategoryService : IDtoService<AccountCategory, DtoQueryParameters>
    {
        /*
        string DtoName { get; }
        string DtoNamePlural { get; }

        Task<IEnumerable<AccountCategory>> Get(DtoQueryParameters? q = null);
        Task<AccountCategory> GetById(int id);
        //Task<int> Add(ModelStateDictionary ModelState, AccountCategory accountCategory);
        Task<int> Add(bool isValid, AccountCategory accountCategory);
        //Task<int> Update(ModelStateDictionary ModelState, int id, AccountCategory accountCategory);
        Task<int> Update(bool isValid, int id, AccountCategory accountCategory);
        //Task<int> Patch(int id, JsonPatchDocument<Account> patchDoc);
        Task<int> RemoveById(int id);
        */
    }



    public interface IDtoServiceClient<T, TT> : IDtoInfo
        where T : Dto, new()
        where TT : DtoQueryParameters
    {
        T New();

        //Task<IEnumerable<T>> Get(TT? q = null);
        Task<IEnumerable<T>> GetAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T?> AddAsync(T entry);
        Task<bool> UpdateAsync(int id, T entry);
        //Task<int> Patch(int id, JsonPatchDocument<T> patchDoc);
        Task<bool> RemoveByIdAsync(int id);
    }

    public interface IAccountServiceClient : IDtoServiceClient<Account, DtoWithActiveQueryParameters>
    {
        //string DtoName { get; }
        //string DtoNamePlural { get; }

        ////Task<IEnumerable<Account>> Get(DtoWithActiveQueryParameters? q = null);
        //Task<IEnumerable<Account>> GetAsync();
        //Task<Account?> GetByIdAsync(int id);
        //Task<Account?> AddAsync(Account account);
        //Task<bool> UpdateAsync(int id, Account account);
        ////Task<int> Patch(int id, JsonPatchDocument<Account> patchDoc);
        //Task<bool> RemoveByIdAsync(int id);
    }

    public interface IAccountCategoryServiceClient : IDtoServiceClient<AccountCategory, DtoQueryParameters>
    {
        //string DtoName { get; }
        //string DtoNamePlural { get; }

        ////Task<IEnumerable<AccountCategory>> Get(AccountQueryParameters? q = null);
        //Task<IEnumerable<AccountCategory>> GetAsync();
        //Task<AccountCategory?> GetByIdAsync(int id);
        //Task<AccountCategory?> AddAsync(AccountCategory accountCategory);
        //Task<bool> UpdateAsync(int id, AccountCategory accountCategory);
        ////Task<int> Patch(int id, JsonPatchDocument<Account> patchDoc);
        //Task<bool> RemoveByIdAsync(int id);
    }

}
