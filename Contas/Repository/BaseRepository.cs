using Contas.Models.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Games.Models.Repository {
    public class BaseRepository {
        protected ContasContext db;

        public BaseRepository() {
            db = new ContasContext();
        }

        public List<T> Listar<T>() where T : class {
            return db.Set<T>().ToList();
        }

    }
}