namespace H4SS.Data
{
    public class TodoCprEntry
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string HashedCpr { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
