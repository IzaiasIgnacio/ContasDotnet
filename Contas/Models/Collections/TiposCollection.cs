using System;
using System.Collections.Generic;

namespace Contas.Models.Collections {
    public static class TiposCollection {
        public class Tipo {
            public string nome;
        }

        private static List<Tipo> tipos;
        public static List<Tipo> Tipos {
            get {
                tipos = new List<Tipo>();
                tipos.Add(new Tipo { nome = "gasto"});
                tipos.Add(new Tipo { nome = "renda"});
                tipos.Add(new Tipo { nome = "terceiros" });
                return tipos;
            }
        }
    }
}
