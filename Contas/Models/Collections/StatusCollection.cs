using System;
using System.Collections.Generic;

namespace Contas.Models.Collections {
    public static class StatusCollection {
        public class Status {
            public string nome;
        }

        private static List<Status> status;
        public static List<Status> ListaStatus {
            get {
                status = new List<Status>();
                status.Add(new Status { nome = "normal"});
                status.Add(new Status { nome = "definido"});
                return status;
            }
        }
    }
}
