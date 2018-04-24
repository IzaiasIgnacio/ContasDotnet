using System;
using System.Collections.Generic;

namespace Contas.Models.Entity
{
    public partial class GastoCartao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime? Data { get; set; }
        public int IdCartao { get; set; }

        public Cartao IdNavigation { get; set; }
    }
}
