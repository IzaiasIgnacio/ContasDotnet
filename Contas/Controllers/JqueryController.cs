﻿using Microsoft.AspNetCore.Mvc;
using Contas.Repository;
using System.Collections.Generic;
using Contas.Models.Entity;
using System.Linq;
using Contas.Services;
using System;
using System.Globalization;
using Contas.Models.ViewModel;

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
        public JsonResult AtualizarValorSaveJquery(int id, string dif) {
            Double diferenca = Double.Parse(dif, CultureInfo.InvariantCulture);
            MovimentacaoRepository movimentacaoRepository = new MovimentacaoRepository();
            var mes_modificado = movimentacaoRepository.Listar<Movimentacao>().Where(m => m.Id == id).FirstOrDefault().Data;
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
                var movimentacao_atualizar = movimentacaoRepository.Listar<Movimentacao>().Where(m => m.Data.Month == mes && m.Data.Year == ano && m.Tipo == "save").FirstOrDefault();
                var novo_valor = ((Double)movimentacao_atualizar.Valor - diferenca).ToString("F");
                diferenca = 0;
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

        [HttpPost]
        public string AtualizarStatusMovimentacao(int id, string status) {
            MovimentacaoRepository movimentacaoRepository = new MovimentacaoRepository();
            return movimentacaoRepository.AtualizarStatus(id, status);
        }

        [HttpPost]
        public void AtualizarConsolidados(FormConsolidadosViewModel dados) {
            ConsolidadoRepository consolidadoRepository = new ConsolidadoRepository();
            consolidadoRepository.AtualizarConsolidados(dados);
        }

        [HttpPost]
        public void AtualizarMovimentacao(FormMovimentacaoViewModel dados) {
            MovimentacaoRepository movimentacaoRepository = new MovimentacaoRepository();
            movimentacaoRepository.AtualizarMovimentacao(dados);
        }

        [HttpPost]
        public void ExluirMovimentacao(int id) {
            MovimentacaoRepository movimentacaoRepository = new MovimentacaoRepository();
            movimentacaoRepository.ExluirMovimentacao(id);
        }

        [HttpPost]
        public void AtualizarPosicaoMovimentacoes(string[] movimentacoes) {
            MovimentacaoRepository movimentacaoRepository = new MovimentacaoRepository();
            movimentacaoRepository.AtualizarPosicaoMovimentacoes(movimentacoes);
        }

        [HttpPost]
        public ActionResult ExibirFormMovimentacao(int id) {
            MovimentacaoRepository movimentacaoRepository = new MovimentacaoRepository();
            var movimentacao = movimentacaoRepository.GetById(id);
            var model = new FormMovimentacaoViewModel();
            model.Id = id;
            model.Nome = movimentacao.Nome;
            model.Data = movimentacao.Data.ToShortDateString();
            model.Tipo = movimentacao.Tipo;
            model.Valor = movimentacao.Valor.ToString();
            model.Status = movimentacao.Status;
            model.Cartao = movimentacao.IdCartao;
            
            return ViewComponent("FormMovimentacao", model);
        }

        [HttpPost]
        public void ExportarContasJquery() {
            PlanilhaService.AtualizarPlanilha();
        }
    }

}
