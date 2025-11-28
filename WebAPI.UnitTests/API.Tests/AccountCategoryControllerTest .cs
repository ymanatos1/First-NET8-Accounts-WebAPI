using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebAPI.Controllers;
using WebAPI.Data.Models;
using WebAPI.Data.Models.Db;
using WebAPI.Data.Services;
using WebAPI.Lib.Data.Services;
using WebAPI.Lib.WebAPI.Query;

namespace WebAPI.UnitTests.API.Tests;

public class AccountCategoryControllerTest
{
    private const string INTERNAL_TEST_FAILURE = "Internal test failure.";

    private AccountCategoriesV1Controller _controller;
    private IAccountCategoryService _service;

    public AccountCategoryControllerTest()
    {
        var options = new DbContextOptionsBuilder<InMemoryAccountsContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;
        IAccountsDbContext db = new InMemoryAccountsContext(options);
        db.Database.EnsureCreated();

        _service = new AccountCategoryService(db);
        _controller = new AccountCategoriesV1Controller(_service, db);
    }


    /********************************************************************************
     * GetAll tests
     ********************************************************************************/
    [Fact]
    public async Task GetAll_TestAsync()
    {
        //Arrange

        //Act
        ActionResult<IEnumerable<AccountCategory>> result = await _controller.Get();

        //Assert
        var actionResult = result!.Result!;
        //assertGetAccountsActionResult(actionResult, 1);
        assertGetAccountsActionResult(actionResult, 6);
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
        ActionResult<IEnumerable<AccountCategory>> result = await _controller.Get(q);

        //Assert
        var actionResult = result!.Result! as OkObjectResult;
        assertGetAccountsActionResult(actionResult!, 1);
    }
    /*[Fact]
    public async Task GetAll_Active_TestAsync()
    {
        //Arrange
        var q = new DtoWithActiveQueryParameters();
        q.IsActive = true;

        //Act
        ActionResult<IEnumerable<AccountCategory>> result = await _controller.Get(q);

        //Assert
        var actionResult = result!.Result! as OkObjectResult;
        assertGetAccountsActionResult(actionResult!, 1);
    }
    [Fact]
    public async Task GetAll_Inactive_TestAsync()
    {
        //Arrange
        var q = new DtoWithActiveQueryParameters();
        q.IsActive = false;

        //Act
        ActionResult<IEnumerable<Account>> result = await _controller.Get(q);

        //Assert
        var actionResult = result!.Result! as OkObjectResult;
        assertGetAccountsActionResult(actionResult!, 0);
    }*/
    [Theory]
    [InlineData("BadName")]
    [InlineData("nimdA")]
    [InlineData("Admin ")]
    public async Task GetAllByBadName_TestAsync(string badName)
    {
        //Arrange
        var q = new DtoQueryParameters();
        q.Name = badName;

        //Act
        ActionResult<IEnumerable<AccountCategory>> result = await _controller.Get(q);

        //Assert
        var actionResult = result!.Result! as OkObjectResult;
        assertGetAccountsActionResult(actionResult!, 0);
    }

