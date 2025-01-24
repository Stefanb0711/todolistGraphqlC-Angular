using Microsoft.AspNetCore.SignalR;
using System.Text.Json.Serialization;

namespace todListBackend.Models
{
    public class TodolistModelFromFrontend
    {
        //[JsonPropertyName("userId")] // Namespace: System.Text.Json.Serialization
        public string UserId { get; set; }

        //[JsonPropertyName("name")]

        public string Name { get; set; }

        //[JsonPropertyName("date")]

        public long Date {  get; set; }
    }
}
