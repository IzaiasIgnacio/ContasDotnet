using System;
using Contas.Models.Entity;
using Contas.Models.ViewModel;
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

        public void AdicionarMovimentacao(string nome, DateTime data, string tipo, string valor, string status) {
            db.Movimentacao.Add(new Movimentacao {
                Nome = nome,
                Data = data,
                Tipo = tipo,
                Valor = decimal.Parse(valor),
                Status = status
            });
            db.SaveChanges();
        }

        public string AtualizarStatus(int id, string status) {
            Movimentacao movimentacao = db.Movimentacao.Find(id);
            string status_anterior = movimentacao.Status;
            movimentacao.Status = status;
            db.Entry(movimentacao).State = EntityState.Modified;
            db.SaveChanges();
            return status_anterior;
        }

        public void AtualizarMovimentacao(FormMovimentacaoViewModel dados) {
            if (dados.Id == 0) {
                AdicionarMovimentacao(dados.Nome, dados.Data, dados.Tipo, dados.Valor, dados.Status);
            }
        }
    }
}
