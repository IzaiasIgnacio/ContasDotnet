using Microsoft.AspNetCore.Mvc;
using Contas.Repository;
using System.Collections.Generic;
using Contas.Models.Entity;
using System.Linq;
using Contas.Services;
using System;

namespace Contas.Controllers {
    public class JqueryController : Controller {

        [HttpPost]
        public string AtualizarValorMovimentacaoJquery(int id, string novo_valor) {
            if (id == 0) {
                return "0";
            }
            MovimentacaoRepository movimentacaoRepository = new MovimentacaoRepository();
            return movimentacaoRepository.AtualizarValor(id, novo_valor);
        }

        [HttpPost]
        public JsonResult AtualizarValorSaveJquery(int id, double diferenca) {
            MovimentacaoRepository movimentacaoRepository = new MovimentacaoRepository();
            var mes_modificado = movimentacaoRepository.Listar<Movimentacao>().Where(m => m.Id == id).FirstOrDefault().Data.Value;
            var mes_atual = ConsolidadoService.GetValue("mes_atual");
            string data = "01/" + mes_atual;
            DateTime data_atual = Convert.ToDateTime(data);

            int inicio = ((mes_modificado.Year - data_atual.Year) * 12) + mes_modificado.Month - data_atual.Month;

            if (inicio == 0) {
                inicio = 1;
            }

            Dictionary<int, string> novos_valores = new Dictionary<int, string>();
            for (int i=inicio;i<=5;i++) {
                var dt = data_atual.AddMonths(i);
                var mes = dt.Month;
                var ano = dt.Year;
                var movimentacao_atualizar = movimentacaoRepository.Listar<Movimentacao>().Where(m => m.Data.Value.Month == mes && m.Data.Value.Year == ano && m.Tipo == "save").FirstOrDefault();
                var novo_valor = ((Double)movimentacao_atualizar.Valor - diferenca).ToString("F");
                movimentacaoRepository.AtualizarValor(movimentacao_atualizar.Id, novo_valor);
                novos_valores.Add(i, novo_valor);
            }

            return Json(novos_valores);
        }

        [HttpPost]
        public IActionResult AtualizarTabelaSavings(int indice) {
            var mes_atual = ConsolidadoService.GetValue("mes_atual");
            string data = "01/" + mes_atual;
            DateTime data_modificada = Convert.ToDateTime(data);
            return ViewComponent("TabelaSavings", new { mes = data_modificada.AddMonths(indice), indice });
        }
    }

}
