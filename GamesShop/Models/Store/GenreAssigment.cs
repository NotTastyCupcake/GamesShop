using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Models.Store
{
    public class GenreAssigment
    {
        public int GameId { get; set; }

        public int GenreId { get; set; }

        public Game Game { get; set; }

        public Genre Genre { get; set; }
    }
}
