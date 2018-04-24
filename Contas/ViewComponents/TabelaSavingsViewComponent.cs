using Contas.Models.Entity;
using Contas.Models.ViewModel;
using Contas.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contas.ViewComponents {
    public class TabelaSavingsViewComponent : ViewComponent {

        public IViewComponentResult Invoke(DateTime mes, int indice) {
            MovimentacaoRepository mr = new MovimentacaoRepository();
            var busca = mr.Listar<Movimentacao>().Where(m => m.Tipo == "save" && m.Data.Value.Month == mes.Month).FirstOrDefault();
            decimal save = 0;
            if (busca != null) {
                save = busca.Valor;
            }
            var model = new TabelaSavingsViewModel(mes, indice, save);
            return View("TabelaSavings", model);
        }

    }
}
