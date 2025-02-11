using todListBackend.Graphql.Types;
using MongoDB.Driver;
using todoList.Services;
using todoList.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using todListBackend.Models;
using todListBackend.Services;

namespace todListBackend.Graphql.Mutations;

public class Mutation
{


	private readonly TodoService _todoService;
    
    private readonly IMongoCollection<User> _users;
    private readonly JwtTokenService _jwtTokenService;
    public Mutation(MongoDbService mongoDbService,
        JwtTokenService jwtTokenService,
        TodoService todoService
    )
    {
        _users = mongoDbService.GetCollection<User>("users");
        _jwtTokenService = jwtTokenService;
        _todoService = todoService;
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
    
    /*Beispielmutation zum Testen:
     mutation {
	  registerUser(registrationData: {
	    username: "testuser",
	    email: "test@example.com",
	    password: "testpassword",
	    passwordConfirm: "testpassword"
	  }) {
	    success
	    message
	  }
	}
     */

	
    public async Task<ResponseType> DeleteTodolist(string todolistId)
    {
	    return await _todoService.DeleteTodolist(todolistId);
    }


    public async Task<ResponseTodolistId> DeleteTodo(string todoId)
    {
	    
	    return await _todoService.DeleteTodo(todoId);
	    
	    
    }

    public async Task<List<TodolistType>> GetAllTodolists(string currentUserId)
    {
	    
	    List<Todolist> todolists = await _todoService.GetAllTodolists(currentUserId);
	    List<TodolistType> todolistTypes = new List<TodolistType>();

	    foreach (var todolist in todolists)
	    {
		    todolistTypes.Add(new TodolistType
		    {
			    Id = todolist.Id,
			    UserId = todolist.UserId,
			    Name = todolist.Name,
			    Date = todolist.Date
		    });
	    }

	    return todolistTypes;
	    
		
    }
	
    public async Task<ResponseType> AddTodolist(AddTodolistInput input)
    {
	    var todoListData = new Todolist
	    {
		    Id = input.Id,
		    UserId = input.UserId,
		    Name = input.Name,
		    Date = input.Date
	    };
	    
	    var response = await _todoService.AddTodolist(todoListData);
	    
	    return new ResponseType
	    {
		    Success = response.Success,
		    Message = response.Message
	    };
	    
    }

    public async Task<ResponseType> AddTodo(AddTodoInput todo)
    {
	    var todoData = new TodoModel
	    {
		    Id = todo.Id,
		    Content = todo.Content,
		    Date = todo.Date,
		    TodolistId = todo.TodolistId
	    };
	    
	    
	    
	    var response = await _todoService.AddTodo(todoData);

	    return new ResponseType
	    {
		    Success = response.Success,
		    Message = response.Message
	    };
    }


    public async Task<List<TodoType>> GetTodos(string todolistId)
    {
	    List<TodoModel> todos = await _todoService.GetTodos(todolistId);
	    List<TodoType> todoType = new List<TodoType>();

	    foreach (var todo in todos)
	    {
		    todoType.Add(new TodoType
		    {
			    Id = todo.Id,
			    Content = todo.Content,
			    Date = todo.Content,
			    TodolistId = todo.TodolistId
		    });
	    }
		
	    return todoType;
    }
    
    public async Task<ResponseType> RegisterUser(RegisterInput registrationData)
    {
	    
	    Console.WriteLine("In der Registerfunktion");
	    
        if (registrationData.Password != registrationData.PasswordConfirm)
        {
	        
	        return new ResponseType { Success = false, Message = "Passwords do not match" };
	        
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
		        return new ResponseType
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


	        return new ResponseType {Success = true, Message = "Erfolgreich registriert" };
	        
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ResponseType {Success = false, Message = ex.Message};
			
        }

    }
    
    /*
    mutation {
	    loginUser(loginData: {
		    usernameOrEmail: "testuser",
		    password: "testpassword"
	    }) {
		    success
			    message
		    token
	    }
    }*/
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