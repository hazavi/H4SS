using System.ComponentModel.DataAnnotations;

namespace H4SS.Data
{
    public partial class Todolist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Item { get; set; } = null!;

        // Navigation property for the related Cpr
        public virtual Cpr User { get; set; } = null!;
    }
}
