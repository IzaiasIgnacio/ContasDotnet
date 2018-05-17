using System;
using System.Collections.Generic;

namespace Contas.Models.Entity
{
    public partial class Movimentacao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
        public string Loja { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; }
        public int? IdCartao { get; set; }
        public int? Posicao { get; set; }

        public Cartao IdCartaoNavigation { get; set; }
    }
}
