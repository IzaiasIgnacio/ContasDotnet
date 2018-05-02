using Contas.Models.Entity;
using Contas.Models.ViewModel;
using Games.Models.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Contas.Repository {
    public class ConsolidadoRepository : BaseRepository {
        public void AtualizarConsolidados(FormConsolidadosViewModel dados) {
            Atualizar("casa", dados.Casa);
            Atualizar("inter", dados.Inter);
            Atualizar("itau", dados.Itau);
            Atualizar("savings", dados.Savings);
            Atualizar("salario", dados.Salario);
            Atualizar("mensal", dados.Mensal);
            Atualizar("mes_atual", dados.MesAtual);
            db.SaveChanges();
        }

        private void Atualizar(string nome, string valor) {
            Consolidado consolidado = db.Consolidado.Where(c => c.Nome == nome).FirstOrDefault();
            consolidado.Valor = valor;
            db.Entry(consolidado).State = EntityState.Modified;
        }
    }
}
