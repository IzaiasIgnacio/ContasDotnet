using Contas.Models.ViewModel;
using Contas.Services;
using Microsoft.AspNetCore.Mvc;

namespace Contas.ViewComponents {
    public class FormConsolidadosViewComponent : ViewComponent {
        public IViewComponentResult Invoke() {
            var model = new FormConsolidadosViewModel();
            model.Casa = ConsolidadoService.GetValue("casa");
            model.Itau = ConsolidadoService.GetValue("itau");
            model.Inter = ConsolidadoService.GetValue("inter");
            model.Savings = ConsolidadoService.GetValue("savings");
            model.Salario = ConsolidadoService.GetValue("salario");
            model.Mensal = ConsolidadoService.GetValue("mensal");
            model.MesAtual = ConsolidadoService.GetValue("mes_atual");

            return View("FormConsolidados", model);
        }
    }
}
