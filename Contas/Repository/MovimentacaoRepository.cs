using System;
using System.Linq;
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

        public void AdicionarMovimentacao(string nome, DateTime data, string tipo, string valor, string status, int? id_cartao = 0, int Parcelas = 1) {
            string parc = null;
            DateTime data_movimentacao = data;

            if (id_cartao == 0) {
                id_cartao = null;
            }

            for (int i=1;i<=Parcelas;i++) {
                if (Parcelas > 1) {
                    parc = " "+i.ToString()+"/"+Parcelas.ToString();
                    data_movimentacao = data.AddMonths(i-1);
                }
                
                db.Movimentacao.Add(new Movimentacao {
                    Nome = nome+parc,
                    Data = data_movimentacao,
                    Tipo = tipo,
                    Valor = decimal.Parse(valor),
                    Status = status,
                    IdCartao = id_cartao
                });
            }
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
                DateTime data = DateTime.Parse(dados.Data);
                AdicionarMovimentacao(dados.Nome, data, dados.Tipo, dados.Valor, dados.Status, dados.Cartao, Int32.Parse(dados.Parcelas));
            }
        }

        public void ExluirMovimentacao(int id) {
            db.Remove(db.Movimentacao.Find(id));
            db.SaveChanges();
        }

        public void AtualizarPosicaoMovimentacoes(string[] movimentacoes) {
            for (int i=1;i<=movimentacoes.Length;i++) {
                Movimentacao m = db.Movimentacao.Find(Int32.Parse(movimentacoes.ElementAtOrDefault(i-1)));
                m.Posicao = i;
            }
            db.SaveChanges();
        }
    }
}
