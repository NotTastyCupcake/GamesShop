using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamesShop.Models.Store
{
    public class Genre
    {
        [Key]
        public int IdGenre { get; set; }
        [Required]
        [Display(Name = "Название жанра")]
        public string Name { get; set; }
        public virtual ICollection<GenreAssigment> GenreAssigment { get; set; }

    }
}
