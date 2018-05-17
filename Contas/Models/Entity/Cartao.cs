using System;
using System.Collections.Generic;

namespace Contas.Models.Entity
{
    public partial class Cartao
    {
        public Cartao()
        {
            Movimentacao = new HashSet<Movimentacao>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Numero { get; set; }
        public string Sigla { get; set; }
        public decimal Credito { get; set; }

        public GastoCartao GastoCartao { get; set; }
        public ICollection<Movimentacao> Movimentacao { get; set; }
    }
}
