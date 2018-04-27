﻿using Contas.Models.Entity;
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
        public string Folga;
        private DateTime data { get; set; }
        public List<Movimentacao> Movimentacoes { get; set; }

        public TabelaMesViewModel(List<Movimentacao> movimentacoes, DateTime data, int indice, int linhas, decimal save) {
            Movimentacoes = movimentacoes;
            this.data = data;
            Linhas = linhas;
            Indice = indice;
            Save = save.ToString("F");
            switch (indice) {
                case 0:
                    salario = 0;
                    somar = ConsolidadoService.GetValoresConsolidados();
                break;
                case 1:
                    salario = Double.Parse(ConsolidadoService.GetValue("salario"), CultureInfo.InvariantCulture);
                    somar = ContasService.Sobra;
                    var t = Double.Parse(ConsolidadoService.GetValue("mensal"), CultureInfo.InvariantCulture) / 4;
                    var a = Double.Parse(Sobra.Replace(",", "."), CultureInfo.InvariantCulture);
                    Folga = (a - t).ToString("F");
                break;
                default:
                    salario = Double.Parse(ConsolidadoService.GetValue("salario"), CultureInfo.InvariantCulture);
                    somar = ContasService.Sobra;
                    var ta = Double.Parse(ConsolidadoService.GetValue("mensal"), CultureInfo.InvariantCulture)*indice;
                    var aa = Double.Parse(Sobra.Replace(",", "."), CultureInfo.InvariantCulture);
                    Folga = (aa - ta).ToString("F");
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
                return Movimentacoes.Sum(m => m.Valor).ToString("F");
            }
        }

        public string Sobra {
            get {
                ContasService.Sobra = ((salario + somar) - (Double)(Movimentacoes.Sum(m => m.Valor)) - Double.Parse(Save));
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