    /********************************************************************************
     * GetById tests
     ********************************************************************************/
    [Fact]
    public async Task GetById_TestAsync()
    {
        //Arrange

        //Act
        ActionResult<AccountCategory> result = await _controller.Get(1);

        //Assert
        var actionResult = result!.Result! as OkObjectResult;
        assertGetAccountActionResult(actionResult!, 1);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task GetById_BadId_TestAsync(int badId)
    {
        //Arrange

        //Act
        ActionResult<AccountCategory> result = await _controller.Get(badId);

        //Assert
        assertException<BadRequestObjectResult>(result, HttpStatusCode.BadRequest);
    }
    [Theory]
    [InlineData(7)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task GetById_NotFound_TestAsync(int notfoundId)
    {
        //Arrange

        //Act
        ActionResult<AccountCategory> result = await _controller.Get(notfoundId);

        //Assert
        assertException<NotFoundObjectResult>(result, HttpStatusCode.NotFound);
    }

    /********************************************************************************
     * Post tests
     ********************************************************************************/
    [Theory]
    [InlineData("Test Name 1", "Test Description 1")]
    [InlineData("Test Name 2", "Test Description 2")]
    public async Task Post_TestAsync(string name, string description)
    {
        //Arrange
        var account = new AccountCategory
        {
            Id = 0,
            Name = name,
            Description = description
        };

        //Act
        ActionResult<AccountCategory> result = await _controller.Post(account);

        //Assert
        var actionResult = result!.Result! as CreatedAtActionResult;
        //assertPostAccountsActionResult(actionResult!, account);
        assertPostAccountsActionResult(actionResult!, account, 7);
    }
    [Fact]
    public async Task Post_IncompleteModelState_TestAsync()
    {
        //Arrange
        var account = new AccountCategory
        {
            Id = 0,
            //Name = "name",
            //Description = "description"
        };
        _controller.ModelState.AddModelError("Custom", "Custom ModelState error.");

        //Act
        ActionResult<AccountCategory> result = await _controller.Post(account);

        //Assert
        assertException<BadRequestObjectResult>(result, HttpStatusCode.BadRequest);
    }

    /********************************************************************************
     * Put tests
     ********************************************************************************/
    [Theory]
    [InlineData(1, "Test Name 1", "Test Description 1")]
    [InlineData(0, "Test Name 2", "Test Description 2")]
    public async Task Put_TestAsync(int id, string name, string description)
    {
        //Arrange
        var account = new AccountCategory
        {
            Id = id,
            Name = name,
            Description = description
        };

        //Act
        ActionResult<AccountCategory> result = await _controller.Put(1, account);

        //Assert
        var actionResult = result!.Result! as NoContentResult;
        assertPutAccountsActionResult(actionResult!, account);


        //Act
        ActionResult<AccountCategory> get_result = await _controller.Get(1);

        //Assert
        var get_actionResult = get_result!.Result! as OkObjectResult;
        assertPutGetAccountActionResult(account, get_actionResult!, 1);
    }
    [Fact]
    public async Task Put_IncompleteModelState_TestAsync()
    {
        //Arrange
        var account = new AccountCategory
        {
            Id = 1,
            Name = "new name",
            //Description = "description"
        };
        _controller.ModelState.AddModelError("Custom", "Custom ModelState error.");

        //Act
        ActionResult<AccountCategory> result = await _controller.Put(1, account);

        //Assert
        //var actionResult = result!.Result! as NoContentResult;
        //assertPutAccountsActionResult(actionResult!, account);
        assertException<BadRequestObjectResult>(result, HttpStatusCode.BadRequest);
    }
    [Fact]
    public async Task Put_BadIdNegative_TestAsync()
    {
        //Arrange
        var account = new AccountCategory
        {
            Id = -1,
            Name = "new name",
            Description = "description"
        };

        //Act
        ActionResult<AccountCategory> result = await _controller.Put(-1, account);

        //Assert
        assertException<BadRequestObjectResult>(result, HttpStatusCode.BadRequest);
    }
    [Theory]
    [InlineData("Test Name 1", "Test Description 1")]
    [InlineData("Test Name 2", "Test Description 2")]
    public async Task Put_BadIdDiff_TestAsync(string name, string description)
    {
        //Arrange
        var account = new AccountCategory
        {
            Id = 1,
            Name = name,
            Description = description
        };

        //Act
        ActionResult<AccountCategory> result = await _controller.Put(2, account);

        //Assert
        assertException<BadRequestObjectResult>(result, HttpStatusCode.BadRequest);
    }
    [Fact]
    public async Task Put_NotFound_TestAsync()
    {
        //Arrange
        var account = new AccountCategory
        {
            //Id = 2,
            Id = 7,
            Name = "Test Name",
            Description = "Test Description"
        };

        //Act
        //ActionResult<AccountCategory> result = await _controller.Put(2, account);
        ActionResult<AccountCategory> result = await _controller.Put(7, account);

        //Assert
        assertException<NotFoundObjectResult>(result, HttpStatusCode.NotFound);
    }

    /********************************************************************************
     * Delete tests
     ********************************************************************************/
    [Fact]
    public async Task Delete_TestAsync()
    {
        //Arrange

        //Act
        //var entriesRemoved = await _service.RemoveById(1);
        ActionResult result = await _controller.Delete(1);

        //Assert
        var actionResult = result as OkResult;
        Assert.Equal((int)HttpStatusCode.OK, actionResult!.StatusCode);
    }
    [Fact]
    public async Task Delete_BadId_TestAsync()
    {
        //Arrange

        //Act
        ActionResult<AccountCategory> result = await _controller.Delete(-1);

        //Assert
        assertException<BadRequestObjectResult>(result, HttpStatusCode.BadRequest);
    }
    [Fact]
    public async Task Delete_NotFound_TestAsync()
    {
        //Arrange

        //Act
        ActionResult<AccountCategory> result = await _controller.Delete(7);

        //Assert
        assertException<NotFoundObjectResult>(result, HttpStatusCode.NotFound);
    }




    /********************************************************************************
     * private helper functions
     ********************************************************************************/

    private void assertException<T_error>(ActionResult<AccountCategory> result, HttpStatusCode statusCode)
        where T_error : ObjectResult
        //where T_error : DataException
    {
        var actionResult = result.Result as T_error;
        Assert.IsType<T_error>(actionResult);
        Assert.Equal((int)statusCode, actionResult.StatusCode);
    }

    private void assertPostAccountsActionResult(object objectResult, AccountCategory account, int new_id = 2)
    {
        Assert.IsType<CreatedAtActionResult>(objectResult);
        var actionResult = objectResult as CreatedAtActionResult;

        Assert.Equal((int)HttpStatusCode.Created, actionResult!.StatusCode);

        var new_account = actionResult!.Value as AccountCategory;
        //assertAccountProperties(account, new_account!);
        assertAccountProperties(account, new_account!, new_id);
    }
    private void assertPutAccountsActionResult(object objectResult, AccountCategory account)
    {
        Assert.IsType<NoContentResult>(objectResult);
        var actionResult = objectResult as NoContentResult;

        Assert.Equal((int)HttpStatusCode.NoContent, actionResult!.StatusCode);
    }

    private void assertGetAccountsActionResult(object objectResult, int expectedAcounts)
    {
        Assert.IsType<OkObjectResult>(objectResult);
        var actionResult = (OkObjectResult)objectResult;

        Assert.Equal((int)HttpStatusCode.OK, actionResult!.StatusCode);

        var accounts = actionResult!.Value! as IEnumerable<AccountCategory>;
        assertAccountsEmptyOrSingleAdmin(accounts!, expectedAcounts);
    }
    private void assertGetAccountActionResult(OkObjectResult objectResult, int expectedAcounts)
    {
        Assert.IsType<OkObjectResult>(objectResult);
        OkObjectResult actionResult = objectResult as OkObjectResult;

        Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);

        var account = actionResult!.Value! as AccountCategory ;
        assertAccountAdminProperties(account!);
    }
    private void assertPutGetAccountActionResult(AccountCategory put_account, OkObjectResult objectResult, int expectedAcounts)
    {
        Assert.IsType<OkObjectResult>(objectResult);
        OkObjectResult actionResult = objectResult as OkObjectResult;

        Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);

        var account = actionResult!.Value! as AccountCategory;
        assertAccountProperties(account!, put_account, 1);
    }

    private void assertAccountsEmptyOrSingleAdmin(IEnumerable<AccountCategory> accounts, int expectedAcounts)
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
