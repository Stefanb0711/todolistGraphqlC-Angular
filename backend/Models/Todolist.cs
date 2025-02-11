namespace todListBackend.Models
{
    public class Todolist
    {
        public string? Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }

        public long Date { get; set; }

    }
}
