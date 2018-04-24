using Contas.Models.Entity;
using Contas.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Contas.Models.ViewModel {
    public class TabelaMesViewModel {
        private double salario;
        private double somar;
        public int linhas;
        public string save;
        private DateTime data { get; set; }
        public List<Movimentacao> movimentacoes { get; set; }

        public TabelaMesViewModel(List<Movimentacao> movimentacoes, DateTime data, int indice, int linhas, decimal save) {
            this.movimentacoes = movimentacoes;
            this.data = data;
            this.linhas = linhas;
            this.save = save.ToString("F");
            if (indice == 0) {
                salario = 0;
                somar = ConsolidadoService.GetValoresConsolidados();
            }
            if (indice > 0) {
                salario = Double.Parse(ConsolidadoService.GetValue("salario"), CultureInfo.InvariantCulture);
                somar = ContasService.Sobra;
            }
        }

        public string ValorMes {
            get {
                return (salario + somar).ToString("F");
            }
        }

        public string ValorTotal {
            get {
                return movimentacoes.Sum(m => m.Valor).ToString("F");
            }
        }

        public string Sobra {
            get {
                ContasService.Sobra = ((salario + somar) - (Double)(movimentacoes.Sum(m => m.Valor)) - Double.Parse(save));
                return ContasService.Sobra.ToString("F");
            }
        }

        public string Mes {
            get {
                var dt = data.ToString("MMMM", CultureInfo.CreateSpecificCulture("pt-BR"));
                return dt.First().ToString().ToUpper()+ dt.Substring(1);
            }
        }

        
    }
}
