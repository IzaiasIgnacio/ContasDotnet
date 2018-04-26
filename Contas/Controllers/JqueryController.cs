using Microsoft.AspNetCore.Mvc;
using Contas.Repository;

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
    }

}
