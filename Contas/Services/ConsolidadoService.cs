using Contas.Models.Entity;
using Contas.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Contas.Services {
    public static class ConsolidadoService {
        public static string GetValue(string nome) {
            ConsolidadoRepository cr = new ConsolidadoRepository();
            return cr.Listar<Consolidado>().Where(c => c.Nome == nome).FirstOrDefault().Valor;
        }

        public static double GetValoresConsolidados() {
            ConsolidadoRepository cr = new ConsolidadoRepository();
            var itau = Double.Parse(cr.Listar<Consolidado>().Where(c => c.Nome == "itau").FirstOrDefault().Valor, CultureInfo.InvariantCulture);
            var inter = Double.Parse(cr.Listar<Consolidado>().Where(c => c.Nome == "inter").FirstOrDefault().Valor, CultureInfo.InvariantCulture);
            var casa = Double.Parse(cr.Listar<Consolidado>().Where(c => c.Nome == "casa").FirstOrDefault().Valor, CultureInfo.InvariantCulture);
            return itau + inter + casa;
        }

        public static List<Cartao> GetCartoes() {
            CartaoRepository cr = new CartaoRepository();
            return cr.Listar<Cartao>().ToList();
        }

        public static decimal GetGastosCartao(int id) {
            MovimentacaoRepository mr = new MovimentacaoRepository();
            return mr.Listar<Movimentacao>().Where(m => m.IdCartao == id).Sum(m => m.Valor);
        }

        public static Double GetValorMensal(DateTime data) {
            Double consolidado_mensal = Double.Parse(GetValue("mensal"), CultureInfo.InvariantCulture);
            int semanas_mes = NumeroSemanasMes(data.AddMonths(-1));
            int semana_atual = SemanaAtual();
            int mult = 0;
            if (semanas_mes == 4) {
                mult = ValoresCalculo4Semanas[semana_atual];
            }
            if (semanas_mes == 5) {
                mult = ValoresCalculo5Semanas[semana_atual];
            }
            return 340;
            //return (consolidado_mensal / semanas_mes) * mult;
        }

        public static int SemanaAtual() {
            DateTime data_atual = DateTime.Now;
            DateTime inicio_mes = new DateTime(data_atual.Year, data_atual.Month, 1);
            int sextas = 0;
            int dia_atual = data_atual.Day;
            for (int i = 0; i < dia_atual; i++) {
                if (inicio_mes.AddDays(i).DayOfWeek == DayOfWeek.Friday) {
                    sextas++;
                }
            }
            return sextas;
        }

        private static int NumeroSemanasMes(DateTime mes) {
            int sextas = 0;
            int DiasNoMes = DateTime.DaysInMonth(mes.Year, mes.Month);
            for (int i = 0; i < DiasNoMes; i++) {
                if (mes.AddDays(i).DayOfWeek == DayOfWeek.Friday) {
                    sextas++;
                }
            }
            var proximoMes = mes.AddMonths(1);
            int proximo = (int)proximoMes.DayOfWeek;
            if (proximo >= 4 || proximo == 0) {
                sextas++;
            }
            return sextas;
        }

        private static Dictionary<int, int> ValoresCalculo4Semanas = new Dictionary<int, int>() {
            { 0, 4 },
            { 1, 4 },
            { 2, 3 },
            { 3, 2 },
            { 4, 1 }
        };

        private static Dictionary<int, int> ValoresCalculo5Semanas = new Dictionary<int, int>() {
            { 0, 5 },
            { 1, 5 },
            { 2, 4 },
            { 3, 3 },
            { 4, 2 },
            { 5, 1 }
        };
    }
}
