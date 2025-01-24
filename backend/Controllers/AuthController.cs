using Microsoft.AspNetCore.Mvc;
using todListBackend.Models;

[ApiController]
[Route("api/auth/")]
public class AuthController : ControllerBase
{
	private readonly AuthService _authService;
	private readonly JwtTokenService _jwtService;

	public AuthController(AuthService authService, JwtTokenService jwtTokenService)
	{
		_authService = authService;
		_jwtService = jwtTokenService;

	}


	[HttpGet("get-users")]
	public async Task<IActionResult> GetUsers()
	{
		return Ok(new { Message = "Im Getusersroute" });
	}


    [HttpPost("get-userid")]
	public async Task<IActionResult> GetUserId([FromBody] TokenRequest request)
	{

		Console.WriteLine("Der Token in GetUseIdRoute" + request);
		try
		{
			var currentUserId = _jwtService.GetUserIdFromJwt(request.Token);
			
			Console.WriteLine("CurrentUserId: " + currentUserId);	


			return Ok(new {CurrentUserId = currentUserId });
		}
		catch (Exception ex) { 


			return BadRequest(ex.Message);
		}



	}

    [HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginModel loginData)
	{
        var (success, message, token) = await _authService.Login(loginData);


		//var result = new { success= true, message= "Erfolgreich registriert", token= "msdklwmdldw"};

		Console.WriteLine("Erstellter Token: " + token);
		Console.WriteLine("Erstelle Message: " + message);



		
		if (success)
		{
			return Ok(new {Message = message, Token = token});
		}

		return BadRequest(new { Message = message });
		
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterModel registrationData)
	{

        
		var (success, message) = await _authService.Register(registrationData);

		if (success)
		{
			return Ok(new { Message = message });
		}

		return BadRequest(new { Message = message });

    }



}