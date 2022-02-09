using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Models.Store
{
    public class Genre
    {
        [Key]
        public int IdGenre { get; set; }
        public string Name { get; set; }
        public virtual ICollection<GenreAssigment> GenreAssigment { get; set; }

    }
}
