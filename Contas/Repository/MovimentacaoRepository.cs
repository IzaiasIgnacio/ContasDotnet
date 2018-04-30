using System;
using Contas.Models.Entity;
using Games.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace Contas.Repository {
    public class MovimentacaoRepository : BaseRepository {

        public string AtualizarValor(int id, string novo_valor) {
            Movimentacao movimentacao = db.Movimentacao.Find(id);
            string valorOriginal = movimentacao.Valor.ToString("F");
            if (novo_valor == valorOriginal) {
                return "0";
            }
            movimentacao.Valor = decimal.Parse(novo_valor);
            db.Entry(movimentacao).State = EntityState.Modified;
            db.SaveChanges();
            return valorOriginal;
        }

        internal void AdicionarMovimentacao(string nome, DateTime data, string tipo, string valor, string status) {
            db.Movimentacao.Add(new Movimentacao {
                Nome = nome,
                Data = data,
                Tipo = tipo,
                Valor = decimal.Parse(valor),
                Status = status
            });
            db.SaveChanges();
        }
    }
}
