using System.Collections.Generic;

namespace Contas.Models.ViewModel {
    public class ConsolidadosViewModel {
        public string Casa { get; set; }
        public string Itau { get; set; }
        public string Inter { get; set; }
        public string Savings { get; set; }
        private List<CartaoConsolidado> cartoes;
        public List<CartaoConsolidado> Cartoes {
            get {
                if (cartoes == null) {
                    cartoes = new List<CartaoConsolidado>();
                }
                return cartoes;
            }
            set {
                cartoes = value;
            }
        }
        public class CartaoConsolidado {
            public string Nome;
            public decimal CreditoAtual;
            public decimal CreditoTotal;
        }
    }
}
