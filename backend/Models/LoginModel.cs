using System.ComponentModel.DataAnnotations;

public class LoginModel
{
	[Required(ErrorMessage = "Benutzername oder Email ist ewrfordderlich")]
	public string UsernameOrEmail { get; set; }

	[Required(ErrorMessage = "Geben Sie ein Passwort ein")]
	public string Password { get; set; }
}