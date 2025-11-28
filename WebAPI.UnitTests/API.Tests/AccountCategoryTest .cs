using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data.Models;
using WebAPI.Data.Models.Db;
using WebAPI.Data.Services;
using WebAPI.Exceptions;
using WebAPI.Lib.Data.Services;
using WebAPI.Lib.WebAPI.Query;

namespace WebAPI.UnitTests.API.Tests;

public class AccountCategoryTest
{
    private const string INTERNAL_TEST_FAILURE = "Internal test failure.";

    private IAccountCategoryService _service;

    public AccountCategoryTest()
    {
        var options = new DbContextOptionsBuilder<InMemoryAccountsContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;
        IAccountsDbContext db = new InMemoryAccountsContext(options);
        db.Database.EnsureCreated();

        _service = new AccountCategoryService(db);
    }


    /********************************************************************************
     * GetAll tests
     ********************************************************************************/
    [Fact]
    public async Task GetAll_TestAsync()
    {
        //Arrange

        //Act
        var accounts = await _service.Get();

        //Assert
        //assertGetAccountsEmptyOrSingleAdmin(accounts, 1);
        assertGetAccountsEmptyOrSingleAdmin(accounts, 6);
    }

    [Theory]
    [InlineData("System")]
    //[InlineData("Account")]
    [InlineData("System Account")]
    public async Task GetAll_ByName_TestAsync(string name)
    {
        //Arrange
        var q = new DtoQueryParameters();
        q.Name = name;

        //Act
        var accounts = await _service.Get(q);

        //Assert
        assertGetAccountsEmptyOrSingleAdmin(accounts, 1);
    }
    /*[Fact]
    public async Task GetAll_Active_TestAsync()
    {
        //Arrange
        var q = new DtoQueryParameters();
        q.IsActive = true;

        //Act
        var accounts = await _service.Get(q);

        //Assert
        assertGetAccountsEmptyOrSingleAdmin(accounts, 1);
    }
    [Fact]
    public async Task GetAll_Inactive_TestAsync()
    {
        //Arrange
        var q = new DtoWithActiveQueryParameters();
        q.IsActive = false;

        //Act
        var accounts = await _service.Get(q);

        //Assert
        assertGetAccountsEmptyOrSingleAdmin(accounts, 0);
    }*/
    [Theory]
    [InlineData("BadName")]
    [InlineData("nimdA")]
    [InlineData("Admin ")]
    public async Task GetAll_ByBadName_TestAsync(string badName)
    {
        //Arrange
        var q = new DtoQueryParameters();
        q.Name = badName;

        //Act
        var accounts = await _service.Get(q);

        //Assert
        assertGetAccountsEmptyOrSingleAdmin(accounts, 0);
    }

