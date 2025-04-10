using System.ComponentModel.DataAnnotations;

namespace H4SS.Data
{
    public class Todolist
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Item { get; set; } = string.Empty;

        // Navigation property for the related Cpr
        public Cpr Cpr { get; set; } = null!;
    }
}
