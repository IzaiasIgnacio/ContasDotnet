using Contas.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contas.Models.ViewModel {
    public class ConsolidadosViewModel {
        public string Casa { get; set; }
        public string Itau { get; set; }
        public string Inter { get; set; }
        public string Savings { get; set; }
        private List<Cartao> cartoes;
        public List<Cartao> Cartoes {
            get {
                if (cartoes == null) {
                    cartoes = new List<Cartao>();
                }
                return cartoes;
            }
            set {
                cartoes = value;
            }
        }
    }
}
