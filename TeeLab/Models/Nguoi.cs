using System.ComponentModel.DataAnnotations;

namespace TeeLab.Models
{
    public class Nguoi
    {
        [Key]
        public int NguoiId { get; set; }
        public string? Hoten { get; set; }

    }
}
