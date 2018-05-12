using Contas.Models.Entity;
using Contas.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contas.Services {
    public static class ContasService {
        public static double Sobra { get; set;}

        public static List<Movimentacao> GetMovimentacoes(DateTime data) {
            MovimentacaoRepository movimentacaoRepository = new MovimentacaoRepository();
            SetValoresFixosMes(data, movimentacaoRepository);
            return movimentacaoRepository.Listar<Movimentacao>()
                .Where(m => m.Data.Value.Month == data.Month && m.Data.Value.Year == data.Year && m.Tipo != "save")
                .OrderBy(m =>m.Posicao)
                .ToList();
        }

        private static void SetValoresFixosMes(DateTime data, MovimentacaoRepository movimentacaoRepository) {
            Dictionary<string, Double> valores_fixos = new Dictionary<string, Double>() {
                {"virtua", 220},
                {"netflix", 37.9},
                {"m", 800},
                {"passion", 258},
                {"cel", 42.99}
            };

            foreach (var valor in valores_fixos) {
                if (!movimentacaoRepository.Listar<Movimentacao>().Where(m => m.Nome == valor.Key && m.Data.Value.Month == data.Month && m.Data.Value.Year == data.Year).Any()) {
                    movimentacaoRepository.AdicionarMovimentacao(valor.Key, new DateTime(data.Year, data.Month, 1) , "gasto", valor.Value.ToString(), "definido");
                }
            }
        }

        public static decimal GetSaveMes(DateTime mes) {
            MovimentacaoRepository movimentacaoRepository = new MovimentacaoRepository();
            var busca = movimentacaoRepository.Listar<Movimentacao>().Where(m => m.Tipo == "save" && m.Data.Value.Month == mes.Month && m.Data.Value.Year == mes.Year).FirstOrDefault();
            decimal save = 0;
            if (busca != null) {
                save = busca.Valor;
            }
            return save;
        }

        public static void SetSaveMes(DateTime mes, string valor) {
            MovimentacaoRepository movimentacaoRepository = new MovimentacaoRepository();
            var busca = movimentacaoRepository.Listar<Movimentacao>().Where(m => m.Tipo == "save" && m.Data.Value.Month == mes.Month && m.Data.Value.Year == mes.Year).FirstOrDefault();
            if (busca != null) {
                movimentacaoRepository.AtualizarValor(busca.Id, valor);
                return;
            }
            movimentacaoRepository.AdicionarMovimentacao("Save", mes, "save", valor, "definido");
        }

        public static int GetSaveId(DateTime data) {
            MovimentacaoRepository movimentacaoRepository = new MovimentacaoRepository();
            return movimentacaoRepository.Listar<Movimentacao>().Where(m => m.Tipo == "save" && m.Data.Value.Month == data.Month && m.Data.Value.Year == data.Year).FirstOrDefault().Id;
        }

        public static string GetSiglaCartao(int? id) {
            if (id == null) {
                return null;
            }
            CartaoRepository cartaoRepository = new CartaoRepository();
            return " ["+cartaoRepository.Listar<Cartao>().Where(c => c.Id == id).FirstOrDefault().Sigla+"]";
        }
    }
}
