using Contas.Models.Entity;
using Contas.Models.ViewModel;
using Contas.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Contas.Services;

namespace Contas.ViewComponents {
    public class TabelaSavingsViewComponent : ViewComponent {

        public IViewComponentResult Invoke(DateTime mes, int indice) {
            var model = new TabelaSavingsViewModel(mes, indice, ContasService.GetSaveMes(mes));
            return View("TabelaSavings", model);
        }

    }
}