    /********************************************************************************
     * GetById tests
     ********************************************************************************/
    [Fact]
    public async Task GetById_TestAsync()
    {
        //Arrange

        //Act
        var account = await _service.GetById(1);

        //Assert
        assertAccountAdminProperties(account);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task GetById_BadId_TestAsync(int badId)
    {
        //Arrange

        //Act
        var ex = await Assert.ThrowsAsync<BadIdException>(async () =>
        {
            var account = await _service.GetById(badId);
        });

        //Assert
        Assert.Equal("Id must be greater than 0.", ex.Message); // Optional
    }
    [Theory]
    [InlineData(7)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task GetById_NotFound_TestAsync(int notfoundId)
    {
        //Arrange

        //Act
        var ex = await Assert.ThrowsAsync<DataNotFoundException>(async () =>
        {
            var account = await _service.GetById(notfoundId);
        });

        //Assert
        Assert.Equal($"{_service.DtoName} not found.", ex.Message); // Optional
    }

    /********************************************************************************
     * Add tests
     ********************************************************************************/
    [Theory]
    [InlineData("Test Name 1", "Test Description 1")]
    [InlineData("Test Name 2", "Test Description 2")]
    public async Task Add_TestAsync(string name, string description)
    {
        //Arrange
        var account = new AccountCategory
        {
            Id = 0,
            Name = name,
            Description = description
        };
        var modelState = new ModelStateDictionary();

        //Act
        var entriesWritten = await _service.Add(modelState.IsValid, account);

        //Assert
        Assert.Equal(1, entriesWritten);
        //assertAccountProperties(account, account);
        assertAccountProperties(account, account, 7);
    }
    [Fact]
    public async Task Add_IncompleteModelState_TestAsync()
    {
        //Arrange
        var account = new AccountCategory
        {
            Id = 0,
            //Name = "name",
            //Description = "description"
        };
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Custom", "Custom error.");

        //Act
        var ex = await Assert.ThrowsAsync<ModelStateInvalidException<AccountCategory>>(async () =>
        {
            var entriesWritten = await _service.Add(modelState.IsValid, account);
        });

        //Assert
        Assert.Equal("ModelState is invalid.", ex.Message); // Optional
    }

    /********************************************************************************
     * Update tests
     ********************************************************************************/
    [Theory]
    [InlineData(1, "Test Name 1", "Test Description 1")]
    [InlineData(0, "Test Name 2", "Test Description 2")]
    public async Task Update_TestAsync(int id, string name, string description)
    {
        //Arrange
        var account = new AccountCategory
        {
            Id = id,
            Name = name,
            Description = description
        };
        var modelState = new ModelStateDictionary();

        //Act
        var entriesWritten = await _service.Update(modelState.IsValid, 1, account);

        //Assert
        Assert.Equal(1, entriesWritten);


        //Act
        var get_account = await _service.GetById(1);

        //Assert
        assertAccountProperties(account, get_account, 1);
    }
    [Fact]
    public async Task Update_IncompleteModelState_TestAsync()
    {
        //Arrange
        var account = new AccountCategory
        {
            Id = 1,
            Name = "new name",
            //Description = "description"
        };
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Custom", "Custom ModelState error.");

        //Act
        var ex = await Assert.ThrowsAsync<ModelStateInvalidException<AccountCategory>>(async () =>
        {
            var entriesWritten = await _service.Update(modelState.IsValid, 1, account);
        });

        //Assert
        Assert.Equal("ModelState is invalid.", ex.Message); // Optional
        //Assert.Equal(1, entriesWritten);


        //Act
        //var get_account = await _service.GetById(1);

        //Assert
        //assertAccountProperties(account, get_account, 1);
    }
    [Fact]
    public async Task Update_BadIdNegative_TestAsync()
    {
        //Arrange
        var account = new AccountCategory
        {
            Id = 1,
            Name = "new name",
            Description = "description"
        };
        var modelState = new ModelStateDictionary();

        //Act
        var ex = await Assert.ThrowsAsync<BadIdException>(async () =>
        {
            var entriesWritten = await _service.Update(modelState.IsValid, -1, account);
        });

        //Assert
        Assert.Equal("Id must be greater than 0.", ex.Message); // Optional
    }
    [Theory]
    [InlineData("Test Name 1", "Test Description 1")]
    [InlineData("Test Name 2", "Test Description 2")]
    public async Task Update_BadIdDiff_TestAsync(string name, string description)
    {
        //Arrange
        var account = new AccountCategory
        {
            Id = 1,
            Name = name,
            Description = description
        };
        var modelState = new ModelStateDictionary();

        //Act
        var ex = await Assert.ThrowsAsync<BadIdException>(async () =>
        {
            var entriesWritten = await _service.Update(modelState.IsValid, 2, account);
        });

        //Assert
        Assert.Equal("Cannot update entry with a different id.", ex.Message); // Optional
    }
    [Fact]
    public async Task Update_NotFound_TestAsync()
    {
        //Arrange
        var account = new AccountCategory
        {
            //Id = 2,
            Id = 7,
            Name = "Test Name",
            Description = "Test Description"
        };
        var modelState = new ModelStateDictionary();

        //Act
        var ex = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
        {
            //var entriesWritten = await _service.Update(modelState.IsValid, 2, account);
            var entriesWritten = await _service.Update(modelState.IsValid, 7, account);
        });

        //Assert
        Assert.Equal("Attempted to update or delete an entity that does not exist in the store.", ex.Message); // Optional
    }

    /********************************************************************************
     * Remove tests
     ********************************************************************************/
    [Fact]
    public async Task RemoveById_TestAsync()
    {
        //Arrange

        //Act
        var entriesRemoved = await _service.RemoveById(1);

        //Assert
        Assert.Equal(1, entriesRemoved);
        //Assert.Empty(await _service.Get());
        Assert.Equal(6 - 1, (await _service.Get()).Count());
    }
    [Fact]
    public async Task RemoveById_BadId_TestAsync()
    {
        //Arrange

        //Act
        int entriesRemoved = 0;
        var ex = await Assert.ThrowsAsync<BadIdException>(async () =>
        {
            entriesRemoved = await _service.RemoveById(-1);
        });

        //Assert
        Assert.Equal(0, entriesRemoved);
        Assert.NotEmpty(await _service.Get());
    }
    [Fact]
    public async Task RemoveById_NotFound_TestAsync()
    {
        //Arrange

        //Act
        int entriesRemoved = 0;
        var ex = await Assert.ThrowsAsync<DataNotFoundException>(async () =>
        {
            //entriesRemoved = await _service.RemoveById(2);
            entriesRemoved = await _service.RemoveById(6 + 1);
        });

        //Assert
        Assert.Equal(0, entriesRemoved);
        Assert.NotEmpty(await _service.Get());
    }




    /********************************************************************************
     * private helper functions
     ********************************************************************************/

    private void assertGetAccountsEmptyOrSingleAdmin(IEnumerable<AccountCategory> accounts, int expectedAcounts)
    {
        switch (expectedAcounts)
        {
            case 0:
                Assert.Empty(accounts);
                break;
            case 1:
                Assert.Single(accounts);

                var account = accounts.First();
                assertAccountAdminProperties(account);
                break;
            case > 1:
                Assert.Equal(expectedAcounts, accounts.Count());
                assertAccountAdminProperties(accounts.First());
                break;
            default:
                Assert.Fail(INTERNAL_TEST_FAILURE);
                break;
        }
    }
    private void assertAccountAdminProperties(AccountCategory account)
    {
        Assert.Equal(1, account.Id);
        //Assert.Equal("Admin", account.Name);
        Assert.Equal("System Account", account.Name);
        //Assert.True(account.IsActive);
    }
    private void assertAccountProperties(AccountCategory account, AccountCategory new_account, int new_id = 2)
    {
        Assert.Equal(new_id, new_account!.Id);
        Assert.Equal(account.Name, new_account!.Name);
        Assert.Equal(account.Description, new_account!.Description);
        //Assert.Equal(account.IsActive, new_account!.IsActive);
        //Assert.Equal(account.CategoryId, new_account!.CategoryId);
    }
}
