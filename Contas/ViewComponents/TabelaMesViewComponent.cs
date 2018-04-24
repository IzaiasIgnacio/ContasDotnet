using Contas.Models.Entity;
using Contas.Models.ViewModel;
using Contas.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contas.ViewComponents {
    public class TabelaMesViewComponent : ViewComponent {

        public IViewComponentResult Invoke(List<Movimentacao> movimentacoes, DateTime mes, int indice, int linhas) {
            MovimentacaoRepository mr = new MovimentacaoRepository();
            var busca = mr.Listar<Movimentacao>().Where(m => m.Tipo == "save" && m.Data.Value.Month == mes.Month).FirstOrDefault();
            decimal save = 0;
            if (busca != null) {
                save = busca.Valor;
            }
            var model = new TabelaMesViewModel(movimentacoes, mes, indice, linhas, save);
            return View("TabelaMes", model);
        }

    }
}
