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
    }
}
