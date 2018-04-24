using Contas.Models.Entity;
using Contas.Models.ViewModel;
using Contas.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Contas.Services;

namespace Contas.ViewComponents {
    public class TabelaMesViewComponent : ViewComponent {

        public IViewComponentResult Invoke(List<Movimentacao> movimentacoes, DateTime mes, int indice, int linhas) {
            var model = new TabelaMesViewModel(movimentacoes, mes, indice, linhas, ContasService.GetSaveMes(mes));
            return View("TabelaMes", model);
        }

    }
}
