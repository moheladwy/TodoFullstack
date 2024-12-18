namespace Todo.UnitTests.Controllers;

public class AuthControllerTests
{
  private readonly IAuthService _authenticationService;
  private readonly ILogger<AuthController> _logger;
  private readonly AuthController _authController;

  public AuthControllerTests()
  {
    _authenticationService = A.Fake<IAuthService>();
    _logger = A.Fake<ILogger<AuthController>>();
    _authController = new AuthController(_authenticationService, _logger);
  }

  [Fact]
  public async Task Login_WithValidModel_ReturnsOk()
  {
    // Arrange
    var loginUserDto = new LoginUserDto
    {
      Email = "mohamed.h.eladwy@gmail.com",
      Password = "Al-Adawy@123"
    };
    A.CallTo(() => _authenticationService.Login(loginUserDto)).Returns(new AuthResponse());

    // Act
    var result = await _authController.Login(loginUserDto);

    // Assert
    Assert.NotNull(result);
    Assert.IsType<OkObjectResult>(result);
  }

  [Fact]
  public async Task Login_WithInvalidModel_ReturnsBadRequest()
  {
    // Arrange
    var loginUserDto = new LoginUserDto
    {
      Email = "invalid-email",
      Password = "short"
    };

    _authController.ModelState.AddModelError("Email", "Invalid email format");
    _authController.ModelState.AddModelError("Password", "Password doesn't meet requirements");

    // Act & Assert
    await Assert.ThrowsAsync<InvalidModelStateException>(() => _authController.Login(loginUserDto));
  }

  [Fact]
  public async Task Register_WithValidModel_ReturnsOk()
  {
    // Arrange
    var registerUserDto = new RegisterUserDto
    {
      Email = "mohamed.h.eladwy@gmail.com",
      Password = "Al-Adawy@123",
      Username = "mohamed.h.eladwy",
      FirstName = "Mohamed",
      LastName = "Al-Adawy"
    };
    A.CallTo(() => _authenticationService.Register(registerUserDto)).Returns(new AuthResponse());

    // Act
    var result = await _authController.Register(registerUserDto);

    // Assert
    Assert.NotNull(result);
    Assert.IsType<OkObjectResult>(result);
  }

  [Fact]
  public async Task Register_WithInvalidModel_ReturnsBadRequest()
  {
    // Arrange
    var registerUserDto = new RegisterUserDto
    {
      Email = "invalid-email",
      Password = "short",
      Username = "short",
      FirstName = "short",
      LastName = "short"
    };

    _authController.ModelState.AddModelError("Email", "Invalid email format");
    _authController.ModelState.AddModelError("Password", "Password doesn't meet requirements");
    _authController.ModelState.AddModelError("Username", "Username doesn't meet requirements");
    _authController.ModelState.AddModelError("FirstName", "First name doesn't meet requirements");
    _authController.ModelState.AddModelError("LastName", "Last name doesn't meet requirements");

    // Act & Assert
    await Assert.ThrowsAsync<InvalidModelStateException>(() => _authController.Register(registerUserDto));
  }

  [Fact]
  public async Task LoginWithRefreshToken_WithValidModel_ReturnsOk()
  {
    // Arrange
    string refreshToken = "valid-refresh-token";
    A.CallTo(() => _authenticationService.LoginWithRefreshToken(refreshToken)).Returns(new AuthResponse());

    // Act
    var result = await _authController.LoginWithRefreshToken(refreshToken);

    // Assert
    Assert.NotNull(result);
    Assert.IsType<OkObjectResult>(result);
  }

  [Fact]
  public async Task LoginWithRefreshToken_WithInvalidModel_ReturnsBadRequest()
  {
    // Arrange
    string refreshToken = "null";

    _authController.ModelState.AddModelError("RefreshToken", "Invalid refresh token");

    // Act & Assert
    await Assert.ThrowsAsync<InvalidModelStateException>(() => _authController.LoginWithRefreshToken(refreshToken));
  }

  [Fact]
  public async Task Logout_WithValidModel_ReturnsOk()
  {
    // Arrange
    string username = "mohamed.h.eladwy";
    A.CallTo(() => _authenticationService.Logout(username)).Returns(Task.CompletedTask);

    // Act
    var result = await _authController.Logout(username);

    // Assert
    Assert.NotNull(result);
    Assert.IsType<OkResult>(result);
  }

  [Fact]
  public async Task Logout_WithInvalidModel_ReturnsBadRequest()
  {
    // Arrange
    string username = "";

    _authController.ModelState.AddModelError("Username", "Username is required");

    // Act & Assert
    await Assert.ThrowsAsync<InvalidModelStateException>(() => _authController.Logout(username));
  }
}