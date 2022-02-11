using GamesShop.Models.Store;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Data
{
    public interface IRepositoryGame : IRepository<Game>
    {
        void Create(Game item, string[] selectedItems);
        void Update(Game itemToUpdate, string[] selectedItems);
        public IQueryable<Game> SearchItems(string gameName, int? idDeveloper, int? idGenre);
    }
}
