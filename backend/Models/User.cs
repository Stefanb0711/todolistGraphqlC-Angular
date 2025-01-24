using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using todListBackend.Models;


namespace todoList.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public List<TodolistModel> Todos { get; set; }
    }
}
