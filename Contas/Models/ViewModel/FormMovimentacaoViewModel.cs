using System;

namespace Contas.Models.ViewModel {
    public class FormMovimentacaoViewModel {
        public int Id  { get; set; }
        public string Nome { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
        public string Valor { get; set; }
        public string Status { get; set; }
    }
}
