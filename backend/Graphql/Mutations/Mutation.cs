using todListBackend.Graphql.Types;
using MongoDB.Driver;
using todoList.Services;
using todoList.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace todListBackend.Graphql.Mutations;

public class Mutation
{
    
    private readonly IMongoCollection<User> _users;
    private readonly JwtTokenService _jwtTokenService;
	
    public Mutation(MongoDbService mongoDbService,
        JwtTokenService jwtTokenService
    )
    {
        _users = mongoDbService.GetCollection<User>("users");
        _jwtTokenService = jwtTokenService;
    }
    
    public Book AddBook(string title, string authorName)
    {
	    return new Book
	    {
		    Title = title,
		    Author = new Author
		    {
			    Name = authorName
		    }
	    };
    }
    
    public async Task<Response> RegisterUser(RegisterInput registrationData)
    {
	    
	    Console.WriteLine("In der Registerfunktion");
	    
        if (registrationData.Password != registrationData.PasswordConfirm)
        {
	        
	        return new Response { Success = false, Message = "Passwords do not match" };
	        
            /*
            ViewData["ErrorMessage"] = "Passwords do not match";
            return Page();*/
            //return new Response {false, "Passwords do not match" };
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
		        return new Response
		        {
			        Success = false,
			        Message = "Email oder Benutzername ist schon vergeben"
		        };

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


	        return new Response {Success = true, Message = "Erfolgreich registriert" };
	        
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response {Success = false, Message = ex.Message};
			
        }

    }
    
    public async Task<LoginResponse> LoginUser(LoginInput loginData)
    {
	    if (loginData == null)
	    {
		    Console.WriteLine("loginData ist NULL!");
		    return new LoginResponse
		    {
			    Success = false,
			    Message = "Die Eingabe-Daten sind leer oder fehlen.",
			    Token = null
		    };
	    }
	    else
	    {
		    Console.WriteLine($"LoginRequest erhalten: UsernameOrEmail={loginData.UsernameOrEmail}, Password={loginData.Password}");
	    }
	    
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


						return new LoginResponse{Success = true, Message = "Login erfolgreich", Token = token};
						
						//return (true, "Login erfolgreich", token);

						//return RedirectToPage("/Index");

					}
					else
					{
						Console.WriteLine("Das eingegebene Passwort ist falsch");
						//ViewData["ErrorMessage"] = "Das eingegebene Passwort ist falsch";

						return new LoginResponse{Success = false, Message = "Das eingegebene Passwort ist falsch", Token = null};
						//return (false, "Das eingegebene Passwort ist falsch", null);
						
					}

				}
				else
				{
					Console.WriteLine("Das Passwortfeld ist nicht ausgef�llt.");
					return new LoginResponse {Success = false, Message = "Das Passwortfeld ist nicht ausgef�llt.", Token = null};
					
                }

            }
			//Falls der Benutzername nicht existiert
			else
			{
				Console.WriteLine("Benutzer konnte nicht gefunden werden");
				//ViewData["ErrorMessage"] = "Benutzer konnte nicht gefunden werden";
				return new LoginResponse {Success = false, Message = "Benutzer konnte nicht gefunden werden", Token = null};
			}

		}
		catch (Exception ex)
		{
            //ViewData["ErrorMessage"] = "E�n interner Fehler ist aufgetreten. Probieren Sie es nochmal";


			return new LoginResponse {Success = false, Message = "Ein interner Fehler ist aufgetreten. Probieren Sie es nochmal", Token = null};

        }
        
		return new LoginResponse {Success = true, Message = "Erfolgreicher Login", Token = null};
    }
    
    
}