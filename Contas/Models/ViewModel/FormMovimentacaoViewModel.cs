using System;
using System.Collections.Generic;
using Contas.Models.Collections;
using Contas.Models.Entity;
using Contas.Services;
using static Contas.Models.Collections.StatusCollection;
using static Contas.Models.Collections.TiposCollection;

namespace Contas.Models.ViewModel {
    public class FormMovimentacaoViewModel {
        public int Id  { get; set; }
        public string Nome { get; set; }
        public string Data { get; set; }
        public string Tipo { get; set; }
        public string Valor { get; set; }
        public string Status { get; set; }
        private string parcelas;
        public string Parcelas {
            get {
                if (parcelas == null) {
                    return "1";
                }
                return parcelas;
            }
            set {
                parcelas = value;
            }
        }
        public int Cartao { get; set; }
        public List<Tipo> ListaTipos {
            get {
                return TiposCollection.Tipos;
            }
        }
        public List<Status> ListaStatus {
            get {
                return StatusCollection.ListaStatus;
            }
        }
        public List<Cartao> ListaCartoes {
            get {
                return ConsolidadoService.GetCartoes();
            }
        }
    }
}
