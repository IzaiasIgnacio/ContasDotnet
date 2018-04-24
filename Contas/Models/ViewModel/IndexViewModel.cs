using Contas.Models.Entity;
using System;
using System.Collections.Generic;

namespace Contas.Models.ViewModel {
    public class IndexViewModel {
        public DateTime MesAtual { get; set; }
        public int linhas { get; set; }
        private List<List<Movimentacao>> movimentacoes;

        public List<List<Movimentacao>> Movimentacoes {
            get {
                if (movimentacoes == null) {
                    movimentacoes = new List<List<Movimentacao>>();
                }
                return movimentacoes;
            }
            set {
                movimentacoes = value;
            }
        }
    }
}
