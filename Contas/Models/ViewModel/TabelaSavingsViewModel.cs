using Contas.Models.Entity;
using Contas.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Contas.Models.ViewModel {
    public class TabelaSavingsViewModel {
        public string save;
        private DateTime data { get; set; }
        private double somar;

        public TabelaSavingsViewModel(DateTime data, int indice, decimal save) {
            this.save = save.ToString("F");
            this.data = data;
            if (indice == 0) {
                somar = Double.Parse(ConsolidadoService.GetValue("savings"), CultureInfo.InvariantCulture);
            }
            if (indice > 0) {
                somar = ContasService.Sobra;
            }
        }

        public string ValorMes {
            get {
                return somar.ToString("F");
            }
        }

        public string Sobra {
            get {
                ContasService.Sobra = somar + Double.Parse(save);
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
