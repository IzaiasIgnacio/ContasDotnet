using Contas.Models.ViewModel;
using Contas.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Contas.Models.Entity;
using static Contas.Models.ViewModel.ConsolidadosViewModel;

namespace Contas.ViewComponents {
    public class ConsolidadosViewComponent : ViewComponent {
        public IViewComponentResult Invoke() {
            var model = new ConsolidadosViewModel();
            model.Casa = Double.Parse(ConsolidadoService.GetValue("casa"), CultureInfo.InvariantCulture).ToString("F");
            model.Itau = Double.Parse(ConsolidadoService.GetValue("itau"), CultureInfo.InvariantCulture).ToString("F");
            model.Inter = Double.Parse(ConsolidadoService.GetValue("inter"), CultureInfo.InvariantCulture).ToString("F");
            model.Savings = Double.Parse(ConsolidadoService.GetValue("savings"), CultureInfo.InvariantCulture).ToString("F");
            List<Cartao> cartoes = ConsolidadoService.GetCartoes();
            foreach (var cartao in cartoes) {
                model.Cartoes.Add(new CartaoConsolidado {Nome = cartao.Nome, CreditoAtual = cartao.Credito - ConsolidadoService.GetGastosCartao(cartao.Id), CreditoTotal = cartao.Credito});
            }
            return View("Consolidados", model);
        }
    }
}
