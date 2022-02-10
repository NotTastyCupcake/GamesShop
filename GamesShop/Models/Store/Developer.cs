using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamesShop.Models.Store
{
    public class Developer
    {
        [Key]
        public int IdDeveloper { get; set; }
        [Required]
        [Display(Name = "Команда разработчиков")]
        public string Name { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}
