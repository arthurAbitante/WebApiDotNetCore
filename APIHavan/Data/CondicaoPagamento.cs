using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIHavan.Data
{
    public class CondicaoPagamento
    {
        [Key]
        public int condicaoPagamentoId { get; set; }
        public string descricao { get; set; }
        [Required]
        public string dias { get; set; }
    }
}
