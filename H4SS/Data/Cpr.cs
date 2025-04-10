using System.ComponentModel.DataAnnotations;

namespace H4SS.Data
{
    public partial class Cpr
    {
        public int Id { get; set; }
        public string User { get; set; } = null!;
        public string CprNr { get; set; } = null!;
        public virtual ICollection<Todolist> Todolists { get; set; } = new List<Todolist>();

    }
}
