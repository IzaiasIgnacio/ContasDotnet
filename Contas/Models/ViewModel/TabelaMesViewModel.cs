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
        public int Linhas;
        public int Indice;
        public string Save;
        private DateTime data { get; set; }
        public List<Movimentacao> Movimentacoes { get; set; }

        public TabelaMesViewModel(List<Movimentacao> movimentacoes, DateTime data, int indice, int linhas) {
            Movimentacoes = movimentacoes;
            this.data = data;
            Linhas = linhas;
            Indice = indice;
            Save = "0";
            Double valor_mensal;
            Double sobra_atual;
            switch (indice) {
                case 0:
                    salario = 0;
                    somar = ConsolidadoService.GetValoresConsolidados();
                    Save = ContasService.GetSaveMes(data).ToString("F");
                break;
                case 1:
                    salario = Double.Parse(ConsolidadoService.GetValue("salario"), CultureInfo.InvariantCulture);
                    somar = ContasService.Sobra;
                    valor_mensal = ConsolidadoService.GetValorMensal(data);
                    sobra_atual = Double.Parse(Sobra.Replace(",", "."), CultureInfo.InvariantCulture);
                    Save = (sobra_atual - valor_mensal).ToString("F");
                    ContasService.SetSaveMes(data, Save);
                break;
                default:
                    salario = Double.Parse(ConsolidadoService.GetValue("salario"), CultureInfo.InvariantCulture);
                    somar = ContasService.Sobra;
                    valor_mensal = Double.Parse(ConsolidadoService.GetValue("mensal"), CultureInfo.InvariantCulture)*indice;
                    sobra_atual = Double.Parse(Sobra.Replace(",", "."), CultureInfo.InvariantCulture);
                    Save = (sobra_atual - valor_mensal).ToString("F");
                    ContasService.SetSaveMes(data, Save);
                break;
            }
        }

        public string ValorMes {
            get {
                return (salario + somar).ToString("F");
            }
        }

        public string ValorTotal {
            get {
                var gastos = Movimentacoes.Where(m => m.Tipo == "gasto" && m.Status != "pago").Sum(m => m.Valor);
                var rendas = Movimentacoes.Where(m => m.Tipo == "renda").Sum(m => m.Valor);
                return (gastos - rendas).ToString("F");
            }
        }

        public string Sobra {
            get {
                ContasService.Sobra = ((salario + somar) - (Double)(Movimentacoes.Where(m => m.Tipo == "gasto" && m.Status != "pago").Sum(m => m.Valor)) - Double.Parse(Save));
                return ContasService.Sobra.ToString("F");
            }
        }

        public string Mes {
            get {
                var dt = data.ToString("MMMM", CultureInfo.CreateSpecificCulture("pt-BR"));
                return dt.First().ToString().ToUpper()+ dt.Substring(1);
            }
        }

        public int SaveId {
            get {
                return ContasService.GetSaveId(data);
            }
        }

        public string GetImagemCartao(int? id_cartao) {
            switch (id_cartao) {
                case 1:
                    return "<span class='dot nubank_dot'></span>";
                case 2:
                    return "<span class='dot digio_dot'></span>";
                case 3:
                    return "<span class='dot inter_dot'></span>";
            }
            return null;
        }

    }
}
