using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIHavan.Data
{
    public class HistoricoPreco
    {
        [Key]
        public int id { get; set; }
        public Produto Produto { get; set; }
        public double preco { get; set; }
    }
}
