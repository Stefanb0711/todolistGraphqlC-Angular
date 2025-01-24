using System.ComponentModel.DataAnnotations;

public class RegisterModel
{
    [Required(ErrorMessage = "Name ist erforderlich")]
    public string Username { get; set; }

    [EmailAddress(ErrorMessage = "Ungültige E-Mail-Adresse")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Passwort erforderlich")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Passwortbestätigung erforderlich")]
    public string PasswordConfirm { get; set; }
}