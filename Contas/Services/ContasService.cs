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
            return movimentacaoRepository.Listar<Movimentacao>()
                .Where(m => m.Data.Value.Month == data.Month && m.Data.Value.Year == data.Year && m.Tipo == "gasto")
                .ToList();
        }
    }
}
