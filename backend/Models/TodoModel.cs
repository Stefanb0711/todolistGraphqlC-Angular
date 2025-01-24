namespace todListBackend.Models
{
    public class TodoModel
    {
        public string? Id { get; set; }
        public string Content { get; set; }

        public string TodolistId { get; set; }

        public long Date { get; set; }
    }
}
