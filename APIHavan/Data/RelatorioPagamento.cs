using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIHavan.Data
{
    public class RelatorioPagamento
    {
        [Key]
        public int id { get; set; }
        public HistoricoPreco HistorioPreco{ get; set; }
        public CondicaoPagamento CondicaoPagamento { get; set; }
        public Cliente Cliente { get; set; }
    }
}
