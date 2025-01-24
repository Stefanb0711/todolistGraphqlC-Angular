using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;
using todListBackend.Models;
using todoList.Models;
using todoList.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace todListBackend.Services
{
    public class TodoService
    {

        private readonly IMongoCollection<TodolistModel> _todolistCollection;
        private readonly IMongoCollection<TodoModel> _todoCollection;


        public TodoService(MongoDbService mongoDbService) {
            _todolistCollection = mongoDbService.GetCollection<TodolistModel>("todolists");
            _todoCollection = mongoDbService.GetCollection<TodoModel>("todos");
        }


        public async Task<(bool success, string message)> DeleteTodolist(string todolistId)
        {
            try
            {
                var filter = Builders<TodolistModel>.Filter.Eq(t => t.Id, todolistId);
                var result = await _todolistCollection.DeleteOneAsync(filter);

                if (result.DeletedCount > 0)
                {
                    Console.WriteLine("Todolist erfolgreich gelöscht.");
                    return (true, "Todolist erfolgreich gelöscht");
                }
                else
                {
                    Console.WriteLine("Kein Todolist gefunden.");
                    return (false, "Kein Todolist gefunden");
                }
                // Datenbankabfrage für Löschen
                
               

                return (false, "Todolist nicht gefunden oder unbefugt.");
            }
            catch (Exception ex)
            {
                return (false, $"Fehler: {ex.Message}");
            }
        }

        public async Task<(bool success, string message, string todolistId)> DeleteTodo(string todoId)
        {
            try
            {
                
                
                
                var filter = Builders<TodoModel>.Filter.Eq(t => t.Id, todoId);
                
                var todoItem = await _todoCollection.Find(filter).FirstOrDefaultAsync();

                if (todoItem != null)
                {
                    var todolistId = todoItem.TodolistId;
                    var result = await _todoCollection.DeleteOneAsync(filter);

                    if (result.DeletedCount > 0)
                    {
                        Console.WriteLine("Todo erfolgreich gelöscht.");
                        return (true, "Todo erfolgreich gelösch", todolistId);
                    }
                    else
                    {
                        Console.WriteLine("Kein Todo gefunden");
                        return (false, "Kein Todo gefunden", "");
                    }
                }

                return (false, "", "");

            }
            catch (Exception ex)
            {
                return (false, $"Fehler: {ex.Message}", "");
            }
        }

        public async Task<List<TodolistModel>> GetAllTodolists(string currentUserId)
        {
            /*
            var filter = Builders<TodolistModel>.Filter.Eq(todo => todo.UserId, currentUserId);

            var projection = Builders<TodolistModel>.Projection
            .Include(todo => todo.Name)
            .Include(todo => todo.Date)
            .Include(todo => todo.UserId);

            // Projektion auf das Resultat anwenden
            var result = await _todoCollection.Find(filter)
                .Project<TodolistModel>(projection)
                .ToListAsync();

            return result;*/
            /*
            var connectionString = "mongodb://localhost:27017"; // Deine MongoDB-Verbindungszeichenfolge
            var client = new MongoClient(connectionString);

            // Datenbank und Sammlung auswählen
            var database = client.GetDatabase("CSharpTodoList");
            var collection = database.GetCollection<BsonDocument>("users");
            */

            var filter = Builders<TodolistModel>.Filter.Eq("UserId", currentUserId);

            var results = await _todolistCollection.Find(filter).ToListAsync();

            //var jsonResult = JsonConvert.SerializeObject(results, Formatting.Indented);
            /*
            Console.WriteLine("Die aktuellen Todos: " + jsonResult);
            Console.WriteLine("Die ganzen Todos: ");
            foreach (var document in results)
            {
                Console.WriteLine(document.ToJson());
            }*/

            return results;

        }

        public async Task<(bool Success, string Message)> AddTodolist(TodolistModelFromFrontend todoListData)
        {

            
            Console.WriteLine("Todolistdata: ", todoListData);
            
            
            var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(todoListData.Date).DateTime;

            Console.WriteLine("Umgewandletes Fate in AddTodoList" + dateTime);
            
            try
            {
 
                var todoList = new TodolistModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = todoListData.Name,
                    UserId = todoListData.UserId,  
                    Date = todoListData.Date
                };

                await _todolistCollection.InsertOneAsync(todoList);
                

                return (true, "Erfolgeich Todolist hinzugefügt");

            } catch {
                return (false, "Fehler beim hinzufügen der Todlist");
            }
            return (false, "Fehler beim hinzufügen der Todlist");
         

            //return (true, "Erfolgeich Todolist hinzugefügt");
           

        }

        public async Task<(bool Success, string Message)> AddTodo(TodoModel todo)
        {

            try
            {
            var newTodo = new TodoModel
            {
                Id = ObjectId.GenerateNewId().ToString(),

                Content = todo.Content,
                Date = todo.Date,
                TodolistId = todo.TodolistId
            };

            await _todoCollection.InsertOneAsync(newTodo);
            

            return (true, "Erfolgeich Todolist hinzugefügt");

            } catch
            {
                return (false, "Fehler beim hinzufügen der Todos");

            }

            
        }


        public async Task<List<TodoModel>> GetTodos(string todolistId)
        {
            var filter = Builders<TodoModel>.Filter.Eq("TodolistId", todolistId);

            var results = await _todoCollection.Find(filter).ToListAsync();



            return results;
        }




    }
}
