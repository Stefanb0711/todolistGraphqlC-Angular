using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;
using todListBackend.Models;
using todoList.Models;
using todoList.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using todListBackend.Graphql.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Todolist = todListBackend.Models.Todolist;

namespace todListBackend.Services
{
    public class TodoService
    {

        private readonly IMongoCollection<Todolist> _todolistCollection;
        private readonly IMongoCollection<TodoModel> _todoCollection;


        public TodoService(MongoDbService mongoDbService) {
            _todolistCollection = mongoDbService.GetCollection<Todolist>("todolists");
            _todoCollection = mongoDbService.GetCollection<TodoModel>("todos");
        }


        public async Task<ResponseType> DeleteTodolist(string todolistId)
        {
            try
            {
                var filter = Builders<Todolist>.Filter.Eq(t => t.Id, todolistId);
                var result = await _todolistCollection.DeleteOneAsync(filter);

                if (result.DeletedCount > 0)
                {
                    Console.WriteLine("Todolist erfolgreich gelöscht.");
                    return new ResponseType
                    {
                        Success = true,
                        Message = "Todolist erfolgreich gelöscht"
                    };
                    
                    //return (true, "Todolist erfolgreich gelöscht");
                }
                else
                {
                    Console.WriteLine("Kein Todolist gefunden.");
                    return new ResponseType
                    {
                        Success = false,
                        Message = "Keine Todolist gefunden"
                    };
                    //return (false, "Keine Todolist gefunden");
                }
                // Datenbankabfrage für Löschen


                return new ResponseType
                {
                    Success = false,
                    Message = "Todolist nicht gefunden oder unbefugt"
                };
                
                //return (false, "Todolist nicht gefunden oder unbefugt.");
            }
            catch (Exception ex)
            {
                return new ResponseType
                {
                    Success = false,
                    Message = $"Fehler: {ex.Message}"
                };
                //return (false, $"Fehler: {ex.Message}");
            }
        }

        public async Task<ResponseTodolistId> DeleteTodo(string todoId)
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

                        return new ResponseTodolistId
                        {
                            Success = true,
                            Message = "Todo erfolgreich gelöscht",
                            TodolistId = todolistId
                        };
                        //return (true, "Todo erfolgreich gelösch", todolistId);
                    }
                    else
                    {
                        Console.WriteLine("Kein Todo gefunden");

                        return new ResponseTodolistId
                        {
                            Success = false,
                            Message = "Kein Todo gefunden",
                            TodolistId = null
                        };
                        
                        //return (false, "Kein Todo gefunden", "");
                    }
                }

                return new ResponseTodolistId
                {
                    Success = false,
                    Message = "",
                    TodolistId = ""
                };
                //return (false, "", "");

            }
            catch (Exception ex)
            {
                return new ResponseTodolistId
                {
                    Success = false,
                    Message = $"Fehler: {ex.Message}",
                    TodolistId = ""
                };
                //return (false, $"Fehler: {ex.Message}", "");
            }
        }

        public async Task<List<Todolist>> GetAllTodolists(string currentUserId)
        {

            var filter = Builders<Todolist>.Filter.Eq("UserId", currentUserId);

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

        public async Task<ResponseType> AddTodolist(Todolist todoListData)
        {
            Console.WriteLine("Todolistdata: ", todoListData);
            
            
            var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(todoListData.Date).DateTime;

            Console.WriteLine("Umgewandletes Fate in AddTodoList" + dateTime);
            
            try
            {
 
                var todoList = new Todolist
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = todoListData.Name,
                    UserId = todoListData.UserId,  
                    Date = todoListData.Date
                };

                await _todolistCollection.InsertOneAsync(todoList);

                return new ResponseType
                {
                    Message = "Erfolgeich Todolist hinzugefügt",
                    Success = true
                };
                //return (true, "Erfolgeich Todolist hinzugefügt");

            } catch
            {
                return new ResponseType
                {
                    Success = false,
                    Message = "Fehler beim hinzufügen der Todlist"
                };
                //return (false, "Fehler beim hinzufügen der Todlist");
            }

            return new ResponseType
            {
                Success = false,
                Message = "Fehler beim hinzufügen der Todlist"
            };
            //return (false, "Fehler beim hinzufügen der Todlist");


            //return (true, "Erfolgeich Todolist hinzugefügt");


        }

        public async Task<ResponseType> AddTodo(TodoModel todo)
        {

            try
            {
            /*
            var newTodo = new TodoModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Content = todo.Content,
                Date = todo.Date,
                TodolistId = todo.TodolistId
            };*/

                await _todoCollection.InsertOneAsync(todo);

                return new ResponseType
                {
                    Success = true,
                    Message = "Erfolgeich Todolist hinzugefügt"
                };
            
            //return (true, "Erfolgeich Todolist hinzugefügt");

            } catch
            {
                return new ResponseType
                {
                    Success = false,
                    Message = "Fehler beim hinzufügen der Todos"
                };

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
