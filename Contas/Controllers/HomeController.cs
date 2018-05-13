using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Contas.Models.ViewModel;
using Contas.Services;
using System;
using Contas.Models.Entity;
using System.Collections.Generic;

namespace Contas.Controllers {
    public class HomeController : Controller {

        public IActionResult Index() {
            IndexViewModel index = new IndexViewModel();
            var mes_atual = ConsolidadoService.GetValue("mes_atual");
            string data = "01/"+mes_atual;
            DateTime Date = Convert.ToDateTime(data);
            index.MesAtual = Date;

            List<Movimentacao> mov;

            for (int i=0;i<=5;i++) {
                mov = ContasService.GetMovimentacoes(Date.AddMonths(i));
                index.Movimentacoes.Add(mov);
                SetLinhas(mov.Count);
            }
            index.linhas = linhas;
            // PlanilhaService.AtualizarPlanilha();
            return View(index);
        }

        private int linhas;
        private int SetLinhas(int linhas) {
            if (linhas > this.linhas) {
                this.linhas = linhas;
            }
            return this.linhas;
        }

        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
