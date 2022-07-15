
using System.ComponentModel.DataAnnotations;

namespace APIHavan.Data
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Sku { get; set; }
        [Required]
        public string Descricao { get; set; }
    }
}
