namespace Todo.UnitTests.Controllers;

public class AccountControllerTests
{
    private IAccountService _accountService;
    private ILogger<AccountController> _logger;
    private AccountController _accountController;

    public AccountControllerTests()
    {
        _accountService = A.Fake<IAccountService>();
        _logger = A.Fake<ILogger<AccountController>>();
        _accountController = new AccountController(_accountService, _logger);
    }
}