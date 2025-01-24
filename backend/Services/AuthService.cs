using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using todoList.Models;
using todoList.Services;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Bson;


/*
public interface IAuthService
{
	Task<IActionResult> Login(LoginModel loginData);

	Task<IActionResult> Register(RegisterModel registerData);

}
*/

public class AuthService
{

	private readonly IMongoCollection<User> _users;
	private readonly JwtTokenService _jwtTokenService;
	private readonly AuthService _authService;
	
	public AuthService(MongoDbService mongoDbService,
		JwtTokenService jwtTokenService
		)
	{
		_users = mongoDbService.GetCollection<User>("users");
		_jwtTokenService = jwtTokenService;
	}

	public async Task<(bool Success, string Message, string Token)> Login(LoginModel loginData)
	{
		
		try
		{
			//Filtern ob Benutzername oder Passwort existiert 
			var filter = Builders<User>.Filter.Or(
				Builders<User>.Filter.Eq(user => user.Username, loginData.UsernameOrEmail),
				Builders<User>.Filter.Eq(user => user.Email, loginData.UsernameOrEmail)
            );

			var user = await _users.Find(filter).FirstOrDefaultAsync();

		

			//Falls der Benutzername existiert
			if (user != null)
			{
				if (!string.IsNullOrEmpty(loginData.Password))
				{
					var passwordHasher = new PasswordHasher<User>();
					var result = passwordHasher.VerifyHashedPassword(
						user, user.Password, loginData.Password);

					Console.WriteLine($"Result of VerifyHashedPassword: {result}");

					if (result == PasswordVerificationResult.Success)
					{

						Console.WriteLine("Sie sind erfolgreich eingeloggt");


						var token = _jwtTokenService.GenerateToken(user.Id, user.Username, loginData.Password);



						return (true, "Login erfolgreich", token);

						//return RedirectToPage("/Index");

					}
					else
					{
						Console.WriteLine("Das eingegebene Passwort ist falsch");
						//ViewData["ErrorMessage"] = "Das eingegebene Passwort ist falsch";

						return (false, "Das eingegebene Passwort ist falsch", null);


					}

				}
				else
				{
					Console.WriteLine("Das Passwortfeld ist nicht ausgefüllt.");
                    //ViewData["ErrorMessage"] = "Das Passwortfeld ist nicht ausgefüllt.";
                    return (false, "Das Passwortfeld ist nicht ausgefüllt.", null);

                }

            }
			//Falls der Benutzername nicht existiert
			else
			{
				Console.WriteLine("Benutzer konnte nicht gefunden werden");
				//ViewData["ErrorMessage"] = "Benutzer konnte nicht gefunden werden";
				return (false, "Benutzer konnte nicht gefunden werden", null);
			}

		}
		catch (Exception ex)
		{
            //ViewData["ErrorMessage"] = "Eín interner Fehler ist aufgetreten. Probieren Sie es nochmal";


			return (false, "Ein interner Fehler ist aufgetreten. Probieren Sie es nochmal", null);

        }
		

		return (true, "Erfolgreicher Login", null);
    }
	
	public async Task<(bool Success, string Message)> Register(RegisterModel registrationData)
	{
		
        if (registrationData.Password != registrationData.PasswordConfirm)
        {
			/*
            ViewData["ErrorMessage"] = "Passwords do not match";
            return Page();*/
			return (false, "Passwords do not match");
        }

        try
        {
            /*
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("CSharpTodoList");
            var collection = database.GetCollection<User>("users");
            */


            //Checken ob Email und Benutzername schon vergeben sind

            var filter = Builders<User>.Filter.Or(
                Builders<User>.Filter.Eq(u => u.Email, registrationData.Email),
                Builders<User>.Filter.Eq(u => u.Username, registrationData.Username)
                );


            var count = await _users.CountDocumentsAsync(filter);

            if (count > 0)
            {
                return (false, "Email oder Benutzername ist schon vergeben");

                /*
                ViewData["ErrorMessage"] = "Email oder Benutzername ist schon vergeben";
                return Page();*/
            }

            var passwordHasher = new PasswordHasher<object>();



            string hashedPassword = passwordHasher.HashPassword(null, registrationData.Password);

            Console.WriteLine(hashedPassword);


            var newUser = new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Username = registrationData.Username,
                Email = registrationData.Email,
                Password = hashedPassword

            };

            _users.InsertOne(newUser);


			return (true, "Erfolgreich registriert");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return (false, ex.Message);

        }

    }



}