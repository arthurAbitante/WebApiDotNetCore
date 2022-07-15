using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIHavan.Data
{
    public class Cliente
    {
        [Key]
        public int clienteId { get; set; }
        [Required]
        public string cnpj { get; set; }
        [Required]
        public string razaoSocial { get; set; }
        public List<RelatorioPagamento>? RelatorioPagamento { get; set; }
    }
}
