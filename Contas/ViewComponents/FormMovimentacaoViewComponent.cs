using System;
using Contas.Models.ViewModel;
using Contas.Services;
using Microsoft.AspNetCore.Mvc;

namespace Contas.ViewComponents {
    public class FormMovimentacaoViewComponent : ViewComponent {
        public IViewComponentResult Invoke() {
            var model = new FormMovimentacaoViewModel();
            model.Data = DateTime.Now.Date.ToShortDateString();

            return View("FormMovimentacao", model);
        }
    }
}
