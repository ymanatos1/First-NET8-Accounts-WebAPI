using WebAPI.Data.Models;
using WebAPI.Data.Models.Db;
using WebAPI.Data.Services.Base;
using WebAPI.Lib.Data.Services;
using WebAPI.Lib.WebAPI.Query;

namespace WebAPI.Data.Services;

public class AccountService : DtoApiServiceBase<Account>, IAccountService
{
    //private readonly ILogger _logger;
    //private readonly IAccountsDbContext _db;

    public override string DtoName { get; } = "Account";
    public override string DtoNamePlural { get; } = "Accounts";
    public override string DtoPath { get; } = "AccountCategories"; // DtoNamePlural.Replace(" ", "");

    public bool DBSetHasAny(int id) { return base.DbSetHasAny(id); }


    public AccountService(IAccountsDbContext? db = null, ILogger? logger = null)
        : base(db, logger, db?.Accounts ?? null)
    {
        /*
        _logger = logger ?? NullLogger<AccountService>.Instance;
        
        //_db = db ?? ServiceLocator.Provider!..GetRequiredService<IAccountsDbContext>();
        _db = db ?? ServiceLocator.InMemoryAccountsDB!;
        if (_db == null)
        {
            throw new ArgumentNullException("NULL database context!");
        }
        */
    }

    public async Task<IEnumerable<Account>> Get(DtoWithActiveQueryParameters? q = null)
    {
        /*
        IQueryable<Account> accounts = q == null
            ? _db.Accounts
            : _db.Accounts.Where(a => q.IsActive == null ? true : a.IsActive == q.IsActive);

        if (q != null)
        {
            if (!string.IsNullOrEmpty(q.Name))
            {
                accounts = accounts.Where(a => a.Name.ToLower().Contains(q.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(q.Description))
            {
                accounts = accounts.Where(a => a.Description.ToLower().Contains(q.Description.ToLower()));
            }

            if (!string.IsNullOrEmpty(q.SortBy))
            {
                var prop = typeof(Account).GetProperty(q.SortBy,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop != null)
                {
                    accounts = accounts.OrderByCustom(prop.Name, q.SortOrder);
                }
            }

            // Pagination
            if (q.Page < 1) _logger.LogWarning("Pagination page is {page}!", q.Page);
            accounts = accounts
                .Skip(q.Size * (Math.Max(q.Page, 1) - 1))
                .Take(q.Size);
        }

        return await accounts.ToArrayAsync();
        */
        return await base.Get(q);
    }

    //public new async Task<Account> GetById(int id)
    //{
        /*
        if (id <= 0)
        {
            throw new BadIdException("Id must be greater than 0.", id);
        }

        //var account = await _db.Accounts.FindAsync(id);
        var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == id);
        if (account == null)
        {
            throw new DataNotFoundException("Account not found.", id);
        }

        return account;
        */
    //    return await base.GetById(id);
    //}

    ////public async Task<int> Add(ModelStateDictionary ModelState, Account account)
    //public new async Task<int> Add(bool isValid, Account entry)
    //{
        /*
        //if (!ModelState.IsValid)
        if (!isValid)
        {
            throw new ModelStateInvalidException("ModelState is invalid.", account);
        }

        _db.Accounts.Add(account);
        var entriesWritten = await _db.SaveChangesAsync();

        switch (entriesWritten)
        {
            case 1:
                return entriesWritten;
            case 0:
                throw new DataException("No data written in database.");
            default:
                throw new DataException($"{entriesWritten} entries written in database when 1 entry is added.");
        }
        */
    //    return await base.Add(isValid, entry);
    //}

    ////public async Task<int> Update(ModelStateDictionary ModelState, int id, Account account)
    //public new async Task<int> Update(bool isValid, int id, Account entry)
    //{
        /*
        //if (!ModelState.IsValid)
        if (!isValid)
        {
            throw new ModelStateInvalidException("ModelState is invalid.", account);
        }

        if (account.Id == 0)
        {
            account.Id = id;
        }
        else if (id <= 0)
        {
            throw new BadIdException("Id must be greater than 0.", id);
        }
        else if (id != account.Id)
        {
            throw new BadIdException("Cannot update entry with a different id.", id);
        }

        _db.Entry(account).State = EntityState.Modified;

        var entriesWritten = await _db.SaveChangesAsync();

        switch (entriesWritten)
        {
            case 1:
                return entriesWritten;
            case 0:
                throw new DataException("No data updated in database.");
            default:
                throw new DataException($"{entriesWritten} entries updated in database when 1 entry is changed.");
        }
        */
    //    return await base.Update(isValid, id, entry);
    //}

    /*public async Task<int> Patch(int id, JsonPatchDocument<Account> patchDoc)
    {
        //if (!ModelState.IsValid)
        //{
        //    throw new ModelStateInvalidException("ModelState is invalid.", account);
        //}

        //if (account.Id == 0)
        //{
        //    account.Id = id;
        //}
        //else if (id <= 0)
        //{
        //    throw new BadIdException("Id must be greater than 0.", id);
        //}
        //else if (id != account.Id)
        //{
        //    throw new BadIdException("Cannot update entry with a different id.", id);
        //}

        ////var account = await _db.Accounts.FindAsync(id);
        //var record = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == id);
        //if (record == null)
        //{
        //    throw new DataNotFoundException("Account not found.", id);
        //}

        //_db.Entry(account).State = EntityState.Modified;

        //var entriesWritten = await _db.SaveChangesAsync();

        //switch (entriesWritten)
        //{
        //    case 1:
        //        return entriesWritten;
        //    case 0:
        //        throw new DataException("No data updated in database.");
        //    default:
        //        throw new DataException($"{entriesWritten} entries updated in database when 1 entry is changed.");
        //}

        await Task.Delay(10);
        return 1;
    }*/

    //public new async Task<int> RemoveById(int id)
    //{
        /*
        if (id <= 0)
        {
            throw new BadIdException("Id must be greater than 0.", id);
        }

        //var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == id);
        var account = await _db.Accounts.FindAsync(id);
        if (account == null)
        {
            throw new DataNotFoundException("Account not found.", id);
        }

        var entry = _db.Accounts.Remove(account);
        var entriesRemoved = await _db.SaveChangesAsync();

        switch (entriesRemoved)
        {
            case 1:
                return entriesRemoved;
            case 0:
                throw new DataException("No data removed from database.");
            default:
                throw new DataException($"{entriesRemoved} entries deleted from database when 1 entry is removed.");
        }
        */
    //    return await base.RemoveById(id);
    //}

}
