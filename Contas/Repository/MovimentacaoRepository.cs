using Contas.Models.Entity;
using Games.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace Contas.Repository {
    public class MovimentacaoRepository : BaseRepository {

        public string AtualizarValor(int id, string novo_valor) {
            Movimentacao movimentacao = db.Movimentacao.Find(id);
            string valorOriginal = movimentacao.Valor.ToString("F");
            movimentacao.Valor = decimal.Parse(novo_valor);
            db.Entry(movimentacao).State = EntityState.Modified;
            db.SaveChanges();
            return valorOriginal;
        }

    }
}
