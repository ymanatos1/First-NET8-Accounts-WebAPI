using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;
using WebAPI.Data.Models;
using WebAPI.Data.Models.Db;
using WebAPI.Exceptions;
using WebAPI.Lib.WebAPI.Query;

namespace WebAPI.Data.Services.Base;

public abstract class DtoApiServiceBase<T>
    where T : Dto
{
    protected readonly ILogger _logger;
    protected readonly IAccountsDbContext _db;

    //protected abstract DbSet<T> _dbSet { get; }
    //protected DbSet<T> _dbSet { get; protected init; }
    protected DbSet<T>? _dbSet { get; }
    protected bool DbSetHasAny(int id)
    {
        return _dbSet!.Any(a => a.Id == id);
    }

    public abstract string DtoName { get; }
    public abstract string DtoNamePlural { get; }
    public abstract string DtoPath { get; }

    public DtoApiServiceBase(IAccountsDbContext? db = null, ILogger? logger = null, DbSet<T>? dbSet = null)
    {
        _logger = logger ?? NullLogger<AccountCategoryService>.Instance;

        //_db = db ?? ServiceLocator.Provider!..GetRequiredService<IAccountsDbContext>();
        _db = db ?? ServiceLocator.InMemoryAccountsDB!;
        if (_db == null)
        {
            throw new ArgumentNullException("NULL database context!");
        }
        _dbSet = dbSet;
    }

    public async Task<IEnumerable<T>> Get(DtoQueryParameters? q = null)
    {
        IQueryable<T> entries; /* = q == null
            ? _db.Accounts
            : _db.Accounts.Where(a => q.IsActive == null ? true : a.IsActive == q.IsActive); */
        if (q != null && q is DtoWithActiveQueryParameters && typeof(IHasActive).IsAssignableFrom(typeof(T)))
        {
            var qq = (DtoWithActiveQueryParameters)q;
            entries = _dbSet!.Where(a => qq.IsActive == null ? true : ((IHasActive)a).IsActive == qq.IsActive);
        }
        else
        {
            entries = _dbSet!;
        }

        if (q != null)
        {
            if (!string.IsNullOrEmpty(q.Name))
            {
                entries = entries.Where(a => a.Name.ToLower().Contains(q.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(q.Description))
            {
                entries = entries.Where(a => a.Description.ToLower().Contains(q.Description.ToLower()));
            }

            if (!string.IsNullOrEmpty(q.SortBy))
            {
                var prop = typeof(Account).GetProperty(q.SortBy,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop != null)
                {
                    entries = entries.OrderByCustom(prop.Name, q.SortOrder);
                }
            }

            // Pagination
            if (q.Page < 1) _logger.LogWarning("{dtoNamePlural} pagination page is {page}!", DtoNamePlural, q.Page);
            entries = entries
                .Skip(q.Size * (Math.Max(q.Page, 1) - 1))
                .Take(q.Size);
        }

        return await entries.ToArrayAsync();
    }

    public async Task<T> GetById(int id)
    {
        if (id <= 0)
        {
            throw new BadIdException("Id must be greater than 0.", id);
        }

        var entries = await _dbSet!.FirstOrDefaultAsync(a => a.Id == id);
        if (entries == null)
        {
            throw new DataNotFoundException($"{DtoName} not found.", id);
        }

        return entries;
    }

    //public async Task<int> Add(ModelStateDictionary ModelState, Account account)
    public async Task<int> Add(bool isValid, T entry)
    {
        //if (!ModelState.IsValid)
        if (!isValid)
        {
            throw new ModelStateInvalidException<T>("ModelState is invalid.", entry);
        }

        _dbSet!.Add(entry);
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
    }

    //public async Task<int> Update(ModelStateDictionary ModelState, int id, Account account)
    public async Task<int> Update(bool isValid, int id, T entry)
    {
        //if (!ModelState.IsValid)
        if (!isValid)
        {
            throw new ModelStateInvalidException<T>("ModelState is invalid.", entry);
        }

        if (entry.Id == 0)
        {
            entry.Id = id;
        }
        else if (id <= 0)
        {
            throw new BadIdException("Id must be greater than 0.", id);
        }
        else if (id != entry.Id)
        {
            throw new BadIdException("Cannot update entry with a different id.", id);
        }

        _db.Entry(entry).State = EntityState.Modified;

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
    }

    public async Task<int> RemoveById(int id)
    {
        if (id <= 0)
        {
            throw new BadIdException("Id must be greater than 0.", id);
        }

        //var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == id);
        var entry = await _dbSet!.FindAsync(id);
        if (entry == null)
        {
            throw new DataNotFoundException($"{DtoName} not found.", id);
        }

        var entryRemoved = _dbSet.Remove(entry);
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
    }


}
