using System.ComponentModel.DataAnnotations;

namespace H4SS.Data
{
    public class Cpr
    {
        [Key]
        public int Id { get; set; }
        public string User { get; set; } = string.Empty;
        public string CprNr { get; set; } = string.Empty;
    }
}
